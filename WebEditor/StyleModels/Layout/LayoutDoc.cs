using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebEditor.StyleModels.Layout
{
    public class LayoutDoc : ICssDoc
    {
        public Container Container {get; set;}

        public Content Content { get; set; }
    }
}
