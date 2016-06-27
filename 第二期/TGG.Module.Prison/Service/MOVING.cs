using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Prison.Service
{
    /// <summary>
    /// 移动
    /// </summary>
    public class MOVING
    {
        public static MOVING objInstance = null;

        /// <summary> MOVING单体模式 </summary>
        public static MOVING getInstance()
        {
            return objInstance ?? (objInstance = new MOVING());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}移动", "MOVING", session.Player.User.player_name);
#endif
            var x = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "x").Value);
            var y = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "y").Value);
            var userid = session.Player.User.id;
            if (x == 0 || y == 0)
                return Common.GetInstance().BuildData((int)ResultType.PRISON_POINT_ERROR);
            var key = string.Format("{0}_{1}", (int)ModuleNumber.PRISON, userid);
            if (!Variable.SCENCE.ContainsKey(key))
                return Common.GetInstance().BuildData((int)ResultType.PRISON_POINT_ERROR);
            Variable.SCENCE[key].X = x;
            Variable.SCENCE[key].Y = y;
           new Share.Prison().PusuMovingPrison(Variable.SCENCE[key]);
            return Common.GetInstance().BuildData((int)ResultType.SUCCESS);
        }
    }
}
