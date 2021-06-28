using DotNetCom.DataBase;
using DotNetCom.General.Tags;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ProDAq
{
    public partial class Datalogger : Component
    {
        public string FilePath { get; set; }

        public int FileSize { get; set; }

        private FileStream fs;

        public Datalogger()
        {
            InitializeComponent();
            Data.TagsDataBase.TagChanged += TagsDataBase_TagChanged;
        }

        private void TagsDataBase_TagChanged(object sender, EventArgs e)
        {
            if(fs != null)
            {
                var tag = sender as Tag;
                var json = LogData.ToJson(tag.Name, tag.Value);
                var data = Encoding.UTF8.GetBytes(json);
                var size = data.Length;
                fs.Write(data, 0, size);
            }
        }

        public Datalogger(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void Start()
        {
            fs = File.OpenWrite(FilePath);
        }

        public void Stop()
        {

        }
    }

    class LogData
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string Date { get => DateTime.Now.ToString("HH:mm:ss.fff"); }

        public LogData(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public static string ToJson(string name, object value)
        {
            var data = new LogData(name, value);
            var str = JsonConvert.SerializeObject(data);
            return str;
        }
    }
}
