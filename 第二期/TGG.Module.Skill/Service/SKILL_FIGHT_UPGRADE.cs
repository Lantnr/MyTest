using System;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.Skill.Service
{
    /// <summary>
    /// 战斗技能升级
    /// </summary>
    public class SKILL_FIGHT_UPGRADE
    {
        private static SKILL_FIGHT_UPGRADE _objInstance;

        /// <summary>SKILL_FIGHT_UPGRADE单体模式</summary>
        public static SKILL_FIGHT_UPGRADE GetInstance()
        {
            return _objInstance ?? (_objInstance = new SKILL_FIGHT_UPGRADE());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_COIN, session, data);
        }

        public ASObject CommandStart(int goodType, TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "SKILL_FIGHT_UPGRADE", "战斗技能升级");
#endif
                var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value.ToString());  //战斗技能主键Id

                var skill = tg_role_fight_skill.GetBySkillId(id);
                if (skill == null) return Result((int)ResultType.DATABASE_ERROR);    //验证技能数据
                var role = tg_role.GetEntityById(skill.rid);
                if (role == null) return Result((int)ResultType.DATABASE_ERROR);    //验证武将数据
                if (skill.skill_state == (int)SkillLearnType.STUDYING) return Result((int)ResultType.FRONT_DATA_ERROR);  //验证技能状态信息

                if (tg_role_fight_skill.GetStudySkill(skill.rid, skill.skill_genre, (int)SkillLearnType.STUDYING))     //验证是否有其他正在学习或升级的技能
                    return Result((int)ResultType.SKILL_GENRE_HAS_LEARN);

                var baseskill = Variable.BASE_FIGHTSKILL.FirstOrDefault(m => m.id == skill.skill_id);        //技能基表数据
                if (baseskill == null) return Result((int)ResultType.BASE_TABLE_ERROR);
                if (skill.skill_level + 1 > baseskill.levelLimit) return Result((int)ResultType.SKILL_LEVEL_FALL);   //技能等级达到上限

                var nextskilleffect = Variable.BASE_FIGHTSKILLEFFECT.FirstOrDefault(m => m.skillid == skill.skill_id && m.level == skill.skill_level + 1);
                if (nextskilleffect == null) return Result((int)ResultType.BASE_TABLE_ERROR);  //下一等级战斗技能效果基表信息

                var power = RuleConvert.GetCostPower();   //固定消耗体力 
                if (!Common.GetInstance().PowerOperate(role, power)) return Result((int)ResultType.BASE_ROLE_POWER_ERROR);  //验证武将体力信息

                return UpgradeSkill(session, nextskilleffect, skill, role, power);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>技能升级过程处理</summary>
        public ASObject UpgradeSkill(TGGSession session, BaseFightSkillEffect nextskilleffect, tg_role_fight_skill skill, tg_role role, int power)
        {
            var user = session.Player.User.CloneEntity();
            var coin = user.coin;
            user.coin = user.coin - nextskilleffect.costCoin;
            if (user.coin < 0) return Result((int)ResultType.BASE_PLAYER_COIN_ERROR);   //验证用户金钱信息
            user.Update();
            session.Player.User = user;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, user);  //向用户推送更新

            var time = nextskilleffect.costTimer * 60 * 1000;
            skill.skill_state = (int)SkillLearnType.STUDYING;
            skill.skill_time = Common.GetInstance().Time(time);          //升级到达时间
            skill.Update();

            new Share.Role().PowerUpdateAndSend(role, power, user.id);    //推送体力更新
            var rolevo = (new Share.Role()).BuildRole(skill.rid);
            Common.GetInstance().SkillUpgrade(user.id, skill, time);                //开启升级线程

            //日志记录金钱消费
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Coin", coin, nextskilleffect.costCoin, user.coin);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.SKILL, (int)SkillCommand.SKILL_FIGHT_UPGRADE, logdata);


            (new Share.DaMing()).CheckDaMing(user.id, (int)DaMingType.技能);
            return new ASObject(Common.GetInstance().DataBuild((int)ResultType.SUCCESS, rolevo));
        }

        private ASObject Result(int result)
        {
            return new ASObject(Common.GetInstance().DataBuild(result, null));
        }

    }
}
