using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;
using TGG.Core.Base;

namespace TGG.Module.Skill.Service
{
    /// <summary>
    /// 战斗技能学习
    /// </summary>
    public class SKILL_FIGHT_STUDY
    {
        private static SKILL_FIGHT_STUDY _objInstance;

        /// <summary>SKILL_FIGHT_STUDY单体模式</summary>
        public static SKILL_FIGHT_STUDY GetInstance()
        {
            return _objInstance ?? (_objInstance = new SKILL_FIGHT_STUDY());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "SKILL_FIGHT_STUDY", "战斗技能学习");
#endif
                var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value.ToString());                   //技能基表id
                var roleId = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "roleId").Value.ToString());       //武将主键id

                if (tg_role_fight_skill.GetFightSkill(roleId, id)) return Result((int)ResultType.SKILL_SKILL_REPEAT);   //验证技能是否已经学习

                var baseskill = Variable.BASE_FIGHTSKILL.FirstOrDefault(m => m.id == id);  //技能基表数据
                if (baseskill == null) return Result((int)ResultType.BASE_TABLE_ERROR);         //验证基表数据信息

                if (tg_role_fight_skill.GetStudySkill(roleId, baseskill.genre, (int)SkillLearnType.STUDYING))
                    return Result((int)ResultType.SKILL_GENRE_HAS_LEARN);   //验证同一流派技能是否有正在学习升级的
                var role = tg_role.GetEntityById(roleId);

                if (role == null) return Result((int)ResultType.DATABASE_ERROR);
                if (!CheckSkill(baseskill.genre, role)) return Result((int)ResultType.SKILL_LEARN_OPEN_ERROR);       //验证技能是否开启学习

                return RoleSkillLearn(session, role, baseskill);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>武将技能学习</summary>
        private ASObject RoleSkillLearn(TGGSession session, tg_role role, BaseFightSkill skill)
        {
            if (!string.IsNullOrEmpty(skill.studyCondition)) //验证所需前置技能信息
            {
                var item = SkillData(skill.studyCondition);   //获取前置技能集合信息
                foreach (var id in item)
                {
                    if (Convert.ToInt32(id) == 0) continue;
                    var condition = tg_role_fight_skill.GetSkillByRidSkillId(role.id, Convert.ToInt32(id));
                    if (condition == null) return Result((int)ResultType.SKILL_CONDITION_LACK);  //验证前置技能
                    if (condition.skill_level < skill.conditionLevel) return Result((int)ResultType.SKILL_CONDITION_LEVEL_LACK); //验证前置技能等级
                }
            }
            if (role.role_level < skill.studyLevel) return Result((int)ResultType.BASE_ROLE_LEVEL_ERROR);   //验证武将等级

            var power = RuleConvert.GetCostPower();   //固定消耗体力 
            if (!Common.GetInstance().PowerOperate(role, power)) return Result((int)ResultType.BASE_ROLE_POWER_ERROR);     //验证体力

            return LearningSkill(session, skill, role, power);        //学习过程处理
        }

        /// <summary>学习技能过程处理</summary>
        public ASObject LearningSkill(TGGSession session, BaseFightSkill fight, tg_role role, int power)
        {
            var condition = Variable.BASE_FIGHTSKILLEFFECT.FirstOrDefault(m => m.skillid == fight.id && m.level == 1);
            if (condition == null) return Result((int)ResultType.BASE_TABLE_ERROR);
            var user = session.Player.User.CloneEntity();
            var coin = user.coin;
            user.coin = user.coin - condition.costCoin;
            if (user.coin < 0) return Result((int)ResultType.BASE_PLAYER_COIN_ERROR);

            var time = condition.costTimer * 60 * 1000;
            var tabskill = tg_role_fight_skill.GetTabSkill(role.id, fight.id, -1);
            if (tabskill == null)           //未有标记技能
            {
                if (!InsertSkill(role.id, fight, Common.GetInstance().Time(time))) return Result((int)ResultType.DATABASE_ERROR);//验证插入学习技能信息
            }
            else
            {
                tabskill.skill_level = 0;
                tabskill.skill_time = Common.GetInstance().Time(time);   //学习完成需要的时间
                tabskill.skill_state = (int)SkillLearnType.STUDYING;
                tabskill.Update();
            }

            user.Update();
            session.Player.User = user;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, user);   //推送用户资源消耗更新

            new Share.Role().PowerUpdateAndSend(role, power, role.user_id);    //推送体力更新  
            Common.GetInstance().SkillLearnOk(user.id, role.id, fight.id, time);      //开启学习技能线程

            //日志记录金钱消费
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Coin", coin, condition.costCoin, user.coin);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.SKILL, (int)SkillCommand.SKILL_FIGHT_STUDY, logdata);

            (new Share.DaMing()).CheckDaMing(user.id, (int)DaMingType.技能);  //检测大名令技能完成度
            (new Share.TGTask()).MainTaskUpdate(TaskStepType.STUDY_SKILL, user.id, 0, 0); //任务验证
            var rolevo = (new Share.Role()).BuildRole(role.id);
            return new ASObject(Common.GetInstance().DataBuild((int)ResultType.SUCCESS, rolevo));
        }

        /// <summary>插入战斗技能</summary>
        private bool InsertSkill(Int64 rid, BaseFightSkill fight, Int64 time)
        {
            var skill = new tg_role_fight_skill
            {
                rid = rid,
                skill_id = fight.id,
                skill_type = fight.type,
                skill_genre = fight.genre,
                type_sub = fight.typeSub,
                skill_state = (int)SkillLearnType.STUDYING,
                skill_level = 0,
                skill_time = time,
            };
#if DEBUG
            XTrace.WriteLine("{0}:   {1}   {2}:   {3}", "当前技能的等级", skill.skill_level, "技能的学习状态 0：正在学习；1：学习完成", skill.skill_state);
#endif
            try
            {
                skill.Insert(); return true;
            }
            catch { return false; }
        }

        /// <summary>获取前置技能id集合</summary>
        private IEnumerable<string> SkillData(string skills)
        {
            var item = new List<string>();
            if (skills.Contains('|'))
            {
                item = skills.Split('|').ToList();
            }
            else
            { item.Add(skills); }
            return item;
        }

        /// <summary>验证战斗技能是否可以学习</summary>
        private bool CheckSkill(int school, tg_role role)
        {
            var skills = tg_role_fight_skill.GetRoleSkillByRid(role.id);
            if (skills == null) return false;
            var genre = (new Share.Role()).LearnGenreOrNinja(skills, role.role_genre, role.role_ninja);
            return genre.Contains(school);
        }

        private ASObject Result(int result)
        {
            return new ASObject(Common.GetInstance().DataBuild(result, null));
        }
    }
}