using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Building.Service
{
    /// <summary>
    /// 传送回出生点
    /// 开发者：李德雁
    /// </summary>
    public class BACK_POINT
    {
        private static BACK_POINT _objInstance;

        /// <summary>BACK_POINT 单体模式</summary>
        public static BACK_POINT GetInstance()
        {
            return _objInstance ?? (_objInstance = new BACK_POINT());
        }

        /// <summary> 一夜墨俣--传送回出生点</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("一夜墨俣： 传送回出生点{0}——{1}", session.Player.User.player_name, "BACK_POINT");
#endif
            var userid = session.Player.User.id;
            var key = (int)ModuleNumber.BUILDING + "_" + session.Player.User.id;
            if (!Variable.Activity.ScenePlayer.ContainsKey(key)) return BuildData((int)ResultType.BUILDING_SCENE_ERROR);
            var acp = Variable.Activity.ScenePlayer[key];
            if (acp == null) return BuildData((int)ResultType.BUILDING_SCENE_ERROR);
            Common.GetInstance().GetActivityPoint(acp, session.Player.User.player_camp); //初始坐标
            Common.GetInstance().PushRestartActivity(userid); //向活动内玩家推送回到出生点
            return BuildData((int)ResultType.SUCCESS);
        }

        /// <summary> 组装数据</summary>
        private ASObject BuildData(int result)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result}
            };
            return new ASObject(dic);

        }
    }
}
