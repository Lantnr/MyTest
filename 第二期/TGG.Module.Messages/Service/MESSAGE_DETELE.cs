using FluorineFx;
using NewLife.Log;
using System;
using System.Linq;
using TGG.Core.Enum.Type;
using tg_messages = TGG.Core.Entity.tg_messages;

namespace TGG.Module.Messages.Service
{
    /// <summary>
    /// 删除邮件
    /// Author:arlen xiao
    /// </summary>
    public class MESSAGE_DETELE
    {
        private static MESSAGE_DETELE ObjInstance;

        /// <summary>MESSAGE_DETELE单体模式</summary>
        public static MESSAGE_DETELE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new MESSAGE_DETELE());
        }

        /// <summary>删除邮件</summary>
        public ASObject CommandStart(SocketServer.TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "MESSAGE_DETELE", "删除邮件");
#endif
            /*
             * type:[int] 0：当前邮件 1:所有邮件
             * ids:[List<double>]邮件主键集合(全部删除时为空)
             */
            //var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value);
            var _type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value);
            var _ids = data.FirstOrDefault(q => q.Key == "ids").Value as object[];
            //_type = 1;
#if DEBUG        
            XTrace.WriteLine("type {0} (0：当前邮件 1:所有邮件)", _type);
#endif
            switch (_type)
            {
                case (int)MessageSelectType.SELECT:
                    {
                        if (_ids == null) return Common.GetInstance().BuildData((int)ResultType.MESSAGE_SUBMIT_DATA_ERROR);
                        var ids = _ids.Select(Convert.ToInt64).ToList();
                        //var list = tg_messages.GetMessagesByIds(ids);
                        //if (!list.Any()) return Common.GetInstance().BuildData((int)ResultType.MESSAGE_DATA_NO_EXIST_ERROR);
                        //var ids_is = list.Where(m => m.isattachment == (int)MessageIsAnnexType.UN_ANNEX).Select(m => m.id).ToList();
                        //if (!ids_is.Any()) return Common.GetInstance().BuildData((int)ResultType.MESSAGE_NO_DELETE_ATTACHMENT_ERROR);
                        if (!tg_messages.GetMessagesDelete(ids, (int)MessageIsAnnexType.UN_ANNEX)) return Common.GetInstance().BuildData((int)ResultType.DATABASE_ERROR);
                    }
                    break;
                case (int)MessageSelectType.ALL:
                    if (!tg_messages.GetMessagesDeleteByUser(session.Player.User.id, (int)MessageIsAnnexType.UN_ANNEX))
                        return Common.GetInstance().BuildData((int)ResultType.DATABASE_ERROR);
                    break;
            }
            return Common.GetInstance().BuildData((int)ResultType.SUCCESS);
        }
    }
}
