namespace TGG.Core.Vo.Role
{
    /// <summary>
    /// 家臣称号Vo
    /// </summary>
    public class TitleVo : BaseVo
    {
        /// <summary>
        /// 编号id
        /// </summary>
        public double id { get; set; }

        /// <summary>
        /// 基础数据编号
        /// </summary>
        public int baseId { get; set; }

        /// <summary>
        /// 称号达成状态  0：未达成；1：已达成
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// 称号装备状态  0：未装备；1：已装备
        /// </summary>
        public int load_state { get; set; }

        /// <summary>
        /// 称号解锁格子数
        /// </summary>
        public int packetCount { get; set; }

        /// <summary>
        /// 格子1装备武将主键Rid
        /// </summary>
        public double packet1_role { get; set; }

        /// <summary>
        /// 格子2装备武将主键Rid
        /// </summary>
        public double packet2_role { get; set; }

        /// <summary>
        /// 格子3装备武将主键Rid
        /// </summary>
        public double packet3_role { get; set; }
    }
}
