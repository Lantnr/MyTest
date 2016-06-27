using System;

namespace TGM.API.Entity.Model
{
    /// <summary>用户信息</summary> 
	public class User : BaseEntity 
    {
        #region 属性        
        /// <summary>角色主键编号</summary>
        public Int32 id { get; set; }
        
        /// <summary>平台编号</summary>
        public Int32 pid { get; set; }

        /// <summary>平台名称</summary>
        public String platform_name { get; set; }
        
        /// <summary>用户名称</summary>
        public String name { get; set; }
        
        /// <summary>密码</summary>
        //public String password { get; set; }
        
        /// <summary>角色</summary>
        public Int32 role { get; set; }
        
        /// <summary>创建时间</summary>
        public Int64 createtime { get; set; }
        
        /// <summary>会话编号</summary>
        public Guid token { get; set; }     
        
		#endregion


    }
}