using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace MokkAnnotator.MkaImageProcess
{
    /// <summary>
    /// Set of statistics functions.
    /// </summary>
    /// 
    /// <remarks>The class represents collection of simple functions used
    /// in statistics.</remarks>
    /// 
    public static class Statistics
    {
        public static unsafe double Mean(BitmapData image, int x, int y, int size)
        {
            int count = 0;
            int sum = 0;
            byte* p = (byte*)image.Scan0.ToPointer();
            int size2 = size / 2;
            int widthStep = image.Stride;
            int xs = Math.Max(x - size2, 0);
            int xe = Math.Min(x + size2 + 1, image.Width);
            int ys = Math.Max(y - size2, 0);
            int ye = Math.Min(y + size2 + 1, image.Height);
            byte v;

            for (int xx = xs; xx < xe; xx++)
            {
                for (int yy = ys; yy < ye; yy++)
                {
                    v = p[widthStep * yy + xx];
                    sum += v;
                    count++;
                }
            }

            double mean = (double)sum / count;

            return mean;
        }

        public static unsafe double Median(BitmapData image, int x, int y, int size)
        {
            byte* p = (byte*)image.Scan0.ToPointer();
            int size2 = size / 2;
            int widthStep = image.Stride;
            int xs = Math.Max(x - size2, 0);
            int xe = Math.Min(x + size2 + 1, image.Width);
            int ys = Math.Max(y - size2, 0);
            int ye = Math.Min(y + size2 + 1, image.Height);
            int i = 0;
            int length = (xe - xs) * (ye - ys);
            byte[] data = new byte[length];

            for (int xx = xs; xx < xe; xx++)
                for (int yy = ys; yy < ye; yy++, i++)
                    data[i] = p[widthStep * yy + xx];
            Array.Sort(data);

            double median;
            if (length % 2 == 0)
                median = (byte)((data[length / 2] + data[(length / 2) - 1]) * 0.5 + 0.5); // N is even 
            else median = data[length / 2];                      // N is odd

            return median;
        }

        public static unsafe double StdDev(BitmapData image, int width, int height, int x, int y, int size)
        {
            int count = 0;
            int sum = 0;
            int sqsum = 0;
            byte* p = (byte*)image.Scan0.ToPointer();
            int size2 = size / 2;
            int widthStep = image.Stride;
            int xs = Math.Max(x - size2, 0);
            int xe = Math.Min(x + size2 + 1, image.Width);
            int ys = Math.Max(y - size2, 0);
            int ye = Math.Min(y + size2 + 1, image.Height);
            byte v;

            for (int xx = xs; xx < xe; xx++)
            {
                for (int yy = ys; yy < ye; yy++)
                {
                    v = p[widthStep * yy + xx];
                    sum += v;
                    sqsum += v * v;
                    count++;
                }
            }
            double mean = (double)sum / count;
            double var = ((double)sqsum / count) - (mean * mean);
            if (var < 0.0) var = 0.0;
            double stddev = Math.Sqrt(var);

            return stddev;
        }

        public static unsafe void MeanStdDev(BitmapData image, int x, int y, int size, out double mean, out double stddev)
        {
            int count = 0;
            int sum = 0;
            int sqsum = 0;
            byte* p = (byte*)image.Scan0.ToPointer();
            int size2 = size / 2;
            int widthStep = image.Stride;
            int xs = Math.Max(x - size2, 0);
            int xe = Math.Min(x + size2 + 1, image.Width);
            int ys = Math.Max(y - size2, 0);
            int ye = Math.Min(y + size2 + 1, image.Height);
            byte v;

            for (int xx = xs; xx < xe; xx++)
            {
                for (int yy = ys; yy < ye; yy++)
                {
                    v = p[widthStep * yy + xx];
                    sum += v;
                    sqsum += v * v;
                    count++;
                }
            }
            mean = (double)sum / count;
            double var = ((double)sqsum / count) - (mean * mean);
            if (var < 0.0) var = 0.0;
            stddev = Math.Sqrt(var);
        }

        public static unsafe double VarAver(BitmapData image, int x, int y, int size)
        {
            double sum = 0;
            int size2 = size / 2;
            int xs = Math.Max(x - size2, 0);
            int xe = Math.Min(x + size2 + 1, image.Width);
            int ys = Math.Max(y - size2, 0);
            int ye = Math.Min(y + size2 + 1, image.Height);
            double m, v;

            for (int xx = xs; xx < xe; xx++)
            {
                for (int yy = ys; yy < ye; yy++)
                {
                    MeanVar(image, xx, yy, size, out m, out v);
                    sum += v;
                }
            }

            return (double)sum / (size * size);
        }

        public static unsafe void MeanVar(BitmapData image, int x, int y, int size, out double mean, out double var)
        {
            int sum = 0;
            int sqsum = 0;
            byte* p = (byte*)image.Scan0.ToPointer();
            int size2 = size / 2;
            int widthStep = image.Stride;
            int xs = Math.Max(x - size2, 0);
            int xe = Math.Min(x + size2 + 1, image.Width);
            int ys = Math.Max(y - size2, 0);
            int ye = Math.Min(y + size2 + 1, image.Height);
            byte v;

            for (int xx = xs; xx < xe; xx++)
            {
                for (int yy = ys; yy < ye; yy++)
                {
                    v = p[widthStep * yy + xx];
                    sum += v;
                    sqsum += v * v;
                }
            }
            mean = (double)sum / (size * size);
            var = (double)sqsum / (size * size) - mean * mean;
        }

        public static unsafe void MinMax(BitmapData image, int x, int y, int size, out byte min, out byte max)
        {
            byte* p = (byte*)image.Scan0.ToPointer();
            int size2 = size / 2;
            int widthStep = image.Stride;
            min = byte.MaxValue;
            max = byte.MinValue;
            int xs = Math.Max(x - size2, 0);
            int xe = Math.Min(x + size2 + 1, image.Width);
            int ys = Math.Max(y - size2, 0);
            int ye = Math.Min(y + size2 + 1, image.Height);

            for (int xx = xs; xx < xe; xx++)
            {
                for (int yy = ys; yy < ye; yy++)
                {
                    byte v = p[widthStep * yy + xx];
                    if (max < v)
                        max = v;
                    else if (min > v)
                        min = v;
                }
            }
        }

        /// <summary>
        /// Number of background in pixel in the sliding windows
        /// </summary>
        public static unsafe int Sum(BitmapData image, int x, int y, int size)
        {
            byte* p = (byte*)image.Scan0.ToPointer();
            int size2 = size / 2;
            int widthStep = image.Stride;
            int xs = Math.Max(x - size2, 0);
            int xe = Math.Min(x + size2 + 1, image.Width);
            int ys = Math.Max(y - size2, 0);
            int ye = Math.Min(y + size2 + 1, image.Height);
            int sum = 0;

            for (int xx = xs; xx < xe; xx++)
                for (int yy = ys; yy < ye; yy++)
                    if (p[widthStep * yy + xx] == 255)
                        sum++;

            return sum;
        }

        /// <summary>
        /// Calculate mean value.
        /// </summary>
        /// 
        /// <param name="values">Histogram array.</param>
        /// 
        /// <returns>Returns mean value.</returns>
        /// 
        public static double MeanHistogram(int[] values)
        {
            int total = 0;
            double mean = 0;

            // for all values
            for (int i = 0, n = values.Length; i < n; i++)
            {
                // accumulate mean
                mean += i * values[i];
                // accumalate total
                total += values[i];
            }
            return mean / total;
        }

        public static double Mean(int[] values)
        {
            double sum = 0;

            // for all values
            for (int i = 0, n = values.Length; i < n; i++)
                sum += values[i];

            return sum / values.Length;
        }

        public static void MeanMinMax(int[] values, out double mean, out int min, out int max)
        {
            int curMin = int.MaxValue;
            int curMax = int.MinValue;

            mean = 0;
            int n = values.Length;
            // for all values
            for (int i = 0; i < n; i++)
            {
                mean += values[i];

                if (values[i] < curMin)
                    curMin = values[i];
                else if (values[i] > curMax)
                    curMax = values[i];
            }

            mean /= n;
            min = curMin;
            max = curMax;
        }

        public static void MeanMinMax(int[] values, int start, int stop, out double mean, out int min, out int max)
        {
            int curMin = int.MaxValue;
            int curMax = int.MinValue;

            mean = 0;
            int n = stop - start;
            // for all values
            for (int i = start; i < stop; i++)
            {
                mean += values[i];

                if (values[i] < curMin)
                    curMin = values[i];
                else if (values[i] > curMax)
                    curMax = values[i];
            }

            mean /= n;
            min = curMin;
            max = curMax;
        }

        public static int Max(int[] values)
        {
            int max = 0;
            int n = values.Length;
            // for all values
            for (int i = 0; i < n; i++)
            {
                if (values[i] > max)
                    max = values[i];
            }

            return max;
        }

        public static int MaxIndex(int[] values)
        {
            int max = 0;
            int ret = 0;
            int n = values.Length;
            // for all values
            for (int i = 0; i < n; i++)
            {
                if (values[i] > max)
                {
                    max = values[i];
                    ret = i;
                }
            }

            return ret;
        }

        public static double Max(double[] values)
        {
            double max = 0;
            int n = values.Length;
            // for all values
            for (int i = 0; i < n; i++)
            {
                if (values[i] > max)
                    max = values[i];
            }

            return max;
        }

        public static int Moment(int[] values)
        {
            double mean = MeanHistogram(values);
            int n = values.Length;
            double moment = 0, sum = 0;
            double x;

            for (int i = 0; i < n; i++)
            {
                x = values[i] - mean;
                moment += x * x * i;
                sum += x * x;
            }
            if (sum == 0) return 0;
            else return (int)(2 * Math.Sqrt(moment / sum) + 0.5);
        }

        /// <summary>
        /// Calculate standard deviation.
        /// </summary>
        /// 
        /// <param name="values">Histogram array.</param>
        /// 
        /// <returns>Returns value of standard deviation.</returns>
        ///        
        public static double StdDev(int[] values)
        {
            double mean = MeanHistogram(values);
            double stddev = 0;
            double centeredValue;
            int total = 0;

            // for all values
            for (int i = 0, n = values.Length; i < n; i++)
            {
                centeredValue = (double)i - mean;

                // accumulate mean
                stddev += centeredValue * centeredValue * values[i];
                // accumulate total
                total += values[i];
            }

            return Math.Sqrt(stddev / total);
        }

        /// <summary>
        /// Calculate median value.
        /// </summary>
        /// 
        /// <param name="values">Histogram array.</param>
        /// 
        /// <returns>Returns value of median.</returns>
        /// 
        public static int Median(int[] values)
        {
            int total = 0, n = values.Length;

            // for all values
            for (int i = 0; i < n; i++)
            {
                // accumulate total
                total += values[i];
            }

            int halfTotal = total / 2;
            int median = 0, v = 0;

            // find median value
            for (; median < n; median++)
            {
                v += values[median];
                if (v >= halfTotal)
                    break;
            }

            return median;
        }

        /// <summary>
        /// Calculate entropy value.
        /// </summary>
        /// 
        /// <param name="values">Histogram array.</param>
        /// 
        /// <returns>Returns entropy value of the specified histagram array.</returns>
        /// 
        public static double Entropy(int[] values)
        {
            int n = values.Length;
            int total = 0;
            double entropy = 0;
            double p;

            // calculate total amount of hits
            for (int i = 0; i < n; i++)
            {
                total += values[i];
            }

            // for all values
            for (int i = 0; i < n; i++)
            {
                // get item's probability
                p = (double)values[i] / total;
                // calculate entropy
                if (p != 0)
                    entropy += (-p * Math.Log(p, 2));
            }
            return entropy;
        }
    }
}
