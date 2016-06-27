using System;
using TGG.Core.Vo.Fight;

namespace TGG.Core.Entity
{
    /// <summary>战斗使用的个人战实体</summary>
    public class FightPersonal
    {
        /// <summary>主键id</summary>
        public Int64 id { get; set; }

        /// <summary>玩家编号</summary>
        public Int64 user_id { get; set; }

        /// <summary>YinVo</summary>
        public YinVo yinvo { get; set; }

        /// <summary>阵法位置1武将表编号</summary>
        public Int64 matrix1_rid { get; set; }

        /// <summary>阵法位置2武将编号</summary>
        public Int64 matrix2_rid { get; set; }

        /// <summary>阵法位置3武将编号</summary>
        public Int64 matrix3_rid { get; set; }

        /// <summary>阵法位置4武将编号</summary>
        public Int64 matrix4_rid { get; set; }

        /// <summary>阵法位置5武将编号</summary>
        public Int64 matrix5_rid { get; set; }
    }
}
