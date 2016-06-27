using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Vo.Friend
{
    /// <summary>
    /// 好友Vo
    /// </summary>
    public class FriendVo : BaseVo
    {
        /// <summary>主键id</summary>
        public double id { get; set; }

        /// <summary>好友id</summary>
        public double friendid { get; set; }

        /// <summary>好友名字</summary>
        public string friendname { get; set; }

        /// <summary>职业 VocationType</summary>
        public int vocation { get; set; }

        /// <summary>性别</summary>
        public int sex { get; set; }

        /// <summary>等级</summary>
        public int level { get; set; }

        /// <summary>是否在线 0:不在线 1:在线</summary>
        public int isonline { get; set; }

        /// <summary>身份baseid</summary>
        public int identityid { get; set; }

        /// <summary> 好友状态 0:好友 1:黑名单</summary>
        public int groupState { get; set; }
    }
}
