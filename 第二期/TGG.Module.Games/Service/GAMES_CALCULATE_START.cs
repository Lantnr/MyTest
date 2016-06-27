using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.Games.Service
{
    /// <summary>
    /// 游艺园算术游戏开始
    /// </summary>
    public class GAMES_CALCULATE_START
    {
        private static GAMES_CALCULATE_START _objInstance;

        /// <summary>
        /// GAMES_CALCULATE_START单体模式
        /// </summary>
        public static GAMES_CALCULATE_START GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAMES_CALCULATE_START());
        }

        /// <summary>游艺园算术游戏开始</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "GAMES_CALCULATE_START", "游艺园算术游戏开始");
#endif
                var list = data.FirstOrDefault(m => m.Key == "values").Value as object[];  //数字集合
                if (list == null) return Result((int)ResultType.FRONT_DATA_ERROR);

                var game = session.Player.Game;
                var number = list.Select(Convert.ToInt32).ToList();

                if (game.GameType == (int)GameType.ACTIONGAME)     //闯关模式
                {
                    //验证剩余次数
                    var b = Common.GetInstance().CheckCount((int)GameEnterType.老虎机, session.Player.UserExtend);
                    if (!b) return Result((int)ResultType.GAME_COUNT_FAIL);
                    Common.GetInstance().CreateVariable(session.Player.User.id, (int)GameEnterType.老虎机);
                }

                return ResultData(game, session.Player.User.id, number);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>处理数据</summary>
        public ASObject ResultData(GameItem game, Int64 userid, List<Int32> nb)
        {
            if (nb[0] == nb[1] && nb[0] == nb[2])     //通关
            {
                if (game.GameType == (int)GameType.ACTIONGAME)
                {
                    var b = Common.GetInstance().UpdateMax(userid, (int)GameEnterType.老虎机, game.Calculate.calculate_pass);
                    if (!b) return Result((int)ResultType.DATABASE_ERROR);
                }
                game.Calculate.calculate_pass++;
                GAMES_PUSH.GetInstance().CommandStart(userid, (int)GameResultType.WIN, game.Calculate.calculate_pass, (int)GameEnterType.老虎机);//推送
            }
            else
            {
                if (game.GameType == (int)GameType.ACTIONGAME)
                {
                    if (!CheckDay(userid)) return Result((int)ResultType.DATABASE_ERROR);
                    Common.GetInstance().ClearVariable(userid);
                }
                GAMES_PUSH.GetInstance().CommandStart(userid, (int)GameResultType.FAIL, game.Calculate.calculate_pass, (int)GameEnterType.老虎机);//推送
                game.Calculate.calculate_pass = 0;
            }
            return Result((int)ResultType.SUCCESS);
        }

        /// <summary>处理每日完成度</summary>
        private bool CheckDay(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return false;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return false;

            var ex = session.Player.UserExtend.CloneEntity();
            ex.calculate_count++;
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
