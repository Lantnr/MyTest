using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 取消家臣
    /// </summary>
    public class TRAIN_ROLE_UNSELECT
    {
        private static TRAIN_ROLE_UNSELECT ObjInstance;

        /// <summary> TRAIN_ROLE_UNSELECT单体模式 </summary>
        public static TRAIN_ROLE_UNSELECT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TRAIN_ROLE_UNSELECT());
        }

        /// <summary> 取消家臣</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var rid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);
            var train_role = tg_train_role.GetEntityByRid(rid);
            if(train_role.state==(int)RoleTrainStatusType.TRAINING)
                return new ASObject(Common.GetInstance().BuilData((int)ResultType.TRAINROLE_TRAINING));
            train_role.state = (int)RoleTrainStatusType.FREE;
            return !tg_train_role.GetUpdate(train_role) ? 
                new ASObject(Common.GetInstance().BuilData((int)ResultType.DATABASE_ERROR)) : 
                new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS));
        }
    }
}
