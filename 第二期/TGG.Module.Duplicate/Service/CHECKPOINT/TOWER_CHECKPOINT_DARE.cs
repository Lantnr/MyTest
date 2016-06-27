using System;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;
using TGG.SocketServer;
using Convert = FluorineFx.Util.Convert;

namespace TGG.Module.Duplicate.Service.CHECKPOINT
{
    /// <summary>
    /// 挑战怪物
    /// </summary>
    public class TOWER_CHECKPOINT_DARE
    {
        private static TOWER_CHECKPOINT_DARE ObjInstance;

        /// <summary>TOWER_CHECKPOINT_DARE单体模式</summary>
        public static TOWER_CHECKPOINT_DARE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TOWER_CHECKPOINT_DARE());
        }
        private int result;
        /// <summary>挑战怪物</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "TOWER_CHECKPOINT_DARE", "挑战怪物");
#endif
            var npcid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "npcid").Value);
            var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);
            var user = session.Player.User.CloneEntity();
            var checkpoint = tg_duplicate_checkpoint.GetEntityByUserId(session.Player.User.id);
            result = 0; int fightresult = 0;
            if (npcid == 0||checkpoint == null) return Error((int)ResultType.DATABASE_ERROR);
            var towerpass = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == checkpoint.site);
            if (towerpass == null) return Error((int)ResultType.BASE_TABLE_ERROR);
            if (!CheckSiteResult(checkpoint, towerpass)) return Error(result);
               
            Int64 rolelife;  //主角血量
            if (type == (int)DuplicateTargetType.MONSTER)
            {
                rolelife = checkpoint.blood;
                if (!CheckDifficult(npcid, ref checkpoint))
                    return Error((int)ResultType.BASE_TABLE_ERROR);
                checkpoint.Update();
                fightresult = NpcChallenge(session.Player.User.id, npcid,FightType.DUPLICATE_SHARP, ref rolelife);
                result = RoleLifeProcess(rolelife);
            }
            else if (type == (int)DuplicateTargetType.WATCHMEM)
            {
                if (!CheckWatchMen(checkpoint, towerpass)) return Error(result);                    
                rolelife = session.Player.Role.Kind.att_life;
                fightresult = NpcChallenge(session.Player.User.id, npcid,FightType.ONE_SIDE, ref rolelife);
                result = WatchResult(fightresult);
            }
            else
                return Error((int)ResultType.FRONT_DATA_ERROR);

            if (fightresult < 0) return Error((int)ResultType.FIGHT_ERROR);           
            TOWER_CHECKPOINT_DARE_PUSH.GetInstance().CommandStart(checkpoint.user_id,type, result, rolelife);
            if (!FameReward(user, npcid, result, type)) //推送奖励
                return Error((int)ResultType.BASE_TABLE_ERROR);
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS));
        }

        private int WatchResult(int fightresult)
        {
            if (fightresult == (int)FightResultType.WIN)
                return (int)DuplicateClearanceType.CLEARANCE_SUCCESS;
            return (int)DuplicateClearanceType.CLEARANCE_FAIL;
        }

        /// <summary>验证是否可以挑战改关卡</summary>
        private bool CheckSiteResult(tg_duplicate_checkpoint checkpoint, BaseTowerPass pass)
        {           
            if (pass.enemyType != (int)DuplicateEnemyType.FIGHTING)
            {
                result = (int)ResultType.TOWER_NPC_NORIGHT;
                return false;
            }
            if (checkpoint.state == (int)DuplicateClearanceType.CLEARANCE_FAIL || checkpoint.blood <= 0)
            {
                result = (int)ResultType.CHALLENGE_FAIL;
                return false;
            }
            return true;
        }

        /// <summary>验证挑战守护者的权利</summary>
        private bool CheckWatchMen(tg_duplicate_checkpoint checkpoint, BaseTowerPass towerpass)
        {
            if (towerpass.watchmen != (int)DuplicateTowerType.TOWER)//验证是否达到守护者关卡
            {
                result = (int)ResultType.TOWER_WATCHMEN_NO_OPEN;
                return false;
            }
            if (checkpoint.dekaron != (int)DuplicateRightType.HAVERIGHT && checkpoint.state != (int)DuplicateClearanceType.CLEARANCE_SUCCESS) //验证是否有权挑战守护者
            {
                result = (int)ResultType.TOWER_WATCHMEN_FIGHT_NORIGHT;
                return false;
            }
            if (checkpoint.site == checkpoint.tower_site)
            {
                result = (int)ResultType.TOWER_NOFIGHT_MYSELF;
                return false;
            }
            return true;
        }

        /// <summary>验证挑战怪物难度</summary>
        private bool CheckDifficult(int npcid, ref tg_duplicate_checkpoint checkpoint)
        {
            var towerenemy = Variable.BASE_TOWERENEMY.FirstOrDefault(m => m.id == npcid);
            if (towerenemy == null) return false;
            if (towerenemy.difficulty == (int)DuplicateDifficultyType.CENTER ||
                towerenemy.difficulty == (int)DuplicateDifficultyType.LOW)
                checkpoint.dekaron = (int)DuplicateRightType.NORIGHT;
            return true;
        }


        /// <summary> 进入战斗 </summary>
        /// <param name="userid"></param>
        /// <param name="npcid">rivalid 包括npc</param>
        /// <param name="rolelife"></param>
        private int NpcChallenge(Int64 userid, int npcid, FightType type, ref Int64 rolelife)
        {
            var fight = new Share.Fight.Fight().GeFight(userid, npcid, type, rolelife, false, true);
            new Share.Fight.Fight().Dispose();
            if (fight.Result != ResultType.SUCCESS)
                return Convert.ToInt32(fight.Result);
            rolelife = fight.PlayHp;
            return fight.Iswin ? (int)FightResultType.WIN : (int)FightResultType.LOSE;
        }

        /// <summary>主角血量验证</summary>
        private int RoleLifeProcess(Int64 rolelife)
        {
            int result = 0;
            if (rolelife <= 0)
            {
                result = (int)DuplicateClearanceType.CLEARANCE_FAIL;
            }
            else if (rolelife > 0)
            {
                result = (int)DuplicateClearanceType.CLEARANCE_SUCCESS;
            }
            return result;
        }

        /// <summary>挑战胜利计算奖励</summary>
        private bool FameReward(tg_user user,int npcid, int fightresult, int type)
        {
            var fame = user.fame;
            if (fightresult == (int)DuplicateClearanceType.CLEARANCE_SUCCESS)
            {
                if (type == (int)DuplicateTargetType.MONSTER)
                {
                    var towerenemy = Variable.BASE_TOWERENEMY.FirstOrDefault(m => m.id == npcid);
                    if (towerenemy == null)
                        return false;
                    user.fame = tg_user.IsFameMax(user.fame, towerenemy.award);
                    user.Update();
                    Common.GetInstance().RewardsToUser(user, (int)GoodsType.TYPE_FAME); 

                    //日志
                    Common.GetInstance(). LogOperate(user.id, fame, towerenemy.award, user.fame, (int) DuplicateCommand.TOWER_CHECKPOINT_DARE);
                }
            }
            return true;
        }    

        /// <summary>
        /// 获取能加的声望
        /// </summary>
        /// <param name="user"></param>
        /// <param name="receive">领取的金钱</param>
        private Int64 GetFame(tg_user user, int fame)
        {
            var _total_false = user.fame + fame;
            var poor = _total_false - Variable.MAX_FAME;
            if (!(poor > 0)) return fame;
            var use = fame - poor;
            return use < 0 ? 0 : use;
        }

        /// <summary>返回错误结果</summary>
        private ASObject Error(int error)
        {
            return new ASObject(Common.GetInstance().BuildData(error));
        }
    }
}
