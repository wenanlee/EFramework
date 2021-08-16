using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EFramework.Network
{
    /// <summary>
    /// 
    /// </summary>
    public class SocketServer : IDisposable
    {
        /// <summary>
        /// Unique server id.
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// Listening ip adress.
        /// </summary>
        public IPAddress IPAddress { get; set; }
        /// <summary>
        /// Listening port.
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// List of connected clients.
        /// </summary>
        public List<ClientConnection> ConnectedClients { get; set; } = new List<ClientConnection>();
        /// <summary>
        /// Actions ms interval for reduce CPU usage.
        /// </summary>
        public int LoopInterval { get; set; } = 50;
        /// <summary>
        /// Socket backlog. The maximum length of the pending connections queue.
        /// </summary>
        public int Backlog { get; set; } = 0;
        /// <summary>
        /// Server type.
        /// </summary>
        public ESocketType ServerType { get; set; }
        /// <summary>
        /// Only for UDP. Require to use SocketClient class for this.
        /// </summary>
        public bool UDPClientManage { get; set; } = true;
        /// <summary>
        /// Only for UDP. Accept 1 byte data for check connected state.
        /// </summary>
        private int _UDPDataInterval { get => (int)(UDPDataInterval * 1.5); }
        public int UDPDataInterval { get; set; } = 5000;

        /// <summary>
        /// Called when new client connected
        /// </summary>
        public event EventHandler<SocketServerClientEventArgs> OnConnected;
        /// <summary>
        /// Called when client disconnected
        /// </summary>
        public event EventHandler<SocketServerClientEventArgs> OnDisconnected;
        /// <summary>
        /// Called when received data from client
        /// </summary>
        public event EventHandler<SocketServerDataEventArgs> OnReceived;
        /// <summary>
        /// Called when server catch error
        /// </summary>
        public event EventHandler<ErrorEventArgs> OnError;
        /// <summary>
        /// Called when executing Start() function
        /// </summary>
        public event EventHandler OnStart;
        /// <summary>
        /// Called when executing Stop() function
        /// </summary>
        public event EventHandler OnStop;

        private Socket Listener { get; set; }
        private System.Timers.Timer DisconnectTimer { get; set; }
        private Dictionary<EndPoint, double> LastDataRecievedTime { get; set; }
        private double TimeNow { get => (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Server type (TCP, UDP)</param>
        /// <param name="address">Server listening ip</param>
        /// <param name="port">Server listening port</param>
        public SocketServer(ESocketType type, IPAddress address, int port)
        {
            Uid = Guid.NewGuid().ToString();
            ServerType = type;
            IPAddress = address;
            Port = port;
            DisconnectTimer = new System.Timers.Timer(LoopInterval);
        }

        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// Start server
        /// </summary>
        public void Start()
        {
            try
            {
                if (ServerType == ESocketType.Tcp)
                {
                    Listener = new Socket(IPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    DisconnectTimer.Elapsed += (s, e) =>
                    {
                        lock (ConnectedClients)
                        {
                            ConnectedClients.RemoveAll(x =>
                            {
                                if (!x.IsConnected)
                                {
                                    OnDisconnected?.Invoke(this, new SocketServerClientEventArgs() { Client = x });
                                    return true;
                                }
                                return false;
                            });
                        }
                    };
                }
                else if (ServerType == ESocketType.Udp)
                {
                    if (UDPClientManage)
                    {
                        LastDataRecievedTime = new Dictionary<EndPoint, double>();
                        DisconnectTimer.Elapsed += (s, e) =>
                        {
                            double now = TimeNow;
                            var times = LastDataRecievedTime;
                            var removed = new List<EndPoint>();
                            foreach (var kp in times)
                            {
                                if (now - kp.Value > _UDPDataInterval / 1000)
                                {
                                    lock (ConnectedClients)
                                    {
                                        var client = ConnectedClients.Where(x => x.Uid == kp.Key.ToString());
                                        if (client.Count() > 0)
                                        {
                                            OnDisconnected?.Invoke(this, new SocketServerClientEventArgs() { Client = client.First() });
                                            ConnectedClients.Remove(client.First());
                                            removed.Add(kp.Key);
                                        }
                                    }
                                }
                            }
                            lock (LastDataRecievedTime)
                            {
                                foreach (var r in removed)
                                {
                                    LastDataRecievedTime.Remove(r);
                                }
                            }
                        };
                    }
                    Listener = new Socket(IPAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                }
                Listener.ReceiveBufferSize = ClientConnection.BufferSize;
                Listener.SendBufferSize = ClientConnection.BufferSize;
                Listener.Bind(new IPEndPoint(IPAddress, Port));
                if (ServerType == ESocketType.Tcp)
                {
                    Listener.Listen(Backlog);
                }
                DisconnectTimer.Start();
                ListenerLoop();
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
        }

        /// <summary>
        /// Stop server
        /// </summary>
        public void Stop()
        {
            try
            {
                DisconnectTimer.Stop();
                foreach (ClientConnection c in ConnectedClients)
                {
                    OnDisconnected?.Invoke(this, new SocketServerClientEventArgs() { Client = c });
                    c.Socket.Close();
                }
                ConnectedClients.Clear();
                OnStop?.Invoke(this, new EventArgs());
                Listener.Close();
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
        }

        private void ListenerLoop()
        {
            try
            {
                OnStart?.Invoke(this, new EventArgs());
                if (ServerType == ESocketType.Tcp)
                {
                    Listener.BeginAccept(new AsyncCallback(AcceptCallback), Listener);
                }
                if (ServerType == ESocketType.Udp)
                {
                    if (UDPClientManage)
                    {
                        byte[] buffer = new byte[ClientConnection.BufferSize];
                        EndPoint remote_ip = new IPEndPoint(IPAddress.Loopback, 0);
                        Listener.BeginReceiveFrom(buffer, 0, ClientConnection.BufferSize, 0, ref remote_ip, new AsyncCallback(AcceptCallbackUDP), new object[] { buffer, remote_ip, Listener });
                    }
                    else
                    {
                        byte[] buffer = new byte[ClientConnection.BufferSize];
                        EndPoint remote_ip = new IPEndPoint(IPAddress.Loopback, 0);
                        Listener.BeginReceiveFrom(buffer, 0, ClientConnection.BufferSize, 0, ref remote_ip, new AsyncCallback(ReadCallbackUDP), new object[] { buffer, remote_ip, Listener });
                    }
                }
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
        }

        #region TCP
        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);
                ClientConnection client = new ClientConnection()
                {
                    Uid = Guid.NewGuid().ToString(),
                    Socket = handler
                };
                lock (ConnectedClients)
                {
                    ConnectedClients.Add(client);
                }
                OnConnected?.Invoke(this, new SocketServerClientEventArgs() { Client = client });
                client.Socket.BeginReceive(client.Buffer, 0, ClientConnection.BufferSize, 0, new AsyncCallback(ReadCallback), client);
                listener.BeginAccept(new AsyncCallback(AcceptCallback), Listener);
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
            catch (ObjectDisposedException) { }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                ClientConnection client = (ClientConnection)ar.AsyncState;
                int bytesRead = client.Socket.EndReceive(ar);
                if (bytesRead > 0)
                {
                    byte[] data = new byte[bytesRead];
                    Array.Copy(client.Buffer, 0, data, 0, bytesRead);
                    OnReceived?.Invoke(this, new SocketServerDataEventArgs() { Client = client, Data = data });
                }
                Thread.Sleep(LoopInterval);
                client.Socket.BeginReceive(client.Buffer, 0, ClientConnection.BufferSize, 0, new AsyncCallback(ReadCallback), client);
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
            catch (ObjectDisposedException) { }
        }
        #endregion

        #region UDP
        private void AcceptCallbackUDP(IAsyncResult ar)
        {
            try
            {
                object[] objects = (object[])ar.AsyncState;
                byte[] buffer = objects[0] as byte[];
                EndPoint remote_ip = objects[1] as EndPoint;
                Socket listener = objects[2] as Socket;
                int bytesRead = listener.EndReceiveFrom(ar, ref remote_ip);
                var clients = ConnectedClients.Where(x => x.Uid == remote_ip.ToString()).ToList();
                ClientConnection client;
                if (clients.Count == 0)
                {
                    client = new ClientConnection()
                    {
                        Uid = remote_ip.ToString(),
                        Socket = new Socket(remote_ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp)
                    };
                    client.SendData += SendData;
                    client.Socket.Connect(remote_ip);
                    if (client.IsConnected)
                    {
                        ConnectedClients.Add(client);
                        OnConnected?.Invoke(this, new SocketServerClientEventArgs() { Client = client });
                    }
                }
                else
                {
                    client = clients.First();
                }
                if (bytesRead > 1)
                {
                    byte[] data = new byte[bytesRead];
                    Array.Copy(buffer, 0, data, 0, bytesRead);
                    OnReceived?.Invoke(this, new SocketServerDataEventArgs() { Client = client, Data = data, });
                }
                else
                {
                    if (LastDataRecievedTime.ContainsKey(remote_ip))
                    {
                        LastDataRecievedTime[remote_ip] = TimeNow;
                    }
                    else
                    {
                        LastDataRecievedTime.Add(remote_ip, TimeNow);
                    }
                }
                Listener.BeginReceiveFrom(buffer, 0, ClientConnection.BufferSize, 0, ref remote_ip, new AsyncCallback(AcceptCallbackUDP), new object[] { buffer, remote_ip, Listener });
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
            catch (ObjectDisposedException) { }
        }

        private void ReadCallbackUDP(IAsyncResult ar)
        {
            try
            {
                object[] objects = (object[])ar.AsyncState;
                byte[] buffer = objects[0] as byte[];
                EndPoint remote_ip = objects[1] as EndPoint;
                Socket listener = objects[2] as Socket;
                int bytesRead = listener.EndReceiveFrom(ar, ref remote_ip);
                if (bytesRead > 0)
                {
                    byte[] data = new byte[bytesRead];
                    Array.Copy(buffer, 0, data, 0, bytesRead);
                    OnReceived?.Invoke(this, new SocketServerDataEventArgs() { Data = data });
                }
                Listener.BeginReceiveFrom(buffer, 0, ClientConnection.BufferSize, 0, ref remote_ip, new AsyncCallback(ReadCallbackUDP), new object[] { buffer, remote_ip, Listener });
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
            catch (ObjectDisposedException) { }
        }
        #endregion

        private void DisconnectCallback(IAsyncResult ar)
        {
            try
            {
                ClientConnection client = (ClientConnection)ar.AsyncState;
                client.Socket.EndDisconnect(ar);
                OnDisconnected?.Invoke(this, new SocketServerClientEventArgs() { Client = client });
                lock (ConnectedClients)
                {
                    ConnectedClients.Remove(client);
                }
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
        }

        public void SendData(byte[] data, EndPoint point)
        {
            try
            {
                Listener.BeginSendTo(data, 0, data.Length, 0, point, new AsyncCallback(EndSendTo), null);
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
        }

        private void EndSendTo(IAsyncResult ar)
        {
            try
            {
                Listener.EndSendTo(ar);
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
        }
    }
}