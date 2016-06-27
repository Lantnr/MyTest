using System;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Scene.Service
{
    /// <summary>
    /// 推送玩家进入当前场景
    /// </summary>
    public class PLAYER_ENTER_SCENE
    {

        /// <summary>推送玩家进入当前场景</summary>
        public void CommandStart(view_scene_user sceneplayer, Int64 userid, Int64 otheruserid)
        {
            const int result = (int)ResultType.SUCCESS;
            var aso = new ASObject(Common.GetInstance().BuildData(result, sceneplayer));
#if DEBUG
            XTrace.WriteLine("玩家{0}向其他玩家推送上线！\t", userid);
#endif
            Common.GetInstance().SendPv(userid, aso, (int)SceneCommand.PLAYER_ENTER_SCENE, otheruserid);
        }
    }
}
