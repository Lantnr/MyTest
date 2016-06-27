using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.War;
using TGG.SocketServer;

namespace TGG.Module.War.Service
{
    /// <summary>
    /// 战报拉取
    /// 开发者：李德雁
    /// </summary>
    public class WAR_REPORT_JOIN : IDisposable
    {
        //private static WAR_REPORT_JOIN _objInstance;

        ///// <summary>WAR_REPORT_JOIN单体模式</summary>
        //public static WAR_REPORT_JOIN GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_REPORT_JOIN());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 战报拉取 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var report = tg_war_fight_report.GetEntityByUserid(session.Player.User.id,15,0);
            var listvo = report.Select(t => new WarRecordInfoVo
            {
                id = t.id,
                warRecordId = t.base_id,
                times = t.report_time.ToString("yyyy-MM-dd HH:mm:ss"),
                replaceList = t.base_id>=100042 ? new List<string>() { t.city_id.ToString() }
                : new List<string>() { t.city_id.ToString(), t.attack_user_name.ToString() },
            }).ToList();

            return BuildData(listvo);
        }

        private ASObject BuildData(List<WarRecordInfoVo> listvo)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", (int) ResultType.SUCCESS},
                {"report", listvo.Select(AMFConvert.ToASObject)}

            };
            return new ASObject(dic);

        }
    }
}
