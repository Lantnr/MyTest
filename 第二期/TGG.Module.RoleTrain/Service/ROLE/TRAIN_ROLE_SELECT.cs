using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Role;
using TGG.SocketServer;

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 选取家臣
    /// </summary>
    public class TRAIN_ROLE_SELECT
    {
        private static TRAIN_ROLE_SELECT ObjInstance;

        /// <summary> TRAIN_ROLE_SELECT单体模式 </summary>
        public static TRAIN_ROLE_SELECT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TRAIN_ROLE_SELECT());
        }

        /// <summary> 选取家臣</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "TRAIN_ROLE_SELECT", "选取家臣");
#endif
           var rid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);
            var train_role = tg_train_role.GetEntityByRid(rid);
            if (train_role == null)
            {
                train_role = new tg_train_role {rid = rid, state = (int) RoleTrainStatusType.STOP};
                return !tg_train_role.GetInsert(train_role) ? 
                    new ASObject(Common.GetInstance().BuilData((int)ResultType.DATABASE_ERROR)) : 
                    new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS));
            }
            train_role.state = (int)RoleTrainStatusType.STOP;
            if(!tg_train_role.GetUpdate(train_role))
                return new ASObject(Common.GetInstance().BuilData((int)ResultType.DATABASE_ERROR));
# if DEBUG
            XTrace.WriteLine("{0}", "返回选取家臣成功结果");
#endif
            return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS));
        }
    }
}
