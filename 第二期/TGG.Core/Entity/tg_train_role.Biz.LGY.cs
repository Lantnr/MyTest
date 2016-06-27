using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;

namespace TGG.Core.Entity
{
    public partial class tg_train_role
    {
        /// <summary>根据武将id获取实体</summary>
        public static tg_train_role GetEntityByRid(Int64 rid)
        {
            return Find(_.rid,rid);
        }

        /// <summary>插入武将修行实体</summary>
        public static bool GetInsert(tg_train_role model)
        {
            try
            {
                tg_train_role.Insert(model);
                return true;
            }
            catch { return false; }
        }

        /// <summary>更新武将修行实体</summary>
        public static bool GetUpdate(tg_train_role model)
        {
            try
            {
                tg_train_role.Update(model);
                return true;
            }
            catch { return false; }
        }

        /// <summary>删除武将修行实体</summary>
        public static bool GetDelete(tg_train_role model)
        {
            try
            {
                tg_train_role.Delete(model);
                return true;
            }
            catch { return false; }
        }

        /// <summary>根据武将id集合查询武将视图信息</summary>
        public static List<tg_train_role> GetEntityByIds(List<Int64> lists)
        {
            var ids = string.Join(",", lists);
            return FindAll(string.Format("rid in({0})", ids), null, null, 0, 0);
        }

        /// <summary>根据修炼状态获取实体集合</summary>
        public static List<tg_train_role> GetEntityListByState(int state, string rids)
        {
            return FindAll(string.Format("state={0} and rid in({1})", state, rids), null, null, 0, 0);
        }

        /// <summary>根据时间操作tg_train_role</summary>
        public static List<tg_train_role> GetEntityListByTime(Int64 time, string rids)
        {
            var list = FindAll(string.Format("state=2 and time<={0} and rid in({1})", time, rids), null, null, 0, 0);
            foreach (var item in list)
            {
                item.state = (int)RoleTrainStatusType.FREE;
                item.time = 0;
            }
            list.Update();
            return list;
        }
    }
}
