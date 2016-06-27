using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Vo.Messages
{
    public class MessagesVo : BaseVo
    {
        /// <summary>主键</summary>
        public decimal id { get; set; }

        /// <summary>接收玩家id</summary>
        public decimal receive_id { get; set; }

        /// <summary>发送玩家id</summary>
        public decimal send_id { get; set; }

        /// <summary>发送玩家昵称</summary>
        public string send_playname { get; set; }

        /// <summary>消息类型 0:玩家邮件 1:系统邮件</summary>
        public int type { get; set; }

        /// <summary>标题</summary>
        public string title { get; set; }

        /// <summary>内容</summary>
        public string contents { get; set; }

        /// <summary>是否已读</summary>
        public int isread { get; set; }

        /// <summary>是否有附件</summary>
        public int isattachment { get; set; }

        /// <summary>附件</summary>
        public string attachment { get; set; }

        /// <summary>创建时间 年-月-日	时:分:秒</summary>
        public string create_time { get; set; }
    }
}
