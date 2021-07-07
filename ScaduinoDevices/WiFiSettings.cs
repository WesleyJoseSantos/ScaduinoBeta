using Newtonsoft.Json;
using System.ComponentModel;

namespace ScaduinoDevices
{
    public enum WiFiMode
    {
        AccessPoint,
        Station
    }

    [JsonObject(MemberSerialization.OptIn)]
    public partial class WiFiSettings : Component
    {
        [JsonProperty]
        [Category("General")]
        [DisplayName("SSID")]
        [Description("WiFi access point name.")]
        public string Ssid { get; set; }

        [JsonProperty]
        [Category("General")]
        [DisplayName("Password")]
        [Description("WiFi access point password.")]
        public string Pass { get; set; }

        [JsonProperty]
        [Category("General")]
        [DisplayName("WiFi mode")]
        [Description("In AccessPoint mode, the device will search and connecto to configured access point. On Station mode, device will create an access point.")]
        public WiFiMode Mode { get; set; }

        public WiFiSettings()
        {
            InitializeComponent();
        }

        public WiFiSettings(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
