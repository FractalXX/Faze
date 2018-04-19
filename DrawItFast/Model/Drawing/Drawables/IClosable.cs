using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawItFast.Model.Drawing.Drawables
{
    interface IClosable
    {
        bool Closed { get; set; }
    }
}
