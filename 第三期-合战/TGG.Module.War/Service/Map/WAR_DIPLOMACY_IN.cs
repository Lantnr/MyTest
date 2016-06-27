using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary> 建交 </summary>
    public class WAR_DIPLOMACY_IN : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_DIPLOMACY_IN()
        {
            Dispose();
        }
    
        #endregion

    //    private static WAR_DIPLOMACY_IN _objInstance;

    //    /// <summary>WAR_DIPLOMACY_IN单体模式</summary>
    //    public static WAR_DIPLOMACY_IN GetInstance()
    //    {
    //        return _objInstance ?? (_objInstance = new WAR_DIPLOMACY_IN());
    //    }

        /// <summary> 建交 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("type") || !data.ContainsKey("id"))
                return null;

            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value); //id:[double] 据点基表id
            var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value); //type:[int] 1 建交 2 同盟
            var userid = session.Player.User.id;
            var city = (new Share.War()).GetWarCity(id);
            if (city == null) return CommonHelper.ErrorResult(ResultType.WAR_CITY_NOEXIST);
            var p = tg_war_partner.GetEntityByUserId(userid, city.user_id);
            switch (type)
            {
                case 1://type:[int] 1 建交
                    {
                        if (p == null) return BuildData(0);
                        return p.friendly >= 100 ?
                            CommonHelper.ErrorResult(ResultType.WAR_DIPLOMACY_FULL)
                            : BuildData(p.friendly);
                    }
                case 2://type:[int] 2 同盟
                    {
                        if (p == null) return CommonHelper.ErrorResult(ResultType.WAR_DIPLOMACY_NOTFULL);
                        var time = Common.GetInstance().CurrentTime();
                        if (p.time < time) return p.friendly < 100 ?
                                CommonHelper.ErrorResult(ResultType.WAR_DIPLOMACY_NOTFULL)
                                : BuildData(p.friendly);
                        return CommonHelper.ErrorResult(ResultType.WAR_DIPLOMACY_TIME_IN);
                    }
                default: { return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR); }
            }
        }

        /// <summary> 组装数据 </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private ASObject BuildData(int count)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", (int)ResultType.SUCCESS },
            { "friendly",count},
            };
            return new ASObject(dic);
        }
    }
}
