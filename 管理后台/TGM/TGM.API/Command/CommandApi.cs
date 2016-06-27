using System.Runtime.Serialization;
using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluorineFx.AMF3;
using NewLife.Log;
using TGG.Core.AMF;
using TGM.API.Entity.Enum;
using TGM.API.Entity.Helper;
using TGM.API.Entity.Model;
using TGM.API.Entity.Vo;

namespace TGM.API.Command
{
    /// <summary>
    /// 游戏指令API类
    /// </summary>
    public class CommandApi:IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~CommandApi()
        {
            Dispose();
        }
    
        #endregion

        #region

        /// <summary>IP</summary>
        public String IP { get; set; }

        /// <summary>端口</summary>
        public Int32 Port { get; set; }

        /// <summary>模块号</summary>
        public const Int32 MN = 9999;

        /// <summary>指令号</summary>
        public ApiCommand CN { get; set; }

        /// <summary>构造函数</summary>
        public CommandApi() { }

        /// <summary>构造函数</summary>
        /// <param name="ip">IP</param>
        /// <param name="port">端口</param>
        /// <param name="cn">指令号</param>
        public CommandApi(string ip, Int32 port, ApiCommand cn)
        {
            IP = ip;
            Port = port;
            CN = cn;
        }



        #endregion

        internal void Test()
        {
            var s = new MvcSocket(IP, Port);

            var data = Build(new ASObject());
            s.Send(data);
            var result = s.Receive();
            s.Dispose();
        }

        /// <summary>组建发送协议</summary>
        /// <param name="aso">协议数据</param>
        protected byte[] Build(ASObject aso)
        {
            //组装发送数据
            var pv = new ProtocolVo
            {
                status = 0,
                sendTime = 30000,
                serialNumber = 1,
                verificationCode = 1,
                serverTime = DateTime.Now.Ticks,
                moduleNumber = MN,                  //模块号
                commandNumber = (Int32)CN,      //指令号
                data = aso,
            };

            var protocol = new Protocol(pv);
            return protocol.AFMData();
        }

        /// <summary>
        /// 充值
        /// </summary>
        internal Int32 Recharge(Int64 user_id, Int32 amount)
        {
            var s = new MvcSocket(IP, Port);

            var str = string.Format("{0}|{1}", user_id, amount);
            var dic = new Dictionary<string, object>()
            {
                {"key","" },
                {"data",CryptoHelper.Encrypt(str,"") },
            };
            XTrace.WriteLine("充值:{0}", str);
            var data = Build(new ASObject(dic));
            s.Send(data);
            var result = s.Receive();
            s.Dispose();
            //解析返回结果
            var len = result.Length - 8;
            var da = new byte[len];
            Array.Copy(result, 8, da, 0, len);

            var bytea = new ByteArray(da);
            var dy = bytea.ReadObject();
            var retype = dy.GetType().Name;
            //XTrace.WriteLine("retype:{0}", retype);
            ASObject obj;
            switch (retype)  //判断接收数据类型
            {
                case "ASObject": { obj = (ASObject)dy; break; }
                case "ProtocolVo": { obj = AMFConvert.ToASObject(dy); break; }
                default: { return (int)ApiType.FAIL; }
            }
            var pv = AutoParseAsObject<ProtocolVo>.Parse(obj);
            //XTrace.WriteLine("pv.className:{0}", pv.className);
            if (pv.className != "ProtocolVo") return (int)ApiType.FAIL;
            var r = pv.data;

            // 解析返回结果数据
            var _state = Convert.ToInt32(r.FirstOrDefault(q => q.Key == "state").Value);

            return _state;
        }

        /// <summary>
        /// Gm操作玩家信息
        /// </summary>
        internal Int32 Gmoperate(Int64 userId)
        {
#if DEBUG
            IP = "192.168.1.111";
            Port = 10086;
#endif
            var s = new MvcSocket(IP, Port);

            var str = string.Format("{0}", userId);
            var dic = new Dictionary<string, object>()
            {
                {"key","" },
                {"data",CryptoHelper.Encrypt(str,"") },
            };

            var data = Build(new ASObject(dic));
            s.Send(data);
            var result = s.Receive();
            s.Dispose();
            //解析返回结果
            var len = result.Length - 8;
            var da = new byte[len];
            Array.Copy(result, 8, da, 0, len);

            var bytea = new ByteArray(da);
            var dy = bytea.ReadObject();
            var retype = dy.GetType().Name;
            ASObject obj;
            switch (retype)  //判断接收数据类型
            {
                case "ASObject": { obj = (ASObject)dy; break; }
                case "ProtocolVo": { obj = AMFConvert.ToASObject(dy); break; }
                default: { return (int)ApiType.FAIL; }
            }
            var pv = AutoParseAsObject<ProtocolVo>.Parse(obj);
            if (pv.className != "ProtocolVo") return (int)ApiType.FAIL;
            var r = pv.data;

            // 解析返回结果数据
            var _state = Convert.ToInt32(r.FirstOrDefault(q => q.Key == "state").Value);

            return _state;
        }


        /// <summary>
        /// 发送即时公告
        /// </summary>
        internal Int32 NoticePush()
        {
#if DEBUG
            IP = "192.168.1.111";
            Port = 10086;
#endif
            var s = new MvcSocket(IP, Port);
            var data = Build(new ASObject());
            s.Send(data);
            var result = s.Receive();
            s.Dispose();

            //解析返回结果
            var len = result.Length - 8;
            var da = new byte[len];
            Array.Copy(result, 8, da, 0, len);

            var bytea = new ByteArray(da);
            var dy = bytea.ReadObject();
            var retype = dy.GetType().Name;
            ASObject obj;
            switch (retype)  //判断接收数据类型
            {
                case "ASObject": { obj = (ASObject)dy; break; }
                case "ProtocolVo": { obj = AMFConvert.ToASObject(dy); break; }
                default: { return (int)ApiType.FAIL; }
            }
            var pv = AutoParseAsObject<ProtocolVo>.Parse(obj);
            if (pv.className != "ProtocolVo") return (int)ApiType.FAIL;
            var r = pv.data;

            // 解析返回结果数据
            var state = Convert.ToInt32(r.FirstOrDefault(q => q.Key == "state").Value);
            return state;
        }

        /// <summary>
        /// 停止公告
        /// </summary>
        internal Int32 StopNotice(Int64 nid)
        {
#if DEBUG
            IP = "192.168.1.111";
            Port = 10086;
#endif
            var s = new MvcSocket(IP, Port);
            var str = string.Format("{0}", nid);
            var dic = new Dictionary<string, object>()
            {
                {"key","" },
                {"data",CryptoHelper.Encrypt(str,"") },
            };

            var data = Build(new ASObject(dic));
            s.Send(data);
            var result = s.Receive();
            s.Dispose();
            //解析返回结果
            var len = result.Length - 8;
            var da = new byte[len];
            Array.Copy(result, 8, da, 0, len);

            var bytea = new ByteArray(da);
            var dy = bytea.ReadObject();
            var retype = dy.GetType().Name;
            ASObject obj;
            switch (retype)  //判断接收数据类型
            {
                case "ASObject": { obj = (ASObject)dy; break; }
                case "ProtocolVo": { obj = AMFConvert.ToASObject(dy); break; }
                default: { return (int)ApiType.FAIL; }
            }
            var pv = AutoParseAsObject<ProtocolVo>.Parse(obj);
            if (pv.className != "ProtocolVo") return (int)ApiType.FAIL;
            var r = pv.data;

            // 解析返回结果数据
            var state = Convert.ToInt32(r.FirstOrDefault(q => q.Key == "state").Value);
            return state;
        }
    }

    /// <summary>
    /// API 指令枚举
    /// </summary>
    public enum ApiCommand
    {
        充值 = 1,
        冻结封号 = 2,
        停止公告 = 3,
        公告 = 4,
    }
}