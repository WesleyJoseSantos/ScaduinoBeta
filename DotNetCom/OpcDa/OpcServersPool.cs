using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetCom.OpcDa
{
    public static class OpcServersPool
    {
        private static Dictionary<string, OpcServerConnection> servers;

        static OpcServersPool()
        {
            servers = new Dictionary<string, OpcServerConnection>();
        }

        public static OpcServerConnection GetServer(string name)
        {
            if (name == null) return null;
            if (name == "") return null;
            if (servers.ContainsKey(name))
            {
                return servers[name];
            }
            else
            {
                var server = new OpcServerConnection();
                server.ServerName = name;
                servers.Add(name, server);
                return servers[name];
            }
        }
    }
}
