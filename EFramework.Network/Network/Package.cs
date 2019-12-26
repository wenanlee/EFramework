using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EFramework.Network
{
    public class PackageBase
    {
        public int type;
        public string msg;
        public PackageBase() { }
        public PackageBase(int type, string msgs)
        {
            this.type = type;
            this.msg = string.Join("|", msgs);
        }
        public virtual byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes(type.ToString("0000") + msg);
        }
        public virtual string[] GetStrings()
        {
            return msg.Split('|');
        }
    }
    public class Package : PackageBase
    {
        public Package(Enum type, params string[] msgs) : base(type.GetHashCode(), string.Join("|", msgs)) { }
        public Package(Enum type, params int[] msgs) : base(type.GetHashCode(), string.Join("|", msgs)) { }
        public Package(int type, params string[] msgs) : base(type, string.Join("|", msgs)) { }
        public Package(int type, params int[] msgs) : base(type, string.Join("|", msgs)) { }

        public Package(byte[] bytes, long offset, long size) : base(int.Parse(Encoding.UTF8.GetString(bytes, (int)offset, 4)), Encoding.UTF8.GetString(bytes, (int)offset + 4, (int)size - 4)) { }
        public Package(string msg) : base(int.Parse(msg.Substring(0, 4)), msg.Substring(4).TrimEnd('\0')) { }
        public Package() { }
        public override string ToString()
        {
            return ((int)type).ToString("0000") + msg;
        }
        public string GetString()
        {
            return msg;
        }
        public int GetInt()
        {
            return int.Parse(msg);
        }
        public int[] GetInts()
        {
            return Array.ConvertAll(msg.Split('|'), s => int.Parse(s));
        }
    }
    public class PackagePlus : Package
    {
        public int size;
        private byte[] bytes;
        public PackagePlus(Enum type, params string[] msgs)
        {
            this.type = type.GetHashCode();
            this.msg = string.Join("|", msgs);
            //        type size
            this.size = 4 + 4 + msg.Length;
            bytes = Encoding.UTF8.GetBytes(this.type.ToString("0000") + size.ToString("0000") + msg);
        }
        public PackagePlus(byte[] bytes, long offset, long size)
        {
            this.type = int.Parse(Encoding.UTF8.GetString(bytes, (int)offset, 4));
            this.size = int.Parse(Encoding.UTF8.GetString(bytes, (int)offset + 4, 4));
            //this.msg = Encoding.UTF8.GetString(bytes, (int)offset + 8, this.size);
            this.msg = Encoding.UTF8.GetString(bytes, (int)offset + 8, bytes.Length-8);
        }
        public PackagePlus()
        {
            
        }
        public PackagePlus(string msg)
        {
            this.type = int.Parse(msg.Substring(0,4));
            this.size = int.Parse(msg.Substring(4, 4));
            this.msg = msg.Substring(8);
        }
        public override byte[] ToBytes()
        {
            return bytes;
        }
        public override string[] GetStrings()
        {
            return msg.Split('|');
        }
        public override string ToString()
        {
            return type.ToString("0000") + size.ToString("0000") + msg;
        }

        
    }
}
