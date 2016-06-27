using System;
using System.ComponentModel.DataAnnotations;

namespace TGM.API.Entity.Model
{
    /// <summary>用户报表</summary> 
    public class PlayerReport : BaseEntity 
    {
        #region 属性        
        /// <summary>编号</summary>
        public Int64 id { get; set; }
        
        /// <summary>当前总人数</summary>
        public Int32 total { get; set; }
        
        /// <summary>当前在线人数</summary>
        public Int32 online { get; set; }
        
        /// <summary>当前不在线人数</summary>
        public Int32 unonline { get; set; }
        
        /// <summary>当前时间</summary>
        public Int64 time { get; set; }
        
		#endregion


    }
}