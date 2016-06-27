using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 身份分页信息
    /// </summary>
    public class PagerServerIdentity : BaseEntity
    {
        /// <summary>分页信息</summary>
        public PagerInfo Pager { get; set; }

        /// <summary>分页信息集合</summary>
        public List<IdentitySpread> ServerIdentitys { get; set; }
    }
}
