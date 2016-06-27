using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 开启商圈
    /// </summary>
    public class BUSINESS_AREA_OPEN : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(long user_id, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_AREA_OPEN", "开启商圈");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.FAIL);
            var tid = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "id").Value);
            return Logic(tid, session);
        }

        /// <summary>逻辑方法</summary>
        private ASObject Logic(Int32 tid, TGGSession session)
        {
            var player = session.Player.CloneEntity();

            var _area = Variable.BASE_TING.FirstOrDefault(m => m.id == tid);
            var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "3015"); //获取基表数据
            var base_vip = Variable.BASE_VIP.FirstOrDefault(m => m.level == player.Vip.vip_level);
            if (baserule == null || _area == null || base_vip == null)
                return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);
            var cost = Convert.ToInt32(baserule.value);
            player.User.gold -= cost;

            var ishave = player.BusinessArea.Count(m => m.area_id == _area.areaId) > 0;
            if (ishave) return CommonHelper.ErrorResult((int)ResultType.BUSINESS_AREA_ISEXIST_ERROR);

            var isnum = base_vip.area - player.Vip.area;
            if (isnum <= 0) return CommonHelper.ErrorResult((int)ResultType.BUSINESS_VIP_AREA_COUNR_ERROR);

            var area = new tg_user_area
            {
                user_id = player.User.id,
                area_id = _area.areaId,
            };

            area.Save();

            player.BusinessArea.Add(area);
            player.Vip.area += 1;
            player.User.Save();
            player.Vip.Save();

            session.Player = player;
            //元宝更新推送
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);

            return BuildData((int)ResultType.SUCCESS, area.area_id);

        }

        /// <summary>组装数据</summary>
        private ASObject BuildData(int result, int area)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"area",area}
            };
            return new ASObject(dic);
        }

    }
}
