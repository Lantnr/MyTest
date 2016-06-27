using System;
using System.Linq;
using FluorineFx;
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
    /// 放火
    /// </summary>
    public class WAR_ARSON : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~WAR_ARSON()
        {
            Dispose();
        }

        #endregion

        /// <summary> 放火 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id") || !data.ContainsKey("roleid"))
                return null;
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);
            var rid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "roleid").Value); //合战的武将主Id
            var uid = session.Player.User.id;
            var warRole = tg_war_role.GetEntityByUserIdAndId(uid, rid);
            if (warRole == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);
            if (!tg_war_role.RoleIsIdle(warRole)) //验证武将状态
                return CommonHelper.ErrorResult(ResultType.WAR_ROLE_STATE_ERROR);

            var item = Common.GetInstance().GetRoleItem(warRole.rid,uid);
            if (item == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32046");
            if (rule == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            var totalpower = tg_role.GetTotalPower(item.Kind);
            var power = Convert.ToInt32(rule.value);
            if (totalpower < power) return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_POWER_ERROR);

            var prob = (new TGTask()).GetTaskProb(item, (int)TaskStepType.SEARCH_GOODS);
            prob += Convert.ToInt32((new Share.War()).GetTactics(uid, (int)WarTacticsType.FIRE_PROBABILITY) * 100);
            var b = new RandomSingle().IsTrue(prob);      //概率结果

            var r = item.Kind.CloneEntity();
            if (b)
            {
                var city = (new Share.War()).GetWarCity(id);
                var rv = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32019");
                if (city == null || rv == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
                var value = Convert.ToInt32(rv.value);
                city.fire_time = (DateTime.Now.AddMinutes(value).Ticks - 621355968000000000) / 10000;
                var f = city.Update() > 0;
                if (!f) return CommonHelper.ErrorResult(ResultType.DATABASE_ERROR);
                (new Share.War()).SaveWarCityAll(city);
                (new Share.War()).SendCity(city.base_id, city.user_id);
                (new Role()).PowerUpdateAndSend(item.Kind, power, item.Kind.user_id);
                Common.GetInstance().SaveRoleStateAndSend(warRole, (int)WarRoleStateType.FIRE, "32020");
            }
            else
            {
                (new Role()).PowerUpdateAndSend(item.Kind, power, item.Kind.user_id);
                Common.GetInstance().SaveRoleStateAndSend(warRole, (int)WarRoleStateType.PRISONERS, "32021");
            }
            //日志插入
            (new Role()).LogInsert(r, power, ModuleNumber.WAR, (int)WarCommand.WAR_ARSON, "合战", "放火");
            return CommonHelper.SuccessResult();
        }
    }
}
