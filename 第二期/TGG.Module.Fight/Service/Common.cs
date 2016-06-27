using System;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using FluorineFx;
using TGG.Core.Vo.Fight;

namespace TGG.Module.Fight.Service
{
    /// <summary>
    /// 部分公共方法
    /// </summary>
    public partial class Common
    {
        public static Common ObjInstance;

        /// <summary>
        /// Common单体模式
        /// </summary>
        /// <returns></returns>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }

        /// <summary> 推送协议 </summary>
        /// <param name="session">session</param>
        /// <param name="model">战斗Vo</param>
        public void SendProtocol(TGGSession session, FightVo model)
        {
            var aso = new ASObject(FIGHT_PERSONAL_ENTER.GetInstance().BuildData(Convert.ToInt32(ResultType.SUCCESS), model));
            var pv = session.InitProtocol((int)ModuleNumber.FIGHT, (int)FightCommand.FIGHT_PERSONAL_ENTER, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary> 推送协议 </summary>
        /// <param name="session">session</param>
        /// <param name="aso">aso</param>
        public void SendProtocol(TGGSession session, ASObject aso)
        {
            var pv = session.InitProtocol((int)ModuleNumber.FIGHT, (int)FightCommand.FIGHT_PERSONAL_JOIN, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }
    }
}
