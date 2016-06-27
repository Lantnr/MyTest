using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.Module.War.Service.Defence;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Attack
{
    /// <summary>
    /// 兵种设置
    /// 开发者：李德雁
    /// </summary>
    public class WAR_ARMY_SOLDIER_SET : IDisposable
    {
        //private static WAR_ARMY_SOLDIER_SET _objInstance;

        ///// <summary>WAR_ARMY_SOLDIER_SET单体模式</summary>
        //public static WAR_ARMY_SOLDIER_SET GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_ARMY_SOLDIER_SET());
        //}
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_ARMY_SOLDIER_SET()
        {
            Dispose();
        }
    
        #endregion

        /// <summary> 兵种设置 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var cityid = session.Player.War.PlayerInCityId;
            var city = tg_war_city.GetEntityByBaseId(cityid);
            //获取前端数据
            var tuple = GetClientData(data,session.Player.User.id);
            if (!tuple.Item1 || cityid == 0) return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            var role = tuple.Item2; var baseid = tuple.Item3; var count = tuple.Item4;
            //验证资源
            var resourse_check = CheckResourse(baseid, count, city, role);
            if (!resourse_check.Item1) return CommonHelper.ErrorResult(ResultType.BAG_RESOURCES_ERROR);
            var resourse = resourse_check.Item2;
            SavaData(resourse, role, baseid, count, city);

            return BuildData(role);
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="role">合战武将实体</param>
        /// <returns></returns>
        private ASObject BuildData(tg_war_role role)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", ResultType.SUCCESS},
                {"role", EntityToVo.ToWarRoleInfoVo(role)}
            };
            return new ASObject(dic);

        }

        /// <summary>检测资源 </summary>
        private Tuple<bool, List<Common.Resourse>> CheckResourse(Int32 baseid, int count, tg_war_city city, tg_war_role role)
        {
            var listentity = new List<Common.Resourse>();
            var add = Common.GetInstance().GetResourseString(role.army_id, 1);
            if (add == null) return Tuple.Create(false, listentity);
            var oldcount = role.army_soldier;
            if (oldcount > 0)
            {
                foreach (var resourse in add)
                {
                    switch (resourse.type)
                    {
                        //兵粮和军资金回归城市，不用锁定
                        case (int)WarResourseType.兵粮: { city.res_foods += resourse.value * oldcount; } break;
                        case (int)WarResourseType.军资金: { city.res_funds += resourse.value * oldcount; } break;
                        case (int)WarResourseType.苦无: { if (city.res_use_kuwu >= resourse.value * oldcount) city.res_use_kuwu -= resourse.value * oldcount; } break;
                        case (int)WarResourseType.薙刀: { if (city.res_use_razor >= resourse.value * oldcount) city.res_use_razor -= resourse.value * oldcount; } break;
                        case (int)WarResourseType.足轻: { if (city.res_use_soldier >= resourse.value * oldcount) city.res_use_soldier -= resourse.value * oldcount; } break;
                        case (int)WarResourseType.铁炮: { if (city.res_use_gun >= resourse.value * oldcount)city.res_use_gun -= resourse.value * oldcount; } break;
                        case (int)WarResourseType.马匹: { if (city.res_use_horse >= resourse.value * oldcount) city.res_use_horse -= resourse.value * oldcount; } break;
                    }
                }
            }

            // listentity.AddRange(add);
            var reduce = Common.GetInstance().GetResourseString(baseid, 1);
            if (reduce == null) return Tuple.Create(false, listentity);
            foreach (var resourse in reduce)
            {
                switch (resourse.type)
                {
                    case (int)WarResourseType.兵粮: { if (resourse.value * count > city.res_foods)  return Tuple.Create(false, listentity); } break;
                    case (int)WarResourseType.军资金: { if (resourse.value * count > city.res_funds) return Tuple.Create(false, listentity); } break;
                    case (int)WarResourseType.苦无: { if (resourse.value * count > city.res_kuwu - city.res_use_kuwu)  return Tuple.Create(false, listentity); } break;
                    case (int)WarResourseType.薙刀: { if (resourse.value * count > city.res_razor - city.res_use_razor) return Tuple.Create(false, listentity); } break;
                    case (int)WarResourseType.足轻: { if (resourse.value * count > city.res_soldier - city.res_use_soldier)  return Tuple.Create(false, listentity); } break;
                    case (int)WarResourseType.铁炮: { if (resourse.value * count > city.res_gun - city.res_use_gun)  return Tuple.Create(false, listentity); } break;
                    case (int)WarResourseType.马匹: { if (resourse.value * count > city.res_horse - city.res_use_horse) return Tuple.Create(false, listentity); } break;
                }
            }
            listentity.AddRange(reduce);
            return Tuple.Create(true, listentity);

        }


        /// <summary>
        /// 获取前端数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>1.数据是否正确 2.合战武将实体  3.部队基表id 4.部队数量 5.原始数量</returns>
        private Tuple<bool, tg_war_role, Int32, Int32> GetClientData(ASObject data,Int64 userid)
        {
            if (!data.ContainsKey("roleId") || !data.ContainsKey("soldierId") || !data.ContainsKey("count"))
                return Tuple.Create(false, new tg_war_role(), 0, 0);
            var _roleid = data.FirstOrDefault(q => q.Key == "roleId").Value.ToString();
            var _baseid = data.FirstOrDefault(q => q.Key == "soldierId").Value.ToString();
            var _count = data.FirstOrDefault(q => q.Key == "count").Value.ToString();

            Int64 roleid; Int32 baseid; Int32 count;

            Int64.TryParse(_roleid, out roleid);
            Int32.TryParse(_baseid, out baseid);
            Int32.TryParse(_count, out count);


            var role = tg_war_role.GetEntityById(roleid, userid);//FindByid(roleid);
            var soldier = tg_war_army_type.GetEntityByRidAndBaseId(roleid, baseid);
            if (role == null || soldier == null || count <= 0)
                return Tuple.Create(false, role, 0, 0);

            if (role.type == (int)WarRoleType.NPC)
            {
                var identify = Variable.BASE_IDENTITY.FirstOrDefault(q => q.vocation == (int)VocationType.Roles);
                if (identify == null) return Tuple.Create(false, role, 0, 0);
                if (identify.soldier < count) return Tuple.Create(false, role, 0, 0);
            }
            else
            {
                var roleinfo = tg_role.GetEntityByIdUid(role.rid, userid);//FindByid(role.rid);
                if (roleinfo == null) return Tuple.Create(false, role, 0, 0);
                var baseidentify = Variable.BASE_IDENTITY.FirstOrDefault(q => q.id == roleinfo.role_identity);
                if (baseidentify == null) return Tuple.Create(false, role, 0, 0);
                if (baseidentify.soldier < count) return Tuple.Create(false, role, 0, 0);
            }

            return Tuple.Create(true, role, baseid, count);
        }

        private bool SavaData(List<Common.Resourse> resourses, tg_war_role role, Int32 baseid, Int32 count, tg_war_city city)
        {
            role.army_id = baseid;
            role.army_soldier = count;
            //更新城市资源和武将资源
            foreach (var resourse in resourses)
            {
                switch (resourse.type)
                {
                    case (int)WarResourseType.兵粮: { city.res_foods -= resourse.value * count; } break;
                    case (int)WarResourseType.军资金: { city.res_funds -= resourse.value * count; } break;
                    case (int)WarResourseType.苦无: { city.res_use_kuwu += resourse.value * count; role.army_kuwu = resourse.value * count; } break;
                    case (int)WarResourseType.薙刀: { city.res_use_razor += resourse.value * count; role.army_razor = resourse.value * count; } break;
                    case (int)WarResourseType.足轻: { city.res_use_soldier += resourse.value * count; role.army_soldier = resourse.value * count; } break;
                    case (int)WarResourseType.铁炮: { city.res_use_gun += resourse.value * count; role.army_gun = resourse.value * count; } break;
                    case (int)WarResourseType.马匹: { city.res_use_horse += resourse.value * count; role.army_horse = resourse.value * count; } break;
                }
            }
            //保存数据
            role.Update();
            city.Update();
            Variable.WarCityAll.TryUpdate(city.base_id, city, city);
            //推送数据
            var list = new[] { "armyBaseId", "armyCount" };
            new Share.War().SendWarRole(role, list);
            new Share.War().SendCity(city.base_id, role.user_id);
            //tg_war_city.Update(string.Format("res_use_foods={0},res_use_funds={1},res_use_kuwu={2},res_use_razor={3}," +
            //                                 "res_use_soldier={4},res_use_gun={5},res_use_horse={6}"city.res_use_foods, city.res_use_funds))
            return true;

        }

        ///// <summary>
        ///// 获取资源
        ///// </summary>
        ///// <param name="baseid"></param>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public List<Common.Resourse> GetResourseString(Int32 baseid, int type)
        //{
        //    var listentity = new List<Common.Resourse>();
        //    var baseinfo = Variable.BASE_WAR_ARMY_SOLDIER.FirstOrDefault(q => q.id == baseid);
        //    if (baseinfo == null) return null;
        //    var re_string = baseinfo.cost;
        //    listentity.AddRange(Common.Resourse.GetList(re_string, type));

        //    return listentity;
        //}
    }
}
