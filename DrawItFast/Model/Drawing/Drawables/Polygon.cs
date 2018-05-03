using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DrawItFast.View.Windows;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DrawItFast.Model.Drawing.Drawables
{
    class Polygon : Shape
    {
        public override bool IsMouseHovering(Point p)
        {
            for(int i = 0; i < this.PointCount - 1; i++)
            {
                float slope = (this.points[i + 1].Y - this.points[i].Y) / (this.points[i + 1].X - this.points[i].X);
                float param = this.points[i].Y - slope * this.points[i].X;
                float upper = (float)Math.Abs(p.Y - slope * p.X - param);
                float denom = (float)Math.Sqrt(1 + slope * slope);
                float distance = upper / denom;

                if(distance <= MainWindow.Instance.PointSize)
                {
                    return true;
                }
            }

            return false;
        }

        public override void Draw(RenderTarget target)
        {
            SolidColorBrush polyBrush = new SolidColorBrush(target, this.LineColor);
            PathGeometry geometry = new PathGeometry(target.Factory);
            GeometrySink gs = geometry.Open();
            gs.BeginFigure(this.points[0], FigureBegin.Filled);

            for (int i = 0; i < this.PointCount - 1; i++)
            {
                gs.AddLine(new RawVector2() { X = this.points[i].X, Y = this.points[i].Y });
                gs.AddLine(new RawVector2() { X = this.points[i + 1].X, Y = this.points[i + 1].Y });
            }

            if(this.Closed)
            {
                gs.EndFigure(FigureEnd.Closed);
            }
            else
            {
                gs.EndFigure(FigureEnd.Open);
            }

            gs.Close();

            target.DrawGeometry(geometry, polyBrush);

            if (this.Filled)
            {
                SolidColorBrush fillBrush = new SolidColorBrush(target, this.FillColor);
                target.FillGeometry(geometry, fillBrush);
                fillBrush.Dispose();
            }

            gs.Dispose();
            geometry.Dispose();
            polyBrush.Dispose();

            base.Draw(target);
        }
    }
}
