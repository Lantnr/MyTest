using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TGG.Core.Vo.Fight;
using TGG.Core.Vo.Role;

namespace TGG.Core.Entity
{
    /// <summary> 战斗使用的武将实体 </summary>
     [Serializable]
    public class FightRole : ICloneable
    {
        public FightRole()
        {
            buffVos = new List<BuffVo>();
            buffVos2 = new List<BuffVo>();
            foreverBuffVos = new List<BuffVo>();
        }

        /// <summary>武将主键</summary>
        public Int64 id { get; set; }

        /// <summary>基础 id </summary>
        public int baseId { get; set; }

        /// <summary> 怪物类型  0人物  1怪物 </summary>
        public int monsterType { get; set; }

        /// <summary> 奥义</summary>
        public SkillVo mystery { get; set; }

        /// <summary>秘技</summary>
        public SkillVo cheatCode { get; set; }

        /// <summary>伤害 </summary>
        public Int64 damage { get; set; }

        /// <summary> 生命 </summary>
        public Int64 hp { get; set; }

        // <summary> 初始血量 </summary>
        public Int64 initHp { get; set; }

        /// <summary>攻击</summary>
        public Double attack { get; set; }

        /// <summary>防御 </summary>
        public Double defense { get; set; }

        /// <summary> 增伤 </summary>
        public Double hurtIncrease { get; set; }

        /// <summary>减伤</summary>
        public Double hurtReduce { get; set; }

        /// <summary> 会心几率 </summary>
        public Double critProbability { get; set; }

        /// <summary> 会心加成  </summary>
        public Double critAddition { get; set; }

        /// <summary> 闪避几率 </summary>
        public Double dodgeProbability { get; set; }

        /// <summary>奥义触发几率</summary>
        public Double mystery_probability { get; set; }

        /// <summary> 无视闪避几率 </summary>
        public Double IgnoreDuck { get; set; }

        /// <summary> 气力值 </summary>
        public int angerCount { get; set; }

        /// <summary>持续技能Buff [BuffVo]</summary>
        public List<BuffVo> buffVos { get; set; }

        /// <summary>新增技能Buff [BuffVo]</summary>
        public List<BuffVo> buffVos2 { get; set; }

        /// <summary>永久Buff BuffVo</summary>
        public List<BuffVo> foreverBuffVos { get; set; }

        /// <summary> 等级</summary>
        public int lv { get; set; }

        /// <summary> 用户id</summary>
        public Int64 user_id { get; set; }

        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public FightRole CloneEntity()
        {
            return Clone() as FightRole;
        }

        /// <summary>深拷贝</summary>
        public FightRole CloneDeepEntity()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as FightRole;
            }
        }

        #endregion
    }
}
