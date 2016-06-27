using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class view_arena_report
    {
        /// <summary>根据用户id获取竞技场战报信息</summary>
        public static List<view_arena_report> GetEntityList(Int64 userid, int number)
        {
            return FindAll(string.Format("user_id={0}", userid), " id desc ", string.Format(" top {0} *", number), 0, 0);
        }

        /// <summary>根据id获取战报信息</summary>
        public static view_arena_report FindById(Int64 id)
        {
            return Find(new String[] { _.id }, new Object[] { id });
        }

    }
}
