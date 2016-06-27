using System;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.Common;
using TGG.Core.Common.Util;

namespace TGG.SocketServer
{
    /// <summary>
    /// 协议过滤
    /// </summary>
    public class TGGReceiveFilter : FixedHeaderReceiveFilter<BinaryRequestInfo>
    {
        public TGGReceiveFilter(): base(8){}

        protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            return new BinaryRequestInfo(BitConverter.ToString(header.Array, header.Offset, 4), bodyBuffer.CloneRange(offset, length));
        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            var bl = new byte[4];
            Array.Copy(header, offset + 4, bl, 0, 4);
            var len = UConvert.ByteToInt(bl);
            return len;
        }
    }
}
