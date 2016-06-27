using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Role.Service
{
    /// <summary>
    /// 武将属性保存
    /// </summary>
    public class ROLE_ATTRIBUTE
    {
        private static ROLE_ATTRIBUTE _objInstance;

        /// <summary>ROLE_ATTRIBUTE单体模式</summary>
        public static ROLE_ATTRIBUTE GetInstance()
        {
            return _objInstance ?? (_objInstance = new ROLE_ATTRIBUTE());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "ROLE_ATTRIBUTE", "武将属性保存");
#endif
                var att = data.FirstOrDefault(m => m.Key == "att").Value as object[];
                if (att == null) return Result((int)ResultType.FRONT_DATA_ERROR);      //验证前端数据

                var add = att.Select(Convert.ToInt32).ToList();
                if (!CheckPoint(add)) Result((int)ResultType.FRONT_DATA_ERROR);   //验证数据正确性

                var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7014");   //主角加点数上限
                var t = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7025");        //单项属性加点总数
                if (rule == null || t == null) return Result((int)ResultType.BASE_TABLE_ERROR);      //基础属性上限

                var mrole = session.Player.Role.Kind.CloneEntity();
                var total = add.Sum();
                var attp = mrole.att_points;
                if (attp < total) return Result((int)ResultType.ROLE_GROWADDCOUNT_LACK);      //剩余点数不足
                attp -= total;

                var rec = Record(mrole);  //记录变化前属性已加点数

                var lt = Convert.ToInt32(t.value) - 55;    //减去生活技能55点
                if (!CheckLimit(add, mrole, Convert.ToDouble(rule.value), lt))   //验证加点数是否超限
                    return Result((int)ResultType.ROLE_ADDATT_OVERRUN);
                CheckAtt(mrole);

                mrole.att_points = attp;
                mrole.Update();
                session.Player.Role.Kind = mrole;

                Change(rec, mrole);   //记录变化值
                var rolevo = (new Share.Role()).BuildRole(mrole.id);
                return new ASObject(Common.GetInstance().RoleAttData((int)ResultType.SUCCESS, mrole.att_points, rolevo));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        //验证前端数据
        private bool CheckPoint(List<int> list)
        {
            if (list.Count != 5) return false;
            return list[0] >= 0 && list[1] >= 0 && list[2] >= 0 && list[3] >= 0 && list[4] >= 0;
        }

        /// <summary>验证属性点信息</summary>
        private Boolean CheckLimit(List<int> list, tg_role role, double limit, double st)
        {
            role.base_captain_level += Convert.ToDouble(list[0]);  //统率
            if (role.base_captain_level > limit) return false;
            if (Total(RoleAttributeType.ROLE_CAPTAIN, role) > st) return false;

            role.base_force_level += Convert.ToDouble(list[1]);  //武力
            if (role.base_force_level > limit) return false;
            if (Total(RoleAttributeType.ROLE_FORCE, role) > st) return false;

            role.base_brains_level += Convert.ToDouble(list[2]);  //智谋
            if (role.base_brains_level > limit) return false;
            if (Total(RoleAttributeType.ROLE_BRAINS, role) > st) return false;

            role.base_govern_level += Convert.ToDouble(list[3]);  //政务
            if (role.base_govern_level > limit) return false;
            if (Total(RoleAttributeType.ROLE_GOVERN, role) > st) return false;

            role.base_charm_level += Convert.ToDouble(list[4]);  //魅力
            if (role.base_charm_level > limit) return false;
            if (Total(RoleAttributeType.ROLE_CHARM, role) > st) return false;

            var tadd = role.base_captain_level + role.base_force_level + role.base_brains_level + role.base_govern_level + role.base_charm_level;
            return !(tadd > limit);
        }

        /// <summary>属性点向下验证</summary>
        private void CheckAtt(tg_role role)
        {
            role.base_captain_level = role.base_captain_level < 0 ? 0 : role.base_captain_level;
            role.base_force_level = role.base_force_level < 0 ? 0 : role.base_force_level;
            role.base_brains_level = role.base_brains_level < 0 ? 0 : role.base_brains_level;
            role.base_govern_level = role.base_govern_level < 0 ? 0 : role.base_govern_level;
            role.base_charm_level = role.base_charm_level < 0 ? 0 : role.base_charm_level;
        }

        /// <summary>记录变化前属性加点值</summary>
        private List<double> Record(tg_role role)
        {
            return new List<double>
            {
                role.base_captain_level,
                role.base_force_level ,
                 role.base_brains_level ,
                 role.base_govern_level,
                 role.base_charm_level,
            };
        }

        /// <summary>记录变化过程</summary>
        private void Change(List<double> list, tg_role role)
        {
            try
            {
                var logdata = string.Format("统{0}_武{1}_智{2}_政{3}_魅{4}|{5}_{6}_{7}_{8}_{9}", list[0], list[1],
                    list[2], list[3], list[4], role.base_captain_level, role.base_force_level, role.base_brains_level,
                    role.base_govern_level, role.base_charm_level);

                (new Share.Log()).WriteLog(role.user_id, (int)LogType.Use, (int)ModuleNumber.ROLE, (int)RoleCommand.ROLE_ATTRIBUTE, logdata);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        private ASObject Result(int result)
        {
            return new ASObject(Common.GetInstance().RoleLoadData(result, null));
        }

        /// <summary>单项属性固定值-生活技能55点</summary>
        /// <param name="type">属性类型</param>
        /// <param name="model">武将信息</param>
        private Double Total(RoleAttributeType type, tg_role model)
        {
            Double result = 0;
            switch (type)
            {
                case RoleAttributeType.ROLE_CAPTAIN:
                    {
                        result += model.base_captain;
                        result += model.base_captain_level;
                        result += model.base_captain_train;
                        break;
                    }
                case RoleAttributeType.ROLE_FORCE:
                    {
                        result += model.base_force;
                        result += model.base_force_level;
                        result += model.base_force_train;
                        break;
                    }
                case RoleAttributeType.ROLE_BRAINS:
                    {
                        result += model.base_brains;
                        result += model.base_brains_level;
                        result += model.base_brains_train;
                        break;
                    }
                case RoleAttributeType.ROLE_CHARM:
                    {
                        result += model.base_charm;
                        result += model.base_charm_level;
                        result += model.base_charm_train;
                        break;
                    }
                case RoleAttributeType.ROLE_GOVERN:
                    {
                        result += model.base_govern;
                        result += model.base_govern_level;
                        result += model.base_govern_train;
                        break;
                    }
            }
            return result;
        }
    }
}
