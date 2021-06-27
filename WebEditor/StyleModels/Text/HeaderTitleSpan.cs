using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WebEditor.StyleModels.Text
{
    public class HeaderTitleSpan : ICss
    {
        [DisplayName("selector")]
        [Description("Patterns used to select the element(s) you want to style.")]
        [DefaultValue("header.title span")]
        public string Selector { get => "header.title span"; }

        [DisplayName("font-size")]
        [Description("Specifies the font size of text")]
        [DefaultValue("3em")]
        public string FontSize { get; set; } = "3em";
    }
}
