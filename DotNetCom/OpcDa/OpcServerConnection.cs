using DotNetCom.General.Controls;
using Newtonsoft.Json;
using OPCAutomation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace DotNetCom.OpcDa
{
    public enum ServerState
    {
        Running = 1,
        Failed,
        Noconfig,
        Suspended,
        Test,
        Disconnected
    }

    public enum OPCServerConnectionAction
    {
        None,
        Connect,
        Disconnect
    }

    [JsonObject(MemberSerialization.OptIn)]
    [DesignTimeVisible(false)]
    public class OpcServerConnection : Component, OPCServer
    {
        private OPCServer thisServer;

        private OPCServer server 
        {
            get
            {
                try
                {
                    if(thisServer == null)
                    {
                        thisServer = new OPCServer();
                    }
                    return thisServer;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        [JsonProperty]
        [Category("General")]
        [DisplayName("Server Name")]
        [Description("OPC server name.")]
        [TypeConverter(typeof(ServerNameTypeConverter))]
        public string ServerName { get; set; }

        [JsonProperty]
        [Category("General")]
        [DisplayName("Computer Name")]
        [Description("Computer Name where opc server is running.")]
        public string ServerNode { get; set; }

        [JsonProperty]
        [Category("General")]
        [DisplayName("Server State")]
        [Description("Current State of OPC server.")]
        public ServerState CurrentState => (ServerState)server.ServerState;

        [Category("General")]
        [DisplayName("Connected Clients")]
        [Description("Local instances of opc clients connected to this opc server connection instance.")]
        public OpcClient[] ConnectedClients => connectedClients;

        [Browsable(false)]
        public ModuleConsole Console { get; set; }

        [Browsable(false)]
        public string ClientName { get => server.ClientName; set => server.ClientName = value; }

        [Browsable(false)]
        public DateTime StartTime => server.StartTime;

        [Browsable(false)]
        public DateTime CurrentTime => server.CurrentTime;

        [Browsable(false)]
        public DateTime LastUpdateTime => server.LastUpdateTime;

        [Browsable(false)]
        public short MajorVersion => server.MajorVersion;

        [Browsable(false)]
        public short MinorVersion => server.MinorVersion;

        [Browsable(false)]
        public short BuildNumber => server.BuildNumber;

        [Browsable(false)]
        public string VendorInfo => server.VendorInfo;

        [Browsable(false)]
        public int ServerState => server.ServerState;

        [Browsable(false)]
        public int LocaleID { get => server.LocaleID; set => server.LocaleID = value; }

        [Browsable(false)]
        public int Bandwidth => server.Bandwidth;

        [Browsable(false)]
        public OPCGroups OPCGroups => server.OPCGroups;

        [Browsable(false)]
        public dynamic PublicGroupNames => server.PublicGroupNames;

        public event DIOPCServerEvent_ServerShutDownEventHandler ServerShutDown;

        public event EventHandler ServerStarted;
        public event EventHandler ServerStopped;

        private IContainer components = null;
        private OpcClient[] connectedClients;

        public OpcServerConnection()
        {
            if (server == null) return;
            server.ServerShutDown += ThisServer_ServerShutDown;
            InitializeComponent();
            Console = new ModuleConsole();
        }

        public dynamic GetOPCServers(object Node)
        {
            var anServer = new OPCServer();
            object servers = anServer.GetOPCServers();
            var arrServers = (Array)servers;
            var serverList = new List<string>();

            for (int i = 1; i <= arrServers.Length; i++)
            {
                object server = arrServers.GetValue(i);
                var newServer = new OpcServerConnection();
                serverList.Add(server.ToString());
            }

            return serverList;
        }

        public void Connect()
        {
            try
            {
                server?.Connect(ServerName, ServerNode);
                if (CurrentState == OpcDa.ServerState.Running)
                {
                    ServerStarted?.Invoke(this, null);
                }
                Console?.Log?.AppendText("Server status: " + CurrentState + '\n');
            }
            catch (Exception ex)
            {
                Console?.Errors?.AppendText(ex.Message + '\n');
            }
        }

        public void Connect(string ProgID, object Node)
        {
            ServerName = ProgID;
            ServerNode = Node.ToString();
            server.Connect(ProgID, Node);
            Console?.Log?.AppendText("Server status: " + CurrentState + '\n');
        }

        public void Disconnect()
        {
            server.Disconnect();
            Console?.Log?.AppendText("Server status: " + CurrentState + '\n');
            ServerStopped?.Invoke(this, null);
        }

        public void AddClient(OpcClient client)
        {
            var list = connectedClients?.ToList() ?? new List<OpcClient>();
            list.Add(client);
            connectedClients = list.ToArray();
            Console?.Log?.AppendText("Server client added: " + client.GroupName + '\n');
        }

        public void RemoveClient(OpcClient client)
        {
            var list = connectedClients?.ToList() ?? new List<OpcClient>();
            list.Remove(client);
            connectedClients = list.ToArray();

            if (connectedClients.Length == 0)
            {
                Disconnect();
            }

            Console?.Log?.AppendText("Server client removed: " + client.GroupName + '\n');
        }

        public OPCBrowser CreateBrowser()
        {
            if(CurrentState != OpcDa.ServerState.Running)
            {
                MessageBox.Show("Server connection not estabilished. Server status: " + CurrentState.ToString());
                return null;
            }
            return server.CreateBrowser();
        }

        public string GetErrorString(int ErrorCode)
        {
            return server.GetErrorString(ErrorCode);
        }

        public dynamic QueryAvailableLocaleIDs()
        {
            return server.QueryAvailableLocaleIDs();
        }

        public void QueryAvailableProperties(string ItemID, out int Count, out Array PropertyIDs, out Array Descriptions, out Array DataTypes)
        {
            server.QueryAvailableProperties(ItemID, out Count, out PropertyIDs, out Descriptions, out DataTypes);
        }

        public void GetItemProperties(string ItemID, int Count, ref Array PropertyIDs, out Array PropertyValues, out Array Errors)
        {
            server.GetItemProperties(ItemID, Count, ref PropertyIDs, out PropertyValues, out Errors);
        }

        public void LookupItemIDs(string ItemID, int Count, ref Array PropertyIDs, out Array NewItemIDs, out Array Errors)
        {
            server.LookupItemIDs(ItemID, Count, ref PropertyIDs, out NewItemIDs, out Errors);
        }

        private void ThisServer_ServerShutDown(string Reason)
        {
            ServerShutDown?.Invoke(Reason);
            Console?.Errors?.AppendText("Server ShutDown! Reason: " + Reason);
        }

        public OpcServerConnection(IContainer container)
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

    class ServerNameTypeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var server = new OpcServerConnection();
            var client = context.Instance as OpcClient;
            try
            {
                var list = server.GetOPCServers("");
                return new StandardValuesCollection(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "OPC Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
