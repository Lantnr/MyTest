using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Family.Service
{
    /// <summary>
    /// 修改公告
    /// </summary>
    public class FAMILY_NOTICE_UPDATE
    {
        private static FAMILY_NOTICE_UPDATE ObjInstance;

        /// <summary>
        /// FAMILY_NOTICE_UPDATE单体模式
        /// </summary>
        public static FAMILY_NOTICE_UPDATE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_NOTICE_UPDATE());
        }

        /// <summary>修改公告</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "FAMILY_NOTICE_UPDATE", "修改公告");
#endif

# if DEBUG
                XTrace.WriteLine("{0}", "获取前端发送过来的公告数据");
#endif
                var notice = Convert.ToString(data.FirstOrDefault(m => m.Key == "notice").Value);                
                var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "18008");
                if (rule == null)
                    return Common.GetInstance().Error((int)ResultType.BASE_TABLE_ERROR);
                if (notice.Length > Convert.ToInt32(rule.value))
                    return Common.GetInstance().Error((int)ResultType.FAMILY_NOTICE_WORDS_ENOUGH);
                var member = session.Player.Family;
                var basepost = Variable.BASE_FAMILYPOST.FirstOrDefault(m => m.post == member.degree);
                if (basepost == null)
                    return Common.GetInstance().Error((int)ResultType.BASE_TABLE_ERROR); 
                if (basepost.notice == (int)FamilyRightType.NORIGHT)
                    return Common.GetInstance().Error((int)ResultType.FAMILY_DROIT_LACK);  //没权利修改公告
                var family = tg_family.GetEntityById(member.fid);
                family.notice = notice;
                if (!tg_family.GetUpdate(family))
                    return Common.GetInstance().Error((int)ResultType.DATABASE_ERROR);
                return Common.GetInstance().SuccessResult(family, member);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }
    }
}
