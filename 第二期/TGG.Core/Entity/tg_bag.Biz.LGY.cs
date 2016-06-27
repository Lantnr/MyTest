using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    public partial class tg_bag
    {
        /// <summary>
        /// 根据装备主键id获取装备实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static tg_bag GetEntityById(Int64 id)
        {
            return Find(_.id, id);
        }

        /// <summary>
        /// 更新装备数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int GetEquipUpdate(tg_bag model)
        {
            return Update(model);
        }

        /// <summary>
        /// 根据装备主键id删除装备实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool GetDeleteId(Int64 id)
        {
            try
            {
                string where = string.Format("id = {0}", id);
                Delete(where);
                return true;
            }
            catch
            { return false; }
        }

        /// <summary>根据装备id集合 查询装备</summary>
        public static List<tg_bag> GetEquipsByIds(List<Int64> ids)
        {
            if (!ids.Any()) return new List<tg_bag>();
            var _ids = string.Join(",", ids.ToArray());
            var where = string.Format("id in ({0})", _ids);
            return FindAll(where, null, null, 0, 0);
        }

        public static bool GetUpdateBagSynthetic(List<tg_bag> list_update, List<tg_bag> list_delete)
        {
            var list_delete_save = new EntityList<tg_bag>();
            list_delete_save.AddRange(list_delete);
            list_delete_save.Delete();
            var list_update_save = new EntityList<tg_bag>();
            list_update_save.AddRange(list_update);
            list_update_save.Save();
            return true;
        }
    }
}
