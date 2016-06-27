using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.SocketServer;
using TGG.Core.Enum.Type;
using TGG.Core.Entity;

namespace TGG.Module.Equip.Service
{
    /// <summary>
    /// 装备数据
    /// </summary>
    [Obsolete("已经放入道具加载是拉取数据")]
    public class EQUIP_JOIN
    {
        private static EQUIP_JOIN ObjInstance;

        /// <summary> EQUIP_JOIN单体模式 </summary>
        public static EQUIP_JOIN GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new EQUIP_JOIN());
        }

        /// <summary> 装备数据 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "EQUIP_JOIN", "装备数据");
#endif
                var list_equip = tg_bag.GetFindByUserId(session.Player.User.id);
                return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS,list_equip));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }           
        }
    }
}
