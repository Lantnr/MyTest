using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.SocketServer;

namespace TGG.Module.Skill.Service
{
    /// <summary>
    /// 战斗技能学习公共方法
    /// </summary>
    public partial class Common
    {
        /// <summary>技能学习组装数据</summary>
        public Dictionary<String, Object> DataBuild(int result, RoleInfoVo rolevo)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "role", rolevo } };
            return dic;
        }

        /// <summary>战斗技能学习升级重新加入线程（启服）</summary>
        public void FightSkillRecovery()
        {
            var users = tg_user.FindAll().ToList();
            var roles = tg_role.FindAll().ToList();
            var skills = tg_role_fight_skill.FindAll().ToList();

            foreach (var u in users)
            {
                var uroles = roles.Where(m => m.user_id == u.id).ToList();
                if (!uroles.Any()) continue;

                foreach (var r in uroles)
                {
                    var rskills = skills.Where(m => m.rid == r.id).ToList();
                    if (!rskills.Any()) continue;

                    var time = CurrentMs() + 5000;      //+5000毫秒预热

                    //已经完成升级或学习的技能
                    var finish = rskills.Where(m => m.skill_state == (int)SkillLearnType.STUDYING && m.skill_time <= time).ToList();
                    if (finish.Any())
                    {
                        var learn = finish.Where(m => m.skill_level == 0).ToList();
                        var up = finish.Where(m => m.skill_level > 0).ToList();                     //修改升级完成的技能
                        UpdateLearnSkill(learn);
                        UpdateLearnSkill(up);
                    }

                    //未完成升级学习技能继续加入线程
                    var unfinish = rskills.Where(m => m.skill_state == (int)SkillLearnType.STUDYING).ToList();
                    if (!unfinish.Any()) continue;

                    var learnskill = unfinish.Where(m => m.skill_level == 0).ToList();      //学习
                    var upskill = unfinish.Where(m => m.skill_level > 0).ToList();            //升级
                    foreach (var s in learnskill)
                    {
                        var lspan = s.skill_time - CurrentMs();
                        if (lspan < 0) continue;
                        SkillLearnOk(u.id, s.rid, s.skill_id, lspan);
                    }
                    foreach (var index in upskill)
                    {
                        var uspan = index.skill_time - CurrentMs();
                        if (uspan < 0) continue;
                        SkillUpgrade(u.id, index, uspan);
                    }
                }
            }
        }

        /// <summary>当前毫秒</summary>
        private Int64 CurrentMs()
        {
            return (DateTime.Now.Ticks - 621355968000000000) / 10000;
        }

        /// <summary>更新技能信息</summary>
        public void UpdateLearnSkill(IEnumerable<tg_role_fight_skill> lskills)
        {
            var ls = new List<tg_role_fight_skill>();
            foreach (var item in lskills)
            {
                item.skill_time = 0;
                item.skill_level = item.skill_level + 1;
                item.skill_state = (int)SkillLearnType.LEARNED;
                ls.Add(item);
            }
            tg_role_fight_skill.UpdateFightSkills(ls);
        }

        /// <summary>验证体力信息</summary>
        public bool PowerOperate(tg_role role, int power)
        {
            var totalpower = tg_role.GetTotalPower(role);
            return totalpower >= power;
        }

        /// <summary>学习升级到达时间</summary>
        public Int64 Time(Int64 time)
        {
            var nowtime = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            var needtime = nowtime + time;
            return needtime;
        }


        #region 战斗技能学习
        /// <summary>开启学习技能线程</summary>
        public void SkillLearnOk(Int64 user_id, Int64 rid, int skillid, Int64 time)
        {
            try
            {
                var token = new CancellationTokenSource();
# if DEBUG
                time = 60000;
#endif
                Object obj = new FightObject { user_id = user_id, rid = rid, skillid = skillid };
                Task.Factory.StartNew(m =>
                {
                    var t = m as FightObject;
                    if (t == null) return;
                    var key = t.GetKey();
                    Variable.CD.AddOrUpdate(key, false, (k, v) => false);
                    SpinWait.SpinUntil(() =>
                    {
                        var b = Convert.ToBoolean(Variable.CD[key]);
                        return b;
                    }, Convert.ToInt32(time));
                }, obj, token.Token)
                .ContinueWith((m, n) =>
                {
                    var lo = n as FightObject;
                    if (lo == null) { token.Cancel(); return; }
                    PushLearn(lo.user_id, lo.rid, lo.skillid);
                    //移除全局变量
                    var key = lo.GetKey();
                    bool r;
                    Variable.CD.TryRemove(key, out r);
                    token.Cancel();
                }, obj, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        public class FightObject
        {
            public Int64 user_id { get; set; }

            public Int64 rid { get; set; }

            public int skillid { get; set; }

            public String GetKey()
            {
                return string.Format("{0}_{1}_{2}_{3}", (int)CDType.FightSkillLearn, user_id, rid, skillid);   //用于线程标示战斗技能学习
            }
        }

        /// <summary>更新学习的技能信息</summary>
        private void PushLearn(Int64 user_id, Int64 rid, int skillid)
        {
            tg_role_fight_skill.UpdateSkill(rid, skillid, (int)SkillLearnType.LEARNED, 1);
            var rolevo = (new Share.Role()).BuildRole(rid);
            PushLearnSkill(user_id, rolevo);        //战斗技能学习完成推送
        }

        /// <summary>推送战斗技能学习结束协议</summary>
        public void PushLearnSkill(Int64 userid, RoleInfoVo rolevo)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "SKILL_FIGHT_STUDY_PUSH", "战斗技能学习推送");
#endif
                if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
                var session = Variable.OnlinePlayer[userid] as TGGSession;     //向在线玩家推送数据
                if (session == null) return;

                var dic = new Dictionary<string, object> { { "role", rolevo } };
                var aso = new ASObject(dic);
                var pv = session.InitProtocol((int)ModuleNumber.SKILL, (int)SkillCommand.SKILL_FIGHT_STUDY, (int)ResponseType.TYPE_SUCCESS, aso);
                session.SendData(pv);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }
        #endregion

        #region 战斗技能升级
        /// <summary>开启升级线程</summary>
        public void SkillUpgrade(Int64 user_id, tg_role_fight_skill skill, Int64 time)
        {
            try
            {
                var token = new CancellationTokenSource();
# if DEBUG
                time = 60000;
#endif
                Object obj = new FightUpObject { user_id = user_id, skill = skill };
                Task.Factory.StartNew(m =>
                {
                    var t = m as FightUpObject;
                    if (t == null) return;
                    var key = t.GetKey();
                    Variable.CD.AddOrUpdate(key, false, (k, v) => false);
                    SpinWait.SpinUntil(() =>
                    {
                        var b = Convert.ToBoolean(Variable.CD[key]);
                        return b;
                    }, Convert.ToInt32(time));
                }, obj, token.Token)
                .ContinueWith((m, n) =>
                {
                    var lo = n as FightUpObject;
                    if (lo == null) { token.Cancel(); return; }
                    FightUpPush(lo.user_id, lo.skill);
                    //移除全局变量
                    var key = lo.GetKey();
                    bool r;
                    Variable.CD.TryRemove(key, out r);
                    token.Cancel();
                }, obj, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        public class FightUpObject
        {
            public Int64 user_id { get; set; }

            public tg_role_fight_skill skill { get; set; }

            public String GetKey()
            {
                return string.Format("{0}_{1}_{2}", (int)CDType.FightSkillUp, user_id, skill.id);   //用于线程标示  Fup表示战斗技能升级  Fup_user_id_skill.id
            }
        }

        /// <summary>更新战斗技能升级信息</summary>
        public void FightUpPush(Int64 user_id, tg_role_fight_skill skill)
        {
            UpdateSkill(skill);
            var rolevo = (new Share.Role()).BuildRole(skill.rid);
            SKILL_FIGHT_PUSH.GetInstance().CommandStart(user_id, rolevo);  //技能升级推送
        }

        /// <summary>升级后更新技能信息</summary>
        public void UpdateSkill(tg_role_fight_skill skill)
        {
            skill.skill_time = 0;
            skill.skill_level = skill.skill_level + 1;
            skill.skill_state = (int)SkillLearnType.LEARNED;
            skill.Update();
        }

        #endregion
    }
}
