using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 背包实体逻辑类
    /// </summary>
    public partial class tg_bag
    {
        /// <summary>根据用户id查找</summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<tg_bag> GetFindByUserId(Int64 user_id)
        {
            return FindAll(_.user_id, user_id);
        }

        /// <summary>根据用户id查找未满99个的道具</summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<tg_bag> GetFindByCount(Int64 userid)
        {
            var where = string.Format("user_id={0} and count< 99 and type !={1}", userid, (int)GoodsType.TYPE_EQUIP);
            return FindAll(where, null, null, 0, 0);
        }

        /// <summary>获取用户背包总数</summary>
        public static int GetFindCount(Int64 user_id)
        {
            return FindCount(new String[] { _.user_id, _.state }, new Object[] { user_id, 0 });//去除已穿戴装备
            //return FindCount(_.user_id, user_id);
        }

        /// <summary>根据ids集合查找</summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<tg_bag> GetFindByIds(List<Int64> ids)
        {
            var _ids = string.Join(",", ids.ToArray());
            var where = string.Format("id in ({0})", _ids);
            return FindAll(where, null, null, 0, 0);
        }

        /// <summary>根据ids集合删除数据</summary>
        public static bool GetDeleteIds(List<Int64> ids)
        {
            if (!ids.Any()) return false;
            var _ids = string.Join(",", ids.Where(m => m > 0).ToArray());
            var where = string.Format("id in ({0})", _ids);
            return Delete(where) > 0;
        }

        /// <summary>获取某一类用户道具</summary>
        public static EntityList<tg_bag> GetFindBag(Int64 userid, Int64 baseid)
        {
            return FindAll(new String[] { _.user_id, _.base_id }, new Object[] { userid, baseid });
        }

        /// <summary>获取背包未满道具</summary>>
        public static EntityList<tg_bag> GetFindBagNotFull(Int64 userid, int noequip, IEnumerable<int> baseids, IEnumerable<Int64> ids)
        {
            var exp = new WhereExpression();
            exp &= _.count < 99;
            exp &= _.user_id == userid;
            exp &= _.type != noequip;
            exp &= _.base_id.In(baseids);
            exp &= _.id.NotIn(ids);

            return FindAll(exp, null, null, 0, 0);
        }



        /// <summary>批量保存对象集合</summary>
        public static EntityList<tg_bag> GetSaveList(IEnumerable<tg_bag> list)
        {
            var entitylist = new EntityList<tg_bag>();
            entitylist.AddRange(list);
            entitylist.Insert();
            return entitylist;
        }
    }
}
