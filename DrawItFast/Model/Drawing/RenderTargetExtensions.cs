using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DrawItFast.Model.Drawing
{
    public static class RenderTargetExtensions
    {
        public static void DrawArrow(this RenderTarget target, SolidColorBrush brush, RawVector2 V0, RawVector2 V1, float alpha, float size)
        {
            target.DrawLine(V0, V1, brush);
            float distance = Distance(V0, V1);

            RawVector2 pH = new RawVector2() { X = (size * V0.X + (distance - size) * V1.X) / distance, Y = (size * V0.Y + (distance - size) * V1.Y) / distance };

            RawVector2[] triangle = new RawVector2[3];
            triangle[0] = V1;

            float beta = (float)Math.Atan2(V1.Y - V0.Y, V1.X - V0.X);
            float d = Distance(V1, pH) * (float)Math.Tan(alpha * Math.PI / 360);
            float angle1 = (float)(beta + Math.PI / 2);
            float angle2 = (float)(beta - Math.PI / 2);

            triangle[1] = new RawVector2() { X = pH.X + d * (float)Math.Cos(angle1), Y = pH.Y + d * (float)Math.Sin(angle1) };
            triangle[2] = new RawVector2() { X = pH.X + d * (float)Math.Cos(angle2), Y = pH.Y + d * (float)Math.Sin(angle2) };

            PathGeometry polygonGeometry = new PathGeometry(target.Factory);
            GeometrySink gs = polygonGeometry.Open();
            gs.BeginFigure(triangle[0], FigureBegin.Filled);
            gs.AddLines(new RawVector2[]
            {
                triangle[1],
                triangle[2]
            });
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();

            target.FillGeometry(polygonGeometry, brush);

            gs.Dispose();
            polygonGeometry.Dispose();
        }

        public static RawVector2 ToRawVector2(this Point point)
        {
            RawVector2 vector = new RawVector2();
            vector.X = (float)point.X;
            vector.Y = (float)point.Y;
            return vector;
        }

        public static Point ToPoint(this RawVector2 vector)
        {
            return new Point(vector.X, vector.Y);
        }

        private static float Distance(RawVector2 V1, RawVector2 V2)
        {
            return (float)Math.Sqrt(Math.Pow(V1.X - V2.X, 2) + Math.Pow(V1.Y - V2.Y, 2));
        }
    }
}
