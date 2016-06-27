using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Task;
using TGG.SocketServer;

namespace TGG.Module.Task.Service
{
    /// <summary>
    /// 搜寻宝物验证
    /// </summary>
    public class TASK_SEARCH
    {
        public static TASK_SEARCH ObjInstance = null;

        /// <summary> TASK_SEARCH单体模式 </summary>
        public static TASK_SEARCH getInstance()
        {
            return ObjInstance ?? (ObjInstance = new TASK_SEARCH());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var task = tg_task.GetEntityByStepType(session.Player.User.id, (int)TaskStepType.SEARCH_GOODS);
            if (task == null || task.task_type != (int)TaskType.VOCATION_TASK)
                return new ASObject(BulidData((int)ResultType.TASK_VOCATION_NOTASK, null, false));
            var istrue = (new Share.TGTask()).IsTaskSuccess(session.Player.Role, (int)TaskStepType.SEARCH_GOODS);
            if (!istrue) //采集失败
            {
                CheckIsInPrison(session.Player.User.id);
                return new ASObject(BulidData((int)ResultType.SUCCESS, task, istrue));
            }
            if (!CheckSearch(task))
                return new ASObject(BulidData((int)ResultType.TASK_VOCATION_UPDATEWRONG, null, istrue));

            return new ASObject(BulidData((int)ResultType.SUCCESS, task, istrue));
        }

        /// <summary>
        /// 验证是否进监狱
        /// </summary>
        /// <param name="userid"></param>
        private void CheckIsInPrison(Int64 userid)
        {
            var taskinfo = Common.getInstance().GetWorkInfo(userid);
            taskinfo.SearchFailTimes++;
            if (taskinfo.SearchFailTimes < 3) return;
            if (taskinfo.SearchFailTimes > 3) taskinfo.SearchFailTimes = 3;
            new Share.Prison().PutInPrison(userid);
        }

        /// <summary>
        /// 搜寻宝物更新
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private bool CheckSearch(tg_task task)
        {
            var steplist = task.task_step_data.Split('_');
            var count = Convert.ToInt32(steplist[2]);
            var needcount = 3;
            count++;
            if (count > needcount) return false;
            steplist[2] = count.ToString();
            var newstep = string.Join("_", steplist);
            task.task_step_data = newstep;
            if (count == needcount)
                task.task_state = (int)TaskStateType.TYPE_REWARD;
            task.Update();
            return true;

        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="result"></param>
        /// <param name="newtask">任务实体</param>
        /// <param name="issucccess">0：失败 1：成功</param>
        /// <returns></returns>
        private Dictionary<String, Object> BulidData(int result, tg_task newtask, bool issucccess)
        {
            var taskvo = new VocationTaskVo();
            var dic = new Dictionary<string, object>();
            if (newtask != null)
                taskvo = EntityToVo.ToVocationTaskVo(newtask);
            dic.Add("result", result);
            dic.Add("taskVo", newtask == null ? null : taskvo);
            dic.Add("type", issucccess ? 1 : 0);
            return dic;
        }

    }
}
