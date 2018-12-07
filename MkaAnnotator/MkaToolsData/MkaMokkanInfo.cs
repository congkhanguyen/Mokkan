using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using MokkAnnotator.MkaDocToolkit.Xml;
using System.Xml;
using Microsoft.Win32;
using MokkAnnotator.MkaCommon;
using MokkAnnotator.MkaDrawTools;
using System.Drawing;
using System.IO;

namespace MokkAnnotator.MkaToolsData
{
    /// <summary>
    /// Definition of individual Mokkan Information
    /// </summary>
    [Xmlable("info")]
    public class MkaMokkanInfo : ICloneable, IXmlable
    {
        // logger
        private static log4net.ILog _log = MkaDefine.Logger;
        
        /// <summary>
        /// Last registered remainder id
        /// </summary>
        public static int LastRBangou = 1;

        /// <summary>
        /// Graphical Properties
        /// </summary>
        [Browsable(false)]
        public GraphicsProperties Properties { get; set; }

        /// <summary>
        /// Remainder ID
        /// </summary>
        [Category(MkaDefine.MokkanLblJyouhou)]
        [DisplayName(MkaDefine.MokkanLblRBangou)]   
        public int RBangou { get; set; }

        /// <summary>
        /// Mokkan Notation
        /// </summary>
        [Category(MkaDefine.MokkanLblJyouhou)]
        [DisplayName(MkaDefine.MokkanLblKariShakubun)]
        public String KariShakubun { get; set; } 

        /// <summary>
        /// Outline Information
        /// </summary>
        [Category(MkaDefine.MokkanLblJyouhou)]
        [DisplayName(MkaDefine.MokkanLblGaihouShoshuuJyouhou)]
        public String GaihouShoshuuJyouhou { get; set; }

        /// <summary>
        /// Image Number Information
        /// </summary>        
        [Category(MkaDefine.MokkanLblJyouhou)]
        [DisplayName(MkaDefine.MokkanLblShasinBangouJyouhou)]
        public String ShasinBangouJyouhou { get; set; }

        /// <summary>
        /// Comment
        /// </summary>        
        [Category(MkaDefine.MokkanLblJyouhou)]
        [DisplayName(MkaDefine.MokkanLblBikou)]
        public String Bikou { get; set; }

        /// <summary>
        /// Show Position
        /// </summary>                
        [Category(MkaDefine.FormatLblInformation)]
        [DisplayName(MkaDefine.FormatLblRShowPosition)]
        public ShowPosition RShowPosition
        {
            get { return Properties.RShowPosition; }
            set { Properties.RShowPosition = value; }
        }

        /// <summary>
        /// Border line's color
        /// </summary>                
        [Category(MkaDefine.FormatLblInformation)]
        [DisplayName(MkaDefine.FormatLblBorderColor)]
        public Color BorderColor
        {
            get { return Properties.BorderColor; }
            set { Properties.BorderColor = value; }
        }

        /// <summary>
        /// Border line's pen width
        /// </summary>
        [Category(MkaDefine.FormatLblInformation)]
        [DisplayName(MkaDefine.FormatLblPenWidth)]
        public float PenWidth
        {
            get { return Properties.PenWidth; }
            set { Properties.PenWidth = value; }
        }

        /// <summary>
        /// Fill area's color (RGB color)
        /// </summary>        
        [Category(MkaDefine.FormatLblInformation)]
        [DisplayName(MkaDefine.FormatLblFillColor)]
        public Color FillColor
        {
            get { return Properties.FillColor; }
            set { Properties.FillColor = value; }
        }

        /// <summary>
        /// Fill area's transparent
        /// </summary>
        [Category(MkaDefine.FormatLblInformation)]
        [DisplayName(MkaDefine.FormatLblFillColorAlpha)]
        public int FillColorAlpha
        {
            get { return Properties.FillColorAlpha; }
            set { Properties.FillColorAlpha = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>        
        public MkaMokkanInfo()
        {
            this.RBangou = LastRBangou;            
            this.KariShakubun = String.Empty;
            this.GaihouShoshuuJyouhou = String.Empty;
            this.ShasinBangouJyouhou = String.Empty;
            this.Bikou = String.Empty;

            Properties = new GraphicsProperties();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="properties">graphical properties</param>
        public MkaMokkanInfo(GraphicsProperties properties)
        {
            this.RBangou = LastRBangou;
            this.KariShakubun = String.Empty;
            this.GaihouShoshuuJyouhou = String.Empty;
            this.ShasinBangouJyouhou = String.Empty;
            this.Bikou = String.Empty;

            Properties = properties;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        /// <returns></returns>
        public MkaMokkanInfo Clone()
        {
            MkaMokkanInfo ret = (MkaMokkanInfo)this.MemberwiseClone();
            ret.Properties = this.Properties.Clone();
            return ret;
        }
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Load last remainder id from registry
        /// </summary>
        public static void LoadFromRegistry()
        {
            try
            {
                RegistryKey key = MkaDefine.RootKey.OpenSubKey(MkaDefine.LastUsedInfo);

                LastRBangou = (int)key.GetValue(MkaDefine.MokkanRBangou);
            }
            catch (Exception ex)
            {
                LastRBangou = 1;
                _log.Warn(MkaMessage.WarnLoadRegistry + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Save last remainder id to registry
        /// </summary>
        public static void SaveToRegistry()
        {
            try
            {
                RegistryKey key = MkaDefine.RootKey.CreateSubKey(MkaDefine.LastUsedInfo);

                key.SetValue(MkaDefine.MokkanRBangou, LastRBangou);
            }
            catch (Exception ex)
            {
                _log.Warn(MkaMessage.WarnSaveRegistry + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Export mokkan information to csv file
        /// </summary>        
        public void ExportToCsv(StreamWriter sw)
        {
            sw.Write(String.Format("{0},", RBangou));
            sw.Write(String.Format("{0},", KariShakubun));
            sw.Write(String.Format("{0},", GaihouShoshuuJyouhou));
            sw.Write(String.Format("{0},", ShasinBangouJyouhou));
            sw.Write(String.Format("{0}", Bikou));
            sw.WriteLine();
        }

        /// <summary>
        /// Insert mokkan information to database query string
        /// </summary>        
        public String GetInsertQueryString(String glassId)
        {
            String queryStr = "";
            queryStr += "INSERT INTO mka_mokkandata VALUES(";
            queryStr += String.Format("'{0}',", RBangou);
            queryStr += String.Format("'{0}',", glassId);
            queryStr += String.Format("'{0}',", MkaDBConnect.EscapeSQL(KariShakubun));
            queryStr += String.Format("'{0}',", MkaDBConnect.EscapeSQL(GaihouShoshuuJyouhou));
            queryStr += String.Format("'{0}',", MkaDBConnect.EscapeSQL(ShasinBangouJyouhou));
            queryStr += String.Format("'{0}'", MkaDBConnect.EscapeSQL(Bikou));
            queryStr += ")";

            return queryStr;
        }

        #region IXmlable Members

        /// <summary>
        /// Load object from xml file
        /// </summary>      
        void IXmlable.FromXml(XmlElement xmlEle, IXmlContext cnt)
        {
            // remainder number
            try { RBangou = Int32.Parse(xmlEle.GetAttribute(MkaDefine.MokkanTagRBangou)); }
            catch { RBangou = 0; }

            // mokkan notation
            try { KariShakubun = xmlEle.GetAttribute(MkaDefine.MokkanTagKariShakubun); }
            catch { KariShakubun = String.Empty; }

            // outline information
            try { GaihouShoshuuJyouhou = xmlEle.GetAttribute(MkaDefine.MokkanTagGaihouShoshuuJyouhou); }
            catch { GaihouShoshuuJyouhou = String.Empty; }

            // image number information
            try { ShasinBangouJyouhou = xmlEle.GetAttribute(MkaDefine.MokkanTagShasinBangouJyouhou); }
            catch { ShasinBangouJyouhou = String.Empty; }

            // comment
            try { Bikou = xmlEle.GetAttribute(MkaDefine.MokkanTagBikou); }
            catch { Bikou = String.Empty; }
        }
                
        /// <summary>
        /// Save object to xml file
        /// </summary> 
        void IXmlable.ToXml(XmlElement xmlEle, IXmlContext cnt)
        {
            // remainder number
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.MokkanTagRBangou, this.RBangou.ToString());

            // mokkan notation
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.MokkanTagKariShakubun, this.KariShakubun);

            // outline information
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.MokkanTagGaihouShoshuuJyouhou, this.GaihouShoshuuJyouhou);

            // image number information
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.MokkanTagShasinBangouJyouhou, this.ShasinBangouJyouhou);

            // comment
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.MokkanTagBikou, this.Bikou);
        }

        #endregion        
    }
}
