using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Enum
{
    /// <summary>
    /// Api枚举
    /// </summary>
    public enum ApiType
    {
        /// <summary>成功</summary>
        OK = 1,
        /// <summary>指令执行失败</summary>
        FAIL = -1,
        /// <summary>用户不存在</summary>
        NO_USER = -2,
        /// <summary>基表错误</summary>
        BASE_TABLE_ERROR = -3,
        /// <summary>数据库查询错误</summary>
        DB_ERROR = -4,
        /// <summary>数据库保存错误</summary>
        DB_SAVE_ERROR = -5,
    }
}
