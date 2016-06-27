using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using NewLife.Reflection;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using Convert = FluorineFx.Util.Convert;

namespace TGG.Module.Duplicate.Service.CHECKPOINT
{
    /// <summary>
    /// 翻将
    /// </summary>
    public class TOWER_CHECKPOINT_NPC_REFRESH
    {
        private static TOWER_CHECKPOINT_NPC_REFRESH ObjInstance;

        /// <summary>TOWER_CHECKPOINT_NPC_REFRESH单体模式</summary>
        public static TOWER_CHECKPOINT_NPC_REFRESH GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TOWER_CHECKPOINT_NPC_REFRESH());
        }

        /// <summary>翻将</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "TOWER_CHECKPOINT_NPC_REFRESH", "翻将");
#endif
            var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);
            var checkpoint = tg_duplicate_checkpoint.GetEntityByUserId(session.Player.User.id);
            int result = 0;
            string npcid;
            var towerenemys = new List<BaseTowerEnemy>();

            if (checkpoint == null) return Error((int)ResultType.DATABASE_ERROR);
            result = CheckSite(checkpoint);
            if (result != (int)ResultType.SUCCESS)
                return Error(result);
            if (!CheckRefreshCount(checkpoint, ref towerenemys))
                return Error((int)ResultType.TOWER_NPC_ENOUGH);
            result = Procress(checkpoint.user_id, type);
            if (result != (int)ResultType.SUCCESS)
                return Error(result);

            if (!towerenemys.Any()) return Error((int)ResultType.TOWER_NPC_ENOUGH);
            npcid = towerenemys.Count == 1 ? towerenemys[0].id.ToString() : Common.GetInstance().RefreshNpc(towerenemys);
            if (npcid.Length == 0)
                return Error((int)ResultType.TOWER_NPC_ENOUGH);
            checkpoint.npcids += "_" + npcid;
            if (!tg_duplicate_checkpoint.UpdateSite(checkpoint))
                return Error((int)ResultType.DATABASE_ERROR);

            return new ASObject(Common.GetInstance().BulidData((int)ResultType.SUCCESS, checkpoint,
             (int)DuplicateEnemyType.FIGHTING, Convert.ToInt32(npcid), session.Player.UserExtend.npc_refresh_count));
        }

        /// <summary>验证该层是否是挑战怪物层</summary>
        private int CheckSite(tg_duplicate_checkpoint checkpoint)
        {
            var towerpass = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == checkpoint.site);
            if (towerpass == null)
                return (int)ResultType.BASE_TABLE_ERROR;
            if (towerpass.enemyType != (int)DuplicateEnemyType.FIGHTING)
                return (int)ResultType.TOWER_NPC_NORIGHT;
            if (checkpoint.state == (int)DuplicateClearanceType.CLEARANCE_FAIL)
                return (int)ResultType.CHALLENGE_FAIL;
            if (towerpass.watchmen == (int)DuplicateTargetType.WATCHMEM &&
                checkpoint.state == (int)DuplicateClearanceType.CLEARANCE_SUCCESS)
            {
                return (int)ResultType.TOWER_WATCHEMEN_NOREFRESH;
            }
            return (int)ResultType.SUCCESS;
        }

        /// <summary>验证翻将使用元宝或翻将次数</summary>
        private int Procress(Int64 userid, int type)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid))
                return (int)ResultType.BASE_PLAYER_OFFLINE_ERROR;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null)
                return (int)ResultType.BASE_PLAYER_OFFLINE_ERROR;
            var user_extend = session.Player.UserExtend.CloneEntity();
            var user = session.Player.User.CloneEntity();
            if (type == (int)DuplicateRefreshType.REFRESHCOUNT_USE)
            {
                if (user_extend.npc_refresh_count == 0)
                    return (int)ResultType.CHALLENGENUM_LACK;
                user_extend.npc_refresh_count--;
                user_extend.Update();
                session.Player.UserExtend = user_extend;
            }
            else if (type == (int)DuplicateRefreshType.GOLD_USE)
            {
                var base_gold = Variable.BASE_RULE.FirstOrDefault(m => m.id == "9004");
                if (base_gold == null)
                    return (int)ResultType.BASE_TABLE_ERROR;
                if (!UserResource(Convert.ToInt32(base_gold.value), ref user))
                    return (int)ResultType.BASE_PLAYER_GOLD_ERROR;
                user.Update();
                Common.GetInstance().RewardsToUser(user, (int)GoodsType.TYPE_GOLD);
            }
            else
                return (int)ResultType.FRONT_DATA_ERROR;
            return (int)ResultType.SUCCESS;
        }

        /// <summary>判断当层怪物是否刷新完</summary>
        private bool CheckRefreshCount(tg_duplicate_checkpoint checkpoint, ref List<BaseTowerEnemy> towerenemys)
        {
            var ids = new List<int>();
            var npcs = Variable.BASE_TOWERENEMY.Where(m => m.pass == checkpoint.site).ToList();
            if (npcs.Any())
            {
                if (checkpoint.npcids != null)
                {
                    if (checkpoint.npcids.Contains("_"))
                    {
                        var npcids = checkpoint.npcids.Split('_');
                        if (npcids.Length == npcs.Count)
                            return false;
                        ids.Add(Convert.ToInt32(npcids[0]));
                        ids.Add(Convert.ToInt32(npcids[1]));
                        foreach (var item in npcs)
                        {
                            var te = ids.FirstOrDefault(m => m == item.id);
                            if (te == 0) towerenemys.Add(item);
                        }
                        return true;
                    }
                    ids.Add(Convert.ToInt32(checkpoint.npcids));
                    foreach (var item in npcs)
                    {
                        var te = ids.FirstOrDefault(m => m == item.id);
                        if (te == 0) towerenemys.Add(item);
                    }
                }
            }
            else
                return false;
            return true;
        }

        /// <summary>用户金钱判断</summary>
        private bool UserResource(int cost, ref tg_user user)
        {

            var _gold = user.gold;
            if (user.gold > cost)
            {
                var gold = user.gold - cost;
                user.gold = gold;
                //日志
                var logdata = string.Format("{0}_{1}_{2}_{3}", "G", _gold, cost, user.gold);
                (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.DUPLICATE, (int)DuplicateCommand.TOWER_CHECKPOINT_NPC_REFRESH, logdata);
                return true;
            }
            return false;
        }
        /// <summary>返回错误结果</summary>
        private ASObject Error(int result)
        {
            return new ASObject(Common.GetInstance().BulidData(result, null, 0, 0, 0));
        }
    }
}
