using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_messages
    {
        /// <summary>根据用户Id获取实体集合数量</summary>
        public static int GetCountByUserId(Int64 userid, int isread)
        {
            //return FindCount(string.Format("receive_id={0} and isread={1}", userid, isread), null, null, 0, 0);
            return FindCount(new String[] { _.receive_id, _.isread }, new Object[] { userid, isread });
        }

        /// <summary>邮件是否已读状态修改</summary>
        public static void GetMessagesIsReadUpdate(Int64 id, int isread)
        {
            Update(string.Format("isread={0}", isread), string.Format(" id ={0}", id));
        }

        /// <summary>根据邮件id 删除邮件</summary>
        public static void GetMessagesDelete(Int64 id)
        {
            Delete(string.Format("id ={0}", id));
        }


        /// <summary>根据邮件id集合 删除邮件</summary>
        public static bool GetMessagesDelete(IEnumerable<long> ids)
        {
            try
            {
                var _ids = string.Join(",", ids).ToArray();
                Delete(string.Format("id in ({0})", _ids));
                return true;
            }
            catch
            {
                return false;
            }
           
        }

        /// <summary>根据邮件id集合 查询邮件</summary>
        public static List<tg_messages> GetMessagesByIds(IEnumerable<long> ids)
        {
            var _ids = string.Join(",", ids).ToArray();
            return FindAll(string.Format("id in ({0})", _ids));
        }

        /// <summary>根据收件人和附件状态获取数据</summary>
        public static List<tg_messages> GetMessagesIsAttByUserId(Int64 userid, int isattachment)
        {
            return FindAll(string.Format(" receive_id={0} and isattachment={1} ", userid, isattachment), null, null, 0, 0);
        }
    }
}
