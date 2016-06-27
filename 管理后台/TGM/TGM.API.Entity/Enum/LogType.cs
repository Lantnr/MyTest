using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 日志枚举类
    /// </summary>
    public enum LogType
    {
        /// <summary>服务器停止</summary>
        Stop = 0,
        /// <summary>服务器运行</summary>
        Run = 1,


        /// <summary>插入数据</summary>
        Insert = 100,
        /// <summary>删除数据</summary>
        Delete = 200,
        /// <summary>修改数据</summary>
        Update = 300,
        /// <summary>查询数据</summary>
        Select = 400,

        /// <summary>使用</summary>
        Use=1000,
        /// <summary>获取</summary>
        Get=2000
    }
}
