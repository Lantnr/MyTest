using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Games.Service
{
    /// <summary>
    /// 游艺园花月茶道开始
    /// </summary>
    public class GAMES_TEA_START
    {
        private static GAMES_TEA_START _objInstance;

        /// <summary>
        /// GAMES_TEA_START单体模式
        /// </summary>
        public static GAMES_TEA_START GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAMES_TEA_START());
        }

        /// <summary>游艺园花月茶道开始</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "GAMES_TEA_START", "游艺园花月茶道开始");
#endif
                var loc = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "loc").Value.ToString());         //获取玩家查看的牌的位置（1-30）
                var game = session.Player.Game;

                if (game.GameType == (int)GameType.ACTIONGAME)     //闯关模式
                {
                    //验证剩余次数
                    var b = Common.GetInstance().CheckCount((int)GameEnterType.花月茶道, session.Player.UserExtend);
                    if (!b) return Result((int)ResultType.GAME_COUNT_FAIL);
                    Common.GetInstance().CreateVariable(session.Player.User.id, (int)GameEnterType.花月茶道);
                }

                if (game.Tea.select_position == " " || game.Tea.all_cards == " ")
                    return Result((int)ResultType.GAME_DATA_ERROR);     //验证茶道翻开卡牌及随机好的卡牌信息

                var _position = ConvertToList(game.Tea.select_position);
                if (_position[loc - 1] != 0) return Result((int)ResultType.TOWER_CARD_FLOPED);        //验证卡牌是否翻过

                return ReadyData(game, session.Player.User.id, _position, loc);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>处理茶道数据</summary>
        private ASObject ReadyData(GameItem game, Int64 userid, List<int> _position, int loc)
        {
            var _record = ConvertToList(game.Tea.all_cards);
            var value = _record[loc - 1];                            //获取玩家卡牌图案
            _position[loc - 1] = value;                               //更新翻牌信息

            var nloc = RandomPosition(_position);
            var nvalue = _record[nloc];
            _position[nloc] = nvalue;
            game.Tea.select_position = Common.GetInstance().ConvertToString(_position);   //npc翻牌后更新牌位置

            var rule = Variable.BASE_TOWERTEA.FirstOrDefault(m => m.myIcon == value && m.enemyIcon == nvalue);
            if (rule == null) return Result((int)ResultType.BASE_TABLE_ERROR);     //验证数据

            var tea = Variable.BASE_RULE.FirstOrDefault(m => m.id == "30002");
            if (tea == null) return Result((int)ResultType.BASE_TABLE_ERROR);
            var l = Convert.ToInt32(tea.value);      //初始茶席值（上限值）

            game.Tea.npc_tea += rule.enemyScore;
            if (game.Tea.npc_tea > l) game.Tea.npc_tea = l;
            game.Tea.user_tea += rule.myScore;
            if (game.Tea.user_tea > l) game.Tea.user_tea = l;        //验证玩家星数是否超过上限
            return IsClearance(game, userid, _position, loc, nloc, value, nvalue);
        }

        /// <summary>判定是否通关</summary>
        private ASObject IsClearance(GameItem game, Int64 userid, List<int> pInfo, int loc, int nloc, int photo, int nphoto)
        {
            if (game.Tea.user_tea <= 0)  //通关失败
            { if (!ResultData(userid, (int)GameResultType.FAIL, game)) return Result((int)ResultType.DATABASE_ERROR); }
            else
            {
                if (game.Tea.npc_tea <= 0) //通关成功
                { if (!ResultData(userid, (int)GameResultType.WIN, game)) return Result((int)ResultType.DATABASE_ERROR); }
                else
                {
                    if (!CheckCards(pInfo).Any())  //通关成功
                    { if (!ResultData(userid, (int)GameResultType.WIN, game)) return Result((int)ResultType.DATABASE_ERROR); }
                }
            }
            return new ASObject(Common.GetInstance().TeaFlopData((int)ResultType.SUCCESS, game.Tea.npc_tea, game.Tea.user_tea, loc, photo, nloc + 1, nphoto));
        }

        /// <summary>结果数据处理</summary>
        private bool ResultData(Int64 userid, int type, GameItem game)
        {
            if (type == (int)GameResultType.FAIL)
            {
                if (game.GameType == (int)GameType.ACTIONGAME)
                {
                    if (!CheckDay(userid)) return false;                 //处理使用次数  每日完成度
                    Common.GetInstance().ClearVariable(userid);
                }
                GAMES_PUSH.GetInstance().CommandStart(userid, (int)GameResultType.FAIL, game.Tea.tea_pass, (int)GameEnterType.花月茶道);//推送
                UpdateData(game);
            }
            else
            {
                if (game.GameType == (int)GameType.ACTIONGAME)   //闯关模式验证最高闯关次数
                {
                    var b = Common.GetInstance().UpdateMax(userid, (int)GameEnterType.花月茶道, game.Tea.tea_pass);
                    if (!b) return false;
                }
                game.Tea.tea_pass++;
                GAMES_PUSH.GetInstance().CommandStart(userid, (int)GameResultType.WIN, game.Tea.tea_pass, (int)GameEnterType.花月茶道);  //推送
            }
            return true;
        }

        /// <summary>转换卡牌信息</summary>
        private List<int> ConvertToList(string select)
        {
            var list = new List<int>();
            if (string.IsNullOrEmpty(select)) return list;
            if (!select.Contains("_")) return list;
            var n = select.Split("_").ToList();
            list.AddRange(n.Select(item => Convert.ToInt32(item)));
            return list;
        }

        /// <summary>npc随机翻牌位置</summary>
        private int RandomPosition(List<int> flopinfo)
        {
            var listIndex = new List<int>();
            for (var i = 0; i < flopinfo.Count; i++)
            {
                if (flopinfo[i] != 0) continue;
                listIndex.Add(i);
            }
            var n = RNG.Next(0, listIndex.Count - 1);
            return listIndex[n];
        }

        /// <summary>检查未翻的卡牌数量</summary>
        private IEnumerable<int> CheckCards(List<int> pInfo)
        {
            var list = new List<int>();
            if (!pInfo.Any()) return list;
            list.AddRange(pInfo.Where(item => item == 0));
            return list;
        }

        /// <summary>初始化茶道数据</summary>
        private void UpdateData(GameItem game)
        {
            game.Tea.all_cards = " ";
            game.Tea.select_position = " ";
            game.Tea.npc_tea = 0;
            game.Tea.user_tea = 0;
            game.Tea.tea_pass = 0;
        }

        /// <summary>处理每日次数</summary>
        private bool CheckDay(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return false;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return false;

            var ex = session.Player.UserExtend.CloneEntity();
            ex.tea_count++;
            if (!tg_user_extend.GetUpdate(ex)) return false;
            session.Player.UserExtend = ex;

            Common.GetInstance().PushReward(userid);   //每日完成度统计
            return true;
        }

        /// <summary>错误信息</summary>
        private ASObject Result(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }
    }
}
