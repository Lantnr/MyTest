using System;

namespace TGG.Core.Base
{
    /// <summary>
    /// 固定规则
    /// </summary>
    //[Serializable]
    public class BaseRule : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseRule CloneEntity()
        {
            return Clone() as BaseRule;
        }

        #endregion

        /// <summary>模块+序号</summary>
        public string id { get; set; }
        /// <summary>
        /// 规则
        /// 函数说明：(用法:Math.floor(x),表示x值向下取整)
        /// 向下取整:Math.floor
        /// 向上取整:Math.ceil
        /// 四舍五入:Math.round
        /// 取绝对值:Math.abs
        /// 取平方根:Math.sqrt
        /// 取随机数:Math.random(用法:Math.random() * 10,表示取10以内的随机数)
        /// </summary>
        public string value { get; set; }
    }
}
