using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Task.Service
{
    /// <summary>
    /// 检测是否触发站岗
    /// </summary>
    public class TASK_CHECK_DEFEND : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~TASK_CHECK_DEFEND()
        {
            Dispose();
        }
    
        #endregion
        //private static TASK_CHECK_DEFEND _objInstance;

        ///// <summary> TASK_CHECK_DEFEND单体模式 </summary>
        //public static TASK_CHECK_DEFEND GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new TASK_CHECK_DEFEND());
        //}

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var sceneid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value);
            var taskinfo = Variable.TaskInfo.Where(q => q.GuardSceneId == sceneid && q.GuardCamp != session.Player.User.player_camp).ToList();
            var type = taskinfo.Any() ? 1 : 0;
            return BuildData(type);
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="type">0：未触发 1：触发</param>
        /// <returns></returns>
        private ASObject BuildData(int type)
        {
            return new ASObject(
             new Dictionary<string, object>
            {
                {"result", (int) ResultType.SUCCESS},
                {"type", type}
            });
        }
    }
}
