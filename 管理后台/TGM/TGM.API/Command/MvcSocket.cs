/**************************************************************************************
* 对象名称: MvcSocket 
* 功能说明: 远程发送与接收 
* 试用示例: 
* using TGM.API.Command;                //引用空间名 
* string url = "192.168.1.254";         //URL也可以是(http://www.baidu.com/)这种形式 
* int port = 10086;                     //端口 
* string SendString = "发送数据";       //组织要发送的字符串 
* MvcSocket s = new MvcSocket();        //创建新对象 
* s.Connection(url, port);              //打开远程端口 
* s.Send(SendString);                   //发送数据 
* Response.Write(s.Receive());          //接收数据 
* s.Dispose();                          //销毁对象 
***************************************************************************************/

using System;
using System.Net.Sockets;
using System.Text;
namespace TGM.API.Command
{
    /// <summary>
    /// socket类
    /// </summary>
    public class MvcSocket : IDisposable
    {
        private NetworkStream ns;
        private bool _alreadyDispose;

        #region 构造与释构
        /// <summary>构造函数</summary>
        public MvcSocket() { }

        /// <summary>构造函数</summary>
        public MvcSocket(string url, int port)
        {
            Connection(url, port);
        }

        /// <summary>析构函数</summary>
        ~MvcSocket()
        {
            Dispose();
        }
        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDispose) return;
            if (isDisposing)
            {
                if (ns != null)
                {
                    try
                    {
                        ns.Close();
                    }
                    catch (Exception e) { }
                    ns.Dispose();
                }
            }
            _alreadyDispose = true;
        }
        #endregion

        #region 打开端口
        /// <summary>打开端口</summary> 
        /// <param name="url">URL或者:IP地址</param> 
        /// <param name="port"></param> 
        public virtual void Connection(string url, int port)
        {
            if (string.IsNullOrEmpty(url)) return;
            if (port < 0) return;
            if (port == 0) port = 80;
            TcpClient tcp;
            try
            {
                tcp = new TcpClient(url, port);
            }
            catch (Exception e)
            {
                throw new Exception("Can't connection:" + url);
            }
            ns = tcp.GetStream();
        }
        #endregion

        #region 发送Socket
        /// <summary>发送Socket</summary> 
        public virtual bool Send(string message)
        {
            if (ns == null) return false;
            if (string.IsNullOrEmpty(message)) return false;
            var buf = Encoding.ASCII.GetBytes(message);
            try
            {
                ns.Write(buf, 0, buf.Length);
            }
            catch (Exception e)
            {
                throw new Exception("Send Date Fail!");
            }
            return true;
        }
        /// <summary>发送Socket</summary> 
        public virtual bool Send(byte[] data)
        {
            if (ns == null) return false;
            try
            {
                ns.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                throw new Exception("Send Date Fail!");
            }
            return true;
        }
        #endregion

        #region 收取信息
        /// <summary>接收数据</summary> 
        public byte[] Receive()
        {
            if (ns == null) return null;
            var buf = new byte[4096];
            var length = 0;
            try
            {
                length = ns.Read(buf, 0, buf.Length);
            }
            catch (Exception e)
            {
                throw new Exception("Receive data fail!");
            }
            var da = new byte[length];
            Array.Copy(buf, 0, da, 0, length);
            return da;
        }
        #endregion

        #region IDisposable 成员
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}