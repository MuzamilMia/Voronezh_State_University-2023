using Doll_Managment_Project.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Doll_Managment_Project
{
    public enum FacewearType
    {
        None,
        Glasses,
        SkiMask,
        HockeyMask,
        Cap,
        newglass,
        nofacwear
    }

    public class DollRenderer
    {
        private Color _shirtColor = Color.Blue;
        private Color _skirtColor = Color.Green;
        private Bitmap baseImage;
        private Bitmap currentImage;
        private readonly Color ShirtBaseColor = Color.FromArgb(150, 120, 190);
        private readonly Color SkirtBaseColor = Color.FromArgb(215, 190, 160);
        private readonly int ShirtYMin = 190, ShirtYMax = 850;
        private readonly int SkirtYMin = 585, SkirtYMax = 850;
        private readonly int ColorTolerance = 60;
        private Bitmap facewearImage;
        private int facewearX = 140;
        private int facewearY = 80;
        private int facewearWidth, facewearHeight;


        private List<(Point Location, SparkleShape Shape)> sparkles = new List<(Point, SparkleShape)>();
        public SparkleShape SelectedSparkleShape { get; set; } = SparkleShape.Diamond;
        public int ImageWidth => baseImage.Width;
        public int ImageHeight => baseImage.Height;
        public DollRenderer()
        {
            try
            {
                baseImage = new Bitmap(Resources.girldoll);
                facewearImage = null;
                UpdateImageColors();
            }
            catch
            {
                baseImage = new Bitmap(400, 500);
                using (Graphics g = Graphics.FromImage(baseImage))
                {
                    g.Clear(Color.White);
                    g.DrawString("Image missing", new Font("Arial", 12), Brushes.Red, 10, 10);
                }
            }
        }
        public void SetFacewear(FacewearType type)
        {
            facewearImage?.Dispose();
            switch (type)
            {
                case FacewearType.None:
                    facewearImage = null;
                    break;
                case FacewearType.Glasses:
                    facewearImage = new Bitmap(Resources.sunglasses);
                    facewearX = 435;
                    facewearY = 140;
                    facewearWidth = 150;
                    facewearHeight = 60;
                    break;
                case FacewearType.newglass:
                    facewearImage = new Bitmap(Resources.newglass);
                    facewearX = 440;
                    facewearY = 115;
                    facewearWidth =130;
                    facewearHeight = 120;
                    break;
                case FacewearType.SkiMask:
                    facewearImage = new Bitmap(Resources.ski_mask);
                    facewearX = 350;
                    facewearY = 60;
                    facewearWidth = 320;
                    facewearHeight = 250;
                    break;
                case FacewearType.HockeyMask:
                    facewearImage = new Bitmap(Resources.hockey_mask);
                    facewearX = 350;
                    facewearY = 60;
                    facewearWidth = 320;
                    facewearHeight = 280;
                    break;
                case FacewearType.Cap:
                    facewearImage = new Bitmap(Resources.mycap);
                    facewearX = 390;
                    facewearY = 10;
                    facewearWidth = 250;
                    facewearHeight = 250;
                    break;
                case FacewearType.nofacwear:
                    facewearImage = new Bitmap(Resources.no_facewear);
                    facewearX = 420;
                    facewearY = 135;
                    facewearWidth = 180;
                    facewearHeight = 130;
                    break;
            }
        }
        public bool AddSparkle(Point location)
        {
            Point[] shirtPoints = new Point[]
            {
                new Point(458, 249),
                new Point(565, 253),
                new Point(598, 563),
                new Point(423, 563)
            };

            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddPolygon(shirtPoints);
                if (path.IsVisible(location))
                {
                    sparkles.Add((location, SelectedSparkleShape));
                    return true;
                }
            }
            return false;
        }


        public bool ClearSparkles()
        {
            if(sparkles.Count > 0)
            { 
                sparkles.RemoveAt(sparkles.Count - 1);
                return true;
            }
            return false;
        }
        public Color ShirtColor
        {
            get => _shirtColor;
            set { _shirtColor = value; UpdateImageColors(); }
        }
        public Color SkirtColor
        {
            get => _skirtColor;
            set { _skirtColor = value; UpdateImageColors(); }
        }
        public void DrawDoll(Graphics g, int width, int height)
        {
            // Calculate scaling factors
            float scaleX = (float)width / baseImage.Width;
            float scaleY = (float)height / baseImage.Height;

            // Draw the base doll image (scaled)
            g.DrawImage(currentImage, 0, 0, width, height);

            // Draw facewear if exists (scaled)
            if (facewearImage != null)
            {
                g.DrawImage(facewearImage,
                    facewearX * scaleX,
                    facewearY * scaleY,
                    facewearWidth * scaleX,
                    facewearHeight * scaleY);
            }

            foreach (var sparkle in sparkles)
            {
                float x = sparkle.Location.X * scaleX;
                float y = sparkle.Location.Y * scaleY;

                using (var brush = new SolidBrush(SparkleColor.Pink))
                {
                    switch (sparkle.Shape)
                    {
                        case SparkleShape.Diamond:
                            PointF[] diamond = new PointF[]
                            {
                                new PointF(x, y - 6),
                                new PointF(x + 6, y),
                                new PointF(x, y + 6),
                                new PointF(x - 6, y)
                            };
                            g.FillPolygon(brush, diamond);
                            g.DrawPolygon(Pens.White, diamond);
                            break;

                        case SparkleShape.Circle:
                            g.FillEllipse(brush, x - 6, y - 6, 12, 12);
                            g.DrawEllipse(Pens.White, x - 6, y - 6, 12, 12);
                            break;

                        case SparkleShape.Heart:
                            using (GraphicsPath heartPath = CreateHeartShape(x, y))
                            {
                                g.FillPath(brush, heartPath);
                                g.DrawPath(Pens.White, heartPath);

                            }
                            break;
                    }
                }
            }



        }
        private System.Drawing.Drawing2D.GraphicsPath CreateHeartShape(float x, float y)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();

            // Simple heart shape with two arcs and a triangle
            RectangleF leftArc = new RectangleF(x - 6, y - 6, 6, 6);
            RectangleF rightArc = new RectangleF(x, y - 6, 6, 6);
            PointF bottom = new PointF(x, y + 6);

            path.AddArc(leftArc, 135, 180);
            path.AddArc(rightArc, 225, 180);
            path.AddLine(x - 3, y + 3, x, y + 6);
            path.AddLine(x + 3, y + 3, x, y + 6);

            return path;
        }

        private void UpdateImageColors()
        {
            using (Bitmap original = new Bitmap(baseImage))
            {
                currentImage?.Dispose();
                currentImage = new Bitmap(original.Width, original.Height);
                for (int x = 0; x < original.Width; x++)
                {
                    for (int y = 0; y < original.Height; y++)
                    {
                        Color pixel = original.GetPixel(x, y);
                        if (y >= ShirtYMin && y <= ShirtYMax &&
                            IsColorCloseTo(pixel, ShirtBaseColor, ColorTolerance))
                        {
                            currentImage.SetPixel(x, y, _shirtColor);
                        }
                        else if (y >= SkirtYMin && y <= SkirtYMax &&
                                 IsColorCloseTo(pixel, SkirtBaseColor, ColorTolerance))
                        {
                            currentImage.SetPixel(x, y, _skirtColor);
                        }
                        else
                            currentImage.SetPixel(x, y, pixel);
                    }
                }
            }
        }
        private bool IsColorCloseTo(Color color, Color baseColor, int tolerance)
        {
            return Math.Abs(color.R - baseColor.R) < tolerance &&
                   Math.Abs(color.G - baseColor.G) < tolerance &&
                   Math.Abs(color.B - baseColor.B) < tolerance;
        }
    }
}