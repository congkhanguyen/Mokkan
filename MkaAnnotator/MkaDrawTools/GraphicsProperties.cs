using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MokkAnnotator;
using Microsoft.Win32;
using MokkAnnotator.MkaCommon;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using MokkAnnotator.MkaDocToolkit.Xml;
using System.Xml;

namespace MokkAnnotator.MkaDrawTools
{
    /// <summary>
    /// Graphical properties of selected area
    /// </summary>    
    [Xmlable("format", IsUnique = false)]    
    public class GraphicsProperties : ICloneable, IXmlable  
    {
        // logger
        private static log4net.ILog _log = MkaDefine.Logger;

        /// <summary>
        /// Remain id show position
        /// </summary>        
        public ShowPosition RShowPosition { get; set; }

        /// <summary>
        /// Remain id show location
        /// </summary>
        public PointF RShowLocation 
        { 
            get
            {
                return (RShowPosition == ShowPosition.Top) ? RShowTop : RShowBottom;
            }
        }
        public PointF RShowTop { get; set; }
        public PointF RShowBottom { get; set; }

        /// <summary>
        /// Remain id show font size
        /// </summary>
        public float RFontSize { get; set; }

        /// <summary>
        /// Border line's color
        /// </summary>                
        public Color BorderColor { get; set; }        

        /// <summary>
        /// Border line's pen width
        /// </summary>
        public float PenWidth { get; set; }

        /// <summary>
        /// Fill area's color (RGB color)
        /// </summary>        
        public Color FillColor { get; set; }

        /// <summary>
        /// Fill area's transparent
        /// </summary>
        public int FillColorAlpha { get; set; }

        // last used graphical properties
        public static GraphicsProperties LastProperties = new GraphicsProperties();

        /// <summary>
        /// Border line's color (Html color format)
        /// </summary>
        public String BorderColorHtml
        {
            get { return ColorTranslator.ToHtml(this.BorderColor); }
            set { this.BorderColor = ColorTranslator.FromHtml(value); }
        }

         /// <summary>
        /// Fill area's transparent color (Html color format)
        /// </summary>
        public String FillColorHtml
        {
            get { return ColorTranslator.ToHtml(this.FillColor); }
            set { this.FillColor = ColorTranslator.FromHtml(value); }
        }

        /// <summary>
        /// Get pen to draw border line
        /// </summary>        
        public Pen GetPen()
        {           
            return new Pen(this.BorderColor, this.PenWidth);
        }

        /// <summary>
        /// Get brush to fill area
        /// </summary>        
        public SolidBrush GetBrush()
        {
            return new SolidBrush(Color.FromArgb(this.FillColorAlpha,this.FillColor));
        }
                
        /// <summary>
        /// Constructor
        /// </summary>
        public GraphicsProperties()
        {
            this.BorderColor = MkaDefine.DefaultBorderColor;
            this.PenWidth = MkaDefine.DefaultPenWidth;
            this.FillColor = MkaDefine.DefaultFillColor;
            this.FillColorAlpha = MkaDefine.DefaultColorAlpha;
            this.RFontSize = MkaDefine.RDefaultFontSize;
            this.RShowPosition = ShowPosition.Top;
            this.RShowTop = new PointF(0, 0);
            this.RShowBottom = new PointF(0, 0);
        }

        /// <summary>
        /// Load properties from registry
        /// </summary>
        public static void LoadFromRegistry()
        {
            try
            {
                RegistryKey key = MkaDefine.RootKey.OpenSubKey(MkaDefine.GraphicsKey);

                LastProperties.BorderColorHtml = key.GetValue(MkaDefine.GraphicBorderColor).ToString();
                LastProperties.PenWidth = float.Parse(key.GetValue(MkaDefine.GraphicPenWidth).ToString());
                LastProperties.FillColorHtml = key.GetValue(MkaDefine.GraphicFillColor).ToString();
                LastProperties.FillColorAlpha = int.Parse(key.GetValue(MkaDefine.GraphicColorAlpha).ToString());
                LastProperties.RFontSize = float.Parse(key.GetValue(MkaDefine.RFontSize).ToString());
            }
            catch (Exception ex)
            {
                LastProperties = new GraphicsProperties();
                _log.Warn(MkaMessage.WarnLoadRegistry + " - " + ex.Message);
            }            
        }

        /// <summary>
        /// Save properties to registry
        /// </summary>
        public static void SaveToRegistry()
        {
            try
            {
                RegistryKey key = MkaDefine.RootKey.CreateSubKey(MkaDefine.GraphicsKey);

                key.SetValue(MkaDefine.GraphicBorderColor, LastProperties.BorderColorHtml);
                key.SetValue(MkaDefine.GraphicPenWidth, LastProperties.PenWidth);
                key.SetValue(MkaDefine.GraphicFillColor, LastProperties.FillColorHtml);
                key.SetValue(MkaDefine.GraphicColorAlpha, LastProperties.FillColorAlpha);
                key.SetValue(MkaDefine.RFontSize, LastProperties.RFontSize);
            }
            catch (Exception ex)
            {
                _log.Warn(MkaMessage.WarnSaveRegistry + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Clone this instance
        /// </summary>        
        public GraphicsProperties Clone()
        {
            GraphicsProperties ret = (GraphicsProperties)this.MemberwiseClone();
            return ret;
        }
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #region IXmlable Members

        /// <summary>
        /// Load object from xml file
        /// </summary> 
        void IXmlable.FromXml(XmlElement xmlEle, IXmlContext cnt)
        {
            // border color
            try { BorderColorHtml = xmlEle.GetAttribute(MkaDefine.FormatTagBorderColor); }
            catch { BorderColor = MkaDefine.DefaultBorderColor; }

            // pen width
            try { PenWidth = (float)Convert.ToDouble(xmlEle.GetAttribute(MkaDefine.FormatTagPenWidth)); }
            catch { PenWidth = MkaDefine.DefaultPenWidth; }

            // fill color
            try { FillColorHtml = xmlEle.GetAttribute(MkaDefine.FormatTagFillColor); }
            catch { FillColor = MkaDefine.DefaultFillColor; }

            // transparent
            try { FillColorAlpha = Int32.Parse(xmlEle.GetAttribute(MkaDefine.FormatTagFillColorAlpha)); }
            catch { FillColorAlpha = MkaDefine.DefaultColorAlpha; }

            // r show position
            try { RShowPosition = (xmlEle.GetAttribute(MkaDefine.FormatTagRShowPosition) == ShowPosition.Top.ToString()) ? ShowPosition.Top : ShowPosition.Bottom; }
            catch { RShowPosition = ShowPosition.Top; }

            // r show location
            try
            {
                String location = xmlEle.GetAttribute(MkaDefine.FormatTagRShowLocation);
                String[] coords = location.Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                PointF pnt = new PointF(float.Parse(coords[0]), float.Parse(coords[1]));
                if (RShowPosition == ShowPosition.Top)
                    RShowTop = pnt;
                else
                    RShowBottom = pnt;
            }
            catch { }
        }

        /// <summary>
        /// Save object to xml file
        /// </summary> 
        void IXmlable.ToXml(XmlElement xmlEle, IXmlContext cnt)
        {
            // border color
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.FormatTagBorderColor, BorderColorHtml);

            // pen width
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.FormatTagPenWidth, PenWidth.ToString());

            // fill color
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.FormatTagFillColor, FillColorHtml);

            // transparent
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.FormatTagFillColorAlpha, FillColorAlpha.ToString());

            // r show position
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.FormatTagRShowPosition, this.RShowPosition.ToString());

            // r show position
            String location = String.Format("({0},{1}))", RShowLocation.X, RShowLocation.Y);
            XmlToolkit.SetAttribute(xmlEle, MkaDefine.FormatTagRShowLocation, location);            
        }

        #endregion        
    }
}
