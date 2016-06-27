using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Rankings
{
    /// <summary>
    /// 富豪榜vo
    /// </summary>
    public class UserRankingVo : BaseVo
    {
        /// <summary>编号</summary>
        public double id { get; set; }

        /// <summary>用户id</summary>		
        public double userid { get; set; }

        /// <summary>0：其他玩家 1：自己</summary>
        public int oneself { get; set; }

        /// <summary>名次</summary>
        public int ranking { get; set; }

        /// <summary>角色名字</summary>
        public string name { get; set; }

        /// <summary>角色等级</summary>
        public int level { get; set; }

        /// <summary>阵营</summary>
        public int camp { get; set; }

        /// <summary>势力</summary>
        public int forces { get; set; }

        /// <summary>身份id</summary>
        public int Identity { get; set; }

        /// <summary>闯关数</summary>
        public int pass { get; set; }
    }
}
