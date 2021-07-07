﻿using Common;
using DotNetCom.DataBase;
using DotNetCom.General;
using DotNetCom.OpcDa;
using DotNetCom.Text;
using DotNetScadaComponents.Trend;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProDAq
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AppData : IAppData
    {
        public string File { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ProDAq\\ProDAq.json";

        public bool IsDefaultFile { get => File == Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ProDAq\\ProDAq.json"; }

        public string Directory { get; set; }

        private TagsDataBase tagsDataBase;
        
        public IDaqModule[] DaqModules 
        {
            get
            {
                var list = new List<IDaqModule>();
                list.AddRange(OpcClientModules);
                list.AddRange(SerialTextModules);
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

        [JsonProperty(Order = 1)]
        public OpcClient[] OpcClientModules { get; set; }

        [JsonProperty(Order = 2)]
        public SerialText[] SerialTextModules { get; set; }

        [JsonProperty(Order = 3)]
        public Trend[] Trends { get; set; }

        [JsonProperty(Order = 4)]
        public Datalogger Datalogger { get; set; }

        public AppData()
        {
            TagsDataBase = new TagsDataBase();
            OpcClientModules = new OpcClient[0];
            SerialTextModules = new SerialText[0];
            Trends = new Trend[0];
            Datalogger = new Datalogger();
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
            System.IO.File.WriteAllText(file, strData);
        }

        public void Save()
        {
            SaveAs(File);
        }

        public IAppData Load(string file)
        {
            var appData = new AppData();
            try
            {
                var strData = System.IO.File.ReadAllText(file);
                appData = JsonConvert.DeserializeObject<AppData>(strData);
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

        public void AddTrend(Trend trend)
        {
            var list = Trends.ToList();
            list.Add(trend);
            Trends = list.ToArray();
        }

        public void RemoveTrend(Trend trend)
        {
            var list = Trends.ToList();
            list.Remove(trend);
            Trends = list.ToArray();
        }

        public void AddModule(OpcClient opcClient)
        {
            var list = OpcClientModules.ToList();
            list.Add(opcClient);
            OpcClientModules = list.ToArray();
        }

        public void AddModule(SerialText serialText)
        {
            var list = SerialTextModules.ToList();
            list.Add(serialText);
            SerialTextModules = list.ToArray();
        }

        public void RemoveItem(object item)
        {
            if (item is OpcClient) RemoveModule(item as OpcClient);
            if (item is SerialText) RemoveModule(item as SerialText);
        }

        public void RemoveModule(OpcClient opcClient)
        {
            var list = OpcClientModules.ToList();
            list.Remove(opcClient);
            OpcClientModules = list.ToArray();
        }

        public void RemoveModule(SerialText serialText)
        {
            var list = SerialTextModules.ToList();
            list.Remove(serialText);
            SerialTextModules = list.ToArray();
        }
    }
}
