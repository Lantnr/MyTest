using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.Guide.Service
{
    /// <summary>
    /// 进入页面
    /// </summary>
    public class ENTER
    {
        public static ENTER ObjInstance;

        /// <summary>ENTER单体模式</summary>
        public static ENTER GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new ENTER());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "ENTER", "进入页面");
#endif
            var player = session.Player.CloneEntity();
            var number = Variable.BASE_DAMING;
            if (!number.Any()) return Error((int)ResultType.BASE_TABLE_ERROR);

            if (!player.DamingLog.Any() || player.DamingLog.Count != number.Count) return Error((int)ResultType.DAMING_INFO_ERROR);  //验证大名令任务数据

            var listdata = player.DamingLog.Select(item => AMFConvert.ToASObject(EntityToVo.ToDaMingLingVo(item))).ToList();
            return new ASObject(BuildData((int)ResultType.SUCCESS, listdata));
        }

        /// <summary>错误信息</summary>
        private ASObject Error(int error)
        {
            return new ASObject(new Dictionary<string, object> { { "result", error } });
        }

        /// <summary>组装返回前端数据</summary>
        private Dictionary<String, Object> BuildData(int result, List<ASObject> datas)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "data", datas.Any() ? datas : null } };
            return dic;
        }
    }
}
