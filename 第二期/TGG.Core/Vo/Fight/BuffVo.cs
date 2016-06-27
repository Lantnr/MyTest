using System;

namespace TGG.Core.Vo.Fight
{
    /// <summary>
    /// BuffVo
    /// </summary>
    [Serializable]
    public class BuffVo : BaseVo
    {
        /// <summary>
        /// buff类型
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// buff数值
        /// </summary>
        public double buffValue { get; set; }
    }
}
