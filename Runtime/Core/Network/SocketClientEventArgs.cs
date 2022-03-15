using System;

namespace EFramework.Network
{
    public class SocketClientEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }
}
