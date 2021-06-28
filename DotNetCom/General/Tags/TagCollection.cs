using DotNetCom.DataBase;
using DotNetCom.General.NamedObject;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;

namespace DotNetCom.General.Tags
{
    [JsonObject(MemberSerialization.OptIn)]
    [Editor(typeof(TagsSelectorEditor), typeof(UITypeEditor))]
    public class TagCollection
    {
        [JsonProperty]
        [Category("Database")]
        [DisplayName("Tags")]
        [Description("Tags collection.")]
        public string[] Names { get; set; }

        [Browsable(false)]
        public Tag[] Tags { get => Data.TagsDataBase.GetTags(Names); }

        public event EventHandler TagChanged;

        private void Item_ValueChanged(object sender, EventArgs e)
        {
            TagChanged?.Invoke(sender, e);
        }

        public void Add(string tag)
        {
            var list = Names?.ToList() ?? new System.Collections.Generic.List<string>();
            list.Add(tag);
            Names = list.ToArray();
        }

        public void EnableEvents()
        {
            var tags = Tags;
            DisableEvents();
            if (tags == null) return;
            foreach (var item in tags)
            {
                item.ValueChanged += Item_ValueChanged;
            }
        }

        public void DisableEvents()
        {
            var tags = Tags;
            if (tags == null) return;
            foreach (var item in tags)
            {
                item.ValueChanged -= Item_ValueChanged;
            }
        }
    }
}
