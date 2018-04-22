using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

using DrawItFast.Model.Drawing;

namespace DrawItFast.Model.Drawing.Drawables.Curves
{
    class HermiteCurve : Shape
    {
        public static float HeadSize;
        public static float HeadAngle;

        private float resolution;

        static HermiteCurve()
        {
            HeadSize = 10;
            HeadAngle = 30;
        }

        public HermiteCurve()
        {
            this.resolution = 250;
        }

        public override void Draw(RenderTarget target)
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
                    float h = (b - a) / this.resolution;
                    float t = a;
                    RawVector2 P0, P1;

                    P0 = MathHelper.Interpolate(t, this.points[i], this.points[i + 1], this.points[i + 2], this.points[i + 3]);

                    while (t < b)
                    {
                        t += h;

                        P1 = MathHelper.Interpolate(t, this.points[i], this.points[i + 1], this.points[i + 2], this.points[i + 3]);

                        // target.DrawLine(P0, P1, curveBrush);
                        gs.AddLine(P0);
                        gs.AddLine(P1);

                        P0 = P1;
                    }
                }

                if (this.Closed)
                {
                    this.Close(target, gs);
                    gs.EndFigure(FigureEnd.Closed);
                    gs.Close();
                    if (this.Filled)
                    {
                        SolidColorBrush fillBrush = new SolidColorBrush(target, this.FillColor);
                        target.FillGeometry(geometry, fillBrush);
                        fillBrush.Dispose();
                    }
                }
                else
                {
                    gs.EndFigure(FigureEnd.Open);
                    gs.Close();
                }

                target.DrawGeometry(geometry, curveBrush, this.LineThickness);

                gs.Dispose();
                geometry.Dispose();
                curveBrush.Dispose();
            }

            base.Draw(target);
        }

        protected override void DrawGuides(RenderTarget target)
        {
            SolidColorBrush tangentBrush = new SolidColorBrush(target, this.GuideColor);
            for (int i = 0; i < this.points.Count; i += 2)
            {
                target.DrawArrow(tangentBrush, this.points[i], this.points[i + 1], HeadSize, HeadAngle);
            }
            tangentBrush.Dispose();

            base.DrawGuides(target);
        }

        private void Close(RenderTarget target, GeometrySink gs)
        {
            float a = 0;
            float b = 1;
            float h = (b - a) / this.resolution;
            float t = a;
            RawVector2 P0, P1;

            SolidColorBrush curveBrush = new SolidColorBrush(target, this.LineColor);

            P0 = MathHelper.Interpolate(t, this.points[this.points.Count - 2], this.points[this.points.Count - 1], this.points[0], this.points[1]);

            while (t < b)
            {
                t += h;

                P1 = MathHelper.Interpolate(t, this.points[this.points.Count - 2], this.points[this.points.Count - 1], this.points[0], this.points[1]);
                gs.AddLine(P0);
                gs.AddLine(P1);

                P0 = P1;
            }

            curveBrush.Dispose();
        }

        public override bool IsMouseHovering(Point p)
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

                    H = MathHelper.Interpolate(t, this.points[i], this.points[i + 1], this.points[i + 2], this.points[i + 3]);
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

            return curve;
        }
    }
}
