using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace MokkAnnotator.MkaImageProcess
{
    class ImageAdjustment
    {
        /// <summary>
        /// Adjust hue saturation lightness of image
        /// </summary>        
        public static unsafe Bitmap AdjustHSL(Bitmap image, Rectangle rect, int hue, int saturation, int lightness)
        {
            int pixelSize = Image.GetPixelFormatSize(image.PixelFormat) / 8;
            Bitmap dest = (Bitmap)image.Clone();
            BitmapData imgDat = dest.LockBits(rect, ImageLockMode.ReadWrite, dest.PixelFormat);

            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;
            int offset = imgDat.Stride - rect.Width * pixelSize;

            RGB rgb = new RGB();
            HSL hsl = new HSL();

            // do the job
            byte* img = (byte*)imgDat.Scan0;

            // align pointer to the first pixel to process
            img += (startY * imgDat.Stride + startX * pixelSize);

            int intensity;
            int a, invA;
            RGB blendColor;
            if (lightness > 0)
            {
                a = (int)((lightness * 255) / 100);
                blendColor = new RGB(255, 255, 255);
            }
            else
            {
                a = (int)((-lightness * 255) / 100);
                blendColor = new RGB(0, 0, 0);
            }
            invA = 255 - a;

            if (lightness == 0)
            {
                // for each row
                for (int y = startY; y < stopY; y++)
                {
                    // for each pixel
                    for (int x = startX; x < stopX; x++, img += pixelSize)
                    {
                        // adjust saturation
                        intensity = (int)((7471 * img[RGB.B] + 38470 * img[RGB.G] + 19595 * img[RGB.R]) >> 16);
                        rgb.Red = ClampToByte((intensity * 1024 + (img[RGB.R] - intensity) * saturation) >> 10);
                        rgb.Green = ClampToByte((intensity * 1024 + (img[RGB.G] - intensity) * saturation) >> 10);
                        rgb.Blue = ClampToByte((intensity * 1024 + (img[RGB.B] - intensity) * saturation) >> 10);

                        // adjust hue
                        hsl = HSL.FromRGB(rgb);
                        hsl.Hue += hue;
                        if (hsl.Hue < 0)
                            hsl.Hue += 360;
                        else if (hsl.Hue > 360)
                            hsl.Hue -= 360;
                        rgb = hsl.ToRGB();

                        img[RGB.R] = rgb.Red;
                        img[RGB.G] = rgb.Green;
                        img[RGB.B] = rgb.Blue;
                    }
                    img += offset;
                }
            }
            else
            {
                // for each row
                for (int y = startY; y < stopY; y++)
                {
                    // for each pixel
                    for (int x = startX; x < stopX; x++, img += pixelSize)
                    {
                        // adjust saturation
                        intensity = (int)((7471 * img[RGB.B] + 38470 * img[RGB.G] + 19595 * img[RGB.R]) >> 16);
                        rgb.Red = ClampToByte((intensity * 1024 + (img[RGB.R] - intensity) * saturation) >> 10);
                        rgb.Green = ClampToByte((intensity * 1024 + (img[RGB.G] - intensity) * saturation) >> 10);
                        rgb.Blue = ClampToByte((intensity * 1024 + (img[RGB.B] - intensity) * saturation) >> 10);

                        // adjust hue
                        hsl = HSL.FromRGB(rgb);
                        hsl.Hue += hue;
                        if (hsl.Hue < 0)
                            hsl.Hue += 360;
                        else if (hsl.Hue > 360)
                            hsl.Hue -= 360;
                        rgb = hsl.ToRGB();

                        // adjust lightness
                        img[RGB.R] = (byte)(((rgb.Red * invA) + (blendColor.Red * a)) / 256);
                        img[RGB.G] = (byte)(((rgb.Green * invA) + (blendColor.Green * a)) / 256);
                        img[RGB.B] = (byte)(((rgb.Blue * invA) + (blendColor.Blue * a)) / 256);
                    }
                    img += offset;
                }
            }

            dest.UnlockBits(imgDat);

            return dest;
        }

        /// <summary>
        /// Adjust brightness and contrast of image
        /// </summary>        
        public static unsafe Bitmap AdjustBrightnessContrast(Bitmap image, Rectangle rect, int brightness, int contrast)
        {
            int multiply;
            int divide;
            byte[] rgbTable = new byte[65536];

            if (contrast < 0)
            {
                multiply = contrast + 100;
                divide = 100;
            }
            else if (contrast > 0)
            {
                multiply = 100;
                divide = 100 - contrast;
            }
            else
            {
                multiply = 1;
                divide = 1;
            }

            if (divide == 0)
            {
                for (int intensity = 0; intensity < 256; ++intensity)
                {
                    if (intensity + brightness < 128)
                    {
                        rgbTable[intensity] = 0;
                    }
                    else
                    {
                        rgbTable[intensity] = 255;
                    }
                }
            }
            else if (divide == 100)
            {
                for (int intensity = 0; intensity < 256; ++intensity)
                {
                    int shift = (intensity - 127) * multiply / divide + 127 - intensity + brightness;

                    for (int col = 0; col < 256; ++col)
                    {
                        int index = (intensity * 256) + col;
                        rgbTable[index] = ClampToByte(col + shift);
                    }
                }
            }
            else
            {
                for (int intensity = 0; intensity < 256; ++intensity)
                {
                    int shift = (intensity - 127 + brightness) * multiply / divide + 127 - intensity;

                    for (int col = 0; col < 256; ++col)
                    {
                        int index = (intensity * 256) + col;
                        rgbTable[index] = ClampToByte(col + shift);
                    }
                }
            }

            int pixelSize = Image.GetPixelFormatSize(image.PixelFormat) / 8;
            Bitmap dest = (Bitmap)image.Clone();
            BitmapData imgDat = dest.LockBits(rect, ImageLockMode.ReadWrite, dest.PixelFormat);

            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;
            int offset = imgDat.Stride - rect.Width * pixelSize;

            RGB rgb = new RGB();

            // do the job
            byte* img = (byte*)imgDat.Scan0;

            // align pointer to the first pixel to process
            img += (startY * imgDat.Stride + startX * pixelSize);

            if (divide == 0)
            {
                // for each row
                for (int y = startY; y < stopY; y++)
                {
                    // for each pixel
                    for (int x = startX; x < stopX; x++, img += pixelSize)
                    {
                        int i = (int)((7471 * img[RGB.B] + 38470 * img[RGB.G] + 19595 * img[RGB.R]) >> 16);
                        uint c = rgbTable[i];
                        uint bgra = (uint)img[RGB.B] + ((uint)img[RGB.G] << 8) + ((uint)img[RGB.R] << 16);
                        bgra = (bgra & 0xff000000) | c | (c << 8) | (c << 16);

                        img[RGB.R] = (byte)(bgra & 0x000000ff);
                        img[RGB.G] = (byte)((bgra & 0x0000ff00) >> 8);
                        img[RGB.B] = (byte)((bgra & 0x00ff0000) >> 16);
                    }
                    img += offset;
                }
            }
            else
            {
                // for each row
                for (int y = startY; y < stopY; y++)
                {
                    // for each pixel
                    for (int x = startX; x < stopX; x++, img += pixelSize)
                    {
                        int i = (int)((7471 * img[RGB.B] + 38470 * img[RGB.G] + 19595 * img[RGB.R]) >> 16);
                        int shiftIndex = i * 256;
                        img[RGB.R] = rgbTable[shiftIndex + img[RGB.R]];
                        img[RGB.G] = rgbTable[shiftIndex + img[RGB.G]];
                        img[RGB.B] = rgbTable[shiftIndex + img[RGB.B]];
                    }
                    img += offset;
                }
            }

            dest.UnlockBits(imgDat);

            return dest;
        }

        private static byte ClampToByte(int x)
        {
            if (x > 255)
            {
                return 255;
            }
            else if (x < 0)
            {
                return 0;
            }
            else
            {
                return (byte)x;
            }
        }      
    }
}
