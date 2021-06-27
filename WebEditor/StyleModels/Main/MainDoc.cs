using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebEditor.StyleModels.Main
{
    public class MainDoc : ICssDoc
    {
        public Global Global { get; set; }

        public Body Body { get; set; }
    }
}
