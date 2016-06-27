using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Vo.Duplicate
{
    /// <summary>
    /// 爬塔Vo
    /// </summary>
    public class TowerPassVo : BaseVo
    {
        /// <summary>编号</summary>
        public double id { get; set; }

        /// <summary>基表id</summary>
        public int baseid { get; set; }

        /// <summary>挑战对象  1:小游戏	2:战斗</summary>
        public int enemyType { get; set; }

        /// <summary>对象id</summary>
        public int enemyId { get; set; }

        /// <summary>翻将次数</summary>
        public int challengeNum { get; set; }

        /// <summary>刷新怪物id集</summary>
        public List<int> refreshEnemyId { get; set; }

        /// <summary>是否为塔主</summary>
        public int towerHost { get; set; }
    }
}
