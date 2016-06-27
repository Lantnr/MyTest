using System.Linq;
using FluorineFx;
using System;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Fight.Service
{
    /// <summary>
    /// 进入个人战设置
    /// </summary>
    public class FIGHT_PERSONAL_JOIN
    {
        public static FIGHT_PERSONAL_JOIN ObjInstance;

        /// <summary>
        /// FIGHT_PERSONAL_JOIN单体模式
        /// </summary>
        /// <returns></returns>
        public static FIGHT_PERSONAL_JOIN GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FIGHT_PERSONAL_JOIN());
        }

        /// <summary>
        /// 进入个人战设置
        /// </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            tg_fight_yin yin;
            var user = session.Player.User;
            var role = session.Player.Role;

            var matrix = tg_fight_personal.GetFindByUserId(user.id);
            if (matrix == null)
            {
                matrix = tg_fight_personal.PersonalInsert(user.id, role.Kind.id);
                yin = null;
            }
            else
                yin = matrix.yid == 0 ? null : tg_fight_yin.FindByid(matrix.yid);

            session.Fight.Personal = matrix;

            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, matrix, yin));//matrix:[MatrixVo] 阵Vo
        }
    }
}
