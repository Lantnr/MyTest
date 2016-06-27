using System;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 邮件
    /// </summary>
    public class Email : BaseEntity
    {
        #region 属性

        /// <summary>邮件编号</summary>
        public Int64 id { get; set; }

        /// <summary>邮件标题</summary>
        public string title { get; set; }

        /// <summary>邮件内容</summary>
        public string content { get; set; }

        /// <summary>创建时间</summary>
        public Int64 createtime { get; set; }

        #endregion
    }
}
