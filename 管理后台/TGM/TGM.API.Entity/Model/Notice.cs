using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    public class Notice : BaseEntity
    {
        #region 属性

        /// <summary>公告编号</summary>
        public Int64 id { get; set; }

        ///// <summary>基表id</summary>
        //public Int32 base_Id { get; set; }

        /// <summary>开始时间</summary>
        public DateTime start_time { get; set; }

        /// <summary>结束时间</summary>
        public DateTime end_time { get; set; }

        ///// <summary>时间间隔</summary>
        //public Int32 time_interval { get; set; }

        /// <summary>内容</summary>
        public string content { get; set; }

        ///// <summary>状态</summary>
        //public Int32 state { get; set; }

        /// <summary>优先级别</summary>
        //public Int32 level { get; set; }

         ///<summary>服务器id</summary>
        public Int64 sid { get; set; }

        ///<summary>平台id</summary>
        public Int64 pid { get; set; }

        /// <summary>服务器名</summary>
        public string server { get; set; }

        /// <summary>平台名</summary>
        public string platform { get; set; }

        #endregion
    }
}
