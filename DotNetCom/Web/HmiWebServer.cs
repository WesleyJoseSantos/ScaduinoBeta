using Newtonsoft.Json;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Net;
using System.Text;

namespace DotNetCom.Web
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class HmiWebServer : HttpTagsServer
    {
        private string root;
        private string homePage;

        [JsonProperty]
        [Category("General")]
        [DisplayName("Server Root")]
        [Description("Directory where server will search by requested files/data.")]
        public string ServerRoot { get => root; }

        [JsonProperty]
        [Category("General")]
        [DisplayName("Home Page")]
        [Description("Html file to be home page of web server")]
        [Editor(typeof(HomePageEditor), typeof(UITypeEditor))]
        public string HomePage
        {
            get => homePage;
            set
            {
                if (value == null) return;
                var fileInfo = new FileInfo(value);
                if (fileInfo.Exists)
                {
                    homePage = value;
                    root = fileInfo.DirectoryName;
                }

            }
        }

        public HmiWebServer()
        {
            InitializeComponent();
        }

        public HmiWebServer(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private string GetContentType(string url)
        {
            if (url.EndsWith(".json") || url.EndsWith(".jso"))
            {
                return "application/json";
            }
            else if (url.EndsWith(".css"))
            {
                return "text/css";
            }
            else if (url.EndsWith(".svg"))
            {
                return "image/svg+xml";
            }
            else if (url.EndsWith(".png"))
            {
                return "image/png";
            }
            else if (url.EndsWith(".ico"))
            {
                return "image/x-icon";
            }
            else if (url.EndsWith(".js"))
            {
                return "application/javascript";
            }
            else
            {
                return "text/html";
            }
        }

        protected override bool ProcessRequest(HttpListenerRequest req, ref string contentType, ref byte[] data)
        {
            if(base.ProcessRequest(req, ref contentType, ref data))
            {
                return true;
            }

            if (req.RawUrl == "/")
            {
                data = File.ReadAllBytes(HomePage);
                contentType = "text/html";
                return true;
            }

            string filePath = root + req.RawUrl.Replace('/', '\\');
            contentType = GetContentType(req.RawUrl);
            if (File.Exists(filePath))
            {
                data = File.ReadAllBytes(filePath);
                return true;
            }
            else
            {
                data = Encoding.UTF8.GetBytes($"Cannot GET {req.RawUrl}");
                return false;
            }
        }
    }
}
