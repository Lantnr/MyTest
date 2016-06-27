using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 评定刷新
    /// 开发者：李德雁
    /// </summary>
   public class TASK_VOCATION_REFRESH:IConsume
    {
        public ASObject Execute(Int64 userid, ASObject data)
        {
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.FAIL);
#if DEBUG
            XTrace.WriteLine("{0}职业任务刷新--{1}", session.Player.User.player_name, "TASK_VOCATION_REFRESH");
#endif
            var mytasks = tg_task.GetTaskQueryByType(session.Player.User.id, (int)TaskType.VOCATION_TASK);
            var newtasks = new List<tg_task>();
            var s_task = 0;

            var user = session.Player.User.CloneEntity();
            foreach (var item in mytasks)
            {
                var newone = GetNewTask(item, newtasks);
                if (newone == null) continue;
                var baseinfo = Variable.BASE_TASKVOCATION.FirstOrDefault(q => q.id == item.task_id);
                if (baseinfo == null) continue;
                if (baseinfo.type == 2)
                {
                    s_task++;
                    newtasks.Add(item); continue;//高级评定
                }
                newone.id = item.id;
                newtasks.Add(newone);
            }
            if (s_task == mytasks.Count) return BuildData((int)ResultType.TASK_VOCATION_FINISHED, null);//没有普通的评定任务
            if (newtasks.Count != mytasks.Count) return BuildData((int)ResultType.TASK_VOCATION_NOREFLASH, null);
            if (!CheckCoin(user, mytasks.Count - s_task, session))
                return BuildData((int)ResultType.BASE_PLAYER_COIN_ERROR, null);
            tg_task.GetListSave(newtasks);
            return BuildData((int)ResultType.SUCCESS, newtasks);
        }

        /// <summary> 组装数据 </summary>
        private ASObject BuildData(int result, List<tg_task> dailytask)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"vocationTask",dailytask==null?null: new Share.TGTask().ConvertListASObject(dailytask, "VocationTask")}
            };
            return new ASObject(dic);

        }

        /// <summary> 得到新的职业任务 </summary>
        private tg_task GetNewTask(tg_task task, List<tg_task> newtasks)
        {
            var baseids = newtasks.Select(q => q.task_id).ToList();
            var list = Variable.BASE_TASKVOCATION.
                FindAll(q => q.identity.Split(',').ToList().Contains(task.task_base_identify.ToString()))
                .Where(q => !baseids.Contains(q.id) && q.type == 1).ToList();
            if (!list.Any()) return null;
            var indexlist = RNG.Next(0, list.Count - 1);
            var step = list[indexlist].stepInit;
            var taskid = list[indexlist].id;
            var userid = task.user_id;
            var taskidentify = task.task_base_identify;
            var steptype = list[indexlist].stepType;
            return indexlist >= list.Count ? null : new Share.TGTask().BuildNewVocationTask(step, taskid, userid, taskidentify, steptype);
        }

        /// <summary>
        /// 金钱处理
        /// </summary>
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
    }
}
