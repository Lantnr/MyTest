using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Guide;
using TGG.SocketServer;

namespace TGG.Share
{
    /// <summary>
    /// 大名指引类
    /// </summary>
    public partial class DaMing : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>检验大名引导完成度</summary>
        public void CheckDaMing(Int64 userid, int type)
        {
            var isbigbag = false;
            List<tg_daming_log> damings;

            var s = Variable.OnlinePlayer.ContainsKey(userid);   //在线
            if (s)
            {
                var session = Variable.OnlinePlayer[userid] as TGGSession;
                if (session == null) return;
                if (session.Player.Role.Kind.role_level < 30) return;
                damings = session.Player.DamingLog;
            }
            else   //参加活动结算前下线继续统计
            {
                var role = tg_role.GetEntityByUserId(userid);
                if (role.role_level < 30) return;
                damings = tg_daming_log.GetEntityByUserId(userid);
                if (!damings.Any()) return;
            }

            var _da = Variable.BASE_DAMING.FirstOrDefault(m => m.id == type);   //查询工作引导基表数据
            if (_da == null) return;

            var data = damings.FirstOrDefault(m => m.base_id == _da.id);   //session中当前工作数据
            if (data == null) return;

            if (data.user_finish >= _da.degree) return;   //次数已完成或超过，不执行下面步骤
            data.user_finish++;
            if (data.user_finish < _da.degree)
            {
                tg_daming_log.UpdateDaMing(data);
            }
            else
            {
                isbigbag = true;
                data.is_finish = 1;
                data.is_reward = (int)DaMingRewardType.TYPE_CANREWARD;
                tg_daming_log.UpdateDaMing(data);
            }
            //推送协议  大名指引任务
            var _data = new ASObject(BulidData(EntityToVo.ToDaMingLingVo(data)));
            Push(userid, _data);

            //检测大礼包 可否领取
            if (isbigbag) CheckBigBag(damings);
        }

        public void Push(Int64 userid, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "PUSH", "完成推送");
#endif
            var s = Variable.OnlinePlayer.ContainsKey(userid);
            if (!s) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;

            var pv = session.InitProtocol((int)ModuleNumber.GUIDE, (int)GuideCommand.PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }

        /// <summary>检测 是否领取大礼包</summary>
        private void CheckBigBag(IEnumerable<tg_daming_log> listdata)
        {
            var bigbag = listdata.FirstOrDefault(m => m.base_id == (int)DaMingType.大礼包);
            if (bigbag == null) return;
            if (bigbag.is_reward == (int)DaMingRewardType.TYPE_REWARDED || bigbag.is_finish == 1) return;

            var b = Variable.BASE_DAMING.FirstOrDefault(m => m.id == (int)DaMingType.大礼包);
            if (b == null) return;
            if (bigbag.user_finish >= b.degree) return;

            bigbag.user_finish++;
            if (bigbag.user_finish < b.degree)
            {
                tg_daming_log.UpdateDaMing(bigbag);
            }
            else
            {
                bigbag.is_finish = 1;
                bigbag.is_reward = (int)DaMingRewardType.TYPE_CANREWARD;
                tg_daming_log.UpdateDaMing(bigbag);

                new TGTask().TaskDaMingPush(bigbag.user_id,0);   //推送完成大名令主线任务
            }

            //推送协议  完成所有大名引导  推送大礼包协议
            var _data = new ASObject(BulidData(EntityToVo.ToDaMingLingVo(bigbag)));
            Push(bigbag.user_id, _data);
        }

        /// <summary> 组装数据 </summary>
        private Dictionary<String, Object> BulidData(DaMingLingVo daminvo)
        {
            return new Dictionary<string, object> { { "data", daminvo } };
        }
    }
}
