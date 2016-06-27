using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Global;
using TGG.Core.Vo.Activity;
using TGG.SocketServer;

namespace TGG.Module.User.Service
{
    /// <summary>
    /// 活动开启
    /// </summary>
    public class ACTIVITY_OPEN
    {
        private static ACTIVITY_OPEN ObjInstance;

        /// <summary>ACTIVITY_OPEN单例模式</summary>
        public static ACTIVITY_OPEN GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new ACTIVITY_OPEN());
        }

        /// <summary>创建玩家</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            //筑城活动是否开启
            var buildingisopen = Variable.Activity.BuildActivity.isover ? 0 : 1;
            var siegeisopen = Variable.Activity.Siege.IsOpen ? 1 : 0;
            var listvo = new List<ActivityOpenVo>
            {
                new ActivityOpenVo()
                {
                    openId = 14,
                    state = siegeisopen
                },
                new ActivityOpenVo()
                {
                    openId = 15,
                    state = buildingisopen
                }
            };
            var list_aso = listvo.Select(AMFConvert.ToASObject).ToList();
            return BuildData(list_aso);
        }

        /// <summary>
        /// 组装返回数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private ASObject BuildData(List<ASObject> list)
        {
            var dic = new Dictionary<string, object>()
            {
                {"listVo", list}
            };
            return new ASObject(dic);

        }
    }
}
