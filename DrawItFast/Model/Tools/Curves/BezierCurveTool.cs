﻿using DrawItFast.Model.Drawing;
using DrawItFast.Model.Drawing.Drawables;
using DrawItFast.Model.Drawing.Drawables.Curves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawItFast.Model.Tools.Curves
{
    class BezierCurveTool : ShapeTool
    {
        public override Type ShapeType
        {
            get
            {
                return typeof(BezierCurve);
            }
        }

        public override void MouseDown(Point point, MouseEventArgs args)
        {
            base.MouseDown(point, args);
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                if (this.selectedShape != null && this.grabbedPointIndex == -1)
                {
                    int degree = (this.selectedShape as BezierCurve).Degree;
                    if(this.selectedShape.PointCount > degree && this.selectedShape.PointCount % degree == 1)
                    {
                        this.selectedShape.AddPoint(MathHelper.GetPointOnLine(this.selectedShape.GetPoint(this.selectedShape.PointCount - 2), this.selectedShape.GetPoint(this.selectedShape.PointCount - 1), (int)point.Y));
                    }
                    else
                    {
                        this.selectedShape.AddPoint(point);
                    }
                    this.grabbedPointIndex = this.selectedShape.PointCount - 1;
                }
            }
        }

        public override void MouseMove(Point point, MouseEventArgs args)
        {
            if(this.grabbedPointIndex != -1)
            {
                int degree = (this.selectedShape as BezierCurve).Degree;
                if (this.selectedShape.PointCount > degree && this.grabbedPointIndex % degree == 1)
                {
                    this.selectedShape.SetPoint(
                        this.grabbedPointIndex,
                        MathHelper.GetPointOnLine(this.selectedShape.GetPoint(this.selectedShape.PointCount - 2), this.selectedShape.GetPoint(this.selectedShape.PointCount - 1), (int)point.Y));
                }
                else
                {
                    base.MouseMove(point, args);
                }
            }
        }

        protected override Shape CreateShape(Point startPoint, Color lineColor, Color fillColor, int lineThickness)
        {
            BezierCurve newCurve = new BezierCurve();
            newCurve.LineColor = lineColor.ToRawColor4();
            newCurve.FillColor = fillColor.ToRawColor4();
            newCurve.LineThickness = lineThickness;
            newCurve.AddPoint(startPoint);

            return newCurve;
        }
    }
}
