using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Diagnostics;

namespace MokkAnnotator.MkaDocToolkit.Xml
{
    internal class XmlContext : IXmlContext
    {
        private Dictionary<String, IXmlable> _xmlIdToObj = new Dictionary<String, IXmlable>();
        private Dictionary<IXmlable, String> _objToXmlId = new Dictionary<IXmlable, String>();
        private static Type[] _emptyArgument = new Type[0];

        private static readonly String _xmlIdAttr = "id";

        private IXmlFormatter _xmlFormater;
        private XmlDocument _xmlDocument;

        public XmlContext(IXmlFormatter xmlFormatter, XmlDocument document)
        {
            if (Object.ReferenceEquals(null, xmlFormatter))
                throw new ArgumentNullException("xmlFormatter");
            if (Object.ReferenceEquals(null, document))
                throw new ArgumentNullException("document");
            _xmlFormater = xmlFormatter;
            _xmlDocument = document;
        }

        public virtual IXmlable FromXml(XmlElement xmlEle)
        {
            if (Object.ReferenceEquals(null, xmlEle))
                return null;

            ConstructorInfo constructInfo = null;
            bool isUnique = false;
            if (!_xmlFormater.GetXmlTypeInfo(xmlEle.Name, out constructInfo, out isUnique))
            {
                System.Diagnostics.Debug.Assert(false, "could not find the Type of tag name " + xmlEle.Name);
                return null;
            }
            if (!Object.ReferenceEquals(null, constructInfo))
            {
                IXmlable objXmlable = null;
                if (isUnique)
                {
                    String strId = xmlEle.GetAttribute(_xmlIdAttr);
                    if (!String.IsNullOrEmpty(strId))
                    {
                        if (!_xmlIdToObj.TryGetValue(strId, out objXmlable))
                        {
                            objXmlable = constructInfo.Invoke(_emptyArgument) as IXmlable;
                            _objToXmlId.Add(objXmlable, strId);
                            _xmlIdToObj.Add(strId, objXmlable);
                        }
                        System.Diagnostics.Debug.Assert(null != objXmlable);

                        objXmlable.FromXml(xmlEle, this);
                        return objXmlable;
                    }
                }

                if (null == objXmlable)
                    objXmlable = constructInfo.Invoke(_emptyArgument) as IXmlable;
                objXmlable.FromXml(xmlEle, this);
                return objXmlable;
            }
            System.Diagnostics.Debug.Assert(false, "could not load " + xmlEle.Name);
            return null;
        }

        public virtual XmlElement ToXml(IXmlable xmlable)
        {
            if (Object.ReferenceEquals(null, xmlable))
                return null;

            String xmlName = null;
            bool isUnique = false;
            if (!_xmlFormater.GetTypeXmlInfo(xmlable.GetType(), out xmlName, out isUnique))
            {
                System.Diagnostics.Debug.Assert(false, "could not find the Xmlable information of " + xmlable.GetType().ToString());
                return null;
            }
            if (!String.IsNullOrEmpty(xmlName))
            {
                if (isUnique)
                {
                    String strObjId = null;
                    if (_objToXmlId.TryGetValue(xmlable, out strObjId) && !String.IsNullOrEmpty(strObjId))
                    {
                        XmlElement ele = this.Document.CreateElement(xmlName);
                        XmlToolkit.SetAttribute(ele, _xmlIdAttr, strObjId);                        
                        return ele;
                    }
                }

                XmlElement ele2 = this.Document.CreateElement(xmlName);
                if (isUnique)
                {
                    String xmlId = Guid.NewGuid().ToString();
                    _objToXmlId.Add(xmlable, xmlId);
                    _xmlIdToObj.Add(xmlId, xmlable);
                    XmlToolkit.SetAttribute(ele2, _xmlIdAttr, xmlId);                    
                }
                xmlable.ToXml(ele2, this);
                return ele2;
            }
            return null;
        }

        public virtual XmlDocument Document
        {
            get
            {
                return _xmlDocument;
            }
        }

        public virtual IXmlFormatter Formatter
        {
            get
            {
                return _xmlFormater;
            }
        }
    }

    internal class XmlFormatter : IXmlFormatter
    {
        private struct XmlTypeInfo
        {
            public ConstructorInfo _constructInfo;
            public bool _isUnique;
        };

        private struct TypeXmlInfo
        {
            public String _xmlName;
            public bool _isUnique;
        };

        private static Dictionary<String, XmlTypeInfo> s_xmlTypeInfo;
        private static Dictionary<Type, TypeXmlInfo> s_typeXmlInfo;
        private static Type[] _emptyArgument = new Type[0];

        private static void InitXmlTypeInfo()
        {
            if (Object.ReferenceEquals(null, s_xmlTypeInfo))
            {
                s_xmlTypeInfo = new Dictionary<String, XmlTypeInfo>();
                s_typeXmlInfo = new Dictionary<Type, TypeXmlInfo>();
                AppDomain app = AppDomain.CurrentDomain;
#if !PocketPC
                foreach (Assembly a in app.GetAssemblies())
#else
                Assembly a = Assembly.GetExecutingAssembly();
#endif
                {
                    foreach (Type type in a.GetTypes())
                    {
                        if (!type.IsClass)
                            continue;

                        Object[] xmlAttrs = type.GetCustomAttributes(typeof(XmlableAttribute), false);
                        if (xmlAttrs.Length > 0)
                        {
                            XmlableAttribute xmlable = xmlAttrs[0] as XmlableAttribute;
                            if (!Object.ReferenceEquals(null, xmlable))
                            {
                                CheckTypeXmlable(type);

                                ConstructorInfo defaultConstructor = type.GetConstructor(_emptyArgument);
                                if (Object.ReferenceEquals(null, defaultConstructor))
                                    throw new TypeLoadException("Xmlable class should have a public default constructor! Modify your class: " + type.ToString());
                                String strName = xmlable.XmlName.ToUpper();
                                XmlTypeInfo typeInfo;
                                if (s_xmlTypeInfo.TryGetValue(strName, out typeInfo) &&
                                    !Object.ReferenceEquals(null, typeInfo._constructInfo) &&
                                    defaultConstructor != typeInfo._constructInfo &&
                                    defaultConstructor.ReflectedType != typeInfo._constructInfo.ReflectedType)
                                {
                                    throw new TypeLoadException("Xmlable class: " + defaultConstructor.ReflectedType.ToString() +
                                        " and " + typeInfo._constructInfo.ReflectedType.ToString() + " have a same XML name (case insensitive) " + strName);
                                }
                                typeInfo._constructInfo = defaultConstructor;
                                typeInfo._isUnique = xmlable.IsUnique;
                                s_xmlTypeInfo.Add(xmlable.XmlName.ToUpper(), typeInfo);

                                TypeXmlInfo xmlInfo;
                                xmlInfo._xmlName = xmlable.XmlName;
                                xmlInfo._isUnique = xmlable.IsUnique;
                                s_typeXmlInfo.Add(type, xmlInfo);
                            }
                        }
                    }
                }
            }
        }

        [Conditional("DEBUG")]
        private static void CheckTypeXmlable(Type type)
        {
            if (Object.ReferenceEquals(null, type))
                return;
            bool result = false;
            Type[] itfs = type.GetInterfaces();
            foreach (Type itf in itfs)
            {
                if (itf == typeof(IXmlable))
                {
                    result = true;
                    break;
                }
            }
            if (!result)
                throw new Exception("Xmlable class " + type.ToString() + " doesn't implement interface IXmlable");
        }

        public virtual bool GetXmlTypeInfo(String xmlName, out ConstructorInfo constructInfo, out bool isUnique)
        {
            constructInfo = null;
            isUnique = false;
            InitXmlTypeInfo();
            XmlTypeInfo typeInfo = new XmlTypeInfo();
            if (!String.IsNullOrEmpty(xmlName))
            {
                if (s_xmlTypeInfo.TryGetValue(xmlName.ToUpper(), out typeInfo))
                {
                    constructInfo = typeInfo._constructInfo;
                    isUnique = typeInfo._isUnique;
                    return true;
                }
            }
            return false;
        }

        public virtual bool GetTypeXmlInfo(Type type, out String xmlName, out bool isUnique)
        {
            xmlName = null;
            isUnique = false;
            InitXmlTypeInfo();
            TypeXmlInfo xmlInfo = new TypeXmlInfo();
            if (!Object.ReferenceEquals(null, type))
            {
                if (s_typeXmlInfo.TryGetValue(type, out xmlInfo))
                {
                    xmlName = xmlInfo._xmlName;
                    isUnique = xmlInfo._isUnique;
                    return true;
                }
            }
            return false;
        }

        public virtual Object Deserialize(XmlDocument doc)
        {
            if (Object.ReferenceEquals(null, doc))
                throw new ArgumentNullException();

            XmlContext cnt = new XmlContext(this, doc);
            return cnt.FromXml(doc.DocumentElement as XmlElement);
        }

        public virtual void Serialize(XmlDocument doc, Object graph)
        {
            if (Object.ReferenceEquals(null, doc))
                throw new ArgumentNullException("doc");
            if (Object.ReferenceEquals(null, graph))
                throw new ArgumentNullException("graph");
            IXmlable objXmlable = graph as IXmlable;
            if (Object.ReferenceEquals(null, objXmlable))
                throw new ArgumentException("can't convert to IXmlable", "graph");

            doc.RemoveAll();

            XmlContext cnt = new XmlContext(this, doc);
            XmlElement ele = cnt.ToXml(objXmlable);
            doc.AppendChild(ele);
        }
    }
}
