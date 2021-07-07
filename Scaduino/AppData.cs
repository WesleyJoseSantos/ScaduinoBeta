using Common;
using DotNetCom.DataBase;
using DotNetCom.General;
using DotNetCom.OpcDa;
using DotNetCom.SerialInterface;
using DotNetCom.Text;
using DotNetCom.Web;
using DotNetScadaComponents.Trend;
using Newtonsoft.Json;
using ProDAq;
using ScaduinoDevices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Scaduino
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AppData : IAppData
    {
        public string File { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Scaduino\\Default\\Scaduino.json";

        public string Directory { get; set; }

        public bool IsDefaultFile { get => File == Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Scaduino\\Default\\Scaduino.json"; }

        private TagsDataBase tagsDataBase;

        public IDaqModule[] DaqModules
        {
            get
            {
                var list = new List<IDaqModule>();
                list.AddRange(OpcClientModules ?? new OpcClient[0]);
                list.AddRange(SerialTextModules ?? new SerialText[0]);
                return list.ToArray();
            }
        }

        public IDasModule[] DasModules
        {
            get
            {
                var list = new List<IDasModule>();
                list.AddRange(HttpServerModules ?? new HttpTagsServer[0]);
                list.AddRange(SerialTagsServerModules ?? new SerialTagsServer[0]);
                list.AddRange(HmiWebServers ?? new HmiWebServer[0]);
                return list.ToArray();
            }
        }

        public IDevice[] Devices
        {
            get
            {
                var list = new List<IDevice>();
                list.AddRange(UsbWiFiHmiServers ?? new UsbWifiHmiServer[0]);
                list.AddRange(HmiServerStations ?? new HmiServerStation[0]);
                return list.ToArray();
            }
        }

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
        public OpcClient[] OpcClientModules { get; set; }

        [JsonProperty(Order = 3)]
        public SerialText[] SerialTextModules { get; set; }

        [JsonProperty(Order = 4)]
        public HttpTagsServer[] HttpServerModules { get; set; }

        [JsonProperty(Order = 5)]
        public SerialTagsServer[] SerialTagsServerModules { get; set; }

        [JsonProperty(Order = 6)]
        public HmiWebServer[] HmiWebServers { get; set; }

        [JsonProperty(Order = 7)]
        public UsbWifiHmiServer[] UsbWiFiHmiServers { get; set; }

        [JsonProperty(Order = 8)]
        public HmiServerStation[] HmiServerStations { get; set; }

        [JsonProperty(Order = 9)]
        public Trend[] Trends { get; set; }

        [JsonProperty(Order = 10)]
        public Datalogger Datalogger { get; set; }

        [JsonProperty(Order = 11)]
        public FileInfo[] Files { get; set; }

        [JsonProperty(Order = 12)]
        public AppSettings Settings { get; set; }

        public AppData()
        {
            var fileInfo = new FileInfo(File);
            Directory = fileInfo.Directory.FullName;
            TagsDataBase = new TagsDataBase();
            OpcClientModules = new OpcClient[0];
            SerialTextModules = new SerialText[0];
            HttpServerModules = new HttpTagsServer[0];
            SerialTagsServerModules = new SerialTagsServer[0];
            HmiWebServers = new HmiWebServer[0];
            UsbWiFiHmiServers = new UsbWifiHmiServer[0];
            HmiServerStations = new HmiServerStation[0];
            Trends = new Trend[0];
            Datalogger = new Datalogger();
            Files = new FileInfo[0];
            Settings = new AppSettings();
        }

        public IAppData New()
        {
            return new AppData();
        }

        public void SaveAs(string file)
        {
            var strData = JsonConvert.SerializeObject(this, Formatting.Indented);
            var fileInfo = new FileInfo(file);
            if (!fileInfo.Directory.Exists)
            {
                System.IO.Directory.CreateDirectory(fileInfo.Directory.FullName);
            }
            Directory = fileInfo.Directory.FullName;
            System.IO.File.WriteAllText(file, strData);
        }

        public void Save()
        {
            SaveAs(File);
        }

        public IAppData Load(string file)
        {
            var appData = new AppData();
            var fileInfo = new FileInfo(file);
            appData.Datalogger.FilePath = Directory;
            try
            {
                var strData = System.IO.File.ReadAllText(file);
                appData = JsonConvert.DeserializeObject<AppData>(strData);
                Directory = fileInfo.Directory.FullName;
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(file))
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (appData == null) return new AppData();
            return appData;
        }

        public IAppData Load()
        {
            return Load(File);
        }

        //public T[] Add<T>(T item, T[] collection)
        //{
        //    var list = collection?.ToList() ?? new List<T>();
        //    list.Add(item);
        //    collection = list.ToArray();
        //    return collection;
        //}

        //public T[] Remove<T>(T item, T[] collection)
        //{
        //    var list = collection.ToList();
        //    list.Remove(item);
        //    collection = list.ToArray();
        //    return collection;
        //}

        public void Add<T>(T item)
        {
            PropertyInfo[] properties = GetType().GetProperties();

            foreach (var p in properties)
            {
                if(p.PropertyType == typeof(T[]))
                {
                    var collection = p.GetValue(this, null) as T[];
                    var list = collection.ToList();
                    list.Add(item);
                    p.SetValue(this, list.ToArray(), null);
                }
            }
        }

        public void Remove<T>(T item)
        {
            PropertyInfo[] properties = GetType().GetProperties();

            foreach (var p in properties)
            {
                if (p.PropertyType == typeof(T[]))
                {
                    var collection = p.GetValue(this, null) as T[];
                    var list = collection.ToList();
                    list.Remove(item);
                    p.SetValue(this, list.ToArray(), null);
                }
            }
        }
    }
}
