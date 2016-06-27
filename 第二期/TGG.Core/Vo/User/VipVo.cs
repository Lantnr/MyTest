using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.User
{
    /// <summary>
    /// 前端Vip VO类
    /// </summary>
    public class VipVo : BaseVo
    {

        /// <summary>当前vip等级</summary>
        public int level { get; set; }


        /// <summary>当前充值元宝</summary>
        public int costGold { get; set; }
    }
}
