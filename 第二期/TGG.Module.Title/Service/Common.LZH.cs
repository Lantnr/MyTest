using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Role;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.Title.Service
{
    /// <summary>
    /// 称号公共方法
    /// </summary>
    public partial class Common
    {
        #region 组装数据

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuildData(int result)
        {
            var dic = new Dictionary<string, object> { { "result", result } };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuildLoadData(int result, tg_role_title title)
        {
            var dic = new Dictionary<string, object>
            {
                { "result", result }, 
                { "title", title == null ? null : EntityToVo.ToTitleVo(title) }
            };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuildTitleSelect(int result, int count, tg_role_title title)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"count", count},
                { "title", title == null ? null : EntityToVo.ToTitleVo(title) }
            };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuildTitlesData(int result, List<int> value, List<tg_role_title> listtitles)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"value", value},
                {"titles", listtitles.Any() ? ListAsObject(listtitles) : null}
            };
            return dic;
        }

        /// <summary>转换为ListAsObject</summary>
        public List<ASObject> ListAsObject(List<tg_role_title> listtitles)
        {
            if (!listtitles.Any()) return null;
            var list = listtitles.Select(item => AMFConvert.ToASObject(EntityToVo.ToTitleVo(item))).ToList();
            return list;
        }

        /// <summary>组装数据 </summary>
        private Dictionary<string, object> BuildData(int type, RoleInfoVo role)
        {
            var dic = new Dictionary<string, object> { { "type", type }, { "role", role }, };
            return dic;
        }

        #endregion

        /// <summary>更新武将信息</summary>
        /// <param name="role">武将信息</param>
        /// <param name="type">加成或是削减</param>
        /// <param name="value">值</param>
        public tg_role UpdateRole(tg_role role, int type, string value)
        {
            if (string.IsNullOrEmpty(value)) return role;
            if (value.Contains("|"))
            {
                var add = value.Split("|").ToList();
                foreach (var item in add)
                {
                    if (!item.Contains("_")) continue;
                    var str = item.Split("_").ToList();
                    var addType = Convert.ToInt32(str[0]);
                    var sum = Convert.ToInt32(str[1]);
                    role = AddTionUpdate(role, type, addType, sum);
                }
            }
            else if (value.Contains("_"))
            {
                var str = value.Split("_").ToList();
                var addType = Convert.ToInt32(str[0]);
                var sum = Convert.ToInt32(str[1]);
                role = AddTionUpdate(role, type, addType, sum);
            }
            return role;
        }

        /// <summary>处理武将基础属性</summary>
        public tg_role AddTionUpdate(tg_role role, int type, int addType, double value)
        {
            if (type == (int)RoleDatatype.ROLEDATA_ADD)
            {
                switch (addType)
                {
                    case (int)RoleAttributeType.ROLE_CAPTAIN: role.base_captain_title += value; break;  //统率
                    case (int)RoleAttributeType.ROLE_FORCE: role.base_force_title += value; break;         //武力
                    case (int)RoleAttributeType.ROLE_BRAINS: role.base_brains_title += value; break;       //智谋
                    case (int)RoleAttributeType.ROLE_GOVERN: role.base_govern_title += value; break;    //政务
                    case (int)RoleAttributeType.ROLE_CHARM: role.base_charm_title += value; break;       //魅力
                }
            }
            else
            {
                switch (addType)
                {
                    case (int)RoleAttributeType.ROLE_CAPTAIN: role.base_captain_title -= value; break;
                    case (int)RoleAttributeType.ROLE_FORCE: role.base_force_title -= value; break;
                    case (int)RoleAttributeType.ROLE_BRAINS: role.base_brains_title -= value; break;
                    case (int)RoleAttributeType.ROLE_GOVERN: role.base_govern_title -= value; break;
                    case (int)RoleAttributeType.ROLE_CHARM: role.base_charm_title -= value; break;
                }
                role = CheckRoleAtt(role);
            }
            return role;
        }

        /// <summary>卸载称号向下验证武将属性</summary>
        public tg_role CheckRoleAtt(tg_role role)
        {
            if (role.base_captain_title < 0) role.base_captain_title = 0;
            if (role.base_force_title < 0) role.base_force_title = 0;
            if (role.base_brains_title < 0) role.base_brains_title = 0;
            if (role.base_govern_title < 0) role.base_govern_title = 0;
            if (role.base_charm_title < 0) role.base_charm_title = 0;
            if (role.att_sub_hurtIncrease < 0) role.att_sub_hurtIncrease = 0;
            if (role.att_sub_hurtReduce < 0) role.att_sub_hurtReduce = 0;
            if (role.att_attack < 0) role.att_attack = 0;
            if (role.att_defense < 0) role.att_defense = 0;
            if (role.att_life < 0) role.att_life = 0;
            if (role.att_crit_addition < 0) role.att_crit_addition = 0;
            if (role.att_crit_probability < 0) role.att_crit_probability = 0;
            if (role.att_dodge_probability < 0) role.att_dodge_probability = 0;
            if (role.att_mystery_probability < 0) role.att_mystery_probability = 0;
            return role;
        }

        /// <summary>称号装备武将数量</summary>
        public int Count(tg_role_title title)
        {
            var sum = 0;
            if (title.packet_role1 != 0) sum += 1;
            if (title.packet_role2 != 0) sum += 1;
            if (title.packet_role3 != 0) sum += 1;
            return sum;
        }

        /// <summary>推送武将属性更新</summary>
        /// <param name="userid">用户userid</param>
        /// <param name="rid">武将主键rid</param>
        public void RoleUpdatePush(Int64 userid, Int64 rid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;

            var rolevo = (new Share.Role()).BuildRole(rid);
            (new Share.Role()).SendPv(session, new ASObject(BuildData(0, rolevo)));   //加入到推送模块
        }
    }
}

