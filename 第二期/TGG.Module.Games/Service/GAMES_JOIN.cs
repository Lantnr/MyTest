using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Games;
using TGG.SocketServer;

namespace TGG.Module.Games.Service
{
    /// <summary>
    /// 进入游戏
    /// </summary>
    public class GAMES_JOIN
    {
        private static GAMES_JOIN _objInstance;

        /// <summary>
        /// GAMES_JOIN单体模式
        /// </summary>
        public static GAMES_JOIN GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAMES_JOIN());
        }

        /// <summary>进入游戏</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "GAMES_JOIN", "进入游戏");
#endif
            var game = tg_game.GetEntityByUserId(session.Player.User.id);
            var userextend = tg_user_extend.GetEntityByUserId(session.Player.User.id);
            var baselist = Variable.BASE_YOUYIYUAN;
            if (game == null)
            {
                game = new tg_game
                {
                    user_id = session.Player.User.id
                };
                if (!tg_game.GetInsert(game))
                    return Error((int)ResultType.DATABASE_ERROR);
            }
            if (!baselist.Any()) return Error((int)ResultType.BASE_TABLE_ERROR);
            return Result(game, userextend, baselist);
        }

        /// <summary>返回前端结果</summary>
        /// <param name="game">游艺园实体</param>
        /// <param name="userextend">拓展表实体</param>
        /// <param name="list">游艺园基表集合</param>
        /// <returns></returns>
        private ASObject Result(tg_game game, tg_user_extend userextend, List<BaseYouYiYuan> list)
        {
            var volist = new List<YouYiyuanVo>();
            var youvo =Common.GetInstance(). CreateGame((int)GameEnterType.猎魂, game, userextend, list);
            if (youvo == null) return Error((int)ResultType.BASE_TABLE_ERROR);
            volist.Add(youvo);
            youvo = Common.GetInstance().CreateGame((int)GameEnterType.猜宝游戏, game, userextend, list);
            if (youvo == null) return Error((int)ResultType.BASE_TABLE_ERROR);
            volist.Add(youvo);
            youvo = Common.GetInstance().CreateGame((int)GameEnterType.老虎机, game, userextend, list);
            if (youvo == null) return Error((int)ResultType.BASE_TABLE_ERROR);
            volist.Add(youvo);
            youvo = Common.GetInstance().CreateGame((int)GameEnterType.花月茶道, game, userextend, list);
            if (youvo == null) return Error((int)ResultType.BASE_TABLE_ERROR);
            volist.Add(youvo);
            youvo = Common.GetInstance().CreateGame((int)GameEnterType.辩驳游戏, game, userextend, list);
            if (youvo == null) return Error((int)ResultType.BASE_TABLE_ERROR);
            volist.Add(youvo);
            return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS, volist, game.week_max_pass, userextend.game_finish_count, userextend.game_receive));
        }
        

        private ASObject Error(int error)
        {
            return new ASObject(Common.GetInstance().BuilData(error, null, 0, 0, 0));
        }
    }
}
