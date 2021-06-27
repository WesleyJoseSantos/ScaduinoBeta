using System.ComponentModel;
using System.Windows.Forms;

namespace WebEditor.StyleModels.Main
{
    public class Global : ICss
    {
        [DisplayName("selector")]
        [Description("Patterns used to select the element(s) you want to style.")]
        [DefaultValue("*")]
        public string Selector { get => "*"; }

        [DisplayName("margin")]
        [Description("Sets all the margin properties in one declaration")]
        [DefaultValue("0")]
        public string Margin { get; set; } = "0";

        [DisplayName("padding")]
        [Description("A shorthand property for all the padding* properties")]
        [DefaultValue("0")]
        public string Padding { get; set; } = "0";

        [DisplayName("outline")]
        [Description("A shorthand property for the outline-width, outline-style, and the outline-color properties")]
        [DefaultValue("0")]
        public string Outline { get; set; } = "0";

        [DisplayName("box-sizing")]
        [Description("Defines how the width and height of an element are calculated: should they include padding and borders, or not")]
        [DefaultValue("border-box")]
        public string BoxSizing { get; set; } = "border-box";

        [DisplayName("font-family")]
        [Description("Specifies the font family for text")]
        [DefaultValue("'Segoe UI', Tahoma, Geneva, Verdana, sans-serif")]
        public string FontFamily { get; set; } = "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif";
    }
}
