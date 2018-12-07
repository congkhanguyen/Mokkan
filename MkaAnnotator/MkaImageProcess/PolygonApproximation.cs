using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MokkAnnotator.MkaDrawTools;

namespace MokkAnnotator.MkaImageProcess
{
    class PolygonApproximation
    {
        /// <summary>
        /// Reduce the number of points in a polygon.
        /// </summary>
        /// <param name="Points">The points.</param>
        /// <param name="Tolerance">The tolerance.</param>
        /// <returns></returns>
        public static DrawPolygon ReductionPolygon(List<PointF> Points, Double Tolerance)
        {
            Int32 firstPoint = 0;
            Int32 lastPoint = Points.Count - 1;
            List<Int32> pointIndexsToKeep = new List<Int32>();

            //Add the first and last index to the keepers
            pointIndexsToKeep.Add(firstPoint);
            pointIndexsToKeep.Add(lastPoint);

            //The first and the last PointF can not be the same
            while (Points[firstPoint].Equals(Points[lastPoint]))
            {
                lastPoint--;
            }

            Reduction(Points, firstPoint, lastPoint, Tolerance, ref pointIndexsToKeep);

            DrawPolygon returnPolygon = new DrawPolygon();
            Point point;
            pointIndexsToKeep.Sort();
            foreach (Int32 index in pointIndexsToKeep)
            {
                point = new Point();
                point.X = (int)Points[index].X;
                point.Y = (int)Points[index].Y;
                returnPolygon.AddPoint(point);
            }

            return returnPolygon;
        }

        /// <summary>
        /// Reduction points
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="firstPoint">The first PointF.</param>
        /// <param name="lastPoint">The last PointF.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <param name="pointIndexsToKeep">The PointF indexs to keep.</param>
        private static void Reduction(List<PointF> points, Int32 firstPoint, Int32 lastPoint, Double tolerance, ref List<Int32> pointIndexsToKeep)
        {
            Double maxDistance = 0;
            Int32 indexFarthest = 0;

            for (Int32 index = firstPoint; index < lastPoint; index++)
            {
                Double distance = PerpendicularDistance(points[firstPoint], points[lastPoint], points[index]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    indexFarthest = index;
                }
            }

            if (maxDistance > tolerance && indexFarthest != 0)
            {
                //Add the largest PointF that exceeds the tolerance
                pointIndexsToKeep.Add(indexFarthest);

                Reduction(points, firstPoint, indexFarthest, tolerance, ref pointIndexsToKeep);
                Reduction(points, indexFarthest, lastPoint, tolerance, ref pointIndexsToKeep);
            }
        }

        /// <summary>
        /// The distance of a PointF from a line made from point1 and point2.
        /// </summary>
        /// <param name="pt1">The PT1.</param>
        /// <param name="pt2">The PT2.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public static Double PerpendicularDistance(PointF Point1, PointF Point2, PointF PointF)
        {
            //Area = |(1/2)(x1y2 + x2y3 + x3y1 - x2y1 - x3y2 - x1y3)|   *Area of triangle
            //Base = √((x1-x2)²+(x1-x2)²)                              *Base of Triangle
            //Area = .5*Base*H                                          *Solve for height
            //Height = Area/.5/Base

            Double area = Math.Abs(.5 * (Point1.X * Point2.Y + Point2.X * PointF.Y + PointF.X * Point1.Y - Point2.X * Point1.Y - PointF.X * Point2.Y - Point1.X * PointF.Y));
            Double bottom = Math.Sqrt(Math.Pow(Point1.X - Point2.X, 2) + Math.Pow(Point1.Y - Point2.Y, 2));
            Double height = area / bottom * 2;

            return height; 
        }
    }
}
