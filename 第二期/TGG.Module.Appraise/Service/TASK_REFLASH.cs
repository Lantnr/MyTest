using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;
using XCode;

namespace TGG.Module.Appraise.Service
{
    /// <summary>
    /// 家臣任务刷新
    /// 开发者：李德雁
    /// </summary>
    public class TASK_REFLASH
    {
        private static TASK_REFLASH _objInstance;

        public static TASK_REFLASH getInstance()
        {
            return _objInstance ?? (_objInstance = new TASK_REFLASH());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_GOLD, session, data);
        }

        public ASObject CommandStart(int goodstype, TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}家臣任务刷新--{1}", session.Player.User.player_name, "TASK_REFRESH");
#endif
            var user = session.Player.User.CloneEntity();
            var mytasks = tg_task.GetTaskQueryByType(session.Player.User.id, (int)TaskType.ROLE_TASK);

            var count = mytasks.Where(q => q.task_state == (int)TaskStateType.TYPE_UNRECEIVED).ToList().Count;
            if (count <= 0)
                return BuildData((int)ResultType.APPRAISE_NOTASKS, null);

            if (!CheckCoin(user, count, session))
                return BuildData((int)ResultType.BASE_PLAYER_COIN_ERROR, null);

            var newtasks = GetNewTasks(mytasks);  //得到新任务
            if (newtasks.Count != mytasks.Count) return BuildData((int)ResultType.APPRAISE_REFLASH_WRONG, null);
            tg_task.GetListSave(newtasks);
            return BuildData((int)ResultType.SUCCESS, newtasks);
        }

        /// <summary> 组装数据 </summary>
        private ASObject BuildData(int result, List<tg_task> tasks)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"task", tasks!=null ? Common.GetInstance().ConvertListAsObject(tasks) : null}
            };
            return new ASObject(dic);

        }

        /// <summary> 得到新的家臣任务 </summary>
        private tg_task GetNewTask(tg_task task, IEnumerable<tg_task> newtasks)
        {
            var baseids = newtasks.Select(q => q.task_id).ToList();
            var basetask = Variable.BASE_APPRAISE.Where(q => !baseids.Contains(q.id)).ToList();
            if (!basetask.Any()) return null;
            var indexlist = RNG.Next(0, basetask.Count - 1);
            return indexlist >= basetask.Count ? null : Common.GetInstance().BuildNewRoleTask(basetask[indexlist].id, task.user_id);
        }

        /// <summary>金钱处理 </summary>
        private bool CheckCoin(tg_user user, int count, TGGSession session)
        {
            var baseinfo = Variable.BASE_RULE.FirstOrDefault(q => q.id == "2007");
            if (baseinfo == null) return false;
            var cost = Convert.ToInt32(baseinfo.value) * count;
            if (user.coin < cost) return false;
            user.coin -= cost;
            user.Update();
            session.Player.User = user;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, session.Player.User);
            return true;
        }

        /// <summary>
        ///  //对任务进行循环刷新
        /// </summary>
        /// <param name="oldtasks">老的任务</param>
        /// <returns></returns>
        private List<tg_task> GetNewTasks(IEnumerable<tg_task> oldtasks)
        {
            var newtasks = new List<tg_task>();
            foreach (var item in oldtasks)
            {
                if (item.task_state == (int)TaskStateType.TYPE_UNFINISHED)//已经接受的任务不处理，未接受的任务进行随机刷新
                {
                    newtasks.Add(item);
                    continue;
                }
                var newone = GetNewTask(item, newtasks);
                if (newone == null) continue;
                newone.id = item.id;
                newtasks.Add(newone);
            }
            return newtasks;
        }

    }
}
