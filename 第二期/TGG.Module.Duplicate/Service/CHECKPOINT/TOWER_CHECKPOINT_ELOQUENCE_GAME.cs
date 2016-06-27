using System;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using Convert = FluorineFx.Util.Convert;

namespace TGG.Module.Duplicate.Service.CHECKPOINT
{
    /// <summary>
    /// 辩才小游戏
    /// </summary>
    public class TOWER_CHECKPOINT_ELOQUENCE_GAME
    {
        private static TOWER_CHECKPOINT_ELOQUENCE_GAME ObjInstance;

        /// <summary>TOWER_CHECKPOINT_ELOQUENCE_GAME单体模式</summary>
        public static TOWER_CHECKPOINT_ELOQUENCE_GAME GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TOWER_CHECKPOINT_ELOQUENCE_GAME());
        }

        /// <summary>辩才小游戏</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "TOWER_CHECKPOINT_ELOQUENCE_GAME", "辩才小游戏");
#endif
            var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);
            var checkpoint = tg_duplicate_checkpoint.GetEntityByUserId(session.Player.User.id);
            if (checkpoint == null) return Error((int)ResultType.DATABASE_ERROR);
            if (!CheckType(type)) return Error((int)ResultType.FRONT_DATA_ERROR);

            if (checkpoint.user_blood == 0 || checkpoint.npc_blood == 0 ||
                checkpoint.state == (int)DuplicateClearanceType.CLEARANCE_FAIL)
            {
                return Error((int)ResultType.CHALLENGE_FAIL);
            }
            var npccard = CardRandom();

            var base_tower = Variable.BASE_TOWERELOQUENCE.FirstOrDefault(m => m.myCard == type && m.enemyCard == npccard);
            var base_pass = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == checkpoint.site);
            if (base_tower == null || base_pass == null)
                return Error((int)ResultType.BASE_TABLE_ERROR);

            checkpoint = GameProcess(checkpoint, base_tower);
            if (!tg_duplicate_checkpoint.UpdateSite(checkpoint))
                return Error((int)ResultType.DATABASE_ERROR);
            var user = session.Player.User.CloneEntity();
            if (!CheckResult(user,Convert.ToInt32(base_pass.passAward), ref checkpoint))
                TOWER_CHECKPOINT_DARE_PUSH.GetInstance().CommandStart(checkpoint.user_id, checkpoint.site, checkpoint.state);
            return new ASObject(Common.GetInstance().BulidData((int)ResultType.SUCCESS, npccard, checkpoint.user_blood, checkpoint.npc_blood));
        }

        /// <summary>验证前端传递过来的卡牌类型</summary>
        private bool CheckType(int type)
        {
            if (type == (int)DuplicateCardType.TONGUE_LOTUS || type == (int)DuplicateCardType.SOPHISTRY ||
                type == (int)DuplicateCardType.MOTIVATION)
            {
                return true;
            }
            return false;
        }

        /// <summary>NPC 随机获取一张卡牌</summary>
        private int CardRandom()
        {
            var ran = RNG.Next((int)DuplicateCardType.TONGUE_LOTUS, (int)DuplicateCardType.SOPHISTRY);
            return ran;
        }


        /// <summary>分数处理</summary>
        private tg_duplicate_checkpoint GameProcess(tg_duplicate_checkpoint checkpoint, BaseTowerEloquence base_tower)
        {
            checkpoint.user_blood = checkpoint.user_blood - base_tower.myScore;
            checkpoint.npc_blood = checkpoint.npc_blood - base_tower.enemyScore;
            return checkpoint;
        }

        /// <summary>验证分数结果</summary>
        private bool CheckResult(tg_user user,int passaward, ref tg_duplicate_checkpoint checkpoint)
        {
            if (checkpoint.user_blood == 0 || checkpoint.npc_blood == 0)
            {
                if (checkpoint.user_blood == 0)
                {
                    checkpoint.state = (int)DuplicateClearanceType.CLEARANCE_FAIL;
                    return false;
                }
                checkpoint.state = (int)DuplicateClearanceType.CLEARANCE_SUCCESS;
                var _fame = user.fame;
                user.fame = tg_user.IsFameMax(user.fame, passaward);
                user.Update();
                Common.GetInstance().RewardsToUser( user, (int)GoodsType.TYPE_FAME);
                Common.GetInstance().LogOperate(user.id, _fame, passaward,user.fame, (int)DuplicateCommand.TOWER_CHECKPOINT_ELOQUENCE_GAME);
                return false;
            }
            return true;
        }

        /// <summary>返回错误结果</summary>
        private ASObject Error(int result)
        {
            return new ASObject(Common.GetInstance().BulidData(result, 0, 0, 0));
        }
    }
}
