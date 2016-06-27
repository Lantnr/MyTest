using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.Fight;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Arena.Service
{
    /// <summary>
    /// 挑战
    /// </summary>
    public class ARENA_DEKARON
    {
        private static ARENA_DEKARON _objInstance;

        /// <summary>ARENA_DEKARON单体模式</summary>
        public static ARENA_DEKARON GetInstance()
        {
            return _objInstance ?? (_objInstance = new ARENA_DEKARON());
        }

        /// <summary> 挑战 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "ARENA_DEKARON", "竞技场挑战");
#endif
            var rivalid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);
            if (rivalid == 0) return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            var rivalrank = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "rivalrank").Value);

            var user = session.Player.User.CloneEntity();
            var arena = tg_arena.FindByUserId(user.id) ?? Common.GetInstance().InsertArena(user.id);
            if (arena.count <= 0) return CommonHelper.ErrorResult(ResultType.ARENA_DEKARON_COUNT_LOCK);
            if (arena.time > Common.GetInstance().GetTime()) return CommonHelper.ErrorResult(ResultType.ARENA_TIME_ERROR);

            var rivalArena = tg_arena.FindByUserId(rivalid);
            if (rivalArena == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);
            if (rivalArena.ranking != rivalrank) return ARENA_JOIN.GetInstance().CommandStart(session, new ASObject());

            var mintime = GetBaseRuleValue("23004");
            if (mintime == 0) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);

            var fightvo = GetFightVo(user.id, rivalid);
            if (fightvo == null) return CommonHelper.ErrorResult(ResultType.FIGHT_ERROR);

            UpdateRanking(user.id, user.player_name, fightvo, arena, rivalArena, mintime);
            fightvo = GetRewards(session, user, fightvo);
            FightHistory(arena, rivalArena, fightvo, session.Player.User.player_name);
            PushArenaJoin(session);
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "ARENA_DEKARON", "竞技场挑战成功");
#endif
            (new Share.DaMing()).CheckDaMing(user.id, (int)DaMingType.竞技场);   //检测大名令竞技场完成度
            return new ASObject(BuildData((int)ResultType.SUCCESS, fightvo));
        }

        /// <summary> 储存战斗 </summary>
        public void FightHistory(tg_arena arena, tg_arena rivalArena, FightVo fight, string playname)
        {
            if (arena.ranking <= 200) FightHistory(arena.user_id, fight, rivalArena.user_id, true);
            if (rivalArena.ranking > 200) return;
            var f = fight.CloneEntity();
            f.wuJiangName = playname;
            FightHistory(rivalArena.user_id, f, arena.user_id, false);
        }

        /// <summary> 战斗记录 </summary>
        public void FightHistory(Int64 userid, FightVo fight, Int64 rivalid, bool flag)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "FightHistory()", "记录战斗过程");
#endif
            var model = new tg_arena_reports();
            var list = tg_arena_reports.FindByUserId(userid).ToList();
            var w = flag ? fight.isWin : !fight.isWin;
            if (!flag)
            {
                fight.rewards = null;
                fight.isWin = w;
            }
            model.isWin = w;
            model.other_user_id = rivalid;
            model.history = CommonHelper.OToB(fight);
            model.time = Convert.ToInt64(Common.GetInstance().GetTime());
            model.type = flag ? (int)ArenaReportType.ATTACK : (int)ArenaReportType.DEFENSE;
            if (list.Count() < 5)
                model.user_id = userid;
            else
                model.id = list.OrderBy(m => m.time).First().id;
            model.Save();
        }

        /// <summary> 战斗胜负所得奖励 </summary>
        /// <param name="session"></param>
        /// <param name="user"></param>
        /// <param name="fight"></param>
        public FightVo GetRewards(TGGSession session, tg_user user, FightVo fight)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "GetRewards()", "战斗胜负所得奖励发放");
#endif
            int moneyValue;
            int fameValue;

            if (fight.isWin)
            {
                moneyValue = GetBaseRuleValue("23005");
                fameValue = GetBaseRuleValue("23006");
            }
            else
            {
                moneyValue = GetBaseRuleValue("23007");
                fameValue = GetBaseRuleValue("23008");
            }

            fight.rewards.Add(new RewardVo { goodsType = (int)GoodsType.TYPE_COIN, value = moneyValue });
            fight.rewards.Add(new RewardVo { goodsType = (int)GoodsType.TYPE_FAME, value = fameValue });

            user.coin = tg_user.IsCoinMax(user.coin, moneyValue);
            user.fame = tg_user.IsFameMax(user.fame, fameValue);
            user.Update();

            Common.GetInstance().RewardsToUser(session, user, (int)GoodsType.TYPE_COIN);
            Common.GetInstance().RewardsToUser(session, user, (int)GoodsType.TYPE_FAME);

            return fight;
        }

        /// <summary> 获取基表Value值 </summary>
        private int GetBaseRuleValue(string id)
        {
            var b = Variable.BASE_RULE.FirstOrDefault(m => m.id == id);
            return b != null ? Convert.ToInt32(b.value) : 0;
        }

        /// <summary> 更改双方排名 </summary>
        /// <param name="userid">用户id</param>
        /// <param name="pname">玩家昵称</param>
        /// <param name="fight">战斗</param>
        /// <param name="onsarena">己方排名</param>
        /// <param name="rivalarena">对方排名</param>
        public void UpdateRanking(Int64 userid, string pname, FightVo fight, tg_arena onsarena, tg_arena rivalarena, int mintime)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1} {2}", "IsUpdateRanking()", "更改双方排名方法   更改前排名:", onsarena.ranking);
#endif

            rivalarena = tg_arena.FindByUserId(rivalarena.user_id);
            onsarena = tg_arena.FindByUserId(onsarena.user_id);
            var timeStamp = Convert.ToInt64(Common.GetInstance().GetTime());
            onsarena.count -= 1; onsarena.time = (mintime * 60000) + timeStamp;
            if (!fight.isWin)
            {
                onsarena.winCount = 0;
                onsarena.Update();
                return;
            }
            var count = fight.moves.Sum(m => m.Count());
            if (onsarena.ranking > rivalarena.ranking)
            {
                var ranking = onsarena.ranking;
                onsarena.ranking = rivalarena.ranking;
                rivalarena.ranking = ranking;
                rivalarena.Update();
                if (onsarena.ranking == 1)
                    PushSystemNews(userid, pname, 100005, Convert.ToInt32(count * 2)); //打赢后 是否排名第一，是:推送所有玩家
            }
            onsarena.winCount += 1;
            onsarena.Update();

            IsPushSystem(userid, pname, onsarena, Convert.ToInt32(count * 2));
        }

        /// <summary> 验证是否推送系统公告 </summary>
        /// <param name="userid">用户id</param>
        /// <param name="pname">玩家昵称</param>
        /// <param name="onsarena">当前玩家 tg_arena</param>
        /// <param name="time">战斗时间</param>
        private void IsPushSystem(Int64 userid, string pname, tg_arena onsarena, int time)
        {
            switch (onsarena.winCount)
            {
                case 10: { PushSystemNews(userid, pname, 100006, time); break; }//达成十连杀
                case 15: { PushSystemNews(userid, pname, 100007, time); break; }//达成十五连杀
                case 20: { PushSystemNews(userid, pname, 100008, time); break; }//达成二十连杀
                case 25: { PushSystemNews(userid, pname, 100009, time); break; }//达成二十五连杀
                case 30: { PushSystemNews(userid, pname, 100010, time); break; }//达成三十连杀
                default: { break; }
            }
        }

        /// <summary> 系统公告推送 </summary>
        private static void PushSystemNews(Int64 userid, string pname, int id, int time)
        {
            var aso = new List<ASObject> { (new Chat()).BuildData((int)ChatsASObjectType.PLAYERS, null, userid, pname, null) };

            var list = Variable.OnlinePlayer.Keys;
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                var obj = new PushObject()
                {
                    rivalid = item,
                    user_id = userid,
                    player_name = pname,
                };
                Task.Factory.StartNew(m =>
             {
                 SpinWait.SpinUntil(() => false, Convert.ToInt32(m));
             }, time * 1000, token.Token)
             .ContinueWith((m, n) =>
             {
                 var lo = n as PushObject;
                 if (lo == null) { token.Cancel(); return; }
                 if (!Variable.OnlinePlayer.ContainsKey(lo.rivalid)) return;
                 var session = Variable.OnlinePlayer[lo.rivalid];
                 (new Chat()).SystemChatSend(session, aso, id);
                 token.Cancel();
             }, obj, token.Token);
            }
        }

        /// <summary>
        /// 线程对象
        /// </summary>
        public class PushObject
        {
            public Int64 rivalid { get; set; }

            public Int64 user_id { get; set; }

            public String player_name { get; set; }
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result, FightVo model)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", result },
            { "fight", model },
            };
            return dic;
        }

        /// <summary>调用战斗 得到战斗过程</summary>
        public FightVo GetFightVo(Int64 userid, Int64 rivalid)
        {
            var fight = new Share.Fight.Fight().GeFight(userid, rivalid, FightType.ONE_SIDE, 0, true);
            new Share.Fight.Fight().Dispose();
            return fight.Result != ResultType.SUCCESS ? null : fight.Ofight;
        }

        /// <summary>推送竞技场数据</summary>
        public void PushArenaJoin(TGGSession session)
        {
            var aso = ARENA_JOIN.GetInstance().CommandStart(session, new ASObject());
            var key = string.Format("{0}_{1}", (int)ModuleNumber.ARENA, (int)ArenaCommand.ARENA_JOIN);
            session.SPM.AddOrUpdate(key, aso, (m, n) => aso);
        }
    }
}
