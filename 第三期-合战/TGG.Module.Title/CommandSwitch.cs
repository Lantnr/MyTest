using FluorineFx;
using NewLife.Log;
using System;
using System.Diagnostics;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.Title.Service;
using TGG.SocketServer;

namespace TGG.Module.Title
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
        #endregion

        /// <summary>指令处理</summary>
        public ASObject Switch(int commandNumber, TGGSession session, ASObject data)
        {
            return Switch((int)ModuleNumber.TITLE, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.称号))
                return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);

            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)TitleCommand.ROLE_TITLE_JOIN:
                    {
                        var title = new ROLE_TITLE_JOIN();
                        aso = title.CommandStart(session, data);
                        title.Dispose();
                        break;
                    }
                case (int)TitleCommand.ROLE_TITLE_LOAD:
                    {
                        var title = new ROLE_TITLE_LOAD();
                        aso = title.CommandStart(session, data);
                        title.Dispose();
                        break;
                    }
                case (int)TitleCommand.ROLE_TITLE_UNLOAD:
                    {
                        var title = new ROLE_TITLE_UNLOAD();
                        aso = title.CommandStart(session, data);
                        title.Dispose();
                        break;
                    }
                case (int)TitleCommand.ROLE_TITLE_PACKET_BUY:
                    {
                        var title = new ROLE_TITLE_PACKET_BUY();
                        aso = title.CommandStart(session, data);
                        title.Dispose();
                        break;
                    }
                case (int)TitleCommand.ROLE_TITLE_SELECT:
                    {
                        var title = new ROLE_TITLE_SELECT();
                        aso = title.CommandStart(session, data);
                        title.Dispose();
                        break;
                    }
                default: { aso = null; break; }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
