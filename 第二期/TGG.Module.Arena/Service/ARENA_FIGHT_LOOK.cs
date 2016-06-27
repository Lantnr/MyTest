using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Fight;
using TGG.SocketServer;

namespace TGG.Module.Arena.Service
{
    /// <summary>
    /// 查看战斗过程
    /// </summary>
    public class ARENA_FIGHT_LOOK
    {
        private static ARENA_FIGHT_LOOK ObjInstance;

        /// <summary>ARENA_FIGHT_LOOK单体模式</summary>
        public static ARENA_FIGHT_LOOK GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new ARENA_FIGHT_LOOK());
        }

        /// <summary> 查看战斗过程 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "ARENA_FIGHT_LOOK", "查看战斗过程");
#endif
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);
            var report = tg_arena_reports.FindByid(id);
            if (report == null) return new ASObject(BuildData((int)ResultType.FRONT_DATA_ERROR, null));
            try
            {
                var fight = TGG.Core.Common.Util.CommonHelper.BToO(report.history);
                var fightvo = fight as FightVo;
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "战斗记录实体大小: {0} byte", report.history.Length);
#endif
                return new ASObject(BuildData((int)ResultType.SUCCESS, fightvo));
            }
            catch { return new ASObject(BuildData((int)ResultType.DATABASE_ERROR, null)); }

        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result, FightVo model)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", result },
            { "fight", model },
            };
            return dic;
        }
    }
}
