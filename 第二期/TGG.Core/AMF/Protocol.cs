using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.AMF
{
    /// <summary>
    /// Class Protocol.
    /// AMF协议类
    /// </summary>
    public class Protocol
    {
        private const int HEAD = -1887536;

        /// <summary>AMF协议头</summary>
        public byte[] AMF_HEAD { get; set; }

        /// <summary>AMF协议长度</summary>
        public byte[] AMF_LEN { get; set; }

        /// <summary>AMF协议内容</summary>
        public byte[] AMF_BADY { get; set; }

        /// <summary>构造函数</summary>
        public Protocol()
        {
            AMF_HEAD = Common.Util.UConvert.IntToBytes(HEAD);
        }

        /// <summary>构造函数</summary>
        public Protocol(byte[] bady)
            : this()
        {
            AMF_LEN = Common.Util.UConvert.IntToBytes(bady.Length);
            AMF_BADY = bady;
        }

        /// <summary>构造函数</summary>
        public Protocol(object bady)
            : this()
        {
            AMF_BADY = AMFConvert.AMF_Serializer(bady);
            AMF_LEN = Common.Util.UConvert.IntToBytes(AMF_BADY.Length);
        }

        /// <summary>AMF协议数据</summary>
        public byte[] AFMData()
        {
            return Common.Util.UConvert.Combine(AMF_HEAD, AMF_LEN, AMF_BADY);
        }

        /// <summary>解析协议包获取数据内容</summary>
        /// <param name="packet">协议包</param>
        /// <returns>协议包内容</returns>
        public void Analyze(byte[] packet)
        {
            var head = new byte[4];
            Array.Copy(packet, 0, head, 0, 4);
            AMF_HEAD = head;
            var dl = new byte[4];
            Array.Copy(packet, 4, dl, 0, 4);
            AMF_LEN = dl;
            var len = packet.Length - 8;//前4位是AMF协议头,后4位是AMF协议长度
            var data = new byte[len];
            Array.Copy(packet, 8, data, 0, len);
            AMF_BADY = data;
        }

    }
}
