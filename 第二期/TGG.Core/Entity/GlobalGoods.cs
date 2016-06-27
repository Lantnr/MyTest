using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace TGG.Core.Entity
{
    /// <summary>
    /// 全局货物实体类
    /// </summary>

    [Serializable]
    public class GlobalGoods : ICloneable
    {
        /// <summary>
        /// 町id
        /// </summary>
        public int ting_id { get; set; }

        /// <summary>
        /// 货物id
        /// </summary>
        public int goods_id { get; set; }

        /// <summary>
        /// 买入价格
        /// </summary>
        public int goods_buy_price { get; set; }

        /// <summary>
        /// 卖出价格
        /// </summary>
        public int goods_sell_price { get; set; }

        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        /// <summary>
        /// 深拷贝
        /// </summary>
        public GlobalGoods DeepClone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as GlobalGoods;
            }
        }

        /// <summary>
        /// 浅拷贝
        /// </summary>
        public GlobalGoods CloneEntity()
        {
            return Clone() as GlobalGoods;
        }

        #endregion


    }
}
