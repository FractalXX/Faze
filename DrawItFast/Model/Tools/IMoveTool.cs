using DrawItFast.Model.Drawing.Drawables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawItFast.Model.Tools
{
    interface IMoveTool : ITool
    {
        IDrawable SelectedShape { get; }
        bool TrySelectShape(IDrawable shape);
    }
}
