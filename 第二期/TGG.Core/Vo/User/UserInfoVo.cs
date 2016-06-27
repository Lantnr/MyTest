using System;
using System.Collections.Generic;

namespace TGG.Core.Vo.User
{
    /// <summary>
    /// UserInfoVO 用户
    /// </summary>
    [Serializable]
    public class UserInfoVo : BaseVo
    {
        public UserInfoVo() { moduleIds = new List<int>(); }

        /// <summary>玩家ID</summary>
        public double id { get; set; }

        /// <summary>玩家名称</summary>
        public string playerName { get; set; }

        /// <summary>性别</summary>
        public int sex { get; set; }

        /// <summary>职业 VocationType </summary>
        public int vocation { get; set; }

        /// <summary>玩家当前势力 </summary>
        public int area { get; set; }

        /// <summary>阵营 CampType </summary>
        public int camp { get; set; }

        /// <summary>官位 </summary>
        public int positionId { get; set; }

        /// <summary> 魂</summary>
        public int spirit { get; set; }

        /// <summary>声望</summary>
        public int fame { get; set; }

        /// <summary>剩余点数</summary>
        public int growAddCount { get; set; }

        /// <summary> 金币</summary>
        public int gold { get; set; }

        /// <summary>货币</summary>
        public double coin { get; set; }

        /// <summary>内币</summary>
        public int rmb { get; set; }

        /// <summary>礼券</summary>
        public int coupon { get; set; }

        /// <summary>家族id</summary>
        public int familyId { get; set; }

        /// <summary>俸禄领取状态</summary>
        public int rewardState { get; set; }

        /// <summary>已激活的功能 id 数组 </summary>	
        public List<int> moduleIds { get; set; }

        /// <summary>vipvo</summary>
        public VipVo vipVo { get; set; }

        /// <summary>当前开启商圈</summary>
        public List<int> areas { get; set; }

        /// <summary>今日在线时长</summary>
        public int onlineTime { get; set; }

        /// <summary>防沉迷</summary>
        public int fcm { get; set; }

        /// <summary>大名令	0:未激活	1:已激活 </summary>
        public int dml { get; set; }

    }
}
