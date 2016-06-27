using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 主角体力购买
    /// 开发者：李德雁
    /// </summary>
    public class POWER_BUY : IConsume
    {
        /// <summary> 购买体力 </summary>
        public ASObject Execute(Int64 userid, ASObject data)
        {
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.FAIL);
            var player = session.Player.CloneEntity();
            var mycount = player.UserExtend.power_buy_count; mycount++;

            //VIP购买体力次数验证
            if (RuleConvert.GetBuyPower(player.Vip.vip_level) < mycount)
                return CommonHelper.ErrorResult((int)ResultType.VIP_POWER_ERROR);

            var baseinfo = Variable.BASE_BUYPOWER.FirstOrDefault(q => q.id == mycount); //读取基表数据
            if (baseinfo == null) return BuildData((int)ResultType.BASE_TABLE_ERROR, 0);
            //够买次数已经达到上限
            var maxpower = Variable.BASE_RULE.FirstOrDefault(q => q.id == "1009");
            if (maxpower == null) return BuildData((int)ResultType.BASE_TABLE_ERROR, 0);
            if (player.User.gold < baseinfo.gold) return BuildData((int)ResultType.BASE_PLAYER_GOLD_ERROR, 0);   //金钱验证

            if (baseinfo.power + player.Role.Kind.power > Convert.ToInt32(maxpower.value))  //最大体力数验证
                return BuildData((int)ResultType.ROLE_POWER_OVER, 0);
            GoldSaveAndSend(player.User, baseinfo, userid);   //保存金币数据
            PowerSaveAndSend(player.Role.Kind, baseinfo.power, userid);//保存体力数据
            player.UserExtend.power_buy_count = mycount;
            player.UserExtend.Save();
            session.Player = player;
            return BuildData((int)ResultType.SUCCESS, mycount);
        }

        /// <summary>组装数据</summary>
        private ASObject BuildData(int result, int count)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"count", count},
            };
            return new ASObject(dic);
        }

        /// <summary>
        /// 用户金币保存并推送
        /// </summary>
        private void GoldSaveAndSend(tg_user user, BaseBuyPower baseinfo, Int64 userid)
        {
            user.gold -= baseinfo.gold;
            user.Save();
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            session.Player.User = user;
            new Share.User().REWARDS_API((int)GoodsType.TYPE_GOLD, user);
        }

        /// <summary>
        /// 用户金币保存并推送
        /// </summary>
        private void PowerSaveAndSend(tg_role role, int power, Int64 userid)
        {
            role.power += power;
            role.Save();
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            session.Player.Role.Kind = role;
            var list = new List<string>
                {
                    Expressions.GetPropertyName<RoleInfoVo>(q => q.rolePower),
                    Expressions.GetPropertyName<RoleInfoVo>(q => q.power)
                };
            new RoleAttUpdate().RoleUpdatePush(role, userid, list);
        }

    }
}
