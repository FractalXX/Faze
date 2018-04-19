using DrawItFast.Model.Drawing.Drawables;
using DrawItFast.View.Windows;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DrawItFast.Model
{
    static class ShapeFactory
    {
        public static HermiteCurve CreateHermiteCurve(Point startPoint, RawColor4 lineColor, RawColor4 fillColor, int lineThickness)
        {
            HermiteCurve newCurve = new HermiteCurve();
            newCurve.SetLineStyle(lineColor, lineThickness);
            newCurve.SetFillColor(lineColor);
            newCurve.AddPoint(startPoint);
            newCurve.AddPoint(startPoint);

            MainWindow.Instance.AddShape(newCurve);
            return newCurve;
        }
    }
}
