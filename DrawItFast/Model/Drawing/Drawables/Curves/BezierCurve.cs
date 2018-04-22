using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DrawItFast.Model.Drawing.Drawables.Curves
{
    class BezierCurve : Shape
    {
        private float resolution;

        public BezierCurve()
        {
            this.resolution = 100;
        }

        public override void Draw(RenderTarget target)
        {
            if (this.points.Count > 2)
            {
                int n = this.points.Count - 1;

                float t = 0;
                float h = 1 / this.resolution;
                float x0 = 0, y0 = 0, x1, y1;

                SolidColorBrush curveBrush = new SolidColorBrush(target, this.LineColor);
                PathGeometry geometry = new PathGeometry(target.Factory);
                GeometrySink gs = geometry.Open();
                gs.BeginFigure(this.points[0], FigureBegin.Filled);

                for (int i = 0; i <= n; i++)
                {
                    x0 += MathHelper.B(t, n, i) * this.points[i].X;
                    y0 += MathHelper.B(t, n, i) * this.points[i].Y;

                }

                while (t < 1)
                {
                    t += h;

                    x1 = 0;
                    y1 = 0;

                    for (int i = 0; i <= n; i++)
                    {
                        x1 += MathHelper.B(t, n, i) * this.points[i].X;
                        y1 += MathHelper.B(t, n, i) * this.points[i].Y;
                    }

                    gs.AddLine(new RawVector2() { X = x0, Y = y0 });
                    gs.AddLine(new RawVector2() { X = x1, Y = y1 });

                    x0 = x1;
                    y0 = y1;
                }

                gs.EndFigure(FigureEnd.Open);
                gs.Close();

                target.DrawGeometry(geometry, curveBrush);
                curveBrush.Dispose();
            }

            base.Draw(target);
        }

        protected override void DrawGuides(RenderTarget target)
        {
            SolidColorBrush brush = new SolidColorBrush(target, this.GuideColor);
            for (int i = 0; i < this.points.Count - 1; i++)
            {
                target.DrawLine(this.points[i], this.points[i + 1], brush);
            }
            brush.Dispose();

            base.DrawGuides(target);
        }

        public override bool IsMouseHovering(Point p)
        {
            return this.IsPointInRange(p, View.Windows.MainWindow.Instance.InterpolationOffset);
        }

        private bool IsPointInRange(Point p, float range)
        {
            int n = this.points.Count - 1;

            float t = 0;
            float h = 1 / this.resolution;
            RawVector2 P0 = new RawVector2();

            for (int i = 0; i <= n; i++)
            {
                P0 = MathHelper.ApproximateNext(P0, t, n, i);
            }

            while (t < 1)
            {
                if (Math.Abs(p.X - P0.X) <= range && Math.Abs(p.Y - P0.Y) <= range)
                {
                    return true;
                }

                t += h;

                for (int i = 0; i <= n; i++)
                {
                    P0 = MathHelper.ApproximateNext(P0, t, n, i);
                }
            }

            return false;
        }

        public BezierCurve Clone()
        {
            BezierCurve curve = this.MemberwiseClone() as BezierCurve;
            curve.points = new List<RawVector2>(this.points);

            return curve;
        }
    }
}
