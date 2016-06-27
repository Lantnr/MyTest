using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    public class ROLE_HIRE : IConsume
    {
        public ASObject Execute(long user_id, ASObject data)
        {
            if (!Variable.OnlinePlayer.ContainsKey(user_id))
                return CommonHelper.ErrorResult(ResultType.CHAT_NO_ONLINE);
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            return CommandStart(session, data);
        }

        /// <summary> 武将雇佣 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "ROLE_HIRE", "武将雇佣");
#endif
            var identityId = session.Player.Role.Kind.role_identity;
            var user = session.Player.User.CloneEntity();
            var userextend = session.Player.UserExtend;

            var identity = Variable.BASE_IDENTITY.FirstOrDefault(m => m.id == identityId);
            if (identity == null) return CommonHelper.ErrorResult(ResultType.ROLE_PLAY_IDENTITY_ERROR);
            var npc = Variable.BASE_NPCHIRE.FirstOrDefault(m => m.id == identity.value);
            if (npc == null) return CommonHelper.ErrorResult(ResultType.ROLE_HIRE_ERROR);
            if (npc.money > user.coin) CommonHelper.ErrorResult(ResultType.BASE_PLAYER_COIN_ERROR);
            if (userextend.hire_id != 0 || userextend.hire_time != 0) return CommonHelper.ErrorResult(ResultType.ROLE_HIRE_SELECTOK);

            var time = npc.time * 1000 * 60;
            user.coin -= npc.money;
            userextend.hire_id = npc.id;
            userextend.hire_time = (DateTime.Now.Ticks - 621355968000000000) / 10000 + time;

            user.Update();
            userextend.Update();
            session.Player.User = user;
            session.Player.UserExtend = userextend;
            new Share.Role().ThreadTask(time, user.id);
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, session.Player.User);
            (new Share.DaMing()).CheckDaMing(user.id, (int)DaMingType.忍卫);   //检测大名令忍卫完成度
            return new ASObject(BuildData((int)ResultType.SUCCESS, EntityToVo.ToRoleHomeHireVo(userextend)));
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(int result, HomeHireVo model)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"homeHireVo", model},
            };
            return dic;
        }
    }
}
