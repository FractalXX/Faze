using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawItFast.Model.Tools
{
    interface IDrawTool : ITool
    {
        RawColor4 Color1 { get; set; }
        RawColor4 Color2 { get; set; }

        int LineThickness { get; set; }
    }
}
