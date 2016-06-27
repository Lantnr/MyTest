using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Rankings;
using TGG.Core.Vo.RoleTrain;
using TGG.SocketServer;

namespace TGG.Module.Rankings.Service
{
    /// <summary>
    /// 功名榜
    /// </summary>
    public class RANKING_HONOR_LIST
    {
        private static RANKING_HONOR_LIST ObjInstance;

        /// <summary>HONOR_RANKING_LIST单体模式</summary>
        public static RANKING_HONOR_LIST GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new RANKING_HONOR_LIST());
        }

        /// <summary> 功名榜 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "HONOR_RANKING_LIST", "功名榜");
#endif
            var listrank = view_ranking_honor.GetEntityList(10);
            if (!listrank.Any())
                return new ASObject(Common.GetInstance().BuildData((int)ResultType.DATABASE_ERROR, null));

            //var listrank = list.Where(m => (m.ranks < 11)).ToList();
            var myrank = listrank.FirstOrDefault(m => m.id == session.Player.User.id);
            if (myrank == null)
            {
                var rk = view_ranking_honor.GetEntityByUserId(session.Player.User.id);
                if (rk != null)
                    listrank.Add(rk);
            }
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, Common.GetInstance().ConverListHonorVo(listrank, session.Player.User.id)));
        }

    }
}
