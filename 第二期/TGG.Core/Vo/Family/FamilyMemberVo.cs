using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Family
{
    public class FamilyMemberVo : BaseVo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public double id { get; set; }

        /// <summary>
        /// 玩家编号
        /// </summary>
        public double userid { get; set; }

        /// <summary>
        /// 成员名字
        /// </summary>
        public string memberName { get; set; }

        /// <summary>
        /// 玩家等级
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// 身份
        /// </summary>
        public int degree { get; set; }

        /// <summary>
        /// 贡献
        /// </summary>
        public int devote { get; set; }

        /// <summary>
        /// 登陆时间
        /// </summary>
        public double debarkTime { get; set; }

        /// <summary>
        /// 是否在线	0:离线	1:在线
        /// </summary>      
        public int state { get; set; }
    }
}
