using DotNetCom.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ScaduinoDevices
{
    public partial class HmiServerStation : HmiWebServer, IDevice
    {
        public HmiServerStation()
        {
            InitializeComponent();
        }

        public HmiServerStation(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
