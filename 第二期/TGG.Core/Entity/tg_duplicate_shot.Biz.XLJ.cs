using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 射击副本类
    /// </summary>
    public partial class tg_duplicate_shot
    {
        /// <summary>根据用户Id获取实体</summary>
        public static tg_duplicate_shot GetEntityByUserId(Int64 user_id = 0)
        {
            var entity = Find(_.user_id ,user_id );
            return entity;
        }

    }
}
