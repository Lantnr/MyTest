using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Games.Service
{
    /// <summary>
    /// 推送游戏结果
    /// </summary>
    public class GAMES_PUSH
    {
        private static GAMES_PUSH _objInstance;

        /// <summary>
        /// GAMES_PUSH单体模式
        /// </summary>
        public static GAMES_PUSH GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAMES_PUSH());
        }

        /// <summary>推送游戏结果 </summary>
        /// <param name="userid">玩家id</param>
        /// <param name="type">游戏结果</param>
        /// <param name="pass">当前闯关关卡</param>
        /// <param name="gametype">游戏类型</param>
        public void CommandStart(Int64 userid, int type, int pass, int gametype)
        {
            var baselist = Variable.BASE_YOUYIYUAN;
            var game = tg_game.GetEntityByUserId(userid);
            var userextend = tg_user_extend.GetEntityByUserId(userid);           
           
            if (!baselist.Any() || game == null || userextend == null) return;           
            var youvo = Common.GetInstance().CreateGame(gametype, game, userextend, baselist);
            if (youvo == null) return;
            var dic = new Dictionary<string, object>
            {
                { "type", type },
                { "pass", pass },
                { "gametype", gametype },
                { "game", youvo },
                { "total", game.week_max_pass },
                { "count", userextend.game_finish_count }
            };
            GamePush(userid, new ASObject(dic));
        }

        /// <summary>游戏推送</summary>
        private static void GamePush(Int64 userid, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "GAMES_PUSH", "推送游戏结果");
#endif
            if (!Variable.OnlinePlayer.ContainsKey(userid))
                return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if(session==null)return;
            var pv = session.InitProtocol((int)ModuleNumber.GAMES, (int)Core.Enum.Command.GameCommand.GAMES_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }
    }
}
