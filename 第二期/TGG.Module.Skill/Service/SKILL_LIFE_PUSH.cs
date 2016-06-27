using System;
using FluorineFx;
using NewLife.Log;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Base;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Skill.Service
{
    /// <summary>
    /// 生活技能学习推送
    /// </summary>
    public class SKILL_LIFE_PUSH
    {
        private static SKILL_LIFE_PUSH ObjInstance;

        /// <summary> SKILL_LIFE_PUSH单体模式 </summary>
        public static SKILL_LIFE_PUSH GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new SKILL_LIFE_PUSH());
        }

        /// <summary> 生活技能学习推送</summary>
        public void CommandStart(TGGSession session, RoleItem role)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "SKILL_LIFE_PUSH", "生活技能学习完成 ");
#endif
            LifePush(session.Player.User.id, role.Kind.id);
# if DEBUG
            XTrace.WriteLine("{0}", "生活技能学习完成 ");
#endif
        }
       
        public void LifePush(Int64 user_id, Int64 rid)
        {
            //XTrace.WriteLine("-------------- Start -------------");
            var role = tg_role.GetEntityById(rid);
            if (role == null) return;
            var life = tg_role_life_skill.GetEntityByRid(role.id);
            var _bl = Common.GetInstance().SkillStudying(life);
            var base_life = Variable.BASE_LIFESKILL.FirstOrDefault(m => m.id == _bl);
            if (base_life == null) return;
            var skilllt = Common.GetInstance().GetSkillLevel(base_life.type, life);
            var base_effect = Variable.BASE_LIFESKILLEFFECT.FirstOrDefault(m => m.level == skilllt.level && m.skillid == _bl);
            var next_effect = Variable.BASE_LIFESKILLEFFECT.FirstOrDefault(m => base_effect != null && m.id == base_effect.nextId);
            if (next_effect != null) life = Common.GetInstance().GetRoleLifeSkill(base_life.type, life, next_effect.level);


            //后置技能id集合
            var ids = Common.GetInstance().SkillBeforePracticeSplit(base_life.studypostposition);
            ids = Common.GetInstance().GetNoShoolIds(ids, life);
            if (ids.Count > 0)
            {
                var st = Common.GetInstance().GetSkillLevel(base_life.type, life);
                life = Common.GetInstance().SkillStateChange(ids, life, st.level);
            }

            var n_effect = Common.GetInstance().SkillEffectSplit(next_effect);
            role = Common.GetInstance().SkillEffectIncrease(role, n_effect);

            tg_role_life_skill.GetUpdate(life);
            tg_role.GetRoleUpdate(role);

            if (!Variable.OnlinePlayer.ContainsKey(user_id)) return;
            //向在线玩家推送数据
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return;
            if (role.id == session.Player.Role.Kind.id)
            {
                session.Player.Role.Kind = role;
                session.Player.Role.LifeSkill = life;
            }
            Common.GetInstance().SkillChange(session.Player.User.id);
            var aso = BuildData(role.id);
            LifeSkillStudyPush(session, aso);
            //XTrace.WriteLine("-------------- End -------------");
        }

        /// <summary>组装数据</summary>
        private ASObject BuildData(Int64 rid)
        {
            var dic = new Dictionary<string, object> { { "role", Common.GetInstance().RoleInfo(rid) } };
            return new ASObject(dic);
        }

        /// <summary>发送锻炼结束协议</summary>
        private static void LifeSkillStudyPush(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "SKILL_LIFE_PUSH", "生活技能结束协议发送");
#endif
            var pv = session.InitProtocol((int)ModuleNumber.SKILL, (int)SkillCommand.SKILL_LIFE_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }
    }
}

