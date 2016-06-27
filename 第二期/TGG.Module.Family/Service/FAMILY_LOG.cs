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
    /// 日志
    /// </summary>
    public class FAMILY_LOG
    {
        private static FAMILY_LOG ObjInstance;

        /// <summary>
        /// FAMILY_LOG单体模式
        /// </summary>
        public static FAMILY_LOG GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_LOG());
        }

        /// <summary>日志</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "FAMILY_LOG", "日志");
#endif                             
                var list_flvo = new List<FamilyLogVo>();
                var userlist = new List<tg_user>();
                var fluser = new List<tg_family_log>();
                var member = session.Player.Family;
                if (member.fid == 0) return new ASObject(Common.GetInstance().BuilDataLog((int)ResultType.NO_DATA, list_flvo));
                var loglist = tg_family_log.GetEntityByFid(member.fid);
                if (!loglist.Any())
                    return new ASObject(Common.GetInstance().BuilDataLog((int)ResultType.NO_DATA, list_flvo));
                var list = loglist.OrderByDescending(m => m.time).ToList();  //最近时间的前10条
                var list10 = list.Select(m => m).ToList().Take(10);
                foreach (var item in list10)
                {
                    if (item.userid > 0)
                        fluser.Add(item);
                    else
                        list_flvo.Add(EntityToVo.ToFamilyLogVo(item, ""));
                }
                if (fluser.Count > 0)
                {
                    if (fluser.Count == 1)
                    {
                        var userid = fluser[0].userid;
                        userlist.Add(tg_user.GetUsersById(userid));
                    }
                    else
                    {
                        var userids = string.Join(",", loglist.ToList().Select(m => m.userid).ToArray());
                        userlist = tg_user.GetUsersByIds(userids);
                    }
                }
                foreach (var fl in list10)
                {
                    var userlog = userlist.FirstOrDefault(m => m.id == fl.userid);
                    if (userlog != null)
                    { list_flvo.Add(EntityToVo.ToFamilyLogVo(fl, userlog.player_name)); }
                }
                return new ASObject(Common.GetInstance().BuilDataLog((int)ResultType.SUCCESS, list_flvo));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }
    }
}
