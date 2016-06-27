using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGM.API.Entity
{
    public partial class tgm_goods_type
    {
        /// <summary> 分页获取福利卡类型信息 </summary>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数</param>
        public static EntityList<tgm_goods_type> GetPageEntity(Int32 index, Int32 size, out Int32 count)
        {
            count = FindCount();
            return FindAll(null, "type_id", null, index * size, size);
        }

        /// <summary>根据类型枚举ID查询是否存在同ID信息</summary>
        public static tgm_goods_type GetTypeByTypeId(Int32 typeId)
        {
            return Find(new String[] { _.type_id }, new Object[] { typeId });
        }

        /// <summary>根据类型枚举名称查询是否存在同名称信息</summary>
        public static tgm_goods_type GetTypeByName(String name)
        {
            return Find(new String[] { _.name }, new Object[] { name });
        }

        /// <summary>根据类型枚举ID查询是否存在同ID信息</summary>
        public static bool GetTypeByTypeId(Int32 gid, Int32 typeId)
        {
            var where = string.Format("id !={0} and type_id={1}", gid, typeId);
            return FindAll(where, null, null, 0, 0).Count > 0;
        }

        /// <summary>根据类型枚举名称查询是否存在同名称信息</summary>
        public static bool GetTypeByName(Int32 gid, String name)
        {
            var where = string.Format("id !={0} and name='{1}'", gid, name);
            return FindAll(where, null, null, 0, 0).Count > 0;
        }
    }
}
