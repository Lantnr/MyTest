using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Module.Building.Service
{
    /// <summary>
    /// 一夜墨俣：采集木材
    /// 开发者：李德雁
    /// </summary>
    public class GET_WOOD
    {
        private static GET_WOOD _objInstance;

        /// <summary>GET_WOOD 单体模式</summary>
        public static GET_WOOD GetInstance()
        {
            return _objInstance ?? (_objInstance = new GET_WOOD());
        }

        /// <summary>采集木材</summary>
        public ASObject CommandStart(SocketServer.TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("一夜墨俣：采集木材 -{0}——{1}", session.Player.User.player_name, "GET_WOOD");
#endif
            var userid = session.Player.User.id;
            var camp = session.Player.User.player_camp;

            if (!Variable.Activity.BuildActivity.userGoods.ContainsKey(userid))
                return BuildData((int)ResultType.BUILDING_NOT_IN, false, 0);
            var ac = Variable.Activity.BuildActivity.userGoods[userid];

            if (!CheckTime(ac.woodTime))
                return BuildData((int)ResultType.BUILDING_TIME_OUT, false, ac.wood); //倒计时是否完成
            if (!CheckPoint(camp, userid))
                return BuildData((int)ResultType.BUILDING_POINT_OUT, false, 0); //验证玩家当前坐标
            if (ac.wood >= Variable.Activity.BuildActivity.WoodFull)
                return BuildData((int)ResultType.BUILDING_WOOD_FULL, false, 0); //验证木头有没有到达上限

            var pro = Variable.BASE_BUILD.FirstOrDefault(q => q.content == (int)BuildStepType.GET_WOOD
                && q.level == session.Player.Role.LifeSkill.sub_calculate_level);
            if (pro == null) return BuildData((int)ResultType.BUILDING_BASE_ERROR, false, 0);
            var istrue = (new RandomSingle()).IsTrue(pro.probability);

            if (istrue) { ac.wood++; ac.totalwood++; }
            ac.woodTime = DateTime.Now;
            return BuildData((int)ResultType.SUCCESS, istrue, ac.wood);
        }

        /// <summary> 倒计时时间验证 </summary>
        private Boolean CheckTime(DateTime time)
        {
            return DateTime.Now >= time.AddSeconds(Variable.Activity.BuildActivity.WoodTime);
        }

        /// <summary> 组装数据 </summary>
        private ASObject BuildData(int result, Boolean istrue, int count)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"type", istrue ? 1 : 0},
                {"count", count},
            };
            return new ASObject(dic);
        }

        /// <summary> 验证坐标 </summary>
        private bool CheckPoint(int camp, Int64 userid)
        {
            var baseinfo = Variable.BASE_BUILDING_COLLECT.FirstOrDefault(q => q.type == (int)BuildStepType.GET_WOOD && q.camp == camp);
            if (baseinfo == null) return false;
            if (!baseinfo.coorPoint.Contains(",")) return false;
            var point = baseinfo.coorPoint.Split(',');
            return Common.GetInstance().CheckPoint(Convert.ToInt32(point[0]), Convert.ToInt32(point[1]), userid);
        }


    }
}
