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
    /// 申请加入
    /// </summary>
    public class FAMILY_APPLY
    {
        private static FAMILY_APPLY ObjInstance;

        /// <summary>
        /// FAMILY_APPLY单体模式
        /// </summary>
        public static FAMILY_APPLY GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_APPLY());
        }

        /// <summary>申请加入</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "FAMILY_APPLY", "申请加入");
#endif
                var id = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);
                var family_member = session.Player.Family;
                if (family_member.fid != 0)
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.FAMILY_MEMBER_EXIST));
                var family = tg_family.GetEntityById(id);
                if (family == null)
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.FAMILY_NOEXIST));
                var base_level = Variable.BASE_FAMILYLEVEL.FirstOrDefault(m => m.level == family.family_level);
                if (base_level == null)
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.BASE_TABLE_ERROR));
                if (family.number + 1 > base_level.amount)
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.FAMILY_MEMBER_ENOUGH));
                var model = new tg_family_apply();
                model.fid = id;
                model.userid = session.Player.User.id;
                model.time = Common.GetInstance().CurrentTime();
                model.state = 1;  //申请结果  0未通过  1 通过
                if (!tg_family_apply.GetInsert(model))
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.DATABASE_ERROR));
                return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }
    }
}
