using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DrawItFast.Model.Drawing.Drawables
{
    abstract class Shape : IDrawable
    {
        protected List<RawVector2> points;

        public RawColor4 LineColor { get; set; }
        public RawColor4 GuideColor { get; set; }
        public RawColor4 FillColor { get; set; }

        public int LineThickness { get; set; }

        private bool closed;
        private bool filled;

        private float resolution;

        public static RawColor4 DefaultLineColor;
        public static RawColor4 DefaultGuideColor;
        public static RawColor4 DefaultFillColor;

        public static int PointSize;

        public bool IsSelected { get; set; }

        public bool Closed
        {
            get
            {
                return this.closed;
            }
            set
            {
                this.closed = value;
            }
        }

        public bool Filled
        {
            get
            {
                return this.filled;
            }
            set
            {
                this.filled = value;
            }
        }

        public int PointCount
        {
            get
            {
                return this.points.Count;
            }
        }

        static Shape()
        {
            DefaultLineColor = new RawColor4(0.0f, 1.0f, 0.0f, 1.0f);
            DefaultGuideColor = new RawColor4(0.6f, 0.6f, 0.6f, 1.0f);
            DefaultFillColor = new RawColor4(0.0f, 0.0f, 1.0f, 1.0f);

            PointSize = 5;
        }

        protected Shape()
        {
            this.FillColor = DefaultFillColor;
            this.LineColor = DefaultLineColor;
            this.GuideColor = DefaultGuideColor;

            this.points = new List<RawVector2>();
        }

        public void AddPoint(Point point)
        {
            this.points.Add(new RawVector2() { X = (float)point.X, Y = (float)point.Y });
        }

        public Point GetPoint(int index)
        {
            if (index > this.points.Count - 1)
            {
                throw new IndexOutOfRangeException();
            }

            return new Point(this.points[index].X, this.points[index].Y);
        }

        public void SetPoint(int index, Point point)
        {
            if (index > this.points.Count - 1 || index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            this.points[index] = new RawVector2() { X = (float)point.X, Y = (float)point.Y };
        }

        public Point GetLastPoint()
        {
            return new Point(this.points[this.points.Count - 1].X, this.points[this.points.Count - 1].Y);
        }

        public abstract bool IsMouseHovering(Point p);

        public virtual void Draw(RenderTarget target)
        {
            if(this.IsSelected)
            {
                this.DrawGuides(target);
            }
        }

        protected virtual void DrawGuides(RenderTarget target)
        {
            if (this.IsSelected)
            {
                SolidColorBrush brush = new SolidColorBrush(target, this.GuideColor);
                for (int i = 0; i < this.points.Count; i++)
                {
                    target.FillRectangle(new RawRectangleF(this.points[i].X - PointSize / 2, this.points[i].Y - PointSize / 2, this.points[i].X + PointSize, this.points[i].Y + PointSize), brush);
                }
            }
        }
    }
}
