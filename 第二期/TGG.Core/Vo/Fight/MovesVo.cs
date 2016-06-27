using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluorineFx;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TGG.Core.Vo.Fight
{
    /// <summary>
    /// 出招Vo
    /// </summary>
    [Serializable]
    public class MovesVo: BaseVo, ICloneable
    {

        public MovesVo()
        {
            rolesA = new List<RoleFightVo>();
            rolesB = new List<RoleFightVo>();
            hitIds = new List<double>();
        }

        /// <summary> 攻击武将 id，dizinessIds 数组中有此武将时则直接旋转 </summary>
        public double attackId { get; set; }

        /// <summary> 是否使用奥义 </summary>
        public bool isMystery { get; set; }

        /// <summary> 是否使用秘技 </summary>
        public bool isSkill { get; set; }

        /// <summary>是否触发印</summary>
        public bool isYin { get; set; }

        /// <summary> 目标武将 id </summary>
        public List<double> hitIds { get; set; }

        /****************以下的A和B与 FightVo 中的A和B始终对应，不要因为轮流出手而变化****************/

        /// <summary> 攻击方武将战斗数据 vo ，顺序始终不变，长度固定为5，死亡武将用 null </summary>
        public List<RoleFightVo> rolesA { get; set; }

        /// <summary> 防守方武将战斗数据 vo ，顺序始终不变，长度固定为5，死亡武将用 null </summary>
        public List<RoleFightVo> rolesB { get; set; }

        /// <summary> 攻击方印数 </summary>
        public int yinA { get; set; }

        /// <summary> 防守方印数 </summary>
        public int yinB { get; set; }

       /// <summary> 回合数 </summary>
        public int times { get; set; }

        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public MovesVo CloneEntity()
        {
            return Clone() as MovesVo;
        }

        /// <summary>深拷贝</summary>
        public MovesVo CloneDeepEntity()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as MovesVo;
            }
        }

        #endregion
    }

      
}
