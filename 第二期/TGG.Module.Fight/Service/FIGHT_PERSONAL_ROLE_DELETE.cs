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
    /// 阵武将丢弃
    /// </summary>
    public class FIGHT_PERSONAL_ROLE_DELETE
    {
        public static FIGHT_PERSONAL_ROLE_DELETE ObjInstance;

        /// <summary>FIGHT_PERSONAL_ROLE_DELETE单体模式</summary>
        public static FIGHT_PERSONAL_ROLE_DELETE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FIGHT_PERSONAL_ROLE_DELETE());
        }

        /// <summary>阵武将丢弃</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var roleid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "roleId").Value);          //roleId:[double]武将主键
            var location = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "location").Value);        //location:[当前武将位置]
            var user = session.Player.User;
            var role = session.Player.Role.Kind;
            if (role.id == roleid) return new ASObject(Common.GetInstance().BuildData((int)ResultType.FIGHT_LEAD_NO_DELETE, null, null));

            var _role = tg_role.FindByid(roleid);
            if (_role == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.DATABASE_ERROR, null, null));
            var matrix = tg_fight_personal.GetFindByUserId(user.id);
            var yin = tg_fight_yin.FindByid(matrix.yid);

            Common.GetInstance().PositionUpdate(ref matrix, location, 0);

            _role.role_state = (int)RoleStateType.IDLE;

            _role.Update();
            matrix.Update();

            session.Fight.Personal = matrix;
            Common.GetInstance().RoleInfoToRole(user.id, _role, "state");
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, matrix, yin));
        }
    }
}
