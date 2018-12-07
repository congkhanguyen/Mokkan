using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace MokkAnnotator.MkaImageProcess
{
    class ImageRotate
    {
        /// <summary>
        /// Rotate image with nearest neighbor interpolation
        /// </summary>
        public static unsafe Bitmap RotateNearestNeighbor(Bitmap image, Rectangle rect, double angle, Color fillColor)
        {
            if (image.PixelFormat == PixelFormat.Format8bppIndexed) return (Bitmap)image.Clone();
            BitmapData source = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);

            // get source image size
            int width = rect.Width;
            int height = rect.Height;
            double halfWidth = (double)width / 2;
            double halfHeight = (double)height / 2;

            // get destination image size
            Size newSize = CalculateNewImageSize(width, height, angle);
            int newWidth = newSize.Width;
            int newHeight = newSize.Height;
            double halfNewWidth = (double)newWidth / 2;
            double halfNewHeight = (double)newHeight / 2;

            Bitmap dest = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            BitmapData destination = dest.LockBits(new Rectangle(0, 0, newWidth, newHeight), ImageLockMode.ReadWrite, dest.PixelFormat);

            int srcStride = source.Stride;
            int dstOffset = destination.Stride - newWidth * 3;

            // fill values
            byte fillR = fillColor.R;
            byte fillG = fillColor.G;
            byte fillB = fillColor.B;

            // angle's sine and cosine
            double angleRad = -angle * Math.PI / 180;
            double angleCos = Math.Cos(angleRad);
            double angleSin = Math.Sin(angleRad);

            // do the job
            byte* src = (byte*)source.Scan0.ToPointer();
            byte* dst = (byte*)destination.Scan0.ToPointer();

            // destination pixel's coordinate relative to image center
            double cx, cy;
            // source pixel's coordinates
            int ox, oy;
            // temporary pointer
            byte* p;

            cy = -halfNewHeight;
            for (int y = 0; y < newHeight; y++)
            {
                cx = -halfNewWidth;
                for (int x = 0; x < newWidth; x++, dst += 3)
                {
                    // coordinate of the nearest point
                    ox = (int)(angleCos * cx + angleSin * cy + halfWidth);
                    oy = (int)(-angleSin * cx + angleCos * cy + halfHeight);

                    // validate source pixel's coordinates
                    if ((ox < 0) || (oy < 0) || (ox >= width) || (oy >= height))
                    {
                        // fill destination image with filler
                        dst[RGB.R] = fillR;
                        dst[RGB.G] = fillG;
                        dst[RGB.B] = fillB;
                    }
                    else
                    {
                        // fill destination image with pixel from source image
                        p = src + oy * srcStride + ox * 3;

                        dst[RGB.R] = p[RGB.R];
                        dst[RGB.G] = p[RGB.G];
                        dst[RGB.B] = p[RGB.B];
                    }
                    cx++;
                }
                cy++;
                dst += dstOffset;
            }

            image.UnlockBits(source);
            dest.UnlockBits(destination);

            return dest;
        }

        /// <summary>
        /// Rotate image with bilinear interpolation
        /// </summary>
        public static unsafe Bitmap RotateBilinear(Bitmap image, Rectangle rect, double angle, Color fillColor)
        {
            if (image.PixelFormat == PixelFormat.Format8bppIndexed) return (Bitmap)image.Clone();
            BitmapData source = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);

            // get source image size
            int width = rect.Width;
            int height = rect.Height;
            double halfWidth = (double)width / 2;
            double halfHeight = (double)height / 2;

            // get destination image size
            Size newSize = CalculateNewImageSize(width, height, angle);
            int newWidth = newSize.Width;
            int newHeight = newSize.Height;
            double halfNewWidth = (double)newWidth / 2;
            double halfNewHeight = (double)newHeight / 2;

            Bitmap dest = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            BitmapData destination = dest.LockBits(new Rectangle(0, 0, newWidth, newHeight), ImageLockMode.ReadWrite, dest.PixelFormat);
            int srcStride = source.Stride;
            int dstOffset = destination.Stride - newWidth * 3;

            // fill values
            byte fillR = fillColor.R;
            byte fillG = fillColor.G;
            byte fillB = fillColor.B;

            // angle's sine and cosine
            double angleRad = -angle * Math.PI / 180;
            double angleCos = Math.Cos(angleRad);
            double angleSin = Math.Sin(angleRad);

            // do the job
            byte* src = (byte*)source.Scan0.ToPointer();
            byte* dst = (byte*)destination.Scan0.ToPointer();

            // destination pixel's coordinate relative to image center
            double cx, cy;
            // coordinates of source points
            double ox, oy, tx, ty, dx1, dy1, dx2, dy2;
            int ox1, oy1, ox2, oy2;
            // width and height decreased by 1
            int ymax = height - 1;
            int xmax = width - 1;
            // temporary pointers
            byte* p1, p2, p3, p4;

            cy = -halfNewHeight;
            for (int y = 0; y < newHeight; y++)
            {
                // do some pre-calculations of source points' coordinates
                // (calculate the part which depends on y-loop, but does not
                // depend on x-loop)
                tx = angleSin * cy + halfWidth;
                ty = angleCos * cy + halfHeight;

                cx = -halfNewWidth;
                for (int x = 0; x < newWidth; x++, dst += 3)
                {
                    // coordinates of source point
                    ox = tx + angleCos * cx;
                    oy = ty - angleSin * cx;

                    // top-left coordinate
                    ox1 = (int)ox;
                    oy1 = (int)oy;

                    // validate source pixel's coordinates
                    if ((ox1 < 0) || (oy1 < 0) || (ox1 >= width) || (oy1 >= height))
                    {
                        // fill destination image with filler
                        dst[RGB.R] = fillR;
                        dst[RGB.G] = fillG;
                        dst[RGB.B] = fillB;
                    }
                    else
                    {
                        // bottom-right coordinate
                        ox2 = (ox1 == xmax) ? ox1 : ox1 + 1;
                        oy2 = (oy1 == ymax) ? oy1 : oy1 + 1;

                        if ((dx1 = ox - (float)ox1) < 0)
                            dx1 = 0;
                        dx2 = 1.0f - dx1;

                        if ((dy1 = oy - (float)oy1) < 0)
                            dy1 = 0;
                        dy2 = 1.0f - dy1;

                        // get four points
                        p1 = p2 = src + oy1 * srcStride;
                        p1 += ox1 * 3;
                        p2 += ox2 * 3;

                        p3 = p4 = src + oy2 * srcStride;
                        p3 += ox1 * 3;
                        p4 += ox2 * 3;

                        // interpolate using 4 points

                        // red
                        dst[RGB.R] = (byte)(
                            dy2 * (dx2 * p1[RGB.R] + dx1 * p2[RGB.R]) +
                            dy1 * (dx2 * p3[RGB.R] + dx1 * p4[RGB.R]));

                        // green
                        dst[RGB.G] = (byte)(
                            dy2 * (dx2 * p1[RGB.G] + dx1 * p2[RGB.G]) +
                            dy1 * (dx2 * p3[RGB.G] + dx1 * p4[RGB.G]));

                        // blue
                        dst[RGB.B] = (byte)(
                            dy2 * (dx2 * p1[RGB.B] + dx1 * p2[RGB.B]) +
                            dy1 * (dx2 * p3[RGB.B] + dx1 * p4[RGB.B]));
                    }
                    cx++;
                }
                cy++;
                dst += dstOffset;
            }

            image.UnlockBits(source);
            dest.UnlockBits(destination);

            return dest;
        }

        /// <summary>
        /// Rotate image with bicubic interpolation
        /// </summary>
        public static unsafe Bitmap RotateBicubic(Bitmap image, Rectangle rect, double angle, Color fillColor)
        {
            if (image.PixelFormat == PixelFormat.Format8bppIndexed) return (Bitmap)image.Clone();
            BitmapData source = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);

            // get source image size
            int width = rect.Width;
            int height = rect.Height;
            double halfWidth = (double)width / 2;
            double halfHeight = (double)height / 2;

            // get destination image size
            Size newSize = CalculateNewImageSize(width, height, angle);
            int newWidth = newSize.Width;
            int newHeight = newSize.Height;
            double halfNewWidth = (double)newWidth / 2;
            double halfNewHeight = (double)newHeight / 2;

            Bitmap dest = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            BitmapData destination = dest.LockBits(new Rectangle(0, 0, newWidth, newHeight), ImageLockMode.ReadWrite, dest.PixelFormat);
            int srcStride = source.Stride;
            int dstOffset = destination.Stride - newWidth * 3;

            // fill values
            byte fillR = fillColor.R;
            byte fillG = fillColor.G;
            byte fillB = fillColor.B;

            // angle's sine and cosine
            double angleRad = -angle * Math.PI / 180;
            double angleCos = Math.Cos(angleRad);
            double angleSin = Math.Sin(angleRad);

            // do the job
            byte* src = (byte*)source.Scan0.ToPointer();
            byte* dst = (byte*)destination.Scan0.ToPointer();

            // destination pixel's coordinate relative to image center
            double cx, cy;
            // coordinates of source points and coefficients
            double ox, oy, dx, dy, k1, k2;
            int ox1, oy1, ox2, oy2;
            // destination pixel values
            double r, g, b;
            // width and height decreased by 1
            int ymax = height - 1;
            int xmax = width - 1;
            // temporary pointer
            byte* p;

            cy = -halfNewHeight;
            for (int y = 0; y < newHeight; y++)
            {
                cx = -halfNewWidth;
                for (int x = 0; x < newWidth; x++, dst += 3)
                {
                    // coordinates of source point
                    ox = angleCos * cx + angleSin * cy + halfWidth;
                    oy = -angleSin * cx + angleCos * cy + halfHeight;

                    ox1 = (int)ox;
                    oy1 = (int)oy;

                    // validate source pixel's coordinates
                    if ((ox1 < 0) || (oy1 < 0) || (ox1 >= width) || (oy1 >= height))
                    {
                        // fill destination image with filler
                        dst[RGB.R] = fillR;
                        dst[RGB.G] = fillG;
                        dst[RGB.B] = fillB;
                    }
                    else
                    {
                        dx = ox - (float)ox1;
                        dy = oy - (float)oy1;

                        // initial pixel value
                        r = g = b = 0;

                        for (int n = -1; n < 3; n++)
                        {
                            // get Y coefficient
                            k1 = Interpolation.BiCubicKernel(dy - (float)n);

                            oy2 = oy1 + n;
                            if (oy2 < 0)
                                oy2 = 0;
                            if (oy2 > ymax)
                                oy2 = ymax;

                            for (int m = -1; m < 3; m++)
                            {
                                // get X coefficient
                                k2 = k1 * Interpolation.BiCubicKernel((float)m - dx);

                                ox2 = ox1 + m;
                                if (ox2 < 0)
                                    ox2 = 0;
                                if (ox2 > xmax)
                                    ox2 = xmax;

                                // get pixel of original image
                                p = src + oy2 * srcStride + ox2 * 3;

                                r += k2 * p[RGB.R];
                                g += k2 * p[RGB.G];
                                b += k2 * p[RGB.B];
                            }
                        }
                        dst[RGB.R] = (byte)r;
                        dst[RGB.G] = (byte)g;
                        dst[RGB.B] = (byte)b;
                    }
                    cx++;
                }
                cy++;
                dst += dstOffset;
            }

            image.UnlockBits(source);
            dest.UnlockBits(destination);

            return dest;
        }

        /// <summary>
        /// Calculates new image size.
        /// </summary>
        /// <returns>New image size - size of the destination image.</returns>
        /// 
        private static Size CalculateNewImageSize(int width, int height, double angle)
        {
            // angle's sine and cosine
            double angleRad = -angle * Math.PI / 180;
            double angleCos = Math.Cos(angleRad);
            double angleSin = Math.Sin(angleRad);

            // calculate half size
            double halfWidth = (double)width / 2;
            double halfHeight = (double)height / 2;

            // rotate corners
            double cx1 = halfWidth * angleCos;
            double cy1 = halfWidth * angleSin;

            double cx2 = halfWidth * angleCos - halfHeight * angleSin;
            double cy2 = halfWidth * angleSin + halfHeight * angleCos;

            double cx3 = -halfHeight * angleSin;
            double cy3 = halfHeight * angleCos;

            double cx4 = 0;
            double cy4 = 0;

            // recalculate image size
            halfWidth = Math.Max(Math.Max(cx1, cx2), Math.Max(cx3, cx4)) - Math.Min(Math.Min(cx1, cx2), Math.Min(cx3, cx4));
            halfHeight = Math.Max(Math.Max(cy1, cy2), Math.Max(cy3, cy4)) - Math.Min(Math.Min(cy1, cy2), Math.Min(cy3, cy4));

            return new Size((int)(halfWidth * 2 + 0.5), (int)(halfHeight * 2 + 0.5));
        }
    }

    /// <summary>
    /// Interpolation routines.
    /// </summary>
    /// 
    internal static class Interpolation
    {
        /// <summary>
        /// Bicubic kernel.
        /// </summary>
        /// 
        /// <param name="x">X value.</param>
        /// 
        /// <returns>Bicubic coefficient.</returns>
        /// 
        public static double BiCubicKernel(double x)
        {
            if (x > 2.0)
                return 0.0;

            double a, b, c, d;
            double xm1 = x - 1.0;
            double xp1 = x + 1.0;
            double xp2 = x + 2.0;

            a = (xp2 <= 0.0) ? 0.0 : xp2 * xp2 * xp2;
            b = (xp1 <= 0.0) ? 0.0 : xp1 * xp1 * xp1;
            c = (x <= 0.0) ? 0.0 : x * x * x;
            d = (xm1 <= 0.0) ? 0.0 : xm1 * xm1 * xm1;

            return (0.16666666666666666667 * (a - (4.0 * b) + (6.0 * c) - (4.0 * d)));
        }
    }
}
