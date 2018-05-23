using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        /*internal static float NubsFunction(float[] U, float u, int j, int k)
        {
            if (k == 1)
            {
                if (u >= U[j] && u < U[j + 1])
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            float denom1 = (U[j + k - 1] - U[j]);
            float denom2 = (U[j + k] - U[j + 1]);

            float part1 = 0;
            float part2 = 0;

            if (denom1 != 0)
            {
                part1 = (u - U[j]) / denom1 * NubsFunction(U, u, j, k - 1);
            }

            if (denom2 != 0)
            {
                part2 = (U[j + k] - u) / denom2 * NubsFunction(U, u, j + 1, k - 1);
            }

            return part1 + part2;
        }*/

        private static int GetLowerLimitIndex(float value, float[] knotValues)
        {
            for(int i = 0; i < knotValues.Length - 1; i++)
            {
                if(value >= knotValues[i] && value < knotValues[i + 1])
                {
                    return i;
                }
            }

            throw new ArgumentException("Can't find index for the given value.");
        }

        internal static float[] NubsFunction(int n, int degree, int m, float u, float[] U)
        {
            float[] baseValues = new float[n + 1];
            if(u == U[0])
            {
                baseValues[0] = 1.0f;
            }
            else if(u == U[m - 1])
            {
                baseValues[n - 1] = 1.0f;
            }
            else
            {
                int k = GetLowerLimitIndex(u, U);

                baseValues[k] = 1;

                for(int i = 1; i < degree; i++)
                {
                    baseValues[k - i] = ((U[k + 1] - u) / (U[k + 1] - U[k - i + 1])) * baseValues[k - i + 1];
                    for (int j = k - i + 1; j < k; j++)
                    {
                        float A = (u - U[j]) / (U[j + i] - U[j]) * baseValues[j];
                        float B = (U[j + i + 1] - u) / (U[j + i + 1] - U[j + 1]) * baseValues[j + 1];

                        baseValues[j] = A + B;
                    }
                    baseValues[k] = (u - U[k]) / (U[k + i] - U[k]) * baseValues[k];
                }
            }

            return baseValues;
        }

        internal static Point GetPointOnLine(Point p1, Point p2, int y)
        {
            Point result = new Point();
            result.Y = y;
            if(p1.Y - p2.Y != 0)
            {
                result.X = (result.Y - p1.Y) * (p1.X - p2.X) / (p1.Y - p2.Y) + p1.X;
            }
            return result;
        }
    }
}
