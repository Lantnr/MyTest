using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_family
    {
        /// <summary>
        /// 查询全部家族
        /// </summary>
        public static List<tg_family> GetAllById()
        {
            return FindAll();
        }

        /// <summary>
        /// 根据id获取家族实体
        /// </summary>
        public static tg_family GetEntityById(Int64 id)
        {
            return Find(_.id,id);
        }

        /// <summary>根据家族id集合 查询家族集合</summary>
        public static List<tg_family> GetFamilysByIds(IEnumerable<Int64> ids)
        {          
            var _ids = string.Join(",", ids.ToArray());
            var where = string.Format("id in (0,{0})", _ids);
            return FindAll(where, null, null, 0, 0);
        }

        /// <summary>插入家族实体</summary>
        public static bool GetInsert(tg_family model)
        {
            try
            {
                Insert(model);
                return true;
            }
            catch { return false; }
            
        }

        /// <summary>更新家族实体</summary>
        public static bool GetUpdate(tg_family model)
        {
            try
            {
                model.Update();
                return true;
            }
            catch { return false; }

        }

        /// <summary>
        /// 根据族长名字获取家族实体
        /// </summary>
        public static tg_family GetEntityByChief(Int64 userid)
        {
            return Find(_.userid, userid);
        }

        /// <summary>
        /// 根据家族id删除家族
        /// </summary>
        public static bool GetDelete(Int64 id)
        {
            string where = string.Format("id = {0}", id);
            return Delete(where) > 0;
        }
    }
}
