using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Vo.Scene
{
    /// <summary>
    /// 场景其他玩家数据
    /// </summary>
    public class ScenePlayerVo : SceneElementVo
    {
        /// <summary>
        /// 用户 id 
        /// </summary>
        public double id { get; set; }
        /// <summary>
        /// 性别 SexType  1:男，2：女，3：未知
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 职业 VocationType 1:法师
        /// </summary>
        public int vocation { get; set; }

        /// <summary>阵营 CampType </summary>
        public int camp { get; set; }

        /// <summary>
        /// 身份
        /// </summary>
        public int identityId { get; set; }  

    }
}
