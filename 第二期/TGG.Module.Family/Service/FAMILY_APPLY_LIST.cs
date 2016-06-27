using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Family;
using TGG.SocketServer;

namespace TGG.Module.Family.Service
{
    /// <summary>
    /// 申请列表
    /// </summary>
    public class FAMILY_APPLY_LIST
    {
        private static FAMILY_APPLY_LIST ObjInstance;

        /// <summary>
        /// FAMILY_APPLY_LIST单体模式
        /// </summary>
        public static FAMILY_APPLY_LIST GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_APPLY_LIST());
        }

        /// <summary>申请列表</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "FAMILY_APPLY_LIST", "申请列表");
#endif
                var family_member = session.Player.Family; 
                var family_apply_list = view_user_role_family_apply.GetEntityByFid(family_member.fid);

                var list_flvo = new List<FamilyApplyVo>();
                if (family_apply_list.Any())
                {
                    foreach (var fl in family_apply_list)
                    {
                        list_flvo.Add(EntityToVo.ToFamilyApplyVo(fl));
                    }
                }
                return new ASObject(Common.GetInstance().BuilDataApply((int)ResultType.SUCCESS, list_flvo));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }
    }
}
