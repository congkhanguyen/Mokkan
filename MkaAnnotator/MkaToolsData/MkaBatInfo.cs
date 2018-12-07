using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using MokkAnnotator.MkaDocToolkit.Xml;
using System.Xml;
using MokkAnnotator.MkaCommon;
using Microsoft.Win32;

namespace MokkAnnotator.MkaToolsData
{
    /// <summary>
    /// Definition of Bat Information
    /// </summary>    
    [Xmlable("bat")]
    public class MkaBatInfo : ICloneable, IXmlable
    {
        // logger
        private static log4net.ILog _log = MkaDefine.Logger;
        
        // last used bat information
        public static MkaBatInfo LastBat = new MkaBatInfo();

        /// <summary>
        /// Dig Order
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblChousaJisuu)]
        public int ChousaJisuu { get; set; }

        /// <summary>
        /// Big Area Id
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblOoChiku)]
        public String OoChiku { get; set; }

        /// <summary>
        /// Small Area Id
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblChuushouChiku)]
        public String ChuushouChiku { get; set; }

        /// <summary>
        /// Remain Id
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblIkoumei)]
        public String Ikoumei { get; set; }

        /// <summary>
        /// Layer Name
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblDosoumei)]
        public String Dosoumei { get; set; }

        /// <summary>
        /// Grid Number
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblGrid)]
        public int Grid { get; set; }

        /// <summary>
        /// Dig Date
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblDate)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Bat Number
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblBatBangou)]
        public String BatBangou { get; set; }
   
        /// <summary>
        /// Bat directory
        /// </summary>
        [Category(MkaDefine.BatLblJyouhou)]
        [DisplayName(MkaDefine.BatLblBatPath)]
        [ReadOnly(true)]
        public String BatDirectory { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MkaBatInfo()
        {              
            this.ChousaJisuu = 1;
            this.OoChiku = String.Empty;
            this.ChuushouChiku = String.Empty;
            this.Ikoumei = String.Empty;
            this.Dosoumei = String.Empty;
            this.Grid = 0;
            this.Date = MkaDefine.StartDate;
            this.BatBangou = String.Empty;
            this.BatDirectory = String.Empty;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>        
        public MkaBatInfo Clone()
        {
            MkaBatInfo ret = (MkaBatInfo)this.MemberwiseClone();
            return ret;
        }
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Load bat information from registry
        /// </summary>
        public static void LoadFromRegistry()
        {
            try
            {
                RegistryKey key = MkaDefine.RootKey.OpenSubKey(MkaDefine.LastUsedInfo);

                LastBat.ChousaJisuu = (int)key.GetValue(MkaDefine.BatChousaJisuu);
                LastBat.OoChiku = key.GetValue(MkaDefine.BatOoChiku).ToString();
                LastBat.ChuushouChiku = key.GetValue(MkaDefine.BatChuushouChiku).ToString();
                LastBat.Ikoumei = key.GetValue(MkaDefine.BatIkoumei).ToString();
                LastBat.Dosoumei = key.GetValue(MkaDefine.BatDosoumei).ToString();
                LastBat.Grid = (int)key.GetValue(MkaDefine.BatGrid);
                LastBat.Date = DateTime.Parse(key.GetValue(MkaDefine.BatDate).ToString());
                LastBat.BatBangou = key.GetValue(MkaDefine.BatBangou).ToString();
                LastBat.BatDirectory = key.GetValue(MkaDefine.BatPath).ToString();                
            }
            catch (Exception ex)
            {
                LastBat = new MkaBatInfo();

                _log.Warn(MkaMessage.WarnLoadRegistry + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Save bat information to registry
        /// </summary>
        public static void SaveToRegistry()
        {
            try
            {
                RegistryKey key = MkaDefine.RootKey.CreateSubKey(MkaDefine.LastUsedInfo);

                key.SetValue(MkaDefine.BatChousaJisuu, LastBat.ChousaJisuu);
                key.SetValue(MkaDefine.BatOoChiku, LastBat.OoChiku);
                key.SetValue(MkaDefine.BatChuushouChiku, LastBat.ChuushouChiku);
                key.SetValue(MkaDefine.BatIkoumei, LastBat.Ikoumei);
                key.SetValue(MkaDefine.BatDosoumei, LastBat.Dosoumei);
                key.SetValue(MkaDefine.BatGrid, LastBat.Grid);
                key.SetValue(MkaDefine.BatDate, LastBat.Date.ToString(MkaDefine.DateFormat));
                key.SetValue(MkaDefine.BatBangou, LastBat.BatBangou);
                key.SetValue(MkaDefine.BatPath, LastBat.BatDirectory);                
            }
            catch (Exception ex)
            {
                _log.Warn(MkaMessage.WarnSaveRegistry + " - " + ex.Message);
            }
        }

        #region IXmlable Members

        /// <summary>
        /// Load object from xml file
        /// </summary>      
        void IXmlable.FromXml(XmlElement xmlEle, IXmlContext cnt)
        {
            // dig order
            try { ChousaJisuu = Int32.Parse(xmlEle.GetAttribute(MkaDefine.BatTagChousaJisuu)); }
            catch { ChousaJisuu = 0; }

            // big area id
            try { OoChiku = xmlEle.GetAttribute(MkaDefine.BatTagOoChiku); }
            catch { OoChiku = String.Empty; }

            // small area id
            try { ChuushouChiku = xmlEle.GetAttribute(MkaDefine.BatTagChuushouChiku); }
            catch { ChuushouChiku = String.Empty; }

            // remain id
            try { Ikoumei = xmlEle.GetAttribute(MkaDefine.BatTagIkoumei); }
            catch { Ikoumei = String.Empty; }

            // layer name
            try { Dosoumei = xmlEle.GetAttribute(MkaDefine.BatTagDosoumei); }
            catch { Dosoumei = String.Empty; }

            // grid number
            try { Grid = Int32.Parse(xmlEle.GetAttribute(MkaDefine.BatTagGrid)); }
            catch { Grid = 0; }

            // dig date
            try { Date = DateTime.Parse(xmlEle.GetAttribute(MkaDefine.BatTagDate)); }
            catch { Date = DateTime.Today; }

            // bat number
            try { BatBangou = xmlEle.GetAttribute(MkaDefine.BatTagBatBangou); }
            catch { BatBangou = String.Empty; }
        }

        /// <summary>
        /// Save object to xml file
        /// </summary> 
        void IXmlable.ToXml(XmlElement xmlEle, IXmlContext cnt)
        {
            // dig order
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagChousaJisuu, this.ChousaJisuu.ToString());

            // big area id
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagOoChiku, this.OoChiku.ToString());

            // small area id
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagChuushouChiku, this.ChuushouChiku);

            // remain id
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagIkoumei, this.Ikoumei);

            // layer name
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagDosoumei, this.Dosoumei);

            // grid number
            if (this.Grid == 0)
                XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagGrid, "");
            else
                XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagGrid, this.Grid.ToString());

            // dig date
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagDate, this.Date.ToString(MkaDefine.DateFormat));

            // bat number
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.BatTagBatBangou, this.BatBangou);
        }

        #endregion
    }
}
