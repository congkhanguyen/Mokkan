using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace MokkAnnotator.MkaImageProcess
{
    class Binarization
    {
        public static bool ExtractBackground = false;

        /// <summary>
        /// Get grayscale image 
        /// </summary>
        /// <param name="image">bitmap image</param>
        /// <param name="rect">boundaries of image</param>
        /// <returns>grayscale image</returns>     
        public static unsafe Bitmap Grayscale(Bitmap image, Rectangle rect)
        {
            //// BT709 algorithm
            //double RedCoefficient = 0.2125;
            //double GreenCoefficient = 0.7154;
            //double BlueCoefficient = 0.0721;

            // R-Y algorithm
            double RedCoefficient = 0.5000;
            double GreenCoefficient = 0.4190;
            double BlueCoefficient = 0.0810;

            //// Y algorithm
            //double RedCoefficient = 0.2990;
            //double GreenCoefficient = 0.5870;
            //double BlueCoefficient = 0.1140;

            // already grayscale
            if (image.PixelFormat == PixelFormat.Format8bppIndexed)
                return (Bitmap)image.Clone();

            Bitmap source = image;
            Bitmap dest = new Bitmap(image.Width, image.Height, PixelFormat.Format8bppIndexed);
            BitmapData sourceData = source.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, source.PixelFormat);
            BitmapData destData = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);

            // get start and stop X-Y coordinates
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;

            // configure palette
            ColorPalette cp = dest.Palette;
            // init palette
            for (int i = 0; i < 256; i++)
                cp.Entries[i] = Color.FromArgb(i, i, i);

            // set palette back
            dest.Palette = cp;


            if ((sourceData.PixelFormat == PixelFormat.Format24bppRgb) ||
                (sourceData.PixelFormat == PixelFormat.Format32bppRgb) ||
                (sourceData.PixelFormat == PixelFormat.Format32bppArgb))
            {
                int pixelSize = (sourceData.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;
                int srcOffset = sourceData.Stride - rect.Width * pixelSize;
                int dstOffset = destData.Stride - rect.Width;

                // do the job
                byte* src = (byte*)sourceData.Scan0 + startY * sourceData.Stride + startX * pixelSize;
                byte* dst = (byte*)destData.Scan0 + startY * destData.Stride + startX;

                // for each line
                for (int y = startY; y < stopY; y++)
                {
                    // for each pixel
                    for (int x = startX; x < stopX; x++, src += pixelSize, dst++)
                        *dst = (byte)(RedCoefficient * src[2] + GreenCoefficient * src[1] + BlueCoefficient * src[0]);

                    src += srcOffset;
                    dst += dstOffset;
                }
            }
            else
            {
                int pixelSize = (sourceData.PixelFormat == PixelFormat.Format48bppRgb) ? 3 : 4;
                int srcBase = (int)sourceData.Scan0;
                int dstBase = (int)destData.Scan0;

                int srcStride = sourceData.Stride;
                int dstStride = destData.Stride;

                // for each line
                for (int y = startY; y < stopY; y++)
                {
                    ushort* src = (ushort*)(srcBase + y * srcStride + startX * pixelSize);
                    ushort* dst = (ushort*)(dstBase + y * dstStride + startX);

                    // for each pixel
                    for (int x = startX; x < stopX; x++, src += pixelSize, dst++)
                        *dst = (ushort)(RedCoefficient * src[2] + GreenCoefficient * src[1] + BlueCoefficient * src[0]);
                }
            }

            int width = image.Width;
            int height = image.Height;
            int stride = destData.Stride;
            byte* ptr;

            // remove upper part            
            for (int y = 0; y < startY; y++)
            {
                ptr = (byte*)destData.Scan0.ToPointer() + y * stride;
                SystemTools.SetUnmanagedMemory(ptr, 255, stride);
            }

            // remove lower part
            for (int y = stopY; y < height; y++)
            {
                ptr = (byte*)destData.Scan0.ToPointer() + y * stride;
                SystemTools.SetUnmanagedMemory(ptr, 255, stride);
            }

            for (int y = startY; y < stopY; y++)
            {
                // remove left part
                ptr = (byte*)destData.Scan0.ToPointer() + y * stride;
                SystemTools.SetUnmanagedMemory(ptr, 255, startX);

                // remove right part
                ptr += stopX;
                SystemTools.SetUnmanagedMemory(ptr, 255, stride - stopX);
            }

            source.UnlockBits(sourceData);
            dest.UnlockBits(destData);

            return dest;
        }

        /// <summary>
        /// Binarized image (global method)
        /// </summary>
        public static unsafe Bitmap GlobalThreshold(Bitmap image, Rectangle rect, int thresholdValue)
        {
            if (ExtractBackground)
                thresholdValue = Math.Min(200, thresholdValue + 30);

            Bitmap dest = (Bitmap)image.Clone();
            BitmapData bmd = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);

            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;
            int offset = bmd.Stride - rect.Width;

            // do the job
            byte* ptr = (byte*)bmd.Scan0;

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
        /// Threshold binarization.
        /// </summary>
        /// 
        /// <remarks><para>The filter does image binarization using specified threshold value. All pixels
        /// with intensities equal or higher than threshold value are converted to white pixels. All other
        /// pixels with intensities below threshold value are converted to black pixels.</para>
        /// 
        public static unsafe Bitmap Threshold(Bitmap image, Rectangle rect, int thresholdValue)
        {
            Bitmap dest = Grayscale(image, rect);

            return GlobalThreshold(dest, rect, thresholdValue);
        }

        /// <summary>
        /// Otsu thresholding.
        /// </summary>
        /// 
        /// <remarks><para>The class implements Otsu thresholding, which is described in
        /// <b>N. Otsu, "A threshold selection method from gray-level histograms", IEEE Trans. Systems,
        /// Man and Cybernetics 9(1), pp. 62・6, 1979.</b></para>
        /// 
        /// <para>This implementation instead of minimizing the weighted within-class variance
        /// does maximization of between-class variance, what gives the same result. The approach is
        /// described in <a href="http://sampl.ece.ohio-state.edu/EE863/2004/ECE863-G-segclust2.ppt">this presentation</a>.</para>
        /// 
        public static unsafe Bitmap OtsuThreshold(Bitmap image, Rectangle rect)
        {
            Bitmap dest = Grayscale(image, rect);
            BitmapData bmd = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);

            // get start and stop X-Y coordinates
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;
            int offset = bmd.Stride - rect.Width;
            int thresholdValue = 128;

            // histogram array
            int[] integerHistogram = new int[256];
            double[] histogram = new double[256];

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
                        integerHistogram[*ptr]++;
                    }
                    ptr += offset;
                }

                // pixels count in the processing region
                int pixelCount = (stopX - startX) * (stopY - startY);
                // mean value of the processing region
                double imageMean = 0;
                for (int i = 0; i < 256; i++)
                {
                    histogram[i] = (double)integerHistogram[i] / pixelCount;
                    imageMean += histogram[i] * i;
                }

                double max = double.MinValue;

                // initial class probabilities
                double class1ProbabiltyInit = 0;
                double class2ProbabiltyInit = 1;

                // initial class 1 mean value
                double class1MeanInit = 0;

                // check all thresholds
                for (int t = 0; t < 256; t++)
                {
                    // calculate class probabilities for the given threshold
                    double class1Probability = class1ProbabiltyInit;
                    double class2Probability = class2ProbabiltyInit;

                    // calculate class means for the given threshold
                    double class1Mean = class1MeanInit;
                    double class2Mean = (imageMean - (class1Mean * class1Probability)) / class2Probability;

                    // calculate between class variance
                    double betweenClassVariance = (class1Probability) * (1.0 - class1Probability) * Math.Pow(class1Mean - class2Mean, 2);

                    // check if we found new threshold candidate
                    if (betweenClassVariance > max)
                    {
                        max = betweenClassVariance;
                        thresholdValue = t;
                    }

                    // update initial probabilities and mean value
                    class1MeanInit *= class1ProbabiltyInit;

                    class1ProbabiltyInit += histogram[t];
                    class2ProbabiltyInit -= histogram[t];

                    class1MeanInit += (double)t * (double)histogram[t];

                    if (class1ProbabiltyInit != 0)
                        class1MeanInit /= class1ProbabiltyInit;
                }
            }

            dest.UnlockBits(bmd);

            return GlobalThreshold(dest, rect, thresholdValue);
        }

        /// <summary>
        /// Threshold using Simple Image Statistics (SIS).
        /// </summary>
        /// 
        /// <remarks><para>The filter performs image thresholding calculating threshold automatically
        /// using simple image statistics method. For each pixel:
        /// <list type="bullet">
        /// <item>two gradients are calculated - ex = |I(x + 1, y) - I(x - 1, y)| and
        /// |I(x, y + 1) - I(x, y - 1)|;</item>
        /// <item>weight is calculated as maximum of two gradients;</item>
        /// <item>sum of weights is updated (weightTotal += weight);</item>
        /// <item>sum of weighted pixel values is updated (total += weight * I(x, y)).</item>
        /// </list>
        /// The result threshold is calculated as sum of weighted pixel values divided by sum of weight.</para>
        /// 
        public static unsafe Bitmap SISThreshold(Bitmap image, Rectangle rect)
        {
            Bitmap dest = Grayscale(image, rect);
            BitmapData bmd = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);

            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;
            int stopXM1 = stopX - 1;
            int stopYM1 = stopY - 1;
            int stride = bmd.Stride;
            int offset = stride - rect.Width;

            // differences and weights
            double ex, ey, weight, weightTotal = 0, total = 0;

            unsafe
            {
                // do the job
                byte* ptr = (byte*)bmd.Scan0;

                // align pointer to the first pixel to process
                ptr += (startY * bmd.Stride + startX);

                // skip the first line for the first pass
                ptr += stride;

                // for each line
                for (int y = startY + 1; y < stopYM1; y++)
                {
                    ptr++;
                    // for each pixels
                    for (int x = startX + 1; x < stopXM1; x++, ptr++)
                    {
                        // the equations are:
                        // ex = | I(x + 1, y) - I(x - 1, y) |
                        // ey = | I(x, y + 1) - I(x, y - 1) |
                        // weight = max(ex, ey)
                        // weightTotal += weight
                        // total += weight * I(x, y)

                        ex = Math.Abs(ptr[1] - ptr[-1]);
                        ey = Math.Abs(ptr[stride] - ptr[-stride]);
                        weight = (ex > ey) ? ex : ey;
                        weightTotal += weight;
                        total += weight * (*ptr);
                    }
                    ptr += offset + 1;
                }
            }

            // calculate threshold
            int thresholdValue = (weightTotal == 0) ? (byte)0 : (byte)(total / weightTotal);

            dest.UnlockBits(bmd);

            return GlobalThreshold(dest, rect, thresholdValue);
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
        public static unsafe Bitmap MaxEntropyThreshold(Bitmap image, Rectangle rect)
        {
            Bitmap dest = Grayscale(image, rect);
            BitmapData bmd = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);

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
            dest.UnlockBits(bmd);

            return GlobalThreshold(dest, rect, thresholdValue);
        }

        /// <summary>
        /// Iterative threshold search and binarization.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>The algorithm works in the following way:
        /// <list type="bullet">
        /// <item>select any start threshold;</item>
        /// <item>compute average value of Background (ｵB) and Object (ｵO) values:
        /// 1) all pixels with a value that is below threshold, belong to the Background values;
        ///	2) all pixels greater or equal threshold, belong to the Object values.
        /// </item>
        ///	<item>calculate new thresghold: (ｵB + ｵO) / 2;</item>
        /// <item>if |oldThreshold - newThreshold| is less than a given minimum allowed error, then stop iteration process
        /// and create the binary image with the new threshold.</item>
        /// </list>
        /// </para>
        /// 
        /// <para>For additional information see <b>Digital Image Processing, Gonzalez/Woods. Ch.10 page:599</b>.</para>
        /// 
        public static unsafe Bitmap IteractiveThreshold(Bitmap image, Rectangle rect, int minError, int threshold)
        {
            Bitmap dest = Grayscale(image, rect);
            BitmapData bmd = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);

            int thresholdValue = threshold;

            // get start and stop X-Y coordinates
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;

            // histogram array
            int[] integerHistogram = null;
            int maxThreshold = 0;

            unsafe
            {
                integerHistogram = new int[256];
                maxThreshold = 256;

                // collect histogram first
                byte* ptr = (byte*)bmd.Scan0.ToPointer();
                int offset = bmd.Stride - rect.Width;

                // align pointer to the first pixel to process
                ptr += (startY * bmd.Stride + startX);

                // for each line	
                for (int y = startY; y < stopY; y++)
                {
                    // for each pixel
                    for (int x = startX; x < stopX; x++, ptr++)
                    {
                        integerHistogram[*ptr]++;
                    }
                    ptr += offset;
                }
            }

            // old threshold value
            int oldThreshold = 0;

            do
            {
                oldThreshold = thresholdValue;

                // object's mean and amount of object's pixels
                double meanObject = 0;
                int objectPixels = 0;

                // background's mean and amount of background's pixels
                double meanBackground = 0;
                int backgroundPixels = 0;

                for (int t = 0; t < thresholdValue; t++)
                {
                    meanBackground += (double)t * integerHistogram[t];
                    backgroundPixels += integerHistogram[t];
                }
                // calculate object pixels
                for (int t = thresholdValue; t < maxThreshold; t++)
                {
                    meanObject += (double)t * integerHistogram[t];
                    objectPixels += integerHistogram[t];
                }
                meanBackground /= backgroundPixels;
                meanObject /= objectPixels;

                // calculate new threshold value
                if (backgroundPixels == 0)
                {
                    thresholdValue = (int)meanObject;
                }
                else if (objectPixels == 0)
                {
                    thresholdValue = (int)meanBackground;
                }
                else
                {
                    thresholdValue = (int)((meanBackground + meanObject) / 2);
                }
            }
            while (Math.Abs(oldThreshold - thresholdValue) > minError);

            dest.UnlockBits(bmd);

            return GlobalThreshold(dest, rect, thresholdValue);
        }

        /// <summary>
        /// Threshold using Minimum Cross Entropy (proposed by Li).
        /// </summary>    
        /// 
        /// <remarks>
        /// <para>This implementation is based on the iterative version (Ref. 2) of the algorithm.
        /// <b>1) Li C.H. and Lee C.K. (1993) "Minimum Cross Entropy Thresholding"  
        /// Pattern Recognition, 26(4): 617-625 </b></para>
        /// 
        /// <para>2) Li C.H. and Tam P.K.S. (1998) "An Iterative Algorithm for Minimum  Cross Entropy Thresholding" 
        /// Pattern Recognition Letters, 18(8): 771-776 </b></para>
        ///
        /// <para>3) Sezgin M. and Sankur B. (2004) "Survey over Image Thresholding Techniques and Quantitative Performance Evaluation"
        /// Journal of  Electronic Imaging, 13(1): 146-165 </b></para>
        /// </remarks>       
        public static unsafe Bitmap LiThreshold(Bitmap image, Rectangle rect)
        {
            Bitmap dest = Grayscale(image, rect);
            BitmapData bmd = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);

            int thresholdValue = 0;

            // get start and stop X-Y coordinates
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;
            int offset = bmd.Stride - rect.Width;

            int i;
            int num_pixels;
            int sum_back; /* sum of the background pixels at a given threshold */
            int sum_obj;  /* sum of the object pixels at a given threshold */
            int num_back; /* number of background pixels at a given threshold */
            int num_obj;  /* number of object pixels at a given threshold */
            double old_thresh;
            double new_thresh;
            double mean_back; /* mean of the background pixels at a given threshold */
            double mean_obj;  /* mean of the object pixels at a given threshold */
            double mean;  /* mean gray-level in the image */
            double tolerance; /* threshold tolerance */
            double temp;

            // histogram array
            int[] histogram = new int[256];

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

                tolerance = 0.5;
                num_pixels = 0;
                for (i = 0; i < 256; i++)
                    num_pixels += histogram[i];

                /* Calculate the mean gray-level */
                mean = 0.0;
                for (i = 0 + 1; i < 256; i++) //0 + 1?
                    mean += i * histogram[i];
                mean /= num_pixels;
                /* Initial estimate */
                new_thresh = mean;

                do
                {
                    old_thresh = new_thresh;
                    thresholdValue = (int)(old_thresh + 0.5);	/* range */
                    /* Calculate the means of background and object pixels */
                    /* Background */
                    sum_back = 0;
                    num_back = 0;
                    for (i = 0; i <= thresholdValue; i++)
                    {
                        sum_back += i * histogram[i];
                        num_back += histogram[i];
                    }
                    mean_back = (num_back == 0 ? 0.0 : (sum_back / (double)num_back));
                    /* Object */
                    sum_obj = 0;
                    num_obj = 0;
                    for (i = thresholdValue + 1; i < 256; i++)
                    {
                        sum_obj += i * histogram[i];
                        num_obj += histogram[i];
                    }
                    mean_obj = (num_obj == 0 ? 0.0 : (sum_obj / (double)num_obj));

                    /* Calculate the new threshold: Equation (7) in Ref. 2 */
                    //new_thresh = simple_round ( ( mean_back - mean_obj ) / ( Math.log ( mean_back ) - Math.log ( mean_obj ) ) );
                    //simple_round ( double x ) {
                    // return ( int ) ( IS_NEG ( x ) ? x - .5 : x + .5 );
                    //}
                    //
                    //#define IS_NEG( x ) ( ( x ) < -DBL_EPSILON ) 
                    //DBL_EPSILON = 2.220446049250313E-16
                    temp = (mean_back - mean_obj) / (Math.Log(mean_back) - Math.Log(mean_obj));

                    if (temp < -2.220446049250313E-16)
                        new_thresh = (int)(temp - 0.5);
                    else
                        new_thresh = (int)(temp + 0.5);
                    /*  Stop the iterations when the difference between the
                    new and old threshold values is less than the tolerance */
                } while (Math.Abs(new_thresh - old_thresh) > tolerance);
            }

            dest.UnlockBits(bmd);

            return GlobalThreshold(dest, rect, thresholdValue);
        }

        /// <summary>
        /// Threshold using Huang's fuzzy method.
        /// </summary>    
        /// 
        /// <remarks>
        /// <para>Uses Shannon's entropy function (one can also use Yager's entropy function) 
        /// <b>Huang L.-K. and Wang M.-J.J. (1995) "Image Thresholding by Minimizing the Measures of Fuzziness"
        /// Pattern Recognition, 28(1): 41-51 </b></para>
        /// </remarks>
        ///      
        public static unsafe Bitmap HuangThreshold(Bitmap image, Rectangle rect)
        {
            Bitmap dest = Grayscale(image, rect);
            BitmapData bmd = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);

            int thresholdValue = 0;

            // get start and stop X-Y coordinates
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;
            int offset = bmd.Stride - rect.Width;

            int ih, it;
            int first_bin;
            int last_bin;
            int sum_pix;
            int num_pix;
            double term;
            double ent;  // entropy 
            double min_ent; // min entropy 
            double mu_x;

            // histogram array
            int[] histogram = new int[256];

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

                /* Determine the first non-zero bin */
                first_bin = 0;
                for (ih = 0; ih < 256; ih++)
                {
                    if (histogram[ih] != 0)
                    {
                        first_bin = ih;
                        break;
                    }
                }

                /* Determine the last non-zero bin */
                last_bin = 255;
                for (ih = 255; ih >= first_bin; ih--)
                {
                    if (histogram[ih] != 0)
                    {
                        last_bin = ih;
                        break;
                    }
                }
                term = 1.0 / (double)(last_bin - first_bin);
                double[] mu_0 = new double[256];
                sum_pix = num_pix = 0;
                for (ih = first_bin; ih < 256; ih++)
                {
                    sum_pix += ih * histogram[ih];
                    num_pix += histogram[ih];
                    /* NUM_PIX cannot be zero ! */
                    mu_0[ih] = sum_pix / (double)num_pix;
                }

                double[] mu_1 = new double[256];
                sum_pix = num_pix = 0;
                for (ih = last_bin; ih > 0; ih--)
                {
                    sum_pix += ih * histogram[ih];
                    num_pix += histogram[ih];
                    /* NUM_PIX cannot be zero ! */
                    mu_1[ih - 1] = sum_pix / (double)num_pix;
                }

                /* Determine the threshold that minimizes the fuzzy entropy */
                thresholdValue = -1;
                min_ent = Double.MaxValue;
                for (it = 0; it < 256; it++)
                {
                    ent = 0.0;
                    for (ih = 0; ih <= it; ih++)
                    {
                        /* Equation (4) in Ref. 1 */
                        mu_x = 1.0 / (1.0 + term * Math.Abs(ih - mu_0[it]));
                        if (!((mu_x < 1e-06) || (mu_x > 0.999999)))
                        {
                            /* Equation (6) & (8) in Ref. 1 */
                            ent += histogram[ih] * (-mu_x * Math.Log(mu_x) - (1.0 - mu_x) * Math.Log(1.0 - mu_x));
                        }
                    }

                    for (ih = it + 1; ih < 256; ih++)
                    {
                        /* Equation (4) in Ref. 1 */
                        mu_x = 1.0 / (1.0 + term * Math.Abs(ih - mu_1[it]));
                        if (!((mu_x < 1e-06) || (mu_x > 0.999999)))
                        {
                            /* Equation (6) & (8) in Ref. 1 */
                            ent += histogram[ih] * (-mu_x * Math.Log(mu_x) - (1.0 - mu_x) * Math.Log(1.0 - mu_x));
                        }
                    }
                    /* No need to divide by NUM_ROWS * NUM_COLS * LOG(2) ! */
                    if (ent < min_ent)
                    {
                        min_ent = ent;
                        thresholdValue = it;
                    }
                }
            }

            dest.UnlockBits(bmd);

            return GlobalThreshold(dest, rect, thresholdValue);
        }

        /// <summary>
        /// Threshold using using an iterative selection method.
        /// </summary>    
        /// 
        /// <remarks>
        /// <para>Iterative procedure based on the isodata algorithm of: 
        /// Ridler TW, Calvard S. (1978) "Picture thresholding using an iterative selection method" 
        /// IEEE Trans. System, Man and Cybernetics, SMC-8: 630-632.</para>
        /// 
        /// <para>The procedure divides the image into objects and background by taking an initial threshold,
        /// then the averages of the pixels at or below the threshold and pixels above are computed. 
        /// The averages of those two values are computed, the threshold is incremented and the process is repeated 
        /// until the threshold is larger than the composite average. 
        /// That is, threshold = (average background + average objects)/2. </para>
        /// 
        /// <para>A description of the method, posted to sci.image.processing on 1996/06/24 by Tim Morris:
        ///Subject: Re: Thresholding method?
        ///The algorithm implemented in NIH Image sets the threshold as 
        ///that grey value, G, for which the average of the averages of
        ///the grey values below and above G is equal to G. It does this
        ///by initialising G to the lowest sensible value and iterating:
        ///L = the average grey value of pixels with intensities <=G
        ///H = the average grey value of pixels with intensities > G
        ///is G = (L + H)/2?
        ///yes: exit
        ///no: increment G and repeat
        /// </para>
        /// </remarks>
        public static unsafe Bitmap IsoDataThreshold(Bitmap image, Rectangle rect)
        {
            Bitmap dest = Grayscale(image, rect);
            BitmapData bmd = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);

            // get start and stop X-Y coordinates
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;
            int offset = bmd.Stride - rect.Width;

            int thresholdValue = 0;

            // histogram array
            int[] histogram = new int[256];

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

                int i, l, toth, totl, h;
                for (i = 1; i < 256; i++)
                {
                    if (histogram[i] > 0)
                    {
                        thresholdValue = i + 1;
                        break;
                    }
                }
                while (true)
                {
                    l = 0;
                    totl = 0;
                    for (i = 0; i < thresholdValue; i++)
                    {
                        totl = totl + histogram[i];
                        l = l + (histogram[i] * i);
                    }
                    h = 0;
                    toth = 0;
                    for (i = thresholdValue + 1; i < 256; i++)
                    {
                        toth += histogram[i];
                        h += (histogram[i] * i);
                    }
                    if (totl > 0 && toth > 0)
                    {
                        l /= totl;
                        h /= toth;
                        if (thresholdValue == (int)Math.Round((l + h) / 2.0))
                            break;
                    }
                    thresholdValue++;
                    if (thresholdValue > 254)
                    {
                        thresholdValue = 128;
                        break;
                    }
                }
            }

            dest.UnlockBits(bmd);

            return GlobalThreshold(dest, rect, thresholdValue);
        }

        /// <summary>
        /// Threshold using the mean of the greyscale data
        /// </summary>    
        /// 
        /// <remarks>
        /// <para>C. A. Glasbey, "An analysis of histogram-based thresholding algorithms,"
        /// <b>CVGIP: Graphical Models and Image Processing, vol. 55, pp. 532-537, 1993. </b></para>
        /// </remarks>
        public static unsafe Bitmap MeanThreshold(Bitmap image, Rectangle rect)
        {
            Bitmap dest = Grayscale(image, rect);
            BitmapData bmd = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);

            int thresholdValue = 0;

            // get start and stop X-Y coordinates
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;
            int offset = bmd.Stride - rect.Width;

            // histogram array
            int[] histogram = new int[256];

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

                thresholdValue = (int)Math.Floor(Statistics.Mean(histogram));
            }

            dest.UnlockBits(bmd);

            return GlobalThreshold(dest, rect, thresholdValue);
        }

        /// <summary>
        /// Implements Yen thresholding method
        /// </summary>
        /// 
        /// <remarks>
        /// <para>1) Yen J.C., Chang F.J., and Chang S. (1995) "A New Criterion for Automatic Multilevel Thresholding"
        /// IEEE Trans. on Image Processing, 4(3): 370-378</para>
        /// 
        /// <para>2) Sezgin M. and Sankur B. (2004) "Survey over Image Thresholding Techniques and Quantitative Performance Evaluation" 
        /// Journal of Electronic Imaging, 13(1): 146-165</para>
        /// </remarks>
        public static unsafe Bitmap YenThreshold(Bitmap image, Rectangle rect)
        {
            Bitmap dest = Grayscale(image, rect);
            BitmapData bmd = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);

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
            double crit;
            double max_crit;
            double[] norm_histo = new double[256]; /* normalized histogram */
            double[] P1 = new double[256]; /* cumulative normalized histogram */
            double[] P1_sq = new double[256];
            double[] P2_sq = new double[256];

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
                for (ih = 1; ih < 256; ih++)
                    P1[ih] = P1[ih - 1] + norm_histo[ih];

                P1_sq[0] = norm_histo[0] * norm_histo[0];
                for (ih = 1; ih < 256; ih++)
                    P1_sq[ih] = P1_sq[ih - 1] + norm_histo[ih] * norm_histo[ih];

                P2_sq[255] = 0.0;
                for (ih = 254; ih >= 0; ih--)
                    P2_sq[ih] = P2_sq[ih + 1] + norm_histo[ih + 1] * norm_histo[ih + 1];

                /* Find the threshold that maximizes the criterion */
                thresholdValue = -1;
                max_crit = Double.MinValue;
                for (it = 0; it < 256; it++)
                {
                    crit = -1.0 * ((P1_sq[it] * P2_sq[it]) > 0.0 ? Math.Log(P1_sq[it] * P2_sq[it]) : 0.0) + 2 * ((P1[it] * (1.0 - P1[it])) > 0.0 ? Math.Log(P1[it] * (1.0 - P1[it])) : 0.0);
                    if (crit > max_crit)
                    {
                        max_crit = crit;
                        thresholdValue = it;
                    }
                }
            }

            dest.UnlockBits(bmd);

            return GlobalThreshold(dest, rect, thresholdValue);
        }

        /// <summary>
        /// Implements Moment-Preserving method
        /// </summary>
        /// 
        /// <remarks>
        /// <para>W. Tsai, "Moment-preserving thresholding: a new approach"
        ///  Computer Vision, Graphics, and Image Processing, vol. 29, pp. 377-393, 1985.</para>
        /// 
        /// <para>
        /// http://sourceforge.net/projects/fourier-ipal
        /// http://www.lsus.edu/faculty/~ecelebi/fourier.htm
        ///</para>
        /// </remarks>
        public static unsafe Bitmap MomentsThreshold(Bitmap image, Rectangle rect)
        {
            Bitmap dest = Grayscale(image, rect);
            BitmapData bmd = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);

            int thresholdValue = 0;

            // get start and stop X-Y coordinates
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;
            int offset = bmd.Stride - rect.Width;

            // histogram array
            int[] histogram = new int[256];

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

                double total = 0;
                double m0 = 1.0, m1 = 0.0, m2 = 0.0, m3 = 0.0, sum = 0.0, p0 = 0.0;
                double cd, c0, c1, z0, z1;	/* auxiliary variables */
                double[] histo = new double[256];

                for (int i = 0; i < 256; i++)
                    total += histogram[i];

                for (int i = 0; i < 256; i++)
                    histo[i] = (double)(histogram[i] / total); //normalised histogram

                /* Calculate the first, second, and third order moments */
                for (int i = 0; i < 256; i++)
                {
                    m1 += i * histo[i];
                    m2 += i * i * histo[i];
                    m3 += i * i * i * histo[i];
                }
                /* 
                First 4 moments of the gray-level image should match the first 4 moments
                of the target binary image. This leads to 4 equalities whose solutions 
                are given in the Appendix of Ref. 1 
                */
                cd = m0 * m2 - m1 * m1;
                c0 = (-m2 * m2 + m1 * m3) / cd;
                c1 = (m0 * -m3 + m2 * m1) / cd;
                z0 = 0.5 * (-c1 - Math.Sqrt(c1 * c1 - 4.0 * c0));
                z1 = 0.5 * (-c1 + Math.Sqrt(c1 * c1 - 4.0 * c0));
                p0 = (z1 - m1) / (z1 - z0);  /* Fraction of the object pixels in the target binary image */

                // The threshold is the gray-level closest  
                // to the p0-tile of the normalized histogram 
                sum = 0;
                for (int i = 0; i < 256; i++)
                {
                    sum += histo[i];
                    if (sum > p0)
                    {
                        thresholdValue = i;
                        break;
                    }
                }
            }

            dest.UnlockBits(bmd);

            return GlobalThreshold(dest, rect, thresholdValue);
        }

        /// <summary>
        /// Implements Percentile method
        /// </summary>
        /// 
        /// <remarks>
        /// <para>W. Doyle, "Operation useful for similarity-invariant pattern recognition,"
        ///  Journal of the Association for Computing Machinery, vol. 9,pp. 259-267, 1962.</para>
        /// 
        /// <para>Original Matlab code Copyright (C) 2004 Antti Niemisto
        /// See http://www.cs.tut.fi/~ant/histthresh/ for an excellent slide presentation and the original Matlab code.</para>
        /// </remarks>
        public static unsafe Bitmap PercentileThreshold(Bitmap image, Rectangle rect)
        {
            Bitmap dest = Grayscale(image, rect);
            BitmapData bmd = dest.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, dest.PixelFormat);

            int thresholdValue = 0;

            // get start and stop X-Y coordinates
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;
            int offset = bmd.Stride - rect.Width;

            // histogram array
            int[] histogram = new int[256];

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

                double ptile = 0.5; // default fraction of foreground pixels
                double[] avec = new double[256];

                for (int i = 0; i < 256; i++)
                    avec[i] = 0.0;

                double total = partialSum(histogram, 255);
                double temp = 1.0;
                for (int i = 0; i < 256; i++)
                {
                    avec[i] = Math.Abs((partialSum(histogram, i) / total) - ptile);
                    if (avec[i] < temp)
                    {
                        temp = avec[i];
                        thresholdValue = i;
                    }
                }
            }

            dest.UnlockBits(bmd);

            return GlobalThreshold(dest, rect, thresholdValue);
        }

        private static double partialSum(int[] y, int j)
        {
            double x = 0;
            for (int i = 0; i <= j; i++)
                x += y[i];
            return x;
        }
    }
}
