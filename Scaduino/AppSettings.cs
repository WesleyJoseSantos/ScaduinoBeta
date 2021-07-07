using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Scaduino
{
    public enum WindowMode
    {
        FullScreen,
        Window
    }

    [JsonObject(MemberSerialization.OptIn)]
    public partial class AppSettings : Component
    {
        [JsonProperty]
        [Category("General")]
        [DisplayName("Start Local HMI Client")]
        [Description("Automatically start local HMI client at application startup.")]
        public bool StartLocalHmi { get; set; } = true;

        [JsonProperty]
        [Category("General")]
        [DisplayName("StartUp Url")]
        [Description("Url to connect local client to HMI Web Server.")]
        public string StartupUrl { get; set; } = "http://localhost:8000";

        [JsonProperty]
        [Category("General")]
        [DisplayName("Window Mode")]
        [Description("How client hmi screen will be showed at startup.")]
        public WindowMode WindowMode {get; set;}

        public AppSettings()
        {
            InitializeComponent();
        }

        public AppSettings(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
