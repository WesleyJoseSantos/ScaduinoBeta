using DotNetCom.General.Tags;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotNetScadaComponents.Trend
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Trend : UserControl, ITagServer
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
}
