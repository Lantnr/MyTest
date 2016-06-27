using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Games.Service
{
    /// <summary>
    /// 猎魂游戏进入
    /// </summary>
    public class GAMES_SHOT_ENTER
    {

        private static GAMES_SHOT_ENTER _objInstance;

        /// <summary>
        /// GAMES_SHOT_ENTER单体模式
        /// </summary>
        public static GAMES_SHOT_ENTER GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAMES_SHOT_ENTER());
        }

        /// <summary>猎魂游戏进入</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "GAMES_SHOT_ENTER", "猎魂游戏进入");
#endif
            var player = session.Player;

            if (player.Game.GameType == (int)GameType.ACTIONGAME)
            {
                //验证剩余次数
                var b = Common.GetInstance().CheckCount((int)GameEnterType.猎魂, player.UserExtend);
                if (!b) return Common.GetInstance().Result((int)ResultType.GAME_COUNT_FAIL);
            }
            if (player.Game.Spirit.spirit_pass == 0) player.Game.Spirit.spirit_pass = 1;

            return Common.GetInstance().Result((int)ResultType.SUCCESS);
        }
    }
}
