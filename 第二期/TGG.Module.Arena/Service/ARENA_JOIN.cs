using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Arena;
using TGG.SocketServer;

namespace TGG.Module.Arena.Service
{
    /// <summary>
    /// 进入竞技场
    /// </summary>
    public class ARENA_JOIN
    {
        private static ARENA_JOIN ObjInstance;

        /// <summary>ARENA_JOIN单体模式</summary>
        public static ARENA_JOIN GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new ARENA_JOIN());
        }

        /// <summary> 进入竞技场 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "ARENA_JOIN", "进入竞技场");
#endif
            var report = new List<view_arena_report>();
            var user = session.Player.User;
            var count = session.Player.Vip.arena_cd;
            var arena = tg_arena.FindByUserId(user.id);
            if (arena == null) arena = Common.GetInstance().InsertArena(user.id);
            else report = view_arena_report.GetEntityList(user.id, 5);

            decimal timeStamp = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            if (arena.time <= timeStamp) arena.time = 0;
            if (session.Fight.Personal.id == 0) session.Fight.Personal = PersonalInsert(session);
            var list = view_arena_ranking.GetFindByRanking(GetRivalRanking(arena.ranking));

            return new ASObject(BuildData((int)ResultType.SUCCESS, arena, list, report, count));
        }

        /// <summary>向用户推送更新</summary>
        public tg_fight_personal PersonalInsert(TGGSession session)
        {
            return tg_fight_personal.PersonalInsert(session.Player.User.id, session.Player.Role.Kind.id);
        }

        /// <summary> 计算当前排名前5名可挑战对手排名 </summary>
        /// <param name="number">当前排名</param>
        public List<int> GetRivalRanking(int number)
        {
            var list = new List<int>();
            if (number < 6)
            {
                for (int i = 1; i <= (number - (number - 5)) + 1; i++)
                {
                    list.Add(i);
                }
                list.Remove(number);
# if DEBUG
                XTrace.WriteLine("{0}:{1} {2}", "GetRivalRanking()", "自己当前排名:" + number, "可挑战的对手排名:" + string.Join(",", list.ToArray()));
#endif
                return list;
            }
            if (number > 200)
                number = 201;
            for (int i = (number - 5); i < number; i++)
            {
                list.Add(i);
            }
# if DEBUG
            XTrace.WriteLine("{0}:{1} {2}", "GetRivalRanking()", "自己当前排名:" + number, "可挑战的对手排名:" + string.Join(",", list.ToArray()));
#endif
            return list;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuildData(int result, tg_arena model, List<view_arena_ranking> list, List<view_arena_report> _list, int cooolCount)
        {
            var dic = new Dictionary<string, object>
            {
                {"result",result },
                {"arena",ConvertArenaVo(model,list,_list,cooolCount) }
            };
            return dic;
        }

        #region 数据转换

        /// <summary> List[view_arena_ranking] TO List[DekaronPlayerVo]</summary>
        private List<DekaronPlayerVo> ConvertArenaVo(IEnumerable<view_arena_ranking> list)
        {
            return list.Select(item => new DekaronPlayerVo
            {
                id = item.id,
                playId = item.user_id,
                level = item.role_level,
                name = item.player_name,
                sex = item.player_sex,
                vocation = item.player_vocation,
                arenaRank = item.ranking,
            }).ToList();
        }

        /// <summary> List[view_arena_report] TO List[ReportVo]</summary>
        private List<ReportVo> ConvertReportVo(IEnumerable<view_arena_report> list)
        {
            return list.Select(item => new ReportVo
            {
                id = item.id,
                playerName = item.player_name,
                isWin = item.isWin,
                time = GetTimeString(item.time),
                type = item.type,
            }).ToList();
        }

        /// <summary>转换前端需要的ArenaVo </summary>
        /// <param name="model">tg_arena实体</param>
        /// <param name="list">List[view_arena_ranking]实体</param>
        /// <param name="reports"> List[view_arena_report]实体</param>
        /// <param name="cooolCount"> 已清除冷却CD次数</param>
        private ArenaVo ConvertArenaVo(tg_arena model, List<view_arena_ranking> list, List<view_arena_report> reports, int cooolCount)
        {
            return new ArenaVo
            {
                id = model.id,
                time = model.time,
                count = model.count,
                ranking = model.ranking,
                buyCount = model.buy_count,
                hasCoolTime = cooolCount,
                isChallage = (model.time == 0),
                winCount = model.winCount,
                totalCount = model.totalCount,
                report = reports.Any() ? ConvertReportVo(reports) : new List<ReportVo>(),
                dekaronPlayer = list.Any() ? ConvertArenaVo(list) : new List<DekaronPlayerVo>(),
            };
        }

        private string GetTimeString(Int64 time)
        {
            return GetNoralTime(time).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary> 将时间戳转换为一般时间格式</summary>
        /// <param name="now"></param>
        /// <returns></returns>
        private DateTime GetNoralTime(double now)
        {
            DateTime dtStart = DateTime.Parse("1970-1-1 00:00:01");
            DateTime dtResult = dtStart.AddMilliseconds(now);
            return dtResult;
        }

        #endregion
    }
}
