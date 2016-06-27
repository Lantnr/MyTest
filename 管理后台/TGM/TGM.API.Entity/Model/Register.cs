using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 注册
    /// </summary>
    public class Register : BaseEntity
    {
        #region 属性
        /// <summary>角色主键编号</summary>
        public Int32 id { get; set; }

        /// <summary>用户主键编号</summary>
        public Int32 admin_id { get; set; }

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
