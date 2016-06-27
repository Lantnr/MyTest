using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 福利卡报表
    /// </summary>
    public class ReportCode
    {
        ///// <summary>服务器名称</summary>
        //public String 服务器名称 { get; set; }

        /// <summary>平台名称</summary>
        public String 平台名称 { get; set; }

        /// <summary>福利卡类型</summary>
        public String 福利卡类型 { get; set; }

        /// <summary>激活码</summary>
        public String 激活码 { get; set; }

        /// <summary>生成序号</summary>
        public String 生成序号 { get; set; }
    }
}
