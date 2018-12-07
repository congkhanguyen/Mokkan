using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MokkAnnotator.MkaDocToolkit.Xml;
using System.Xml;
using MokkAnnotator.MkaCommon;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.Collections;
using MokkAnnotator.MkaDrawTools;
using System.IO;
using System.Windows.Forms;
using MokkAnnotator.MkaWindow;

namespace MokkAnnotator.MkaToolsData
{
    /// <summary>
    /// Definition of Bat Manager
    /// </summary>    
    [Xmlable("batinfo")]
    public class MkaBatManager : IXmlable
    {
        // logger
        private static log4net.ILog _log = MkaDefine.Logger;
        
        public MkaFrmAnnotator Owner;     // parent form

        // Bat filename
        public String BatFileName { get { return BatInfo.BatBangou + MkaDefine.BmkFileExt; } }

        // Bat directory
        public String BatDirectory { get { return BatInfo.BatDirectory; } }

        // Bat file path
        public String BatFilePath { get { return BatInfo.BatDirectory + "\\" + BatInfo.BatBangou + MkaDefine.BmkFileExt; } }

        // Bat information
        public MkaBatInfo BatInfo { get; set; }

        // Glass filenames
        public List<String> GlassFileNames { get; set; }

        // Glass image filenames
        public List<String> ImageFileNames { get; set; }

        // Glass file paths
        public List<String> GlassFilePaths { get; set; }

        // Glass image file paths
        public List<String> ImageFilePaths { get; set; }

        /// <summary>
        /// Get glass image full path from glass name
        /// </summary>        
        public String GetImagePath(String glassName)
        {            
            for(int i = 0; i < GlassFilePaths.Count; i++)
                if(GlassFilePaths[i] == glassName)
                    return ImageFilePaths[i];

            return "";
        }

        /// <summary>
        /// Show bat property dialog
        /// </summary>
        public void ShowPropertyDialog()
        {
            MkaPropertiesBat frm = new MkaPropertiesBat(this);
            frm.ShowDialog(Owner);
            Owner.FileChangeBat();                   
        }
                      
        /// <summary>
        /// Constructor
        /// </summary>
        public MkaBatManager()
        {            
            this.BatInfo = new MkaBatInfo();            
            this.GlassFilePaths = new List<String>();
            this.ImageFilePaths = new List<String>();
            this.GlassFileNames = new List<String>();
            this.ImageFileNames = new List<String>();
        } 
        
        public MkaBatManager(MkaFrmAnnotator owner)
        {
            this.Owner = owner;
            this.BatInfo = new MkaBatInfo();
            this.GlassFilePaths = new List<String>();
            this.ImageFilePaths = new List<String>();
            this.GlassFileNames = new List<String>();
            this.ImageFileNames = new List<String>();
        }

        public void Save()
        {
            try
            {
                // Serialize object to xml format
                XmlDocument doc = new XmlDocument();
                IXmlFormatter formatter = FepXmlableFactory.CreateXmlFormatter();
                IXmlContext cnt = FepXmlableFactory.CreateXmlContext(formatter, doc);

                doc = cnt.Document;
                XmlDeclaration xmldecl = doc.CreateXmlDeclaration("1.0", null, null);

                XmlElement _eleBat = cnt.ToXml(this);
                doc.AppendChild(_eleBat);

                doc.InsertBefore(xmldecl, doc.DocumentElement);
                doc.Save(BatFilePath);   
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(Owner, MkaMessage.ErrSaveFile);
                _log.Error(MkaMessage.ErrSaveFile, ex);
            }
        }

        #region IXmlable Members

        /// <summary>
        /// Load object from xml file
        /// </summary>      
        void IXmlable.FromXml(XmlElement xmlEle, IXmlContext cnt)
        {            
            // dig order
            try { BatInfo.ChousaJisuu = Int32.Parse(xmlEle.GetAttribute(MkaDefine.BatTagChousaJisuu)); }
            catch { BatInfo.ChousaJisuu = 0; }

            // big area id
            try { BatInfo.OoChiku = xmlEle.GetAttribute(MkaDefine.BatTagOoChiku); }
            catch { BatInfo.OoChiku = String.Empty; }

            // small area id
            try { BatInfo.ChuushouChiku = xmlEle.GetAttribute(MkaDefine.BatTagChuushouChiku); }
            catch { BatInfo.ChuushouChiku = String.Empty; }

            // remain id
            try { BatInfo.Ikoumei = xmlEle.GetAttribute(MkaDefine.BatTagIkoumei); }
            catch { BatInfo.Ikoumei = String.Empty; }

            // layer name
            try { BatInfo.Dosoumei = xmlEle.GetAttribute(MkaDefine.BatTagDosoumei); }
            catch { BatInfo.Dosoumei = String.Empty; }

            // grid number
            try { BatInfo.Grid = Int32.Parse(xmlEle.GetAttribute(MkaDefine.BatTagGrid)); }
            catch { BatInfo.Grid = 0; }

            // dig date
            try { BatInfo.Date = DateTime.Parse(xmlEle.GetAttribute(MkaDefine.BatTagDate)); }
            catch { BatInfo.Date = DateTime.Today; }

            // bat number
            try { BatInfo.BatBangou = xmlEle.GetAttribute(MkaDefine.BatTagBatBangou); }
            catch { BatInfo.BatBangou = String.Empty; }

            // glass filenames
            try {
                XmlDocument xmlDoc = cnt.Document;
                XmlNodeList glasses = xmlDoc.GetElementsByTagName("glass");
                String glass;
                for (int i = 0; i < glasses.Count; i++)
                {
                    glass = Path.GetFileName(glasses[i].InnerText);
                    ImageFileNames.Add(glass);
                    glass = Path.GetFileNameWithoutExtension(glass) + MkaDefine.MkaFileExt;
                    GlassFileNames.Add(glass);
                }
            }
            catch { }
        }

        /// <summary>
        /// Save object to xml file
        /// </summary> 
        void IXmlable.ToXml(XmlElement xmlEle, IXmlContext cnt)
        {
            // dig order
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagChousaJisuu, BatInfo.ChousaJisuu.ToString());

            // big area id
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagOoChiku, BatInfo.OoChiku.ToString());

            // small area id
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagChuushouChiku, BatInfo.ChuushouChiku);

            // remain id
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagIkoumei, BatInfo.Ikoumei);

            // layer name
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagDosoumei, BatInfo.Dosoumei);

            // grid number
            if(BatInfo.Grid == 0)
                XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagGrid, "");
            else
                XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagGrid, BatInfo.Grid.ToString());

            // dig date
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagDate, BatInfo.Date.ToString(MkaDefine.DateFormat));

            // bat number
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagBatBangou, BatInfo.BatBangou);

            // glass filenames
            XmlDocument xmlDoc = cnt.Document;
            XmlElement glasses, glass;
            glasses = xmlDoc.CreateElement("glasses");
            for (int i = 0; i < ImageFileNames.Count; i++)
            {
                glass = xmlDoc.CreateElement("glass");
                glass.InnerText = ImageFileNames[i];
                glasses.AppendChild(glass);
            }
            xmlEle.AppendChild(glasses);
         }

        #endregion
    }
}