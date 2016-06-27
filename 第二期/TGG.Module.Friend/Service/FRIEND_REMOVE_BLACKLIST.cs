using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Friend.Service
{
    /// <summary>
    /// 解除黑名单指令
    /// Author:arlen xiao
    /// </summary>
    public class FRIEND_REMOVE_BLACKLIST
    {
        private static FRIEND_REMOVE_BLACKLIST ObjInstance;

        /// <summary>FRIEND_REMOVE_BLACKLIST单体模式</summary>
        public static FRIEND_REMOVE_BLACKLIST GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FRIEND_REMOVE_BLACKLIST());
        }

        /// <summary>解除黑名单指令</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "FRIEND_REMOVE_BLACKLIST", "解除黑名单指令");
#endif
            var id =Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value);
            var entity = tg_friends.FindByid(id);
            if (entity == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.FRIEND_NO_DATA_ERROR));
#if DEBUG
            XTrace.WriteLine("id  {0} user_id {1} friend_id {2} friend_state {3}", entity.id, entity.user_id, entity.friend_id, entity.friend_state);
#endif
            try
            {
                var fid = entity.friend_id;  
                entity.Delete();
                session.Player.BlackList.Remove(fid);
#if DEBUG
                XTrace.WriteLine("黑名单总数:{0}", session.Player.BlackList.Count);
#endif
            }
            catch { return new ASObject(Common.GetInstance().BuildData((int)ResultType.DATABASE_ERROR)); }
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS));
        }
    }
}
