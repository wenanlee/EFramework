using System;
using System.Net;
using System.Net.Sockets;

namespace EFramework.Network
{
    public delegate void SendBytesTo(byte[] data, EndPoint point);

    public class ClientConnection : IDisposable
    {
        /// <summary>
        /// Unique client id.
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// Client socket.
        /// </summary>
        public Socket Socket { get; set; }
        /// <summary>
        /// Recieved temp buffer.
        /// </summary>
        public byte[] Buffer { get; set; }
        /// <summary>
        /// Buffer size, increase if you want receive big data size.
        /// </summary>
        public static int BufferSize { get; set; } = 32768;
        /// <summary>
        /// Socket connection state.
        /// </summary>
        public bool IsConnected => !((Socket.Poll(1000, SelectMode.SelectRead) && (Socket.Available == 0)) || !Socket.Connected);
        /// <summary>
        /// Client ip.
        /// </summary>
        public IPAddress RemoteIP => (Socket.RemoteEndPoint as IPEndPoint)?.Address;
        /// <summary>
        /// Client port.
        /// </summary>
        public int RemotePort => (Socket.RemoteEndPoint as IPEndPoint).Port;

        public event SendBytesTo SendData;

        /// <summary>
        /// ClientConnection class constructor.
        /// </summary>
        public ClientConnection()
        {
            Buffer = new byte[BufferSize];
        }

        public void Dispose()
        {
            Socket.Close();
        }

        /// <summary>
        /// Send data to client.
        /// </summary>
        /// <param name="data">Data for sending</param>
        public void Send(byte[] data)
        {
            if (Socket.SocketType == SocketType.Stream)
            {
                Socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(EndSend), null);
            }
            else if (Socket.SocketType == SocketType.Dgram)
            {
                if (SendData != null)
                {
                    SendData?.Invoke(data, Socket.RemoteEndPoint);
                }
                else
                {
                    Socket.BeginSendTo(data, 0, data.Length, 0, Socket.RemoteEndPoint, new AsyncCallback(EndSendTo), null);
                }
            }
        }

        private void EndSend(IAsyncResult ar)
        {
            Socket.EndSend(ar);
        }

        private void EndSendTo(IAsyncResult ar)
        {
            Socket.EndSendTo(ar);
        }
    }
}