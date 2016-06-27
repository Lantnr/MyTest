using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Family;
using TGG.SocketServer;

namespace TGG.Share
{
    public class Family : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilData(int result, FamilyVo familyvo)
        {
            var dic = new Dictionary<string, object>();
            dic.Add("result", result);
            dic.Add("family", familyvo);
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilData(int result)
        {
            var dic = new Dictionary<string, object> { { "result", result } };
            return dic;
        }

        /// <summary>创建成员 </summary>
        public tg_family_member CreateMember(Int64 userid, Int64 fid, int degree)
        {
            var member = new tg_family_member
            {
                fid = fid,// family.id,
                userid = userid,
                degree = degree,// (int)FamilyPositionType.CHIEF
            };  //创建家族成员
            return member;
        }

        /// <summary>创建日志</summary>
        public tg_family_log CreateFamilyLog(BaseFamilyLog familylog, tg_family_member member)
        {
            var log = new tg_family_log();
            log.baseid = familylog.id;
            log.type = familylog.type;
            log.userid = member.userid;
            log.time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            log.fid = member.fid;
            return log;
        }

        /// <summary>返回玩家家族信息</summary>
        public ASObject SuccessResult(tg_family family, tg_family_member member)
        {
            var rank = GetRanking(family);
            var userextend = tg_user_extend.GetByUserId(member.userid);
            var familyvo = FamilyInfo(family, rank, userextend);
            return new ASObject(BuilData((int)ResultType.SUCCESS, familyvo));
        }

        /// <summary>家族排名</summary>
        public int GetRanking(tg_family familymyself)
        {
            var list = tg_family.GetAllById();
            var list_rank = list.OrderByDescending(m => m.family_level).ThenByDescending(m => m.time).ToList();  //家族排序     
            var family_rank = list_rank.FirstOrDefault(m => m.id == familymyself.id);
            var rank = list_rank.IndexOf(family_rank);   //排名
            rank = rank + 1;
            return rank;
        }

        /// <summary>
        /// 组装家族vo
        /// </summary>
        /// <param name="familymyself">家族信息</param>
        /// <param name="ranking">排名</param>
        /// <returns></returns>
        public FamilyVo FamilyInfo(tg_family familymyself, int ranking, tg_user_extend userextend)
        {
            var list_member = new List<FamilyMemberVo>();
            var list = view_user_role_family_member.GetAllById(familymyself.id);   //家族成员信息          
            var loglist = new List<tg_user_login_log>();

            if (list.Count > 0)
            {
                if (list.Count == 1)
                {
                    var userid = list[0].userid;
                    loglist.Add(tg_user_login_log.GetLoginLogById(userid));
                }
                else
                {
                    var userids = string.Join(",", list.ToList().Select(m => m.userid).ToArray());
                    loglist = tg_user_login_log.GetLoginLogByIds(userids);
                }
            }
            foreach (var fm in list) //家族成员登陆时间
            {
                foreach (var log in loglist)
                {
                    if (fm.userid != log.user_id) continue;
                    if (Variable.OnlinePlayer.ContainsKey(fm.userid))
                    {
                        decimal debarktime = (log.logout_time - 621355968000000000) / 10000;
                        list_member.Add(EntityToVo.ToFamilyMemberVo(1, fm, debarktime));
                    }
                    else
                    {
                        decimal debarktime = (log.logout_time - 621355968000000000) / 10000;
                        list_member.Add(EntityToVo.ToFamilyMemberVo(0, fm, debarktime));
                    }
                }
            }
            var familyvo = EntityToVo.ToFamilyVo(familymyself, ranking, list_member, userextend);
            return familyvo;
        }

        /// <summary>向用户推送更新</summary>
        public void RewardsToUser(TGGSession session, tg_user user, int type)
        {
            session.Player.User = user;
            (new Share.User()).REWARDS_API(type, session.Player.User);
        }

        /// <summary>向所有在线家族成员推送</summary>
        public void FamilyMemberAllPush(tg_family_member member, tg_family_log log)
        {
            var _member = view_user_role_family_member.GetEntityByUserId(member.userid);
            var list = Variable.OnlinePlayer.Select(m => m.Value as TGGSession)
              .Where(m => m.Player.Family.fid == member.fid).ToList();//同家族

            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                var obj = new FamilyLogPushObject
                {
                    session = item,
                    log = log,
                    player_name = _member.player_name,
                };
                Task.Factory.StartNew(m =>
                {
                    var _obj = m as FamilyLogPushObject;
                    if (_obj == null) return;
                    Push(_obj.session, _obj.log, _obj.player_name);
                    token.Cancel();
                }, obj, token.Token);
            }
        }

        /// <summary>
        /// 线程对象
        /// </summary>
        class FamilyLogPushObject
        {
            public TGGSession session { get; set; }

            public tg_family_log log { get; set; }

            public String player_name { get; set; }
        }

        /// <summary> 日志推送</summary>
        public void Push(TGGSession session, tg_family_log log, string name)
        {
            if (session == null) return;
            var dic = new Dictionary<string, object> { { "log", EntityToVo.ToFamilyLogVo(log, name) } };
            LogPush(session, new ASObject(dic));
        }

        /// <summary>日志推送</summary>
        private static void LogPush(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "FAMILY_LOG_PUSH", "日志推送");
#endif
            var pv = session.InitProtocol((int)ModuleNumber.FAMILY, (int)Core.Enum.Command.FamilyCommand.FAMILY_LOG_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }
    }
}
