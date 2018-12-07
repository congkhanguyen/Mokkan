using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;
using MokkAnnotator.MkaToolsData;
using MokkAnnotator.MkaDrawTools;
using MokkAnnotator.MkaCommon;

namespace MokkAnnotator.MkaWindow
{
    /// <summary>
    /// 印刷機能
    /// </summary>
    public partial class MkaPrint : Component
    {
        private PrintAction action; // preview or print action

        /// <summary>
        /// Print document
        /// </summary>
        private MkaDocument _documentArea;
        public MkaDocument DocumentArea
        {
            get { return this._documentArea; }
            set 
            { 
                this._documentArea = value;
                document.DocumentName = value.GlassInfo.GlassName;
            }
        }
        
        /// <summary>
        /// Print background image flag
        /// </summary>
        [DefaultValue(false)]
        public bool PrintGlassImage { get; set; }
        
        /// <summary>
        /// Print mokkan selection items
        /// </summary>
        [DefaultValue(false)]
        public bool PrintMokkanInfo { get; set; }
       
        /// <summary>
        /// Print annotators
        /// </summary>
        [DefaultValue(true)]
        public bool PrintGlassInfo { get; set; }

        private bool _secondPage = false;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public MkaPrint()
        {
            InitializeComponent();

            previewDialog.StartPosition = FormStartPosition.Manual;            
            printDialog.Document.DefaultPageSettings.PrinterSettings.PrintRange = PrintRange.CurrentPage; 
            pageSetupDialog.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);            
            pageSetupDialog.PageSettings.Landscape = true;

            PrintGlassImage = false;
            PrintMokkanInfo = false;
            PrintGlassInfo = true;            
        }
        
        /// <summary>
        /// Show page setup dialog
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public DialogResult ShowPageSetupDialog(IWin32Window owner)
        {
            // *********  Fix bug ***********
            // http://support.microsoft.com/?id=814355            
            // ***************************************
            if (System.Globalization.RegionInfo.CurrentRegion.IsMetric)
            {
                pageSetupDialog.PageSettings.Margins.Top =
                    (int)Math.Round(
                    pageSetupDialog.PageSettings.Margins.Top * 2.54);
                pageSetupDialog.PageSettings.Margins.Bottom =
                    (int)Math.Round(
                    pageSetupDialog.PageSettings.Margins.Bottom * 2.54);
                pageSetupDialog.PageSettings.Margins.Left =
                    (int)Math.Round(
                    pageSetupDialog.PageSettings.Margins.Left * 2.54);
                pageSetupDialog.PageSettings.Margins.Right =
                    (int)Math.Round(
                    pageSetupDialog.PageSettings.Margins.Right * 2.54);
            }
            DialogResult ret = DialogResult.None;
            if (owner != null)
            {
                ret = pageSetupDialog.ShowDialog(owner);
            }
            else
            {
                ret = pageSetupDialog.ShowDialog();
            }
            if (ret != DialogResult.OK)
            { 
                pageSetupDialog.PageSettings.Margins.Top =
                    (int)Math.Round(
                    pageSetupDialog.PageSettings.Margins.Top / 2.54);
                pageSetupDialog.PageSettings.Margins.Bottom =
                    (int)Math.Round(
                    pageSetupDialog.PageSettings.Margins.Bottom / 2.54);
                pageSetupDialog.PageSettings.Margins.Left =
                    (int)Math.Round(
                    pageSetupDialog.PageSettings.Margins.Left / 2.54);
                pageSetupDialog.PageSettings.Margins.Right =
                    (int)Math.Round(
                    pageSetupDialog.PageSettings.Margins.Right / 2.54);
            }

            return ret;
        }

        /// <summary>
        /// Show preview dialog
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public DialogResult ShowPreviewDialog(IWin32Window owner)
        {           
            // same size with owner
            Form frm = (Form)Form.FromHandle(owner.Handle);
            previewDialog.WindowState = FormWindowState.Normal;
            previewDialog.Width = frm.Width;
            previewDialog.Height = frm.Height;
            previewDialog.Left = frm.Left;
            previewDialog.Top = frm.Top;

            action = PrintAction.PrintToPreview;
           
            return previewDialog.ShowDialog();
        }
       
        /// <summary>
        /// Print
        /// </summary>
        /// <returns></returns>
        public DialogResult Print(IWin32Window owner)
        {
            DialogResult result;
            action = PrintAction.PrintToPrinter;
            
            if (DocumentArea == null) 
                return DialogResult.Abort;

            if (owner != null)
                result = printDialog.ShowDialog(owner);
            else
                result = printDialog.ShowDialog();

            if (result == DialogResult.OK)
                document.Print();

            return result;
        }
       
        /// <summary>
        /// Print page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void document_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics gr = Graphics.FromHwnd(this.previewDialog.Handle);
            Graphics g = e.Graphics;            
            
            Bitmap origImg = DocumentArea.Image;
            float imageDpiX = origImg.HorizontalResolution;
            float imageDpiY = origImg.VerticalResolution;

            // get printed image ratio
            float ratio = GetImageRatio(origImg);

            if(PrintMokkanInfo || PrintGlassInfo)
            {
                DrawGlassLeft(g);
                DrawGlassRight(g);
                DrawGlassBottom(g);
            }

          
            Bitmap newImg = null;
            if (PrintGlassImage || (PrintGlassInfo && !_secondPage))
            {
                if ((action != PrintAction.PrintToPreview) || (imageDpiX == gr.DpiX && imageDpiX == gr.DpiY))
                {
                    newImg = new Bitmap(origImg, (int)Math.Round(origImg.Width * ratio), (int)Math.Round(origImg.Height * ratio));
                    newImg.SetResolution(imageDpiX, imageDpiY);
                }
                else
                {
                    // display in high resolution will be slow

                    // convert image to display
                    int x = (int)Math.Round((origImg.Width / imageDpiX) * gr.DpiX * ratio);
                    int y = (int)Math.Round((origImg.Height / imageDpiY) * gr.DpiY * ratio);

                    newImg = new Bitmap(origImg, x, y);
                    newImg.SetResolution(gr.DpiX, gr.DpiY);
                }

                // get start position to draw image
                Point startPoint = GetStartPosition(newImg);

                // draw image
                g.TranslateTransform(startPoint.X, startPoint.Y);
                g.DrawImage(newImg, 0, 0, newImg.Width, newImg.Height);

                // string chiku 
                string chiku_Grid = DocumentArea.GlassInfo.ChuushouChiku + "-" + DocumentArea.GlassInfo.Grid;

                Control[] chikuControls = DocumentArea.Controls.Find(chiku_Grid, false);
                if (chikuControls.Length != 0)
                {
                    MetaJouhouLabel foundControl = (MetaJouhouLabel)chikuControls[0];
                    float scale = ratio / foundControl.CurentRatio;
                    Size changeSize = new Size((int)(foundControl.Width * scale), (int)(foundControl.Height * scale));
                    Point printLocation = new Point(foundControl.Location.X - DocumentArea.AutoScrollPosition.X, foundControl.Location.Y - DocumentArea.AutoScrollPosition.X);
                    if (ratio != foundControl.CurentRatio)
                    {
                        printLocation = new Point((int)(printLocation.X * scale), (int)(printLocation.Y * scale));
                    }

                    //check if change location out of client size
                    if (printLocation.X + changeSize.Width > DocumentArea.ImageWidth * ratio)
                        printLocation.X = (int)(DocumentArea.ImageWidth * ratio - changeSize.Width);
                    if (printLocation.Y + changeSize.Height > DocumentArea.ImageHeight * ratio)
                        printLocation.Y = (int)(DocumentArea.ImageHeight * ratio - changeSize.Height);

                    Bitmap MemoryImage = new Bitmap(foundControl.ClientRectangle.Width, foundControl.ClientRectangle.Height);
                    foundControl.DrawToBitmap(MemoryImage, foundControl.ClientRectangle);
                    MemoryImage.SetResolution(gr.DpiX, gr.DpiY);
                    g.DrawImage(MemoryImage, printLocation.X,printLocation.Y, changeSize.Width, changeSize.Height);
                }

                if (PrintGlassInfo)
                {
                    startPoint = new Point(0, 0);
                    _documentArea.MokkanList.DrawToPrint(g, ratio, startPoint);
                    e.HasMorePages = true;
                    _secondPage = true;
                    return;
                }
            }

            if(PrintMokkanInfo || PrintGlassInfo)
            {
                float pWid = document.DefaultPageSettings.PrintableArea.Width;
                float pHei = document.DefaultPageSettings.PrintableArea.Height;
                float temp;
                if (document.DefaultPageSettings.Landscape)
                {
                    temp = pWid;
                    pWid = pHei;
                    pHei = temp;
                }
                
                int dWid = (int)(pWid * 0.9f);
                int dHei = (int)(pHei * 0.7f);

                int startX = (int)Math.Round((pWid - dWid) / 2);
                int startY = (int)Math.Round((pHei - dHei) / 2);

                Rectangle rec = new Rectangle(startX, startY, dWid, dHei); 
                PrintDataGrid(g, rec);

                e.HasMorePages = false;
            }

            _secondPage = false;
        }

        /// <summary>
        /// Print mokkan info
        /// </summary>        
        private void PrintDataGrid(Graphics g, Rectangle boundary)
        {
            DataGridView dtgrid = DocumentArea.GetDataGrid();
            // no mokkan 
            if(dtgrid.Rows.Count == 0)
            return;
            // alignment
            StringFormat formatHeader = new StringFormat();
            formatHeader.Alignment = StringAlignment.Center;
            formatHeader.LineAlignment = StringAlignment.Center;            
            StringFormat formatText = new StringFormat();
            formatText.Alignment = StringAlignment.Near;
            formatText.LineAlignment = StringAlignment.Center;

            // format            
            Pen penLine = new Pen(Color.Black, 1);
            Brush brushBackground = MkaDefine.PrintBackColor;
            Brush brushHeader = MkaDefine.PrintHeaderBrush;
            Brush brushText = MkaDefine.PrintTextBrush;
            Font fontHeader = new Font(MkaDefine.PrintFontName, MkaDefine.PrintHeaderSize, FontStyle.Bold);
            Font fontText = new Font(MkaDefine.PrintFontName, MkaDefine.PrintTextSize);
            
            int[] wid = new int[5];
            int[] startX = new int[5];
            String[] header = {MkaDefine.MokkanLblRBangou, MkaDefine.MokkanLblKariShakubun, MkaDefine.MokkanLblGaihouShoshuuJyouhou,MkaDefine.MokkanLblShasinBangouJyouhou, MkaDefine.MokkanLblBikou};
            int i,j;
            int startY = boundary.Top;
            int rowHeight;
            Rectangle rec;


            wid[0] = (int)(0.12 * boundary.Width);
            startX[0] = boundary.Left;
            wid[1] = (int)(0.24 * boundary.Width);
            startX[1] = startX[0] + wid[0];
            wid[2] = (int)(0.24 * boundary.Width);
            startX[2] = startX[1] + wid[1];
            wid[3] = (int)(0.24 * boundary.Width);
            startX[3] = startX[2] + wid[2];
            wid[4] = boundary.Width - wid[0] - wid[1] - wid[2] - wid[3];
            startX[4] = startX[3] + wid[3];

            for(i = 0; i < 5; i++)
            {
                rec = new Rectangle(startX[i], startY, wid[i], MkaDefine.PrintHeaderHeight);
                g.FillRectangle(brushBackground, rec);
                g.DrawRectangle(penLine, rec);
                g.DrawString(dtgrid.Columns[i].HeaderText, fontHeader, brushHeader, rec, formatHeader);
            }

            startY += MkaDefine.PrintHeaderHeight;
            rowHeight = (int)((boundary.Height - MkaDefine.PrintHeaderHeight) / dtgrid.Rows.Count);
            for (j = 0; j < dtgrid.Rows.Count; j++)
            {                
                for (i = 0; i < 5; i++)
                {
                    rec = new Rectangle(startX[i], startY, wid[i], rowHeight);
                    g.DrawRectangle(penLine, rec);
                    g.DrawString(MkaDefine.GetString(dtgrid[i, j].Value), fontText, brushText, rec, formatText);
                }
                startY += rowHeight;
            }    
        }

        /// <summary>
        /// Draw glass information in left part
        /// </summary>        
        private void DrawGlassLeft(Graphics g)
        {
            int leftRectWidth = 150;

            // format to print left part
            Pen pen = MkaDefine.PrintBorderPen;
            Brush brush = new SolidBrush(Color.Blue);
            Font font = new Font(MkaDefine.PrintFontName, MkaDefine.PrintContentSize);

            String textBat = DocumentArea.GlassInfo.BatBangou + " - " + DocumentArea.GlassInfo.GlassItaBangou;
            SizeF size = g.MeasureString(textBat, font);
            leftRectWidth = Math.Max(leftRectWidth, (int)size.Width + 10);

            // text align center in vertical and middle in horizontal
            StringFormat formatText = new StringFormat();
            formatText.Alignment = StringAlignment.Center;
            formatText.LineAlignment = StringAlignment.Center;

            int leftX = (int)document.DefaultPageSettings.PrintableArea.X + 5;
            int leftY = (int)document.DefaultPageSettings.PrintableArea.Y + 5;

            // draw left rectangle
            Rectangle rectLeft = new Rectangle(leftX, leftY, leftRectWidth, 3 * MkaDefine.PrintHeaderHeight);
            Pen penBound = MkaDefine.PrintBoundPen;
            g.DrawRectangle(penBound, rectLeft);

           
            // 大地区
            Rectangle recBig = new Rectangle(leftX, leftY, leftRectWidth, MkaDefine.PrintHeaderHeight);
            String textBig = DocumentArea.GlassInfo.OoChiku;
            g.DrawRectangle(pen, recBig);
            g.DrawString(textBig, font, brush, recBig, formatText);

            // 中小地区
            Rectangle recSmall = new Rectangle(leftX, leftY + MkaDefine.PrintHeaderHeight, leftRectWidth, MkaDefine.PrintHeaderHeight);
            String textSmall = DocumentArea.GlassInfo.ChuushouChiku;
            g.DrawRectangle(pen, recSmall);            
            g.DrawString(textSmall, font, brush, recSmall, formatText);

            // バット番号及びガラス板番号
            Rectangle recBat = new Rectangle(leftX, leftY + 2 * MkaDefine.PrintHeaderHeight, leftRectWidth, MkaDefine.PrintHeaderHeight);
            //String textBat = DocumentArea.GlassInfo.BatBangou + " - " + DocumentArea.GlassInfo.GlassItaBangou;
            g.DrawRectangle(pen, recBat); 
            g.DrawString(textBat, font, brush, recBat, formatText);
        }

        /// <summary>
        /// Draw glass information in right part
        /// </summary>        
        private void DrawGlassRight(Graphics g)
        {
            // label align center in vertical and left in horizontal
            StringFormat formatLabel = new StringFormat();
            formatLabel.Alignment = StringAlignment.Near;
            formatLabel.LineAlignment = StringAlignment.Center;

            // text align center in vertical and middle in horizontal
            StringFormat formatText = new StringFormat();
            formatText.Alignment = StringAlignment.Center;
            formatText.LineAlignment = StringAlignment.Center;

            int rightX;
            if (document.DefaultPageSettings.Landscape)
                rightX = (int)document.DefaultPageSettings.PrintableArea.Height - 405;
            else
                rightX = (int)document.DefaultPageSettings.PrintableArea.Width - 405;
            int rightY = (int)document.DefaultPageSettings.PrintableArea.Y + 5;

            // draw right rectangle
            Rectangle rectRight = new Rectangle(rightX, rightY, 400, 2 * MkaDefine.PrintHeaderHeight);
            Pen penBound = MkaDefine.PrintBoundPen;
            g.DrawRectangle(penBound, rectRight);

            // format to print left part
            Pen pen = MkaDefine.PrintBorderPen;
            Brush brushLabel = MkaDefine.PrintTextBrush;
            Brush brushText = MkaDefine.PrintHeaderBrush;
            Font fontContent = new Font(MkaDefine.PrintFontName, MkaDefine.PrintContentSize);
            Font fontLabel = new Font(MkaDefine.PrintFontName, MkaDefine.PrintLabelSize);

            // 調査次数
            Rectangle recChousaJisuu = new Rectangle(rightX, rightY, 250, MkaDefine.PrintHeaderHeight);
            g.DrawRectangle(pen, recChousaJisuu);

            // label
            recChousaJisuu = new Rectangle(rightX + 5, rightY, 100, MkaDefine.PrintHeaderHeight);
            String textChousaJisuu = "調査次数";
            g.DrawString(textChousaJisuu, fontLabel, brushLabel, recChousaJisuu, formatLabel);

            // text
            recChousaJisuu = new Rectangle(rightX + 100, rightY, 150, MkaDefine.PrintHeaderHeight);
            textChousaJisuu = DocumentArea.GlassInfo.ChousaJisuu + "次";
            g.DrawString(textChousaJisuu, fontContent, brushText, recChousaJisuu, formatText);

            // 出土日付
            Rectangle recDate = new Rectangle(rightX + 250, rightY, 150, MkaDefine.PrintHeaderHeight);
            g.DrawRectangle(pen, recDate);

            // text            
            String textDate = DocumentArea.GlassInfo.Date.ToString(MkaDefine.DatePrintFormat);
            g.DrawString(textDate, fontContent, brushText, recDate, formatText);

            // 遺構名
            Rectangle recIkoumei = new Rectangle(rightX, rightY + MkaDefine.PrintHeaderHeight, 250, MkaDefine.PrintHeaderHeight);
            g.DrawRectangle(pen, recIkoumei);

            // label
            recIkoumei = new Rectangle(rightX + 5, rightY + MkaDefine.PrintHeaderHeight, 100, MkaDefine.PrintHeaderHeight);
            String textIkoumei = "遺構名";
            g.DrawString(textIkoumei, fontLabel, brushLabel, recIkoumei, formatLabel);

            // text
            recIkoumei = new Rectangle(rightX + 100, rightY + MkaDefine.PrintHeaderHeight, 150, MkaDefine.PrintHeaderHeight);
            textIkoumei = DocumentArea.GlassInfo.Ikoumei;
            g.DrawString(textIkoumei, fontContent, brushText, recIkoumei, formatText);
        }        

        /// <summary>
        /// Draw glass information in bottom right part
        /// </summary>        
        private void DrawGlassBottom(Graphics g)
        {
            // text align center in vertical and middle in horizontal
            StringFormat formatText = new StringFormat();
            formatText.Alignment = StringAlignment.Center;
            formatText.LineAlignment = StringAlignment.Center;

            int rightX, rightY;
            if (document.DefaultPageSettings.Landscape)
            {
                rightX = (int)document.DefaultPageSettings.PrintableArea.Height - 305;
                rightY = (int)document.DefaultPageSettings.PrintableArea.Width - 35;
            }
            else
            {
                rightX = (int)document.DefaultPageSettings.PrintableArea.Width - 305;
                rightY = (int)document.DefaultPageSettings.PrintableArea.Height - 35;
            }            

            // bottom rectangle
            Rectangle rectBottom = new Rectangle(rightX, rightY, 300, MkaDefine.PrintHeaderHeight);           

            // format to print bottom part            
            Brush brush = MkaDefine.PrintTextBrush;            
            Font font = new Font(MkaDefine.PrintFontName, MkaDefine.PrintHeaderSize);         

            // print name           
            String text = MkaDefine.AssemblyCompanyJp;
            g.DrawString(text, font, brush, rectBottom, formatText);           
        }
        
        /// <summary>
        /// Get printed image ratio
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private float GetImageRatio(Bitmap image)
        {
            float ratio;

            int iWid = image.Width;
            int iHei = image.Height;

            float pWid = document.DefaultPageSettings.PrintableArea.Width;
            float pHei = document.DefaultPageSettings.PrintableArea.Height;

            if (document.DefaultPageSettings.Landscape)
            {
                float temp;
                temp = pWid;
                pWid = pHei;
                pHei = temp;
                ratio = Math.Min((float)pWid / iWid, (float)pHei / iHei);
                if (PrintGlassImage)
                    ratio *= 0.9f;
                else
                    ratio *= 0.7f;               
            }
            else
            {
                ratio = Math.Min((float)pWid / iWid, (float)pHei / iHei);
                if (PrintGlassImage)
                    ratio *= 0.95f;
                else
                    ratio *= 0.8f;              

            }

            return ratio;
        }

        /// <summary>
        /// Get start position to print image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private Point GetStartPosition(Bitmap image)
        {
            int iWid = image.Width;
            int iHei = image.Height;

            float pWid = document.DefaultPageSettings.PrintableArea.Width;
            float pHei = document.DefaultPageSettings.PrintableArea.Height;

            if (document.DefaultPageSettings.Landscape)
            {
                float temp;
                temp = pWid;
                pWid = pHei;
                pHei = temp;
            }

            int startX = (int)Math.Round((pWid - iWid) / 2);
            int startY = (int)Math.Round((pHei - iHei) / 2);

            return new Point(startX, startY);
        }
    }
}
