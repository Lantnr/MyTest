using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Games;

namespace TGG.Module.Games.Service
{
    public partial class Common
    {
        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BulidData(int result, int type, int userblood, int npcblood)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"type", type},
                {"userBlood",userblood},
                {"npcBlood",npcblood},
            };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilData(int result, int count)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"count", count},
            };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilData(int result, List<YouYiyuanVo> games, int tatal, int count, int state)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"games",  ConvertListAsObject(games)},
                {"total", tatal},
                {"count", count},
                {"state", state},
            };
            return dic;
        }

        /// <summary>ToAsObject</summary>
        public List<ASObject> ConvertListAsObject(List<YouYiyuanVo> games)
        {
            return games.Select(AMFConvert.ToASObject).ToList();
        }

        /// <summary>错误信息</summary>
        public ASObject Result(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }

        /// <summary>创建小游戏全局变量</summary>
        /// <param name="userid">玩家id</param>
        /// <param name="type">游戏类型</param>
        public void CreateVariable(Int64 userid, int type)
        {
            var game = Variable.GameInfo.FirstOrDefault(m => m.userid == userid);
            if (game == null)
            {
                game = new Variable.UserGameInfo() { userid = userid, type = type };
                Variable.GameInfo.Add(game);
            }
            else
            {
                if (game.type == 0)
                    game.type = type;
            }
        }

        /// <summary>清理全局变量</summary>
        /// <param name="userid">玩家id</param>
        public void ClearVariable(Int64 userid)
        {
            var game = Variable.GameInfo.FirstOrDefault(m => m.userid == userid);
            if (game == null) return;
            game.type = 0;
        }

        /// <summary>创建游艺园vo</summary>
        /// <param name="type">游戏类型</param>
        /// <param name="tggame">游艺园实体</param>
        /// <param name="userextend">拓展表实体</param>
        /// <param name="list">游艺园基表集合</param>
        /// <returns>游艺园vo</returns>
        public YouYiyuanVo CreateGame(int type, tg_game tggame, tg_user_extend userextend, IEnumerable<BaseYouYiYuan> list)
        {
            int count;
            var game = list.FirstOrDefault(m => m.type == type);
            if (game == null) return null;
            var youvo = new YouYiyuanVo();
            switch (type)
            {
                case (int)GameEnterType.辩驳游戏:
                    {
                        count = game.num - userextend.eloquence_count;
                        youvo = EntityToVo.ToYouYiyuanVo(type, count, tggame.eloquence_max_pass);
                    }
                    break;
                case (int)GameEnterType.花月茶道:
                    {
                        count = game.num - userextend.tea_count;
                        youvo = EntityToVo.ToYouYiyuanVo(type, count, tggame.tea_max_pass);
                    }
                    break;
                case (int)GameEnterType.老虎机:
                    {
                        count = game.num - userextend.calculate_count;
                        youvo = EntityToVo.ToYouYiyuanVo(type, count, tggame.calculate_max_pass);
                    }
                    break;
                case (int)GameEnterType.猜宝游戏:
                    {
                        count = game.num - userextend.ninjutsu_count;
                        youvo = EntityToVo.ToYouYiyuanVo(type, count, tggame.ninjutsu_max_pass);
                    }
                    break;
                case (int)GameEnterType.猎魂:
                    {
                        count = game.num - userextend.ball_count;
                        youvo = EntityToVo.ToYouYiyuanVo(type, count, tggame.spirit_max_pass);
                    }
                    break;
            }
            return youvo;
        }
    }
}
