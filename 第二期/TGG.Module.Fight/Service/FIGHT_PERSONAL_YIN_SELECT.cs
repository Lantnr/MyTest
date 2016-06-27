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
    /// 阵印选择
    /// </summary>
    public class FIGHT_PERSONAL_YIN_SELECT
    {
        public static FIGHT_PERSONAL_YIN_SELECT ObjInstance;

        /// <summary>
        /// FIGHT_PERSONAL_YIN_SELECT单体模式
        /// </summary>
        /// <returns></returns>
        public static FIGHT_PERSONAL_YIN_SELECT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FIGHT_PERSONAL_YIN_SELECT());
        }

        /// <summary>
        /// 阵印选择
        /// </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var id = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);     //id:[double] 印id主键
            var user = session.Player.User;

            var matrix = tg_fight_personal.GetFindByUserId(user.id);
            var list = tg_fight_yin.GetFindByUserId(user.id);
            var model = list.FirstOrDefault(m => m.state != (int)YinStateType.UNUSED);
            var yin = list.FirstOrDefault(m => m.id == id);
            if (yin == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.DATABASE_ERROR, matrix, null));
            if (yin.yin_level == 0) return new ASObject(Common.GetInstance().BuildData((int)ResultType.FIGHT_YIN_NO_LEVEL));

            if (model != null)
            {
                model.state = (int)YinStateType.UNUSED;
                model.Update();
            }
            yin.state = (int)YinStateType.ADORN;
            yin.Update();
            matrix.yid = id;
            matrix.Update();
            session.Fight.Personal = matrix;
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, matrix, yin));
        }
    }
}
