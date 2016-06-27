using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    public class Resource : BaseEntity
    {
        #region 属性

        /// <summary>资源编号</summary>
        public Int64 id { get; set; }

        /// <summary>申请时间</summary>
        public DateTime time { get; set; }

        /// <summary>申请类型</summary>
        public int type { get; set; }

        /// <summary>申请状态</summary>
        public int state { get; set; }

        /// <summary>申请原因</summary>
        public string content { get; set; }

        ///<summary>服务器id</summary>
        public Int64 sid { get; set; }

        ///<summary>平台id</summary>
        public Int64 pid { get; set; }

        /// <summary>服务器名</summary>
        public string server { get; set; }

        /// <summary>平台名</summary>
        public string platform { get; set; }

        /// <summary>补偿物品</summary>
        public string goods { get; set; }

        /// <summary>补偿物品</summary>
        public string player { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string operation { get; set; }

        #endregion
    }
}
