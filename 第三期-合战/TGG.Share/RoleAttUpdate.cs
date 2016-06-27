using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using NewLife.Reflection;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.Share.Event;
using TGG.SocketServer;

namespace TGG.Share
{
    public class RoleAttUpdate
    {
        /// <summary>武将属性更新(多个)并推送 </summary>
        public void RoleUpdatePush(tg_role role, Int64 userid, IEnumerable<string> name)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            var aso = BuildUpdateRoleData(role, name);
            var pv = session.InitProtocol((int)ModuleNumber.ROLE,
                      (int)RoleCommand.ROLE_UPDATE, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }


        /// <summary>组装更新数据 </summary>
        public ASObject BuildUpdateRoleData(tg_role role, IEnumerable<string> name)
        {
#if DEBUG
            var sw = new Stopwatch();
            sw.Start();
#endif
            var dic = new Dictionary<string, object> { { "id", role.id } };
            var dic1 = new Dictionary<string, object>();
            foreach (var _item in name.Select(item => item.ToLower()))
            {
                switch (_item)
                {
                    //总体力
                    case "power": { dic.Add("power", tg_role.GetTotalPower(role)); break; }
                    //基础体力
                    case "rolepower": { dic.Add("rolePower", role.power); break; }
                    //经验
                    //case "experience": { dic.Add("experience", role.role_level == 60 ? 10000 : role.role_exp); break; }
                    case "experience": { dic.Add("experience",role.role_exp); break; }
                    //身份
                    case "identityid": { dic.Add("identityId", role.role_identity); break; }
                    //功勋
                    case "honor": { dic.Add("honor", role.role_honor); break; }
                    //等级
                    case "level": { dic.Add("level", role.role_level); break; }
                    //流派
                    case "genre": { dic.Add("genre", role.role_genre); break; }
                    //忍者众
                    case "ninja": { dic.Add("ninja", role.role_ninja); break; }
                    //统率
                    case "captain": { dic.Add("captain", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_CAPTAIN, role), 2)); break; }
                    //基础统率
                    case "captainbase": { dic.Add("captainBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_CAPTAIN, role), 2)); break; }
                    //武力
                    case "force": { dic.Add("force", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, role), 2)); break; }
                    //基础武力
                    case "forcebase": { dic.Add("forceBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_FORCE, role), 2)); break; }
                    //智谋
                    case "brains": { dic.Add("brains", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_BRAINS, role), 2)); break; }
                    //基础智谋
                    case "brainbase": { dic.Add("brainsBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_BRAINS, role), 2)); break; }
                    //魅力
                    case "charm": { dic.Add("charm", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, role), 2)); break; }
                    //基础魅力
                    case "charmbase": { dic.Add("charmBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_CHARM, role), 2)); break; }
                    //政务
                    case "govern": { dic.Add("govern", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, role), 2)); break; }
                    //基础政务
                    case "governbase": { dic.Add("governBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_GOVERN, role), 2)); break; }
                    //攻击
                    case "attack": { dic.Add("attack", role.att_attack); break; }
                    //会心几率
                    case "critprobability": { dic.Add("critProbability", Math.Round(role.att_crit_probability, 2)); break; }
                    //会心效果
                    case "critaddition": { dic.Add("critAddition", Math.Round(role.att_crit_addition, 2)); break; }
                    //闪避几率
                    case "dodgeprobability": { dic.Add("dodgeProbability", Math.Round(role.att_dodge_probability, 2)); break; }
                    //奥义触发秘技
                    case "mysteryprobability": { dic.Add("mysteryProbability", Math.Round(role.att_mystery_probability, 2)); break; }
                    //血量
                    case "life": { dic.Add("life", role.att_life); break; }
                    //状态
                    case "state": { dic.Add("state", role.role_state); break; }

                    case "characterarr": { dic.Add("characterArr",GetCharacters(role)); break; }

                    case "hezhanskillarrvo": { dic.Add("hezhanSkillArrVo", new Skill().GetSkillListByRid(role.id)); break; }

                    case "skillselarr": { dic.Add("skillSelArr", EntityToVo.BuildSkillSelArr(role)); break; }
                }
            }
            dic1.Add("data", new ASObject(dic));
#if DEBUG
            sw.Stop();
            TimeSpan timespan = sw.Elapsed;
            DisplayGlobal.log.Write(timespan.ToString());
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", timespan.ToString(), GetType().Namespace);
#endif
            return new ASObject(dic1);
        }

        /// <summary>组装特性集合</summary>
        public List<int> GetCharacters(tg_role model)
        {
            var list = new List<int>();
            if (model.character1 != 0)
                list.Add(model.character1);
            if (model.character2 != 0)
                list.Add(model.character2);
            if (model.character3 != 0)
                list.Add(model.character3);
            return list;
        }



        #region 测试重构
        public ASObject BuildUpdateRoleData(tg_role role, IEnumerable<RoleName> name)
        {
#if DEBUG
            var sw = new Stopwatch();
            sw.Start();
#endif
            var dic = new Dictionary<string, object> { { "id", role.id } };
            var dic1 = new Dictionary<string, object>();
            var dictest = new Dictionary<RoleName, Func<tg_role, double>>
            {
                {RoleName.power, GetPower},
                {RoleName.rolePower, GetRolePower},
                {RoleName.experience, GetRoleExperience},
                {RoleName.identityId, GetRoleIdentityid},
                {RoleName.honor, GetRoleHonor},
                {RoleName.level, GetRoleLevel},
                {RoleName.genre, GetRoleGenre},
                {RoleName.ninja, GetRoleNinja},
                {RoleName.captain, GetRoleCaptain},
                {RoleName.captainBase, GetRoleCaptainbase},
                {RoleName.force, GetRoleForce},
                {RoleName.forceBase, GetRoleForcebase},
                {RoleName.brains, GetRoleBrains},
                {RoleName.brainsBase, GetRoleBrainbase},
                {RoleName.charm, GetRoleCharm},
                {RoleName.charmBase, GetRoleCharmbase},
                {RoleName.govern, GetRoleGovern},
                {RoleName.governBase, GetRoleGovernbase},
                {RoleName.attack, GetRoleAttack},
                {RoleName.critProbability, GetRoleCritprobability},
                {RoleName.critAddition, GetRoleCritaddition},
                {RoleName.dodgeProbability, GetRoleDodgeprobability},
                {RoleName.mysteryProbability, GetRoleMysteryprobability},
                {RoleName.life, GetRoleLife},
                {RoleName.state, GetRoleState},
            };

            foreach (var item in name.Where(dictest.ContainsKey))
            {
                dic.Add(item.ToString(), dictest[item](role));
            }


            dic1.Add("data", new ASObject(dic));
#if DEBUG
            sw.Stop();
            TimeSpan timespan = sw.Elapsed;
            DisplayGlobal.log.Write(timespan.ToString());
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", timespan.ToString(), GetType().Namespace);
#endif
            return new ASObject(dic1);
        }


        public enum RoleName
        {
            power, rolePower, experience, identityId, honor, level, genre, ninja, captain, captainBase,
            force, forceBase, brains, brainsBase, charm, charmBase, govern, governBase, attack,
            critProbability, critAddition, dodgeProbability, mysteryProbability, life, state
        }

        #region 获取武将属性值

        private double GetPower(tg_role role)
        {
            return tg_role.GetTotalPower(role);
        }


        private double GetRolePower(tg_role role)
        {
            return role.power;
        }

        private double GetRoleExperience(tg_role role)
        {
            return role.role_exp;
        }

        private double GetRoleIdentityid(tg_role role)
        {
            return role.role_identity;
        }

        private double GetRoleHonor(tg_role role)
        {
            return role.role_honor;
        }

        private double GetRoleLevel(tg_role role)
        {
            return role.role_level;
        }

        private double GetRoleGenre(tg_role role)
        {
            return role.role_genre;
        }

        private double GetRoleNinja(tg_role role)
        {
            return role.role_ninja;
        }

        private double GetRoleCaptain(tg_role role)
        {
            return Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_CAPTAIN, role), 2);
        }

        private double GetRoleCaptainbase(tg_role role)
        {
            return Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_CAPTAIN, role), 2);
        }

        private double GetRoleForce(tg_role role)
        {
            return Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, role), 2);
        }

        private double GetRoleForcebase(tg_role role)
        {
            return Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_FORCE, role), 2);
        }

        private double GetRoleBrains(tg_role role)
        {
            return Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_BRAINS, role), 2);
        }

        private double GetRoleBrainbase(tg_role role)
        {
            return Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_BRAINS, role), 2);
        }

        private double GetRoleCharm(tg_role role)
        {
            return Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, role), 2);
        }

        private double GetRoleCharmbase(tg_role role)
        {
            return Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_CHARM, role), 2);
        }

        private double GetRoleGovern(tg_role role)
        {
            return Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, role), 2);
        }

        private double GetRoleGovernbase(tg_role role)
        {
            return Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_GOVERN, role), 2);
        }

        private double GetRoleAttack(tg_role role)
        {
            return role.att_attack;
        }

        private double GetRoleLife(tg_role role)
        {
            return role.att_life;
        }

        private double GetRoleCritprobability(tg_role role)
        {
            return Math.Round(role.att_crit_probability, 2);
        }
        private double GetRoleCritaddition(tg_role role)
        {
            return Math.Round(role.att_crit_addition, 2);
        }
        private double GetRoleDodgeprobability(tg_role role)
        {
            return Math.Round(role.att_dodge_probability, 2);
        }
        private double GetRoleMysteryprobability(tg_role role)
        {
            return Math.Round(role.att_mystery_probability, 2);
        }

        private double GetRoleState(tg_role role)
        {
            return role.role_state;
        }
        #endregion
        #endregion
    }
}
