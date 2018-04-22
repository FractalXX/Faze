using DrawItFast.Model.Drawing;
using DrawItFast.Model.Drawing.Drawables;
using DrawItFast.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawItFast.Model.Tools
{
    abstract class ShapeTool : IDrawTool, IMoveTool
    {
        protected Shape selectedShape;
        protected int grabbedPointIndex;

        public IDrawable SelectedShape
        {
            get
            {
                return this.selectedShape;
            }
        }

        private Color lineColor;
        private Color fillColor;
        private int lineThickness;

        public Color Color1
        {
            get
            {
                return this.lineColor;
            }
            set
            {
                if (this.selectedShape != null)
                {
                    this.selectedShape.LineColor = value.ToRawColor4();
                }
                this.lineColor = value;
            }
        }

        public Color Color2
        {
            get
            {
                return this.fillColor;
            }
            set
            {
                if (this.selectedShape != null)
                {
                    this.selectedShape.FillColor = value.ToRawColor4();
                }
                this.fillColor = value;
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
                if (this.selectedShape != null)
                {
                    this.selectedShape.LineThickness = value;
                }
                this.lineThickness = value;
            }
        }

        public bool Closed
        {
            get
            {
                if (this.selectedShape != null)
                {
                    return this.selectedShape.Closed;
                }
                return false;
            }
            set
            {
                if (this.selectedShape != null)
                {
                    this.selectedShape.Closed = value;
                }
            }
        }

        public bool Filled
        {
            get
            {
                if (this.selectedShape != null)
                {
                    return this.selectedShape.Filled;
                }
                return false;
            }
            set
            {
                if (this.selectedShape != null)
                {
                    this.selectedShape.Filled = value;
                }
            }
        }

        protected ShapeTool()
        {
            this.selectedShape = null;
            this.grabbedPointIndex = -1;
            this.LineThickness = 1;
        }

        public virtual void MouseDown(Point point, MouseEventArgs args)
        {
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                if (this.selectedShape != null)
                {
                    for (int j = 0; j < this.selectedShape.PointCount; j++)
                    {
                        Point p = this.selectedShape.GetPoint(j);
                        if (Math.Abs(p.X - point.X) <= Shape.PointSize && Math.Abs(p.Y - point.Y) <= Shape.PointSize)
                        {
                            this.grabbedPointIndex = j;
                        }
                    }
                }

                if (this.selectedShape == null)
                {
                    Shape shape = this.CreateShape(point, this.lineColor, this.fillColor, this.lineThickness);

                    if (shape != null && this.TrySelectShape(shape))
                    {
                        this.grabbedPointIndex = this.selectedShape.PointCount - 1;
                        MainWindow.Instance.AddShape(shape); 
                    }
                }
            }
        }

        public virtual void MouseMove(Point point, MouseEventArgs args)
        {
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                if (this.selectedShape != null && this.grabbedPointIndex != -1)
                {
                    this.selectedShape.SetPoint(this.grabbedPointIndex, point);
                }
            }
        }

        public virtual void MouseUp(Point point, MouseEventArgs args)
        {
            this.grabbedPointIndex = -1;
        }

        public abstract bool TrySelectShape(IDrawable shape);
        protected abstract Shape CreateShape(Point startPoint, Color lineColor, Color fillColor, int lineThickness);
    }
}
