using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Vo.Equip
{
    /// <summary>
    /// 装备
    /// </summary>
    public class EquipVo : BaseVo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public double id { get; set; }

        /// <summary>
        /// 基础数据编号
        /// </summary>
        public double baseId { get; set; }

        /// <summary>
        /// 是否绑定
        /// </summary>
        public int bind { get; set; }

        /// <summary>
        /// 是否穿戴     0:没穿戴 1:穿戴 
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// 属性1
        /// </summary>
        public int att1 { get; set; }

        /// <summary>
        /// 属性2
        /// </summary>
        public int att2 { get; set; }

        /// <summary>
        /// 属性3
        /// </summary>
        public int att3 { get; set; }

        /// <summary>
        /// 属性1值
        /// </summary>
        public double value1 { get; set; }

        /// <summary>
        /// 属性2值
        /// </summary>
        public double value2 { get; set; }

        /// <summary>
        /// 属性3值
        /// </summary>
        public double value3 { get; set; }

        /// <summary>
        /// 属性1 魂等级
        /// </summary>
        public int lv1 { get; set; }

        /// <summary>
        /// 属性2 魂等级
        /// </summary>
        public int lv2 { get; set; }

        /// <summary>
        /// 属性3 魂等级
        /// </summary>
        public int lv3 { get; set; }

        /// <summary>
        /// 属性1 魂值
        /// </summary>
        public int spirit1 { get; set; }

        /// <summary>
        /// 属性2 魂值
        /// </summary>
        public int spirit2 { get; set; }

        /// <summary>
        /// 属性3 魂值
        /// </summary>
        public int spirit3 { get; set; }

        /// <summary>
        /// 属性一下一阶是否锁定     0:没解锁 1:解锁 
        /// </summary>
        public int isLock1 { get; set; }

        /// <summary>
        /// 属性二下一阶是否锁定     0:没解锁 1:解锁 
        /// </summary>
        public int isLock2 { get; set; }

        /// <summary>
        /// 属性三下一阶是否锁定     0:没解锁 1:解锁 
        /// </summary>
        public int isLock3 { get; set; }

        /// <summary>
        ///  剩余洗练次数
        /// </summary>
        public int sopCount { get; set; }

        #region   暂时保留字段

        public EquipVo()
        {
            inlayAttribute = new List<ASObject>();
        }

        /// <summary>
        /// 强化等级
        /// </summary>
        public int strengLv { get; set; }

        /// <summary>
        /// 极品属性集合
        /// 
        /// </summary>
        public string bestAttribute { get; set; }

        /// <summary>
        /// 镶嵌属性集合
        /// 宝石VO(道具Vo)集合
        /// </summary>
        public List<ASObject> inlayAttribute { get; set; }

        /// <summary>
        /// 战斗力
        /// </summary>
        public double fighting { get; set; }
        #endregion

    }
}
