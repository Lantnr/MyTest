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
    /// 一夜墨俣--放火
    /// 开发者：李德雁
    /// </summary>
    public class FIRE
    {
        private static FIRE _objInstance;

        /// <summary>FIRE 单体模式</summary>
        public static FIRE GetInstance()
        {
            return _objInstance ?? (_objInstance = new FIRE());
        }


        /// <summary> 一夜墨俣--放火</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("一夜墨俣：放火 -{0}——{1}", session.Player.User.player_name, "FIRE");
#endif
            var userid = session.Player.User.id;
            var camp = session.Player.User.player_camp;
            if (!CheckPoint(camp, userid)) return BuildData((int)ResultType.BUILDING_NOT_IN, false, null, 0);//坐标验证

            if (!Variable.Activity.BuildActivity.userGoods.ContainsKey(userid))
                return BuildData((int)ResultType.BUILDING_NOT_IN, false, null, 0);
            var ac = Variable.Activity.BuildActivity.userGoods[session.Player.User.id];

            if (!CheckTime(ac)) return BuildData((int)ResultType.BUILDING_TIME_OUT, false, null, 0); //倒计时内发数据
            if (!Common.GetInstance().CheckFireOpen(camp))  //验证放火功能有没有开启
                return BuildData((int)ResultType.BUILDING_BOSS_LIVE, false, null, 0);
            if (Variable.Activity.BuildActivity.CostFire > ac.torch) return BuildData((int)ResultType.BUILDING_WOOD_LACK, false, null, 0); //验证数量

            var pro = Variable.BASE_BUILD.FirstOrDefault(q => q.content == (int)BuildStepType.BUILD && q.level == session.Player.Role.LifeSkill.sub_ninjitsu_level); //忍术
            if (pro == null) return BuildData((int)ResultType.BUILDING_BASE_ERROR, false, null, 0);
            ac.FireTime = DateTime.Now;
            var istrue = (new RandomSingle()).IsTrue(pro.probability);
            if (istrue) return DurabilityUpdate(userid, camp, true, ac);

            return BuildData((int)ResultType.SUCCESS, false, ac, camp == (int)CampType.East ? Variable.Activity.BuildActivity.EastCityBlood :
            Variable.Activity.BuildActivity.WestCityBlood);
        }

        /// <summary> 倒计时时间验证 </summary>
        private Boolean CheckTime(BuildActivity.UserGoods goods)
        {
            return DateTime.Now >= goods.FireTime.AddSeconds(Variable.Activity.BuildActivity.FireTime);
        }

        /// <summary> 组装数据 </summary>
        private ASObject BuildData(int result, Boolean istrue, BuildActivity.UserGoods goods, int durability)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"type", istrue ? 1 : 0},
                {"durability",durability},
                {"torch",goods==null?0: goods.torch},
                {"fame",goods==null?0: goods.fame},
            };
            return new ASObject(dic);
        }


        /// <summary> 城池耐久度减少 </summary>
        private ASObject DurabilityUpdate(Int64 userid, int camp, bool istrue, BuildActivity.UserGoods ac)
        {

            ac.torch -= Variable.Activity.BuildActivity.CostFire;
            if (camp == (int)CampType.West) //东军城池耐久度
            {
                if (Variable.Activity.BuildActivity.EastCityBlood == 0)
                    return BuildData((int)ResultType.SUCCESS, istrue, ac, Variable.Activity.BuildActivity.EastCityBlood);
                Variable.Activity.BuildActivity.EastCityBlood -= Variable.Activity.BuildActivity.ReduceBlood;
                if (Variable.Activity.BuildActivity.EastCityBlood < 0) //耐久度最小为0
                    Variable.Activity.BuildActivity.EastCityBlood = 0;
            }
            else
            {
                if (Variable.Activity.BuildActivity.WestCityBlood == 0)
                    return BuildData((int)ResultType.SUCCESS, istrue, ac, Variable.Activity.BuildActivity.WestCityBlood);
                Variable.Activity.BuildActivity.WestCityBlood -= Variable.Activity.BuildActivity.ReduceBlood;
                if (Variable.Activity.BuildActivity.WestCityBlood < 0) //耐久度最小为0
                    Variable.Activity.BuildActivity.WestCityBlood = 0;
            }
            Common.GetInstance().PushDurability(userid, camp == (int)CampType.East ? 2 : 1); //推送城池血量更新
            return BuildData((int)ResultType.SUCCESS, istrue, ac, camp == (int)CampType.East ? Variable.Activity.BuildActivity.WestCityBlood :
             Variable.Activity.BuildActivity.EastCityBlood);
        }

        /// <summary> 验证坐标 </summary>
        private bool CheckPoint(int camp, Int64 userid)
        {
            //阵营取对面的阵营
            if (camp == (int)CampType.East) camp = (int)CampType.West;
            else
                camp = (int)CampType.East;
            var baseinfo = Variable.BASE_NPC_BUILD.FirstOrDefault(q => q.type == (int)ActivityBuildType.CITY && q.camp == camp);
            if (baseinfo == null) return false;
            if (!baseinfo.coorPoint.Contains(",")) return false;
            var point = baseinfo.coorPoint.Split(',');
            return Common.GetInstance().CheckPoint(Convert.ToInt32(point[0]), Convert.ToInt32(point[1]), userid);
        }

    }
}
