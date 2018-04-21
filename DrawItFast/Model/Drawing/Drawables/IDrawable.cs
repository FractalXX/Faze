using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DrawItFast.Model.Drawing.Drawables
{
    interface IDrawable
    {
        int PointCount { get; }
        bool IsSelected { get; set; }

        void Draw(RenderTarget target);
        bool IsMouseHovering(Point p);

        void SetPoint(int index, Point p);
        void AddPoint(Point p);
        Point GetPoint(int index);
    }
}
