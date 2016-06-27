using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary>
    /// 破坏
    /// </summary>
    public class WAR_DESTRUCTION : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~WAR_DESTRUCTION()
        {
            Dispose();
        }

        #endregion

        //private static WAR_DESTRUCTION _objInstance;

        ///// <summary>WAR_DESTRUCTION单体模式</summary>
        //public static WAR_DESTRUCTION GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DESTRUCTION());
        //}

        /// <summary> 破坏 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id") || !data.ContainsKey("roleid")) return null;

            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);
            var rid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "roleid").Value);
            var uid = session.Player.User.id;
            var warRole = tg_war_role.GetEntityByUserIdAndId(uid, rid);
            if (!tg_war_role.RoleIsIdle(warRole))
                return CommonHelper.ErrorResult(ResultType.WAR_ROLE_STATE_ERROR);

            var city = (new Share.War()).GetWarCity(id);
            if (city == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            if (city.destroy_time > time) return CommonHelper.ErrorResult(ResultType.WAR_CITY_PROTECT_STATE);


            var item = Common.GetInstance().GetRoleItem(warRole.rid, uid);
            if (item == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32046");
            var baseRuleFanRong = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32115");
            var baseRuleNaiJiu = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32116");
            var baseRuleZhiAn = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32117");
            if (rule == null || baseRuleFanRong == null || baseRuleNaiJiu == null || baseRuleZhiAn == null)
                return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);

            var totalpower = tg_role.GetTotalPower(item.Kind);
            var power = Convert.ToInt32(rule.value);
            if (totalpower < power) return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_POWER_ERROR);

            var prob = (new TGTask()).GetTaskProb(item, (int)TaskStepType.SEARCH_GOODS);
            prob += Convert.ToInt32((new Share.War()).GetTactics(uid, (int)WarTacticsType.DESTROY_PROBABILITY) * 100);
            var b = new RandomSingle().IsTrue(prob);      //概率结果

            var r = item.Kind.CloneEntity();
            if (b)
            {
                var rv = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32022");
                if (rv == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
                var value = Convert.ToInt32(rv.value);
                city.destroy_time = (DateTime.Now.AddMinutes(value).Ticks - 621355968000000000) / 10000;

                #region 破坏效果扣除

                var number = Convert.ToInt32((new Share.War()).GetTactics(uid, (int)WarTacticsType.DESTROY));
                var boom = Convert.ToInt32(baseRuleFanRong.value) + number;
                var peace = Convert.ToInt32(baseRuleZhiAn.value) + number;
                var strong = Convert.ToInt32(baseRuleNaiJiu.value) + number;

                if (boom < city.boom) city.boom -= boom;
                else
                {
                    boom = Convert.ToInt32(city.boom);
                    city.boom = 0;
                }
                if (peace < city.peace) city.peace -= peace;
                else
                {
                    peace = Convert.ToInt32(city.peace);
                    city.peace = 0;
                }
                if (strong < city.strong) city.strong -= strong;
                else
                {
                    strong = Convert.ToInt32(city.strong);
                    city.strong = 0;
                }
                city.destroy_boom = boom;
                city.destroy_peace = peace;
                city.destroy_strong = strong;

                #endregion

                city.Update();
                (new Share.War()).SaveWarCityAll(city);
                (new Share.War()).DestructionTask(city);
                (new Role()).PowerUpdateAndSend(item.Kind, power, item.Kind.user_id);
                Common.GetInstance().SaveRoleStateAndSend(warRole, (int)WarRoleStateType.DESTROY, "32023");
                (new Share.War()).SendCity(city.base_id, city.user_id);
            }
            else
            {
                (new Role()).PowerUpdateAndSend(item.Kind, power, item.Kind.user_id);
                Common.GetInstance().SaveRoleStateAndSend(warRole, (int)WarRoleStateType.PRISONERS, "32021");
            }

            //日志插入
            (new Role()).LogInsert(r, power, ModuleNumber.WAR, (int)WarCommand.WAR_DESTRUCTION, "合战", "破坏");
            return CommonHelper.SuccessResult();
        }


    }
}
