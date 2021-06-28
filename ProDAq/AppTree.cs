﻿using DotNetCom.General;
using DotNetCom.OpcDa;
using DotNetCom.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProDAq
{
    public partial class AppTree : TreeView
    {
        public enum Images : int
        {
            OpcDa,
            TextInterface,
            Group,
            Serial,
            Item,
            SquareWave,
            AnalogWave,
            Datalogging
        }

        public AppNodes AppNodes { get; set; }

        public AppTree()
        {
            InitializeComponent();
            AppNodes = new AppNodes(this);
        }

        public void AddModules(IComModule[] comModules)
        {
            foreach (var module in comModules)
            {
                AddModule(module);
            }
        }

        public void AddModule(IComModule comModule)
        {
            if(comModule is OpcClient)
            {
                AddModuleToNode(AppNodes.OpcDa, comModule, (int)Images.OpcDa);
            }
            if(comModule is SerialText)
            {
                AddModuleToNode(AppNodes.TextInterface, comModule, (int)Images.TextInterface);
            }
        }

        private TreeNode AddModuleToNode(TreeNode parentNode, IComModule newModule, int imgIdx)
        {
            var newNode = new TreeNode();
            newNode.Text = $"{newModule.Name}";
            newModule.Name = newNode.Text;
            newNode.Tag = newModule;
            newNode.ImageIndex = imgIdx;
            newNode.SelectedImageIndex = imgIdx;
            parentNode.Nodes.Add(newNode);
            parentNode.Expand();
            return newNode;
        }
    }
    public class AppNodes
    {
        public enum Nodes
        {
            Datalogging,
            OpcDa,
            TextInterface
        }

        private AppTree appTree;
        
        public TreeNode Datalogging { get => appTree.Nodes[(int)Nodes.Datalogging]; } 
        public TreeNode OpcDa { get => appTree.Nodes[(int)Nodes.OpcDa]; } 
        public TreeNode TextInterface { get => appTree.Nodes[(int)Nodes.TextInterface]; } 

        public AppNodes(AppTree appTree)
        {
            this.appTree = appTree;
            Datalogging.ImageIndex = (int)AppTree.Images.Datalogging;
            Datalogging.SelectedImageIndex = (int)AppTree.Images.Datalogging;
            OpcDa.ImageIndex = (int)AppTree.Images.OpcDa;
            OpcDa.SelectedImageIndex = (int)AppTree.Images.OpcDa;
            TextInterface.ImageIndex = (int)AppTree.Images.TextInterface;
            TextInterface.SelectedImageIndex = (int)AppTree.Images.TextInterface;
        }
    }
}
