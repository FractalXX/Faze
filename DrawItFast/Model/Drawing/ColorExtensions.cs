using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DrawItFast.Model.Drawing
{
    public static class ColorExtensions
    {
        public static RawColor4 ToRawColor4(this Color color)
        {
            return new RawColor4(color.ScR, color.ScG, color.ScB, color.ScA);
        }

        public static Color ToMediaColor(this RawColor4 color)
        {
            Color result = new Color();
            result.ScR = color.R;
            result.ScG = color.G;
            result.ScB = color.B;
            result.ScA = color.A;
            return result;
        }
    }
}
