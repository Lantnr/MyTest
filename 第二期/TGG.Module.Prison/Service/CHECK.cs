using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Prison.Service
{
    /// <summary>
    /// 进入监狱检测
    ///开发者：李德雁
    /// </summary>
    public class CHECK
    {
        public static CHECK objInstance = null;

        /// <summary> CHECK单体模式 </summary>
        public static CHECK getInstance()
        {
            return objInstance ?? (objInstance = new CHECK());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}进入监狱检测", "CHECK", session.Player.User.player_name);
#endif
            const int result = (int)ResultType.SUCCESS;
            var userid = session.Player.User.id;
          
            var prison = tg_prison.GetPrisonByUserId(userid);
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var doubledate = date.Ticks;
            if (prison == null) return Common.GetInstance().BuildData(result, 0, 0, null);//不在监狱里
            var key = string.Format("{0}_{1}", (int)ModuleNumber.PRISON, userid);
            var now = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            #region 特殊情况处理，如中途停服，出狱时间已经过了
            if (now >= prison.prison_time) //验证出狱时间
            {
                prison.Delete();
                view_scene_user scene;
                if (Variable.SCENCE.ContainsKey(key)) Variable.SCENCE.TryRemove(key, out scene);//移除监狱数据
                var mapkey = string.Format("{0}_{1}", (int)ModuleNumber.SCENE, prison.user_id);
                if (Variable.SCENCE.ContainsKey(mapkey)) Variable.SCENCE[mapkey].model_number = (int)ModuleNumber.SCENE;
                return Common.GetInstance().BuildData(result, 0, 0, null);
            }
            #endregion
            var count = tg_prison_messages.GetUserMessageCount(doubledate, userid); //今日留言次数
            var times = (prison.prison_time - now);
            var prisonscene = session.Player.Scene.CloneEntity();
            prisonscene.model_number = (int)ModuleNumber.PRISON;
            new Share.Prison().GetPrisonPoint(prisonscene);
            Variable.SCENCE.AddOrUpdate(key, prisonscene, (m, n) => n);
            new Share.Prison().NewTaskStart(times, session.Player.User.id);
            new Share.Prison().PushEnterPrison(userid); //向监狱内玩家推送进入监狱
            var list = new Share.Prison().GetPrisonOhters(userid);
            return Common.GetInstance().BuildData(result, Convert.ToDouble(prison.prison_time), count, list);
        }


    }
}
