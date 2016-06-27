namespace TGG.Core.Vo.RoleTrain
{
    /// <summary>
    /// NpcMonsterVo 点将武将Vo
    /// </summary>
    public class NpcMonsterVo : BaseVo
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public double id { get; set; }

        /// <summary>
        /// 武将基表id
        /// </summary>
        public int baseId { get; set; }

        /// <summary>
        /// 挑战状态   0：未战胜；1：已战胜
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// 剩余魂数
        /// </summary>
        public int spirit { get; set; }

        /// <summary>
        /// 偷窃状态   0：未偷窃；1：已偷窃
        /// </summary>
        public int isSteal { get; set; }

        /// <summary>
        ///  武将限制    0：不限制；1：只能喝茶；2：只能挑战
        /// </summary>
        public int limit { get; set; }
    }
}
