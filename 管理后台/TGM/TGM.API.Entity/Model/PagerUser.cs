using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 账号分页
    /// </summary>
    public class PagerUser : BaseEntity
    {
        public PagerInfo Pager { get; set; }
        /// <summary>用户集合</summary>
        public List<User> Users { get; set; }
    }
}
