using FluorineFx;
using NewLife.Log;
using System;
using System.Diagnostics;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.Work.Service;
using TGG.SocketServer;

namespace TGG.Module.Work
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
            return Switch((int)ModuleNumber.WORK, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            if (commandNumber == (int)WorkCommand.WORK_UPDATE||
                commandNumber == (int)WorkCommand.WORK_ACCEPT||
                commandNumber == (int)WorkCommand.WORK_DROP
                )
            {
                if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.工作))
                    return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            }

            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)WorkCommand.WORK_JOIN:
                {
                    var work = new WORK_JOIN();
                    aso =work.CommandStart(session, data);
                    work.Dispose();
                    break;
                }
                case (int)WorkCommand.WORK_UPDATE:
                {
                    var work = new WORK_UPDATE();
                    aso = work.CommandStart(session, data);
                    work.Dispose();
                    break;
                }
                case (int)WorkCommand.WORK_ACCEPT:
                {
                    var work = new WORK_ACCEPT();
                    aso = work.CommandStart(session, data);
                    work.Dispose();
                    break;
                }
                case (int)WorkCommand.WORK_CANCEL:
                {
                    var work = new WORK_CANCEL();
                    aso = work.CommandStart(session, data);
                    work.Dispose();
                    break;
                }
                case (int)WorkCommand.WORK_IS_FIGHT:
                {
                    var work = new WORK_IS_FIGHT();
                    aso = work.CommandStart(session, data);
                    work.Dispose();
                    break;
                }
                case (int)WorkCommand.WORK_FIGHT:
                {
                    var work = new WORK_FIGHT();
                    aso = work.CommandStart(session, data);
                    work.Dispose();
                    break;
                }
                case (int)WorkCommand.WORK_SEARCH:
                {
                    var work = new WORK_SEARCH();
                    aso = work.CommandStart(session, data);
                    work.Dispose();
                    break;
                }
                case (int)WorkCommand.WORK_DROP:
                {
                    var work = new WORK_DROP();
                    aso = work.CommandStart(session, data);
                    work.Dispose();
                    break;
                }
                case (int)WorkCommand.WORK_SELECT_DEFEND:
                {
                    var work = new WORK_SELECT_DEFEND();
                    aso = work.CommandStart(session, data);
                    work.Dispose();
                    break;
                }
                case (int)WorkCommand.WORK_CHECK_DEFEND:
                {
                    var work = new WORK_CHECK_DEFEND();
                    aso = work.CommandStart(session, data);
                    work.Dispose();
                    break;
                }

                default:{aso = null; break;}
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
