using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Family
{
    public class FamilyVo : BaseVo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public double id { get; set; }

        /// <summary>
        /// 家族名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public int rankings { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public int number { get; set; }
        /// <summary>
        /// 资源
        /// </summary>
        public int resource { get; set; }

        /// <summary>
        /// 名望
        /// </summary>
        public int renown { get; set; }

        /// <summary>
        /// 公告
        /// </summary>
        public string notice { get; set; }

        /// <summary>
        /// 族徽id
        /// </summary>
        public int clanbadge { get; set; }

        /// <summary>家族成员Vo集合</summary>
        public List<FamilyMemberVo> familyMemberArrVo { get; set; }

        /// <summary>
        /// 是否领取俸禄	0:未领取	1:已领取
        /// </summary>
        public int daySalary { get; set; }

        /// <summary>
        /// 个人当天已捐献的金钱值
        /// </summary>
        public int dayDonate { get; set; }
    }
}
