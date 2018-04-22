using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawItFast.Model.Drawing
{
    static class MathHelper
    {
        private static float H0(float t)
        {
            return (t - 1) * (t - 1) * (2 * t + 1);
        }

        private static float H1(float t)
        {
            return t * t * (3 - 2 * t);
        }

        private static float H2(float t)
        {
            return (t - 1) * (t - 1) * t;
        }

        private static float H3(float t)
        {
            return (t - 1) * t * t;
        }

        internal static RawVector2 Interpolate(float t, RawVector2 P1, RawVector2 P2, RawVector2 P3, RawVector2 P4)
        {
            RawVector2 p = new RawVector2();
            p.X = H0(t) * P1.X + H1(t) * P3.X + H2(t) * (P2.X - P1.X) + H3(t) * (P4.X - P3.X);
            p.Y = H0(t) * P1.Y + H1(t) * P3.Y + H2(t) * (P2.Y - P1.Y) + H3(t) * (P4.Y - P3.Y);
            return p;
        }

        internal static float B(float t, int n, int i)
        {
            //return (float)(this.NAlattK(n, i) * Math.Pow(1 - t, n - i) * Math.Pow(t, i));

            if (n == 0 && i == 0)
                return 1;
            if (i < 0 || i > n)
                return 0;
            return (1 - t) * B(t, n - 1, i) + t * B(t, n - 1, i - 1);
        }

        internal static RawVector2 ApproximateNext(RawVector2 point, float t, int n, int i)
        {
            return new RawVector2()
            {
                X = point.X + B(t, n, i) * point.X,
                Y = point.Y + B(t, n, i) * point.Y
            };
        }
    }
}
