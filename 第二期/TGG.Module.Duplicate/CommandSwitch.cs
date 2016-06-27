using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.Duplicate.Service.CHECKPOINT;
using TGG.Module.Duplicate.Service.SHOT;
using TGG.SocketServer;

namespace TGG.Module.Duplicate
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
            return Switch((int)ModuleNumber.DUPLICATE, commandNumber, session, data);
        }

        /// <summary>
        /// 指令处理
        /// </summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            if (commandNumber == (int) DuplicateCommand.TOWER_SHOT_ENTER ||
                commandNumber == (int) DuplicateCommand.TOWER_SHOT_FUNISH)
            {
                if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.猎魂))
                    return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            }
            else
            {
                if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.一骑当千))
                    return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            }
            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)DuplicateCommand.TOWER_SHOT_ENTER: { aso = TOWER_SHOT_ENTER.GetInstance().CommandStart(session, data); break; }
                case (int)DuplicateCommand.TOWER_SHOT_FUNISH: { aso = TOWER_SHOT_FUNISH.GetInstance().CommandStart(session, data); break; }
                case (int)DuplicateCommand.TOWER_CHECKPOINT_NINJUTSU_GAME_START: { aso = TOWER_CHECKPOINT_NINJUTSU_GAME_START.GetInstance().CommandStart(session, data); break; }
                case (int)DuplicateCommand.TOWER_CHECKPOINT_NINJUTSU_GAME_RESULT: { aso = TOWER_CHECKPOINT_NINJUTSU_GAME_RESULT.GetInstance().CommandStart(session, data); break; }
                case (int)DuplicateCommand.TOWER_CHECKPOINT_CALCULATE_GAME: { aso = TOWER_CHECKPOINT_CALCULATE_GAME.GetInstance().CommandStart(session, data); break; }
                case (int)DuplicateCommand.TOWER_CHECKPOINT_ENTER: { aso = TOWER_CHECKPOINT_ENTER.GetInstance().CommandStart(session, data); break; }
                case (int)DuplicateCommand.TOWER_CHECKPOINT_NPC_REFRESH: { aso = TOWER_CHECKPOINT_NPC_REFRESH.GetInstance().CommandStart(session, data); break; }
                case (int)DuplicateCommand.TOWER_CHECKPOINT_CALCULATE_GAME_JOIN: { aso = TOWER_CHECKPOINT_CALCULATE_GAME_JOIN.GetInstance().CommandStart(session, data); break; }
                case (int)DuplicateCommand.TOWER_CHECKPOINT_TEA_GAME_ENTER: { aso = TOWER_CHECKPOINT_TEA_GAME_ENTER.GetInstance().CommandStart(session, data); break; }
                case (int)DuplicateCommand.TOWER_CHECKPOINT_TEA_GAME_FLOP: { aso = TOWER_CHECKPOINT_TEA_GAME_FLOP.GetInstance().CommandStart(session, data); break; }
                case (int)DuplicateCommand.TOWER_CHECKPOINT_DARE: { aso = TOWER_CHECKPOINT_DARE.GetInstance().CommandStart(session, data); break; }
                case (int)DuplicateCommand.TOWER_CHECKPOINT_ELOQUENCE_GAME: { aso = TOWER_CHECKPOINT_ELOQUENCE_GAME.GetInstance().CommandStart(session, data); break; }
                case (int)DuplicateCommand.TOWER_CHECKPOINT_ELOQUENCE_GAME_ENTER: { aso = TOWER_CHECKPOINT_ELOQUENCE_GAME_ENTER.GetInstance().CommandStart(session, data); break; }
                case (int)DuplicateCommand.TOWER_CHECKPOINT_NEXT_PASS: { aso = TOWER_CHECKPOINT_NEXT_PASS.GetInstance().CommandStart(session, data); break; }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
