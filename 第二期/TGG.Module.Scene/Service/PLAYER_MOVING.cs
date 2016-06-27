using System;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Scene.Service
{
    /// <summary>
    /// 推送玩家移动
    /// </summary>
    public partial class MOVING
    {

        /// <summary>推送玩家移动</summary>
        public void PlayerMoving( int x, int y, Int64 userid, Int64 otheruserid)
        {
            const int result = (int)ResultType.SUCCESS;
            var aso = new ASObject(Common.GetInstance().BuildData(result, userid, x, y));
            Common.GetInstance().SendPv(userid, aso, (int)SceneCommand.PLAYER_MOVING, otheruserid);
        }
    }
}
