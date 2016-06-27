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
using TGG.SocketServer;

namespace TGG.Module.Building.Service
{
    /// <summary>
    /// 制造建材
    /// 开发者：李德雁
    /// </summary>
    public class MAKE_BUILD
    {
        private static MAKE_BUILD _objInstance;

        /// <summary>MAKE_BUILD 单体模式</summary>
        public static MAKE_BUILD GetInstance()
        {
            return _objInstance ?? (_objInstance = new MAKE_BUILD());
        }

        /// <summary>制造建材</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("一夜墨俣：制造建材 -{0}——{1}", session.Player.User.player_name, "MAKE_BUILD");
#endif

            if (!Variable.Activity.BuildActivity.userGoods.ContainsKey(session.Player.User.id)) 
                return BuildData((int)ResultType.BUILDING_NOT_IN, false, null);
            var ac = Variable.Activity.BuildActivity.userGoods[session.Player.User.id];

            if (!CheckTime(ac)) return BuildData((int)ResultType.BUILDING_TIME_OUT, false, ac); //倒计时内发数据
            if (!CheckBuild(ac.basebuild)) return BuildData((int)ResultType.BUILDING_BUILD_FULL, false, ac); //建材已满
            if (!CheckPoint(session.Player.User.player_camp, session.Player.User.id))
                return BuildData((int)ResultType.BUILDING_POINT_OUT, false, ac);//验证玩家当前坐标

            if (Variable.Activity.BuildActivity.CostWood > ac.wood) return BuildData((int)ResultType.BUILDING_WOOD_LACK, false, ac); //验证木头数量
            var pro = GetProbability(session);
            if (pro == 0) return BuildData((int)ResultType.BUILDING_BASE_ERROR, false, ac);

            var istrue = (new RandomSingle()).IsTrue(pro);
            if (istrue)
            {
                ac.wood -= Variable.Activity.BuildActivity.CostWood;
                ac.basebuild++;
                ac.totalbasebuild++;
                Common.GetInstance().AddFame(Variable.Activity.BuildActivity.MakeAddFame, ac);
            }
            ac.MakeBuildTime = DateTime.Now;
            return BuildData((int)ResultType.SUCCESS, istrue, ac);
        }

        /// <summary> 倒计时时间验证 </summary>
        private Boolean CheckTime(BuildActivity.UserGoods goods)
        {
            return DateTime.Now >= goods.MakeBuildTime.AddSeconds(Variable.Activity.BuildActivity.MakeBuildTime);
        }


        /// <summary> 验证建材是否达到上限 </summary>
        private bool CheckBuild(int mybuild)
        {
            var max = Variable.Activity.BuildActivity.MakeBuildFull;
            return mybuild < max;
        }

        /// <summary> 组装数据 </summary>
        private ASObject BuildData(int result, Boolean istrue, BuildActivity.UserGoods goods)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"type", istrue ? 1 : 0},
                {"wood",goods==null?0: goods.wood},
                {"baseBuild",goods==null?0: goods.basebuild},
                 {"fame",goods==null?0: goods.fame},
            };
            return new ASObject(dic);
        }

        /// <summary> 获取概率值 </summary>
        private int GetProbability(TGGSession session)
        {
            var myvalue = tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, session.Player.Role.Kind);
            var pro = Variable.BASE_BUILD.FirstOrDefault(q => q.content == (int)BuildStepType.MAKE_BUILD && q.value <= myvalue);//政务
            return pro == null ? 0 : pro.probability;
        }

        /// <summary> 验证坐标 </summary>
        private bool CheckPoint(int camp, Int64 userid)
        {
            var baseinfo = Variable.BASE_BUILDING_COLLECT.FirstOrDefault(q => q.type == (int)BuildStepType.MAKE_BUILD && q.camp == camp);
            if (baseinfo == null) return false;
            if (!baseinfo.coorPoint.Contains(",")) return false;
            var point = baseinfo.coorPoint.Split(',');
            return Common.GetInstance().CheckPoint(Convert.ToInt32(point[0]), Convert.ToInt32(point[1]), userid);
        }

    }
}
