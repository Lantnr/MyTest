using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Global;

namespace TGG.Core.Common
{
    public class Scene
    {
        /// <summary>
        /// 地图场景内其他玩家
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="sceneid">场景id</param>
        /// <param name="mn">模块号</param>
        /// <returns></returns>
        public static List<view_scene_user> GetOtherPlayers(Int64 userid, Int64 sceneid, int mn)
        {
            var mykey = string.Format("{0}_{1}", mn, userid);
            var otherscontains = string.Format("{0}_", mn);
            var keys = Variable.SCENCE.Keys.Where(q => q.Contains(otherscontains) && q != mykey).ToList();
            return (from item in keys where Variable.SCENCE[item].model_number == (int)ModuleNumber.SCENE && Variable.SCENCE[item].scene_id == sceneid select Variable.SCENCE[item]).ToList();
        }

      
        /// <summary>
        /// 地图场景内其他玩家
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="sceneid">场景id</param>
        /// <param name="mn">模块号</param>
        /// <returns></returns>
        public static List<view_scene_user> GetOtherPlayersByArea(Int64 userid, Int64 sceneid, int mn, int x, int y, Int32 wx, int wy)
        {
            var mykey = string.Format("{0}_{1}", mn, userid);
            var otherscontains = string.Format("{0}_", mn);
            var keys = Variable.SCENCE.Keys.Where(q => q.Contains(otherscontains) && q != mykey).ToList();
            var others = (from item in keys
                          where Variable.SCENCE[item].model_number == (int)ModuleNumber.SCENE && Variable.SCENCE[item].scene_id == sceneid
                              && Math.Abs(Variable.SCENCE[item].X - x) < wx && Math.Abs(Variable.SCENCE[item].Y - y) < wy
                          select Variable.SCENCE[item]).ToList();
            return others;

        }

        /// <summary>
        /// 单个场景内的其他玩家(监狱）
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="mn">模块号</param>
        /// <returns></returns>
        public static List<string> GetOtherPlayers(Int64 userid, int mn)
        {
            var mykey = string.Format("{0}_{1}", mn, userid);
            var otherscontains = string.Format("{0}_", mn);
            return Variable.SCENCE.Keys.Where(q => q.Contains(otherscontains) && q != mykey).ToList();
        }

        /// <summary>
        /// 单个场景内的其他玩家(活动）
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="mn">模块号</param>
        /// <returns></returns>
        public static List<string> ActivityOtherPlayers(Int64 userid, int mn)
        {
            var mykey = string.Format("{0}_{1}", mn, userid);
            var otherscontains = string.Format("{0}_", mn);
            return Variable.Activity.ScenePlayer.Keys.Where(q => q.Contains(otherscontains) && q != mykey).ToList();

        }

        /// <summary>
        /// 获取玩家全局场景信息
        /// </summary>
        /// <param name="mn">模块号</param>
        /// <param name="user_id">用户id</param>
        /// <returns></returns>
        public static view_scene_user GetSceneInfo(int mn, long user_id)
        {
            var key = string.Format("{0}_{1}", mn, user_id);
            return !Variable.SCENCE.ContainsKey(key) ? null : Variable.SCENCE[key];
        }

    }
}
