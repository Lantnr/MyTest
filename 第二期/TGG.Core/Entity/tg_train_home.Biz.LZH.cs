using System;
using System.Collections.Generic;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// tg_train_home 业务逻辑类
    /// </summary>
    public partial class tg_train_home
    {
        /// <summary>根据用户id，居城id，将册难度获取npc信息</summary>
        public static List<tg_train_home> GetByUserIdCityIdLevel(Int64 userid, Int64 cityid, int level)
        {
            return FindAll(new String[] { _.userid, _.city_id, _.npc_type }, new Object[] { userid, cityid, level });
        }

        /// <summary>根据实体插入npc信息</summary>
        /// <param name="npcs">list[tg_train_home]</param>
        public static bool GetInsertNpc(IEnumerable<tg_train_home> npcs)
        {
            var list = new EntityList<tg_train_home>();
            list.AddRange(npcs);
            return list.Insert() > 0;
        }

        /// <summary>根据npc主键id获取实体</summary>
        public static tg_train_home GetNpcById(Int64 id)
        {
            var exp = new WhereExpression();
            if (id > 0) exp &= _.id == id;
            return Find(exp);
        }

        /// <summary>根据主键ids集合删除npc信息</summary>
        public static bool GetDeleteByIds(List<Int64> nids)
        {
            string ids = string.Join(",", nids.ToArray());
            string where = string.Format("id in ({0})", ids);
            return Delete(where) > 0;
        }

        /// <summary>根据npc实体更新npc信息</summary>
        public static bool UpdateNpc(tg_train_home model)
        {
            try
            {
                model.Update();
                return true;
            }
            catch { return false; }
        }

        /// <summary>每日更新武将宅 NPC 信息</summary>
        public static void UpdateNpcSpirit()
        {
            Update("npc_spirit=total_spirit,npc_state=0,is_steal=0", null);
        }
    }
}
