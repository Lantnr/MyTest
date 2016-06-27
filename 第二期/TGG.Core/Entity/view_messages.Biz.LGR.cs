using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class view_messages
    {
        /// <summary>根据用户id获取实体集合</summary>
        public static List<view_messages> GetEntityListByUserId(Int64 userid)
        {
            return FindAll(string.Format("receive_id={0}", userid), null, null, 0, 0);
        }

        /// <summary>根据id获取实体</summary>
        public static view_messages GetEntityById(Int64 id)
        {
            return Find(new string[]{_.id},new object[]{id});
        }

        /// <summary>根据收件人和附件状态获取数据</summary>
        public static List<view_messages> GetMessagesIsAttByUserId(Int64 userid, int isattachment)
        {
            return FindAll(string.Format(" receive_id={0} and isattachment={1} ", userid, isattachment), null, null, 0, 0);
        }
    }
}
