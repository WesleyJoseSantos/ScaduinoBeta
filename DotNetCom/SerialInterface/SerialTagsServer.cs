using DotNetCom.General;
using DotNetCom.General.Controls;
using DotNetCom.General.Tags;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotNetCom.SerialInterface
{
    public partial class SerialTagsServer : Serial, ITagServer
    {
        [JsonProperty]
        [Category("General")]
        [DisplayName("Tags Collection")]
        [Description("Collection of tags linked to this control.")]
        public TagCollection TagCollection 
        {
            get => tagCollection;
            set
            {
                if(tagCollection != null)
                {
                    tagCollection.TagChanged -= TagCollection_TagChanged;
                }
                if(value != null)
                {
                    tagCollection = value;
                    tagCollection.TagChanged += TagCollection_TagChanged;
                    tagCollection.EnableEvents();
                }
            }
        }

        [Browsable(false)]
        public ModuleConsole Console { get; set; }

        [JsonProperty]
        [Category("General")]
        [DisplayName("Enabled")]
        [Description("Enable or disable this module")]
        public bool Enabled { get; set; }

        private TagCollection tagCollection;
        private int bacc, bps;
        private Timer t1s;

        public SerialTagsServer()
        {
            InitializeComponent();
            Init();
        }

        public SerialTagsServer(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            Init();
        }

        private void Init()
        {
            t1s = new Timer();
            serialPort.DataReceived += SerialPort_DataReceived;
            t1s.Interval = 1000;
            t1s.Tick += T1s_Tick;
            t1s.Start();
        }

        private void T1s_Tick(object sender, EventArgs e)
        {
            bps = bacc;
            bacc = 0;
            if (Console?.Status != null) Console.Status.Speed = bps.ToString() + "bps";
            if (bps >= BaudRate) Console?.Errors?.AppendText("BaudRate may be slower than current real needed speed.");
        }

        private void TagCollection_TagChanged(object sender, EventArgs e)
        {
            SendData(sender);
        }

        private void SendData(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            if (serialPort.IsOpen)
            {
                serialPort.Write(json);
                bacc += json.Length * 8;
            }
            else
            {
                Begin();
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var data = serialPort.ReadLine();

            Console?.Log?.AppendText(data + '\n');

            if (data == "ALL")
            {
                foreach (var tag in TagCollection.Tags)
                {
                    SendData(tag);
                }
            }
        }
    }
}
