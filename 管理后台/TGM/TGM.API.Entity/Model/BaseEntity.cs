using System;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 实体基类
    /// </summary>
    //[Serializable]
    public class BaseEntity
    {
        /// <summary>结果状态</summary>
       public Int32 result { get; set; }

        /// <summary>消息信息</summary>
       public String message { get; set; }
    }
}
