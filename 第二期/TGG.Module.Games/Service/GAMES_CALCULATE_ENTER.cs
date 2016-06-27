using System;
using System.Collections.Generic;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Games.Service
{
    /// <summary>
    /// 游艺园算术游戏进入
    /// </summary>
    public class GAMES_CALCULATE_ENTER
    {
        private static GAMES_CALCULATE_ENTER _objInstance;

        /// <summary>
        /// GAMES_CALCULATE_ENTER单体模式
        /// </summary>
        public static GAMES_CALCULATE_ENTER GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAMES_CALCULATE_ENTER());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "GAMES_CALCULATE_ENTER", "游艺园算术游戏进入");
#endif
                var player = session.Player;
                var game = player.Game;

                if (game.GameType == (int)GameType.ACTIONGAME)     //闯关模式
                {
                    //验证剩余次数
                    var b = Common.GetInstance().CheckCount((int)GameEnterType.老虎机, player.UserExtend);
                    if (!b) return Result((int)ResultType.GAME_COUNT_FAIL);
                }
                if (game.Calculate.calculate_pass == 0) game.Calculate.calculate_pass = 1;

                return Result((int)ResultType.SUCCESS);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>错误信息</summary>
        private ASObject Result(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }
    }
}
