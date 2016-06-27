using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Module.Props.Service;
using TGG.SocketServer;

namespace TGG.Module.Props
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
            return Switch((int)ModuleNumber.BAG, commandNumber, session, data);
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
                case (int)PropCommand.PROP_JOIN: { aso = PROP_JOIN.GetInstance().CommandStart(session, data); break; }
                case (int)PropCommand.PROP_USE: { aso = PROP_USE.GetInstance().CommandStart(session, data); break; }
                case (int)PropCommand.PROP_SELL: { aso = PROP_SELL.GetInstance().CommandStart(session, data); break; }
                case (int)PropCommand.PROP_SELL_BULK: { aso = PROP_SELL_BULK.GetInstance().CommandStart(session, data); break; }
                case (int)PropCommand.PROP_PICKUP: { aso = PROP_PICKUP.GetInstance().CommandStart(session, data); break; }
                case (int)PropCommand.PROP_DISCARD: { aso = PROP_DISCARD.GetInstance().CommandStart(session, data); break; }
                case (int)PropCommand.PROP_SYNTHETIC: { aso = PROP_SYNTHETIC.GetInstance().CommandStart(session, data); break; }
                case (int)PropCommand.PROP_ROLE_USE: { aso = PROP_ROLE_USE.GetInstance().CommandStart(session, data); break; }
                case (int)PropCommand.PROP_GENRE_USE: { aso = PROP_GENRE_USE.GetInstance().CommandStart(session, data); break; }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
