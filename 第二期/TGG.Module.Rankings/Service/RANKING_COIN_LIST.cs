using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Rankings;
using TGG.SocketServer;

namespace TGG.Module.Rankings.Service
{
    /// <summary>
    /// 富豪榜
    /// </summary>
    public class RANKING_COIN_LIST
    {
        private static RANKING_COIN_LIST ObjInstance;

        /// <summary>COIN_RANKING_LIST单体模式</summary>
        public static RANKING_COIN_LIST GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new RANKING_COIN_LIST());
        }

        /// <summary> 富豪榜 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "COIN_RANKING_LIST", "富豪榜");
#endif
            var listrank = view_ranking_coin.GetEntityList(10);
            if (!listrank.Any())
                return new ASObject(Common.GetInstance().BuildData((int)ResultType.DATABASE_ERROR, null));

            //var listrank = list.Where(m => (m.ranks < 11)).ToList();
            var myrank = listrank.FirstOrDefault(m => m.id == session.Player.User.id);
            if (myrank == null)
            {
                var rk = view_ranking_coin.GetEntityByUserId(session.Player.User.id);
                if (rk != null)
                    listrank.Add(rk);
            }
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, Common.GetInstance().ConverListHonorVo(listrank, session.Player.User.id)));
        }
    }
}
