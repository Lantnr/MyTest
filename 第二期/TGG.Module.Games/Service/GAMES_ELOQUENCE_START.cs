using System;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Games.Service
{
    /// <summary>
    /// 辩才游戏
    /// </summary>
    public class GAMES_ELOQUENCE_START
    {
        private static GAMES_ELOQUENCE_START _objInstance;

        /// <summary>
        /// GAMES_ELOQUENCE_START单体模式
        /// </summary>
        public static GAMES_ELOQUENCE_START GetInstance()
        {
            return _objInstance ?? (_objInstance = new GAMES_ELOQUENCE_START());
        }

        /// <summary>辩才游戏</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "GAMES_CALCULATE_START", "辩才小游戏");
#endif
            var cardtype = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "cardtype").Value);
            var game = session.Player.Game;
            var userextend = session.Player.UserExtend.CloneEntity();

            if (game.GameType == (int)GameType.ACTIONGAME)
            {
                //验证剩余次数
                var b = Common.GetInstance().CheckCount((int)GameEnterType.辩驳游戏, userextend);
                if (!b) return Error((int)ResultType.GAME_COUNT_FAIL);
                Common.GetInstance().CreateVariable(session.Player.User.id, (int)GameEnterType.辩驳游戏);
            }

            if (!CheckType(cardtype)) return Error((int)ResultType.FRONT_DATA_ERROR);
            var npccard = CardRandom();
            var base_tower = Variable.BASE_TOWERELOQUENCE.FirstOrDefault(m => m.myCard == cardtype && m.enemyCard == npccard);  //获取基表游戏数据
            if (base_tower == null) return Error((int)ResultType.BASE_TABLE_ERROR);

            game = GameProcess(game, base_tower);
            CheckResult(game, session.Player.User.id);
            return new ASObject(Common.GetInstance().BulidData((int)ResultType.SUCCESS, npccard, game.Eloquence.user_blood, game.Eloquence.npc_blood));
        }

        /// <summary>验证前端传递过来的卡牌类型</summary>
        private bool CheckType(int type)
        {
            return type == (int)DuplicateCardType.TONGUE_LOTUS || type == (int)DuplicateCardType.SOPHISTRY || type == (int)DuplicateCardType.MOTIVATION;
        }

        /// <summary>NPC 随机获取一张卡牌</summary>
        private int CardRandom()
        {
            var ran = RNG.Next((int)DuplicateCardType.TONGUE_LOTUS, (int)DuplicateCardType.SOPHISTRY);
            return ran;
        }

        /// <summary>分数处理</summary>
        private GameItem GameProcess(GameItem game, BaseTowerEloquence base_tower)     
        {
            game.Eloquence.user_blood = game.Eloquence.user_blood - base_tower.myScore;
            game.Eloquence.npc_blood = game.Eloquence.npc_blood - base_tower.enemyScore;
            return game;
        }

        /// <summary>推送游戏结果</summary>
        private void CheckResult(GameItem game, Int64 userid)
        {
            if (game.Eloquence.user_blood == 0 || game.Eloquence.npc_blood == 0)
            {
                if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
                var session = Variable.OnlinePlayer[userid] as TGGSession;
                if (session == null) return;
                if (game.Eloquence.user_blood == 0)   //闯关失败
                {
                    if (game.GameType == (int)GameType.ACTIONGAME)
                    {
                        var userextend = session.Player.UserExtend.CloneEntity();
                        userextend.eloquence_count++;
                        userextend.Save();
                        session.Player.UserExtend = userextend;
                        Common.GetInstance().PushReward(userid); //每日完成次数
                        Common.GetInstance().ClearVariable(userid);
                    }             
                    GAMES_PUSH.GetInstance().CommandStart(userid, (int)GameResultType.FAIL, game.Eloquence.eloquence_pass, (int)GameEnterType.辩驳游戏);
                    session.Player.Game.Eloquence.eloquence_pass = 0;
                }
                else
                {
                    //闯关成功
                    if (game.GameType == (int)GameType.ACTIONGAME)
                    {
                        Common.GetInstance().UpdateMax(userid, (int)GameEnterType.辩驳游戏, game.Eloquence.eloquence_pass);  //验证最高闯关次数
                    }
                    game.Eloquence.eloquence_pass += 1;
                    GAMES_PUSH.GetInstance().CommandStart(userid, (int)GameResultType.WIN, game.Eloquence.eloquence_pass, (int)GameEnterType.辩驳游戏);//推送胜利

                    var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "30001");
                    if (rule == null) return;

                    session.Player.Game.Eloquence.user_blood = Convert.ToInt32(rule.value);
                    session.Player.Game.Eloquence.npc_blood = Convert.ToInt32(rule.value);
                    //session.Player.Game.Eloquence.eloquence_pass = game.Eloquence.eloquence_pass;
                }
            }
        }

        /// <summary>返回错误结果</summary>
        private ASObject Error(int result)
        {
            return new ASObject(Common.GetInstance().BulidData(result, 0, 0, 0));
        }
    }
}
