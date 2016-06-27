using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Duplicate;
using TGG.Core.Vo.User;
using TGG.SocketServer;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;

namespace TGG.Module.Duplicate.Service.CHECKPOINT
{
    /// <summary>
    /// 进入爬塔
    /// </summary>
    public class TOWER_CHECKPOINT_ENTER
    {
        private static TOWER_CHECKPOINT_ENTER ObjInstance;

        /// <summary>TOWER_CHECKPOINT_ENTER单体模式</summary>
        public static TOWER_CHECKPOINT_ENTER GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TOWER_CHECKPOINT_ENTER());
        }

        /// <summary>进入爬塔</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "TOWER_CHECKPOINT_ENTER", "进入爬塔");
#endif
            var userextend = session.Player.UserExtend.CloneEntity();
            var checkpoint = tg_duplicate_checkpoint.GetEntityByUserId(session.Player.User.id);

            //第一次爬塔或第二天爬塔处理
            if (checkpoint == null || checkpoint.site == 0)
            {
                var brains = tg_role.GetSingleTotal(RoleAttributeType.ROLE_BRAINS, session.Player.Role.Kind);
                var count = RuleData(brains);
                var npcid = Common.GetInstance().RefreshNpc(1);
                if (checkpoint == null)
                {
                    var base_rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "9008");
                    if (base_rule == null) return Error((int)ResultType.BASE_TABLE_ERROR);
                    var role = session.Player.Role.Kind.CloneEntity();
                    checkpoint = InitTower(role, npcid);
                    userextend.npc_refresh_count = Convert.ToInt32(base_rule.value);
                    userextend.challenge_count = 1;
                    if (!tg_duplicate_checkpoint.GetInsert(checkpoint)) return Error((int)ResultType.DATABASE_ERROR);
                }
                else
                {
                    checkpoint.blood = session.Player.Role.Kind.att_life;  //第一次爬塔血回满
                    checkpoint.site = 1;
                    checkpoint.npcids = npcid;
                    userextend.challenge_count = 1;
                    if (!tg_duplicate_checkpoint.UpdateSite(checkpoint)) return Error((int)ResultType.DATABASE_ERROR);
                }

                userextend.npc_refresh_count += count;
                userextend.Update();
                session.Player.UserExtend = userextend;
                var towerpass = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == checkpoint.site);
                if (towerpass == null) return Error((int)ResultType.BASE_TABLE_ERROR);
                return CheckPointResult(towerpass.enemyType, Convert.ToInt32(npcid), checkpoint,
                    userextend.npc_refresh_count, userextend.challenge_count);
            }
            return FightAgain(checkpoint, userextend.npc_refresh_count, userextend.challenge_count);
        }

        /// <summary>再次进入爬塔界面</summary>
        /// <param name="checkpoint">爬塔实体</param>
        /// <param name="ncount">翻将次数</param>
        /// <param name="fcount">挑战次数</param>
        /// <param name="enemytype">挑战类型</param>
        /// <param name="enemyid">挑战对象</param>
        /// <returns></returns>
        private ASObject FightAgain(tg_duplicate_checkpoint checkpoint, int ncount, int fcount)
        {
            var aso = new ASObject();
            var user = tg_user.GetUsersById(checkpoint.user_id);
            var towerpass = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == checkpoint.site);
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "9009");
            if (user == null || towerpass == null || rule == null) return Error((int)ResultType.NO_DATA);

            var enemytype = towerpass.enemyType;
            var enemyid = towerpass.enemyId;
            if (checkpoint.state == (int)DuplicateClearanceType.CLEARANCE_FAIL) //当层通关失败
            {
                if (fcount > 0)
                {
                    aso = ChallengeAgain(ncount, fcount, enemytype, enemyid, checkpoint);
                    return aso;
                }
            }
            if (enemytype == (int)DuplicateEnemyType.SMALL_GAME) //验证关卡类型
                aso = CheckPointResult(enemytype, enemyid, checkpoint, ncount, fcount);
            else
            {
                if (checkpoint.site == Convert.ToInt32(rule.value) && checkpoint.tower_site == Convert.ToInt32(rule.value))
                {
                    aso = ReturnWatchMen(user, enemytype, enemyid, checkpoint,
                        ncount, fcount);
                }
                else
                    aso = ReturnNpc(ncount, fcount, checkpoint);
            }
            return aso;
        }

        /// <summary>返回挑战怪物层或守护者信息</summary>
        private ASObject ReturnNpc(int ncount, int fcount, tg_duplicate_checkpoint checkpoint)
        {
            var towerpass = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == checkpoint.site);
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "9009");
            if (towerpass == null || rule == null) return Error((int)ResultType.BASE_TABLE_ERROR);

            var npcid = Common.GetInstance().GetNpcId(checkpoint);
            if (Common.GetInstance().CheckWatchmenRight(towerpass.watchmen, checkpoint))
            {
                var tower = tg_duplicate_checkpoint.GetEntityByTowerSite(checkpoint.site);
                if (tower != null)
                {
                    var wahtchmen = tg_user.GetUsersById(tower.user_id);
                    var uservo = Common.GetInstance().ConvertUser(wahtchmen);
                    return new ASObject(Common.GetInstance().BulidData((int)ResultType.SUCCESS, checkpoint,
                        towerpass.enemyType, npcid, ncount, fcount,
                        uservo, checkpoint.dekaron));
                }
                var role = tg_role.GetEntityByUserId(checkpoint.user_id);
                checkpoint = Common.GetInstance().BecomeTower(role.att_life, checkpoint);

                if (checkpoint.site == Convert.ToInt32(rule.value))
                {
                    var user = tg_user.GetUsersById(checkpoint.user_id);
                    checkpoint.Save();
                    return ReturnWatchMen(user, towerpass.enemyType, npcid, checkpoint,
                       ncount, fcount);
                }
                var towervo = Common.GetInstance().TowerPassMessage(ncount, ref checkpoint);
                Common.GetInstance().CheckpointUpdate(checkpoint);
                return new ASObject(Common.GetInstance().BulidData((int)ResultType.SUCCESS, fcount,
                    towervo, null, (int)DuplicateRightType.NORIGHT));
            }
            return CheckPointResult(towerpass.enemyType, npcid, checkpoint, ncount, fcount);
        }

        /// <summary>返回玩家自己是守护者的信息</summary>
        private ASObject ReturnWatchMen(tg_user user, int enemytype, int enemyid, tg_duplicate_checkpoint checkpoint, int ncount, int fcount)
        {
            var uservo = Common.GetInstance().ConvertUser(user);
            var aso = new ASObject(Common.GetInstance().BulidData((int)ResultType.SUCCESS, checkpoint,
                 enemytype, enemyid, ncount, fcount,
                uservo, checkpoint.dekaron));
            return aso;
        }

        /// <summary>
        /// 玩家智谋值换算翻将次数
        /// </summary>
        /// <param name="brains">智谋值</param>
        private int RuleData(double brains)
        {
            var base_rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "9003");
            if (base_rule == null) return 0;
            var temp = base_rule.value;
            temp = temp.Replace("brains", brains.ToString("0.00"));
            var express = CommonHelper.EvalExpress(temp);
            var count = Convert.ToInt32(express);
            return count;
        }

        /// <summary>初始化关卡信息</summary>
        private tg_duplicate_checkpoint InitTower(tg_role role, string npcid)
        {
            return new tg_duplicate_checkpoint()
            {
                user_id = role.user_id,
                site = 1,
                npcids = npcid,
                blood = role.att_life,//第一次爬塔血回满
            };
        }

        /// <summary>还有挑战次数，返回当前层</summary>
        private ASObject ChallengeAgain(int ncount, int fcount, int enemytype, int enemyid, tg_duplicate_checkpoint checkpoint)
        {
            Common.GetInstance().CheckpointUpdate(checkpoint);

            if (enemytype == (int)DuplicateEnemyType.FIGHTING)
            {
                var npcid = Common.GetInstance().GetNpcId(checkpoint);
                return CheckPointResult(enemytype, npcid, checkpoint, ncount, fcount);
            }
            return CheckPointResult(enemytype, enemyid, checkpoint, ncount, fcount);
        }


        /// <summary>返回错误结果</summary>
        private ASObject Error(int result)
        {
            return new ASObject(Common.GetInstance().BulidData(result, null, 0, 0, 0, 0, null, (int)DuplicateRightType.NORIGHT));
        }

        /// <summary>返回闯关成功信息</summary>
        public ASObject CheckPointResult(int enemytype, int enemyid, tg_duplicate_checkpoint checkpoint, int ncount, int fcount)
        {
            var aso = new ASObject(Common.GetInstance().BulidData((int)ResultType.SUCCESS, checkpoint,
          enemytype, enemyid, ncount, fcount, null, (int)DuplicateRightType.NORIGHT));
            return aso;
        }
    }
}
