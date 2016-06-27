using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Building.Service
{
    /// <summary>
    /// 筑城
    /// </summary>
    public class BUILDING
    {
        private static BUILDING _objInstance;

        /// <summary>BUILDING 单体模式</summary>
        public static BUILDING GetInstance()
        {
            return _objInstance ?? (_objInstance = new BUILDING());
        }


        /// <summary>筑城</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("一夜墨俣：筑城 -{0}——{1}", session.Player.User.player_name, "BUILDING");
#endif
            var camp = session.Player.User.player_camp;
            var userid = session.Player.User.id;
            if (!Variable.Activity.BuildActivity.userGoods.ContainsKey(userid))
                return BuildData((int)ResultType.BUILDING_NOT_IN, false, 0, 0, 0);
            var ac = Variable.Activity.BuildActivity.userGoods[userid];

            if (!CheckTime(ac.BuildTime)) return BuildData((int)ResultType.BUILDING_TIME_OUT, false, 0, 0, 0); //倒计时内发数据

            if (!CheckPoint(camp, userid)) return BuildData((int)ResultType.BUILDING_POINT_OUT, false, 0, 0, 0); //验证坐标

            if (!Common.GetInstance().CheckBossBlood(camp))  //验证boss有没有打死
                return BuildData((int)ResultType.BUILDING_BOSS_LIVE, false, 0, 0, 0);
            if (Variable.Activity.BuildActivity.CostMakeWood > ac.basebuild)
                return BuildData((int)ResultType.BUILDING_WOOD_LACK, false, 0, 0, 0); //验证数量

            var pro = GetProbability(session.Player.Role.Kind);
            if (pro == 0) return BuildData((int)ResultType.BUILDING_BASE_ERROR, false, 0, 0, 0);
            ac.BuildTime = DateTime.Now;
            var istrue = (new RandomSingle()).IsTrue(pro);
            if (!istrue) return BuildData((int)ResultType.SUCCESS, false, ac.fame, ac.basebuild, camp == (int)CampType.East ? Variable.Activity.BuildActivity.EastCityBlood : Variable.Activity.BuildActivity.WestCityBlood);
            ac.basebuild -= Variable.Activity.BuildActivity.CostMakeWood;
            var add = Variable.BASE_BUILD.FirstOrDefault(q => q.content == (int)BuildStepType.BUILD && q.level == session.Player.Role.LifeSkill.sub_build_level); //筑城增加耐久度
            Common.GetInstance().AddFame(Variable.Activity.BuildActivity.BuildAddFame, ac);
            return DurabilityUpdate(add == null ? 0 : add.count, true, userid, camp, ac.basebuild, ac.fame);

        }

        /// <summary> 倒计时时间验证 </summary>
        private Boolean CheckTime(DateTime dt)
        {
            return DateTime.Now >= dt.AddSeconds(Variable.Activity.BuildActivity.BuildTime);
        }

        /// <summary> 组装数据 </summary>
        private ASObject BuildData(int result, Boolean istrue, int fame, int basebuild, int durability)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"type", istrue ? 1 : 0},
                {"durability",durability},
                {"baseBuild",basebuild},
                {"fame",fame},
            };
            return new ASObject(dic);
        }


        /// <summary> 城池耐久度增加 </summary>
        private ASObject DurabilityUpdate(int add, bool istrue, Int64 userid, Int32 camp, int basebuild, int fame)
        {
            if (camp == (int)CampType.East) //东军城池耐久度
            {
                Variable.Activity.BuildActivity.EastCityBlood += add;
                if (CheckGameOver(camp)) //耐久度满,游戏结束
                    return BuildData((int)ResultType.SUCCESS, istrue, fame, basebuild, Variable.Activity.BuildActivity.EastCityBlood);
            }
            else
            {
                Variable.Activity.BuildActivity.WestCityBlood += add;
                if (CheckGameOver(camp)) //耐久度满,游戏结束
                    return BuildData((int)ResultType.SUCCESS, istrue, fame, basebuild, Variable.Activity.BuildActivity.WestCityBlood);
            }
            Common.GetInstance().PushDurability(userid, camp == (int)CampType.East ? 1 : 2); //推送城池血量更新
            return BuildData((int)ResultType.SUCCESS, istrue, fame, basebuild, camp == (int)CampType.East ? Variable.Activity.BuildActivity.EastCityBlood :
             Variable.Activity.BuildActivity.WestCityBlood);
        }

        /// <summary> 验证筑城耐久度是否达到上限 </summary>
        private bool CheckGameOver(int camp)
        {
            var blood = camp == (int)CampType.East
                ? Variable.Activity.BuildActivity.EastCityBlood
                : Variable.Activity.BuildActivity.WestCityBlood;
            if (blood < Variable.Activity.BuildActivity.CityBloodFull)

                return false;
            if (camp == (int)CampType.East)
                Variable.Activity.BuildActivity.EastCityBlood = Variable.Activity.BuildActivity.CityBloodFull;
            else
                Variable.Activity.BuildActivity.WestCityBlood = Variable.Activity.BuildActivity.CityBloodFull;
            END.GetInstance().CommandStart(Variable.Activity.BuildActivity.EastCityBlood == Variable.Activity.BuildActivity.CityBloodFull ? 1 : 2);
            return true;
        }

        /// <summary> 获取成功率</summary>
        private int GetProbability(tg_role role)
        {
            var myvalue = tg_role.GetSingleTotal(RoleAttributeType.ROLE_BRAINS, role);
            var pro = Variable.BASE_BUILD.FirstOrDefault(q => q.content == (int)BuildStepType.MAKE_BUILD && q.value <= myvalue);//智谋
            return pro == null ? 0 : pro.probability;
        }

        /// <summary> 验证坐标 </summary>
        private bool CheckPoint(int camp, Int64 userid)
        {
            var baseinfo = Variable.BASE_NPC_BUILD.FirstOrDefault(q => q.type == (int)ActivityBuildType.CITY && q.camp == camp);
            if (baseinfo == null) return false;
            if (!baseinfo.coorPoint.Contains(",")) return false;
            var point = baseinfo.coorPoint.Split(',');
            return Common.GetInstance().CheckPoint(Convert.ToInt32(point[0]), Convert.ToInt32(point[1]), userid);
        }
    }
}
