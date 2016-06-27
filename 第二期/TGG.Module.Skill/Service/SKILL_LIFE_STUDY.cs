using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Skill.Service
{
    /// <summary>
    /// 生活技能学习
    /// </summary>
    public class SKILL_LIFE_STUDY
    {
        private static SKILL_LIFE_STUDY ObjInstance;

        /// <summary> SKILL_LIFE_STUDY单体模式 </summary>
        public static SKILL_LIFE_STUDY GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new SKILL_LIFE_STUDY());
        }
        private int result;
        /// <summary> 生活技能学习</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "SKILL_LIFE_STUDY", "生活技能学习");
#endif
            var baseid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "baseId").Value);
            var rid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);
            var user = session.Player.User.CloneEntity();
            var baselife = Variable.BASE_LIFESKILL.FirstOrDefault(m => m.id == baseid);
            if (baselife == null) return ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            var role = tg_role.GetEntityById(rid);
            var _role = role.CloneEntity();
            var life = tg_role_life_skill.GetEntityByRid(role.id);
            if (life == null) life = new tg_role_life_skill();

            var skilllt = Common.GetInstance().GetSkillLevel(baselife.type, life);
            //读取基表数据
            var effect = Variable.BASE_LIFESKILLEFFECT.FirstOrDefault(m => m.skillid == baseid && m.level == skilllt.level);
            if (effect == null) return ErrorResult((int)ResultType.BASE_TABLE_ERROR);
            var _effect = Variable.BASE_LIFESKILLEFFECT.FirstOrDefault(m => m.id == effect.nextId);
            if (_effect == null) return ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            if (!Check(baselife, _effect.costCoin, session.Player.User.coin, skilllt.level, role)) return ErrorResult(result);
            var list_lifeskillid = Common.GetInstance().SkillStudied(life);
            //验证是否已学前置技能
            if (!Common.GetInstance().IsStudied(baselife, baselife.studyCondition, list_lifeskillid, life))
                return ErrorResult((int)ResultType.SKILL_CONDITION_LACK);

            var study = Common.GetInstance().SkillStudying(life);
            if (study > 0) return ErrorResult((int)ResultType.SKILL_GENRE_HAS_LEARN);
            //体力判断
            if (!PowerOperate(role, session)) return ErrorResult((int)ResultType.BASE_ROLE_POWER_ERROR);

            (new Share.Skill()).PowerLog(_role, (int)ModuleNumber.SKILL, (int)SkillCommand.SKILL_LIFE_STUDY);
            if (life.id != 0) return GetRoleLifeSkill(user, baseid, role.id, life, _effect, false);
            life.rid = rid;
            return GetRoleLifeSkill(user, baseid, role.id, life, _effect, true);
        }

        /// <summary>体力操作</summary>
        private bool PowerOperate(tg_role role, TGGSession session)
        {
            var userid = session.Player.User.id;
            var power = RuleConvert.GetCostPower();
            var totalpower = tg_role.GetTotalPower(role);
            if (totalpower < power) return false;
            new Share.Role().PowerUpdateAndSend(role, power, userid);
            return true;
        }

        /// <summary>验证基表数据，武将等级，已学技能，武将体力,玩家金钱 ，技能等级</summary>
        private bool Check(BaseLifeSkill base_life, Int32 cost_coin, Int64 coin, int level, tg_role role)
        {
            if (role.role_level < base_life.studyLevel) //武将等级是否大于学习等级
            {
                result = (int)ResultType.BASE_ROLE_LEVEL_ERROR;
                return false;
            }
            if (!Consume(coin, cost_coin)) //验证金钱
            {
                result = (int)ResultType.BASE_PLAYER_COIN_ERROR;
                return false;
            }
            if (level >= base_life.levelLimit) //技能等级是否达到最大级
            {
                result = (int)ResultType.SKILL_LEVEL_FALL;
                return false;
            }
            return true;
        }

        /// <summary>生活技能处理</summary>
        public ASObject GetRoleLifeSkill(tg_user user, int baseid, Int64 rid, tg_role_life_skill life, BaseLifeSkillEffect effect, bool type)
        {
# if DEBUG
            effect.costTimer = 1;//测试
#endif
            life = LifeSkillPractice(life, baseid, effect.costTimer * 60 * 1000);
            CreateLog(user.id, user.coin, effect.costCoin);
            user.coin = user.coin - effect.costCoin;
            if (!tg_user.GetUserUpdate(user))
                return ErrorResult((int)ResultType.DATABASE_ERROR);
            if (!type)
            {
                if (!tg_role_life_skill.GetUpdate(life))
                    return ErrorResult((int)ResultType.DATABASE_ERROR);
            }
            else
            {
                if (!tg_role_life_skill.GetInsert(life))
                    return ErrorResult((int)ResultType.DATABASE_ERROR);
            }
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, user);
            Common.GetInstance().ThreadingUpgrade(user.id, rid, effect.costTimer * 60 * 1000);

            if (!Variable.OnlinePlayer.ContainsKey(user.id))
                return ErrorResult((int)ResultType.BASE_PLAYER_OFFLINE_ERROR);
            var session = Variable.OnlinePlayer[user.id] as TGGSession;
            session.Player.User = user;

            (new Share.DaMing()).CheckDaMing(user.id, (int)DaMingType.技能);
            (new Share.TGTask()).MainTaskUpdate(TaskStepType.STUDY_SKILL, user.id, 0, 0); //任务验证
            return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS, rid));
        }

        private ASObject ErrorResult(int error)
        {
            return new ASObject(Common.GetInstance().BuilData(error, 0));
        }

        /// <summary>
        /// 判断用户资源是否足够
        /// </summary>
        /// <param name="resource">用户资源</param>
        /// <param name="cost">消耗资源</param>
        private bool Consume(Int64 coin, Int32 cost_coin)
        {
            return coin - cost_coin >= 0;
        }

        /// <summary>更新生活技能值</summary>
        private tg_role_life_skill LifeSkillPractice(tg_role_life_skill life, int baseid, int costtime)
        {
            var currenttime = Common.GetInstance().CurrentTime();
            var time = costtime + currenttime;
            var type = (int)SkillLearnType.STUDYING;
            var temp = Variable.BASE_LIFESKILL.FirstOrDefault(m => m.id == baseid);
            var t = temp != null ? temp.type : 0;
            switch (t)
            {
                #region
                case (int)LifeSkillType.ASHIGARU: life.sub_ashigaru_time = time; life.sub_ashigaru_state = type; break;
                case (int)LifeSkillType.ARTILLERY: life.sub_artillery_time = time; life.sub_artillery_state = type; break;
                case (int)LifeSkillType.ARCHER: life.sub_archer_time = time; life.sub_archer_state = type; break;
                case (int)LifeSkillType.BUILD: life.sub_build_time = time; life.sub_build_state = type; break;
                case (int)LifeSkillType.CALCULATE: life.sub_calculate_time = time; life.sub_calculate_state = type; break;
                case (int)LifeSkillType.CRAFT: life.sub_craft_time = time; life.sub_craft_state = type; break;
                case (int)LifeSkillType.ELOQUENCE: life.sub_eloquence_time = time; life.sub_eloquence_state = type; break;
                case (int)LifeSkillType.EQUESTRIAN: life.sub_equestrian_time = time; life.sub_equestrian_state = type; break;
                case (int)LifeSkillType.ETIQUETTE: life.sub_etiquette_time = time; life.sub_etiquette_state = type; break;
                case (int)LifeSkillType.MARTIAL: life.sub_martial_time = time; life.sub_martial_state = type; break;
                case (int)LifeSkillType.MEDICAL: life.sub_medical_time = time; life.sub_medical_state = type; break;
                case (int)LifeSkillType.MINE: life.sub_mine_time = time; life.sub_mine_state = type; break;
                case (int)LifeSkillType.NINJITSU: life.sub_ninjitsu_time = time; life.sub_ninjitsu_state = type; break;
                case (int)LifeSkillType.RECLAIMED: life.sub_reclaimed_time = time; life.sub_reclaimed_state = type; break;
                case (int)LifeSkillType.TACTICAL: life.sub_tactical_time = time; life.sub_tactical_state = type; break;
                case (int)LifeSkillType.TEA: life.sub_tea_time = time; life.sub_tea_state = type; break;
                #endregion
            }
            return life;
        }

        /// <summary>消耗金钱日志</summary>
        private void CreateLog(Int64 user_id, Int64 coin, int cost)
        {
            var _coin = coin;
            coin -= cost;
            //日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Coin", _coin, cost, coin);
            (new Share.Log()).WriteLog(user_id, (int)LogType.Use, (int)ModuleNumber.SKILL, (int)SkillCommand.SKILL_LIFE_STUDY, logdata);
        }
    }
}
