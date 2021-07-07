using DotNetCom.General;
using DotNetCom.General.Tags;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace DotNetScadaComponents.Trend
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Trend : UserControl, IDasModule
    {
        [JsonProperty]
        [Category("General")]
        [DisplayName("Settings")]
        [Description("Trend settings.")]
        public TrendChartSettings Settings
        {
            get => trendChart.Settings;
            set => trendChart.Settings = value;
        }

        [Category("Database")]
        [DisplayName("Tags Collection")]
        [Description("Collection of tags linked to this control.")]
        public TagCollection TagCollection
        {
            get => Settings.TagCollection;
            set => Settings.TagCollection = value;
        }

        public new event EventHandler Click
        {
            add
            {
                base.Click += value;
                foreach (Control control in Controls)
                {
                    control.Click += Control_Click;
                }
            }
            remove
            {
                base.Click += value;
                foreach (Control control in Controls)
                {
                    control.Click -= Control_Click;
                }
            }
        }

        private void Control_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        private Timer timer;

        public Trend()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Tick += Timer_Tick;
            TagCollection = new TagCollection();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (TagCollection?.Tags != null)
            {
                foreach (var tag in TagCollection.Tags)
                {
                    var time = DateTime.Now.ToString("HH:mm:ss.fff");
                    if(tag.Value != null)
                    {
                        trendChart.AddData(tag.Name, tag.Value, time);
                    }
                }
            }
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Open(string[] files)
        {
            foreach (var file  in files)
            {
                var content = File.ReadAllText(file).Split('\n');
                foreach (var line in content)
                {
                    var data = JsonConvert.DeserializeObject<TrendPointData>(line);
                    if (data == null) continue;
                    trendChart.AddData(data.Name, data.Value, data.Date);
                }
            }
        }

        public void Initialize()
        {
            trendChart.UpdateSeries();
        }

        public void Add(Tag tag)
        {
            if(TagCollection == null){
                TagCollection = new TagCollection();
            }
            var time = DateTime.Now.ToString("HH:mm:ss.fff");
            TagCollection.Add(tag.Name);
            trendChart.AddData(tag.Name, tag.Value, time);
        }
    }

    public class TrendPointData
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string Date { get => DateTime.Now.ToString("HH:mm:ss.fff"); }

        public TrendPointData(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public static string ToJson(string name, object value)
        {
            var data = new TrendPointData(name, value);
            var str = JsonConvert.SerializeObject(data);
            return str;
        }
    }
}
