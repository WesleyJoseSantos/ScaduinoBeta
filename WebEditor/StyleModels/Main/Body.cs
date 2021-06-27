using System.ComponentModel;

namespace WebEditor.StyleModels.Main
{
    public class Body : ICss
    {
        [DisplayName("selector")]
        [Description("Patterns used to select the element(s) you want to style.")]
        [DefaultValue("body")]
        public string Selector { get => "body"; }

        [DisplayName("width")]
        [Description("Sets the width of an element")]
        [DefaultValue("100%")]
        public string Width { get; set; } = "100%";

        [DisplayName("height")]
        [Description("Sets the height of an element")]
        [DefaultValue("100vh")]
        public string Height { get; set; } = "100vh";

        [DisplayName("padding-top")]
        [Description("Sets the top padding of an element")]
        [DefaultValue("30px")]
        public string PaddingTop { get; set; } = "30px";

        [DisplayName("display")]
        [Description("Specifies how a certain HTML element should be displayed")]
        [DefaultValue("flex")]
        public string Display { get; set; } = "flex";

        [DisplayName("flex-direction")]
        [Description("Specifies the direction of the flexible items")]
        [DefaultValue("column")]
        public string FlexDirection { get; set; } = "column";

        [DisplayName("align-items")]
        [Description("Specifies the alignment for items inside a flexible container")]
        [DefaultValue("center")]
        public string AlignItems { get; set; } = "center";

        [DisplayName("justify-content")]
        [Description("Specifies the alignment between the items inside a flexible container when the items do not use all available space")]
        [DefaultValue("center")]
        public string JustifyContent { get; set; } = "center";
    }
}
