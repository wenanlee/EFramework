using System;
using System.Text;
using PENet;

namespace Protocol {
    [Serializable]
    public class StringMsg : PEMsg {
        public string msg;
    }
    public class IntMsg : PEMsg
    {
        public int msg;
    }
    public class BytesMsg : PEMsg
    {
        public byte[] msg;
        public string GetString()
        {
            return Encoding.UTF8.GetString(msg);
        }
        public int GetInt()
        {
            return 0;
        }
    }

    public class IPCfg {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 60000;
    }
}