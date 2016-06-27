using System;
using System.Collections.Generic;
using TGG.Core.Enum.Type;

namespace TGG.Core.Entity
{
    public class Siege
    {
        public Siege()
        {
            BaseData = new BaseData();
            PlayerData = new List<SiegePlayer>();
            BossCondition = new List<CampCondition>();
            IsOpen = false;
        }

        /// <summary> 活动玩家数据 </summary>
        public List<SiegePlayer> PlayerData { get; set; }

        /// <summary> 活动BOSS数据 </summary>
        public List<CampCondition> BossCondition { get; set; }

        /// <summary> 活动基础数据 </summary>
        public BaseData BaseData { get; set; }

        /// <summary> 活动是否开启 </summary>
        public bool IsOpen { get; set; }
    }

    /// <summary> 基表数据 </summary>
    public class BaseData
    {
        /// <summary> 东军默认出生点X </summary>
        public int EastBirthX { get; set; }

        /// <summary> 东军默认出生点Y </summary>
        public int EastBirthY { get; set; }

        /// <summary> 西军默认出生点X </summary>
        public int WestBirthX { get; set; }

        /// <summary> 西军默认出生点Y </summary>
        public int WestBirthY { get; set; }

        /// <summary> 制造云梯时间(毫秒) </summary>
        public Int64 MakeLadderTime { get; set; }

        /// <summary> 破坏城门时间(毫秒) </summary>
        public int GateTime { get; set; }

        /// <summary> 每日活动开始时间 </summary>
        public DateTime ActivityTime { get; set; }

        /// <summary> 活动结束时间 </summary>
        public DateTime ActivityEndTime { get; set; }

        /// <summary> 活动持续时间（分钟） </summary>
        public int ActivityDuration { get; set; }

        /// <summary> 最后战斗胜利的玩家获得声望奖励 </summary>
        public int WinFame { get; set; }

        /// <summary> 系统保护时间（毫秒）</summary>
        public Int64 ProtectTime { get; set; }

        /// <summary>攻击城门消耗云梯数量</summary>
        public int GateLadder { get; set; }

        /// <summary>攻击大将消耗云梯数量</summary>
        public int BossLadder { get; set; }

        /// <summary>战斗一次出手的时间(毫秒)</summary>
        public int AShotTime { get; set; }
    }

    /// <summary> 活动玩家实体 </summary>
    public class SiegePlayer
    {
        public SiegePlayer()
        {
            ismatching = false;
            //isSiege = true;
        }

        /// <summary> 云梯数量 </summary>
        public int count { get; set; }

        /// <summary> 上一次制作云梯时间 </summary>
        public double time { get; set; }

        /// <summary> 上一次攻城时间 </summary>
        public double gatetime { get; set; }

        /// <summary> 上一次攻击boss时间 </summary>
        public double bosstime { get; set; }

        /// <summary> 是否匹配过（攻城用） </summary>
        public bool ismatching { get; set; }

        /// <summary>阵营</summary>
        public int player_camp { get; set; }

        /// <summary>状态 1:普通 2:守城 3:休息中(守城不被匹配用)</summary>
        public int state { get; set; }

        /// <summary>对大将造成伤害</summary>
        public Int64 hurt { get; set; }

        /// <summary>声望</summary>
        public int fame { get; set; }

        /// <summary>玩家Id</summary>
        public Int64 user_id { get; set; }
    }

    /// <summary> 阵营情况 </summary>
    public class CampCondition
    {
        /// <summary>阵营 0:东军 1:西军</summary>
        public int player_camp { get; set; }

        /// <summary> 城门生命 </summary>
        public int GateLife { get; set; }

        /// <summary> Boss生命 </summary>
        public Int64 BossLife { get; set; }

        /// <summary> 本丸生命 </summary>
        public int BaseLife { get; set; }
    }
}
