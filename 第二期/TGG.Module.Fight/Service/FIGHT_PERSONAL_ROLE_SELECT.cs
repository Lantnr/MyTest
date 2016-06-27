using System.Collections.Generic;
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
    /// 阵武将选择
    /// </summary>
    public class FIGHT_PERSONAL_ROLE_SELECT
    {
        public static FIGHT_PERSONAL_ROLE_SELECT ObjInstance;

        /// <summary>
        /// FIGHT_PERSONAL_ROLE_SELECT单体模式
        /// </summary>
        /// <returns></returns>
        public static FIGHT_PERSONAL_ROLE_SELECT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FIGHT_PERSONAL_ROLE_SELECT());
        }

        /// <summary> 阵武将选择 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var roleid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "roleId").Value);            //roleId:[double]武将主键
            var location = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "location").Value);        //location:[当前武将位置]
            var player = session.Player.CloneEntity();
            //var role = session.Player.Role;
            var matrix = tg_fight_personal.GetFindByUserId(player.User.id);

            var rid = GetRole(matrix, location);
            if (rid == player.Role.Kind.id) return new ASObject(Common.GetInstance().BuildData((int)ResultType.FIGHT_LEAD_NO_DELETE));//主角不能删除

            IsContainRole(matrix, roleid);             //检查阵中其他位置是否有该武将  有就清除
            PositionUpdate(matrix, location, roleid);  //放置指定位置

            var roles = tg_role.GetFindAllByIds(new List<long> { rid, roleid });
            if (!roles.Any()) return new ASObject(Common.GetInstance().BuildData((int)ResultType.FIGHT_NO_ROLE));
            var r = roles.FirstOrDefault(m => m.id == roleid);
            if (r == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.FIGHT_NO_ROLE));
            var _r = roles.FirstOrDefault(m => m.id == rid);


            if (_r != null)
            {
                if (_r.role_state != (int)RoleStateType.PROTAGONIST)
                    _r.role_state = (int)RoleStateType.IDLE;
            }
            if (r.role_state != (int)RoleStateType.PROTAGONIST)
                r.role_state = (int)RoleStateType.PERSONAL_WAR;

            if (!tg_role.RoleUpdate(roles)) return new ASObject(Common.GetInstance().BuildData((int)ResultType.DATABASE_ERROR));
            matrix.Update();

            session.Fight.Personal = matrix;
            Common.GetInstance().RoleInfoToRole(player.User.id, r, "state");
            if (_r != null) Common.GetInstance().RoleInfoToRole(player.User.id, _r, "state");

            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, matrix, tg_fight_yin.FindByid(matrix.yid)));
        }

        /// <summary> 验证阵中有该武将 有就清除</summary>
        /// <param name="matrix">要验证的阵</param>
        /// <param name="roleid">要验证的武将id</param>
        private void IsContainRole(tg_fight_personal matrix, Int64 roleid)
        {

            if (matrix.matrix1_rid == roleid)
                matrix.matrix1_rid = 0;

            if (matrix.matrix2_rid == roleid)
                matrix.matrix2_rid = 0;

            if (matrix.matrix3_rid == roleid)
                matrix.matrix3_rid = 0;

            if (matrix.matrix4_rid == roleid)
                matrix.matrix4_rid = 0;

            if (matrix.matrix5_rid == roleid)
                matrix.matrix5_rid = 0;
        }

        /// <summary> 获取阵中武将 </summary>
        /// <param name="matrix">阵</param>
        /// <param name="location">要获取的位置</param>
        private Int64 GetRole(tg_fight_personal matrix, int location)
        {
            switch (location)
            {
                case 1: { return matrix.matrix1_rid; }
                case 2: { return matrix.matrix2_rid; }
                case 3: { return matrix.matrix3_rid; }
                case 4: { return matrix.matrix4_rid; }
                case 5: { return matrix.matrix5_rid; }
                default: { break; }
            }
            return 0;
        }

        /// <summary> 将武将放置对应的位置 </summary>
        /// <param name="matrix">阵</param>
        /// <param name="location">要放置的位置</param>
        /// <param name="roleid">要放置的武将id</param>
        public void PositionUpdate(tg_fight_personal matrix, int location, Int64 roleid)
        {
            switch (location)
            {
                case 1: { matrix.matrix1_rid = roleid; break; }
                case 2: { matrix.matrix2_rid = roleid; break; }
                case 3: { matrix.matrix3_rid = roleid; break; }
                case 4: { matrix.matrix4_rid = roleid; break; }
                case 5: { matrix.matrix5_rid = roleid; break; }
                default: { break; }
            }
        }
    }
}
