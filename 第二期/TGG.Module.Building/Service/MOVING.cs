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
    /// 一夜墨俣--玩家移动
    /// 开发者：李德雁
    /// </summary>
    public class MOVING
    {
        public static MOVING objInstance = null;

        /// <summary> MOVING单体模式 </summary>
        public static MOVING getInstance()
        {
            return objInstance ?? (objInstance = new MOVING());
        }

        /// <summary>
        /// 一夜墨俣--玩家移动
        /// </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}移动", "MOVING", session.Player.User.player_name);
#endif
            var userid = session.Player.User.id;
            var x = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "x").Value);
            var y = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "y").Value);
            if (x == 0 || y == 0)
                return BuildData((int)ResultType.BUILDING_POINT_ERROR);
            var key = (int)ModuleNumber.BUILDING + "_" + session.Player.User.id;
            if (!Variable.Activity.ScenePlayer.ContainsKey(key))
                return BuildData((int)ResultType.BUILDING_SCENE_ERROR); 
            var acp = Variable.Activity.ScenePlayer[key];
            acp.X = x;
            acp.Y = y;
            Common.GetInstance().PusuMovingActivity(userid, acp);
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
