using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Enum;
using TGG.Module.Prison.Service;
using TGG.SocketServer;

namespace TGG.Module.Prison
{
    /// <summary>
    /// 指令开关类
    /// </summary>
    public class CommandSwitch
    {
        public static CommandSwitch ObjInstance;

        /// <summary>CommandSwitch单例模式</summary>
        public static CommandSwitch GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new CommandSwitch());
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int commandNumber, TGGSession session, ASObject data)
        {
            return Switch((int)ModuleNumber.PRISON, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)TGG.Core.Enum.Command.PrisonCommand.CHECK:
                    {
                        aso = CHECK.getInstance().CommandStart(session, data);
                        break;
                    }
                case (int)TGG.Core.Enum.Command.PrisonCommand.MESSAGE_PAGE:
                    {
                        aso = MESSAGE_PAGE.GetInstance().CommandStart(session, data);
                        break;
                    }
                case (int)TGG.Core.Enum.Command.PrisonCommand.MOVING:
                    {
                        aso = MOVING.getInstance().CommandStart(session, data);
                        break;
                    }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
