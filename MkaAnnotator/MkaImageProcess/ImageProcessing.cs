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
    /// Image processing for create selection automatically
    /// </summary>
    class ImageProcessing
    {
        public static unsafe void CalHSL(Bitmap image, Rectangle rect, ref float sat, ref float lumi)
        {
            int[] Shist = new int[101];
            int[] Lhist = new int[101];

            RGB rgb = new RGB();
            HSL hsl = new HSL();

            BitmapData imgDat = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);

            if ((imgDat.PixelFormat == PixelFormat.Format24bppRgb) ||
                (imgDat.PixelFormat == PixelFormat.Format32bppRgb) ||
                (imgDat.PixelFormat == PixelFormat.Format32bppArgb))
            {
                int pixelSize = (imgDat.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;
                int offset = imgDat.Stride - rect.Width * pixelSize;

                // do the job
                byte* img = (byte*)imgDat.Scan0;

                // for each line
                for (int y = 0; y < rect.Height; y++)
                {
                    // for each pixel
                    for (int x = 0; x < rect.Width; x++, img += pixelSize)
                    {
                        rgb.Red = img[RGB.R];
                        rgb.Green = img[RGB.G];
                        rgb.Blue = img[RGB.B];
                        hsl = HSL.FromRGB(rgb);
                        Shist[(int)(hsl.Saturation * 100)]++;
                        Lhist[(int)(hsl.Luminance * 100)]++;
                    }

                    img += offset;
                }
            }
            else
            {
                int pixelSize = (imgDat.PixelFormat == PixelFormat.Format48bppRgb) ? 3 : 4;
                int imgBase = (int)imgDat.Scan0;
                int imgStride = imgDat.Stride;

                // for each line
                for (int y = 0; y < rect.Height; y++)
                {
                    ushort* src = (ushort*)(imgBase + y * imgStride);

                    // for each pixel
                    for (int x = 0; x < rect.Width; x++, imgBase += pixelSize)
                    {
                        rgb.Red = (byte)src[RGB.R];
                        rgb.Green = (byte)src[RGB.G];
                        rgb.Blue = (byte)src[RGB.B];
                        hsl = HSL.FromRGB(rgb);
                        Shist[(int)(hsl.Saturation * 100)]++;
                        Lhist[(int)(hsl.Luminance * 100)]++;
                    }
                }
            }

            image.UnlockBits(imgDat);

            int SMaxIndex = Statistics.MaxIndex(Shist);
            int LMaxIndex = Statistics.MaxIndex(Lhist);
            const int SPROP = 50;
            const int LPROP = 10;
            float minSat = (float)Shist[SMaxIndex] / SPROP;
            float minLum = (float)Lhist[LMaxIndex] / LPROP;

            int iSat, iLum;
            for (iSat = SMaxIndex; iSat < 100; iSat++)
                if (Shist[iSat] < minSat) break;

            for (iLum = LMaxIndex; iLum > 0; iLum--)
                if (Lhist[iLum] < minLum) break;

            sat = (float)iSat / 100;
            lumi = (float)iLum / 100;
        }

        public static unsafe void CalYCbCr(Bitmap image, Rectangle rect, ref float Yval, ref float Cbval, ref float Crval)
        {
            int[] Yhist = new int[101];
            int[] Cbhist = new int[101];
            int[] Crhist = new int[101];

            RGB rgb = new RGB();
            YCbCr ycbcr = new YCbCr();

            BitmapData imgDat = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);

            if ((imgDat.PixelFormat == PixelFormat.Format24bppRgb) ||
                (imgDat.PixelFormat == PixelFormat.Format32bppRgb) ||
                (imgDat.PixelFormat == PixelFormat.Format32bppArgb))
            {
                int pixelSize = (imgDat.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;
                int offset = imgDat.Stride - rect.Width * pixelSize;

                // do the job
                byte* img = (byte*)imgDat.Scan0;

                // for each line
                for (int y = 0; y < rect.Height; y++)
                {
                    // for each pixel
                    for (int x = 0; x < rect.Width; x++, img += pixelSize)
                    {
                        rgb.Red = img[RGB.R];
                        rgb.Green = img[RGB.G];
                        rgb.Blue = img[RGB.B];
                        ycbcr = YCbCr.FromRGB(rgb);
                        Yhist[(int)(ycbcr.Y * 100)]++;
                        Cbhist[(int)((ycbcr.Cb + 0.5) * 100)]++;
                        Crhist[(int)((ycbcr.Cr + 0.5) * 100)]++;
                    }

                    img += offset;
                }
            }
            else
            {
                int pixelSize = (imgDat.PixelFormat == PixelFormat.Format48bppRgb) ? 3 : 4;
                int imgBase = (int)imgDat.Scan0;
                int imgStride = imgDat.Stride;

                // for each line
                for (int y = 0; y < rect.Height; y++)
                {
                    ushort* src = (ushort*)(imgBase + y * imgStride);

                    // for each pixel
                    for (int x = 0; x < rect.Width; x++, imgBase += pixelSize)
                    {
                        rgb.Red = (byte)src[RGB.R];
                        rgb.Green = (byte)src[RGB.G];
                        rgb.Blue = (byte)src[RGB.B];
                        ycbcr = YCbCr.FromRGB(rgb);
                        Yhist[(int)(ycbcr.Y * 100)]++;
                        Cbhist[(int)((ycbcr.Cb + 0.5) * 100)]++;
                        Crhist[(int)((ycbcr.Cr + 0.5) * 100)]++;
                    }
                }
            }

            image.UnlockBits(imgDat);

            int YMaxIndex = Statistics.MaxIndex(Yhist);
            int CbMaxIndex = Statistics.MaxIndex(Cbhist);
            int CrMaxIndex = Statistics.MaxIndex(Crhist);
            const int YPROP = 10;
            const int CbPROP = 100;
            const int CrPROP = 100;
            float minY = (float)Yhist[YMaxIndex] / YPROP;
            float minCb = (float)Cbhist[CbMaxIndex] / CbPROP;
            float minCr = (float)Crhist[CrMaxIndex] / CrPROP;

            int iY, iCb, iCr;
            for (iY = YMaxIndex; iY > 0; iY--)
                if (Yhist[iY] < minY) break;
            for (iCb = CbMaxIndex; iCb > 0; iCb--)
                if (Cbhist[iCb] < minCb) break;
            for (iCr = CrMaxIndex; iCr > 0; iCr--)
                if (Crhist[iCr] < minCr) break;

            Yval = (float)iY / 100;
            Cbval = (float)(iCb - 50) / 100;
            Crval = (float)(iCr - 50) / 100;
        }

        /// <summary>
        /// Color filtering in HSL color space.
        /// </summary>
        public static unsafe Bitmap HSLFilter(Bitmap image, Rectangle rect, float sat, float lumi)
        {
            Bitmap dest = new Bitmap(rect.Width, rect.Height, PixelFormat.Format8bppIndexed);
            BitmapData source = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);
            BitmapData destination = dest.LockBits(rect, ImageLockMode.ReadWrite, dest.PixelFormat);

            // get pixel size
            int pixelSize = (image.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;
            int srcOffset = source.Stride - rect.Width * pixelSize;
            int dstOffset = destination.Stride - rect.Width;

            RGB rgb = new RGB();
            HSL hsl = new HSL();

            // do the job
            byte* src = (byte*)source.Scan0.ToPointer();
            byte* dst = (byte*)destination.Scan0.ToPointer();

            // for each row
            for (int y = 0; y < rect.Height; y++)
            {
                // for each pixel
                for (int x = 0; x < rect.Width; x++, src += pixelSize, dst++)
                {
                    rgb.Red = src[RGB.R];
                    rgb.Green = src[RGB.G];
                    rgb.Blue = src[RGB.B];

                    // convert to HSL
                    HSL.FromRGB(rgb, hsl);

                    // check HSL values
                    if (hsl.Saturation <= sat && hsl.Luminance >= lumi)
                        *dst = 255;
                    else
                        *dst = 0;
                }

                src += srcOffset;
                dst += dstOffset;
            }

            image.UnlockBits(source);
            dest.UnlockBits(destination);

            return dest;
        }

        /// <summary>
        /// Color filtering in YCbCr color space.
        /// </summary>
        public static unsafe Bitmap YCbCrFilter(Bitmap image, Rectangle rect, float Y, float Cb, float Cr)
        {
            Bitmap dest = new Bitmap(rect.Width, rect.Height, PixelFormat.Format8bppIndexed);
            BitmapData source = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);
            BitmapData destination = dest.LockBits(rect, ImageLockMode.ReadWrite, dest.PixelFormat);

            // get pixel size
            int pixelSize = (image.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;
            int srcOffset = source.Stride - rect.Width * pixelSize;
            int dstOffset = destination.Stride - rect.Width;

            RGB rgb = new RGB();
            YCbCr ycbcr = new YCbCr();

            // do the job
            byte* src = (byte*)source.Scan0.ToPointer();
            byte* dst = (byte*)destination.Scan0.ToPointer();

            // for each row
            for (int y = 0; y < rect.Height; y++)
            {
                // for each pixel
                for (int x = 0; x < rect.Width; x++, src += pixelSize, dst++)
                {
                    rgb.Red = src[RGB.R];
                    rgb.Green = src[RGB.G];
                    rgb.Blue = src[RGB.B];

                    // convert to YCbCr
                    ycbcr = YCbCr.FromRGB(rgb);

                    // check YCbCr values
                    if ((ycbcr.Y >= Y) && (ycbcr.Cb >= Cb) && (ycbcr.Cr >= Cr))
                        *dst = 255;
                    else
                        *dst = 0;
                }

                src += srcOffset;
                dst += dstOffset;
            }

            image.UnlockBits(source);
            dest.UnlockBits(destination);

            return dest;
        }

        /// <summary>
        /// Get thresholded image
        /// </summary>
        public static unsafe Bitmap Thresholding(Bitmap image, Rectangle rect)
        {
            Bitmap dest = (Bitmap)image.Clone();
            BitmapData bmd = dest.LockBits(rect, ImageLockMode.ReadOnly, dest.PixelFormat);

            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;
            int offset = bmd.Stride - rect.Width;

            // calculate threshold value
            int threshold = MaxEntropyThreshold(bmd, rect);
            threshold = (int)((threshold + MkaDefine.THRESHOLD) / 2 + 0.5);

            // do the job
            byte* ptr = (byte*)bmd.Scan0;

            // allign pointer to the first pixel to process
            ptr += (startY * bmd.Stride + startX);

            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                {
                    *ptr = (byte)((*ptr >= threshold) ? 255 : 0);
                }
                ptr += offset;
            }

            dest.UnlockBits(bmd);

            return dest;
        }

        /// <summary>
        /// Threshold using Entropy of the Histogram
        /// </summary>    
        /// 
        /// <remarks>
        /// <para>Implements Kapur-Sahoo-Wong (Maximum Entropy) thresholding method
        /// <b>Kapur J.N., Sahoo P.K., and Wong A.K.C. (1985) "A New Method for Gray-Level Picture Thresholding Using the Entropy of the Histogram"
        /// Graphical Models and Image Processing, 29(3): 273-285 </b></para>
        /// </remarks>
        public static unsafe int MaxEntropyThreshold(BitmapData bmd, Rectangle rect)
        {
            int thresholdValue = 0;

            // get start and stop X-Y coordinates
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;
            int offset = bmd.Stride - rect.Width;

            // histogram array
            int[] histogram = new int[256];

            int ih, it;
            int first_bin;
            int last_bin;
            double tot_ent;  /* total entropy */
            double max_ent;  /* max entropy */
            double ent_back; /* entropy of the background pixels at a given threshold */
            double ent_obj;  /* entropy of the object pixels at a given threshold */
            double[] norm_histo = new double[256]; /* normalized histogram */
            double[] P1 = new double[256]; /* cumulative normalized histogram */
            double[] P2 = new double[256];

            unsafe
            {
                // collect histogram first
                byte* ptr = (byte*)bmd.Scan0.ToPointer();

                // align pointer to the first pixel to process
                ptr += (startY * bmd.Stride + startX);

                // for each line	
                for (int y = startY; y < stopY; y++)
                {
                    // for each pixel
                    for (int x = startX; x < stopX; x++, ptr++)
                    {
                        histogram[*ptr]++;
                    }
                    ptr += offset;
                }

                int total = 0;
                for (ih = 0; ih < 256; ih++)
                    total += histogram[ih];

                for (ih = 0; ih < 256; ih++)
                    norm_histo[ih] = (double)histogram[ih] / total;

                P1[0] = norm_histo[0];
                P2[0] = 1.0 - P1[0];
                for (ih = 1; ih < 256; ih++)
                {
                    P1[ih] = P1[ih - 1] + norm_histo[ih];
                    P2[ih] = 1.0 - P1[ih];
                }

                /* Determine the first non-zero bin */
                first_bin = 0;
                for (ih = 0; ih < 256; ih++)
                {
                    if (!(Math.Abs(P1[ih]) < 2.220446049250313E-16))
                    {
                        first_bin = ih;
                        break;
                    }
                }

                /* Determine the last non-zero bin */
                last_bin = 255;
                for (ih = 255; ih >= first_bin; ih--)
                {
                    if (!(Math.Abs(P2[ih]) < 2.220446049250313E-16))
                    {
                        last_bin = ih;
                        break;
                    }
                }

                // Calculate the total entropy each gray-level
                // and find the threshold that maximizes it 
                max_ent = Double.MinValue;

                for (it = first_bin; it <= last_bin; it++)
                {
                    /* Entropy of the background pixels */
                    ent_back = 0.0;
                    for (ih = 0; ih <= it; ih++)
                    {
                        if (histogram[ih] != 0)
                        {
                            ent_back -= (norm_histo[ih] / P1[it]) * Math.Log(norm_histo[ih] / P1[it]);
                        }
                    }

                    /* Entropy of the object pixels */
                    ent_obj = 0.0;
                    for (ih = it + 1; ih < 256; ih++)
                    {
                        if (histogram[ih] != 0)
                        {
                            ent_obj -= (norm_histo[ih] / P2[it]) * Math.Log(norm_histo[ih] / P2[it]);
                        }
                    }

                    /* Total entropy */
                    tot_ent = ent_back + ent_obj;

                    // IJ.log(""+max_ent+"  "+tot_ent);
                    if (max_ent < tot_ent)
                    {
                        max_ent = tot_ent;
                        thresholdValue = it;
                    }
                }
            }

            return thresholdValue;
        }

        /// <summary>
        /// Opening processing
        /// </summary>
        /// <param name="image">process image</param>        
        /// <param name="rect">boundary of image</param>
        public static Bitmap Opening(Bitmap image, Rectangle rect)
        {
            Bitmap procImg = (Bitmap)image.Clone();

            // erosion
            procImg = Erosion(procImg, rect);

            // dilatation
            procImg = Dilatation(procImg, rect);

            return procImg;
        }

        /// <summary>
        /// Dilatation processing
        /// </summary>
        /// <param name="image">process image</param>        
        /// <param name="rect">boundary of image</param>
        public static unsafe Bitmap Dilatation(Bitmap image, Rectangle rect)
        {
            Bitmap source = image;
            Bitmap dest = (Bitmap)image.Clone();

            BitmapData sourceData = source.LockBits(rect, ImageLockMode.ReadOnly, source.PixelFormat);
            BitmapData destData = dest.LockBits(rect, ImageLockMode.ReadOnly, dest.PixelFormat);
            PixelFormat pixelFormat = sourceData.PixelFormat;

            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;

            // 3x3 structuring element            
            int size = 3;
            short[,] se = new short[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };

            // structuring element's radius
            int r = size >> 1;

            if ((pixelFormat == PixelFormat.Format8bppIndexed) || (pixelFormat == PixelFormat.Format24bppRgb))
            {
                int pixelSize = (pixelFormat == PixelFormat.Format8bppIndexed) ? 1 : 3;

                int dstStride = destData.Stride;
                int srcStride = sourceData.Stride;

                // base pointers
                byte* baseSrc = (byte*)sourceData.Scan0;
                byte* baseDst = (byte*)destData.Scan0;

                // allign pointers by X
                baseSrc += (startX * pixelSize);
                baseDst += (startX * pixelSize);

                if (pixelFormat == PixelFormat.Format8bppIndexed)
                {
                    // grayscale image

                    // compute each line
                    for (int y = startY; y < stopY; y++)
                    {
                        byte* src = baseSrc + y * srcStride;
                        byte* dst = baseDst + y * dstStride;

                        byte max, v;

                        // loop and array indexes
                        int t, ir, jr, i, j;

                        // for each pixel
                        for (int x = startX; x < stopX; x++, src++, dst++)
                        {
                            max = 0;

                            // for each structuring element's row
                            for (i = 0; i < size; i++)
                            {
                                ir = i - r;
                                t = y + ir;

                                // skip row
                                if (t < startY)
                                    continue;
                                // break
                                if (t >= stopY)
                                    break;

                                // for each structuring slement's column
                                for (j = 0; j < size; j++)
                                {
                                    jr = j - r;
                                    t = x + jr;

                                    // skip column
                                    if (t < startX)
                                        continue;
                                    if (t < stopX)
                                    {
                                        if (se[i, j] == 1)
                                        {
                                            // get new MAX value
                                            v = src[ir * srcStride + jr];
                                            if (v > max)
                                                max = v;
                                        }
                                    }
                                }
                            }
                            // result pixel
                            *dst = max;
                        }
                    }
                }
                else
                {
                    // 24 bpp color image

                    // compute each line
                    for (int y = startY; y < stopY; y++)
                    {
                        byte* src = baseSrc + y * srcStride;
                        byte* dst = baseDst + y * dstStride;

                        byte maxR, maxG, maxB, v;
                        byte* p;

                        // loop and array indexes
                        int t, ir, jr, i, j;

                        // for each pixel
                        for (int x = startX; x < stopX; x++, src += 3, dst += 3)
                        {
                            maxR = maxG = maxB = 0;

                            // for each structuring element's row
                            for (i = 0; i < size; i++)
                            {
                                ir = i - r;
                                t = y + ir;

                                // skip row
                                if (t < startY)
                                    continue;
                                // break
                                if (t >= stopY)
                                    break;

                                // for each structuring element's column
                                for (j = 0; j < size; j++)
                                {
                                    jr = j - r;
                                    t = x + jr;

                                    // skip column
                                    if (t < startX)
                                        continue;
                                    if (t < stopX)
                                    {
                                        if (se[i, j] == 1)
                                        {
                                            // get new MAX values
                                            p = &src[ir * srcStride + jr * 3];

                                            // red
                                            v = p[2];
                                            if (v > maxR)
                                                maxR = v;

                                            // green
                                            v = p[1];
                                            if (v > maxG)
                                                maxG = v;

                                            // blue
                                            v = p[0];
                                            if (v > maxB)
                                                maxB = v;
                                        }
                                    }
                                }
                            }
                            // result pixel
                            dst[2] = maxR;
                            dst[1] = maxG;
                            dst[0] = maxB;
                        }
                    }
                }
            }
            else
            {
                int pixelSize = (pixelFormat == PixelFormat.Format16bppGrayScale) ? 1 : 3;

                int dstStride = destData.Stride / 2;
                int srcStride = sourceData.Stride / 2;

                // base pointers
                ushort* baseSrc = (ushort*)sourceData.Scan0;
                ushort* baseDst = (ushort*)destData.Scan0;

                // allign pointers by X
                baseSrc += (startX * pixelSize);
                baseDst += (startX * pixelSize);

                if (pixelFormat == PixelFormat.Format16bppGrayScale)
                {
                    // 16 bpp grayscale image

                    // compute each line
                    for (int y = startY; y < stopY; y++)
                    {
                        ushort* src = baseSrc + y * srcStride;
                        ushort* dst = baseDst + y * dstStride;

                        ushort max, v;

                        // loop and array indexes
                        int t, ir, jr, i, j;

                        // for each pixel
                        for (int x = startX; x < stopX; x++, src++, dst++)
                        {
                            max = 0;

                            // for each structuring element's row
                            for (i = 0; i < size; i++)
                            {
                                ir = i - r;
                                t = y + ir;

                                // skip row
                                if (t < startY)
                                    continue;
                                // break
                                if (t >= stopY)
                                    break;

                                // for each structuring slement's column
                                for (j = 0; j < size; j++)
                                {
                                    jr = j - r;
                                    t = x + jr;

                                    // skip column
                                    if (t < startX)
                                        continue;
                                    if (t < stopX)
                                    {
                                        if (se[i, j] == 1)
                                        {
                                            // get new MAX value
                                            v = src[ir * srcStride + jr];
                                            if (v > max)
                                                max = v;
                                        }
                                    }
                                }
                            }
                            // result pixel
                            *dst = max;
                        }
                    }
                }
                else
                {
                    // 48 bpp color image

                    // compute each line
                    for (int y = startY; y < stopY; y++)
                    {
                        ushort* src = baseSrc + y * srcStride;
                        ushort* dst = baseDst + y * dstStride;

                        ushort maxR, maxG, maxB, v;
                        ushort* p;

                        // loop and array indexes
                        int t, ir, jr, i, j;

                        // for each pixel
                        for (int x = startX; x < stopX; x++, src += 3, dst += 3)
                        {
                            maxR = maxG = maxB = 0;

                            // for each structuring element's row
                            for (i = 0; i < size; i++)
                            {
                                ir = i - r;
                                t = y + ir;

                                // skip row
                                if (t < startY)
                                    continue;
                                // break
                                if (t >= stopY)
                                    break;

                                // for each structuring element's column
                                for (j = 0; j < size; j++)
                                {
                                    jr = j - r;
                                    t = x + jr;

                                    // skip column
                                    if (t < startX)
                                        continue;
                                    if (t < stopX)
                                    {
                                        if (se[i, j] == 1)
                                        {
                                            // get new MAX values
                                            p = &src[ir * srcStride + jr * 3];

                                            // red
                                            v = p[2];
                                            if (v > maxR)
                                                maxR = v;

                                            // green
                                            v = p[1];
                                            if (v > maxG)
                                                maxG = v;

                                            // blue
                                            v = p[0];
                                            if (v > maxB)
                                                maxB = v;
                                        }
                                    }
                                }
                            }
                            // result pixel
                            dst[2] = maxR;
                            dst[1] = maxG;
                            dst[0] = maxB;
                        }
                    }
                }
            }

            source.UnlockBits(sourceData);
            dest.UnlockBits(destData);

            source.Dispose();

            return dest;
        }

        /// <summary>
        /// Erosion processing
        /// </summary>
        /// <param name="image">process image</param>        
        /// <param name="rect">boundary of image</param>
        public static unsafe Bitmap Erosion(Bitmap image, Rectangle rect)
        {
            Bitmap source = image;
            Bitmap dest = (Bitmap)image.Clone();

            BitmapData sourceData = source.LockBits(rect, ImageLockMode.ReadOnly, source.PixelFormat);
            BitmapData destData = dest.LockBits(rect, ImageLockMode.ReadOnly, dest.PixelFormat);
            PixelFormat pixelFormat = sourceData.PixelFormat;

            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;

            // 3x3 structuring element            
            int size = 3;
            short[,] se = new short[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };

            // structuring element's radius
            int r = size >> 1;

            if ((pixelFormat == PixelFormat.Format8bppIndexed) || (pixelFormat == PixelFormat.Format24bppRgb))
            {
                int pixelSize = (pixelFormat == PixelFormat.Format8bppIndexed) ? 1 : 3;

                int srcStride = sourceData.Stride;
                int dstStride = destData.Stride;

                // base pointers
                byte* baseSrc = (byte*)sourceData.Scan0;
                byte* baseDst = (byte*)destData.Scan0;

                // allign pointers by X
                baseSrc += (startX * pixelSize);
                baseDst += (startX * pixelSize);

                if (pixelFormat == PixelFormat.Format8bppIndexed)
                {
                    // grayscale image

                    // compute each line
                    for (int y = startY; y < stopY; y++)
                    {
                        byte* src = baseSrc + y * srcStride;
                        byte* dst = baseDst + y * dstStride;

                        byte min, v;

                        // loop and array indexes
                        int t, ir, jr, i, j;

                        // for each pixel
                        for (int x = startX; x < stopX; x++, src++, dst++)
                        {
                            min = 255;

                            // for each structuring element's row
                            for (i = 0; i < size; i++)
                            {
                                ir = i - r;
                                t = y + ir;

                                // skip row
                                if (t < startY)
                                    continue;
                                // break
                                if (t >= stopY)
                                    break;

                                // for each structuring element's column
                                for (j = 0; j < size; j++)
                                {
                                    jr = j - r;
                                    t = x + jr;

                                    // skip column
                                    if (t < startX)
                                        continue;
                                    if (t < stopX)
                                    {
                                        if (se[i, j] == 1)
                                        {
                                            // get new MIN value
                                            v = src[ir * srcStride + jr];
                                            if (v < min)
                                                min = v;
                                        }
                                    }
                                }
                            }
                            // result pixel
                            *dst = min;
                        }
                    }
                }
                else
                {
                    // 24 bpp color image

                    // compute each line
                    for (int y = startY; y < stopY; y++)
                    {
                        byte* src = baseSrc + y * srcStride;
                        byte* dst = baseDst + y * dstStride;

                        byte minR, minG, minB, v;
                        byte* p;

                        // loop and array indexes
                        int t, ir, jr, i, j;

                        // for each pixel
                        for (int x = startX; x < stopX; x++, src += 3, dst += 3)
                        {
                            minR = minG = minB = 255;

                            // for each structuring element's row
                            for (i = 0; i < size; i++)
                            {
                                ir = i - r;
                                t = y + ir;

                                // skip row
                                if (t < startY)
                                    continue;
                                // break
                                if (t >= stopY)
                                    break;

                                // for each structuring element's column
                                for (j = 0; j < size; j++)
                                {
                                    jr = j - r;
                                    t = x + jr;

                                    // skip column
                                    if (t < startX)
                                        continue;
                                    if (t < stopX)
                                    {
                                        if (se[i, j] == 1)
                                        {
                                            // get new MIN values
                                            p = &src[ir * srcStride + jr * 3];

                                            // red
                                            v = p[2];
                                            if (v < minR)
                                                minR = v;

                                            // green
                                            v = p[1];
                                            if (v < minG)
                                                minG = v;

                                            // blue
                                            v = p[0];
                                            if (v < minB)
                                                minB = v;
                                        }
                                    }
                                }
                            }
                            // result pixel
                            dst[2] = minR;
                            dst[1] = minG;
                            dst[0] = minB;
                        }
                    }
                }
            }
            else
            {
                int pixelSize = (pixelFormat == PixelFormat.Format16bppGrayScale) ? 1 : 3;

                int dstStride = destData.Stride / 2;
                int srcStride = sourceData.Stride / 2;

                // base pointers
                ushort* baseSrc = (ushort*)sourceData.Scan0;
                ushort* baseDst = (ushort*)destData.Scan0;

                // allign pointers by X
                baseSrc += (startX * pixelSize);
                baseDst += (startX * pixelSize);

                if (pixelFormat == PixelFormat.Format16bppGrayScale)
                {
                    // 16 bpp grayscale image

                    // compute each line
                    for (int y = startY; y < stopY; y++)
                    {
                        ushort* src = baseSrc + y * srcStride;
                        ushort* dst = baseDst + y * dstStride;

                        ushort min, v;

                        // loop and array indexes
                        int t, ir, jr, i, j;

                        // for each pixel
                        for (int x = startX; x < stopX; x++, src++, dst++)
                        {
                            min = 65535;

                            // for each structuring element's row
                            for (i = 0; i < size; i++)
                            {
                                ir = i - r;
                                t = y + ir;

                                // skip row
                                if (t < startY)
                                    continue;
                                // break
                                if (t >= stopY)
                                    break;

                                // for each structuring element's column
                                for (j = 0; j < size; j++)
                                {
                                    jr = j - r;
                                    t = x + jr;

                                    // skip column
                                    if (t < startX)
                                        continue;
                                    if (t < stopX)
                                    {
                                        if (se[i, j] == 1)
                                        {
                                            // get new MIN value
                                            v = src[ir * srcStride + jr];
                                            if (v < min)
                                                min = v;
                                        }
                                    }
                                }
                            }
                            // result pixel
                            *dst = min;
                        }
                    }
                }
                else
                {
                    // 48 bpp color image

                    // compute each line
                    for (int y = startY; y < stopY; y++)
                    {
                        ushort* src = baseSrc + y * srcStride;
                        ushort* dst = baseDst + y * dstStride;

                        ushort minR, minG, minB, v;
                        ushort* p;

                        // loop and array indexes
                        int t, ir, jr, i, j;

                        // for each pixel
                        for (int x = startX; x < stopX; x++, src += 3, dst += 3)
                        {
                            minR = minG = minB = 65535;

                            // for each structuring element's row
                            for (i = 0; i < size; i++)
                            {
                                ir = i - r;
                                t = y + ir;

                                // skip row
                                if (t < startY)
                                    continue;
                                // break
                                if (t >= stopY)
                                    break;

                                // for each structuring element's column
                                for (j = 0; j < size; j++)
                                {
                                    jr = j - r;
                                    t = x + jr;

                                    // skip column
                                    if (t < startX)
                                        continue;
                                    if (t < stopX)
                                    {
                                        if (se[i, j] == 1)
                                        {
                                            // get new MIN values
                                            p = &src[ir * srcStride + jr * 3];

                                            // red
                                            v = p[2];
                                            if (v < minR)
                                                minR = v;

                                            // green
                                            v = p[1];
                                            if (v < minG)
                                                minG = v;

                                            // blue
                                            v = p[0];
                                            if (v < minB)
                                                minB = v;
                                        }
                                    }
                                }
                            }
                            // result pixel
                            dst[2] = minR;
                            dst[1] = minG;
                            dst[0] = minB;
                        }
                    }
                }
            }

            source.UnlockBits(sourceData);
            dest.UnlockBits(destData);

            source.Dispose();

            return dest;
        }

        //public static unsafe Bitmap BoundaryTracking(Bitmap image, Rectangle rect)
        public static unsafe List<Boundary> BoundaryTracking(Bitmap image, Rectangle rect)
        {
            List<Boundary> bounds = new List<Boundary>();
            Boundary bound = new Boundary();
            bound.ID = 1;

            // check pixel format
            //if (image.PixelFormat != PixelFormat.Format8bppIndexed) return image;
            if (image.PixelFormat != PixelFormat.Format8bppIndexed) return bounds;
            BitmapData source = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);

            int stride = source.Stride;
            int offset = stride - rect.Width + 2;

            int x, y, nx, ny;
            int label = 1;

            nx = rect.Width;
            ny = rect.Height;
            int[,] hd = new int[nx, ny];
            int[,] f = new int[nx, ny];

            byte* src = (byte*)source.Scan0.ToPointer();
            for (y = 1; y < ny - 1; y++)
            {
                for (x = 1; x < nx - 1; x++, src++)
                    f[x, y] = (*src == 255) ? 0 : 1;
                src += offset;
            }

            image.UnlockBits(source);

            // from top to bottom
            for (y = 1; y < ny - 1; y++)
            {
                // from left to right
                for (x = 1; x < nx - 1; x++)
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

            //// display
            //int count = label + 1;

            //Color[] colors = new Color[count];
            //Random rand = new Random();
            //for (i = 0; i < count; i++)
            //{
            //    colors[i] = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
            //}

            //Bitmap dest = new Bitmap(nx, ny, PixelFormat.Format24bppRgb);            
            //Point p;
            //int id;
            //for (i = 0; i < comps.Count; i++)
            //{
            //    if(comps[i].Points.Count < 100) continue;
            //    for (j = 0; j < comps[i].Points.Count; j++)
            //    {
            //        p = comps[i].Points[j];
            //        id = comps[i].ID;
            //        dest.SetPixel(p.X, p.Y, colors[i]);
            //    }
            //}

            //return dest;
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
    }
}
