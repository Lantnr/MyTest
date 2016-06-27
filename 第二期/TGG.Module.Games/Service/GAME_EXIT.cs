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
    /// 小游戏退出
    /// </summary>
    public class GAME_EXIT
    {
        private static GAME_EXIT _objInstance;

        /// <summary>
        /// GAME_EXIT单体模式
        /// </summary>
        public static GAME_EXIT GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAME_EXIT());
        }

        /// <summary>小游戏退出</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "GAME_EXIT", "小游戏退出");
#endif
            var gt = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "gametype").Value.ToString());    //游戏类型         
            var game = session.Player.Game;

            if (gt != (int)GameEnterType.辩驳游戏 && gt != (int)GameEnterType.老虎机 && gt != (int)GameEnterType.花月茶道 &&
                gt != (int)GameEnterType.猜宝游戏 && gt != (int)GameEnterType.猎魂)
                return Result((int)ResultType.FRONT_DATA_ERROR);         //验证小游戏类型

            if (game.GameType == (int)GameType.ACTIONGAME)
            {
                //验证剩余次数
                var b = Common.GetInstance().CheckCount(gt, session.Player.UserExtend);
                if (!b) return Result((int)ResultType.FRONT_DATA_ERROR);   //验证次数为0后，前端是否发退出游戏指令

                if (!CheckDay(session, gt)) return Result((int)ResultType.DATABASE_ERROR);
                Common.GetInstance().ClearVariable(session.Player.User.id);
                Push(session.Player.User.id, game, gt);
            }

            switch (gt)
            {
                case (int)GameEnterType.辩驳游戏:
                    game.Eloquence.eloquence_pass = 0;
                    game.Eloquence.npc_blood = 0;
                    game.Eloquence.user_blood = 0; break;
                case (int)GameEnterType.老虎机: game.Calculate.calculate_pass = 0; break;
                case (int)GameEnterType.花月茶道:
                    game.Tea.all_cards = " ";
                    game.Tea.select_position = " ";
                    game.Tea.npc_tea = 0;
                    game.Tea.user_tea = 0;
                    game.Tea.tea_pass = 0; break;
                case (int)GameEnterType.猜宝游戏: game.Ninjutsu.ninjutsu_pass = 0; break;
                case (int)GameEnterType.猎魂: game.Spirit.spirit_pass = 0; break;
            }
            return Result((int)ResultType.SUCCESS);
        }

        /// <summary>处理每日完成度</summary>
        private bool CheckDay(TGGSession session, int gt)
        {
            var ex = session.Player.UserExtend.CloneEntity();

            switch (gt)
            {
                case (int)GameEnterType.辩驳游戏: ex.eloquence_count++; break;
                case (int)GameEnterType.老虎机: ex.calculate_count++; break;
                case (int)GameEnterType.花月茶道: ex.tea_count++; break;
                case (int)GameEnterType.猜宝游戏: ex.ninjutsu_count++; break;
                case (int)GameEnterType.猎魂: ex.ball_count++; break;
            }

            if (!tg_user_extend.GetUpdate(ex)) return false;
            session.Player.UserExtend = ex;

            Common.GetInstance().PushReward(ex.user_id);   //每日完成度统计
            return true;
        }

        /// <summary>推送 Vo</summary>
        private void Push(Int64 userid, GameItem game, int gt)
        {
            switch (gt)
            {
                case (int)GameEnterType.辩驳游戏: GAMES_PUSH.GetInstance().CommandStart(userid, (int)GameResultType.FAIL, game.Eloquence.eloquence_pass, gt); break;
                case (int)GameEnterType.老虎机: GAMES_PUSH.GetInstance().CommandStart(userid, (int)GameResultType.FAIL, game.Calculate.calculate_pass, gt); break;
                case (int)GameEnterType.花月茶道: GAMES_PUSH.GetInstance().CommandStart(userid, (int)GameResultType.FAIL, game.Tea.tea_pass, gt); break;
                case (int)GameEnterType.猜宝游戏: ; GAMES_PUSH.GetInstance().CommandStart(userid, (int)GameResultType.FAIL, game.Ninjutsu.ninjutsu_pass, gt); break;
                case (int)GameEnterType.猎魂: ; GAMES_PUSH.GetInstance().CommandStart(userid, (int)GameResultType.FAIL, game.Spirit.spirit_pass, gt); break;
            }
        }

        /// <summary>返回数据</summary>
        private ASObject Result(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }
    }
}
