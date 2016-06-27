using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Games.Service
{
    /// <summary>
    /// 辩才游戏进入
    /// </summary>
    public class GAMES_ELOQUENCE_ENTER
    {
        private static GAMES_ELOQUENCE_ENTER _objInstance;

        /// <summary>
        /// GAMES_ELOQUENCE_ENTER单体模式
        /// </summary>
        public static GAMES_ELOQUENCE_ENTER GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAMES_ELOQUENCE_ENTER());
        }

        /// <summary>辩才游戏进入</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "GAMES_ELOQUENCE_ENTER", "辩才小游戏进入");
#endif
            var player = session.Player;

            if (player.Game.GameType == (int)GameType.ACTIONGAME)
            {
                //验证剩余次数
                var b = Common.GetInstance().CheckCount((int)GameEnterType.辩驳游戏, player.UserExtend);
                if (!b) return Common.GetInstance().Result((int)ResultType.GAME_COUNT_FAIL);
            }

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "30001");
            if (rule == null) return Data((int)ResultType.BASE_TABLE_ERROR,0);

            player.Game.Eloquence.user_blood = Convert.ToInt32(rule.value);
            player.Game.Eloquence.npc_blood = Convert.ToInt32(rule.value);

            if (player.Game.Eloquence.eloquence_pass == 0)
                player.Game.Eloquence.eloquence_pass = 1;

            return Data((int)ResultType.SUCCESS, player.Game.Eloquence.eloquence_pass);
        }

        /// <summary>返回数据</summary>
        private ASObject Data(int error,int guannum)
        {
            return new ASObject(new Dictionary<string, object> { { "result", error }, { "guanNum", guannum } });
        }
    }
}
