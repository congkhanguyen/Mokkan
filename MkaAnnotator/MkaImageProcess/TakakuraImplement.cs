using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace MokkAnnotator.MkaImageProcess
{
    class TakakuraImplement
    {
        /// <summary>
        /// Stretch RGB histogram to create new image
        /// </summary>        
        public static unsafe Bitmap HistogramStretchRGB(Bitmap image, Rectangle rect)
        {
            Bitmap dest = (Bitmap)image.Clone();
            BitmapData srcDat = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);
            BitmapData dstDat = dest.LockBits(rect, ImageLockMode.ReadWrite, dest.PixelFormat);

            int width = rect.Width;
            int height = rect.Height;
            int stride = srcDat.Stride;

            // get pixel size
            int pixelSize = (image.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;

            int offset = stride - width * pixelSize;

            // do the job
            byte* src = (byte*)srcDat.Scan0.ToPointer();
            byte* dst = (byte*)dstDat.Scan0.ToPointer();

            int pix_num = width * height;
            int[] num_R = new int[256];
            int[] num_G = new int[256];
            int[] num_B = new int[256];
            int per1 = pix_num / 100;
            int per99 = pix_num / 100 * 99;

            // RGB histograms
            // for each row
            for (int y = 0; y < height; y++)
            {
                // for each pixel
                for (int x = 0; x < width; x++, src += pixelSize)
                {
                    num_R[src[RGB.R]]++;
                    num_G[src[RGB.G]]++;
                    num_B[src[RGB.B]]++;
                }
                src += offset;
            }

            int min_perR, max_perR;
            int min_perG, max_perG;
            int min_perB, max_perB;
            double ratioR, ratioG, ratioB;
            byte src_pix, dst_pix;
            int count_i;
            int sum;

            // B
            count_i = sum = 0;
            do
            {
                sum += num_R[count_i++];
            } while (sum < per1);
            min_perR = count_i - 1;

            do
            {
                sum += num_R[count_i++];
            } while (sum < per99);
            max_perR = count_i - 1;
            ratioR = (double)255.0 / (max_perR - min_perR);

            // G
            count_i = sum = 0;
            do
            {
                sum += num_G[count_i++];
            } while (sum < per1);
            min_perG = count_i - 1;

            do
            {
                sum += num_G[count_i++];
            } while (sum < per99);
            max_perG = count_i - 1;
            ratioG = (double)255.0 / (max_perG - min_perG);

            // B
            count_i = sum = 0;
            do
            {
                sum += num_B[count_i++];
            } while (sum < per1);
            min_perB = count_i - 1;

            do
            {
                sum += num_B[count_i++];
            } while (sum < per99);
            max_perB = count_i - 1;
            ratioB = (double)255.0 / (max_perB - min_perB);

            // stretch histogram
            src = (byte*)srcDat.Scan0.ToPointer();
            // for each row
            for (int y = 0; y < height; y++)
            {
                // for each pixel
                for (int x = 0; x < width; x++, src += pixelSize, dst += pixelSize)
                {
                    // R
                    src_pix = src[RGB.R];

                    if (src_pix <= min_perR) dst_pix = 0;
                    else if (src_pix >= max_perR) dst_pix = 255;
                    else dst_pix = (byte)((src_pix - min_perR) * ratioR);

                    if (dst_pix < 0) dst[RGB.R] = 0;
                    else if (dst_pix >= 255) dst[RGB.R] = 255;
                    else dst[RGB.R] = dst_pix;

                    // G
                    src_pix = src[RGB.G];

                    if (src_pix <= min_perG) dst_pix = 0;
                    else if (src_pix >= max_perG) dst_pix = 255;
                    else dst_pix = (byte)((src_pix - min_perG) * ratioG);

                    if (dst_pix < 0) dst[RGB.G] = 0;
                    else if (dst_pix >= 255) dst[RGB.G] = 255;
                    else dst[RGB.G] = dst_pix;

                    // B
                    src_pix = src[RGB.B];

                    if (src_pix <= min_perB) dst_pix = 0;
                    else if (src_pix >= max_perB) dst_pix = 255;
                    else dst_pix = (byte)((src_pix - min_perB) * ratioB);

                    if (dst_pix < 0) dst[RGB.B] = 0;
                    else if (dst_pix >= 255) dst[RGB.B] = 255;
                    else dst[RGB.B] = dst_pix;
                }
                src += offset;
                dst += offset;
            }

            image.UnlockBits(srcDat);
            dest.UnlockBits(dstDat);

            return dest;
        }

        /// <summary>
        /// Stretch HSV histogram to create new image
        /// </summary>        
        public static unsafe Bitmap HistogramStretchHSV(Bitmap image, Rectangle rect)
        {
            Bitmap dest = (Bitmap)image.Clone();
            BitmapData srcDat = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);
            BitmapData dstDat = dest.LockBits(rect, ImageLockMode.ReadWrite, dest.PixelFormat);

            int width = rect.Width;
            int height = rect.Height;
            int stride = srcDat.Stride;

            // get pixel size
            int pixelSize = (image.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;

            int offset = stride - width * pixelSize;

            // do the job
            byte* src = (byte*)srcDat.Scan0.ToPointer();
            byte* dst = (byte*)dstDat.Scan0.ToPointer();

            int pix_num = width * height;	    //縦×横
            int[] num_b = new int[512];
            int min_per, max_per;
            int per1 = pix_num / 100;
            int per99 = pix_num / 100 * 99;
            int[] aryH = new int[pix_num];
            int[] aryS = new int[pix_num];
            int[] aryV = new int[pix_num];
            int maxH, maxS, maxV;
            int minV;
            maxH = maxS = maxV = 0;
            minV = 255;

            int tmp_pix, last_pix;
            double ratio;
            int pos;
            int count_i = 0;
            int sum = 0;

            RGB rgb = new RGB();
            HSV hsv = new HSV();

            // RGB histograms
            // for each row
            for (int y = 0; y < height; y++)
            {
                // for each pixel
                for (int x = 0; x < width; x++, src += pixelSize)
                {
                    rgb.Red = src[RGB.R];
                    rgb.Green = src[RGB.G];
                    rgb.Blue = src[RGB.B];
                    hsv = HSV.FromRGB(rgb);

                    pos = y * width + x;
                    aryH[pos] = hsv.Hue;
                    aryS[pos] = hsv.Saturation;
                    aryV[pos] = hsv.Value;

                    if (hsv.Hue > maxH) maxH = hsv.Hue;
                    if (hsv.Saturation > maxS) maxS = hsv.Saturation;
                    if (hsv.Value > maxV) maxV = hsv.Value;
                    if (hsv.Value < minV) minV = hsv.Value;
                }
                src += offset;
            }

            // histogram
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++, src += pixelSize)
                    num_b[aryV[y * width + x]]++;

            do
            {
                sum += num_b[count_i++];
            } while (sum < per1);
            min_per = count_i - 1;

            do
            {
                sum += num_b[count_i++];
            } while (sum < per99);
            max_per = count_i - 1;
            ratio = (double)(511.0 / (max_per - min_per));

            // stretch histogram
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++, src += pixelSize)
                {
                    pos = y * width + x;
                    tmp_pix = aryV[pos];

                    if (tmp_pix <= min_per) last_pix = 0;
                    else if (tmp_pix >= max_per) last_pix = 511;
                    else last_pix = (int)((tmp_pix - min_per) * ratio);

                    if (last_pix < 0) aryV[pos] = 0;
                    else if (last_pix >= 511) aryV[pos] = 511;
                    else aryV[pos] = last_pix;
                }
            }

            // for each row
            for (int y = 0; y < height; y++)
            {
                // for each pixel
                for (int x = 0; x < width; x++, src += pixelSize, dst += pixelSize)
                {
                    pos = y * width + x;
                    hsv.Hue = aryH[pos];
                    hsv.Saturation = aryS[pos];
                    hsv.Value = aryV[pos];

                    HSV.ToRGB(hsv, rgb);

                    dst[RGB.R] = rgb.Red;
                    dst[RGB.G] = rgb.Green;
                    dst[RGB.B] = rgb.Blue;
                }
                src += offset;
                dst += offset;
            }

            image.UnlockBits(srcDat);
            dest.UnlockBits(dstDat);

            return dest;
        }

        /// <summary>
        /// Pseudocolor filter
        /// </summary>        
        public static unsafe Bitmap PseudoColorFilter(Bitmap image, Rectangle rect)
        {
            Bitmap dest = (Bitmap)image.Clone();
            BitmapData srcDat = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);
            BitmapData dstDat = dest.LockBits(rect, ImageLockMode.ReadWrite, dest.PixelFormat);

            int width = rect.Width;
            int height = rect.Height;
            int stride = srcDat.Stride;

            // get pixel size
            int pixelSize = (image.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;

            int offset = stride - width * pixelSize;

            // do the job
            byte* src = (byte*)srcDat.Scan0.ToPointer();
            byte* dst = (byte*)dstDat.Scan0.ToPointer();

            //Pseudo color table
            byte[] pR = new byte[256];
            byte[] pG = new byte[256];
            byte[] pB = new byte[256];

            int i;
            for (i = 0; i < 64; i++)
            {
                pR[i] = 0;
                pG[i] = (byte)(i * 4);
                pB[i] = 255;
            }
            for (; i < 128; i++)
            {
                pR[i] = 0;
                pG[i] = 255;
                pB[i] = (byte)(255 - i * 4);
            }
            for (; i < 192; i++)
            {
                pR[i] = (byte)(i * 4);
                pG[i] = 255;
                pB[i] = 0;
            }
            for (; i < 256; i++)
            {
                pR[i] = 255;
                pG[i] = (byte)(255 - i * 4);
                pB[i] = 0;
            }

            // for each row
            for (int y = 0; y < height; y++)
            {
                // for each pixel
                for (int x = 0; x < width; x++, src += pixelSize, dst += pixelSize)
                {
                    dst[RGB.R] = pR[src[RGB.R]];
                    dst[RGB.G] = pG[src[RGB.G]];
                    dst[RGB.B] = pB[src[RGB.B]];
                }
                src += offset;
                dst += offset;
            }

            image.UnlockBits(srcDat);
            dest.UnlockBits(dstDat);

            return dest;
        }

        /// <summary>
        /// Binarization by Disriminant Analysis Method
        /// </summary>
        /// <param name="image"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static unsafe Bitmap DiscriminantAnalysis(Bitmap image, Rectangle rect)
        {
            Bitmap dest = Binarization.Grayscale(image, rect);
            BitmapData bmd = dest.LockBits(rect, ImageLockMode.ReadWrite, dest.PixelFormat);

            int thresholdValue = 0;

            // get start and stop X-Y coordinates
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;
            int offset = bmd.Stride - rect.Width;

            // histogram array
            int[] hist = new int[256];

            int t, max_t, t_limit;
            int i, n1, n2, count, sum;
            long tmp;
            double ave;
            double ave1, ave2, var1, var2, max;
            double var_w, var_b, r;
            max_t = 0;
            t_limit = 255;

            // collect histogram first
            byte* ptr = (byte*)bmd.Scan0.ToPointer();

            // align pointer to the first pixel to process
            ptr += (startY * bmd.Stride + startX);

            // calculate histogram
            sum = count = 0;
            // for each line            
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                {
                    hist[*ptr]++;
                    sum += *ptr;
                    count++;
                }
                ptr += offset;
            }

            ave = sum / count;

            max = -1.0;
            for (t = 0; t < t_limit; t++)
            {
                n1 = n2 = 0;
                ave1 = ave2 = 0;
                var1 = var2 = 0;

                tmp = 0;
                for (i = 0; i < t; i++)
                {
                    n1 = n1 + hist[i];
                    tmp = tmp + hist[i] * i;
                }

                // Average of class 1
                if (n1 != 0) ave1 = (double)tmp / (double)n1;

                for (i = 0; i < t; i++)
                {
                    var1 = var1 + (i - ave1) * (i - ave1) * hist[i];
                }

                // Variance of class 1
                if (n1 != 0) var1 = var1 / (double)n1;

                tmp = 0;
                for (i = t; i < t_limit; i++)
                {
                    n2 = n2 + hist[i];
                    tmp = tmp + hist[i] * i;
                }

                // Average of class 2
                if (n2 != 0) ave2 = (double)tmp / (double)n2;

                for (i = t; i < t_limit; i++)
                {
                    var2 = var2 + (i - ave2) * (i - ave2) * hist[i];
                }
                //	Variance of class 2
                if (n2 != 0) var2 = var2 / (double)n2;

                var_w = (n1 * var1 + n2 * var2);
                if (var_w == 0) var_w = 1;
                var_b = n1 * (ave1 - ave) * (ave1 - ave) + n2 * (ave2 - ave) * (ave2 - ave);
                r = var_b / var_w;
                if (r > max)
                {
                    max = r;
                    max_t = t;
                }
            }

            if (max_t < 0 || max_t > 255) return dest;
            else thresholdValue = max_t;

            // do the job
            ptr = (byte*)bmd.Scan0;

            // align pointer to the first pixel to process
            ptr += (startY * bmd.Stride + startX);

            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                {
                    *ptr = (byte)((*ptr >= thresholdValue) ? 255 : 0);
                }
                ptr += offset;
            }

            dest.UnlockBits(bmd);

            return dest;
        }

        /// <summary>
        /// Selection based on pixel similarity in RGB color space
        /// </summary>        
        public static unsafe Bitmap Similiar_RGBAver(Bitmap image, Rectangle rect, List<RGB> rgbs, int similarity)
        {
            Bitmap dest = (Bitmap)image.Clone();
            if (rgbs.Count == 0) return dest;

            BitmapData srcDat = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);
            BitmapData dstDat = dest.LockBits(rect, ImageLockMode.ReadWrite, dest.PixelFormat);

            int width = rect.Width;
            int height = rect.Height;
            int stride = srcDat.Stride;

            // get pixel size
            int pixelSize = (image.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;

            int offset = stride - width * pixelSize;

            // do the job
            byte* src = (byte*)srcDat.Scan0.ToPointer();
            byte* dst = (byte*)dstDat.Scan0.ToPointer();

            int sumR, sumG, sumB;
            int averR, averG, averB;
            int minR, minG, minB;
            int maxR, maxG, maxB;            
            sumR = sumG = sumB = 0;            
            foreach (RGB rgb in rgbs)
            {
                sumR += rgb.Red;
                sumG += rgb.Green;
                sumB += rgb.Blue;
            }
            averR = sumR / rgbs.Count;
            averG = sumG / rgbs.Count;
            averB = sumB / rgbs.Count;

            minR = Math.Max(averR - similarity / 2, 0);
            minG = Math.Max(averG - similarity / 2, 0);
            minB = Math.Max(averB - similarity / 2, 0);

            maxR = Math.Min(averR + similarity / 2, 255);
            maxG = Math.Min(averG + similarity / 2, 255);
            maxB = Math.Min(averB + similarity / 2, 255);
            
            // for each row
            for (int y = 0; y < height; y++)
            {
                // for each pixel
                for (int x = 0; x < width; x++, src += pixelSize, dst += pixelSize)
                {
                    if (src[RGB.R] >= minR && src[RGB.R] <= maxR &&
                        src[RGB.G] >= minG && src[RGB.G] <= maxG &&
                        src[RGB.B] >= minB && src[RGB.B] <= maxB)
                    {
                        dst[RGB.R] = dst[RGB.G] = dst[RGB.B] = 0;
                    }
                    else
                    {
                        dst[RGB.R] = dst[RGB.G] = dst[RGB.B] = 255;
                    }
                }
                src += offset;
                dst += offset;
            }

            image.UnlockBits(srcDat);
            dest.UnlockBits(dstDat);

            return dest;
        }

        /// <summary>
        /// Selection based on pixel similarity in RGB color space
        /// </summary>        
        public static unsafe Bitmap Similiar_RGB(Bitmap image, Rectangle rect, List<RGB> rgbs, int similarity)
        {
            Bitmap dest = (Bitmap)image.Clone();
            if (rgbs.Count == 0) return dest;
            
            BitmapData srcDat = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);
            BitmapData dstDat = dest.LockBits(rect, ImageLockMode.ReadWrite, dest.PixelFormat);

            int width = rect.Width;
            int height = rect.Height;
            int stride = srcDat.Stride;

            // get pixel size
            int pixelSize = (image.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;

            int offset = stride - width * pixelSize;

            // do the job
            byte* src = (byte*)srcDat.Scan0.ToPointer();
            byte* dst = (byte*)dstDat.Scan0.ToPointer();

            int[] table_mix = new int[256 * 3];
            for (int i = 0; i < 256 * 3; i++) table_mix[i] = -1;

            int total_c;
            foreach (RGB rgb in rgbs)
            {
                total_c = rgb.Red + rgb.Green + rgb.Blue;

                for (int i = 0; i < similarity / 2; i++)
                {
                    if ((total_c + i) < 256 * 3)
                    {
                        table_mix[total_c + i] = 1;
                    }
                    if ((total_c - i) >= 0)
                    {
                        table_mix[total_c - i] = 1;
                    }
                }
            }

            int koba;
            // for each row
            for (int y = 0; y < height; y++)
            {
                // for each pixel
                for (int x = 0; x < width; x++, src += pixelSize, dst += pixelSize)
                {
                    koba = src[RGB.R] + src[RGB.G] + src[RGB.B];

                    if (table_mix[koba] == 1)
                    {
                        dst[RGB.R] = dst[RGB.G] = dst[RGB.B] = 0;
                    }
                    else
                    {
                        dst[RGB.R] = dst[RGB.G] = dst[RGB.B] = 255;
                    }
                }
                src += offset;
                dst += offset;
            }

            image.UnlockBits(srcDat);
            dest.UnlockBits(dstDat);

            return dest;
        }

        /// <summary>
        /// Selection based on pixel similarity in LAB color space
        /// </summary>        
        public static unsafe Bitmap Similiar_LABAver(Bitmap image, Rectangle rect, List<LAB> labs, int similarity)
        {
            Bitmap dest = (Bitmap)image.Clone();
            if (labs.Count == 0) return dest;
            
            BitmapData srcDat = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);
            BitmapData dstDat = dest.LockBits(rect, ImageLockMode.ReadWrite, dest.PixelFormat);

            int width = rect.Width;
            int height = rect.Height;
            int stride = srcDat.Stride;

            // get pixel size
            int pixelSize = (image.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;

            int offset = stride - width * pixelSize;

            // do the job
            byte* src = (byte*)srcDat.Scan0.ToPointer();
            byte* dst = (byte*)dstDat.Scan0.ToPointer();

            //L = 0～255
            //A = -255～255
            //B = -255～255
            int sumL, sumA, sumB;
            int averL, averA, averB;
            int minL, minA, minB;
            int maxL, maxA, maxB;
            sumL = sumA = sumB = 0;
            foreach (LAB lab in labs)
            {
                sumL += lab.Lightness;
                sumA += lab.A;
                sumB += lab.B;
            }
            averL = sumL / labs.Count;
            averA = sumA / labs.Count;
            averB = sumB / labs.Count;

            minL = Math.Max(averL - similarity / 2, 0);
            minA = Math.Max(averA - similarity / 2, -255);
            minB = Math.Max(averB - similarity / 2, -255);

            maxL = Math.Min(averL + similarity / 2, 255);
            maxA = Math.Min(averA + similarity / 2, 255);
            maxB = Math.Min(averB + similarity / 2, 255);            

            // for each row
            for (int y = 0; y < height; y++)
            {
                // for each pixel
                for (int x = 0; x < width; x++, src += pixelSize, dst += pixelSize)
                {
                    LAB lab = LAB.FromRGB(new RGB(src[RGB.R], src[RGB.G], src[RGB.B]));

                    if (lab.Lightness >= minL && lab.Lightness <= maxL &&
                        lab.A >= minA && lab.A <= maxA &&
                        lab.B >= minB && lab.B <= maxB)
                    {
                        dst[RGB.R] = dst[RGB.G] = dst[RGB.B] = 0;
                    }
                    else
                    {
                        dst[RGB.R] = dst[RGB.G] = dst[RGB.B] = 255;
                    }
                }
                src += offset;
                dst += offset;
            }

            image.UnlockBits(srcDat);
            dest.UnlockBits(dstDat);

            return dest;
        }

        /// <summary>
        /// Selection based on pixel similarity in LAB color space
        /// </summary>        
        public static unsafe Bitmap Similiar_LAB(Bitmap image, Rectangle rect, List<LAB> labs, int similarity)
        {
            Bitmap dest = (Bitmap)image.Clone();
            if (labs.Count == 0) return dest;
            
            BitmapData srcDat = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);
            BitmapData dstDat = dest.LockBits(rect, ImageLockMode.ReadWrite, dest.PixelFormat);

            int width = rect.Width;
            int height = rect.Height;
            int stride = srcDat.Stride;

            // get pixel size
            int pixelSize = (image.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;

            int offset = stride - width * pixelSize;

            // do the job
            byte* src = (byte*)srcDat.Scan0.ToPointer();
            byte* dst = (byte*)dstDat.Scan0.ToPointer();

            //L = 0～255
            //A = -255～255 = 0～511
            //B = -255～255 = 0～511            
            const int LAB_num = 256 + 512 + 512;
            int[] table_mix = new int[LAB_num];
            for (int i = 0; i < LAB_num; i++) table_mix[i] = -1;

            int total_l;
            foreach (LAB lab in labs)
            {
                total_l = (int)(lab.Lightness + lab.A + lab.B + 510);

                for (int i = 0; i < similarity / 2; i++)
                {
                    if ((total_l + i) < LAB_num) table_mix[total_l + i] = 1;
                    if ((total_l - i) >= 0) table_mix[total_l - i] = 1;
                }
            }

            int koba;
            // for each row
            for (int y = 0; y < height; y++)
            {
                // for each pixel
                for (int x = 0; x < width; x++, src += pixelSize, dst += pixelSize)
                {
                    LAB lab = LAB.FromRGB(new RGB(src[RGB.R], src[RGB.G], src[RGB.B]));
                    koba = (int)(lab.Lightness + lab.A + lab.B + 510);

                    if (table_mix[koba] == 1)
                    {
                        dst[RGB.R] = dst[RGB.G] = dst[RGB.B] = 0;
                    }
                    else
                    {
                        dst[RGB.R] = dst[RGB.G] = dst[RGB.B] = 255;
                    }
                }
                src += offset;
                dst += offset;
            }

            image.UnlockBits(srcDat);
            dest.UnlockBits(dstDat);

            return dest;
        }

        /// <summary>
        /// Selection based on pixel similarity in HSV color space
        /// </summary>        
        public static unsafe Bitmap Similiar_HSV(Bitmap image, Rectangle rect, List<HSV> hsvs, int similarity)
        {
            Bitmap dest = (Bitmap)image.Clone();
            if (hsvs.Count == 0) return dest;
            
            BitmapData srcDat = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);
            BitmapData dstDat = dest.LockBits(rect, ImageLockMode.ReadWrite, dest.PixelFormat);

            int width = rect.Width;
            int height = rect.Height;
            int stride = srcDat.Stride;

            // get pixel size
            int pixelSize = (image.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;

            int offset = stride - width * pixelSize;

            // do the job
            byte* src = (byte*)srcDat.Scan0.ToPointer();
            byte* dst = (byte*)dstDat.Scan0.ToPointer();

            //H = 0 ～ 3066
            //S = 0 ～ 511
            //V = 0 ～ 511         
            const int HSV_num = 3067 + 512 + 512;
            int[] table_mix = new int[HSV_num];
            for (int i = 0; i < HSV_num; i++) table_mix[i] = -1;

            int total_l;
            foreach (HSV hsv in hsvs)
            {
                total_l = hsv.Hue + hsv.Saturation + hsv.Value;

                for (int i = 0; i < similarity / 2; i++)
                {
                    if ((total_l + i) < HSV_num) table_mix[total_l + i] = 1;
                    if ((total_l - i) >= 0) table_mix[total_l - i] = 1;
                }
            }

            int koba;
            // for each row
            for (int y = 0; y < height; y++)
            {
                // for each pixel
                for (int x = 0; x < width; x++, src += pixelSize, dst += pixelSize)
                {
                    HSV hsv = HSV.FromRGB(new RGB(src[RGB.R], src[RGB.G], src[RGB.B]));
                    koba = hsv.Hue + hsv.Saturation + hsv.Value;

                    if (table_mix[koba] == 1)
                    {
                        dst[RGB.R] = dst[RGB.G] = dst[RGB.B] = 0;
                    }
                    else
                    {
                        dst[RGB.R] = dst[RGB.G] = dst[RGB.B] = 255;
                    }
                }
                src += offset;
                dst += offset;
            }

            image.UnlockBits(srcDat);
            dest.UnlockBits(dstDat);

            return dest;
        }
    }
}
