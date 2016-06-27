using System;
using System.Collections.Generic;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// tg_role_title 业务逻辑操作类
    /// </summary>
    public partial class tg_role_title
    {
        /// <summary>根据用户id，称号状态获取所有已达成的称号信息</summary>
        public static List<tg_role_title> GetTitlesByUserId(Int64 userid, int state)
        {
            return FindAll(new String[] { _.user_id, _.title_state }, new Object[] { userid, state });
        }

        /// <summary>插入多个称号信息</summary>
        public static bool GetInsertTitles(IEnumerable<tg_role_title> listmodel)
        {
            var list = new EntityList<tg_role_title>();
            list.AddRange(listmodel);
            return list.Insert() > 0;
        }

        /// <summary>根据称号主键tid获取实体</summary>
        public static tg_role_title GetTitleByTid(Int64 tid)
        {
            return Find(new String[] { _.id }, new Object[] { tid });
        }

        /// <summary>根据称号实体更新称号信息</summary>
        public static bool UpdateByTitle(tg_role_title model)
        {
            try
            {
                model.Update();
                return true;
            }
            catch { return false; }
        }

        /// <summary>根据用户id，称号基表编号获取实体</summary>
        public static tg_role_title GetTitleByUseridTid(Int64 userid, int baseid)
        {
            return Find(new String[] { _.user_id, _.title_id }, new Object[] { userid, baseid });
        }

        /// <summary>根据称号ids获取称号信息</summary>
        public static List<tg_role_title> GetTitleByIds(List<Int64> lists)
        {
            var ids = string.Join(",", lists);
            return FindAll(string.Format("id in({0})", ids), null, null, 0, 0);
        }
    }
}
