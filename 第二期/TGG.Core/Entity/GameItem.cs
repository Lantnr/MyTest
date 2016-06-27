using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JScript;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 游艺园小游戏类
    /// </summary>
    public class GameItem
    {
        public GameItem()
        {
            Tea = new Tea();
            Ninjutsu = new Ninjutsu();
            Calculate = new Calculate();
            Eloquence = new Eloquence();
            Spirit = new Spirit();
        }

        /// <summary>茶道数据 </summary>
        public Tea Tea { get; set; }

        /// <summary>猜宝数据</summary>
        public Ninjutsu Ninjutsu { get; set; }

        /// <summary>老虎机数据</summary>
        public Calculate Calculate { get; set; }

        /// <summary>辩驳数据</summary>
        public Eloquence Eloquence { get; set; }

        /// <summary>猎魂数据</summary>
        public Spirit Spirit { get; set; }

        /// <summary>游戏模式  0：闯关  1：练习</summary>
        public int GameType { get; set; }

        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public GameItem CloneEntity()
        {
            return Clone() as GameItem;
        }

        #endregion
    }

    /// <summary>
    /// 茶道游戏（花月茶道）
    /// </summary>
    public class Tea
    {
        /// <summary> 系统茶席值</summary>
        public int npc_tea { get; set; }

        /// <summary>玩家茶席值</summary>
        public int user_tea { get; set; }

        /// <summary>翻牌记录 </summary>
        public string select_position { get; set; }

        /// <summary>最初卡牌记录</summary>
        public string all_cards { get; set; }

        /// <summary>茶道当前关卡</summary>
        public int tea_pass { get; set; }
    }

    /// <summary>
    ///  忍术游戏（猜宝游戏）
    /// </summary>
    public class Ninjutsu
    {
        /// <summary>忍术当前关卡</summary>
        public int ninjutsu_pass { get; set; }
    }

    /// <summary>
    /// 算术游戏（老虎机）
    /// </summary>
    public class Calculate
    {
        /// <summary>算术当前关卡</summary>
        public int calculate_pass { get; set; }
    }

    /// <summary>
    /// 辩才游戏（辩驳游戏）
    /// </summary>
    public class Eloquence
    {
        /// <summary>系统气血值</summary>
        public int npc_blood { get; set; }

        /// <summary>玩家气血值</summary>
        public int user_blood { get; set; }

        /// <summary>辩才当前关卡</summary>
        public int eloquence_pass { get; set; }
    }

    /// <summary>
    /// 猎魂
    /// </summary>
    public class Spirit
    {
        /// <summary>猎魂当前关卡信息</summary>
        public int spirit_pass { get; set; }
    }
}
