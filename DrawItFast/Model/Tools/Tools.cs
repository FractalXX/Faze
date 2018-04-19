﻿using DrawItFast.Model.Tools.Curves;
using DrawItFast.Model.Tools.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawItFast.Model.Tools
{
    static class Tools
    {
        public static readonly BasicMoveTool BasicMoveTool = new BasicMoveTool();
        public static readonly HermiteCurveTool HermiteCurveTool = new HermiteCurveTool();
    }
}