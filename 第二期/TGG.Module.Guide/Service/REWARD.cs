using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Guide;
using TGG.Share;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.Guide.Service
{
    /// <summary>
    /// 领取奖励
    /// </summary>
    public class REWARD
    {
        public static REWARD ObjInstance;

        /// <summary>REWARD单体模式</summary>
        public static REWARD GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new REWARD());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "REWARD", "领取奖励");
#endif
            var id = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);

            var dals = session.Player.DamingLog;
            var dm = dals.FirstOrDefault(m => m.id == id);
            if (dm == null) return Data((int)ResultType.DAMING_GET_NULL);         //查询数据库任务信息
            if (dm.is_finish == 0) return Data((int)ResultType.DAMING_UN_FINISH);     //未达到领取标准
            var isreward = dm.is_reward;
            var baseid = dm.base_id;
            if (isreward == (int)DaMingRewardType.TYPE_REWARDED) return Data((int)ResultType.DAMING_IS_REWARD);       //奖励已经领取

            if (baseid == (int)DaMingType.大礼包)
            {
                if (!CheckBigBag(dals)) return Data((int)ResultType.DAMING_UN_FINISH);  //大礼包验证
            }

            //领取奖励
            if (!Reward(session.Player.User.id, baseid)) return Data((int)ResultType.REWARD_FALSE);   //验证奖励领取信息
            dm.is_reward = (int)DaMingRewardType.TYPE_REWARDED;

            tg_daming_log.UpdateDaMing(dm);

            if (CheckDaMingIsClose(dals))
            {
                var ex = session.Player.UserExtend.CloneEntity();
                ex.dml = 0;            //未激活状态
                ex.Update();
                session.Player.UserExtend = ex;
            }

            var dmvo = EntityToVo.ToDaMingLingVo(dm);
            return new ASObject(BuildData((int)ResultType.SUCCESS, dmvo));
        }

        /// <summary>
        /// 验证大礼包是否  达到领取标准
        /// </summary>
        private bool CheckBigBag(IEnumerable<tg_daming_log> listdatas)
        {
            return listdatas.All(item => item.is_finish == 1);
        }

        /// <summary>领取奖励信息</summary>
        private bool Reward(Int64 userid, int bid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return false;
            var dmInfo = Variable.BASE_DAMING.FirstOrDefault(m => m.id == bid);    //查询基表信息
            if (dmInfo == null) return false;
            return new Reward().GetReward(dmInfo.reward, userid);
        }

        /// <summary>验证是否关闭大名令</summary>
        private bool CheckDaMingIsClose(IEnumerable<tg_daming_log> listdatas)
        {
            return listdatas.All(item => item.is_reward == (int)DaMingRewardType.TYPE_REWARDED);
        }

        /// <summary>组装信息</summary>
        private ASObject Data(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }

        /// <summary>组装信息</summary>
        private Dictionary<String, Object> BuildData(int result, DaMingLingVo dmvo)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result }, { "data", dmvo } });
        }
    }
}
