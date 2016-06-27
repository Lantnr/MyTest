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
    /// 采集火把
    /// 开发者:李德雁
    /// </summary>
    public class TORCH
    {
        private static TORCH _objInstance;

        /// <summary>TORCH 单体模式</summary>
        public static TORCH GetInstance()
        {
            return _objInstance ?? (_objInstance = new TORCH());
        }

        /// <summary>收集火把</summary>
        public ASObject CommandStart(SocketServer.TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("一夜墨俣：收集火把 -{0}——{1}", session.Player.User.player_name, "TORCH");
#endif
            var userid = session.Player.User.id;
            if (!Variable.Activity.BuildActivity.userGoods.ContainsKey(userid))
             return BuildData((int)ResultType.BUILDING_NOT_IN, false, 0);
            var ac = Variable.Activity.BuildActivity.userGoods[userid];
        
            if (!CheckTorch(ac.torch)) return BuildData((int)ResultType.BUILDING_TORCH_FULL, false, 0); //验证火把是否达到上限
            if (!CheckTime(ac)) return BuildData((int)ResultType.BUILDING_TIME_OUT, false, ac.wood); //倒计时内发数据
            if (!CheckPoint(session.Player.User.player_camp, session.Player.User.id))
                return BuildData((int)ResultType.BUILDING_WOOD_FULL, false, 0); //验证玩家当前坐标

            var pro = Variable.BASE_BUILD.FirstOrDefault(q => q.content == (int)BuildStepType.GET_TORCH
                && q.level == session.Player.Role.LifeSkill.sub_eloquence_level); //基表中查询概率(辩才有关)
            if (pro == null) return BuildData((int)ResultType.BUILDING_BASE_ERROR, false, 0);
            var istrue = (new RandomSingle()).IsTrue(pro.probability);
            if (istrue) { ac.torch++; ac.totaltorch++; }
          
            ac.TorchTime = DateTime.Now;
            return BuildData((int)ResultType.SUCCESS, istrue, ac.torch);
        }

        /// <summary> 倒计时时间验证 </summary>
        private Boolean CheckTime(BuildActivity.UserGoods goods)
        {

            return DateTime.Now >= goods.woodTime.AddSeconds(Variable.Activity.BuildActivity.TorchTime);
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

        /// <summary> 验证火把是否达到上限 </summary>
        private bool CheckTorch(int mytorch)
        {
            return mytorch < Variable.Activity.BuildActivity.TorchFull;
        }

        /// <summary> 验证坐标 </summary>
        private bool CheckPoint(int camp, Int64 userid)
        {
            var baseinfo = Variable.BASE_BUILDING_COLLECT.FirstOrDefault(q => q.type == 2 && q.camp == camp);
            if (baseinfo == null) return false;
            if (!baseinfo.coorPoint.Contains(",")) return false;
            var point = baseinfo.coorPoint.Split(',');
            return Common.GetInstance().CheckPoint(Convert.ToInt32(point[0]), Convert.ToInt32(point[1]), userid);
        }
    }
}
