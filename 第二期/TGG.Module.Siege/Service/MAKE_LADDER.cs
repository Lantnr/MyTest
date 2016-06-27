using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 制造云梯
    /// </summary>
    public class MAKE_LADDER
    {
        private static MAKE_LADDER _objInstance;

        /// <summary>MAKE_LADDER单体模式</summary>
        public static MAKE_LADDER GetInstance()
        {
            return _objInstance ?? (_objInstance = new MAKE_LADDER());
        }

        /// <summary> 制造云梯</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var user = session.Player.User;
            var role = session.Player.Role.Kind;
            var lifeskill = session.Player.Role.LifeSkill;
            // if (!IsCoorPoint(user)) return new ASObject(Common.GetInstance().BuildData((int)ResultType.POSITION_ERROR)); 验证坐标

            var playerdata = Common.GetInstance().GetSiegePlayer(user.id, user.player_camp);
            var t = playerdata.time + (Variable.Activity.Siege.BaseData.MakeLadderTime);
            var time = Convert.ToDouble((DateTime.Now.Ticks - 621355968000000000) / 10000);
            if (t > time) return new ASObject(Common.GetInstance().BuildData((int)ResultType.SIEGE_TIME_ERROR)); //时间验证

            var values = GetBaseLimit(role); //获取云梯上限值
            if (values == 0) return new ASObject(Common.GetInstance().BuildData((int)ResultType.BASE_TABLE_ERROR));
            if (playerdata.count >= values) return new ASObject(Common.GetInstance().BuildData((int)ResultType.SIEGE_MAX_VALUES));//云梯数量验证

            var probability = GetBaseProbability(lifeskill);
            if (probability == 0) return new ASObject(Common.GetInstance().BuildData((int)ResultType.BASE_TABLE_ERROR));
            var flag = (new RandomSingle()).IsTrue(probability); //制作云梯是否成功

            playerdata.time = time;
            if (!flag) return new ASObject(BuildData((int)ResultType.SUCCESS, false, playerdata.count)); //制作失败返回

            playerdata.count += 1;
            return new ASObject(BuildData((int)ResultType.SUCCESS, true, playerdata.count));//制作成功返回
        }



        #region 私有方法

        ///// <summary> 验证玩家是否在云梯采集点附近 </summary>
        ///// <param name="user">玩家信息</param>
        //private bool IsCoorPoint(tg_user user)
        //{
        //    var gp = Variable.BASE_COLLECTGOODSSIEGE.FirstOrDefault(m => m.camp == user.player_camp);
        //    if (gp == null) return false;
        //    var scene = Variable.Activity.ScenePlayer.FirstOrDefault(m => m.user_id == user.id);
        //    var xy = gp.coorPoint.Split(',');
        //    return xy.Length == 2 && Common.GetInstance().IsCoorPoint(xy, scene);
        //}

        /// <summary> 获取制作云梯的几率 </summary>
        /// <param name="skill">生活技能</param>
        /// <returns>几率</returns>
        private int GetBaseProbability(tg_role_life_skill skill)
        {
            var list = Common.GetInstance().GetBaseSieges((int)SiegeType.YUNTI_ODDS);
            if (!list.Any()) return 0;
            var basesiege = list.FirstOrDefault();
            if (basesiege == null) return 0;
            var level = Common.GetInstance().GetLifeLevel(basesiege.skillType, skill);

            var bl = list.Where(m => m.level <= level).OrderByDescending(m => m.level).FirstOrDefault();
            return bl == null ? 0 : bl.probability;
        }

        /// <summary> 获取云梯上限值 </summary>
        /// <param name="role">主角武将信息</param>
        /// <returns>上限值</returns>
        private int GetBaseLimit(tg_role role)
        {
            var list = Common.GetInstance().GetBaseSieges((int)SiegeType.YUNTI_LIMIT);
            if (!list.Any()) return 0;
            var basesiege = list.FirstOrDefault();
            if (basesiege == null) return 0;
            var values = tg_role.GetSingleTotal(Common.GetInstance().ConverType(basesiege.ributeType), role);// Common.GetInstance().GetRoleRibute(basesiege.ributeType, role);
            var bl = list.Where(m => m.ributeValues <= values).OrderByDescending(m => m.ributeValues).FirstOrDefault() ??
                list.OrderBy(m => m.id).FirstOrDefault();
            return bl == null ? 0 : bl.count;
        }

        /// <summary> 组装前端需要的数据 </summary>
        private Dictionary<string, Object> BuildData(int result, bool flag, int count)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"isSuccess", flag},
                {"ladder", count},
            };
            return dic;
        }

        #endregion
    }
}
