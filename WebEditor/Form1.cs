﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebEditor.StyleModels.Main;

namespace WebEditor
{
    public partial class Form1 : Form
    {
        Body body = new Body();

        public Form1()
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = body;
        }
    }
}
