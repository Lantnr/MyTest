using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary> 出征确定目标 </summary>
    public class WAR_DETERMINE_TARGET : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_DETERMINE_TARGET()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_DETERMINE_TARGET _objInstance;

        ///// <summary>WAR_DETERMINE_TARGET单体模式</summary>
        //public static WAR_DETERMINE_TARGET GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DETERMINE_TARGET());
        //}

        /// <summary> 出征确定目标 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id"))
                return null;

            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);
            var ids = session.Player.War.WarBattle.OperableRivalCityIds;
            var _ids = session.Player.War.WarBattle.OperableCityIds;
            if (!(ids.Contains(id) || _ids.Contains(id))) return CommonHelper.ErrorResult(ResultType.WAR_WAR_TARGET_ERROR);

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32033");
            if (rule == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            var mps = new List<Core.Common.Util.Map>();
            if (ids.Contains(id)) mps = session.Player.War.WarBattle.rivalMap;
            if (_ids.Contains(id)) mps = session.Player.War.WarBattle.Map;

            var startId = Convert.ToString(session.Player.War.WarBattle.warGoCityId);
            var dij = new Dijkstra(mps, startId, Convert.ToString(id));
            dij.Find();
            var distance = dij.EndNode.MinDistance.ToString();
            var str = rule.value.Replace("distance", distance);
            var express = CommonHelper.EvalExpress(str);
            var time = Convert.ToInt32(express);

            session.Player.War.WarBattle.LockTime = time;
            session.Player.War.WarBattle.LockCityId = id;
            session.Player.War.WarBattle.lastCityId = Convert.ToInt32(dij.EndNode.LastSecond());//是否有其他大名攻击 0:有  -1 没有
            var type = tg_war_battle_queue.GetCityIsAttack(id) ? 0 : -1;
            return BuildData(time, type);
        }

        /// <summary> 组装数据 </summary>
        /// <param name="time">运输总时间</param>
        /// <param name="type">是否有其他大名攻击 0:有  -1 没有</param>
        private ASObject BuildData(int time, int type)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", (int)ResultType.SUCCESS },
            { "time",time}, 
             { "type",type}, 
            };
            return new ASObject(dic);
        }
    }
}
