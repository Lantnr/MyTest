using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>查询玩家信息视图</summary>
    public partial class view_player_detail
    {
        /// <summary>根据类型查询服务器玩家信息</summary>
        /// <param name="type">查询类型</param>
        /// <param name="value">查询值</param>
        /// <param name="index">分页索引值</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">数量</param>
        public static EntityList<view_player_detail> GetPagerEntity(Int32 type, String value, Int32 index, Int32 size, out Int32 count)
        {
            var where = "";
            switch (type)
            {
                case 0:
                    {
                        count = FindCount();
                        return FindAll(where, null, "*", index * size, size);
                    }
                case 1:
                    {
                        where = string.Format("user_code like '%{0}%'", value);
                        count = FindCount(where, null, null, 0, 0);
                        return FindAll(where, null, "*", index * size, size);
                    }
                case 2:
                    {
                        where = string.Format("player_name like '%{0}%'", value);
                        count = FindCount(where, null, null, 0, 0);
                        return FindAll(where, null, "*", index * size, size);
                    }
            }
            count = 0;
            return new EntityList<view_player_detail>();
        }
    }
}
