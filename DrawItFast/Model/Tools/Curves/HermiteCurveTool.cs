using DrawItFast.Model.Drawing.Drawables;
using DrawItFast.View.Windows;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using DrawItFast.Model.Drawing;
using DrawItFast.Model.Drawing.Drawables.Curves;

namespace DrawItFast.Model.Tools.Curves
{
    class HermiteCurveTool : ShapeTool
    {
        public override Type ShapeType
        {
            get
            {
                return typeof(HermiteCurve);
            }
        }

        public override void MouseDown(Point point, MouseEventArgs args)
        {
            base.MouseDown(point, args);
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                if (this.selectedShape != null)
                {
                    if (this.grabbedPointIndex == -1)
                    {
                        this.selectedShape.AddPoint(point);
                        this.selectedShape.AddPoint(point);
                        this.grabbedPointIndex = this.selectedShape.PointCount - 1;
                    }
                }
            }
        }

        protected override Shape CreateShape(Point startPoint, Color lineColor, Color fillColor, int lineThickness)
        {
            HermiteCurve newCurve = new HermiteCurve();
            newCurve.LineColor = lineColor.ToRawColor4();
            newCurve.FillColor = fillColor.ToRawColor4();
            newCurve.LineThickness = lineThickness;
            newCurve.AddPoint(startPoint);
            newCurve.AddPoint(startPoint);

            return newCurve;
        }
    }
}
