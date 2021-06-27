using DotNetCom.General;
using DotNetCom.General.Controls;
using DotNetCom.General.Tags;
using Newtonsoft.Json;
using OPCAutomation;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace DotNetCom.OpcDa
{
    public enum ClientState
    {
        Disconnected,
        Connected
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class OpcClient : Component, IComModule
    {
        private int updateRate;
        private int itHdl;
        private OPCGroup opcGroup;

        [JsonProperty]
        [Category("General")]
        [DisplayName("Name")]
        [Description("Name used to identify this module on GUI.")]
        public string Name { get; set; } = "OPC DA Module";

        [JsonProperty]
        [Category("General")]
        [DisplayName("Server Name")]
        [Description("OPC server name.")]
        [TypeConverter(typeof(ServerNameTypeConverter))]
        public string ServerName
        {
            get => serverName;
            set
            {
                if(Server != null)
                {
                    Server.ServerStarted -= Server_ServerStarted;
                }
                serverName = value;
                if (Server != null)
                {
                    Server.ServerStarted += Server_ServerStarted;
                }
            }
        }

        [Category("OPC")]
        [DisplayName("Server")]
        [Description("OPC Server to be connected with this client.")]
        [Browsable(false)]
        public OpcServerConnection Server
        {
            get
            {
                return OpcServersPool.GetServer(ServerName);
            }
        }

        [JsonProperty]
        [Category("OPC")]
        [DisplayName("Group Name")]
        [Description("Group name used to connecting to the OPC server.")]
        public string GroupName { get; set; }

        [JsonProperty]
        [Category("OPC")]
        [DisplayName("Update Time")]
        [Description("Determine how fast data is retrieved from the OPC server.")]
        public int UpdateTime
        {
            get
            {
                return updateRate;
            }
            set
            {
                updateRate = value;
                if (opcGroup != null)
                {
                    try
                    {
                        opcGroup.UpdateRate = value;
                    }
                    catch (Exception ex)
                    {
                        Console?.Errors?.AppendText(ex.Message);
                    }
                }
            }
        }

        [JsonProperty]
        [Category("OPC")]
        [DisplayName("OPC Items")]
        [Description("OPC Items of this client.")]
        [Editor(typeof(OPCItemCollectionEditor), typeof(UITypeEditor))]
        public TagLink[] TagLinks { get; set; }

        [Browsable(false)]
        public ModuleConsole Console { get; set; }

        [JsonProperty]
        [Category("General")]
        [DisplayName("Enabled")]
        [Description("Enable or disable this module")]
        public bool Enabled { get; set; }

        [JsonProperty]
        [Category("General")]
        [DisplayName("State")]
        [Description("Flag that signs current client state")]
        public ClientState State
        {
            get
            {
                if (Server == null) return ClientState.Disconnected;
                if(Server.CurrentState == ServerState.Running && opcGroup.IsActive)
                {
                    return ClientState.Connected;
                }
                else
                {
                    return ClientState.Disconnected;
                }
            }
        }

        public OpcClient()
        {
            InitializeComponent();
            Console = new ModuleConsole();
        }

        public void Connect()
        {
            Server?.Connect();
        }

        public void Disconnect()
        {
            try
            {
                if(opcGroup != null) Server.OPCGroups.Remove(opcGroup);
            }
            catch (Exception)
            {
            }
            Server.RemoveClient(this);
        }

        public void AddItems()
        {
            if (Server.CurrentState != ServerState.Running)
            {
                Server.Connect();
            }
            var browser = new OpcBrowser();
            browser.OPCBrowser = Server.CreateBrowser();
            if (browser.ShowDialog() == DialogResult.OK)
            {
                TagLinks = browser.SelectedItems;
            }
            UpdateGroup();
        }

        private void InitializeGroup()
        {
            //if (OPCItems != null && opcGroup == null)
            //{
            var groupName = GroupName != "" ? GroupName : Guid.NewGuid().ToString("n").Substring(0, 8);
            opcGroup = Server.OPCGroups.Add(GroupName);
            itHdl = 1;

            opcGroup.DataChange += OpcGroup_DataChange;
            opcGroup.UpdateRate = UpdateTime;
            opcGroup.IsSubscribed = true;
            opcGroup.OPCItems.DefaultIsActive = true;
            //}
        }

        private void UpdateGroup()
        {
            if (TagLinks != null)
            {
                for (int i = itHdl - 1; i < TagLinks.Length; i++)
                {
                    try
                    {
                        opcGroup.OPCItems.AddItem(TagLinks[i].Id, itHdl);
                        TagLinks[i].Status = TagLinkStatus.Good;
                        itHdl++;
                    }
                    catch (Exception ex)
                    {
                        TagLinks[i].Status = TagLinkStatus.Bad;
                        Console?.Errors?.AppendText(TagLinks[i].Id + "is Bad. ");
                        Console?.Errors?.AppendText(ex.Message + '\n');
                    }
                }
            }
        }

        private void OpcGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            for (int i = 1; i <= NumItems; i++)
            {
                var itHdl = Convert.ToInt32(ClientHandles.GetValue(i));
                if (TagLinks != null)
                {
                    var value = ItemValues.GetValue(i);
                    TagLinks[itHdl - 1].Value = value;
                }
            }
        }

        private void Server_ServerStarted(object sender, EventArgs e)
        {
            InitializeGroup();
            UpdateGroup();
            Console?.Log?.AppendText("OPC Server Started!.\n");
        }

        private IContainer components = null;
        private string serverName;

        public OpcClient(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
        }
    }
}
