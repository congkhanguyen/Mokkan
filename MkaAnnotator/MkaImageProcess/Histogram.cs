using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace MokkAnnotator.MkaImageProcess
{
    public static class Histogram
    {
        /// <summary>
        /// Calculate histogram of image and get the area and boundary of objects in the image
        /// </summary>
        /// <returns>Area of objects in the image</returns>
        public static unsafe int CalHistogram(byte* ptr, int stride, ref Rectangle rect, int[] horHist, int[] verHist)
        {
            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;
            int offset = stride - rect.Width; 
            int area = 0;

            // align pointers by X and Y
            ptr += (startY * stride + startX);        

            // calculate vertical histogram
            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                {
                    if (*ptr == 0)
                    {
                        horHist[x]++;
                        verHist[y]++;
                        area++;
                    }
                }
                ptr += offset;
            }            

            stopY--;
            stopX--;
            while (verHist[startY] == 0 && startY < stopY) startY++;
            while (verHist[stopY] == 0 && stopY >= startY) stopY--;
            while (horHist[startX] == 0 && startX < stopX) startX++;
            while (horHist[stopX] == 0 && stopX >= startX) stopX--;

            rect = Rectangle.FromLTRB(startX, startY, stopX + 1, stopY + 1);

            return area;
        }

        #region Grayscale image

        /// <summary>
        /// Calculate horizontal and vertical histogram of grayscale image
        /// </summary>        
        public static unsafe void HistG(Bitmap image, Rectangle rect, int[] horHist, int[] verHist)
        {
            if(image.PixelFormat != PixelFormat.Format8bppIndexed) return;

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;

            int stride = imageData.Stride;
            int offset = stride - rect.Width;

            // image pointers
            byte* ptr = (byte*)imageData.Scan0.ToPointer();

            // align pointers by X and Y
            ptr += (startY * stride + startX);

            // calculate vertical histogram
            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                {                   
                    horHist[x] += 255 - *ptr;
                    verHist[y] += 255 - *ptr;                                         
                }
                ptr += offset;
            }

            image.UnlockBits(imageData);
        }

        /// <summary>
        /// Calculate horizontal histogram of grayscale image
        /// </summary>
        public static unsafe void HorHistG(Bitmap image, Rectangle rect, int[] horHist)
        {
            if (image.PixelFormat != PixelFormat.Format8bppIndexed) return;

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;

            int stride = imageData.Stride;
            int offset = stride - rect.Width;

            // image pointers
            byte* ptr = (byte*)imageData.Scan0.ToPointer();

            // align pointers by X and Y
            ptr += (startY * stride + startX);

            // calculate vertical histogram
            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                    horHist[x] += 255 - *ptr;
                ptr += offset;
            }

            image.UnlockBits(imageData);
        }

        /// <summary>
        /// Calculate vertical histogram of grayscale image
        /// </summary>
        public static unsafe void VerHistG(Bitmap image, Rectangle rect, int[] verHist)
        {
            if (image.PixelFormat != PixelFormat.Format8bppIndexed) return;

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;

            int stride = imageData.Stride;
            int offset = stride - rect.Width;

            // image pointers
            byte* ptr = (byte*)imageData.Scan0.ToPointer();

            // align pointers by X and Y
            ptr += (startY * stride + startX);

            // calculate vertical histogram
            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                    verHist[y] += 255 - *ptr;
                ptr += offset;
            }

            image.UnlockBits(imageData);
        }

        /// <summary>
        /// Calculate horizontal and vertical histogram of grayscale image
        /// </summary>        
        public static unsafe void HistG(byte* ptr, int stride, Rectangle rect, int[] horHist, int[] verHist)
        {
            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;
            int offset = stride - rect.Width;

            // align pointers by X and Y
            ptr += (startY * stride + startX);
            // calculate vertical histogram
            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                {
                    horHist[x] += 255 - *ptr;
                    verHist[y] += 255 - *ptr;
                }
                ptr += offset;
            }
        }

        /// <summary>
        /// Calculate horizontal histogram of grayscale image
        /// </summary>
        public static unsafe void HorHistG(byte* ptr, int stride, Rectangle rect, int[] horHist)
        {
            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;
            int offset = stride - rect.Width; 

            // align pointers by X and Y
            ptr += (startY * stride + startX);

            // calculate vertical histogram
            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                    horHist[x] += 255 - *ptr;
                ptr += offset;
            }
        }

        /// <summary>
        /// Calculate vertical histogram of grayscale image
        /// </summary>
        public static unsafe void VerHistG(byte* ptr, int stride, Rectangle rect, int[] verHist)
        {
            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;
            int offset = stride - rect.Width; 

            // align pointers by X and Y
            ptr += (startY * stride + startX);

            // calculate vertical histogram
            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                    verHist[y] += 255 - *ptr;
                ptr += offset;
            }
        }

        #endregion

        #region Binary image

        /// <summary>
        /// Calculate horizontal and vertical histogram of binary image
        /// </summary>        
        public static unsafe void HistB(Bitmap image, Rectangle rect, int[] horHist, int[] verHist)
        {
            if (image.PixelFormat != PixelFormat.Format8bppIndexed) return;

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;

            int stride = imageData.Stride;
            int offset = stride - rect.Width;

            // image pointers
            byte* ptr = (byte*)imageData.Scan0.ToPointer();

            // align pointers by X and Y
            ptr += (startY * stride + startX);

            // calculate vertical histogram
            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                {
                    if (*ptr == 0)
                    {
                        horHist[x] ++;
                        verHist[y] ++;
                    }
                }
                ptr += offset;
            }

            image.UnlockBits(imageData);
        }

        /// <summary>
        /// Calculate horizontal histogram of binary image
        /// </summary>
        public static unsafe void HorHistB(Bitmap image, Rectangle rect, int[] horHist)
        {
            if (image.PixelFormat != PixelFormat.Format8bppIndexed) return;

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;

            int stride = imageData.Stride;
            int offset = stride - rect.Width;

            // image pointers
            byte* ptr = (byte*)imageData.Scan0.ToPointer();

            // align pointers by X and Y
            ptr += (startY * stride + startX);

            // calculate vertical histogram
            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                    if (*ptr == 0) 
                        horHist[x]++;
                ptr += offset;
            }

            image.UnlockBits(imageData);
        }

        /// <summary>
        /// Calculate vertical histogram of binary image
        /// </summary>
        public static unsafe void VerHistB(Bitmap image, Rectangle rect, int[] verHist)
        {
            if (image.PixelFormat != PixelFormat.Format8bppIndexed) return;

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;

            int stride = imageData.Stride;
            int offset = stride - rect.Width;

            // image pointers
            byte* ptr = (byte*)imageData.Scan0.ToPointer();

            // align pointers by X and Y
            ptr += (startY * stride + startX);

            // calculate vertical histogram
            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                    if (*ptr == 0) 
                        verHist[y]++;
                ptr += offset;
            }

            image.UnlockBits(imageData);
        }

        /// <summary>
        /// Calculate horizontal and vertical histogram of binary image
        /// </summary>        
        public static unsafe void HistB(byte* ptr, int stride, Rectangle rect, int[] horHist, int[] verHist)
        {
            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;
            int offset = stride - rect.Width; 

            // align pointers by X and Y
            ptr += (startY * stride + startX);

            // calculate vertical histogram
            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                {
                    if (*ptr == 0)
                    {
                        horHist[x]++;
                        verHist[y]++;
                    }
                }
                ptr += offset;
            }
        }

        /// <summary>
        /// Calculate horizontal histogram of binary image
        /// </summary>
        public static unsafe void HorHistB(byte* ptr, int stride, Rectangle rect, int[] horHist)
        {
            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;
            int offset = stride - rect.Width; 

            // align pointers by X and Y
            ptr += (startY * stride + startX);

            // calculate vertical histogram
            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                    if (*ptr == 0)
                        horHist[x]++;
                ptr += offset;
            }
        }

        /// <summary>
        /// Calculate vertical histogram of binary image
        /// </summary>
        public static unsafe void VerHistB(byte* ptr, int stride, Rectangle rect, int[] verHist)
        {
            // processing start and stop X,Y positions
            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Right;
            int stopY = rect.Bottom;
            int offset = stride - rect.Width; 

            // align pointers by X and Y
            ptr += (startY * stride + startX);

            // calculate vertical histogram
            // for each line	
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, ptr++)
                    if (*ptr == 0)
                        verHist[y]++;
                ptr += offset;
            }
        }

        #endregion
    }
}
