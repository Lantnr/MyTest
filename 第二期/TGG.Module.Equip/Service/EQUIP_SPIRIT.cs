using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Module.Equip.Service
{
    /// <summary>
    /// 装备铸魂
    /// </summary>
    public class EQUIP_SPIRIT
    {
        private static EQUIP_SPIRIT ObjInstance;

        /// <summary> EQUIP_SPIRIT单体模式 </summary>
        public static EQUIP_SPIRIT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new EQUIP_SPIRIT());
        }

        private int result;
        /// <summary> 装备铸魂</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "EQUIP_SPIRIT", "装备铸魂");
#endif
                var user = session.Player.User.CloneEntity();
                var equipid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);
                var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);
                var count = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "count").Value);
                var roleid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "roleId").Value);
                var location = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "location").Value);
                if (equipid == 0 || location == 0 || count == 0 || roleid == 0) return Error((int)ResultType.FRONT_DATA_ERROR, 0);

                var equipinfo = tg_bag.GetEntityById(equipid);
                var role = tg_role.GetRoleById(roleid);
                if (equipinfo == null || role == null) return Error((int)ResultType.DATABASE_ERROR, 0);
                if (!(new Share.Equip()).IsContainAttritute(type, equipinfo)) return Error((int)ResultType.FRONT_DATA_ERROR, 0);
                if (user.spirit < count) return Error((int)ResultType.BASE_PLAYER_SPIRIT_ERROR, 0);

                #region 获取基表信息
                var base_rule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "6001");
                var level =(new Share.Equip()).GetEquipLevel(location, equipinfo);
                var base_equip = Variable.BASE_EQUIP.FirstOrDefault(m => m.id == equipinfo.base_id);
                if (base_rule == null || base_equip == null) return Error((int)ResultType.BASE_TABLE_ERROR, 0);
                var base_spirit = Variable.BASE_SPIRIT.FirstOrDefault(m => m.userLv == base_equip.useLevel && m.lv == level);
                if (base_spirit == null) return Error((int)ResultType.BASE_TABLE_ERROR, 0);
                var next_spirit = Variable.BASE_SPIRIT.FirstOrDefault(m => m.id == base_spirit.updateId);
                if (next_spirit == null) return Error((int)ResultType.BASE_TABLE_ERROR, 0);
                var _next_spirit = next_spirit.CloneEntity();
                #endregion

                var value = Convert.ToInt32(base_rule.value);
                var spiritvalue = GetSpiritValue(location, count, equipinfo);
                if (spiritvalue > base_spirit.spirit) return Error((int)ResultType.EQUIP_SPIRIT_ENOUGH, 0);
                if (!CheckLockAndSprict(value, type, location, equipinfo)) return Error((int)ResultType.EQUIP_SPIRIT_UNLOCK, 0);
                return Procress(value, location, count, type, base_spirit.spirit, role, equipinfo, _next_spirit);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        // base_rule.value   base_spirit.spirit  _next_spirit


        /// <summary>装备注魂处理</summary>
        /// <param name="rulevalue">解锁等级</param>
        /// <param name="location">属性位置</param>
        /// <param name="count">添加的魂值</param>
        /// <param name="type">属性类型</param>
        /// <param name="basespirit">当前等级最大魂值</param>
        /// <param name="nextspirit">下一等级基表魂</param>
        private ASObject Procress(int rulevalue,int location, int count, int type, int basespirit,tg_role role, tg_bag equipinfo, BaseSpirit nextspirit)
        {
            if (!Variable.OnlinePlayer.ContainsKey(role.user_id))
                return new ASObject();
            var session = Variable.OnlinePlayer[role.user_id] as TGGSession;
            if (session == null) return new ASObject();
            var user = session.Player.User.CloneEntity();

            var spiritvalue = GetSpiritValue(location, count, equipinfo); //当前魂值+添加的魂值
            var valuespirit = GetValueSpirit(location, equipinfo);//升级前的固定属性值
            equipinfo = GetSpirit(rulevalue, type, count, location, equipinfo, basespirit, nextspirit);
           
            if (spiritvalue == basespirit)
            {
                
                role = RoleAttritute(role, type, nextspirit, valuespirit); //武将属性值增加
                if (!tg_role.GetRoleUpdate(role)) return Error((int)ResultType.DATABASE_ERROR, 0);
            }

            user.spirit = user.spirit - count;
            if (tg_bag.GetEquipUpdate(equipinfo) <= 0) return Error((int)ResultType.DATABASE_ERROR, 0);
            if (!tg_user.GetUserUpdate(user)) return Error((int)ResultType.DATABASE_ERROR, 0);
            var org = session.Player.User.spirit;
            session.Player.User = user;

            if (role.id == session.Player.Role.Kind.id)
                session.Player.Role.Kind = role;

            //日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Spirit", org, count, user.spirit);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.EQUIP, (int)EquipCommand.EQUIP_SPIRIT, logdata);
           
            return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS, equipinfo, user.spirit, role.id));
        }



        /// <summary>获取装备属性当前魂值</summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        /// <param name="equip"></param>
        /// <returns></returns>
        private int GetSpiritValue(int location, int value, tg_bag equip)
        {
            switch (location)
            {
                case (int)EquipPositionType.ATT1_LOCATION:
                    return equip.attribute1_spirit_value + value;
                case (int)EquipPositionType.ATT2_LOCATION:
                    return equip.attribute2_spirit_value + value;
                case (int)EquipPositionType.ATT3_LOCATION:
                    return equip.attribute3_spirit_value + value;
            }
            return 0;
        }

        /// <summary>获取装备注魂固定属性值</summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        /// <param name="equip"></param>
        /// <returns></returns>
        private double GetValueSpirit(int location, tg_bag equip)
        {
            switch (location)
            {
                case (int)EquipPositionType.ATT1_LOCATION:
                    return equip.attribute1_value_spirit;
                case (int)EquipPositionType.ATT2_LOCATION:
                    return equip.attribute2_value_spirit;
                case (int)EquipPositionType.ATT3_LOCATION:
                    return equip.attribute3_value_spirit;
            }
            return 0;
        }

        /// <summary> 添加属性魂值 </summary>
        /// <param name="type">装备属性类型</param>
        /// <param name="count">魂数</param>
        /// <param name="equip">注魂的装备</param>
        /// <returns>装备信息</returns>
        private tg_bag GetSpirit(int level, int type, int count, int location, tg_bag equip, int spirit, BaseSpirit next_spirit)
        {
            switch (location)
            {
                case (int)EquipPositionType.ATT1_LOCATION:
                    if (type == equip.attribute1_type)  //属性1
                    {
                        equip.attribute1_spirit_value += count;
                        if (equip.attribute1_spirit_value != spirit) return equip;
                        equip.attribute1_spirit_level = next_spirit.lv;
                        if (equip.attribute1_spirit_level >= level) equip.attribute1_spirit_lock = (int)SpiritLockType.LOCK;
                        equip.attribute1_value_spirit = EquipAttritute(equip.attribute1_type, next_spirit);
                        equip.attribute1_spirit_value = 0; //魂数清0
                    }
                    break;
                case (int)EquipPositionType.ATT2_LOCATION:
                    if (type == equip.attribute2_type)  //属性2
                    {
                        equip.attribute2_spirit_value += count;
                        if (equip.attribute2_spirit_value != spirit) return equip;
                        equip.attribute2_spirit_level = next_spirit.lv;
                        if (equip.attribute2_spirit_level >= level) equip.attribute2_spirit_lock = (int)SpiritLockType.LOCK;
                        equip.attribute2_value_spirit = EquipAttritute(equip.attribute2_type, next_spirit);
                        equip.attribute2_spirit_value = 0; //魂数清0
                    }
                    break;
                case (int)EquipPositionType.ATT3_LOCATION:
                    if (type == equip.attribute3_type)   //属性3
                    {
                        equip.attribute3_spirit_value += count;
                        if (equip.attribute3_spirit_value != spirit) return equip;
                        equip.attribute3_spirit_level = next_spirit.lv;
                        if (equip.attribute3_spirit_level >= level) equip.attribute3_spirit_lock = (int)SpiritLockType.LOCK;
                        equip.attribute3_value_spirit = EquipAttritute(equip.attribute3_type, next_spirit);
                        equip.attribute3_spirit_value = 0; //魂数清0
                    }
                    break;
            }
            return equip;
        }


        /// <summary>验证是否解锁</summary>
        private bool CheckLockAndSprict(int level, int type, int location, tg_bag equip)
        {
            switch (location)
            {
                case (int)EquipPositionType.ATT1_LOCATION:
                    if (type == equip.attribute1_type)
                    {
                        if (equip.attribute1_spirit_level >= level && equip.attribute1_spirit_lock != (int)SpiritLockType.UNLOCK)
                            return false;
                    }
                    break;
                case (int)EquipPositionType.ATT2_LOCATION:
                    if (type == equip.attribute2_type)
                    {
                        if (equip.attribute2_spirit_level >= level && equip.attribute2_spirit_lock != (int)SpiritLockType.UNLOCK)
                            return false;
                    }
                    break;
                case (int)EquipPositionType.ATT3_LOCATION:
                    if (type == equip.attribute3_type)
                    {
                        if (equip.attribute3_spirit_level >= level && equip.attribute3_spirit_lock != (int)SpiritLockType.UNLOCK)
                            return false;
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// 属性魂升阶，增加武将属性值
        /// </summary>
        /// <param name="role">穿戴装备的武将</param>
        /// <param name="type">装备的属性类型</param>
        /// <param name="base_spirit">基表数据</param>
        /// <returns>返回武将信息</returns>
        private tg_role RoleAttritute(tg_role role, int type, BaseSpirit base_spirit, double ov)
        {
            var mn = (int) ModuleNumber.EQUIP;
            var rc = (int)EquipCommand.EQUIP_SPIRIT;
            var base_rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7015");
            if (base_rule == null) return role;
            switch (type)
            {
                #region
                case (int)RoleAttributeType.ROLE_ATTACK:
                    {
                        var attack = role.att_attack;
                        role.att_attack = role.att_attack - ov + base_spirit.attack;
                        RoleAttrituteLog(role.user_id, type, base_spirit.attack-ov, attack, role.att_attack, mn, rc);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_BRAINS:
                    {
                        var brains = role.base_brains_spirit;
                        role.base_brains_spirit = role.base_brains_spirit - ov + base_spirit.brains;
                        RoleAttrituteLog(role.user_id, type, base_spirit.brains - ov, brains, role.base_brains_spirit, mn, rc);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_CAPTAIN:
                    {
                        var captain = role.base_captain_spirit;
                        role.base_captain_spirit = role.base_captain_spirit - ov + base_spirit.captain;
                        RoleAttrituteLog(role.user_id, type, base_spirit.captain - ov, captain, role.base_captain_spirit, mn, rc);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_CHARM:
                    {
                        var charm = role.base_charm_spirit;
                        role.base_charm_spirit = role.base_charm_spirit - ov + base_spirit.charm;
                        RoleAttrituteLog(role.user_id, type, base_spirit.charm - ov, charm, role.base_charm_spirit, mn, rc);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_DEFENSE:
                    {
                        var defense = role.att_defense;
                        role.att_defense = role.att_defense - ov + base_spirit.defense;
                        RoleAttrituteLog(role.user_id, type, base_spirit.defense - ov, defense, role.att_defense, mn, rc);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_FORCE:
                    {
                        var force = role.base_force_spirit;
                        role.base_force_spirit = role.base_force_spirit - ov + base_spirit.force;
                        RoleAttrituteLog(role.user_id, type, base_spirit.force - ov, force, role.base_force_spirit, mn, rc);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_GOVERN:
                    {
                        var govern = role.base_govern_spirit;
                        role.base_govern_spirit = role.base_govern_spirit - ov + base_spirit.govern;
                        RoleAttrituteLog(role.user_id, type, base_spirit.govern - ov, govern, role.base_govern_spirit, mn, rc);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_HURTINCREASE:
                    {
                        var hurtIncrease = role.att_sub_hurtIncrease;
                        role.att_sub_hurtIncrease = role.att_sub_hurtIncrease - ov + base_spirit.hurtIncrease;
                        RoleAttrituteLog(role.user_id, type, base_spirit.hurtIncrease - ov, hurtIncrease, role.att_sub_hurtIncrease, mn, rc);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_HURTREDUCE:
                    {
                        var hurtReduce = role.att_sub_hurtReduce;
                        role.att_sub_hurtReduce = role.att_sub_hurtReduce - ov + base_spirit.hurtReduce;
                        RoleAttrituteLog(role.user_id, type, base_spirit.hurtReduce - ov, hurtReduce, role.att_sub_hurtReduce, mn, rc);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_LIFE:
                    {
                        var life = role.att_life;
                        role.att_life = role.att_life - (int)ov + base_spirit.life;
                        RoleAttrituteLog(role.user_id, type, base_spirit.life - ov, life, role.att_life, mn, rc);
                        break;
                    }
                #endregion
            }
            return role;
        }

        /// <summary>武将属性变动添加日志 </summary>
        /// <param name="userid">玩家id</param>
        /// <param name="type">属性类型</param>
        /// <param name="add">增加的值</param>
        /// <param name="ov">武将增加前的属性值</param>
        /// <param name="nv">武将增加后的属性值</param>
        /// <param name="ModuleNumber">模块号</param>
        /// <param name="command">指令号</param>
        private void RoleAttrituteLog(Int64 userid, int type, double add, double ov, double nv, int ModuleNumber,int command)
        {            
            string logdata = "";
            switch (type)
            {
                #region
                case (int)RoleAttributeType.ROLE_ATTACK:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleAttack", ov, add, nv);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_BRAINS:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleBrains", ov, add, nv);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_CAPTAIN:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleCaptain", ov, add, nv);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_CHARM:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleCharm", ov, add, nv);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_DEFENSE:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleDefense", ov, add, nv);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_FORCE:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleForce", ov, add, nv);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_GOVERN:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleGovern", ov, add, nv);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_HURTINCREASE:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleHurtIncrease", ov, add, nv);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_HURTREDUCE:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleHurtReduce", ov, add, nv);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_LIFE:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleLife", ov, add, nv);
                        break;
                    }
                #endregion
            }
            (new Share.Log()).WriteLog(userid, (int) LogType.Get, ModuleNumber, command,logdata);// (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_ROLE_LOCK, logdata);
        } 


        /// <summary>
        /// 属性魂升阶，增加装备属性值
        /// </summary>
        /// <param name="type">装备的属性类型</param>
        /// <param name="base_spirit">基表数据</param>
        /// <returns>升阶的属性加成值</returns>
        private int EquipAttritute(int type, BaseSpirit base_spirit)
        {
            switch (type)
            {
                case (int)RoleAttributeType.ROLE_ATTACK: return base_spirit.attack;
                case (int)RoleAttributeType.ROLE_BRAINS: return base_spirit.brains;
                case (int)RoleAttributeType.ROLE_CAPTAIN: return base_spirit.captain;
                case (int)RoleAttributeType.ROLE_CHARM: return base_spirit.charm;
                case (int)RoleAttributeType.ROLE_DEFENSE: return base_spirit.defense;
                case (int)RoleAttributeType.ROLE_FORCE: return base_spirit.force;
                case (int)RoleAttributeType.ROLE_GOVERN: return base_spirit.govern;
                case (int)RoleAttributeType.ROLE_HURTINCREASE: return (int)base_spirit.hurtIncrease;
                case (int)RoleAttributeType.ROLE_HURTREDUCE: return (int)base_spirit.hurtReduce;
                case (int)RoleAttributeType.ROLE_LIFE: return base_spirit.life;
            }
            return 0;
        }

        private ASObject Error(int error, int spirit)
        {
            return new ASObject(Common.GetInstance().BuilData(error, null, spirit, 0));//, null));
        }
    }
}
