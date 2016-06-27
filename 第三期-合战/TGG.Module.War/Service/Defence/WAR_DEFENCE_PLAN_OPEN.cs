using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.War;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Defence
{
    /// <summary>
    /// 开启新的防守方案
    /// 开发者：李德雁
    /// </summary>
    public class WAR_DEFENCE_PLAN_OPEN : IDisposable  
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_DEFENCE_PLAN_OPEN()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_DEFENCE_PLAN_OPEN _objInstance;

        ///// <summary>WAR_DEFENCE_PLAN_OPEN单体模式</summary>
        //public static WAR_DEFENCE_PLAN_OPEN GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DEFENCE_PLAN_OPEN());
        //}

        /// <summary> 开启新的防守方案 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var user = session.Player.User.CloneEntity();

            var tuple = CheckCount(session.Player.War.PlayerInCityId, user.id);
            if (!tuple.Item1)
                return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_COIN_ERROR);
            var location = tuple.Item2;
            var cityinfo = tuple.Item3;
            //验证金钱
            if (!CheckCoin(user, session))
                return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_COIN_ERROR);

            //插入防守方案表
            var newplan = new tg_war_city_plan() { user_id = cityinfo.user_id, location = location };

            newplan.Insert();
            switch (location)
            {
                //location: 0-2
                case 1: { cityinfo.plan_2 = newplan.id; } break;
                case 2: { cityinfo.plan_3 = newplan.id; } break;
            }
            newplan.Update();
            cityinfo.Update();
            //防守武将添加
            var defenseroles = tg_war_city_defense.GetListByPlanId(cityinfo.plan_1);
            if (defenseroles != null)
            {
                for (int i = 0; i < defenseroles.Count; i++)
                {
                    defenseroles[i].plan_id = newplan.id;
                }
            }
            tg_war_city_defense.GetListInsert(defenseroles);
            return BuildData(newplan);
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="plan">实体</param>
        /// <returns></returns>
        private ASObject BuildData(tg_war_city_plan plan)
        {
            var vo = new WarDefencePlanVo()
            {
                id = plan.id,
                frontId = (int)plan.formation,
                location = plan.location,
                listarea = null,
                roles = null
            };
            var dic = new Dictionary<string, object>()
            {
                {"result", (int) ResultType.SUCCESS},
                {"warDefencePlanVo", vo}
            };
            return new ASObject(dic);
        }

        /// <summary>
        /// 验证新方案是否可以开启
        /// </summary>
        /// <param name="cityid">据点基表id</param>
        /// <param name="userid">用户id</param>
        /// <returns>1.是否可以开启 2.开启的位置</returns>
        private Tuple<Boolean, int, tg_war_city> CheckCount(Int32 cityid, Int64 userid)
        {
            var cityinfo = tg_war_city.GetCityByBidUserId(cityid, userid);
            if (cityinfo == null) return Tuple.Create(false, 0, cityinfo);
            if (cityinfo.plan_2 == 0) return Tuple.Create(true, 1, cityinfo);
            if (cityinfo.plan_3 == 0) return Tuple.Create(true, 2, cityinfo);

            return Tuple.Create(false, 0, cityinfo);
        }

        /// <summary>
        /// 验证金钱
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <param name="session"></param>
        /// <returns></returns>
        private bool CheckCoin(tg_user user, TGGSession session)
        {
            var baseinfo = Variable.BASE_RULE.FirstOrDefault(q => q.id == "32119");
            if (baseinfo == null) return false;
            //消耗的金钱
            var cost = Convert.ToInt32(baseinfo.value);
            if (user.coin < cost) return false;

            var coin = user.coin;
            user.coin -= cost;
            user.Update();
            session.Player.User.coin = user.coin;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, user);

            //日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Coin", coin, cost, user.coin);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.WAR, (int)WarCommand.WAR_DEFENCE_PLAN_OPEN, "合战", "解锁防守方案", "金钱", (int)GoodsType.TYPE_COIN, cost, user.coin, logdata);
            return true;

        }


    }
}
