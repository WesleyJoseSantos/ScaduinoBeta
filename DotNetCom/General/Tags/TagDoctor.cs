using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotNetCom.General.Tags
{
    public partial class TagDoctor : UserControl
    {
        
        public Tag SelectedTag
        {
            get => selectedTag;
            set
            {
                selectedTag = value;
                tagNumber.SelectedTag = selectedTag;
                tagBoolean.SelectedTag = selectedTag;

                if (selectedTag?.Value == null) return;

                if(selectedTag.Value is string)
                {
                    tagString.SelectedTag = selectedTag;
                    propertyGrid.SelectedObject = tagString;
                    return;
                }

                if(selectedTag.Value is bool)
                {
                    tagBoolean.SelectedTag = selectedTag;
                    propertyGrid.SelectedObject = tagBoolean;
                    return;
                }

                tagNumber.SelectedTag = selectedTag;
                propertyGrid.SelectedObject = tagNumber;
            }
        }

        private Tag selectedTag;
        private TagString tagString = new TagString();
        private TagNumber tagNumber = new TagNumber();
        private TagBoolean tagBoolean = new TagBoolean();

        public TagDoctor()
        {
            InitializeComponent();
        }
    }

    internal class TagInterface
    {
        private Tag selectedTag;

        [Browsable(false)]
        public Tag SelectedTag
        {
            get => selectedTag;
            set
            {
                selectedTag = value;
                if (selectedTag == null || selectedTag.Name == null) return;
                Name = selectedTag.Name;
            }
        }

        public string Name { get; set; }
    }

    internal class TagString : TagInterface
    {
        private string _value;

        public string Value
        {
            get => (string)SelectedTag.Value;
            set
            {
                _value = value;
                SelectedTag.Value = _value;
            }
        }
    }

    internal class TagNumber : TagInterface
    {
        private double _value;

        public double Value
        {
            get => (double)SelectedTag.Value;
            set
            {
                _value = value;
                SelectedTag.Value = _value;
            }
        }
    }

    internal class TagBoolean : TagInterface
    {
        private bool _value;

        public bool Value
        {
            get => (bool)SelectedTag.Value;
            set
            {
                _value = value;
                SelectedTag.Value = _value;
            }
        }
    }

}
