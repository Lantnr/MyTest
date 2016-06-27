using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Games.Service
{
    /// <summary>
    /// 选择游戏模式
    /// </summary>
    public class GAMES_SELECT
    {
        private static GAMES_SELECT _objInstance;

        /// <summary>
        /// GAMES_SELECT单体模式
        /// </summary>
        public static GAMES_SELECT GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAMES_SELECT());
        }

        /// <summary>选择游戏模式</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "GAMES_SELECT", "选择游戏模式");
#endif
            var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);

            //验证选择模式数据
            if (type != (int)GameType.ACTIONGAME && type != (int)GameType.PRACTICEGAME)
                return Error((int)ResultType.FRONT_DATA_ERROR);

            session.Player.Game.GameType = type;
            return Common.GetInstance().Result((int)ResultType.SUCCESS);
        }

        private ASObject Error(int error)
        {
            return new ASObject(new Dictionary<string, object> { { "result", error } });
        }
    }
}
