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
    /// 猎魂游戏
    /// </summary>
    public class GAMES_SHOT_FINISH
    {
        private static GAMES_SHOT_FINISH _objInstance;

        /// <summary>
        /// GAMES_SHOT_FINISH单体模式
        /// </summary>
        public static GAMES_SHOT_FINISH GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAMES_SHOT_FINISH());
        }

        /// <summary>猎魂游戏</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "GAMES_SHOT_FINISH", "猎魂游戏");
#endif
            var score = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "score").Value);
            var pass = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "pass").Value);
            var player = session.Player.CloneEntity();

            var copyfamepass = Variable.BASE_COPYFAMEPASS.FirstOrDefault(m => m.type == pass);    //查询打气球基表数据
            if (copyfamepass == null) return Error((int)ResultType.BASE_TABLE_ERROR);

            var game = session.Player.Game;
            if (game.GameType == (int) GameType.ACTIONGAME) //闯关模式
            {
                //验证剩余次数
                var b = Common.GetInstance().CheckCount((int)GameEnterType.猎魂, player.UserExtend);
                if (!b) return Common.GetInstance().Result((int)ResultType.GAME_COUNT_FAIL);
                Common.GetInstance().CreateVariable(session.Player.User.id, (int)GameEnterType.猎魂);
            }
           

            if (score >= copyfamepass.score)  //过关处理
            {
                if (game.GameType == (int)GameType.ACTIONGAME)
                    Common.GetInstance().UpdateMax(session.Player.User.id, (int)GameEnterType.猎魂, game.Spirit.spirit_pass);
                game.Spirit.spirit_pass += 1;
                GAMES_PUSH.GetInstance().CommandStart(session.Player.User.id, (int)GameResultType.WIN, game.Spirit.spirit_pass, (int)GameEnterType.猎魂);
                return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS, game.Spirit.spirit_pass));
            }
            return UnPass(session.Player.User.id);
        }

        /// <summary>分数未达到结束通关</summary>
        private ASObject UnPass(Int64 userid)
        {
            //验证玩家是否在线
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return Error((int)ResultType.BASE_PLAYER_OFFLINE_ERROR);
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return Error((int)ResultType.BASE_PLAYER_OFFLINE_ERROR);

            var game = session.Player.Game;
            if (game.GameType == (int)GameType.ACTIONGAME)
            {
                var extend = session.Player.UserExtend;
                extend.ball_count++;
                extend.Save();
                session.Player.UserExtend = extend;
                Common.GetInstance().PushReward(userid);
                Common.GetInstance().ClearVariable(userid);
            }

            //推送失败
            GAMES_PUSH.GetInstance().CommandStart(userid, (int)GameResultType.FAIL, game.Spirit.spirit_pass, (int)GameEnterType.猎魂);
            return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS, game.Spirit.spirit_pass));
        }

        private ASObject Error(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }

    }
}
