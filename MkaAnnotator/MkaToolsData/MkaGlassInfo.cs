using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using MokkAnnotator.MkaDocToolkit.Xml;
using System.Xml;
using Microsoft.Win32;
using MokkAnnotator.MkaCommon;
using System.IO;
using MokkAnnotator.MkaWindow;

namespace MokkAnnotator.MkaToolsData
{
    /// <summary>
    /// Definition of Glass Information
    /// </summary> 
    [Xmlable("glass")]
    public class MkaGlassInfo:ICloneable, IXmlable
    {
        // logger
        private static log4net.ILog _log = MkaDefine.Logger;

        /// <summary>
        /// Bat Information
        /// </summary>
        [Browsable(false)]
        public MkaBatInfo BatInfo { get; set; }

        /// <summary>
        /// Mokkan Count
        /// </summary>
        [Category(MkaDefine.GlassLblJyouhou)]
        [DisplayName(MkaDefine.GlassLblMokkanKazu)]
        [DefaultValue(10)]
        public int MokkanKazu { get; set; }

        /// <summary>
        /// Glass Id
        /// </summary>
        [Category(MkaDefine.GlassLblJyouhou)]
        [DisplayName(MkaDefine.GlassLblItaBangou)]
        public String GlassItaBangou { get; set; }

        /// <summary>
        /// Glass name
        /// </summary>
        [Browsable(false)]
        public String GlassName
        {
            get { return Path.GetFileNameWithoutExtension(GlassFilePath); }
        }

        /// <summary>
        /// Dig Order
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblChousaJisuu)]
        public int ChousaJisuu
        {
            get { return BatInfo.ChousaJisuu; }
            set { BatInfo.ChousaJisuu = value; }
        }

        /// <summary>
        /// Big Area Id
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblOoChiku)]
        public String OoChiku
        {
            get { return BatInfo.OoChiku; }
            set { BatInfo.OoChiku = value; }
        }

        /// <summary>
        /// Small Area Id
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblChuushouChiku)]
        public String ChuushouChiku
        {
            get { return BatInfo.ChuushouChiku; }
            set { BatInfo.ChuushouChiku = value; }
        }

        /// <summary>
        /// Remain Id
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblIkoumei)]
        public String Ikoumei
        {
            get { return BatInfo.Ikoumei; }
            set { BatInfo.Ikoumei = value; }
        }

        /// <summary>
        /// Layer Name
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblDosoumei)]
        public String Dosoumei
        {
            get { return BatInfo.Dosoumei; }
            set { BatInfo.Dosoumei = value; }
        }

        /// <summary>
        /// Grid Number
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblGrid)]
        public int Grid
        {
            get { return BatInfo.Grid; }
            set { BatInfo.Grid = value; }
        }

        /// <summary>
        /// Dig Date
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblDate)]
        public DateTime Date
        {
            get { return BatInfo.Date; }
            set { BatInfo.Date = value; }
        }

        /// <summary>
        /// Bat Number
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblBatBangou)]
        public String BatBangou
        {
            get { return BatInfo.BatBangou; }
            set { BatInfo.BatBangou = value; }
        }

        /// <summary>
        /// Glass filename (full path)
        /// </summary>
        [Browsable(false)]
        public String GlassFilePath { get; set; }           

        /// <summary>
        /// Glass filename
        /// </summary>
        [Browsable(false)]
        public String GlassFileName
        {
            get { return Path.GetFileName(GlassFilePath); }           
        }

        /// <summary>
        /// Glass image's filename (full path)
        /// </summary>
        [Category(MkaDefine.GlassLblJyouhou)]
        [DisplayName(MkaDefine.GlassLblImagePath)]
        [ReadOnly(true)]
        public String ImageFilePath { get; set; }

        /// <summary>
        /// Glass image's filename
        /// </summary>    
        [Browsable(false)]
        public String ImageFileName { get; set; }    

        /// <summary>
        /// Width of Glass image
        /// </summary>
        [Category(MkaDefine.GlassLblJyouhou)]
        [DisplayName(MkaDefine.GlassLblWidth)]
        [ReadOnly(true)]
        public int Width { get; set; }

        /// <summary>
        /// Height of Glass image
        /// </summary>
        [Category(MkaDefine.GlassLblJyouhou)]
        [DisplayName(MkaDefine.GlassLblHeight)]
        [ReadOnly(true)]
        public int Height { get; set; }

        /// <summary>
        /// Start remain id of glass
        /// </summary>
        [Category(MkaDefine.GlassLblJyouhou)]
        [DisplayName(MkaDefine.GlassLblKaishiRBangou)]
        public int KaishiRBangou { get; set; }

        /// <summary>
        /// Font size to display r bangou
        /// </summary>
        [Browsable(false)]
        [DefaultValue(24.0f)]
        public float RFontSize { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MkaGlassInfo()
        {
            BatInfo = new MkaBatInfo();

            this.GlassItaBangou = String.Empty;
            this.KaishiRBangou = MkaMokkanInfo.LastRBangou;
            this.GlassFilePath = String.Empty;
            this.ImageFilePath = String.Empty;
            this.Width = 0;
            this.Height = 0;
            this.RFontSize = MkaDefine.RDefaultFontSize;
        }

        /// <summary>
        /// Overload operator ==
        /// </summary>
        public static bool operator ==(MkaGlassInfo g1, MkaGlassInfo g2)
        {
            return g1.Equals(g2);
        }

        /// <summary>
        /// Overload operator !=
        /// </summary>
        public static bool operator !=(MkaGlassInfo g1, MkaGlassInfo g2)
        {
            return !(g1.Equals(g2));
        }

        public override bool Equals(object obj)
        {
            MkaGlassInfo g = (MkaGlassInfo)obj;
            if ((this.ChousaJisuu != g.ChousaJisuu) ||
                (this.OoChiku != g.OoChiku) ||
                (this.ChuushouChiku != g.ChuushouChiku) ||
                (this.Ikoumei != g.Ikoumei) ||
                (this.Dosoumei != g.Dosoumei) ||
                (this.Grid != g.Grid) ||
                (this.Date != g.Date) ||
                (this.BatBangou != g.BatBangou) ||
                (this.GlassItaBangou != g.GlassItaBangou) ||
                (this.KaishiRBangou != g.KaishiRBangou) ||
                (this.RFontSize != g.RFontSize))
                return false;
            else
                return true;
        }

        public override int GetHashCode()
        {
            return GlassItaBangou.GetHashCode() ^ KaishiRBangou;
        } 

        /// <summary>
        /// Clone this instance
        /// </summary>        
        public MkaGlassInfo Clone()
        {
            MkaGlassInfo ret = (MkaGlassInfo)this.MemberwiseClone();
            ret.BatInfo = this.BatInfo.Clone();
            return ret;
        }
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Change glass filename and glass image filename
        /// </summary>
        public void ChangeGlassName(String newName)
        {
            File.Copy(GlassFilePath, newName, true);
            File.Delete(GlassFilePath);
            GlassFilePath = newName;        
            String oldName = ImageFilePath;
            ImageFilePath = Path.GetDirectoryName(ImageFilePath) + "\\" + Path.GetFileNameWithoutExtension(newName) + Path.GetExtension(ImageFilePath);
            ImageFileName = Path.GetFileName(ImageFilePath);
            File.Copy(oldName, ImageFilePath, true);
            File.Delete(oldName);
        }
        
        /// <summary>
        /// Export glass information to csv file
        /// </summary>        
        public void ExportToCsv(StreamWriter sw)
        {
            sw.WriteLine(String.Format("{0},{1}", MkaDefine.BatLblChousaJisuu, BatInfo.ChousaJisuu));
            sw.WriteLine(String.Format("{0},{1}", MkaDefine.BatLblOoChiku, BatInfo.OoChiku));
            sw.WriteLine(String.Format("{0},{1}", MkaDefine.BatLblChuushouChiku, BatInfo.ChuushouChiku));
            sw.WriteLine(String.Format("{0},{1}", MkaDefine.BatLblIkoumei, BatInfo.Ikoumei));
            sw.WriteLine(String.Format("{0},{1}", MkaDefine.BatLblDosoumei, BatInfo.Dosoumei));
            sw.WriteLine(String.Format("{0},{1}", MkaDefine.BatLblGrid, BatInfo.Grid));
            sw.WriteLine(String.Format("{0},{1}", MkaDefine.BatLblDate, BatInfo.Date));
            sw.WriteLine(String.Format("{0},{1}", MkaDefine.BatLblBatBangou, BatInfo.BatBangou));
            sw.WriteLine(String.Format("{0},{1}", MkaDefine.GlassLblItaBangou, GlassItaBangou));
        }

        #region IXmlable Members

        /// <summary>
        /// Load object from xml file
        /// </summary>      
        void IXmlable.FromXml(XmlElement xmlEle, IXmlContext cnt)
        {
            // glass id
            try { GlassItaBangou = xmlEle.GetAttribute(MkaDefine.GlassTagItaBangou); }
            catch { GlassItaBangou = String.Empty; }

            // glass start remain id
            try { KaishiRBangou = Int32.Parse(xmlEle.GetAttribute(MkaDefine.GlassTagKaishiRBangou)); }
            catch { KaishiRBangou = 1; }

            // image filename
            try { ImageFileName = xmlEle.GetAttribute(MkaDefine.GlassTagImageName); }
            catch { ImageFileName = String.Empty; }

            // width of image
            try { Width = Int32.Parse(xmlEle.GetAttribute(MkaDefine.GlassTagWidth)); }
            catch { Width = 0; }

            // height of image
            try { Height = Int32.Parse(xmlEle.GetAttribute(MkaDefine.GlassTagHeight)); }
            catch { Height = 0; }

            // font size to display glass id
            try { RFontSize = float.Parse(xmlEle.GetAttribute(MkaDefine.GlassRFontSize)); }
            catch { RFontSize = MkaDefine.RDefaultFontSize; }
        }

        /// <summary>
        /// Save object to xml file
        /// </summary> 
        void IXmlable.ToXml(XmlElement xmlEle, IXmlContext cnt)
        {
            // glass id
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.GlassTagItaBangou, this.GlassItaBangou.ToString());

            // glass start remain id
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.GlassTagKaishiRBangou, this.KaishiRBangou.ToString());

            // image filename
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.GlassTagImageName, this.ImageFileName);

            // width of image
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.GlassTagWidth, this.Width.ToString());

            // height of image
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.GlassTagHeight, this.Height.ToString());

            // font size to display glass id
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.GlassRFontSize, this.RFontSize.ToString());
        }

        #endregion            
    }
}
