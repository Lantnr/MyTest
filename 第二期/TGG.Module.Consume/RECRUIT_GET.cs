using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using System.Transactions;

namespace TGG.Module.Consume
{
    public class RECRUIT_GET : IConsume
    {
        public ASObject Execute(long user_id, ASObject data)
        {
            if (!Variable.OnlinePlayer.ContainsKey(user_id))
                return CommonHelper.ErrorResult(ResultType.CHAT_NO_ONLINE);
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            return CommandStart(session, data);
        }

        /// <summary> 武将抽取 </summary>
        private ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (session.Player.Bag.BagIsFull) return CommonHelper.ErrorResult((int)ResultType.BAG_ISFULL_ERROR);
            var position = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "position").Value);

            if (position > 10 || position < 1)
                return CommonHelper.ErrorResult((int)ResultType.ROLE_RECRUIT_POSTITION_ERROR);
            if (tg_role_recruit.GetFindIsExist(session.Player.User.id, position))
                return CommonHelper.ErrorResult((int)ResultType.ROLE_RECRUIT_ISEXIST_ERROR);

            var player = session.Player.CloneEntity();
            var identity = player.Role.Kind.role_identity; //随机身份卡牌

            var base_rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7001");  //读取基表
            if (base_rule == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            var cost = Convert.ToInt64(base_rule.value);//金钱消耗
            var curent = player.User.coin;

            if (curent < cost) return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_COIN_ERROR);
            var surplus = curent - cost;

            var grades = RandomGrade(identity);//金钱足够后进行卡牌生成

            var list_card = Variable.BASE_PROP.Where(m => m.grade == grades && m.typeSub == 1).ToList(); //获取道具表家臣卡牌集合
            var card = RandomCard(list_card);
            #region 组装数据

            var recuit = new tg_role_recruit
            {
                position = position,
                prop_id = card.id,
                role_id = card.roleId,
                grade = card.grade,
                user_id = player.User.id,

            };
            var bag = new tg_bag
            {
                base_id = card.id,
                bind = card.bind,
                count = 1,
                type = card.type,
                user_id = player.User.id,
            };

            #endregion
            using (var scope = new TransactionScope())  //卡牌生成好后进行数据锁定操作
            {
                recuit.Insert();
                player.User.coin = surplus;
                tg_user.Update(player.User);
                (new Share.Bag()).BuildReward(player.User.id, new List<tg_bag> { bag });
                scope.Complete();
            }
            session.Player = player;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, session.Player.User);

            (new Share.DaMing()).CheckDaMing(player.User.id, (int)DaMingType.酒馆招募);  //检测大名令酒馆招募完成度
            var log_data = string.Format("武将招募获得P:{0}B:{1}G:{2}", player.User.id, card.id, card.grade);
            (new Share.Log()).WriteLog(player.User.id, (int)LogType.Get, (int)ModuleNumber.ROLE, (int)RoleCommand.RECRUIT_GET, log_data);
            return new ASObject(BuildData((int)ResultType.SUCCESS, recuit));

        }

        private Dictionary<String, Object> BuildData(int result, tg_role_recruit recruit)
        {
            var dic = new Dictionary<string, object> 
            { 
                { "result", result },
                { "role",EntityToVo.ToRecruitVo(recruit) },
            };
            return dic;
        }

        /// <summary>随机一个品质</summary>
        private int RandomGrade(int identity)
        {
            var rs = new RandomSingle();

            var list = Variable.BASE_RECRUITRATE.Where(m => m.identity == identity)
                 .OrderByDescending(m => m.rate)
                 .Select(m => new Objects { Name = m.grade.ToString(), Probabilities = m.rate }).ToList(); //读取当前身份概率集合
            var result = rs.RandomFun(list);

            return Convert.ToInt32(result.Name);
        }

        /// <summary>随机一个卡牌</summary>
        /// <param name="list">同品质卡牌</param>
        private static BaseProp RandomCard(List<BaseProp> list)
        {
            var index = RNG.Next(list.Count - 1);
            return list[index];
        }
    }
}
