using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 武将点将加载
    /// </summary>
    public class TRAIN_HOME_JOIN
    {
        private static TRAIN_HOME_JOIN _objInstance;

        /// <summary>TRAIN_HOME_JOIN单体模式</summary>
        public static TRAIN_HOME_JOIN GetInstance()
        {
            return _objInstance ?? (_objInstance = new TRAIN_HOME_JOIN());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "TRAIN_HOME_JOIN", "武将点将加载");
#endif
                var extend = session.Player.UserExtend;

                var f = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17020");   //可挑战次数
                if (f == null) return Error((int)ResultType.BASE_TABLE_ERROR);

                var cf = Convert.ToInt32(f.value);
                var lfight = cf - extend.fight_count;

                return new ASObject(JoinData((int)ResultType.SUCCESS, extend.refresh_count, lfight, extend.fight_buy));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>返回结果</summary>
        /// <param name="result">结果</param>
        /// <param name="refresh">已刷新次数</param>
        /// <param name="lfight">剩余挑战次数</param>
        /// <param name="buycount">已购买次数</param>
        /// <returns></returns>
        private Dictionary<String, Object> JoinData(int result, int refresh, int lfight, int buycount)
        {
            var dic = new Dictionary<string, object>
            {
                { "result", result }, { "refresh", refresh }, { "fightCount", lfight < 0 ? 0 : lfight }, { "buyCount", buycount }
            };
            return dic;
        }

        /// <summary>返回前端错误信息</summary>
        private ASObject Error(int error)
        {
            return new ASObject(new Dictionary<string, object> { { "result", error } });
        }
    }
}
