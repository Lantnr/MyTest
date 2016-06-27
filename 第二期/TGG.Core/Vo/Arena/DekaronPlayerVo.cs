using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Arena
{
    /// <summary>
    /// 竞技场-可挑战玩家Vo
    /// </summary>
    public class DekaronPlayerVo : BaseVo
    {
        /// <summary>主键</summary>
        public double id { get; set; }

        /// <summary>玩家Id</summary>
        public decimal playId { get; set; }

        /// <summary>玩家昵称</summary>
        public string name { get; set; }

        /// <summary>玩家等级</summary>
        public int level { get; set; }

        /// <summary>玩家性别</summary>
        public int sex { get; set; }

        /// <summary>玩家职业</summary>
        public int vocation { get; set; }

        /// <summary>玩家排名</summary>
        public int arenaRank { get; set; }
    }
}
