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

namespace TGG.Module.Role.Service
{
    /// <summary>
    /// 家臣购买体力
    /// 开发者：李德雁
    /// </summary>
    public class ROLE_POWER_BUY
    {
        private static POWER_BUY _objInstance;

        /// <summary> 购买体力 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var roleid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value);
            var roleinfo = tg_role.FindByid(roleid);
            var userid = session.Player.User.id;
            if (roleinfo == null || roleinfo.role_state == (int)RoleStateType.PROTAGONIST)
                return BuildData((int)ResultType.FRONT_DATA_ERROR);
            //获取基表消耗元宝数
            var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "7037");
            if (baserule == null) return BuildData((int)ResultType.BASE_TABLE_ERROR);
            var basevalue = Convert.ToInt32(baserule.value);
            //用户数据更改保存
            var usergold = session.Player.User.gold;
            if (usergold < basevalue) return BuildData((int)ResultType.BASE_PLAYER_GOLD_ERROR);   //金钱验证
            //武将数据更改保存
            roleinfo.power = 100;
            roleinfo.Update();
            //用户金钱数据保存
            GoldSaveAndSend(session.Player.User.CloneEntity(), basevalue, userid);   //保存金币数据
            PowerSaveAndSend(roleinfo, userid);//保存体力数据
            return BuildData((int)ResultType.SUCCESS);
        }

        /// <summary>组装数据</summary>
        private ASObject BuildData(int result)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
            };
            return new ASObject(dic);
        }

        /// <summary>
        /// 用户金币保存并推送
        /// </summary>
        private void GoldSaveAndSend(tg_user user, Int32 cost, Int64 userid)
        {
            user.gold -= cost;
            user.Save();
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            session.Player.User.gold = user.gold;
            Common.GetInstance().RewardsToUser(session, user, (int)GoodsType.TYPE_GOLD);
        }

        /// <summary>
        /// 武将体力保存并推送
        /// </summary>
        private void PowerSaveAndSend(tg_role role, Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var list = new List<string>
                {
                    Expressions.GetPropertyName<RoleInfoVo>(q => q.rolePower),
                    Expressions.GetPropertyName<RoleInfoVo>(q => q.power)
                };
            new RoleAttUpdate().RoleUpdatePush(role, userid, list);
        }

    }
}
