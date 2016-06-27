using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Module.Scene.Service;
using TGG.SocketServer;

namespace TGG.Module.Scene
{
    /// <summary>
    /// 指令处理
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
            return Switch((int)ModuleNumber.SCENE, commandNumber, session, data);
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
                case(int) SceneCommand.LOGIN_ENTER_SCENE:{aso =LOGIN_ENTER_SCENE.GetInstance().CommandStart(session,data);break;}
                case (int)SceneCommand.MOVING: { aso = MOVING.GetInstance().CommandStart(session, data); break; }
                case (int)SceneCommand.ENTER_SCENE: { aso = ENTER_SCENE.GetInstance().CommandStart(session, data); break; }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(),GetType().Namespace);
#endif
            return aso;
        }
    }
}
