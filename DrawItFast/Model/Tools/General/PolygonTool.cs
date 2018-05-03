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

namespace DrawItFast.Model.Tools.General
{
    class PolygonTool : ShapeTool
    {
        public override bool TrySelectShape(IDrawable shape)
        {
            if (shape is Polygon)
            {
                if (this.selectedShape != null)
                {
                    this.selectedShape.IsSelected = false;
                }

                this.selectedShape = shape as Polygon;

                if (shape != null)
                {
                    this.selectedShape.IsSelected = true;
                }
                return true;
            }
            return false;
        }

        protected override Shape CreateShape(Point startPoint, Color lineColor, Color fillColor, int lineThickness)
        {
            Polygon newPolygon = new Polygon();
            newPolygon.LineColor = lineColor.ToRawColor4();
            newPolygon.FillColor = fillColor.ToRawColor4();
            newPolygon.LineThickness = lineThickness;
            newPolygon.AddPoint(startPoint);

            return newPolygon;
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
