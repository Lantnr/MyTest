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
using TGG.Share.Fight;
using TGG.SocketServer;

namespace TGG.Module.Work.Service
{
    public class WORK_SELECT_DEFEND
    {
        private static WORK_SELECT_DEFEND _objInstance;

        /// <summary> WORK_SELECT_DEFEND单体模式 </summary>
        public static WORK_SELECT_DEFEND GetInstance()
        {
            return _objInstance ?? (_objInstance = new WORK_SELECT_DEFEND());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value);
            var sceneid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value);
            var taskinfo = Variable.WorkInfo.Where(q => q.GuardSceneId == sceneid && q.GuardCamp != session.Player.User.player_camp).ToList();
            var userid = session.Player.User.id;
            if (!taskinfo.Any()) return new ASObject(new Dictionary<string, object> { { "result", (int)ResultType.TASK_CANCLE }, }); ;
            if (type == 0)
            {
                var rivalid = GetFightUserId(taskinfo);
                var fight = new Fight().GeFight(userid, rivalid, FightType.ONE_SIDE, 0, false, true);
                new Fight().Dispose();
                if (fight.Result != ResultType.SUCCESS)
                    return new ASObject(new Dictionary<string, object> { { "result", (int)ResultType.FIGHT_ERROR }, });
            }
            if (type == 1)
            {
                var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "2017");
                if (baserule != null)
                {
                    var user = session.Player.User.CloneEntity();
                    if (user.coin - Convert.ToInt32(baserule.value) < 0)
                        return new ASObject(new Dictionary<string, object> { { "result", (int)ResultType.BASE_PLAYER_COIN_ERROR }, });
                    user.coin -= Convert.ToInt32(baserule.value);
                    if (user.Update() == 0)
                        session.Player.User = user;
                    (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, session.Player.User);
                }
            } return new ASObject(new Dictionary<string, object> { { "result", (int)ResultType.SUCCESS }, });
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
    }
}
