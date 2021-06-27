using DotNetCom.DataBase;
using DotNetCom.OpcDa;
using DotNetCom.SerialInterface;
using DotNetCom.Web;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;
using Common;

namespace MobileHmi
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AppData : IAppData
    {
        private TagsDataBase tagsDataBase;

        public string DefaultFile { get => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MobileHmi.json"; }

        [JsonProperty(Order = 1)]
        public TagsDataBase TagsDataBase
        {
            get => tagsDataBase;
            set
            {
                tagsDataBase = value;
                Data.TagsDataBase = tagsDataBase;
            }
        }

        [JsonProperty(Order = 2)]
        public OpcClient OpcClient { get; set; }

        [JsonProperty(Order = 3)]
        public SerialTagsServer UsbTagsServer { get; set; }

        [JsonProperty(Order = 4)]
        public HttpTagsServer HttpTagsServer { get; set; }

        [JsonProperty(Order = 5)]
        public HmiWebServer HmiWebServer { get; set; }

        [JsonProperty(Order = 6)]
        public MobileHmiDevice MobileHmi { get; set; }

        public AppData()
        {
            TagsDataBase = new TagsDataBase();
            OpcClient = new OpcClient();
            UsbTagsServer = new SerialTagsServer();
            HttpTagsServer = new HttpTagsServer();
            HmiWebServer = new HmiWebServer();
            MobileHmi = new MobileHmiDevice();
        }

        public IAppData New()
        {
            return new AppData();
        }

        public void Save(string file)
        {
            var strData = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(file, strData);
        }

        public void SaveDefault()
        {
            Save(DefaultFile);
        }

        public IAppData Load(string file)
        {
            var appData = new AppData();
            try
            {
                var strData = File.ReadAllText(file);
                appData = JsonConvert.DeserializeObject<AppData>(strData);
                appData.MobileHmi.ProjectFolder = appData.HmiWebServer.ServerRoot;
            }
            catch (Exception ex)
            {
                if (File.Exists(file))
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (appData == null) return new AppData();
            return appData;
        }

        public IAppData LoadDefault()
        {
            return Load(DefaultFile);
        }
    }
}
