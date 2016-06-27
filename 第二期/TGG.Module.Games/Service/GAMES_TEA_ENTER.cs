using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Games.Service
{
    /// <summary>
    /// 游艺园花月茶道进入
    /// </summary>
    public class GAMES_TEA_ENTER
    {
        private static GAMES_TEA_ENTER _objInstance;

        /// <summary>
        /// GAMES_TEA_ENTER单体模式
        /// </summary>
        public static GAMES_TEA_ENTER GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAMES_TEA_ENTER());
        }

        /// <summary>游艺园花月茶道进入</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "GAMES_TEA_ENTER", "游艺园花月茶道进入");
#endif

            var player = session.Player;
            var game = player.Game;

            if (game.GameType == (int)GameType.ACTIONGAME)     //闯关模式
            {
                //验证剩余次数
                var b = Common.GetInstance().CheckCount((int)GameEnterType.花月茶道, player.UserExtend);
                if (!b) return Result((int)ResultType.GAME_COUNT_FAIL);
            }
            return ReadyData(game);
        }

        /// <summary>准备数据</summary>
        private ASObject ReadyData(GameItem game)
        {
            var photo = Variable.BASE_RULE.FirstOrDefault(m => m.id == "9005");    //图标信息
            if (photo == null || string.IsNullOrEmpty(photo.value)) return Result((int)ResultType.BASE_TABLE_ERROR);

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "30002");    //初始血量值
            if (rule == null) return Result((int)ResultType.BASE_TABLE_ERROR);

            var init = Common.GetInstance().InitPosition();
            var pos = Common.GetInstance().ConvertToString(init);   //初始化翻牌记录          
            var rp = Common.GetInstance().RandomPhoto(photo.value);   //随机图案信息
            var rnd = Common.GetInstance().ConvertToString(rp);

            var tea = Convert.ToInt32(rule.value);
            UpdateTea(game, pos, rnd, tea);      //初始化茶道数据

            return new ASObject(Common.GetInstance().BulidData((int)ResultType.SUCCESS, game.Tea.tea_pass, rp));
        }

        /// <summary>初始化茶道游戏数据</summary>
        private void UpdateTea(GameItem game, string pos, string rnd, int tea)
        {
            game.Tea.select_position = pos;
            game.Tea.all_cards = rnd;
            game.Tea.npc_tea = tea;
            game.Tea.user_tea = tea;
            if (game.Tea.tea_pass == 0) game.Tea.tea_pass = 1;
        }

        /// <summary>错误信息</summary>
        private ASObject Result(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }
    }
}
