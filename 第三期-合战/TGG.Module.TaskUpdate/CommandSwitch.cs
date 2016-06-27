using System;
using FluorineFx;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Module.TaskUpdate.Service;
using TGG.SocketServer;

namespace TGG.Module.TaskUpdate
{
    /// <summary>
    /// 指令开关类
    /// </summary>
    public class CommandSwitch : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~CommandSwitch()
        {
            Dispose();
        }

        #endregion
        public static CommandSwitch ObjInstance;

        /// <summary>CommandSwitch单例模式</summary>
        public static CommandSwitch GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new CommandSwitch());
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int commandNumber, TGGSession session, ASObject data)
        {
            return Switch((int)ModuleNumber.TASKUPDATE, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
            if (commandNumber != (int)TaskCommand.TASK_UPDATE || moduleNumber != (int)ModuleNumber.TASKUPDATE) return null;
            var aso = new ASObject();
            var task = new Task_Update();
            aso = task.CommandStart(session, data);
            task.Dispose();
            return aso;
        }
    }
}
