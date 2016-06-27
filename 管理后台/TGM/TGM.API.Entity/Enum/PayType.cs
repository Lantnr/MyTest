using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGM.API.Entity.Enum
{
    /// <summary>
    /// 支付类型
    /// </summary>
    public enum PayType
    {
        /// <summary>人民币</summary>
        RMB=0,
        /// <summary>平台1:1点</summary>
        RATE_1 =1,
        /// <summary>平台1:10点</summary>
        RATE_10 = 2,
        /// <summary>平台1:100点</summary>
        RATE_100 = 3,
        /// <summary>平台1:1000点</summary>
        RATE_1000 = 4,
    }
}