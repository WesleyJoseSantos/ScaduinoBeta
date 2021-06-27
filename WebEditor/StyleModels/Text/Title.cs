using System.ComponentModel;

namespace WebEditor.StyleModels.Text
{
    public class Title : ICss
    {
        [DisplayName("selector")]
        [Description("Patterns used to select the element(s) you want to style.")]
        [DefaultValue(".title")]
        public string Selector { get => ".title"; }

        [DisplayName("text-align")]
        [Description("Specifies the horizontal alignment of text")]
        [DefaultValue("center")]
        public string TextAlign { get; set; } = "center";

        [DisplayName("color")]
        [Description("Sets the color of text")]
        [DefaultValue("#f5f5f5")]
        public string Color { get; set; } = "#f5f5f5";
    }
}
