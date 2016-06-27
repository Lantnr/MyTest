using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Duplicate.Service.CHECKPOINT
{
    /// <summary>
    /// 进入辩才小游戏
    /// </summary>
    public class TOWER_CHECKPOINT_ELOQUENCE_GAME_ENTER
    {
        private static TOWER_CHECKPOINT_ELOQUENCE_GAME_ENTER ObjInstance;

        /// <summary>TOWER_CHECKPOINT_ELOQUENCE_GAME_ENTER单体模式</summary>
        public static TOWER_CHECKPOINT_ELOQUENCE_GAME_ENTER GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TOWER_CHECKPOINT_ELOQUENCE_GAME_ENTER());
        }

        /// <summary>进入辩才小游戏</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "TOWER_CHECKPOINT_ELOQUENCE_GAME_ENTER", "进入辩才小游戏");
#endif
            var checkpoint = tg_duplicate_checkpoint.GetEntityByUserId(session.Player.User.id);
            if (checkpoint == null) return Error((int)ResultType.DATABASE_ERROR);

            if (checkpoint.blood <= 0) 
                return new ASObject(Common.GetInstance().BuildData((int)ResultType.TOWER_BOOLD_UNENOUGH));    //验证玩家临时血量

            var sgame = Variable.BASE_TOWERGAME.FirstOrDefault(m => m.pass == checkpoint.site);
            if (sgame == null || sgame.type != (int)SamllGameType.ELOQUENCE) 
                return new ASObject(Common.GetInstance().BuildData((int)ResultType.TOWER_SITE_ERROR));  //验证关卡是否为辩才游戏

            if (checkpoint.state == (int)DuplicateClearanceType.CLEARANCE_FAIL || checkpoint.state == (int)DuplicateClearanceType.CLEARANCE_SUCCESS)
                return Error((int)ResultType.TOWER_NO_OPEN);

            if (checkpoint.state == (int)DuplicateClearanceType.CLEARANCE_UNBEGIN)
            {
                var towergame = Variable.BASE_TOWERGAME.FirstOrDefault(m => m.pass == checkpoint.site);
                if (towergame == null) return Error((int)ResultType.BASE_TABLE_ERROR);
                checkpoint = CheckpointUpdate(checkpoint, towergame, session.Player.Role.LifeSkill.sub_eloquence_level);
                checkpoint.state = (int)DuplicateClearanceType.CLEARANCE_FIGHTING;
                if (!tg_duplicate_checkpoint.UpdateSite(checkpoint)) return Error((int)ResultType.DATABASE_ERROR);
            }
            return new ASObject(Common.GetInstance().BulidData((int)ResultType.SUCCESS, checkpoint.user_blood, checkpoint.npc_blood));
        }

        /// <summary>获取气血值</summary>
        private tg_duplicate_checkpoint CheckpointUpdate(tg_duplicate_checkpoint checkpoint, BaseTowerGame towergame, int level)
        {
            checkpoint.user_blood = towergame.myHp;
            checkpoint.npc_blood = towergame.enemyHp;

            var blood = level - 3;//3:辩才技能3级有额外气血值
            if (blood > 0)
            {
                checkpoint.user_blood += blood;
            }

            if (checkpoint.user_blood > 10) //10:最高气血值
            {
                checkpoint.user_blood = 10;
            }
            return checkpoint;
        }

        /// <summary>返回错误结果</summary>
        private ASObject Error(int result)
        {
            return new ASObject(Common.GetInstance().BulidData(result, 0, 0));
        }

    }
}
