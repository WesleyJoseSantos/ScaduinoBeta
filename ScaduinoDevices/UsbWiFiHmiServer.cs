using DotNetCom.SerialInterface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ScaduinoDevices
{
    public partial class UsbWifiHmiServer : SerialTagsServer, IDevice
    {
        [JsonProperty]
        [Category("General")]
        [DisplayName("WiFi Settings")]
        [Description("WiFi configuration.")]
        public WiFiSettings WiFiSettings { get; set; }

        public UsbWifiHmiServer()
        {
            InitializeComponent();
            WiFiSettings = new WiFiSettings();
        }

        public UsbWifiHmiServer(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
