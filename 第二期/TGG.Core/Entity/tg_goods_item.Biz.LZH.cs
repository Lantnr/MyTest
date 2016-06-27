using System;

namespace TGG.Core.Entity
{
    public partial class tg_goods_item
    {
        /// <summary>根据用户Id获取实体</summary>
        public static bool GetEntityByTingIdAndUserIdAndGoodId(int tingid, Int64 userid, Int64 goodid)
        {
            return FindCount(new String[] { _.ting_id, _.user_id, _.goods_id }, new Object[] { tingid, userid, goodid })>0;
        }

        /// <summary>判断这个用户这个町货物是否存在</summary>
        public static int GetFindByTingIdUserId(int tingid, Int64 userid)
        {
            return FindCount(new String[] { _.ting_id, _.user_id }, new Object[] { tingid, userid });
        }

    }
}
