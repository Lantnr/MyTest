using System;
using System.Collections.Generic;
using System.Linq;
using XCode;

namespace TGG.Core.Entity
{
    public partial class tg_user_ting
    {
        /// <summary>根据用户Id获取实体</summary>
        public static List<tg_user_ting> GetEntityByUserIdAndState(Int64 user_id, int state)
        {
            return FindAll(new String[] { _.user_id, _.state }, new Object[] { user_id, state });
        }

        /// <summary>
        /// 町货物状态更改
        /// </summary>
        public static void GetStateUpdate()
        {
            var list = view_ting_not_car.GetEntityByState(1);
            if (!list.Any())
                return;
            var ids = string.Join(",", list.ToList().Select(m => m.id).ToArray());
            //var _list = FindAll(string.Format("id in ({0})", ids), null, null, 0, 0);
            //foreach (var item in _list)
            //{
            //    item.state = 0;
            //}
            //_list.Update();
            var exp = new WhereExpression();
            if (!ids.IsNullOrEmpty()) exp &= _.id.In(ids);
            Update("state=0", exp);
        }
    }
}
