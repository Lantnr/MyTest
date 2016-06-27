using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
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
    /// 生活技能加速
    /// </summary>
    public class SKILL_LIFE_ACCELERATE
    {
        private static SKILL_LIFE_ACCELERATE ObjInstance;

        /// <summary> SKILL_LIFE_ACCELERATE单体模式 </summary>
        public static SKILL_LIFE_ACCELERATE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new SKILL_LIFE_ACCELERATE());
        }

        /// <summary> 生活技能加速</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "SKILL_LIFE_ACCELERATE", "生活技能加速");
#endif
            var id = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);
            var baseid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "baseId").Value);
            if (id == 0 || baseid == 0) return ErrorResult((int)ResultType.FRONT_DATA_ERROR);

            var role = tg_role.GetEntityById(id);
            var life = tg_role_life_skill.GetEntityByRid(id);
            if (role == null || life == null) return ErrorResult((int)ResultType.DATABASE_ERROR);
            var user = session.Player.User.CloneEntity();

            var baselife = Variable.BASE_LIFESKILL.FirstOrDefault(m => m.id == baseid);
            if (baselife == null) return ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            var skilllt = Common.GetInstance().GetSkillLevel(baselife.type, life);
            if (skilllt.time <= Common.GetInstance().CurrentTime()) return ErrorResult((int)ResultType.SKILL_SKILL_REPEAT);//验证学习时间是否小于当前时间

            var cost = Common.GetInstance().Consume(skilllt.time);
            if (user.gold < cost) return ErrorResult((int)ResultType.BASE_PLAYER_GOLD_ERROR);//验证消耗元宝是否足够
            var gold = user.gold;
            user.gold = user.gold - cost;
            //日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "GOLD", gold, cost, user.gold);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.SKILL, (int)SkillCommand.SKILL_LIFE_ACCELERATE, logdata);

            var key = string.Format("{0}_{1}_{2}", (int)CDType.LifeSkill, user.id, id);//为打断线程加入全局变量cd
            Variable.CD.AddOrUpdate(key, true, (k, v) => true);

            if(!tg_user.GetUserUpdate(user))
                return ErrorResult((int)ResultType.DATABASE_ERROR);
            session.Player.User = user;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, user);
            log.GoldInsertLog(cost, session.Player.User.id, (int)ModuleNumber.SKILL, (int)SkillCommand.SKILL_LIFE_ACCELERATE);  //玩家消费金币记录
            return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS, id));
        }

        private ASObject ErrorResult(int error)
        {
            return new ASObject(Common.GetInstance().BuilData(error, 0));
        }
    }
}
