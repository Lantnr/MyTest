using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.Core.Vo.War;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary>
    /// 进入一个模块
    /// </summary>
    public class WAR_MODEL_IN : IDisposable
    {
        //private static WAR_MODEL_IN _objInstance;

        ///// <summary>WAR_MODEL_IN单体模式</summary>
        //public static WAR_MODEL_IN GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_MODEL_IN());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 进入模块 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            Variable.RemoveLine(session.Player.User.id);//移除所有路线缓存

            if (!data.ContainsKey("id"))
                return null;
            var mid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);

            var userid = session.Player.User.id;
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;

            var ps = GetPartnersEntityList(userid, time); //获取还在同盟时间内的盟友

            var list = GetPlayerWarCityVo(mid, userid, ps);
            list.AddRange(GetNpcWarCityVo(time));

            var roless = tg_war_role.GetPlayerWarRoleByUserId(userid, (int)WarRoleType.PLAYER).ToList();

            var l = tg_war_battle_queue.GetEntityListByUserid(userid);

            #region 验证是否存在主据点

            if (session.Player.War.WarCityId == 0)
            {
                foreach (var item in l)
                {
                    item.Delete();
                }
                if (session.Player.UserExtend.war_total_own > 0)
                {
                    session.Player.UserExtend.war_total_own = 0;
                    session.Player.UserExtend.Update();
                }
                l = new List<tg_war_battle_queue>();
            }

            #endregion

            var roles = new List<tg_war_role>();
            foreach (var item in l)
            {
                var rls = new List<tg_war_role>();
                var temp = item;
                var tt = (DateTime.Now.Ticks - 621355968000000000) / 10000;
                if (temp.time < tt) (new Share.War()).SettlementResourceByEntity(ref temp, ref rls);
                roless = rls.Aggregate(roless, (current, role) => current.Where(m => m.id != role.id).ToList());
                roles.AddRange(rls);
            }
            roless.AddRange(roles);
            var rs = (new Share.War()).ToWarRoleInfoVos(roless);

            var gos = Common.GetInstance().ConvertGoVos(l);

            Variable.WarInUser.AddOrUpdate(userid, mid, (k, v) => mid);
            var state = tg_war_partner.IsExistPartnerRequest(userid, time) ? 1 : 0;//0:无外交请求  1:有外交请求
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32063");
            if (rule == null)
                return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            var devote = Convert.ToInt32(rule.value) - session.Player.UserExtend.devote_limit;
            return BulidData(list, rs, gos, state, devote);
        }

        /// <summary> 获取指定模块所有玩家的据点集合 </summary>
        /// <param name="mid">模块id</param>
        /// <param name="userid">用户id</param>
        /// <param name="ps">盟友集合</param>
        private List<WarCityVo> GetPlayerWarCityVo(int mid, Int64 userid, List<tg_war_partner> ps)
        {
            var list = view_war_city.GetEntityByMoudleNumber(mid);
            if (!list.Any()) return new List<WarCityVo>();
            return ConvertListWarCityVo(list, ps, userid);
        }

        /// <summary> 获取盟友集合 </summary>
        private List<tg_war_partner> GetPartnersEntityList(Int64 userid, Int64 time)
        {
            return tg_war_partner.GetEntityByUserIdAndTime(userid, time);
        }

        /// <summary> 获取在指定保护时间内的NPC据点 </summary>
        /// <param name="time">指定时间</param>
        private List<WarCityVo> GetNpcWarCityVo(Int64 time)
        {
            var list = new List<WarCityVo>();
            var citynpcs = (new Share.War()).GetWarCityListByType((int)WarCityOwnershipType.NPC, time);
            if (!citynpcs.Any()) return list;
            const int ow = (int)WarCityOwnershipType.NPC;
            const int state = (int)WarCityCampType.ENEMY;
            return (citynpcs.Select(item => EntityToVo.ToWarCityVo(item, state, ow))).ToList();
        }

        /// <summary> 组装数据 </summary>
        /// <param name="list">合战模块据点Vo集合</param>
        /// <param name="roles">合战武将集合</param>
        /// <param name="gos">出征队列集合</param>
        /// <param name="state">0:无外交请求  1:有外交请求</param>
        /// <param name="devote">剩余贡献度</param>
        private ASObject BulidData(List<WarCityVo> list, List<WarRoleInfoVo> roles, List<WarGoVo> gos, int state, int devote)
        {
            var dic = new Dictionary<string, object> { 
            { "result", (int)ResultType.SUCCESS },
            { "list",list},
             { "role",roles},
              { "warGoList",gos},
             { "state",state},
             {"surplus",devote},
            };
            return new ASObject(dic);
        }

        /// <summary> 玩家据点实体转Vo </summary>
        /// <param name="list">玩家视图实体集合</param>
        /// <param name="partners">盟友Id集合</param>
        /// <param name="userid">自己的用户Id</param>
        private List<WarCityVo> ConvertListWarCityVo(List<view_war_city> list, List<tg_war_partner> partners, Int64 userid)
        {
            Int64 time = 0;
            var l = new List<WarCityVo>();
            var roles = new List<tg_war_role>();
            const int ow = (int)WarCityOwnershipType.PLAYER;
            var ls = partners.Select(m => m.partner_id).ToList();
            var cityids = (list.Select(m => m.base_id)).ToList();
            if (cityids.Any())
                roles = tg_war_role.GetFindAllByCityIds(cityids); //获取该模块所有城市的武将

            foreach (var item in list)
            {
                int state;
                if (ls.Contains(item.user_id))
                {
                    state = (int)WarCityCampType.PARTNER;
                    var p = partners.FirstOrDefault(m => m.partner_id == item.user_id);
                    if (p == null) continue;
                    time = p.time;
                }
                else
                    state = item.user_id == userid ? (int)WarCityCampType.OWN : (int)WarCityCampType.ENEMY;
                var count = roles.Count(m => m.station == item.base_id);
                var temp = EntityToVo.ToWarCityVo(item, state, ow, time);
                l.Add(temp);
            }
            return l;
        }
    }
}
