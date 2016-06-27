using System;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.Role.Service
{
    /// <summary>
    /// 秘技/奥义选择
    /// </summary>
    public class ROLE_SKILL_SELECT
    {
        private static ROLE_SKILL_SELECT _objInstance;

        /// <summary>ROLE_SKILL_SELECT单体模式</summary>
        public static ROLE_SKILL_SELECT GetInstance()
        {
            return _objInstance ?? (_objInstance = new ROLE_SKILL_SELECT());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "ROLE_SKILL_SELECT", "秘技/奥义选择");
#endif
                var sid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "skillId").Value.ToString());

                var skill = tg_role_fight_skill.GetBySkillId(sid);
                if (skill == null) return Error((int)ResultType.DATABASE_ERROR);   //验证技能信息

                if (skill.skill_state == (int)SkillLearnType.STUDYING && skill.skill_level <= 0)
                    return Error((int)ResultType.ROLE_SKILL_LEARNING);//验证技能是否已经学习
                if (skill.type_sub == (int)FightSkillType.BATTLES)
                    return Error((int)ResultType.ROLE_SELECT_SKILL_ERROR);//验证不能配置合战技能

                var baseskill = Variable.BASE_FIGHTSKILL.FirstOrDefault(m => m.id == skill.skill_id);
                if (baseskill == null)
                    return Error((int)ResultType.BASE_TABLE_ERROR);   //验证技能基表信息
                if (baseskill.configuration == 0)
                    return Error((int)ResultType.ROLE_SKILL_CONFIGURATION_ERROR);  //被动技能不能配置

                var role = tg_role.GetEntityById(skill.rid);
                if (role == null) return Error((int)ResultType.DATABASE_ERROR);   //验证武将信息

                if (skill.skill_type == (int)RolePersonalSkillType.MYSTERY) //验证技能类型
                {
                    role.art_mystery = skill.id;   //奥义
                }
                else
                {
                    role.art_cheat_code = skill.id;   //秘技  忍术
                }

                if (!tg_role.UpdateByRole(role)) return Error((int)ResultType.DATABASE_ERROR);
                if (session.Player.Role.Kind.id == skill.rid) session.Player.Role.Kind = role;

                var rolevo = (new Share.Role()).BuildRole(role.id);
                return new ASObject(Common.GetInstance().RoleLoadData((int)ResultType.SUCCESS, rolevo));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>返回数据</summary>
        private ASObject Error(int result)
        {
            return new ASObject(Common.GetInstance().RoleLoadData(result, null));
        }
    }
}

