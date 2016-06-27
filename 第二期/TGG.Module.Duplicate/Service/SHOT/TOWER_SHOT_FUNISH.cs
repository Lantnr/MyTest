using System;
using System.Linq;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using NewLife.Log;
using TGG.Core.Entity;

namespace TGG.Module.Duplicate.Service.SHOT
{
    /// <summary>
    /// 关卡结束
    /// </summary>
    public class TOWER_SHOT_FUNISH
    {
        private static TOWER_SHOT_FUNISH ObjInstance;

        /// <summary>TOWER_SHOT_FUNISH单体模式</summary>
        public static TOWER_SHOT_FUNISH GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TOWER_SHOT_FUNISH());
        }

        /// <summary>关卡结束</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "TOWER_SHOT_FUNISH", "关卡结束");
#endif
            var score = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "score").Value);
            var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);
            var isexit = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "isexit").Value);
#if DEBUG
            XTrace.WriteLine("前端提交分数:{0}", score);
#endif
            if (session.Player.UserExtend.shot_count == 0)
                return new ASObject(Common.GetInstance().BuilData((int)ResultType.CHALLENGE_FAIL, null));

            var shot = tg_duplicate_shot.GetEntityByUserId(session.Player.User.id);
            if (isexit == (int)ExitType.Exit) return UnPass(session, shot, score);
            var base_copyfamepass = Variable.BASE_COPYFAMEPASS.FirstOrDefault(m => m.type == type);

            return base_copyfamepass != null && score >= base_copyfamepass.score ? Pass(session, shot, score) : UnPass(session, shot, score);
        }

        /// <summary>分数达到通关</summary>
        private ASObject Pass(TGGSession session, tg_duplicate_shot shot, int score)
        {
            if (shot != null)
            {
                shot.score_current = score;
                shot.score_total += score;
            }
            else
            {
                shot = new tg_duplicate_shot { user_id = session.Player.User.id, score_current = score };
                shot.score_total += score;
            }
            try
            {
                shot.Save();
                return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS, session.Player.UserExtend.shot_count));
            }
            catch { return new ASObject(Common.GetInstance().BuilData((int)ResultType.DATABASE_ERROR, null)); }
        }

        /// <summary>分数未达到结束通关</summary>
        private ASObject UnPass(TGGSession session, tg_duplicate_shot shot, int score)
        {
            var exchange = 0;
            if (shot != null)
            {
                exchange = shot.score_total + score;
                shot.score_current = 0;
                shot.score_total = 0;
            }
            else
            {
                exchange = score;
                shot = new tg_duplicate_shot { user_id = session.Player.User.id };
            }
            try
            {
                var player = session.Player.CloneEntity();
                var user = player.User;
                var extend = player.UserExtend;
#if DEBUG
                XTrace.WriteLine("兑换前分数:{0}", exchange);
#endif
                user.spirit = tg_user.IsSpiritMax(user.spirit, ConventSpirit(exchange));
#if DEBUG
                XTrace.WriteLine("兑换后魂:{0}", user.spirit);
#endif
                if (extend.shot_count != 0)
                    extend.shot_count -= 1;
                user.Save();
                shot.Save();
                extend.Save();
                //资源兑换
                session.Player.User = user;
                session.Player.UserExtend = extend;
                (new Share.User()).REWARDS_API((int)GoodsType.TYPE_SPIRIT, session.Player.User);

                (new Share.DaMing()).CheckDaMing(session.Player.User.id, (int)DaMingType.猎魂);
                return new ASObject(Common.GetInstance().BuilData((int)ResultType.CHALLENGE_FAIL, session.Player.UserExtend.shot_count));
            }
            catch { return new ASObject(Common.GetInstance().BuilData((int)ResultType.DATABASE_ERROR, null)); }
        }

        /// <summary>分数兑换魂</summary>
        private int ConventSpirit(Int32 score)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "9001");
            var rule_max = Variable.BASE_RULE.FirstOrDefault(q => q.id == "9010");
            if (rule == null || rule_max == null) return 0;
            var temp = rule.value;
            temp = temp.Replace("score", score.ToString("0.00"));
            var result = CommonHelper.EvalExpress(temp);

            var _score = Convert.ToInt32(result);
            var max = Convert.ToInt32(rule_max.value);
            _score = _score > max ? max : _score;

            return _score;
        }

    }
}
