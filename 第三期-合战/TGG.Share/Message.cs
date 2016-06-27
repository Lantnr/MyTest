using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Share
{
    /// <summary>
    /// 邮箱共享类
    /// </summary>
    public partial class Message
    {

        #region 公共方法

        /// <summary>推送未读邮件数</summary>
        public void UnMessage(Int64 user_id)
        {
            var dic = new Dictionary<string, object>();
            var count = tg_messages.GetCountByUserId(user_id, (int)MessageIsReadType.UN_READ);
            dic.Add("number", count);
            MessageSend(user_id, new ASObject(dic));
        }

        /// <summary> 组装邮件实体并推送 </summary>
        /// <param name="userid">接收用户Id</param>
        /// <param name="title">邮件的title</param>
        /// <param name="contents">邮件的contents</param>
        /// <param name="list">奖励集合</param>
        public void BuildMessagesSend(Int64 userid, string title, string contents, List<RewardVo> list)
        {
            var attachment = list.Any() ? GetAttachment(list) : "";
            BuildMessagesSend(userid, title, contents, attachment);
        }

        /// <summary> 组装邮件实体并推送 </summary>
        /// <param name="userid">接收用户Id</param>
        /// <param name="title">邮件的title</param>
        /// <param name="contents">邮件的contents</param>
        /// <param name="attachment">附件 无附件填""</param>
        public void BuildMessagesSend(Int64 userid, string title, string contents, string attachment)
        {
            var type = (attachment != String.Empty) ? (int)MessageIsAnnexType.HAVE_ANNEX : (int)MessageIsAnnexType.UN_ANNEX;
            var model = new tg_messages
            {
                send_id = 0,
                title = title,
                isattachment = type,
                receive_id = userid,
                contents = contents,
                attachment = attachment,
                type = (int)MessageType.SYSTEM_MAIL,
                isread = (int)MessageIsReadType.UN_READ,
                create_time = (DateTime.Now.Ticks - 621355968000000000) / 10000,
            };
            model.Insert();
            try
            {
                //var temp = string.Format("{0}_{1}_{2}_{3}", "插入邮件:" + model.id, "标题:" + model.title, "附件为:" + model.attachment, "是否有附件状态:" + (model.isattachment == (int)MessageIsAnnexType.HAVE_ANNEX));
                //(new Log()).WriteLog(userid, (int)LogType.Insert, (int)ModuleNumber.MESSAGES, (int)MessageCommand.MESSAGE_PUSH, "邮件", "推送邮件", "邮件", 0, 0, 0, temp);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
            UnMessage(userid);
        }

        /// <summary> 邮件发送所有在线玩家 </summary>
        /// <param name="type">要发送的目标累in</param>
        /// <param name="title">邮件的title</param>
        /// <param name="contents">邮件的contents</param>
        /// <param name="attachment">附件 无附件添""</param>
        public void MessagesSendAll(string type, string title, string contents, string attachment)
        {
            var list = new List<Int64>();
            switch (type)
            {
                case "0": { list.AddRange(Variable.OnlinePlayer.Keys); break; }                       //在线玩家
                case "1": { list.AddRange(tg_user.FindAll().ToList().Select(m => m.id)); break; }     //所有玩家
                default:
                    {
                        try { list.AddRange(tg_user.GetEntityList(type).ToList().Select(m => m.id)); break; }
                        catch (Exception ex) { XTrace.WriteException(ex); return; }
                    }//自己订条件
            }

            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(n =>
                {
                    var userid = Convert.ToInt64(n);
                    BuildMessagesSend(userid, title, contents, attachment);
                    token.Cancel();
                }, item, token.Token);
            }
        }

        #endregion

        #region 私有方法

        /// <summary> 奖励集合组装成附件字段 </summary>
        /// <param name="list">奖励集合</param>
        private String GetAttachment(List<RewardVo> list)
        {
            //类型_数量
            //类型_id_数量
            //类型_id_是否绑定_数量
            //类型_id_是否绑定_数量_属性项(属性类型-属性值(多个属性用,分割)例:1-100,2-70)

            return new CommonBase().ToMessageAttachment(list);
        }

        /// <summary>发送组装数据</summary>
        private void MessageSend(Int64 user_id, ASObject data)
        {
            var b = Variable.OnlinePlayer.ContainsKey(user_id);
            if (!b) return;
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return;
            var pv = session.InitProtocol((int)ModuleNumber.MESSAGES, (int)MessageCommand.MESSAGE_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }


        #endregion

    }
}
