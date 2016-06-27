using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Rankings.Service
{
    /// <summary>
    /// 闯关排行榜
    /// </summary>
    public class RANKING_YOUYIYUAN_LIST
    {
        private static RANKING_YOUYIYUAN_LIST ObjInstance;

        /// <summary>RANKING_YOUYIYUAN_LIST单体模式</summary>
        public static RANKING_YOUYIYUAN_LIST GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new RANKING_YOUYIYUAN_LIST());
        }

        /// <summary> 闯关排行榜 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "RANKING_YOUYIYUAN_LIST", "闯关排行榜");
#endif
            var userid = session.Player.User.id;
            var list = view_ranking_game.GetEntityList(10, userid);
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, Common.GetInstance().ConverListHonorVo(list, userid)));
        }
    }
}
