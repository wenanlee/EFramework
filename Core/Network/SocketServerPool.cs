using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace EFramework.Network
{
    public class SocketServerPool : IDisposable
    {
        public List<SocketServer> Servers { get; set; } = new List<SocketServer>();

        public SocketServer this[int index]
        {
            get
            {
                if (Servers.Count < index)
                    return Servers[index];
                return null;
            }
        }
        public SocketServer this[string name]
        {
            get
            {
                var server = Servers.Where(x => x.Uid == name);
                if (server.Count() > 0)
                    return server.First();
                return null;
            }
        }

        public void Dispose()
        {
            Servers.ForEach(x => x.Stop());
            Servers.Clear();
        }

        public SocketServer Add(ESocketType type, IPAddress ip, int port)
        {
            SocketServer server = new SocketServer(type, ip, port);
            Servers.Add(server);
            return server;
        }

        public bool Remove(string name)
        {
            int count = Servers.RemoveAll(x => x.Uid == name);
            return count > 0 ? true : false;
        }

        public bool Remove(int index)
        {
            try
            {
                Servers.RemoveAt(index);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
