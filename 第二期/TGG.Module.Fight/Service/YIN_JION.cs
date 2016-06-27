using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Fight.Service
{
    /// <summary>
    /// 拉取印数据
    /// </summary>
    public class YIN_JION
    {
        public static YIN_JION ObjInstance;

        /// <summary>
        /// YIN_JION单体模式
        /// </summary>
        /// <returns></returns>
        public static YIN_JION GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new YIN_JION());
        }

        /// <summary>
        /// 拉取印数据
        /// </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var user = session.Player.User;
            var list = tg_fight_yin.GetFindByUserId(user.id);
            return new ASObject(BuildData((int)ResultType.SUCCESS, list));
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result, List<tg_fight_yin> list)
        {
            var dic = new Dictionary<string, object>
            { 
            { "result", result },
            { "yin",list.Any()? Common.GetInstance().ConvertListYinVo(list):null } ,
            };
            return dic;
        }
    }
}
