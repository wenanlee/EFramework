using System;

namespace EFramework.Network
{
    public class SocketServerDataEventArgs : EventArgs
    {
        /// <summary>
        /// Client connection.
        /// </summary>
        public ClientConnection Client { get; set; }
        /// <summary>
        /// Recieved data.
        /// </summary>
        public byte[] Data { get; set; }
    }

    public class SocketServerClientEventArgs : EventArgs
    {
        /// <summary>
        /// Client connection.
        /// </summary>
        public ClientConnection Client { get; set; }
    }
}
