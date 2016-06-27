using System.Threading;
using FluorineFx;
using FluorineFx.Messaging.Rtmp.SO;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.AMF;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Family;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Family.Service
{
    public partial class Common
    {
        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilData(int result, FamilyVo familyvo, Int64 userid, List<tg_family> familylist, List<view_user_role_family_member> chieffamilymember)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"family", familyvo},
                {"familylist", familylist.Count > 0 ? ConvertListASObject(userid, familylist,chieffamilymember) : null}
            };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilData(int result, FamilyVo familyvo)
        {
            var dic = new Dictionary<string, object>();
            dic.Add("result", result);
            dic.Add("family", familyvo);
            return dic;
        }

        /// <summary>返回错误结果</summary>
        public ASObject Error(int result)
        {
            return new ASObject((new Share.Family()).BuilData(result,null));//BuilData(result, null));
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilData(int result)
        {
            var dic = new Dictionary<string, object> { { "result", result } };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilData(int result, double userid)
        {
            var dic = new Dictionary<string, object>();
            dic.Add("result", result);
            dic.Add("userid", userid);
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilDataLog(int result, List<FamilyLogVo> loglist)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"log", loglist.Count > 0 ? loglist : null}
            };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilDataApply(int result, List<FamilyApplyVo> applylist)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "applylist", applylist } };
            return dic;
        }

        /// <summary>家族列表集合</summary>
        public List<ASObject> ConvertListASObject(Int64 userid, List<tg_family> familylist, List<view_user_role_family_member> chiefmember)
        {
            var list_aso = new List<ASObject>();
            var list = tg_family_apply.GetEntityByUserId(userid);
            if (list.Count > 0)
            {
                foreach (var item in familylist)
                {
                    var apply = list.FirstOrDefault(m => m.fid == item.id);
                    var chief = chiefmember.FirstOrDefault(m => m.userid == item.userid);
                    if (apply != null)
                    {
                        var model = EntityToVo.ToFamilyListVo(item, apply.state, chief.player_name);
                        list_aso.Add(AMFConvert.ToASObject(model));
                    }
                    else
                    {
                        var model = EntityToVo.ToFamilyListVo(item, 0, chief.player_name);
                        list_aso.Add(AMFConvert.ToASObject(model));
                    }
                }
            }
            else
            {
                foreach (var item in familylist)
                {
                    var chief = chiefmember.FirstOrDefault(m => m.userid == item.userid);
                    var model = EntityToVo.ToFamilyListVo(item, 0, chief.player_name);
                    list_aso.Add(AMFConvert.ToASObject(model));
                }
            }
            return list_aso;
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

        /// <summary>向用户推送更新</summary>
        public void RewardsToUser(TGGSession session, tg_user user, int type)
        {
            session.Player.User = user;
            (new Share.User()).REWARDS_API(type, session.Player.User);
        }

        /// <summary> 当前时间毫秒</summary>
        public Int64 CurrentTime()
        {
            // ReSharper disable once PossibleLossOfFraction
            return (DateTime.Now.Ticks - 621355968000000000) / 10000;
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

        /// <summary>创建族长 </summary>
        public tg_family_member CreateMember(Int64 userid, Int64 fid,int degree)
        {
            var member = new tg_family_member
            {
                fid =fid,// family.id,
                userid = userid,
                degree =degree,// (int)FamilyPositionType.CHIEF
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
            log.time = CurrentTime();
            log.fid = member.fid;
            return log;
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
                    FAMILY_LOG_PUSH.GetInstance().CommandStart(_obj.session, _obj.log, _obj.player_name);

                }, obj, token.Token);
            }
        }
        public void FamilyTraining(Int64 uid, Int64 fid, string name, string data)
        {
            //dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Chat", "Common");
            new Chat().FamilyPush(uid, fid, name, data);
        }

        public void ChatPush(tg_family_member member, BaseFamilyLog log, tg_user applyuser)
        {
            FamilyTraining(applyuser.id, member.fid, applyuser.player_name, log.prestige);  //推送家族频道
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
    }
}
