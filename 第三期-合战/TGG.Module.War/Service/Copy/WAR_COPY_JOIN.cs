using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Copy
{
    /// <summary>
    /// 进入合战副本面板
    /// </summary>
    public class WAR_COPY_JOIN : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_COPY_JOIN()
        {
            Dispose();
        }
    
        #endregion
        //private static WAR_COPY_JOIN _objInstance;

        ///// <summary>WAR_COPY_JOIN单体模式</summary>
        //public static WAR_COPY_JOIN GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_COPY_JOIN());
        //}

        /// <summary> 进入合战副本面板 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {

            Variable.RemoveLine(session.Player.User.id, 2);//移除副本路线缓存

            if (!data.ContainsKey("id")) return null;
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);
            var userid = session.Player.User.id;
            var level = session.Player.Role.Kind.role_level;
            var identity = session.Player.Role.Kind.role_identity;

            var baseWarCopy = Variable.BASE_WAR_COPY.FirstOrDefault(m => m.id == id);
            var baseModuleOpen = Variable.BASE_MODULEOPEN.FirstOrDefault(m => m.id == (int)OpenModelType.合战副本);
            var baseIdentity = Variable.BASE_IDENTITY.FirstOrDefault(m => m.id == identity);
            if (baseWarCopy == null || baseModuleOpen == null || baseIdentity == null)
                return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);

            if (baseWarCopy.level > level) return CommonHelper.ErrorResult(ResultType.WAR_LEVEL_ERROR);
            if (baseIdentity.value < baseModuleOpen.identity) return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_IDENTITY_ERROR);

            var planId = GetRandomNumber(baseWarCopy.planId).First();
            if (planId == 0) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);

            session.Player.War.planId = planId;

            var model = tg_war_copy.GetFindByUserId(userid);
            if (model == null) model = InsertBuildWarCopy(userid);
            var temp = tg_war_copy_count.GetEntityBySceneId(userid, baseWarCopy.sceneId);
            if (temp == null) temp = InsertCopyCount(baseWarCopy.sceneId, userid);
            if (model == null || temp == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            if (!CheckCopyCount(temp)) return CommonHelper.ErrorResult(ResultType.WAR_COPY_COUNT_ERROR);

            return BuildData(model, planId, temp.count);
        }

        /// <summary> 切割字符串获取随机数 </summary>
        /// <param name="str">字符串</param>
        /// <param name="count">要获取的个数</param>
        private static IEnumerable<int> GetRandomNumber(string str, int count = 1)
        {
            var array = str.Split('|').Select(m => Convert.ToInt32(m)).ToList();
            return GetRandomNumber(array, count);
        }

        /// <summary> 切割字符串获取随机数 </summary>
        /// <param name="array">字符串</param>
        /// <param name="count">要获取的个数</param>
        private static IEnumerable<int> GetRandomNumber(IReadOnlyList<int> array, int count = 1)
        {
            var length = array.Count;
            if (length < count) return new List<int>();
            if (length == 1) return new List<int> { Convert.ToInt32(array[0]) };
            var number = RNG.Next(0, length - 1, count).ToList();
            return number.Select(n => array[n]).ToList();
        }

        private ASObject BuildData(tg_war_copy temp, int landId, int count)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", (int) ResultType.SUCCESS},
                {"zhen", count},
                {"forces", temp.forces},
                {"morale", temp.morale},
                {"hire", temp.hire_count},
                {"landId", landId}
            };
            return new ASObject(dic);
        }

        private static tg_war_copy InsertBuildWarCopy(Int64 userid)
        {
            var bingli = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32071");
            var shiqi = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32072");
            var gubingcishu = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32073");
            //var chuzhencishu = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32074");
            if (bingli == null || shiqi == null || gubingcishu == null) return null;
            var temp = new tg_war_copy
            {
                forces = Convert.ToInt32(bingli.value),
                hire_count = Convert.ToInt32(gubingcishu.value),
                morale = Convert.ToInt32(shiqi.value),
                //zhen_count = Convert.ToInt32(chuzhencishu.value),
                latest_time = DateTime.Now.Ticks,
                user_id = userid,
            };
            temp.Insert();
            return temp;
        }

        private static bool CheckCopyCount(tg_war_copy_count temp)
        {
            var time = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + " 00:00:00").Ticks;
            if (temp.time > time) return temp.count > 0;
            var chuzhencishu = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32074");
            if (chuzhencishu == null) return false;
            temp.count = Convert.ToInt32(chuzhencishu.value);
            temp.time = DateTime.Now.Ticks;
            temp.Update();
            return true;
        }

        private static tg_war_copy_count InsertCopyCount(int sceneId, Int64 userid)
        {
            var chuzhencishu = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32074");
            if (chuzhencishu == null) return null;
            var temp = new tg_war_copy_count
            {
                scene_id = sceneId,
                count = Convert.ToInt32(chuzhencishu.value),
                time = DateTime.Now.Ticks,
                user_id = userid,
            };
            temp.Insert();
            return temp;
        }
    }
}
