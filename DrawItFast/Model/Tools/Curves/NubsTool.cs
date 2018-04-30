using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DrawItFast.Model.Drawing;
using DrawItFast.Model.Drawing.Drawables;
using DrawItFast.Model.Drawing.Drawables.Curves;

namespace DrawItFast.Model.Tools.Curves
{
    class NubsTool : ShapeTool
    {
        protected override Shape CreateShape(Point startPoint, Color lineColor, Color fillColor, int lineThickness)
        {
            Nubs newCurve = new Nubs();
            newCurve.LineColor = lineColor.ToRawColor4();
            newCurve.FillColor = fillColor.ToRawColor4();
            newCurve.LineThickness = lineThickness;
            newCurve.AddPoint(startPoint);

            return newCurve;
        }

        public override bool TrySelectShape(IDrawable shape)
        {
            if (shape is Nubs)
            {
                if (this.selectedShape != null)
                {
                    this.selectedShape.IsSelected = false;
                }

                this.selectedShape = shape as Nubs;

                if (shape != null)
                {
                    this.selectedShape.IsSelected = true;
                }
                return true;
            }
            return false;
        }

        public override void MouseDown(Point point, MouseEventArgs args)
        {
            base.MouseDown(point, args);
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                if (this.selectedShape != null && this.grabbedPointIndex == -1)
                {
                    this.selectedShape.AddPoint(point);
                    this.grabbedPointIndex = this.selectedShape.PointCount - 1;
                }
            }
        }
    }
}
