using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.War;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service
{
    public partial class Common
    {
        /// <summary> 组装数据 </summary>
        public ASObject BulidData(view_war_city model, int state, Int64 time = 0)
        {
            var dic = new Dictionary<string, object>
            { 
            { "result", (int)ResultType.SUCCESS },
            { "vo",model==null?null:EntityToVo.ToWarCityVo(model,state,(int)WarCityOwnershipType.PLAYER,time)},
            };
            return new ASObject(dic);
        }

        /// <summary> 组装数据 </summary>
        public ASObject BuildData(List<StrongHoldShowVo> ls)
        {
            var dic = new Dictionary<string, object> { { "result", (int)ResultType.SUCCESS }, { "vo", ls } };
            return new ASObject(dic);
        }

        /// <summary> 组装数据 </summary>
        /// <param name="temp">tg_war_home_tactics</param>
        public ASObject BuildData(tg_war_home_tactics temp)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", (int) ResultType.SUCCESS},
                {"vo", temp == null ? null : EntityToVo.ToHomeTacticsVo(temp)}
            };
            return new ASObject(dic);
        }

        /// <summary> 组装数据 </summary>
        public ASObject BulidData(DiplomacyVo model)
        {
            var dic = new Dictionary<string, object>
            { 
            { "result", (int)ResultType.SUCCESS },
            { "vo",model} 
            };
            return new ASObject(dic);
        }

        /// <summary> 转同盟Vo集合 </summary>
        /// <param name="list">同盟集合</param>
        /// <returns></returns>
        public List<DiplomacyVo> ToDiplomacyVos(List<view_war_partner> list)
        {
            var l = new List<DiplomacyVo>();
            var ues = new List<tg_user_extend>();

            var uids = list.Select(m => m.partner_id).ToList();
            if (uids.Any()) ues = tg_user_extend.GetEntityListByUserIds(uids);
            var time = GetInstance().CurrentTime();

            foreach (var item in list)
            {
                var ex = ues.FirstOrDefault(m => m.user_id == item.partner_id);
                if (ex == null) continue;
                var count = ex.war_total_own;
                var of = Variable.BASE_OFFICE.FirstOrDefault(m => m.id == item.office);
                if (of == null) continue;
                var c = Convert.ToDouble(count) / Convert.ToDouble(of.total_own);
                if (item.time < time)
                {
                    if (item.request_end_time < time)
                        item.state = (int)WarPertnerType.DIPLOMACY_IN;
                }
                l.Add(EntityToVo.ToDiplomacyVo(item, c));
            }
            return l;
        }

        public List<WarGoVo> ConvertGoVos(List<tg_war_battle_queue> list)
        {
            var l = new List<WarGoVo>();
            foreach (var item in list)
            {
                l.Add(EntityToVo.ToWarGoVo(item));
            }
            return l;
        }

        /// <summary> 保存武将合战状态并推送 </summary>
        /// <param name="role">要保存的武将实体</param>
        /// <param name="state">要更改的状态</param>
        /// <param name="id">固定规则id</param>
        public void SaveRoleStateAndSend(tg_war_role role, int state, string id)
        {
            var rv = Variable.BASE_RULE.FirstOrDefault(m => m.id == id);
            if (rv == null) return;
            var count = 0;

            #region 外交冷却时间减少

            if (state == (int)WarRoleStateType.DIPLOMATIC_RELATIONS)
            {
                count = Convert.ToInt32((new Share.War()).GetTactics(role.user_id, (int)WarTacticsType.FOREIGN_TIME)); //内政策略外交冷却时间减少
            }

            #endregion

            var value = Convert.ToInt32(rv.value) - count;
            value = value < 0 ? 0 : value;
            role.state = state;
            role.state_end_time = (DateTime.Now.AddMinutes(value).Ticks - 621355968000000000) / 10000;

#if DEBUG
            role.state_end_time = (DateTime.Now.AddMinutes(1).Ticks - 621355968000000000) / 10000;
#endif
            role.Update();
            (new Share.War()).TaskRoleState(role);

            var str = new string[] { "state" };
            if (Convert.ToInt32(WarRoleStateType.PRISONERS) == state)
                str = new string[] { "state", "stateEndTime" };

            (new Share.War()).SendWarRole(role, str); //推送武将合战状态更变
        }

        /// <summary> 获取RoleItem实体 </summary>
        /// <param name="rid">武将Id</param>
        public RoleItem GetRoleItem(Int64 rid, Int64 userid)
        {
            var role = tg_role.GetRoleByIdAndUser(rid, userid);//tg_role.FindByid(rid);
            if (role == null) return null;
            var roleitem = new RoleItem
            {
                Kind = role,
                FightSkill = tg_role_fight_skill.GetRoleSkillByRid(rid),
                LifeSkill = tg_role_life_skill.GetEntityByRid(rid)
            };
            return roleitem;
        }


    }
}
