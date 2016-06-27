using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 邮箱实体业务操作类
    /// </summary>
    public partial class tg_messages
    {
        /// <summary>根据邮件人删除所有收件人邮件</summary>
        public static bool GetMessagesDeleteByUser(Int64 userid, int isattachment)
        {
            return Delete(new String[] { _.receive_id, _.isattachment }, new Object[] { userid, isattachment }) > 0;
        }

        /// <summary>根据附件和id删除邮件</summary>
        public static bool GetMessagesDelete(List<Int64> ids, int isattachment)
        {
            var exp = new WhereExpression();
            exp &= _.isattachment == isattachment;
            if (ids.Any()) exp &= _.id.In(ids);
            return Delete(exp) > 0;
        }

        /// <summary>更新附件信息</summary>
        public static bool GetMessagesUpdate(IEnumerable<view_messages> list, int isattachment, string attachment)
        {
            var _ids = string.Join(",", list.Select(m => m.id).ToArray());
            var _where = string.Format("id in ({0})", _ids);
            var _set = string.Format("isattachment={0},attachment='{1}'", isattachment, attachment);
            return Update(_set, _where) > 0;
        }

        /// <summary>更新多个对象</summary>
        public static bool GetMessagesUpdateList(IEnumerable<view_messages> list)
        {
            var list_update=new EntityList<tg_messages>();
            foreach (var item in list)
            {
                var entity = new tg_messages();
                entity.CopyFrom(item);
                list_update.Add(entity);
            }
            return list_update.Update() > 0;
        }

         /// <summary>更新单个附件信息</summary>
        public static bool GetMessagesUpdateSingle(Int64 id, int isattachment)
        {
            var _where = string.Format("id = ({0})", id);
            var _set = string.Format("isattachment={0},attachment=''", isattachment);
            return Update(_set, _where) > 0;
        }

    }
}
