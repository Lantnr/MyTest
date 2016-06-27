using System;

namespace TGG.Core.Entity
{
    /// <summary>
    /// tg_user_extend  业务逻辑操作
    /// </summary>
    public partial class tg_user_extend
    {
        /// <summary>根据userid 获取实体</summary>
        public static tg_user_extend GetByUserId(Int64 userid)
        {
            return Find(new String[] { _.user_id }, new Object[] { userid });
        }
    }
}
