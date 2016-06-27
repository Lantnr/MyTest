using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Building.Service
{
    /// <summary>
    /// 一夜墨俣--退出活动
    /// 开发者：李德雁
    /// </summary>
    public class EXIT
    {
        private static EXIT _objInstance;

        /// <summary>EXIT 单体模式</summary>
        public static EXIT GetInstance()
        {
            return _objInstance ?? (_objInstance = new EXIT());
        }


        /// <summary> 一夜墨俣--退出活动</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("一夜墨俣：退出 -{0}——{1}", session.Player.User.player_name, "EXIT");
#endif
            var userid = session.Player.User.id;
            var myscene = session.Player.Scene;
            var key = ModuleNumber.BUILDING + "_" + session.Player.User.id;
            var acp = Variable.Activity.ScenePlayer.ContainsKey(key); //活动内是否有该玩家
            if (acp)
            {
                var scene = new view_scene_user();
                Variable.Activity.ScenePlayer.TryRemove(key, out scene);
            }
            Common.GetInstance().PusuLeaveActivity(userid); //向活动内的其他玩家推送离开
            Common.GetInstance().PushMyEnterScene(userid,myscene); //向活动外场景的其他玩家推送进入场景
            var dic = new Dictionary<string, object>() { { "result", (int)ResultType.SUCCESS } };
            return new ASObject(dic);
        }
    }
}
