using System;
using System.Collections.Generic;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Randoms;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Games.Service
{
    /// <summary>
    /// 游艺园忍术游戏进入
    /// </summary>
    public class GAMES_NINJUTSU_ENTER
    {
        private static GAMES_NINJUTSU_ENTER _objInstance;

        /// <summary>
        /// GAMES_NINJUTSU_ENTER单体模式
        /// </summary>
        public static GAMES_NINJUTSU_ENTER GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAMES_NINJUTSU_ENTER());
        }

        /// <summary>游艺园忍术游戏进入</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "GAMES_NINJUTSU_ENTER", "游艺园忍术游戏进入");
#endif
                var player = session.Player;
                var game = player.Game;

                if (game.GameType == (int)GameType.ACTIONGAME)     //闯关模式
                {
                    //验证剩余次数
                    var b = Common.GetInstance().CheckCount((int)GameEnterType.猜宝游戏, player.UserExtend);
                    if (!b) return Result((int)ResultType.GAME_COUNT_FAIL);
                }
                if (game.Ninjutsu.ninjutsu_pass == 0) game.Ninjutsu.ninjutsu_pass = 1;

                var position = RNG.Next(0, 3);
                session.Player.Position.position = position;    //获得色子位置

                return new ASObject(Common.GetInstance().NinjaEnterData((int)ResultType.SUCCESS, position));
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
