using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

using DrawItFast.Model.Drawing;

namespace DrawItFast.Model.Drawing.Drawables
{
    class HermiteCurve : IDrawable, IClosable
    {
        private List<RawVector2> points;
        private List<RawVector2> polygon;

        public RawColor4 LineColor { get; private set; }
        public RawColor4 TangentColor { get; private set; }
        public RawColor4 FillColor { get; private set; }

        public int LineThickness { get; private set; }

        public static float headSize;
        public static float headAngle;

        private bool closed;
        private bool filled;

        private float resolution;

        public static RawColor4 DefaultCurveColor;
        public static RawColor4 DefaultTangentColor;
        public static RawColor4 DefaultFillColor;

        public bool IsSelected { get; set; }

        public bool Closed
        {
            get
            {
                return this.closed;
            }
            set
            {
                this.closed = value;
            }
        }

        public bool Filled
        {
            get
            {
                return this.filled;
            }
            set
            {
                this.filled = value;
            }
        }

        public int PointCount
        {
            get
            {
                return this.points.Count;
            }
        }

        static HermiteCurve()
        {
            headSize = 10;
            headAngle = 30;

            DefaultCurveColor = new RawColor4(0.0f, 1.0f, 0.0f, 1.0f);
            DefaultTangentColor = new RawColor4(0.6f, 0.6f, 0.6f, 1.0f);
            DefaultFillColor = new RawColor4(0.0f, 0.0f, 1.0f, 1.0f);
        }

        public HermiteCurve()
        {
            this.points = new List<RawVector2>();
            this.polygon = new List<RawVector2>();
            this.resolution = 250;

            this.closed = false;
            this.filled = false;

            this.SetFillColor(DefaultFillColor);
            this.SetLineStyle(DefaultCurveColor, 1);
            this.SetTangentColor(DefaultTangentColor);
        }

        public void Draw(RenderTarget target)
        {
            if (this.points.Count >= 4)
            {
                SolidColorBrush curveBrush = new SolidColorBrush(target, this.LineColor);
                PathGeometry geometry = new PathGeometry(target.Factory);
                GeometrySink gs = geometry.Open();
                gs.BeginFigure(this.points[0], FigureBegin.Filled);

                for (int i = 0; i < this.points.Count - 3; i += 2)
                {
                    float a = 0;
                    float b = 1;
                    float h = (b - a) / 250;
                    float t = a;
                    RawVector2 P0, P1;

                    P0 = Interpolate(t, this.points[i], this.points[i + 1], this.points[i + 2], this.points[i + 3]);
                    this.polygon.Add(P0);

                    while (t < b)
                    {
                        t += h;

                        P1 = Interpolate(t, this.points[i], this.points[i + 1], this.points[i + 2], this.points[i + 3]);
                        this.polygon.Add(P1);

                        // target.DrawLine(P0, P1, curveBrush);
                        gs.AddLine(P0);
                        gs.AddLine(P1);

                        P0 = P1;
                    }
                }

                gs.EndFigure(FigureEnd.Open);
                gs.Close();

                target.DrawGeometry(geometry, curveBrush);

                if (this.Closed)
                {
                    this.Close(target);
                    if (this.Filled)
                    {
                        this.Fill(target);
                    }
                }

                gs.Dispose();
                geometry.Dispose();
                curveBrush.Dispose();
            }

            if(this.IsSelected)
            {
                this.DrawGuides(target);
            }

            this.polygon.Clear();
        }

        private void DrawGuides(RenderTarget target)
        {
            SolidColorBrush tangentBrush = new SolidColorBrush(target, this.TangentColor);
            for (int i = 0; i < this.points.Count; i += 2)
            {
                target.DrawArrow(tangentBrush, this.points[i], this.points[i + 1], headSize, headAngle);
                foreach (RawVector2 point in this.points)
                {
                    RawRectangleF rectangle = new RawRectangleF();
                }
            }
            tangentBrush.Dispose();
        }

        private void Close(RenderTarget target)
        {
            float a = 0;
            float b = 1;
            float h = (b - a) / this.resolution;
            float t = a;
            RawVector2 P0, P1;

            SolidColorBrush curveBrush = new SolidColorBrush(target, this.LineColor);

            P0 = Interpolate(t, this.points[this.points.Count - 2], this.points[this.points.Count - 1], this.points[0], this.points[1]);
            this.polygon.Add(P0);

            while (t < b)
            {
                t += h;

                P1 = Interpolate(t, this.points[this.points.Count - 2], this.points[this.points.Count - 1], this.points[0], this.points[1]);
                this.polygon.Add(P1);

                target.DrawLine(P0, P1, curveBrush);

                P0 = P1;
            }

            curveBrush.Dispose();
        }

        private void Fill(RenderTarget target)
        {
            //context.FillPolygon(this.brushFill, this.polygon.ToArray());
        }

        public void AddPoint(Point point)
        {
            this.points.Add(new RawVector2() { X = (float)point.X, Y = (float)point.Y });
        }

        public Point GetPoint(int index)
        {
            if (index > this.points.Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            return new Point(this.points[index].X, this.points[index].Y);
        }

        public void SetPoint(int index, Point point)
        {
            if (index > this.points.Count - 1 || index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            this.points[index] = new RawVector2() { X = (float)point.X, Y = (float)point.Y };
        }

        public Point GetLastPoint()
        {
            return new Point(this.points[this.points.Count - 1].X, this.points[this.points.Count - 1].Y);
        }

        public void SetLineStyle(RawColor4 color, int thickness)
        {
            this.LineColor = color;
            this.LineThickness = thickness;
        }

        public void SetTangentColor(RawColor4 color)
        {
            this.TangentColor = color;
        }

        public void SetFillColor(RawColor4 color)
        {
            this.FillColor = color;
        }

        public bool IsMouseHovering(Point p)
        {
            return this.IsPointInRange(p, View.Windows.MainWindow.Instance.InterpolationOffset);
        }

        private bool IsPointInRange(Point p, float range)
        {
            for (int i = 0; i < this.points.Count - 3; i += 2)
            {
                float a = 0;
                float b = 1;
                float h = (b - a) / 250;
                float t = a;
                RawVector2 H;

                while (t < b)
                {
                    t += h;

                    H = Interpolate(t, this.points[i], this.points[i + 1], this.points[i + 2], this.points[i + 3]);
                    if (Math.Abs(p.X - H.X) <= range && Math.Abs(p.Y - H.Y) <= range)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public HermiteCurve Clone()
        {
            HermiteCurve curve = this.MemberwiseClone() as HermiteCurve;
            curve.points = new List<RawVector2>(this.points);
            curve.polygon = new List<RawVector2>(this.polygon);
            curve.SetLineStyle(this.LineColor, this.LineThickness);
            curve.SetFillColor(this.FillColor);
            curve.SetTangentColor(this.TangentColor);

            return curve;
        }

        private static float H0(float t)
        {
            return (t - 1) * (t - 1) * (2 * t + 1);
        }

        private static float H1(float t)
        {
            return t * t * (3 - 2 * t);
        }

        private static float H2(float t)
        {
            return (t - 1) * (t - 1) * t;
        }

        private static float H3(float t)
        {
            return (t - 1) * t * t;
        }

        internal static RawVector2 Interpolate(float t, RawVector2 P1, RawVector2 P2, RawVector2 P3, RawVector2 P4)
        {
            RawVector2 p = new RawVector2();
            p.X = H0(t) * P1.X + H1(t) * P3.X + H2(t) * (P2.X - P1.X) + H3(t) * (P4.X - P3.X);
            p.Y = H0(t) * P1.Y + H1(t) * P3.Y + H2(t) * (P2.Y - P1.Y) + H3(t) * (P4.Y - P3.Y);
            return p;
        }
    }
}
