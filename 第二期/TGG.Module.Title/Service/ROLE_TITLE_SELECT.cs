using System;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.Title.Service
{
    /// <summary>
    /// 称号选择
    /// </summary>
    public class ROLE_TITLE_SELECT
    {
        private static ROLE_TITLE_SELECT ObjInstance;

        /// <summary>ROLE_TITLE_SELECT单体模式</summary>
        public static ROLE_TITLE_SELECT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new ROLE_TITLE_SELECT());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "ROLE_TITLE_SELECT", "称号选择");
#endif
                var btid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value.ToString());
                var player = session.Player;

                var title = tg_role_title.GetTitleByUseridTid(player.User.id, btid);
                if (title == null) return Error((int)ResultType.FRONT_DATA_ERROR);  //验证前端选择的称号信息

                var btitle = Variable.BASE_ROLETITLE.FirstOrDefault(m => m.id == btid);
                if (btitle == null) return Error((int)ResultType.BASE_TABLE_ERROR);  //验证基表信息

                var count = Count(player.UserExtend, btitle.methods);
                return new ASObject(Common.GetInstance().BuildTitleSelect((int)ResultType.SUCCESS, count, title));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>判断返回称号获取途径的次数</summary>
        private int Count(tg_user_extend extend, int type)
        {
            var count = 0;
            switch (type)
            {
                case (int)TitleGetType.USE_SWORD: count = extend.sword_win_count; break;
                case (int)TitleGetType.USE_GUN: count = extend.gun_win_count; break;
                case (int)TitleGetType.USE_TEA_TABLE: count = extend.tea_table_count; break;
                case (int)TitleGetType.BARGARN_SUCCUSS: count = extend.bargain_success_count; break;
            }
            return count;
        }

        private ASObject Error(int error)
        {
            return new ASObject(Common.GetInstance().BuildData(error));
        }
    }
}
