using System;
using System.Collections.Generic;
using TGG.Core.Enum.Type;

namespace TGG.Core.Entity
{
    /// <summary>
    /// tg_user_ting业务逻辑
    /// </summary>
    public partial class tg_user_ting
    {
        /// <summary>根据用户id 町id获取实体</summary>
        public static tg_user_ting GetEntityByUserIdTingId(Int64 userid, int tingid)
        {
            return Find(new String[] { _.user_id, _.ting_id }, new Object[] { userid, tingid });
        }

        /// <summary>
        /// 根据商圈id，用户id，查询已访问的町集合
        /// </summary>
        public static List<tg_user_ting> GetEntityQueryedTing(Int64 userid, int areaid)
        {
            return FindAll(string.Format("area_id={0} and user_id={1} and state={2}", areaid, userid, (int)CityVisitType.VISIT), null, null, 0, 0);
        }
    }
}
