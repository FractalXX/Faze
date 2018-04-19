using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DrawItFast.Model.Drawing.Drawables;
using SharpDX.Mathematics.Interop;

namespace DrawItFast.Model.Tools.General
{
    class BasicMoveTool : IMoveTool
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

        public BasicMoveTool()
        {
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
                        this.grabbedPointIndex = this.selectedShape.PointCount - 1;
                    }
                }
            }
        }

        public void MouseMove(Point point, MouseEventArgs args)
        {
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                if (this.selectedShape != null)
                {
                    if (this.grabbedPointIndex != -1)
                    {
                        this.selectedShape.SetPoint(this.grabbedPointIndex, point);
                    }
                    else
                    {
                        for (int i = 0; i < this.selectedShape.PointCount; i++)
                        {
                            Point p = this.selectedShape.GetPoint(i);
                            this.selectedShape.SetPoint(i, new Point(p.X + p.X - point.X, p.Y + p.Y - point.Y));
                        }
                    }
                }
            }
        }

        public void MouseUp(Point point, MouseEventArgs args)
        {
            this.grabbedPointIndex = -1;   
        }

        public bool TrySelectShape(IDrawable shape)
        {
            this.selectedShape = shape;
            return true;
        }
    }
}
