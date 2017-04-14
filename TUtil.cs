using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TataBuilder
{
    public static class TUtil
    {
        public static float dotProduct(PointF pt1, PointF pt2)
        {
            return pt1.X * pt2.X + pt1.Y * pt2.Y;
        }

        public static float dotProduct(float x1, float y1, float x2, float y2)
        {
            return x1 * x2 + y1 * y2;
        }

        public static PointF rotatePositionAround(PointF center, PointF pos, float angle)
        {
            float x = (float)(center.X + (pos.X - center.X) * Math.Cos(angle) - (pos.Y - center.Y) * Math.Sin(angle));
            float y = (float)(center.Y + (pos.X - center.X) * Math.Sin(angle) + (pos.Y - center.Y) * Math.Cos(angle));
            return new PointF(x, y);
        }

        public static float angleBetweenVectors(PointF u, PointF v)
        {
/*            float a1 = u.X * v.X + u.Y * v.Y;
            float a2 = (float)(Math.Sqrt(u.X * u.X + u.Y * u.Y) * Math.Sqrt(v.X * v.X + v.Y * v.Y));
            if (a2 == 0)
                return 0;
            return (float)Math.Acos(a1 / a2);*/
            return (float)(Math.Atan2(v.Y, v.X) - Math.Atan2(u.Y, u.X));
        }

        public static bool isInPolygon(PointF[] poly, PointF point)
        {
            var poly2 = poly.Concat(poly.Take(1)).ToArray();
            var coef = poly2.Skip(1).Select((p, i) =>
                                            (point.Y - poly2[i].Y) * (p.X - poly2[i].X)
                                          - (point.X - poly2[i].X) * (p.Y - poly2[i].Y))
                                    .ToList();

            if (coef.Any(p => p == 0))
                return true;

            for (int i = 1; i < coef.Count(); i++) {
                if (coef[i] * coef[i - 1] < 0)
                    return false;
            }
            return true;
        }

        // Returns true if the lines intersect, otherwise false. In addition, if the lines 
        // intersect the intersection point may be stored in the floats i_x and i_y.
        public static bool isLinesIntersect(PointF pt1, PointF pt2, PointF pt3, PointF pt4)
        {
            float s1_x, s1_y, s2_x, s2_y;
            s1_x = pt2.X - pt1.X; s1_y = pt2.Y - pt1.Y;
            s2_x = pt4.X - pt3.X; s2_y = pt4.Y - pt3.Y;

            float s, t;
            s = (-s1_y * (pt1.X - pt3.X) + s1_x * (pt1.Y - pt3.Y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = (s2_x * (pt1.Y - pt3.Y) - s2_y * (pt1.X - pt3.X)) / (-s2_x * s1_y + s1_x * s2_y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1) {
                // Collision detected
                float i_x = pt1.X + (t * s1_x);
                float i_y = pt1.Y + (t * s1_y);
                return true;
            }

            return false; // No collision
        }

        public static bool isPolygonsIntersect(PointF[] poly1, PointF[] poly2)
        {
            foreach (PointF pt in poly1) {
                if (isInPolygon(poly2, pt))
                    return true;
            }

            foreach (PointF pt in poly2) {
                if (isInPolygon(poly1, pt))
                    return true;
            }

            for (int i = 0; i < poly1.Count(); i++) {
                // line of first polygon
                PointF pt1 = poly1[i], pt2 = poly1[(i + 1) % poly1.Count()];

                for (int j = 0; j < poly2.Count(); j++) {
                    // line of second polygon
                    PointF pt3 = poly2[j], pt4 = poly2[(j + 1) % poly2.Count()];

                    if (isLinesIntersect(pt1, pt2, pt3, pt4))
                        return true;
                }
            }

            return false;
        }

        public static float distanceBetweenPointLine(PointF pt, PointF linePt1, PointF linePt2)
        {
            //   | (x2 - x1) (y1 - y0) - (x1 - x0) (y2 - y1) |
            // -------------------------------------------------
            //        ,--------------------------------
            //      \/  (x2 - x1) ^ 2 + (y2 - y1) ^ 2
            float x0 = pt.X, y0 = pt.Y;
            float x1 = linePt1.X, y1 = linePt1.Y;
            float x2 = linePt2.X, y2 = linePt2.Y;
            return (float)(Math.Abs((x2 - x1) * (y1 - y0) - (x1 - x0) * (y2 - y1)) / Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)));
        }

        public static float distanceBetweenPoints(PointF pt1, PointF pt2)
        {
            return (float)Math.Sqrt(Math.Pow(pt2.X - pt1.X, 2) + Math.Pow(pt2.Y - pt1.Y, 2));
        }

        public static bool isPointProjectionInLineSegment(PointF pt, PointF linePt1, PointF linePt2)
        {
            float a = dotProduct(pt.X - linePt1.X, pt.Y - linePt1.Y, linePt2.X - linePt1.X, linePt2.Y - linePt1.Y);
            float b = dotProduct(pt.X - linePt2.X, pt.Y - linePt2.Y, linePt1.X - linePt2.X, linePt1.Y - linePt2.Y);
            return a >= 0 && b >= 0;
        }

        public static float normalizeRadianAngle(float angle)
        {
            while (angle >= 2 * Math.PI)
                angle -= (float)(2 * Math.PI);
            while (angle < 0)
                angle += (float)(2 * Math.PI);
            return angle;
        }

        public static float normalizeDegreeAngle(float angle)
        {
            while (angle >= 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }

        public static RectangleF positiveRectangle(RectangleF rect)
        {
            return new RectangleF(Math.Min(rect.Left, rect.Right), Math.Min(rect.Top, rect.Bottom), Math.Abs(rect.Width), Math.Abs(rect.Height));
        }

        public static PointF[] vertexesOfRectangle(RectangleF rect)
        {
            return new PointF[] { 
                new PointF(rect.Left, rect.Top), 
                new PointF(rect.Left, rect.Bottom), 
                new PointF(rect.Right, rect.Bottom), 
                new PointF(rect.Right, rect.Top) 
            };
        }

        public static Color percentColor(Color color, float percent)
        {
            int a = color.A;
            int r = (int)(color.R * percent) % 255;
            int g = (int)(color.G * percent) % 255;
            int b = (int)(color.B * percent) % 255;
            return Color.FromArgb(a, r, g, b);
        }

        public static bool isChildOf(this Control c, Control parent)
        {
            return ((c.Parent != null && c.Parent == parent) || (c.Parent != null ? c.Parent.isChildOf(parent) : false));
        }

        public static Form ownerForm(this Control c)
        {
            if (c is Form)
                return c as Form;
            else if (c.Parent != null)
                return c.Parent.ownerForm();
            else
                return null;
        }

        public static Control findAncestorControl(this Control c, Type type)
        {
            if (c.GetType() == type)
                return c;
            else if (c.Parent != null)
                return c.Parent.findAncestorControl(type);
            else
                return null;
        }

        public static bool isSerializable(object obj)
        {
            MemoryStream mem = new MemoryStream();
            BinaryFormatter bin = new BinaryFormatter();
            try {
                bin.Serialize(mem, obj);
                return true;
            } catch (Exception ex) {
                MessageBox.Show("Your object cannot be serialized." +
                                 " The reason is: " + ex.ToString());
                return false;
            }
        }

        // return the resized image of specified image with size parameter
        // if stretch is true, image will be stretched, else image will keep the ratio.
        public static Image resizedImage(Image image, Size size, bool stretch)
        {
            int width, height;
            if (stretch) {
                width = size.Width;
                height = size.Height;
            } else {
                double s = Math.Min((double)size.Width / image.Width, (double)size.Height / image.Height);
                width = (int)(image.Width * s);
                height = (int)(image.Height * s);
            }

            //a holder for the result
            Bitmap result = new Bitmap(width, height);
            //set the resolutions the same to avoid cropping due to resolution differences
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result)) {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap
            return result;
        }

        public static void copyStream(Stream source, Stream target) 
        {
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
                target.Write(buf, 0, bytesRead);
        }

        public static string getTemporaryDirectory() 
        {
            string tempDirectory;
            do {
                tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            } while (File.Exists(tempDirectory) || Directory.Exists(tempDirectory));

            try {
                Directory.CreateDirectory(tempDirectory);
            } catch (Exception) {
                return null;
            }

            return tempDirectory;
        }

        public static string parseStringXElement(XElement e, string def) 
        {
            if (e == null)
                return def;
            return e.Value;
        }

        public static int parseIntXElement(XElement e, int def)
        {
            int temp;
            if (e == null || !Int32.TryParse(e.Value, out temp))
                return def;
            return temp;
        }

        public static long parseLongXElement(XElement e, long def)
        {
            long temp;
            if (e == null || !long.TryParse(e.Value, out temp))
                return def;
            return temp;
        }

        public static float parseFloatXElement(XElement e, float def)
        {
            float temp;
            if (e == null || !float.TryParse(e.Value, out temp))
                return def;
            return temp;
        }

        public static bool parseBoolXElement(XElement e, bool def)
        {
            bool temp;
            if (e == null || !bool.TryParse(e.Value, out temp))
                return def;
            return temp;
        }
    }
}
