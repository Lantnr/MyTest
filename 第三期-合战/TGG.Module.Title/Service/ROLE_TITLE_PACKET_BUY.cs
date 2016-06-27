using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Title.Service
{
    /// <summary>
    /// 称号解锁
    /// </summary>
    public class ROLE_TITLE_PACKET_BUY : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~ROLE_TITLE_PACKET_BUY()
        {
            Dispose();
        }
        #endregion

        //private static ROLE_TITLE_PACKET_BUY _objInstance;

        ///// <summary>ROLE_TITLE_PACKET_BUY单体模式</summary>
        //public static ROLE_TITLE_PACKET_BUY GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new ROLE_TITLE_PACKET_BUY());
        //}

        /// <summary>称号解锁</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "ROLE_TITLE_PACKET_BUY", "称号解锁");
#endif
                if (!data.ContainsKey("titleId")) return null;    //验证前端传递数据
                var tid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "titleId").Value.ToString());
                var user = session.Player.User.CloneEntity();

                var title = tg_role_title.GetTitleByTid(tid, user.id);
                if (title == null) return Error((int)ResultType.FRONT_DATA_ERROR);
                if (title.user_id != user.id) return null;   //验证玩家信息，防止发包
                if (title.title_count >= 3) return Error((int)ResultType.TITLE_COUNT_FULL);  //验证已解锁称号格子数量

                var cost = CostGold(title);    //计算解锁格子需要花费的元宝信息
                var gold = user.gold;
                user.gold = user.gold - cost;
                if (user.gold < 0) return Error((int)ResultType.BASE_PLAYER_GOLD_ERROR);
                user.Update();

                //记录玩家元宝资源消费记录
                log.GoldInsertLog(cost, user.id, (int)ModuleNumber.TITLE, (int)TitleCommand.ROLE_TITLE_PACKET_BUY);

                (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, user);  //推送用户资源消耗更新
                session.Player.User = user;

                //更新解锁后称号信息
                if (!tg_role_title.UpdateByTitle(title)) return Error((int)ResultType.DATABASE_ERROR);

                //记录元宝花费日志
                var logdata = string.Format("{0}_{1}_{2}_{3}", "Gold", gold, cost, user.gold);
                (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.TITLE, (int)TitleCommand.ROLE_TITLE_PACKET_BUY, "称号", "解锁", "元宝", (int)GoodsType.TYPE_GOLD, cost, user.gold, logdata);
                return new ASObject(BuildLoadData((int)ResultType.SUCCESS, title));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>计算开格子的花费</summary>
        private int CostGold(tg_role_title title)
        {
            title.title_count = title.title_count + 1;
            var cost = 0;
            if (title.title_count == 2)   //根据解锁格子的数量判断所需元宝数量
            {
                var gold = Variable.BASE_RULE.FirstOrDefault(m => m.id == "24001");
                if (gold == null) return cost;
                cost = Convert.ToInt32(gold.value);
            }
            else if (title.title_count == 3)
            {
                var money = Variable.BASE_RULE.FirstOrDefault(m => m.id == "24002");
                if (money == null) return cost;
                cost = Convert.ToInt32(money.value);
            }
            return cost;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuildLoadData(int result, tg_role_title title)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "title", title == null ? null : EntityToVo.ToTitleVo(title) } };
            return dic;
        }

        private ASObject Error(int error)
        {
            return new ASObject(new Dictionary<string, object> { { "error", error } });
        }
    }
}
