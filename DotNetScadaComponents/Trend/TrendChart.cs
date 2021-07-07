using DotNetCom.General;
using DotNetCom.General.Tags;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace DotNetScadaComponents.Trend
{
    class TrendChart : Chart
    {
        public TrendChartSettings Settings
        {
            get => settings;
            set
            {
                if(settings != null) settings.TagCollectionChanged += Settings_TagCollectionChanged;
                settings = value;
                settings.TagCollectionChanged += Settings_TagCollectionChanged;

            }
        }
        private Dictionary<string, Series> digSeries = new Dictionary<string, Series>();
        private Dictionary<string, Series> analogSeries = new Dictionary<string, Series>();
        private List<Series> digSeriesList = new List<Series>();
        private int cont = 0;
        private TrendChartSettings settings = new TrendChartSettings();

        public TrendChart()
        {
        }

        private void Settings_TagCollectionChanged(object sender, EventArgs e)
        {
            UpdateSeries();
        }

        public void Init()
        {
            //Series.Clear();
            //ChartAreas.Clear();
            //ChartArea chartArea1 = new ChartArea();
            //CustomLabel customLabelX = new CustomLabel();
            //CustomLabel customLabelY = new CustomLabel();
            //Legend legend1 = new Legend();
            //AllowDrop = true;

            //chartArea1.AxisX.CustomLabels.Add(customLabelX);
            //chartArea1.AxisX.MajorGrid.LineColor = Color.LightGray;
            //chartArea1.AxisY.CustomLabels.Add(customLabelY);
            //chartArea1.AxisY.Enabled = AxisEnabled.False;
            //chartArea1.Name = "ChartArea1";
            //ChartAreas.Add(chartArea1);

            //legend1.BackColor = Color.Transparent;
            //legend1.DockedToChartArea = "ChartArea1";
            //legend1.Docking = Docking.Left;
            //legend1.Name = "Legend1";
            //Legends.Add(legend1);


        }

        public void AddData(string name, object value, string time)
        {
            var trendIdx = 0;
            var series = GetSeries(name, value);
            if (value == null) return;
            if (value is bool)
            {
                trendIdx = AddBooleanData(name, (bool)value, time);
            }
            else
            {
                trendIdx = series.Points.AddXY(time, value);
                //var chartArea = ChartAreas.FindByName(series.ChartArea);
                //var digArea = GetDigitalChartArea();
                //chartArea.RecalculateAxesScale();
                //digArea.AxisX.Minimum = chartArea.AxisX.Minimum;
                //digArea.AxisX.Maximum = chartArea.AxisX.Maximum;
                //digArea.AxisY.Minimum = chartArea.AxisY.Minimum;
                //digArea.AxisY.Maximum = chartArea.AxisY.Maximum;
            }
            if (trendIdx > Settings.Axis.X.Size)
            {
                series.Points.RemoveAt(0);
                //var chartArea = ChartAreas.FindByName(series.ChartArea);
                //chartArea.RecalculateAxesScale();
                //foreach (var chart in ChartAreas)
                //{
                //    chart.AxisX.Minimum = trendIdx - Settings.Axis.X.Size;
                //}
                //foreach (var series in Series)
                //{
                //    var chartArea = ChartAreas.FindByName(series.ChartArea);
                //    chartArea.RecalculateAxesScale();
                //}
            }
        }

        public int AddBooleanData(string name, bool value, string time)
        {
            var series = GetSeries(name, value);
            var idx = digSeriesList.IndexOf(series) + 1;
            var offSet = Settings.Axis.Y.DigitalSignals.SizeOffSet;
            Console.WriteLine(value);
            cont = series.Points.AddXY(time, idx - 1, idx - offSet);
            series.Points[cont].Color = series.Color;
            series.Points[cont].Color = value ? series.Color : Color.Transparent;
            return cont;
        }

        public Series GetSeries(string name, object value)
        {
            if (digSeries.ContainsKey(name))
            {
                return digSeries[name];
            }
            if (analogSeries.ContainsKey(name))
            {
                return analogSeries[name];
            }
            var newSeries = Series.Add(name);
            ConfigureSeries(ref newSeries, value);
            return newSeries;
        }

        public ChartArea GetDigitalChartArea()
        {
            var chartArea = ChartAreas.FindByName("DigitalChartArea");
            if (chartArea == null)
            {
                var newChartArea = new ChartArea("DigitalChartArea");
                newChartArea.Name = "DigitalChartArea";
                newChartArea.Position = new ElementPosition(0, 0, 100, 100);
                newChartArea.AxisY.Enabled = AxisEnabled.False;
                newChartArea.AxisX.Maximum = Settings.Axis.X.Size;
                newChartArea.AxisY.Maximum = Settings.Axis.Y.DigitalSignals.Scale;
                ChartAreas.Add(newChartArea);
                return newChartArea;
            }
            return chartArea;
        }

        public void ConfigureSeries(ref Series series, object value)
        {
            if (value is bool)
            {
                digSeries.Add(series.Name, series);
                digSeriesList.Add(series);
                series.ChartType = SeriesChartType.Range;
                series.ChartArea = GetDigitalChartArea().Name;
                //series.CustomProperties = "PointWidth=" + 2;
            }
            else
            {
                var newChart = new ChartArea(series.Name);
                ConfigureNewChart(ref newChart);

                analogSeries.Add(series.Name, series);
                series.ChartType = SeriesChartType.Line;
                series.ChartArea = newChart.Name;
            }
        }

        public void ConfigureNewChart(ref ChartArea chart)
        {
            chart.AlignmentOrientation = AreaAlignmentOrientations.All;
            chart.AlignWithChartArea = GetDigitalChartArea().Name;
            chart.AxisX.IsMarginVisible = false;
            chart.AxisY.Enabled = AxisEnabled.False;
            chart.AxisX.Enabled = AxisEnabled.False;
            chart.AxisX.Maximum = Settings.Axis.X.Size;

            //foreach (var axis in chart.Axes)
            //{
            //    axis.MajorTickMark.Enabled = false;
            //    axis.MajorGrid.Enabled = false;
            //    axis.LabelStyle.Enabled = false;
            //}

            chart.BackColor = Color.Transparent;
            ChartAreas.Add(chart);
        }

        public void UpdateSeries()
        {
            if (Settings.TagCollection.Tags == null) return;
            var tags = Settings.TagCollection.Names;
            var removeDig = digSeries.Keys.Except(tags).ToArray();
            var removeAnalog = analogSeries.Keys.Except(tags).ToArray();
            foreach (var name in removeDig)
            {
                Series.Remove(digSeries[name]);
                digSeries.Remove(name);
            }
            foreach (var name in removeAnalog)
            {
                Series.Remove(analogSeries[name]);
                analogSeries.Remove(name);
                var chartArea = ChartAreas.FindByName(name);
                if (chartArea != null) ChartAreas.Remove(chartArea);
            }
            foreach (var tag in Settings.TagCollection.Tags)
            {
                if(!digSeries.ContainsKey(tag.Name) && !analogSeries.ContainsKey(tag.Name))
                {
                    var time = DateTime.Now.ToString("HH:mm:ss.fff");
                    AddData(tag.Name, tag.Value, time);
                }
            }
        }

        public void Remove(Tag[] tags)
        {
            foreach (var tag in tags)
            {
                if (digSeries.ContainsKey(tag.Name))
                {
                    Series.Remove(digSeries[tag.Name]);
                    digSeries.Remove(tag.Name);
                }
                if (analogSeries.ContainsKey(tag.Name))
                {
                    Series.Remove(analogSeries[tag.Name]);
                    analogSeries.Remove(tag.Name);
                }
                var chartArea = ChartAreas.FindByName(tag.Name);
                if (chartArea != null) ChartAreas.Remove(chartArea);
            }
        }

        private void InitializeComponent()
        {

        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class TrendChartSettings : Component, ITagServer
    {
        private TagCollection tagCollection;

        [JsonProperty]
        [Category("General")]
        [DisplayName("Name")]
        [Description("Name of this trend")]
        public string Name { get; set; } = "trend";

        [JsonProperty]
        [Category("Timing")]
        [DisplayName("Timebase")]
        [Description("Timebase used to update trend")]
        public int TimeBase { get; set; }

        [JsonProperty]
        [Category("General")]
        [DisplayName("Tags Collection")]
        [Description("Collection of tags linked to this control.")]
        public TagCollection TagCollection
        {
            get => tagCollection;
            set
            {
                tagCollection = value;
                TagCollectionChanged?.Invoke(this, null);
            }
        }

        [JsonProperty]
        [Category("Settings")]
        [DisplayName("Axis")]
        [Description("Axis settings.")]
        public TrendChartAxisSettings Axis { get; set; } = new TrendChartAxisSettings();

        public event EventHandler TagCollectionChanged;
    }

    [DesignTimeVisible(false)]
    [JsonObject(MemberSerialization.OptIn)]
    public class TrendChartAxisSettings : Component
    {
        [JsonProperty]
        public TrendChartAxisXSettings X { get; set; } = new TrendChartAxisXSettings();
        [JsonProperty]
        public TrendChartAxisYSettings Y { get; set; } = new TrendChartAxisYSettings();
    }

    [DesignTimeVisible(false)]
    [JsonObject(MemberSerialization.OptIn)]
    public class TrendChartAxisXSettings : Component
    {
        [JsonProperty]
        public int Size { get; set; } = 1000;
    }

    [DesignTimeVisible(false)]
    [JsonObject(MemberSerialization.OptIn)]
    public class TrendChartAxisYSettings : Component
    {
        [JsonProperty]
        public TrendChartAxisYDigitalSettings DigitalSignals { get; set; } = new TrendChartAxisYDigitalSettings();
    }

    [DesignTimeVisible(false)]
    [JsonObject(MemberSerialization.OptIn)]
    public class TrendChartAxisYDigitalSettings : Component
    {
        [JsonProperty]
        public int Scale { get; set; } = 25;
        [JsonProperty]
        public float SizeOffSet { get; set; } = 0.3F;
    }
}
