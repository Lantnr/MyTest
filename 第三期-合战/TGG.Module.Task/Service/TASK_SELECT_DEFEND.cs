using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share;
using TGG.Share.Fight;
using TGG.SocketServer;

namespace TGG.Module.Task.Service
{
    /// <summary>
    ///  触发站岗选择
    /// </summary>
    public class TASK_SELECT_DEFEND : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~TASK_SELECT_DEFEND()
        {
            Dispose();
        }
    
        #endregion

        //private static TASK_SELECT_DEFEND _objInstance;

        ///// <summary> TASK_SELECT_DEFEND单体模式 </summary>
        //public static TASK_SELECT_DEFEND GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new TASK_SELECT_DEFEND());
        //}

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value);
            var sceneid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value);
            var userid = session.Player.User.id;
            var taskinfo = Variable.TaskInfo.Where(q => q.GuardSceneId == sceneid && q.GuardCamp != session.Player.User.player_camp).ToList();
            if (!taskinfo.Any()) return BuildData((int)ResultType.TASK_CANCLE);
            if (type == 0)
            {
                var rivalid = GetFightUserId(taskinfo);
                (new Fight()).GeFight(userid, rivalid, FightType.ONE_SIDE, 0, false, true);
            }
            if (type == 1)
            {
                var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "2017");
                if (baserule != null)
                {
                    var user = session.Player.User.CloneEntity();
                    if (user.coin - Convert.ToInt32(baserule.value) < 0)
                        return BuildData((int)ResultType.BASE_PLAYER_COIN_ERROR);
                    user.coin -= Convert.ToInt32(baserule.value);
                    if (user.Update() > 0)
                        session.Player.User = user;
                    (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, session.Player.User);
                }
            }
            return BuildData((int)ResultType.SUCCESS);
        }


        /// <summary>
        /// 获取战斗用户
        /// </summary>
        /// <param name="taskinfo"></param>
        /// <returns></returns>
        private Int64 GetFightUserId(List<Variable.UserTaskInfo> taskinfo)
        {
            var index = RNG.Next(0, taskinfo.Count - 1);
            return taskinfo[index].userid;
        }
        private ASObject BuildData(int result)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result",result },
            };
            return new ASObject(dic);
        }

    }
}
