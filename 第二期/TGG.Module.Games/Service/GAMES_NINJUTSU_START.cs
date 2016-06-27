using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Games.Service
{
    /// <summary>
    /// 游艺园忍术游戏开始
    /// </summary>
    public class GAMES_NINJUTSU_START
    {
        private static GAMES_NINJUTSU_START _objInstance;

        /// <summary>
        /// GAMES_NINJUTSU_START单体模式
        /// </summary>
        public static GAMES_NINJUTSU_START GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAMES_NINJUTSU_START());
        }

        /// <summary>游艺园忍术游戏开始</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "GAMES_NINJUTSU_START", "游艺园忍术游戏开始");
#endif
                var pos = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "position").Value.ToString());
                var correct = session.Player.Position.position;   //色子的正确位置
                var player = session.Player;
                var game = player.Game;

                if (game.GameType == (int)GameType.ACTIONGAME)     //闯关模式
                {
                    //验证剩余次数
                    var b = Common.GetInstance().CheckCount((int)GameEnterType.猜宝游戏, player.UserExtend);
                    if (!b) return Result((int)ResultType.GAME_COUNT_FAIL);
                    Common.GetInstance().CreateVariable(session.Player.User.id, (int)GameEnterType.猜宝游戏);
                }

                if (pos == correct)  //通关
                {
                    if (game.GameType == (int)GameType.ACTIONGAME)   //闯关模式
                    {
                        var b = Common.GetInstance().UpdateMax(player.User.id, (int)GameEnterType.猜宝游戏, game.Ninjutsu.ninjutsu_pass);
                        if (!b) return Result((int)ResultType.DATABASE_ERROR);
                    }
                    game.Ninjutsu.ninjutsu_pass++;
                    GAMES_PUSH.GetInstance().CommandStart(player.User.id, (int)GameResultType.WIN, game.Ninjutsu.ninjutsu_pass, (int)GameEnterType.猜宝游戏);//推送
                }
                else
                {
                    if (game.GameType == (int)GameType.ACTIONGAME)
                    {
                        if (!CheckDay(session)) return Result((int)ResultType.DATABASE_ERROR);
                        Common.GetInstance().ClearVariable(player.User.id);
                    }
                    GAMES_PUSH.GetInstance().CommandStart(player.User.id, (int)GameResultType.FAIL, game.Ninjutsu.ninjutsu_pass, (int)GameEnterType.猜宝游戏);//推送
                    game.Ninjutsu.ninjutsu_pass = 0;
                }
                return Result((int)ResultType.SUCCESS);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>处理每次完成度</summary>
        private bool CheckDay(TGGSession session)
        {
            var ex = session.Player.UserExtend.CloneEntity();

            ex.ninjutsu_count++;
            if (!tg_user_extend.GetUpdate(ex)) return false;
            session.Player.UserExtend = ex;

            Common.GetInstance().PushReward(ex.user_id);   //每日完成度统计
            return true;
        }

        /// <summary>错误信息</summary>
        private ASObject Result(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }
    }
}
