using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawItFast.Model.Drawing.Drawables;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DrawItFast.View.Controls
{
    public class D2DContainer : D2dControl.D2dControl
    {
        private List<IDrawable> drawables;

        private Random rnd = new Random();

        public override void Render(RenderTarget target)
        {
            target.Clear(new RawColor4(1.0f, 1.0f, 1.0f, 1.0f));
            for(int i = 0; i < drawables.Count; i++)
            {
                this.drawables[i].Draw(target);
            }
        }

        internal void BindList(List<IDrawable> drawables)
        {
            this.drawables = drawables;
        }
    }
}
