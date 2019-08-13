using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace EFramework.Network
{
    public class SocketClient : ClientConnection, IDisposable
    {
        /// <summary>
        /// Client type
        /// </summary>
        public ESocketType ClientType { get; set; }
        /// <summary>
        /// Actions ms interval for reduce CPU usage.
        /// </summary>
        public int LoopInterval { get; set; } = 50;
        /// <summary>
        /// Only for UDP. Sending 1 byte data for check connected state.
        /// </summary>
        public int UDPDataInterval { get; set; } = 5000;

        /// <summary>
        /// Called when client connected
        /// </summary>
        public event EventHandler OnConnected;
        /// <summary>
        /// Called when client disconnected
        /// </summary>
        public event EventHandler OnDisconnected;
        /// <summary>
        /// Called when received data from server
        /// </summary>
        public event EventHandler<SocketClientEventArgs> OnReceived;
        /// <summary>
        /// Called when client catch error
        /// </summary>
        public event EventHandler<ErrorEventArgs> OnError;

        private System.Timers.Timer UDPTimer { get; set; }
        private System.Timers.Timer DisconnectTimer { get; set; }

        /// <summary>
        /// SocketClient class constructor.
        /// </summary>
        /// <param name="type">Client type</param>
        public SocketClient(ESocketType type)
        {
            ClientType = type;
            UDPTimer = new System.Timers.Timer(UDPDataInterval);
            UDPTimer.Elapsed += (s, e) =>
            {
                if (IsConnected)
                    Socket.Send(new byte[] { 0 });
            };
            DisconnectTimer = new System.Timers.Timer(LoopInterval);
            DisconnectTimer.Elapsed += (s, e) =>
            {
                if (!IsConnected)
                {
                    Disconnect();
                }
            };
        }

        public new void Dispose()
        {
            Disconnect();
        }

        /// <summary>
        /// Connect to remote host
        /// </summary>
        /// <param name="address">Remote ip address</param>
        /// <param name="port">Remote port</param>
        public void Connect(IPAddress address, int port) => Connect(new IPEndPoint(address, port));

        /// <summary>
        /// Connect to remote host
        /// </summary>
        /// <param name="point">Remote end point</param>
        public void Connect(IPEndPoint point)
        {
            try
            {
                if (ClientType == ESocketType.Tcp)
                {
                    Socket = new Socket(point.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    Socket.BeginConnect(point, new AsyncCallback(ConnectCallback), null);
                }
                else if (ClientType == ESocketType.Udp)
                {
                    Socket = new Socket(point.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                    Socket.Connect(point);
                    Socket.Send(new byte[] { 0 });
                    OnConnected?.Invoke(this, new EventArgs());
                    UDPTimer.Start();
                    DisconnectTimer.Start();
                    Socket.BeginReceive(Buffer, 0, BufferSize, 0, new AsyncCallback(ReadCallbackUDP), null);
                }
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
        }

        /// <summary>
        /// Disconnect from remote host
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (Socket == null)
                    return;
                UDPTimer.Stop();
                DisconnectTimer.Stop();
                Socket.Close();
                OnDisconnected?.Invoke(this, new EventArgs());
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket.EndConnect(ar);
                OnConnected?.Invoke(this, new EventArgs());
                DisconnectTimer.Start();
                Socket.BeginReceive(Buffer, 0, BufferSize, 0, new AsyncCallback(ReadCallback), null);
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                if (!Socket.Connected)
                    return;
                int readBytes = Socket.EndReceive(ar);
                if (readBytes > 0)
                {
                    byte[] data = new byte[readBytes];
                    Array.Copy(Buffer, data, readBytes);
                    OnReceived?.Invoke(this, new SocketClientEventArgs() { Data = data });
                }
                Socket.BeginReceive(Buffer, 0, BufferSize, 0, new AsyncCallback(ReadCallback), null);
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
        }

        private void ReadCallbackUDP(IAsyncResult ar)
        {
            try
            {
                if (!Socket.Connected)
                    return;
                int readBytes = Socket.EndReceive(ar);
                if (readBytes > 0)
                {
                    byte[] data = new byte[readBytes];
                    Array.Copy(Buffer, data, readBytes);
                    OnReceived?.Invoke(this, new SocketClientEventArgs() { Data = data });
                }
                Socket.BeginReceive(Buffer, 0, BufferSize, 0, new AsyncCallback(ReadCallbackUDP), null);
            }
            catch (SocketException se)
            {
                OnError?.Invoke(this, new ErrorEventArgs(se));
            }
        }
    }
}
