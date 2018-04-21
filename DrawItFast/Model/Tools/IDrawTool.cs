using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DrawItFast.Model.Tools
{
    interface IDrawTool : ITool
    {
        Color Color1 { get; set; }
        Color Color2 { get; set; }

        int LineThickness { get; set; }
    }
}
