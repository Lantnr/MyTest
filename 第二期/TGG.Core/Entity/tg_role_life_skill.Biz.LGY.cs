using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;

namespace TGG.Core.Entity
{
    public partial class tg_role_life_skill
    {
        /// <summary>
        /// 插入武将生活技能数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool GetInsert(tg_role_life_skill model)
        {
            try
            {
                model.Insert();
                return true;
            }
            catch
            { return false; }
        }

        /// <summary>
        /// 更新武将生活技能数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool GetUpdate(tg_role_life_skill model)
        {
            try
            {
                model.Update();
                return true;
            }
            catch
            { return false; }
        }

        /// <summary>根据时间操作tg_car</summary>
        //public static void GetEntityListByTime(Int64 time, Int64 userid)
        //{
        //    var list = FindAll(string.Format("state=1 and time<={0} and user_id={1}", time, userid), null, null, 0, 0);
        //    foreach (var item in list)
        //    {
        //        item.start_ting_id = item.stop_ting_id;
        //        item.state = (int)LifeSkillStudyConditionType.LEARNED;
        //        item.stop_ting_id = 0;
        //        item.distance = 0;
        //        item.time = 0;
        //    }
        //    list.Update();
        //}

        /// <summary>根据时间操作tg_role_life_skill</summary>
        public static List<tg_role_life_skill> GetEntityListRids(List<Int64> rids)
        {
            var _ids = string.Join(",", rids.ToArray());
            var where = string.Format("id in ({0})", _ids);
            return FindAll(where, null, null, 0, 0);
        }
    }
}
