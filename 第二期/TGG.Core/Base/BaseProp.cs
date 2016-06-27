using System;

namespace TGG.Core.Base
{
    /// <summary>
    ///道具基表
    /// </summary>
    //[Serializable]
    public class BaseProp : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseProp CloneEntity()
        {
            return Clone() as BaseProp;
        }

        #endregion

        /// <summary>道具编号</summary>
        public int id { get; set; }

        /// <summary>绑定</summary>
        public int bind { get; set; }

        /// <summary>价格</summary>
        public int price { get; set; }

        /// <summary>品质</summary>
        public int grade { get; set; }

        /// <summary>物品类型</summary>
        public int type { get; set; }

        /// <summary>物品子类型</summary>
        public int typeSub { get; set; }

        /// <summary>等级</summary>
        public int lv { get; set; }

        /// <summary>使用等级</summary>
        public int useLevel { get; set; }

        /// <summary>使用方式</summary>
        public int useMode { get; set; }

        /// <summary>可否交易</summary>
        public int trade { get; set; }

        /// <summary>可否销毁</summary>
        public int destroy { get; set; }

        /// <summary>可否出售</summary>
        public int sell { get; set; }

        /// <summary>可否叠加</summary>
        public int overlay { get; set; }

        /// <summary>获取时是否公告</summary>
        public int notice { get; set; }

        /// <summary>
        /// 基础属性加成
        /// 属性类型_属性值
        /// 多个属性用|隔开如：1_100|2_300
        /// 属性类型：
        /// 1、生命 2、近攻击 3、远攻击 4、近防御 5、远防御 6、敏捷 7、武力 
        /// 8、统帅 9、暴击 10、闪避 11、格挡 12、破防 13、韧性 14、命中
        /// </summary>
        public string baseAtt { get; set; }

        /// <summary>包含资源</summary>
        public string containsResources { get; set; }

        /// <summary>时效</summary>
        public int aging { get; set; }

        /// <summary>技能</summary>
        public int skillId { get; set; }

        /// <summary>关联武将id</summary>
        public int roleId { get; set; }

        /// <summary>可否合成</summary>
        public int make { get; set; }

        /// <summary>合成消耗数量</summary>
        public int consumecount { get; set; }

        /// <summary>合成目标Id</summary>
        public decimal targetId { get; set; }

        /// <summary>使用声望</summary>
        public int fame { get; set; }

        /// <summary>可否展示</summary>
        public int show { get; set; }

        /// <summary>流派</summary>
        public int genre { get; set; }
    }
}
