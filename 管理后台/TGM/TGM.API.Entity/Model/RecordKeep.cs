using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    public class RecordKeep
    {
        #region 属性
        /// <summary></summary>
        public Int64 id { get; set; }

        /// <summary>平台编号</summary>
        public Int32 pid { get; set; }

        /// <summary>服务器编号</summary>
        public Int32 sid { get; set; }

        /// <summary></summary>
        public String server_name { get; set; }

        /// <summary>30天登陆人数</summary>
        public Int32 login_30 { get; set; }

        /// <summary>1天留存率</summary>
        public Double keep_1 { get; set; }

        /// <summary>3天留存率</summary>
        public Double keep_3 { get; set; }

        /// <summary>5天留存率</summary>
        public Double keep_5 { get; set; }

        /// <summary>7天留存率</summary>
        public Double keep_7 { get; set; }

        /// <summary>30天留存率</summary>
        public Double keep_30 { get; set; }

        /// <summary>创建时间</summary>
        public String createtime { get; set; }

        #endregion

    }
}
