using System;
using System.Collections.Generic;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// tg_role_fight_skill 业务逻辑类
    /// </summary>
    public partial class tg_role_fight_skill
    {
        /// <summary>根据武将主键id查询武将所有已学习技能</summary>
        public static List<tg_role_fight_skill> GetRoleSkillByRid(Int64 rid)
        {
            return FindAll(new String[] { _.rid }, new Object[] { rid });
        }

        /// <summary>根据技能主键id获取实体</summary>
        public static tg_role_fight_skill GetBySkillId(Int64 skillid)
        {
            var exp = new WhereExpression();
            if (skillid > 0) exp &= _.id == skillid;
            return Find(exp);
        }

        /// <summary>根据武将主键id，技能id查询技能信息</summary>
        public static tg_role_fight_skill GetSkillByRidSkillId(Int64 rid, int skillid)
        {
            return Find(new String[] { _.rid, _.skill_id }, new Object[] { rid, skillid });
        }

        /// <summary>根据武将主键id，技能id查询技能是否已经学习</summary>
        public static bool GetFightSkill(Int64 rid, int skillid)
        {
            return FindCount(string.Format("rid={0} and skill_id={1} and skill_level>=0", rid, skillid), null, null, 0, 0) > 0;
        }

        /// <summary>根据武将主键id，技能id查询标记流派开启的技能信息</summary>
        public static tg_role_fight_skill GetTabSkill(Int64 rid, int skillid, int level)
        {
            return Find(new String[] { _.rid, _.skill_id, _.skill_level }, new Object[] { rid, skillid, level });
        }

        /// <summary>根据武将主键id，流派查询同一流派是否有其他正在学习的技能</summary>
        public static bool GetStudySkill(Int64 rid, int genre, int state)
        {
            return FindCount(new String[] { _.rid, _.skill_genre, _.skill_state }, new Object[] { rid, genre, state }) > 0;
        }

        /// <summary>根据武将主键rid，技能id更新正在学习的战斗技能信息</summary>
        public static void UpdateSkill(Int64 rid, int skillid, int state, int level)
        {
            Update(new String[] { _.skill_state, _.skill_time, _.skill_level }, new Object[] { state, 0, level }, new String[] { _.rid, _.skill_id }, new Object[] { rid, skillid });
        }

        /// <summary>正在学习的技能</summary>
        public static List<tg_role_fight_skill> GetLearnEntity(int state, List<Int64> rids)
        {
            var ids = string.Join(",", rids);
            return FindAll(string.Format("skill_state={0} and rid in ({1})", state, ids), null, null, 0, 0);
        }

        /// <summary>根据时间操作tg_role_fight_skill</summary>
        public static List<tg_role_fight_skill> GetEntityListByTime(Int64 time, List<Int64> rids)
        {
            var ids = string.Join(",", rids);
            return FindAll(string.Format("skill_state=3 and skill_time<={0} and rid in ({1})", time, ids), null, null, 0, 0);
        }

        /// <summary>根据武将rids集合查询武将战斗技能信息</summary>
        public static List<tg_role_fight_skill> GetRoleFightSkills(List<Int64> rids)
        {
            var ids = string.Join(",", rids);
            return FindAll(string.Format("rid in({0})", ids), null, null, 0, 0);
        }

        /// <summary>更新战斗技能信息</summary>
        public static void UpdateFightSkills(IEnumerable<tg_role_fight_skill> listskills)
        {
            var skills = new EntityList<tg_role_fight_skill>();
            skills.AddRange(listskills);
            skills.Update();
        }
    }
}
