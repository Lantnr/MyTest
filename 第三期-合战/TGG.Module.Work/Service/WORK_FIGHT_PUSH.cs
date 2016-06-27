using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Fight;
using TGG.SocketServer;

namespace TGG.Module.Work.Service
{
    /// <summary>
    /// 战斗推送
    /// </summary>
    public class WORK_FIGHT_PUSH : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WORK_FIGHT_PUSH()
        {
            Dispose();
        }
    
        #endregion
        //private static WORK_FIGHT_PUSH ObjInstance;

        ///// <summary> WORK_FIGHT_PUSH单体模式 </summary>
        //public static WORK_FIGHT_PUSH GetInstance()
        //{
        //    return ObjInstance ?? (ObjInstance = new WORK_FIGHT_PUSH());
        //}

        /// <summary> 战斗推送</summary>
        public void CommandStart(TGGSession session, FightVo fightvo, int fightresult)
        {
            var aso = BuildData(fightvo, fightresult);
            FightPush(session, aso);
        }
        /// <summary>组装数据</summary>
        private ASObject BuildData(FightVo fightvo, int fightresult)
        {
            var dic = new Dictionary<string, object> { { "type", fightresult }, { "fight", fightvo } };
            return new ASObject(dic);
        }

        /// <summary>战斗推送</summary>
        private static void FightPush(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "WORK_FIGHT_PUSH", "战斗推送");
#endif
            var pv = session.InitProtocol((int)ModuleNumber.WORK, (int)TGG.Core.Enum.Command.WorkCommand.WORK_FIGHT_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }
    }
}
