using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DrawItFast.View.Windows;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DrawItFast.Model.Drawing.Drawables.Curves
{
    class Nubs : Shape
    {
        private float[] knotValues;
        private int degree;

        public override bool IsMouseHovering(Point p)
        {
            return this.IsPointInRange(p, MainWindow.Instance.PointSize);
        }

        public Nubs()
        {
            this.degree = 4;
            this.SetNodeValues();
        }

        private void SetNodeValues()
        {
            this.knotValues = new float[this.points.Count + this.degree];
            //this.knotValues[0] = 0;

            for (int i = 1; i < this.knotValues.Length - 1; i++)
            {
                this.knotValues[i] = i * (1f / (this.knotValues.Length - 1));
            }

            this.knotValues[this.knotValues.Length - 1] = 1;
        }

        public override void Draw(RenderTarget target)
        {
            if (this.points.Count >= this.degree)
            {
                float u = this.knotValues[this.degree - 1];
                float h = 1f / 250;

                float x0 = 0;
                float y0 = 0;

                for (int i = 0; i < this.points.Count; i++)
                {
                    x0 += MathHelper.NubsFunction(this.knotValues, u, i, degree) * this.points[i].X;
                    y0 += MathHelper.NubsFunction(this.knotValues, u, i, degree) * this.points[i].Y;
                }

                float x1;
                float y1;

                SolidColorBrush curveBrush = new SolidColorBrush(target, this.LineColor);
                PathGeometry geometry = new PathGeometry(target.Factory);
                GeometrySink gs = geometry.Open();
                gs.BeginFigure(new RawVector2() { X = x0, Y = y0 }, FigureBegin.Filled);

                while (u < this.knotValues[this.points.Count])
                {
                    u += h;

                    x1 = 0;
                    y1 = 0;

                    for (int i = 0; i < this.points.Count; i++)
                    {
                        x1 += MathHelper.NubsFunction(this.knotValues, u, i, degree) * this.points[i].X;
                        y1 += MathHelper.NubsFunction(this.knotValues, u, i, degree) * this.points[i].Y;
                    }

                    gs.AddLine(new RawVector2() { X = x0, Y = y0 });
                    gs.AddLine(new RawVector2() { X = x1, Y = y1 });

                    x0 = x1;
                    y0 = y1;
                }
                gs.EndFigure(FigureEnd.Open);
                gs.Close();

                target.DrawGeometry(geometry, curveBrush, this.LineThickness);

                gs.Dispose();
                geometry.Dispose();
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

        private bool IsPointInRange(Point p, float range)
        {
            if (this.points.Count >= this.degree)
            {
                float u = this.knotValues[this.degree - 1];
                float h = 1f / 250;

                float x1;
                float y1;

                while (u < this.knotValues[this.points.Count + 1])
                {
                    u += h;

                    x1 = 0;
                    y1 = 0;

                    for (int i = 0; i < this.points.Count; i++)
                    {
                        x1 += MathHelper.NubsFunction(this.knotValues, u, i, degree) * this.points[i].X;
                        y1 += MathHelper.NubsFunction(this.knotValues, u, i, degree) * this.points[i].Y;
                    }

                    if(Math.Abs(p.X - x1) <= range && Math.Abs(p.Y - y1) <= range)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected override void OnAddPoint(Point point)
        {
            this.SetNodeValues();
        }
    }
}
