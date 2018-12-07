using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using MokkAnnotator.MkaCommon;

namespace MokkAnnotator.MkaImageProcess
{
    /// <summary>
    /// Mokkan Extraction class
    /// </summary>
    public static class MokkanExtraction
    {
        public static int THRES_ALPHA = 80;
        public static int THRES_MAX = 180;
        public static int MEAN_WID = 20;    // parameter to detect mokkan region
        public static int BORDER = 30;

        public static IntRange Hue = new IntRange(0, 359);
        public static DoubleRange Saturation = new DoubleRange(0.05, 1);
        public static DoubleRange Luminance = new DoubleRange(0, 0.725);

        public static DoubleRange Y = new DoubleRange(0, 0.7);
        public static DoubleRange Cb = new DoubleRange(-0.5, 0.1);
        public static DoubleRange Cr = new DoubleRange(-0.5, 0.1);
                
        public static unsafe List<Boundary> BoundaryTracking(Bitmap image, Rectangle rect)
        {
            List<Boundary> bounds = new List<Boundary>();
            Boundary bound = new Boundary();
            bound.ID = 1;

            // check pixel format            
            if (image.PixelFormat != PixelFormat.Format8bppIndexed) return bounds;

            BitmapData source = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

            // get start and stop X-Y coordinates
            int startX = rect.Left + 1;
            int startY = rect.Top + 1;
            int stopX = rect.Right - 1;
            int stopY = rect.Bottom - 1;
            int offset = source.Stride - rect.Width + 2;

            int x, y;
            int label = 1;

            int wid = image.Width;
            int hei = image.Height;
            int[,] hd = new int[wid, hei];
            int[,] f = new int[wid, hei];

            byte* src = (byte*)source.Scan0;
            src += startY * source.Stride + startX;
            for (y = startY; y < stopY; y++)
            {
                for (x = startX; x < stopX; x++, src++)
                    f[x, y] = (*src == 255) ? 0 : 1;
                src += offset;
            }

            image.UnlockBits(source);

            // from top to bottom
            for (y = startY; y < stopY; y++)
            {
                // from left to right
                for (x = startX; x < stopX; x++)
                {
                    // find black pixel
                    if (f[x, y] == 1)
                    {
                        // current point is unlabeled and its left neighbor is a white pixel
                        if (hd[x, y] == 0 && f[x - 1, y] == 0)           // external contour tracing
                        {
                            hd[x, y] = label;
                            bound = new Boundary();
                            bound.ID = label;
                            bound.AddPoint(x, y);

                            ContourTracing(x, y, 0, label, bound, f, hd);

                            bounds.Add(bound);
                            label++;
                        }
                    }
                }
            }

            return bounds;
        }

        /// <summary>
        /// Find an external or internal contour at a given pixel
        /// </summary>
        /// <param name="i">current position's x</param>
        /// <param name="j">current position's y</param>
        /// <param name="code">initial search position</param>
        /// <param name="label">label index</param>
        /// <param name="f">image of pixels</param>
        /// <param name="hd">label of pixels</param>
        private static void ContourTracing(int i, int j, byte code, int label, Boundary bound, int[,] f, int[,] hd)
        {
            int Sx, Sy;     // start point
            int Tx, Ty;     // start point's next contour point
            int i1, j1;     // current point
            int i2, j2;     // next point
            byte searchpos, prepos;
            bool isolated = false;

            Sx = i1 = i;
            Sy = j1 = j;
            i2 = j2 = 0;
            searchpos = code;

            // search the next contour point of start point
            isolated = !Tracer(i1, j1, searchpos, label, bound, out i2, out j2, out prepos, f, hd);

            // exit if current point were an isolated pixel
            if (isolated) return;

            Tx = i2;
            Ty = j2;

            do
            {
                // trace from the next contour point of start point
                i1 = i2;
                j1 = j2;
                searchpos = (byte)((prepos + 2) % 8);

                // search the next contour point of current point
                isolated = !Tracer(i1, j1, searchpos, label, bound, out i2, out j2, out prepos, f, hd);

                // exit if current point were an isolated pixel
                if (isolated) break;
            } while (i1 != Sx || j1 != Sy || i2 != Tx || j2 != Ty);
        }

        /// <summary>
        /// Search for the next contour point from the current point
        /// </summary>
        /// <param name="i">current point's x</param>
        /// <param name="j">current point's y</param>
        /// <param name="preCon">initial search position</param>
        /// <param name="label">label index</param>
        /// <param name="ii">next contour point's x</param>
        /// <param name="jj">next contour point's y</param>
        /// <param name="curCon">search position</param>
        /// <param name="f">image of pixels</param>
        /// <param name="hd">label of pixels</param>
        /// <returns>true if next contour point were found, otherwise current point is isolated pixel</returns>
        private static bool Tracer(int i, int j, byte searchpos, int label, Boundary bound, out int ii, out int jj, out byte prepos, int[,] f, int[,] hd)
        {
            bool found = false;
            byte code = 0;

            ii = jj = 0;
            prepos = 0;

            for (int k = 0; k < 8; k++)
            {
                // clockwise direction to look for the first black pixel
                code = (byte)((searchpos + k) % 8);
                switch (code)
                {
                    case 0: //right                        
                        ii = i + 1;
                        jj = j;
                        prepos = 4;
                        if (f[ii, jj] == 1)
                        {
                            found = true;
                            if (f[i, j - 1] == 0) hd[i, j - 1] = -1;            // 6
                            if (f[i + 1, j - 1] == 0) hd[i + 1, j - 1] = -1;    // 7
                        }
                        break;
                    case 1: //lower right                        
                        ii = i + 1;
                        jj = j + 1;
                        prepos = 5;
                        if (f[ii, jj] == 1)
                        {
                            found = true;
                            if (f[i + 1, j] == 0) hd[i + 1, j] = -1;            // 0
                        }
                        break;
                    case 2: //lower
                        ii = i;
                        jj = j + 1;
                        prepos = 6;
                        if (f[ii, jj] == 1)
                        {
                            found = true;
                            if (f[i + 1, j] == 0) hd[i + 1, j] = -1;            // 0                               
                            if (f[i + 1, j + 1] == 0) hd[i + 1, j + 1] = -1;    // 1
                        }
                        break;
                    case 3: //lower left
                        ii = i - 1;
                        jj = j + 1;
                        prepos = 7;
                        if (f[ii, jj] == 1)
                        {
                            found = true;
                            if (f[i + 1, j] == 0) hd[i + 1, j] = -1;            // 0
                            if (f[i, j + 1] == 0) hd[i, j + 1] = -1;            // 2
                        }
                        break;
                    case 4: //left
                        ii = i - 1;
                        jj = j;
                        prepos = 0;
                        if (f[ii, jj] == 1)
                        {
                            found = true;
                            if (f[i, j + 1] == 0) hd[i, j + 1] = -1;            // 2
                            if (f[i - 1, j + 1] == 0) hd[i - 1, j + 1] = -1;    // 3                            
                        }
                        break;
                    case 5: //upper left
                        ii = i - 1;
                        jj = j - 1;
                        prepos = 1;
                        if (f[ii, jj] == 1)
                        {
                            found = true;
                            if (f[i - 1, j] == 0) hd[i - 1, j] = -1;            // 4   
                        }
                        break;
                    case 6: //upper
                        ii = i;
                        jj = j - 1;
                        prepos = 2;
                        if (f[ii, jj] == 1)
                        {
                            found = true;
                            if (f[i - 1, j] == 0) hd[i - 1, j] = -1;            // 4
                            if (f[i - 1, j - 1] == 0) hd[i - 1, j - 1] = -1;    // 5                              
                        }
                        break;
                    case 7: //upper right
                        ii = i + 1;
                        jj = j - 1;
                        prepos = 3;
                        if (f[ii, jj] == 1)
                        {
                            found = true;
                            if (f[i - 1, j] == 0) hd[i - 1, j] = -1;            // 4
                            if (f[i, j - 1] == 0) hd[i, j - 1] = -1;            // 6
                        }
                        break;
                }

                if (found)
                {
                    if (hd[ii, jj] > 0 && hd[ii, jj] != label)
                        found = false;
                    else
                    {
                        hd[ii, jj] = label;
                        bound.AddPoint(ii, jj);
                    }
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// Color filtering in HSL color space.
        /// </summary>
        public static unsafe Bitmap HSLFilter(Bitmap image, Rectangle rect, IntRange hue, DoubleRange sat, DoubleRange lum)
        {
            Bitmap dest = new Bitmap(image.Width, image.Height, PixelFormat.Format8bppIndexed);
            BitmapData source = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            BitmapData destination = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;

            // get pixel size
            int pixelSize = (image.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;
            int srcOffset = source.Stride - rect.Width * pixelSize;
            int dstOffset = destination.Stride - rect.Width;

            RGB rgb = new RGB();
            HSL hsl = new HSL();

            // do the job
            byte* src = (byte*)source.Scan0 + startY * source.Stride + startX * pixelSize;
            byte* dst = (byte*)destination.Scan0 + startY * destination.Stride + startX;

            // for each row
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, src += pixelSize, dst++)
                {
                    rgb.Red = src[RGB.R];
                    rgb.Green = src[RGB.G];
                    rgb.Blue = src[RGB.B];

                    // convert to HSL
                    HSL.FromRGB(rgb, hsl);

                    // check HSL values
                    if (hue.IsInside(hsl.Hue) && sat.IsInside(hsl.Saturation) && lum.IsInside(hsl.Luminance))
                        *dst = 0;
                    else
                        *dst = 255;
                }

                src += srcOffset;
                dst += dstOffset;
            }

            int width = image.Width;
            int height = image.Height;
            int stride = destination.Stride;
            // remove upper part            
            for (int y = 0; y < startY; y++)
            {
                dst = (byte*)destination.Scan0.ToPointer() + y * stride;
                SystemTools.SetUnmanagedMemory(dst, 255, stride);
            }

            // remove lower part
            for (int y = stopY; y < height; y++)
            {
                dst = (byte*)destination.Scan0.ToPointer() + y * stride;
                SystemTools.SetUnmanagedMemory(dst, 255, stride);
            }

            for (int y = startY; y < stopY; y++)
            {
                // remove left part
                dst = (byte*)destination.Scan0.ToPointer() + y * stride;
                SystemTools.SetUnmanagedMemory(dst, 255, startX);

                // remove right part
                dst += stopX;
                SystemTools.SetUnmanagedMemory(dst, 255, stride - stopX);
            }

            image.UnlockBits(source);
            dest.UnlockBits(destination);

            return dest;
        }

        /// <summary>
        /// Color filtering in YCbCr color space.
        /// </summary>
        public static unsafe Bitmap YCbCrFilter(Bitmap image, Rectangle rect, DoubleRange Y, DoubleRange Cb, DoubleRange Cr)
        {
            Bitmap dest = new Bitmap(image.Width, image.Height, PixelFormat.Format8bppIndexed);
            BitmapData source = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            BitmapData destination = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;

            // get pixel size
            int pixelSize = (image.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;
            int srcOffset = source.Stride - rect.Width * pixelSize;
            int dstOffset = destination.Stride - rect.Width;

            RGB rgb = new RGB();
            YCbCr ycbcr = new YCbCr();

            // do the job
            byte* src = (byte*)source.Scan0 + startY * source.Stride + startX * pixelSize;
            byte* dst = (byte*)destination.Scan0 + startY * destination.Stride + startX;

            // for each row
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, src += pixelSize, dst++)
                {
                    rgb.Red = src[RGB.R];
                    rgb.Green = src[RGB.G];
                    rgb.Blue = src[RGB.B];

                    // convert to YCbCr
                    ycbcr = YCbCr.FromRGB(rgb);

                    // check YCbCr values
                    if (Y.IsInside(ycbcr.Y) & Cb.IsInside(ycbcr.Cb) && Cr.IsInside(ycbcr.Cr))
                        *dst = 0;
                    else
                        *dst = 255;
                }

                src += srcOffset;
                dst += dstOffset;
            }

            int width = image.Width;
            int height = image.Height;
            int stride = destination.Stride;
            // remove upper part            
            for (int y = 0; y < startY; y++)
            {
                dst = (byte*)destination.Scan0.ToPointer() + y * stride;
                SystemTools.SetUnmanagedMemory(dst, 255, stride);
            }

            // remove lower part
            for (int y = stopY; y < height; y++)
            {
                dst = (byte*)destination.Scan0.ToPointer() + y * stride;
                SystemTools.SetUnmanagedMemory(dst, 255, stride);
            }

            for (int y = startY; y < stopY; y++)
            {
                // remove left part
                dst = (byte*)destination.Scan0.ToPointer() + y * stride;
                SystemTools.SetUnmanagedMemory(dst, 255, startX);

                // remove right part
                dst += stopX;
                SystemTools.SetUnmanagedMemory(dst, 255, stride - stopX);
            }

            image.UnlockBits(source);
            dest.UnlockBits(destination);

            return dest;
        }

        /// <summary>
        /// Extract mokkan region from image
        /// </summary>
        public static unsafe Bitmap ExtractMokkanRegion(Bitmap image, Rectangle rect)
        {           
            Bitmap dest = new Bitmap(image.Width, image.Height);
            TextureBrush tb = new TextureBrush(image);
            Graphics g = Graphics.FromImage(dest);
            g.Clear(Color.White);
            g.FillRectangle(tb, rect);
            g.Dispose();
            tb.Dispose();

            return dest;
        }

        /// <summary>
        /// Search region of mokkans in image
        /// </summary>
        public static unsafe Rectangle SearchMokkanRegion(Bitmap image, Rectangle rect)
        {
            Bitmap bin = Binarization.Threshold(image, rect, MkaDefine.THRESHOLD);
            BitmapData bmd = bin.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, bin.PixelFormat);

            // calculate projection profiles
            int wid = image.Width;
            int hei = image.Height;
            int stride = bmd.Stride;
            int[] hProj = new int[wid];
            int[] vProj = new int[hei];
            int left = rect.Left;
            int right = rect.Right;
            int top = rect.Top;
            int bottom = rect.Bottom;

            // search vertical range
            byte* ptr = (byte*)bmd.Scan0.ToPointer();
            Histogram.VerHistB(ptr, bmd.Stride, rect, vProj);
            SearchMokkanRegion(vProj, ref top, ref bottom);

            // remove upper part
            ptr = (byte*)bmd.Scan0.ToPointer();
            for (int y = 0; y <= top; y++)
            {
                SystemTools.SetUnmanagedMemory(ptr, 255, wid);
                ptr += wid;
            }
            // remove lower part
            ptr = (byte*)bmd.Scan0.ToPointer() + (bottom - 1) * stride;
            for (int y = 0; y < hei - bottom; y++)
            {
                SystemTools.SetUnmanagedMemory(ptr, 255, wid);
                ptr += wid;
            }

            // search horizontal range
            ptr = (byte*)bmd.Scan0.ToPointer();
            Histogram.HorHistB(ptr, bmd.Stride, rect, hProj);
            SearchMokkanRegion(hProj, ref left, ref right);
            
            bin.UnlockBits(bmd);
            bin.Dispose();

            return Rectangle.FromLTRB(left, top, right, bottom);
        }

        /// <summary>
        /// Search region of mokkans in image
        /// </summary>
        private static void SearchMokkanRegion(int[] hist, ref int start, ref int stop)
        {
            double mean = 0.75 * Statistics.Mean(hist);
            bool run = false;
            int i, left = start, right = stop;
            int wid = 0;

            // from left to right
            for (i = start + 1; i < stop; i++)
            {
                if (hist[i] > mean && (hist[i - 1] <= mean || i == start + 1))
                {
                    left = i;
                    wid = 0;
                    run = true;
                }
                else if (hist[i - 1] > mean && (hist[i] <= mean || i == stop - 1))
                {
                    run = false;
                    if (wid > MEAN_WID) break;
                }

                if (run) wid++;
            }
            for (i = left; i >= start; i--)
                if (hist[i] == 0) break;
            left = Math.Max(i - BORDER, start);

            run = false;
            wid = 0;

            // from right to left
            for (i = stop - 2; i >= start; i--)
            {
                if (hist[i] > mean && (hist[i + 1] <= mean || i == stop - 1))
                {
                    right = i + 1;
                    wid = 0;
                    run = true;
                }
                else if (hist[i + 1] > mean && (hist[i] < mean || i == start))
                {
                    run = false;
                    if (wid > MEAN_WID) break;
                    else
                        wid = 0;
                }

                if (run) wid++;
            }
            for (i = right; i < stop; i++)
                if (hist[i] == 0) break;
            right = Math.Min(i + BORDER, stop);


            start = left;
            stop = right;
        }
    }

    /// <summary>
    /// Boundary of mokkan
    /// </summary>
    public class Boundary
    {
        public Boundary()
        {
            ID = 0;
            Left = Top = int.MaxValue;
            Right = Bottom = int.MinValue;
            Points = new List<PointF>();
        }

        public int ID;
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
        public List<PointF> Points;

        public int Width { get { return Right - Left + 1; } }
        public int Height { get { return Bottom - Top + 1; } }
        public Rectangle BoundRec { get { return Rectangle.FromLTRB(Left, Top, Right, Bottom); } }
        public int BoundArea { get { return Width * Height; } }
        public int BoundPointCount { get { return Points.Count; } }
        public float WidHeiRatio { get { return Math.Max((float)Width / Height, (float)Height / Width); } }

        public void AddPoint(int i, int j)
        {
            if (i < Left) Left = i;
            if (i > Right) Right = i;
            if (j < Top) Top = j;
            if (j > Bottom) Bottom = j;
            Points.Add(new PointF(i, j));
        }
    }
}
