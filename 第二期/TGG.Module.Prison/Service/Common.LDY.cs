using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using FluorineFx.Messaging.Rtmp.SO;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using NewLife.Log;

namespace TGG.Module.Prison.Service
{
    public partial class Common
    {

        #region 组装数据
        public ASObject BuildData(int result)
        {
            var dic = new Dictionary<string, object>() { { "result", result } };
            return new ASObject(dic);
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(object result, decimal sceneid, int x, int y, dynamic list_sceneplayers)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"id", sceneid},
                {"x", x},
                {"y", y},
                { "playerList", list_sceneplayers.Count > 0 ? ConvertListASObject(list_sceneplayers, "ScenePlayer") : null
                }
            };
            return dic;
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(object result, decimal userid, int x, int y)
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
        private Dictionary<String, Object> BuildData(object result, view_scene_user sceneplayer)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"playerVo", sceneplayer != null ? EntityToVo.ToScenePlayerVo(sceneplayer)  : null}
            };
            return dic;
        }

        public ASObject BuildData(int result, double time, int count, List<view_scene_user> sceneplayer)
        {
            List<ASObject> player;
            if (sceneplayer == null) player = null;
            else
                player = sceneplayer.Any() ? ConvertListASObject(sceneplayer, "ScenePlayer") : null;
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"time",time},
                {"count",count},
                {"playerList",player }
            };
            return new ASObject(dic);
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

        #region 推送协议
        public void SendPv(Int64 userid, ASObject aso, int commandnumber, decimal otheruserid, int modulenumber)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var key = string.Format("{0}_{1}_{2}", modulenumber, commandnumber, otheruserid);
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            session.SPM.AddOrUpdate(key, aso, (m, n) => aso);
        }
        public void SendPv(Int64 userid, ASObject aso, int commandnumber, int modulenumber)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var key = string.Format("{0}_{1}", modulenumber, commandnumber);
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            session.SPM.AddOrUpdate(key, aso, (m, n) => aso);
        }
        #endregion

        #region 公共方法

        #endregion
    }
}
