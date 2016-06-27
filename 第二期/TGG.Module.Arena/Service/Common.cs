using System.Threading;
using System.Threading.Tasks;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Module.Arena.Service
{
    /// <summary>
    /// 部分公共方法
    /// </summary>
    public partial class Common
    {

        public static Common ObjInstance;

        /// <summary>Common 单体模式</summary>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuildData(int result)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
            };
            return dic;
        }

        /// <summary>向用户推送更新</summary>
        public void RewardsToUser(TGGSession session, tg_user user, int type)
        {
            session.Player.User = user;
            (new Share.User()).REWARDS_API(type, session.Player.User);
        }

        /// <summary> 竞技场每日初始化 </summary>
        public void InitArena()
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "23001");
            if (rule == null) { XTrace.WriteLine("{0}:{1}", "InitArena()", "竞技场固定规则表23001为空！"); return; }
            var count = Convert.ToInt32(rule.value);
            if (!tg_arena.UpdateArenaCount(count))
                XTrace.WriteLine("{0}:{1}", "InitArena()", "竞技场每日可挑战次数初始化数据库操作异常！");
        }

        /// <summary> 插入玩家竞技场信息 </summary>
        /// <param name="userid">用户id</param>
        public tg_arena InsertArena(Int64 userid)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "23001");
            if (rule == null) return null;
            var count = Convert.ToInt32(rule.value);
            var model = new tg_arena
            {
                time = 0,
                count = count,
                winCount = 0,
                user_id = userid,
                remove_cooling = 0,
                ranking = tg_arena.FindCount() + 1,
                totalCount = count,
            };
            model.Insert();
            return model;
        }

        /// <summary> 当前时间 </summary>
        /// <returns></returns>
        public decimal GetTime()
        {
            return (DateTime.Now.Ticks - 621355968000000000) / 10000;
        }

    }
}
