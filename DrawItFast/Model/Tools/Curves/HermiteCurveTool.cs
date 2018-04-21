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

namespace DrawItFast.Model.Tools.Curves
{
    class HermiteCurveTool : IDrawTool, IMoveTool
    {
        private IDrawable selectedShape;
        private int grabbedPointIndex;

        public IDrawable SelectedShape
        {
            get
            {
                return this.selectedShape;
            }
        }

        private RawColor4 lineColor;
        private RawColor4 fillColor;
        private int lineThickness;

        public Color Color1
        {
            get
            {
                return this.lineColor.ToMediaColor();
            }
            set
            {
                this.lineColor = value.ToRawColor4();
            }
        }

        public Color Color2
        {
            get
            {
                return this.fillColor.ToMediaColor();
            }
            set
            {
                this.fillColor = value.ToRawColor4();
            }
        }

        public int LineThickness
        {
            get
            {
                return this.lineThickness;
            }
            set
            {
                this.lineThickness = value;
            }
        }

        public HermiteCurveTool()
        {
            this.selectedShape = null;
            this.grabbedPointIndex = -1;
        }

        public void MouseDown(Point point, MouseEventArgs args)
        {
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                if (this.selectedShape != null)
                {
                    bool grabbed = false;
                    for (int j = 0; j < this.selectedShape.PointCount; j++)
                    {
                        Point p = this.selectedShape.GetPoint(j);
                        if (Math.Abs(p.X - point.X) <= 5 && Math.Abs(p.Y - point.Y) <= 5)
                        {
                            this.grabbedPointIndex = j;
                            grabbed = true;
                        }
                    }

                    if (!grabbed)
                    {
                        this.selectedShape.AddPoint(point);
                        this.selectedShape.AddPoint(point);
                        this.grabbedPointIndex = this.selectedShape.PointCount - 1;
                    }
                }


                if (this.selectedShape == null)
                {
                    HermiteCurve curve = ShapeFactory.CreateHermiteCurve(point, this.lineColor, this.fillColor, this.lineThickness);
                    this.TrySelectShape(curve);
                    this.grabbedPointIndex = this.selectedShape.PointCount - 1;
                }
            }
        }

        public void MouseMove(Point point, MouseEventArgs args)
        {
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                if (this.selectedShape != null && this.grabbedPointIndex != -1)
                {
                    this.selectedShape.SetPoint(this.grabbedPointIndex, point);
                }
            }
        }

        public void MouseUp(Point point, MouseEventArgs args)
        {
            this.grabbedPointIndex = -1;
        }

        public bool TrySelectShape(IDrawable shape)
        {
            if (shape is HermiteCurve)
            {
                if (this.selectedShape != null)
                {
                    this.selectedShape.IsSelected = false;
                }

                this.selectedShape = shape as HermiteCurve;

                if (shape != null)
                {
                    this.selectedShape.IsSelected = true;
                }
                return true;
            }
            return false;
        }
    }
}
