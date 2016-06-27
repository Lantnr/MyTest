using System;
using System.Collections.Generic;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Scene.Service
{
    /// <summary>
    /// 推送玩家离开
    /// </summary>
    public class PLAYER_EXIT_SCENET
    {
      
        /// <summary>推送玩家离开</summary>
        public void CommandStart( Int64 userid, Int64 otheruserid)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1} - {2} - {3}", "PLAYER_EXIT_SCENET", "推送玩家离开场景", userid, otheruserid);
#endif
            var aso = BuildData(userid);
            Common.GetInstance().SendPv(userid, aso, (int)SceneCommand.PLAYER_EXIT_SCENET, otheruserid);
        }

        /// <summary> 组装数据 </summary>
        private ASObject BuildData(Int64 userid)
        {
            var dic = new Dictionary<string, object>();
            const int result = (int)ResultType.SUCCESS;
            dic.Add("result", result);
            dic.Add("userId", userid);
            return new ASObject(dic);
        }

    }
}
