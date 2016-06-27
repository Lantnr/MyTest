using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Module.Equip.Service;
using TGG.SocketServer;

namespace TGG.Module.Equip
{
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
            return Switch((int)ModuleNumber.EQUIP, commandNumber, session, data);
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
                //case (int)EquipCommand.EQUIP_JOIN: { aso = EQUIP_JOIN.GetInstance().CommandStart(session, data); break; }
                case (int)EquipCommand.EQUIP_SPIRIT: { aso = EQUIP_SPIRIT.GetInstance().CommandStart(session, data); break; }

                case (int)EquipCommand.EQUIP_BUY: { aso = EQUIP_BUY.GetInstance().CommandStart(session, data); break; }
                case (int)EquipCommand.EQUIP_SELL: { aso = EQUIP_SELL.GetInstance().CommandStart(session, data); break; }
                case (int)EquipCommand.EQUIP_BAPTIZE: { aso = EQUIP_BAPTIZE.GetInstance().CommandStart(session, data); break; }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
