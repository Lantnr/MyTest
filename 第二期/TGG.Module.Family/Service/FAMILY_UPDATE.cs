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
    /// 修改族徽
    /// </summary>
    public class FAMILY_UPDATE
    {
        private static FAMILY_UPDATE ObjInstance;

        /// <summary>
        /// FAMILY_UPDATE单体模式
        /// </summary>
        public static FAMILY_UPDATE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_UPDATE());
        }

        /// <summary>修改族徽</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "FAMILY_UPDATE", "修改族徽");
#endif
                var cid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "baseId").Value);//族徽id
                var member = session.Player.Family;
                var family = tg_family.GetEntityById(member.fid);
                var basefamilypost = Variable.BASE_FAMILYPOST.FirstOrDefault(m => m.post == member.degree);
                if (basefamilypost == null)
                    return Common.GetInstance().Error((int)ResultType.BASE_TABLE_ERROR);
                if (basefamilypost.setBadge == (int)FamilyRightType.NORIGHT)
                    return Common.GetInstance().Error((int)ResultType.FAMILY_DROIT_LACK); //没权利修改
                family.clanbadge = cid;
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
