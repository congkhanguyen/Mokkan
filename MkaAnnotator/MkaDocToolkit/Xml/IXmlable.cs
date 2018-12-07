using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Diagnostics;

namespace MokkAnnotator.MkaDocToolkit.Xml
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class XmlableAttribute : Attribute
    {
        private String _name = String.Empty;
        private bool _isUnique;

        public XmlableAttribute(String name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException();
            _name = name;
        }

        public String XmlName
        {
            get
            {
                return _name;
            }
        }

        public bool IsUnique
        {
            get
            {
                return _isUnique;
            }
            set
            {
                _isUnique = value;
            }
        }
    }

    public interface IXmlable
    {
        void FromXml(XmlElement xmlEle, IXmlContext cnt);

        void ToXml(XmlElement xmlEle, IXmlContext cnt);
    }

    public interface IXmlFormatter
    {
        Object Deserialize(XmlDocument doc);

        void Serialize(XmlDocument doc, Object graph);

        bool GetXmlTypeInfo(String xmlName, out ConstructorInfo constructInfo, out bool isUnique);

        bool GetTypeXmlInfo(Type type, out String xmlName, out bool isUnique);
    }

    public interface IXmlContext
    {
        IXmlable FromXml(XmlElement xmlEle);

        XmlElement ToXml(IXmlable xmlable);

        IXmlFormatter Formatter
        {
            get;
        }

        XmlDocument Document
        {
            get;
        }
    }

    public static class FepXmlableFactory
    {
        public static IXmlFormatter CreateXmlFormatter()
        {
            return new XmlFormatter();
        }

        public static IXmlContext CreateXmlContext(IXmlFormatter formatter, XmlDocument doc)
        {
            return new XmlContext(formatter, doc);
        }
    };

    public static class XmlToolkit
    {
        [Conditional("DEBUG")]
        public static void CheckAttribute(XmlElement xmlEle, String name)
        {
            if (!Object.ReferenceEquals(null, xmlEle))
            {
                if (xmlEle.HasAttribute(name))
                    throw new Exception("attribute name may have been used by base class!");
            }
        }

        public static void SetAttribute(XmlElement xmlEle, String name, String value)
        {
            if (Object.ReferenceEquals(null, xmlEle))
                throw new ArgumentNullException("xmlEle");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name");
            CheckAttribute(xmlEle, name);
            xmlEle.SetAttribute(name, value);
        }
    }
}
