using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 活动类
    /// </summary>
    public class ActivityItem
    {
        public ActivityItem()
        {
            BuildActivity = new BuildActivity();
            ScenePlayer = new ConcurrentDictionary<string, view_scene_user>();
            Siege = new Siege();
        }

       

        /// <summary> 美浓攻略 </summary>
        public Siege Siege { get; set; }

        /// <summary> 玩家活动内场景数据 </summary>
        public ConcurrentDictionary<string, view_scene_user> ScenePlayer { get; set; }

        /// <summary>一夜墨俣</summary>
        public BuildActivity BuildActivity { get; set; }

    }
}
