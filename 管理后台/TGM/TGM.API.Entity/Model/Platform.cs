using System;
using System.ComponentModel.DataAnnotations;

namespace TGM.API.Entity.Model
{
    /// <summary>后台平台表</summary> 
    public class Platform : BaseEntity
    {
        #region 属性
        /// <summary>编号</summary>
        public Int32 id { get; set; }

        /// <summary>令牌</summary>
        public Guid token { get; set; }

        /// <summary>平台名称</summary>
        public String name { get; set; }

        /// <summary>加密字符串</summary>
        public String encrypt { get; set; }

        /// <summary>创建时间</summary>
        public Int64 createtime { get; set; }

       

        #endregion


    }
}