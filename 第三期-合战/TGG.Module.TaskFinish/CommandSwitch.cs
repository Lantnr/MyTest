using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.TaskFinish.Service;

namespace TGG.Module.TaskFinish
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

        //public static CommandSwitch ObjInstance;
        ///// <summary>CommandSwitch单例模式</summary>
        //public static CommandSwitch GetInstance()
        //{
        //    return ObjInstance ?? (ObjInstance = new CommandSwitch());
        //}

        /// <summary>指令处理</summary>
        public ASObject Switch(int commandNumber, TGG.SocketServer.TGGSession session, ASObject data)
        {
            return Switch((int)TGG.Core.Enum.ModuleNumber.TASKFINISH, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGG.SocketServer.TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif

            if (commandNumber !=(int) TaskCommand.TASK_FINISH || moduleNumber != (int)ModuleNumber.TASKFINISH) return null;
            var aso = new ASObject();
            var task = new Task_Finish.TASK_FINISH();
            aso = task.CommandStart(session, data);
            task.Dispose();
            
          
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
