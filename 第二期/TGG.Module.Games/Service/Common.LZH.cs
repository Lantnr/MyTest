using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Games.Service
{
    /// <summary>
    /// 游艺园公共方法
    /// </summary>
    public partial class Common
    {
        #region 组装数据
        /// <summary>花月茶道进入</summary>
        public Dictionary<String, Object> BulidData(int result, int pass, List<int> photo)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "pass", pass }, { "photoStateList", photo }, };
            return dic;
        }

        /// <summary>花月茶道翻牌</summary>
        public Dictionary<String, Object> TeaFlopData(int result, int npcTea, int userTea, int loc, int uPhoto, int nLoc, int nPhoto)
        {
            var dic = new Dictionary<string, object>
            {
                { "result", result }, { "npcTea", npcTea }, { "userTea", userTea }, { "loc", loc }, { "userPhoto", uPhoto }, { "npcLoc", nLoc }, { "npcPhoto", nPhoto }
            };
            return dic;
        }

        /// <summary>忍术游戏进入数据</summary>
        public Dictionary<String, Object> NinjaEnterData(int result, int position)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "position", position }, };
            return dic;
        }

        #endregion

        /// <summary> 验证剩余次数</summary>
        /// <param name="type">小游戏类型</param>
        /// <param name="ex">tg_user_extend</param>
        /// <returns></returns>
        public bool CheckCount(int type, tg_user_extend ex)
        {
            var rule = Variable.BASE_YOUYIYUAN.FirstOrDefault(m => m.type == type);
            if (rule == null) return false;
            var total = rule.num;    //闯关总次数

            switch (type)
            {
                case (int)GameEnterType.花月茶道: if (ex.tea_count >= total) return false; break;
                case (int)GameEnterType.辩驳游戏: if (ex.eloquence_count >= total) return false; break;
                case (int)GameEnterType.老虎机: if (ex.calculate_count >= total) return false; break;
                case (int)GameEnterType.猜宝游戏: if (ex.ninjutsu_count >= total) return false; break;
                case (int)GameEnterType.猎魂: if (ex.ball_count >= total) return false; break;
                default: return false;
            }
            return true;
        }

        /// <summary>验证最高闯关次数 </summary>
        /// /// <param name="userid">user_id</param>
        /// <param name="type">小游戏类型</param>
        /// <param name="pass">当前过关关卡</param>
        /// <returns></returns>
        public bool UpdateMax(Int64 userid, int type, int pass)
        {
            var y = tg_game.GetByUserId(userid);
            if (y == null) return false;
            if (pass <= 0) return false;

            int val;
            switch (type)
            {
                case (int)GameEnterType.花月茶道: val = pass - y.tea_max_pass;
                    if (val <= 0) return true;
                    y.tea_max_pass += val;
                    y.week_max_pass += val; break;

                case (int)GameEnterType.辩驳游戏: val = pass - y.eloquence_max_pass;
                    if (val <= 0) return true;
                    y.eloquence_max_pass += val;
                    y.week_max_pass += val; break;

                case (int)GameEnterType.老虎机: val = pass - y.calculate_max_pass;
                    if (val <= 0) return true;
                    y.calculate_max_pass += val;
                    y.week_max_pass += val; break;

                case (int)GameEnterType.猜宝游戏: val = pass - y.ninjutsu_max_pass;
                    if (val <= 0) return true;
                    y.ninjutsu_max_pass += val;
                    y.week_max_pass += val; break;

                case (int)GameEnterType.猎魂: val = pass - y.spirit_max_pass;
                    if (val <= 0) return true;
                    y.spirit_max_pass += val;
                    y.week_max_pass += val; break;

                default: return false;
            }
            return y.Update() > 0;
        }

        /// <summary>检测每日完成度 （闯关模式）</summary>
        /// <param name="userid">userid</param>
        public void PushReward(Int64 userid)
        {
            var s = Variable.OnlinePlayer.ContainsKey(userid);
            if (!s) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            var ex = session.Player.UserExtend.CloneEntity();

            var finish = Variable.BASE_RULE.FirstOrDefault(m => m.id == "30004");
            if (finish == null) return;
            var total = Convert.ToInt32(finish.value);

            if (ex.game_finish_count >= total || ex.game_receive != (int)GameRewardType.TYPE_UNREWARD) return;
            ex.game_finish_count++;

            if (ex.game_finish_count < total)
            {
                ex.Update();
                session.Player.UserExtend = ex;
                return;
            }

            ex.game_receive = (int)GameRewardType.TYPE_CANREWARD;
            ex.Update();
            session.Player.UserExtend = ex;

            //推送每日完成度
            var data = new ASObject(new Dictionary<string, object> { { "state", ex.game_receive } });
            Push(userid, data);
        }

        /// <summary>
        /// 推送奖励为可领取
        /// </summary>
        public void Push(Int64 userid, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "GAMES_REWARD_PUSH", "每日完成度推送");
#endif
            var s = Variable.OnlinePlayer.ContainsKey(userid);
            if (!s) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;

            var pv = session.InitProtocol((int)ModuleNumber.GAMES, (int)GameCommand.GAMES_REWARD_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }

        /// <summary>打乱位置后的图形集合</summary>
        public List<int> RandomPhoto(string photo)
        {
            var list = new List<int>();
            if (!photo.Contains("_")) return list;
            var n = photo.Split("_").ToList();
            if (n.Count() < 30) return list;
            list.AddRange(n.Select(item => Convert.ToInt32(item)));
            var number = RNG.Next(0, list.Count - 1, 30);
            return number.Select(item => list[item]).ToList();
        }

        /// <summary>初始化图案位置</summary>
        public List<int> InitPosition()
        {
            var list = new List<int>();
            for (var i = 0; i < 30; i++)
            {
                list.Add(0);
            }
            return list;
        }

        /// <summary>将位置转换为字符串</summary>
        public string ConvertToString(List<int> position)
        {
            var record = "";
            foreach (var item in position)
            {
                record += Convert.ToString(item) + "_";
            }
            return record;
        }
    }
}
