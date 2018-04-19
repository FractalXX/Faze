using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using DrawItFast.Model.Drawing;
using DrawItFast.Model.Drawing.Drawables;
using SharpDX.Mathematics.Interop;

namespace DrawItFast.Model.Tools
{
    interface ITool
    {
        void MouseDown(Point point, MouseEventArgs args);
        void MouseMove(Point point, MouseEventArgs args);
        void MouseUp(Point point, MouseEventArgs args);
    }
}
