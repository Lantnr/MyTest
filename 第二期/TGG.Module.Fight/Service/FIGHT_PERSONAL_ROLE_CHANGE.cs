using FluorineFx;
using System;
using System.Linq;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Fight.Service
{
    /// <summary>
    /// 阵武将交换
    /// </summary>
    public class FIGHT_PERSONAL_ROLE_CHANGE
    {
        public static FIGHT_PERSONAL_ROLE_CHANGE ObjInstance;

        /// <summary>
        /// FIGHT_PERSONAL_ROLE_CHANGE单体模式
        /// </summary>
        /// <returns></returns>
        public static FIGHT_PERSONAL_ROLE_CHANGE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FIGHT_PERSONAL_ROLE_CHANGE());
        }

        /// <summary> 阵武将交换 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var roleid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "roleId").Value);            //roleId:[double]武将主键
            var roleIdNew = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "roleIdNew").Value);      //roleIdNew:[double] 原来位置武将
            var location = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "location").Value);        //location:[当前武将位置]
            var newLocation = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "locationNew").Value);  //locationNew:[int] 新的阵位置
            var user = session.Player.User;

            var matrix = tg_fight_personal.GetFindByUserId(user.id);
            var yin = tg_fight_yin.FindByid(matrix.yid);

            Common.GetInstance().PositionUpdate(ref matrix, newLocation, roleid);
            Common.GetInstance().PositionUpdate(ref matrix, location, roleIdNew);

            matrix.Update();
            session.Fight.Personal = matrix;
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, matrix, yin));
        }
    }
}
