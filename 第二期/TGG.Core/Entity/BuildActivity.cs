using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Core.Entity
{
    public class BuildActivity : ICloneable
    {

        public BuildActivity()
        {
            userGoods = new ConcurrentDictionary<long, UserGoods>();
            isover = true;
        }

        /// <summary>用户活动物品</summary>
        public ConcurrentDictionary<Int64, UserGoods> userGoods { get; set; }

        /// <summary>活动是否结束</summary>
        public bool isover { get; set; }

        /// <summary>
        /// 东军boss血量
        /// </summary>
        public Int64 EastBoosBlood { get; set; }

        /// <summary> 西军boss血量</summary>
        public Int64 WestBoosBlood { get; set; }

        /// <summary> 东军城池默认耐久度</summary>
        public int EastCityBlood { get; set; }

        /// <summary> 东军城池默认耐久度</summary>
        public int WestCityBlood { get; set; }

        /// <summary> 西军bossid </summary>
        public int WestBossId { get; set; }

        /// <summary> 东军bossid </summary>
        public int EastBossId { get; set; }


        /// <summary> 东军出生点x </summary>
        public int EastBornPointX { get; set; }

        /// <summary> 东军出生点y </summary>
        public int EastBornPointY { get; set; }

        /// <summary> 西军出生点X </summary>
        public int WestBornPointX { get; set; }

        /// <summary> 西军出生点Y </summary>
        public int WestBornPointY { get; set; }


        /// <summary> 采集木材时间</summary>
        public int WoodTime { get; set; }

        /// <summary> 合成建材时间</summary>
        public int MakeBuildTime { get; set; }

        /// <summary> 采集火把时间</summary>
        public int TorchTime { get; set; }

        /// <summary>放火时间</summary>
        public int FireTime { get; set; }

        /// <summary> 筑城时间</summary>
        public int BuildTime { get; set; }

        /// <summary> 木材上限</summary>
        public int WoodFull { get; set; }

        /// <summary> 火把上限</summary>
        public int TorchFull { get; set; }

        /// <summary> 建材上限</summary>
        public int MakeBuildFull { get; set; }

        /// <summary> 城池耐久度上限</summary>
        public int CityBloodFull { get; set; }


        /// <summary> 活动获得声望上限</summary>
        public int FameFull { get; set; }

        /// <summary> 合成建材成功消耗的木材</summary>
        public int CostWood { get; set; }

        /// <summary> 筑城成功消耗的建材</summary>
        public int CostMakeWood { get; set; }

        /// <summary> 放火成功消耗的火把</summary>
        public int CostFire { get; set; }

        /// <summary> 筑城成功增加的声望 </summary>
        public int BuildAddFame { get; set; }

        /// <summary> 合成建材增加的声望 </summary>
        public int MakeAddFame { get; set; }

        /// <summary> 活动开始时间 </summary>
        public DateTime StartTime { get; set; }

        /// <summary> 活动时间时间 </summary>
        public int PlayTime { get; set; }

        /// <summary> 活动结束时间 </summary>
        public DateTime EndTime { get; set; }

        /// <summary> 放火减少的耐久度 </summary>
        public int ReduceBlood { get; set; }

        /// <summary> 击杀boss增加的声望 </summary>
        public int KillAddFame { get; set; }

        /// <summary>
        /// 用户物品类
        /// </summary>
        public class UserGoods
        {
            public UserGoods()
            {
                woodTime = new DateTime();
                woodTime = new DateTime();
                MakeBuildTime = new DateTime();
                BuildTime = new DateTime();
                FireTime = new DateTime();
                TorchTime = new DateTime();
                FightTime = new DateTime();
            }

            /// <summary> 木材 </summary>
            public int wood { get; set; }

            /// <summary> 总木材 </summary>
            public int totalwood { get; set; }

            /// <summary> 上一次采集木头时间 </summary>
            public DateTime woodTime { get; set; }

            /// <summary> 建材 </summary>
            public int basebuild { get; set; }

            /// <summary> 总建材 </summary>
            public int totalbasebuild { get; set; }

            /// <summary> 上一次合成建材时间 </summary>
            public DateTime MakeBuildTime { get; set; }

            /// <summary> 声望</summary>
            public int fame { get; set; }

            /// <summary> 火把 </summary>
            public int torch { get; set; }

            /// <summary> 总火把 </summary>
            public int totaltorch { get; set; }

            /// <summary> 上一次筑城时间</summary>
            public DateTime BuildTime { get; set; }

            /// <summary> 上一次放火时间 </summary>
            public DateTime FireTime { get; set; }

            /// <summary> 上一次收集火把的时间 </summary>
            public DateTime TorchTime { get; set; }

            /// <summary> 用户id </summary>
            public Int64 user_id { get; set; }

            /// <summary> 下一次可以战斗时间 </summary>
            public DateTime FightTime { get; set; }

            /// <summary> 阵营 </summary>
            public int camp { get; set; }


        }

        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BuildActivity CloneEntity()
        {
            return Clone() as BuildActivity;
        }

        #endregion

    }
}
