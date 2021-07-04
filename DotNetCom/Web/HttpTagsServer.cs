using DotNetCom.DataBase;
using DotNetCom.General.Controls;
using DotNetCom.General.NamedObject;
using DotNetCom.General.Tags;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DotNetCom.Web
{
    public enum WebServerStatus
    {
        Stopped,
        Running
    }

    [JsonObject(MemberSerialization.OptIn)]
    public partial class HttpTagsServer : Component, ITagServer
    {
        private HttpListener listener;
        

        [JsonProperty]
        [Category("General")]
        [DisplayName("Url")]
        [Description("Url to access this web server via browser.")]
        public string Url { get; set; } = "http://localhost:8000/";

        [Category("General")]
        [DisplayName("Status")]
        [Description("Current status of web server.")]
        public WebServerStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                StatusChanged?.Invoke(this, null);
            }
        }

        [Browsable(false)]
        public ModuleConsole Console { get; set; }

        [JsonProperty]
        [Category("General")]
        [DisplayName("Tags Collection")]
        [Description("Collection of tags linked to this control.")]
        public TagCollection TagCollection { get; set; }

        [JsonProperty]
        [Category("Database")]
        [Browsable(false)]
        [DisplayName("Tags Groups")]
        [Description("Tags groups to be logged this device. " +
            "Only one tags group at time will be logged. " +
            "The clien must select the desired tag group.")]
        [Editor(typeof(TagsGroupsSelectorEditor), typeof(UITypeEditor))]
        public TagGroup[] TagsGroups { get; set; }

        [Browsable(false)]
        public string ReceivedData { get; set; }

        [Browsable(false)]
        public string DataToSend { get; set; }

        [Browsable(false)]
        public bool DataAvailable { get; set; }

        [JsonProperty]
        [Category("General")]
        [DisplayName("Enabled")]
        [Description("Enable or disable this module")]
        public bool Enabled { get; set; }

        public event EventHandler StatusChanged;

        private WebServerStatus _status;

        public HttpTagsServer()
        {
            InitializeComponent();
            Initialize();
        }

        public HttpTagsServer(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            Initialize();
        }

        public void Start()
        {
            if (Status == WebServerStatus.Running) return;

            listener = new HttpListener();
            listener.Start();

            Console?.Log?.Invoke((MethodInvoker)delegate
            {
                Console?.Log?.AppendText($"Web Server started on {Url}.\n");
            });

            try
            {
                listener.Prefixes.Add(Url);
            }
            catch (Exception ex)
            {
                Console?.Errors?.Invoke((MethodInvoker)delegate
                {
                    Console?.Errors?.AppendText(ex.Message + '\n');
                });
                Stop();
                return;
            }

            Status = WebServerStatus.Running;
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                while (Status == WebServerStatus.Running)
                {
                    try
                    {
                        HandleIncomingConnections();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + '\n' + ex.StackTrace);
                    }
                }
                Status = WebServerStatus.Stopped;
                listener?.Stop();
                listener?.Prefixes?.Clear();
                Console?.Errors?.Invoke((MethodInvoker)delegate
                {
                    Console?.Log?.AppendText("Web Server stopped.\n");
                });
            });
        }

        public void Stop()
        {
            Status = WebServerStatus.Stopped;
        }

        private void Initialize()
        {
            Console = new ModuleConsole();
        }

        private void HandleIncomingConnections()
        {
            HttpListenerContext ctx = listener.GetContext();
            HttpListenerRequest req = ctx.Request;
            HttpListenerResponse resp = ctx.Response;
            string contentType = "";
            byte[] data = new byte[0];

            ProcessRequest(req, ref contentType, ref data);

            resp.ContentType = contentType;
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;

            resp.OutputStream.Write(data, 0, data.Length);
            resp.Close();
        }

        protected virtual bool ProcessRequest(HttpListenerRequest req, ref string contentType, ref byte[] data)
        {
            if (req.HttpMethod == "GET")
            {
                
                if (req.QueryString.Get("data") != null)
                {
                    data = Encoding.UTF8.GetBytes(DataToSend ?? "null");
                    contentType = "application/json";
                    return true;
                }
                if (req.QueryString.Get("tags") != null)
                {
                    string json = JsonConvert.SerializeObject(TagCollection.Tags);
                    data = Encoding.UTF8.GetBytes(json);
                    contentType = "application/json";
                    return true;
                }
                if (req.QueryString.Get("values") != null)
                {
                    var values = new List<object>();

                    if (TagCollection.Tags == null)
                    {
                        Console?.Errors?.Invoke((MethodInvoker)delegate
                        {
                            Console?.Errors?.AppendText("Values request error, tags is null.");
                        });
                        return false;
                    }
                    foreach (var tag in TagCollection.Tags)
                    {
                        values.Add(tag.Value);
                    }

                    string json = JsonConvert.SerializeObject(values);
                    data = Encoding.UTF8.GetBytes(json);
                    contentType = "application/json";
                    return true;
                } 
            }
            return false;
        }
    }

    class HomePageEditor : UITypeEditor
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
                fileDialog.Filter = "Web Page File | *.html; *.htm";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    value = fileDialog.FileName;
                }
            }
            return value;
        }
    }
}
