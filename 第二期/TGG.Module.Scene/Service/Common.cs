using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using FluorineFx.Configuration;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.Scene;
using TGG.SocketServer;
using tg_scene = TGG.Core.Entity.tg_scene;
using view_scene_user = TGG.Core.Entity.view_scene_user;


namespace TGG.Module.Scene.Service
{
    /// <summary>
    /// 场景公共方法类
    /// </summary>
    public class Common
    {
        public static Common ObjInstance = null;

        /// <summary>Common 单体模式</summary>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }

        #region 组装数据

        /// <summary>数据组装</summary>
        public Dictionary<string, object> BuildData(object result, decimal sceneid, int x, int y, dynamic list_sceneplayers)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"id", sceneid},
                {"x", x},
                {"y", y},
                {
                    "playerList",
                    list_sceneplayers.Count > 0 ? ConvertListASObject(list_sceneplayers, "ScenePlayer") : null
                }
            };
            return dic;
        }

        /// <summary>数据组装</summary>
        public Dictionary<string, object> BuildData(object result, decimal userid, int x, int y)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"userId", userid}, 
                {"x", x}, 
                {"y", y}
            };
            return dic;
        }

        /// <summary>数据组装</summary>
        public Dictionary<string, object> BuildData(object result, view_scene_user sceneplayer)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"playerVo", sceneplayer != null ? EntityToVo.ToScenePlayerVo(sceneplayer)  : null}
            };
            return dic;
        }

        /// <summary>将dynamic对象转换成ASObject对象</summary>
        public List<ASObject> ConvertListASObject(dynamic list, string classname)
        {
            var list_aso = new List<ASObject>();
            foreach (var item in list)
            {
                dynamic model;
                switch (classname)
                {
                    case "ScenePlayer": model = EntityToVo.ToScenePlayerVo(item); break;
                    default: model = null; break;
                }
                list_aso.Add(AMFConvert.ToASObject(model));
            }
            return list_aso;
        }

        #endregion

        #region 发送协议
        public void Send(TGGSession session, ASObject data, int commandNumber)
        {
            var pv = new ProtocolVo
            {
                serialNumber = 1,
                verificationCode = 1,
                moduleNumber = (int)ModuleNumber.SCENE,
                commandNumber = commandNumber,
                sendTime = 1000,
                serverTime = (DateTime.Now.Ticks - 621355968000000000) / 10000,
                status = (int)ResponseType.TYPE_SUCCESS,
                data = data,
            };
            session.SendData(pv);
        }

        public void SendPv(Int64 userid, ASObject aso, int commandnumber, Int64 otheruserid)
        {
            #region 测试数据
            //Random rd= new Random();
            //for (int i = 10000; i < 10250; i++)
            //{
            //    var aso1 = new ASObject(Common.GetInstance().BuildData(0, i, rd.Next(1, 2000), rd.Next(1, 2000)));
            //    if (!Variable.OnlinePlayer.ContainsKey(otheruserid)) return;
            //    var session1 = Variable.OnlinePlayer[otheruserid] as TGGSession;
            //    if (session1 == null) return;
            //    Send(session1, aso1, commandnumber);
            //}
            #endregion

            if (!Variable.OnlinePlayer.ContainsKey(otheruserid)) return;
            var session = Variable.OnlinePlayer[otheruserid] as TGGSession;
            if (session == null) return;
            Send(session, aso, commandnumber);
        }

      
        #endregion

        #region 公共方法

        /// <summary>将内存的数据更新到数据库</summary>
        public void GetSceneData()
        {
            if (!Variable.SCENCE.Any()) return;
            foreach (var item in Variable.SCENCE.Keys)
            {
                tg_scene.GetSceneUpdate(Variable.SCENCE[item]);
            }
        }

        #endregion

        public class ScenePush
        {
            /// <summary> 场景数据 </summary>
            public view_scene_user user_scene { get; set; }

            /// <summary>用户id </summary>
            public Int64 user_id { get; set; }

            /// <summary>其他用户id </summary>
            public Int64 other_user_id { get; set; }

        }
    }
}
