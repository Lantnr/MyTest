using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Share
{
    public partial class War
    {
        /// <summary>
        /// 插入默认武将合战兵种
        /// </summary>
        /// <param name="rid">武将主键id</param>
        /// <param name="baseid">合战兵种基表id</param>
        public void GetWarRoleArmySoldier(Int64 rid, Int32 baseid)
        {
            //插入武将兵种数据

            tg_war_army_type.Insert(new tg_war_army_type() { base_id = baseid, rid = rid });

        }

        public void SendWarRole(tg_war_role role, List<Int32> otherinfo, params string[] name)
        {
            if (!Variable.OnlinePlayer.ContainsKey(role.user_id)) return;
            var session = Variable.OnlinePlayer[role.user_id] as TGGSession;
            var dic = new Dictionary<string, object>();

            #region 要推送的属性

            foreach (var item in name)
            {
                switch (item)
                {
                    case "state":
                        {
                            dic.Add("state", role.state);
                            break;
                        }
                    case "station":
                        {
                            dic.Add("station", role.station);
                            break;
                        }
                    case "armyBaseId":
                        {
                            dic.Add("armyBaseId", role.army_id);
                            break;
                        }
                    case "armyCount":
                        {
                            dic.Add("armyCount", role.army_soldier);
                            break;
                        }
                    case "ArmyTypes":
                        {
                            dic.Add("ArmyTypes", otherinfo);
                            break;
                        }
                    case "ArmyFormation":
                        {
                            dic.Add("ArmyFormation", otherinfo);
                            break;
                        }
                }
            }
            #endregion

            var data = BulidData(role.id, role.type, new ASObject(dic));
            var pv = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.WAR_ROLE_UPDATE, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }

        /// <summary>
        /// 插入据点默认防守方案并返回主键id
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="cityid">据点基表ID</param>
        /// <returns></returns>
        public Int64 GetDefensePlanInsert(Int64 userid, Int32 cityid)
        {
            //插入阵表
            var basefront = Variable.BASE_WAR_FRONT.FirstOrDefault(q => q.free == 1);
            tg_war_formation.Insert(new tg_war_formation() { user_id = userid, base_id = basefront.id });
            //插入防守方案表
            var newplan = new tg_war_city_plan() { user_id = userid, location = 0, formation = basefront.id };
            newplan.Insert();
            ////关联防守武将表
            //tg_war_city_defense.Insert(new tg_war_city_defense() { user_id = userid, city_id = cityid, plan_id = newplan.id });
            ////关联防守地形表
            //tg_war_plan_area.Insert((new tg_war_plan_area() { plan_id = newplan.id }));

            return newplan.id;

        }

        /// <summary>
        /// 插入新的防守方案
        /// </summary>
        /// <param name="city">据点实体</param>
        /// <param name="location">新防守方案位置（2、3）</param>
        /// <returns></returns>
        public tg_war_city_plan GetDefensePlanInsert(tg_war_city city, int location)
        {
            //插入阵表
            var basefront = Variable.BASE_WAR_FRONT.FirstOrDefault(q => q.free == 1);
            tg_war_formation.Insert(new tg_war_formation() { user_id = city.user_id, base_id = basefront.id });
            //插入防守方案表
            var newplan = new tg_war_city_plan() { user_id = city.user_id, location = location, formation = basefront.id };
            newplan.Insert();
            ////关联防守武将表
            //tg_war_city_defense.Insert(new tg_war_city_defense() { user_id = city.user_id, city_id = city.base_id, plan_id = newplan.id });
            ////关联防守地形表
            //tg_war_plan_area.Insert((new tg_war_plan_area() { plan_id = newplan.id }));
            //更新据点数据
            switch (location)
            {
                //locatio: 0-2
                case 1: { city.plan_2 = newplan.id; } break;
                case 2: { city.plan_3 = newplan.id; } break;
            }
            newplan.Update();
            return newplan;
        }

        /// <summary>
        /// 随机更新npc城防守地形
        /// </summary>
        public void GetNewNpcPlanId()
        {
            var npccitys = Variable.BASE_WARCITY.Where(q => q.type == 1).ToList();
            foreach (var baseWarCity in npccitys)
            {
                var index = RNG.Next(0, Variable.BASE_LAND_POOL.Count - 1);
                var baseLandid = Variable.BASE_LAND_POOL[index].id;
                Variable.WarNpcDefenseInfo.AddOrUpdate(baseWarCity.id, baseLandid, (k, v) => baseLandid);

            }



        }
    }
}
