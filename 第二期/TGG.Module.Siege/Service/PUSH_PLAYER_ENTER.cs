using System;
using System.Collections.Generic;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Vo.Scene;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 其他玩家进入美浓攻略
    /// </summary>
    public class PUSH_PLAYER_ENTER
    {
        private static PUSH_PLAYER_ENTER _objInstance;

        /// <summary>PUSH_PLAYER_ENTER 单体模式</summary>
        public static PUSH_PLAYER_ENTER GetInstance()
        {
            return _objInstance ?? (_objInstance = new PUSH_PLAYER_ENTER());
        }

        /// <summary>推送其他玩家进入美浓攻略</summary>
        public void CommandStart(ScenePlayerVo model, TGGSession session, Int64 otheruserid)
        {
            var aso = new ASObject(BuildData(model));
           Common.GetInstance().PushPv(session, aso, (int)SiegeCommand.PUSH_PLAYER_ENTER, otheruserid);
        }


        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(ScenePlayerVo model)
        {
            var dic = new Dictionary<string, object>
            {
                {"playerVo", model}
            };
            return dic;
        }
    }
}
