using Common;
using DotNetCom.DataBase;
using DotNetCom.General;
using DotNetCom.OpcDa;
using DotNetCom.Text;
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
        public string DefaultFile { get => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ProDAq.json"; }
        
        private TagsDataBase tagsDataBase;

        //ImgIdx, ComModule
        public Dictionary<int, IComModule[]> ComModules {
            get
            {
                var dic = new Dictionary<int, IComModule[]>();
                dic.Add(7, OpcClientModules);
                dic.Add(8, SerialTextModules);
                return dic;
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

        public AppData()
        {
            TagsDataBase = new TagsDataBase();
            OpcClientModules = new OpcClient[0];
            SerialTextModules = new SerialText[0];
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
