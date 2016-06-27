using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Vo.Role;

namespace TGG.Core.Vo.Fight
{
    /// <summary>
    /// 战斗使用的武将Vo
    /// </summary>
    [Serializable]
    public class RoleFightVo : BaseVo
    {
        public RoleFightVo()
        {
            buffVos = new List<BuffVo>();
            buffVos2 = new List<BuffVo>();
        }

        /// <summary> 武将主键  </summary>
        public double id { get; set; }

        /// <summary> 基础 id  </summary>
        public int baseId { get; set; }

        /// <summary> 怪物类型  0人物  1怪物 </summary>
        public int monsterType { get; set; }

        /// <summary> 奥义 </summary>
        public int mystery { get; set; }

        /// <summary> 秘技 </summary>
        public int cheatCode { get; set; }

        /// <summary> 伤害 </summary>
        public Int64 damage { get; set; }

        /// <summary> 当前生命 </summary>
        public Int64 hp { get; set; }

        // <summary> 初始血量 </summary>
        public Int64 initHp { get; set; }

        /// <summary> 攻击 </summary>
        public int attack { get; set; }

        /// <summary> 防御 </summary>
        public int defense { get; set; }

        /// <summary> 增伤 </summary>
        public double hurtIncrease { get; set; }

        /// <summary> 减伤 </summary>
        public double hurtReduce { get; set; }

        /// <summary> 会心几率 </summary>
        public double critProbability { get; set; }

        /// <summary> 会心加成 </summary>
        public double critAddition { get; set; }

        /// <summary> 闪避几率 </summary>
        public double dodgeProbability { get; set; }

        /// <summary> 气力值 </summary>
        public int angerCount { get; set; }

        /// <summary> 持续技能Buff [BuffVo] </summary>
        public List<BuffVo> buffVos { get; set; }

        /// <summary> 新增技能Buff [BuffVo] </summary>
        public List<BuffVo> buffVos2 { get; set; }
    }
}
