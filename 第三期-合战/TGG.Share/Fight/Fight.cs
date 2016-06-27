using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;
using TGG.Share.NewFight;
using TGG.SocketServer;

namespace TGG.Share.Fight
{
    public class Fight : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~Fight()
        {
            Dispose();
        }

        private delegate Core.Entity.Fight AddHandler(Int64 userid, Int64 rivalid, FightType type, Int64 hp = 0, bool of = false, bool po = false, bool or = false, bool pr = false, int rolehomeid = 0);

        /// <summary> 获取战斗 </summary>
        /// <param name="userid">用户Id</param>
        /// <param name="rivalid">对手Id</param>
        /// <param name="type">战斗类型</param>
        /// <param name="hp">要调控血量 (爬塔、活动、连续战斗调用)</param>      
        /// <param name="of">是否获取己方战斗Vo</param>
        /// <param name="po">是否推送己方战斗</param>
        /// <param name="or">是否获取对方战斗Vo</param>
        /// <param name="pr">是否推送对方战斗</param>
        /// <param name="rolehomeid">(武将宅类型时可用)要挑战武将宅id</param>
        /// <returns></returns>
        public Core.Entity.Fight GeFight(Int64 userid, Int64 rivalid, FightType type, Int64 hp = 0, bool of = false, bool po = false, bool or = false, bool pr = false, int rolehomeid = 0)
        {
            var handler = new AddHandler(new FightCommon().GeFight);
            var result = handler.BeginInvoke(userid, rivalid, type, hp, of, po, or, pr, rolehomeid, null, null);
            var handler1 = (AddHandler)((AsyncResult)result).AsyncDelegate;
            return handler1.EndInvoke(result);
        }

        /// <summary> 战斗推送协议 </summary>
        /// <param name="userid">userid</param>
        /// <param name="model">战斗Vo</param>
        public void SendProtocol(Int64 userid, FightVo model)
        {
            var s = Variable.OnlinePlayer.ContainsKey(userid);
            if (!s) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;

            var dic = new Dictionary<string, object> { { "result", (int)ResultType.SUCCESS }, { "fight", model } };
            var aso = new ASObject(dic);
            var pv = session.InitProtocol((int)ModuleNumber.FIGHT, (int)FightCommand.FIGHT_PERSONAL_ENTER, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }
    }
}
