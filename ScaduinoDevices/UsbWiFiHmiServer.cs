using DotNetCom.SerialInterface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ScaduinoDevices
{
    public partial class UsbWifiHmiServer : SerialTagsServer, IDevice
    {
        [JsonProperty]
        [Category("General")]
        [DisplayName("WiFi Settings")]
        [Description("WiFi configuration.")]
        public WiFiSettings WiFiSettings { get; set; }

        [JsonProperty]
        [Category("General")]
        [DisplayName("Files")]
        [Description("Application files to upload to device. If this property is empty, all app files will be uploaded to device.")]
        [Editor(typeof(FilesEditor), typeof(UITypeEditor))]
        public string[] Files { get; set; }

        private bool downloadOnProgress = false;

        public event EventHandler UploadComplete;

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

        public bool Check()
        {
            JObject json = new JObject();
            json["Test"] = true;
            var ret = false;

            try
            {
                DisableEvent();
                Begin();
                _ = serialPort.ReadExisting();
                serialPort.Write(json.ToString(Formatting.None));
                Wait();
                var data = serialPort.ReadLine();
                json = JObject.Parse(data);
                ret = (bool)json["Test"];
            }
            catch (Exception)
            {
                ret = false;
            }
            End();
            EnableEvent();
            return ret;
        }

        public void SendSettings()
        {
            var data = JsonConvert.SerializeObject(WiFiSettings);
            SendData(data);
            SendKey("StartWifi");
        }

        public void SendApplication()
        {
            var data = JsonConvert.SerializeObject(WiFiSettings);
            SendFiles();
        }

        public void SendFiles()
        {
            if (!downloadOnProgress)
            {
                if(Files == null)
                {
                    UploadComplete?.Invoke(this, null);
                    return;
                }

                downloadOnProgress = true;


                System.Threading.ThreadPool.QueueUserWorkItem(delegate
                {
                    SendKey("Format");
                    WaitResponse();

                    JObject data = new JObject();
                    foreach (var file in Files)
                    {
                        var fileInfo = new FileInfo(file);
                        var fileName = '/' + fileInfo.Name;
                        var content = File.ReadAllLines(file, Encoding.UTF8);
                        data["File"] = fileName;
                        data["Length"] = fileInfo.Length;
                        SendData(data.ToString(Formatting.None));
                        Wait();

                        foreach (var line in content)
                        {
                            SendData(line);
                            Wait();
                        }

                        SendData("END");
                        WaitResponse();
                    }

                    SendData(JsonConvert.SerializeObject(WiFiSettings));
                    SendKey("StartWifi");
                    downloadOnProgress = false;

                    UploadComplete?.Invoke(this, null);
                });
            }
        }

        private void WaitResponse()
        {
            DisableEvent();
            Wait();
            //_ = serialPort.ReadExisting();
            serialPort.ReadTimeout = System.IO.Ports.SerialPort.InfiniteTimeout;
            _ = serialPort.ReadLine();
            EnableEvent();
        }

        public void Wait()
        {
            System.Threading.Thread.Sleep(35);
        }

        public void SendKey(string key)
        {
            JObject jKey = new JObject();
            jKey[key] = true;
            SendData(jKey.ToString(Formatting.None));
        }

    }

    class FilesEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "All Files | *.*";
                fileDialog.Multiselect = true;
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    value = fileDialog.FileNames;
                }
            }
            return value;
        }
    }
}
