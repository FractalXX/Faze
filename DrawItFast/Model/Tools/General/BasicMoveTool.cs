using System;
using System.Collections;
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

        private Point mouseBuffer;

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
                    for (int j = 0; j < this.selectedShape.PointCount && this.grabbedPointIndex == -1; j++)
                    {
                        Point p = this.selectedShape.GetPoint(j);
                        if (Math.Abs(p.X - point.X) <= 5 && Math.Abs(p.Y - point.Y) <= 5)
                        {
                            this.grabbedPointIndex = j;
                        }
                    }
                }
            }

            this.mouseBuffer = point;
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
                            this.selectedShape.SetPoint(i, new Point(p.X + point.X - mouseBuffer.X, p.Y + point.Y - mouseBuffer.Y));
                        }
                    }
                }
            }

            this.mouseBuffer = point;
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
