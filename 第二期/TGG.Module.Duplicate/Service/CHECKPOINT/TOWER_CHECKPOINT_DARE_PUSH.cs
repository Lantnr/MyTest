using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.Duplicate;
using TGG.Core.Vo.User;
using TGG.SocketServer;

namespace TGG.Module.Duplicate.Service.CHECKPOINT
{
    /// <summary>
    /// 推送挑战结果
    /// </summary>
    class TOWER_CHECKPOINT_DARE_PUSH
    {
        private static TOWER_CHECKPOINT_DARE_PUSH ObjInstance;

        /// <summary>TOWER_CHECKPOINT_DARE_PUSH单体模式</summary>
        public static TOWER_CHECKPOINT_DARE_PUSH GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TOWER_CHECKPOINT_DARE_PUSH());
        }

        /// <summary>
        /// 推送挑战结果  （挑战小游戏）
        /// </summary>
        /// <param name="site">当前层数</param>
        /// <param name="type">挑战结果类型</param>       
        public void CommandStart(Int64 userid, int site, int type)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "TOWER_CHECKPOINT_DARE_PUSH", "推送挑战结果");
#endif

            var checkpoint = tg_duplicate_checkpoint.GetEntityByUserId(userid);
            var aso = new ASObject();
            if (checkpoint == null) return;
            var towerpass = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == site);
            if (towerpass == null) return;
            if (type == (int)DuplicateClearanceType.CLEARANCE_SUCCESS)
            {
                var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "9009");
                if (rule == null) return;
                if (towerpass.pass < Convert.ToInt32(rule.value)) //30:最后一层
                {
                    var userextend = tg_user_extend.GetByUserId(userid);
                    var towervo = Common.GetInstance().TowerPassMessage(userextend.npc_refresh_count, ref checkpoint);
                    if (towervo == null) return;
                    Common.GetInstance().CheckpointUpdate(checkpoint);
                    aso = BulidData((int)ResultType.SUCCESS, null, towervo, (int)FightResultType.WIN, checkpoint.blood, (int)DuplicateRightType.NORIGHT);
                }
                else
                    return;
            }
            else
            {
                aso = CheckChallengCount(checkpoint, towerpass, checkpoint.blood);
            }
            CheckpointDarePush(checkpoint.user_id, aso);
        }





        /// <summary>
        /// 推送挑战结果  （挑战怪物）
        /// </summary>
        /// <param name="target">挑战目标</param>
        /// <param name="type">挑战结果类型</param>
        /// <param name="blood">挑战剩余血量</param>
        public void CommandStart(Int64 userid, int target, int type, Int64 blood)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "TOWER_CHECKPOINT_DARE_PUSH", "推送挑战结果");
#endif
            var checkpoint = tg_duplicate_checkpoint.GetEntityByUserId(userid);
            var towerpass = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == checkpoint.site);

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "9009");
            if (checkpoint == null || towerpass == null || rule == null) return;

            checkpoint.blood = Convert.ToInt32(blood);
            if (type == (int)DuplicateClearanceType.CLEARANCE_SUCCESS)
            {
                if (towerpass.pass <= Convert.ToInt32(rule.value))
                {

                    if (target == (int)DuplicateTargetType.MONSTER)
                        FightMonster(checkpoint, towerpass.watchmen, Convert.ToInt32(rule.value));
                    else
                        FightWatchMen(checkpoint, Convert.ToInt32(rule.value));
                }
            }
            else
            {
                var aso = CheckChallengCount(checkpoint, towerpass, checkpoint.blood);
                CheckpointDarePush(checkpoint.user_id, aso);
            }
        }

        /// <summary>挑战守护者</summary>
        /// <param name="checkpoint">爬塔实体</param>
        /// <param name="count">翻将次数</param>
        /// <param name="highsite">最高层数</param>
        private void FightWatchMen(tg_duplicate_checkpoint checkpoint, int highsite)
        {
            var role = tg_role.GetEntityByUserId(checkpoint.user_id);
            var user = tg_user.GetUsersById(checkpoint.user_id);
            var userextend = tg_user_extend.GetByUserId(checkpoint.user_id);
            if (role == null || user == null || userextend == null) return;

            var tower = tg_duplicate_checkpoint.GetEntityByTowerSite(checkpoint.site);
            checkpoint = Common.GetInstance().BecomeTower(role.att_life, checkpoint);
            if (checkpoint.site == highsite)
            {
                var uservo = Common.GetInstance().ConvertUser(user);
                checkpoint.state = (int)DuplicateClearanceType.CLEARANCE_SUCCESS;
                checkpoint.Update();
                var _towervo = EntityToVo.ToTowerPassVo(checkpoint, 0, 0, userextend.npc_refresh_count, null);
                CheckpointDarePush(checkpoint.user_id, uservo, _towervo, (int)FightResultType.WIN, checkpoint.blood,
                    (int)DuplicateRightType.NORIGHT);
            }
            else
            {
                var towervo = Common.GetInstance().TowerPassMessage(userextend.npc_refresh_count, ref checkpoint);
                if (towervo == null) return;
                Common.GetInstance().CheckpointUpdate(checkpoint);
                CheckpointDarePush(checkpoint.user_id, null, towervo, (int)FightResultType.WIN, checkpoint.blood,
                    (int)DuplicateRightType.NORIGHT);
            }
            if (tower == null) return;
            tower.tower_site = 0;
            tower.Update();
            SendMessage(tower.user_id);
        }

        /// <summary>发送邮件</summary>
        private void SendMessage(Int64 userid)
        {
            var title = "一骑当千战报";
            var contents = "十分可惜，您被其他守护者击败！丧失了守护者资格";
            new Share.Message().BuildMessagesSend(userid, title, contents, "");
        }


        /// <summary>挑战怪物</summary>
        /// <param name="checkpoint">爬塔实体</param>
        /// <param name="towerid">塔主用户id</param>
        /// <param name="count">翻将次数</param>
        /// <param name="towersite">守护者层</param>
        private void FightMonster(tg_duplicate_checkpoint checkpoint, int towersite, int highsite)
        {
            var role = tg_role.GetEntityByUserId(checkpoint.user_id);
            var user = tg_user.GetUsersById(checkpoint.user_id);
            var userextend = tg_user_extend.GetByUserId(checkpoint.user_id);
            if (role == null || user == null || userextend == null) return;

            if (towersite == (int)DuplicateTargetType.WATCHMEM)
            {
                checkpoint.blood = role.att_life; //血回满    
                var tower = tg_duplicate_checkpoint.GetEntityByTowerSite(checkpoint.site);
                if (tower == null)
                {
                    checkpoint = Common.GetInstance().BecomeTower(role.att_life, checkpoint);
                    if (checkpoint.site == highsite)
                    {
                        var uservo = Common.GetInstance().ConvertUser(user);
                        checkpoint.state = (int)DuplicateClearanceType.CLEARANCE_SUCCESS;
                        checkpoint.Update();
                        var _towervo = EntityToVo.ToTowerPassVo(checkpoint, 0, 0, userextend.npc_refresh_count, null);
                        CheckpointDarePush(checkpoint.user_id, uservo, _towervo, (int)FightResultType.WIN, checkpoint.blood,
                            (int)DuplicateRightType.NORIGHT);
                        return;
                    }
                }
                else
                {
                    var wahtchmen = tg_user.GetUsersById(tower.user_id);
                    var uservo = Common.GetInstance().ConvertUser(wahtchmen);
                    checkpoint.state = (int)DuplicateClearanceType.CLEARANCE_SUCCESS;
                    checkpoint.Update();
                    CheckpointDarePush(checkpoint.user_id, uservo, null, (int)FightResultType.WIN, checkpoint.blood, checkpoint.dekaron);
                    return;
                }
            }
            var towervo = Common.GetInstance().TowerPassMessage(userextend.npc_refresh_count, ref checkpoint);
            if (towervo == null) return;
            //checkpoint.npcids = null;
            Common.GetInstance().CheckpointUpdate(checkpoint);
            CheckpointDarePush(checkpoint.user_id, null, towervo, (int)FightResultType.WIN, checkpoint.blood, (int)DuplicateRightType.NORIGHT);
        }


        /// <summary>推送挑战怪物结果</summary>
        private void CheckpointDarePush(Int64 userid, UserInfoVo uservo, TowerPassVo towerpassvo, int type, Int64 blood, int challenge)
        {
            var aso = new ASObject();
            aso = BulidData((int)ResultType.SUCCESS, uservo, towerpassvo, type, blood, challenge);
            CheckpointDarePush(userid, aso);
        }

        /// <summary>组装数据</summary>
        private ASObject BulidData(int result, UserInfoVo watchmen, TowerPassVo towerpass, int type, Int64 blood, int challenge)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},               
                {"type",type},
                {"blood",blood},
                {"challenge",challenge},
                {"watchmen",watchmen},
                {"towerpass", towerpass},
            };
            return new ASObject(dic);
        }

        /// <summary>发送爬塔挑战结束协议</summary>
        private static void CheckpointDarePush(Int64 userid, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "SKILL_LIFE_PUSH", "推送爬塔挑战结果");
#endif
            if (!Variable.OnlinePlayer.ContainsKey(userid))
                return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            var pv = session.InitProtocol((int)ModuleNumber.DUPLICATE, (int)TGG.Core.Enum.Command.DuplicateCommand.TOWER_CHECKPOINT_DARE_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }


        /// <summary>挑战失败，验证挑战次数</summary>
        public ASObject CheckChallengCount(tg_duplicate_checkpoint checkpoint, BaseTowerPass towerpass, int blood)
        {
            var aso = new ASObject();
            if (!Variable.OnlinePlayer.ContainsKey(checkpoint.user_id)) return aso;

            var session = Variable.OnlinePlayer[checkpoint.user_id] as TGGSession;
            var userextend = session.Player.UserExtend.CloneEntity();
            userextend.challenge_count--;
            userextend.Update();
            session.Player.UserExtend = userextend;

            if (userextend.challenge_count <= 0)
            {
                if (Common.GetInstance().CheckWatchmenRight(towerpass.watchmen, checkpoint))
                    checkpoint.dekaron = (int)DuplicateRightType.NORIGHT;
                else
                    checkpoint.state = (int)DuplicateClearanceType.CLEARANCE_FAIL;
                checkpoint.Update();
                aso = BulidData((int)ResultType.SUCCESS, null, null, (int)FightResultType.LOSE, checkpoint.blood, (int)DuplicateRightType.NORIGHT);
                return aso;
            }
            checkpoint.blood = session.Player.Role.Kind.att_life;
            return GetChalleng(userextend.npc_refresh_count, towerpass, checkpoint);
        }

        /// <summary>还有挑战次数，返回当前层</summary>
        /// <param name="count">翻将次数</param>
        /// <param name="towerpass">当前层基表数据</param>
        /// <param name="checkpoint">爬塔实体</param>
        /// <returns></returns>
        private ASObject GetChalleng(int count, BaseTowerPass towerpass, tg_duplicate_checkpoint checkpoint)
        {
            var aso = new ASObject();
            var towervo = new TowerPassVo();
            if (towerpass.enemyType == (int)DuplicateEnemyType.FIGHTING)
            {
                var npcid = Common.GetInstance().GetNpcId(checkpoint);
                var npclist = Common.GetInstance().GetNpcList(checkpoint);
                towervo = EntityToVo.ToTowerPassVo(checkpoint, towerpass.enemyType, npcid, count, npclist);

                if (Common.GetInstance().CheckWatchmenRight(towerpass.watchmen, checkpoint))
                {
                    var tower = tg_duplicate_checkpoint.GetEntityByTowerSite(checkpoint.site);
                    var wahtchmen = tg_user.GetUsersById(tower.user_id);
                    if (wahtchmen != null)
                    {
                        var uservo = Common.GetInstance().ConvertUser(wahtchmen);
                        aso = BulidData((int)ResultType.SUCCESS, uservo, towervo, (int)FightResultType.LOSE,
                            checkpoint.blood, checkpoint.dekaron);
                        return aso;
                    }
                }
            }
            else
                towervo = EntityToVo.ToTowerPassVo(checkpoint, towerpass.enemyType, towerpass.enemyId, count, null);
            aso = BulidData((int)ResultType.SUCCESS, null, towervo, (int)FightResultType.LOSE, checkpoint.blood, (int)DuplicateRightType.NORIGHT);
            Common.GetInstance().CheckpointUpdate(checkpoint);
            return aso;
        }
    }
}
