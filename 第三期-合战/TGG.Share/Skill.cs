using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Skill;

namespace TGG.Share
{
    public class Skill : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>消耗体力日志</summary>
        public void PowerLog(tg_role role, int modulenumber, int command)
        {
            try
            {
                var cost = Variable.BASE_RULE.FirstOrDefault(m => m.id == "1010");
                if (cost == null) return;
                var _role = tg_role.GetRoleById(role.id);
                if (_role == null) return;

                var oldpower = tg_role.GetTotalPower(role);
                string logdata;
                if (role.role_state == (int)RoleStateType.PROTAGONIST)
                {
                    var totalpower = tg_role.GetTotalPower(_role);
                    logdata = string.Format("{0}_{1}_{2}_{3}", "Power", oldpower, Convert.ToInt32(cost.value), totalpower);
                    (new Log()).WriteLog(role.user_id, (int)LogType.Use, modulenumber, command, "技能", "生活技能学习", "主角体力", (int)GoodsType.TYPE_POWER, Convert.ToInt32(cost.value), totalpower, logdata);
                }
                else
                {
                    logdata = string.Format("{0}_{1}_{2}_{3}", "Power", oldpower, Convert.ToInt32(cost.value), _role.power);
                    (new Log()).WriteLog(role.user_id, (int)LogType.Use, modulenumber, command, "技能", "生活技能学习", "武将体力", (int)GoodsType.TYPE_POWER, Convert.ToInt32(cost.value), _role.power, logdata);
                }
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        /// <summary> 获取武将的所有合战技能信息 并转换成前端的Vo </summary>
        /// <param name="rid">要获取的武将Id</param>
        public List<HeZhanSkillVo> GetSkillListByRid(Int64 rid)
        {
            var list = tg_role_war_skill.GetEntityListByRid(rid);
            return ConvertHeZhanSkillVos(list);
        }

        /// <summary> 合战技能集合转换前端Vo </summary>
        /// <param name="list">合战技能集合</param>
        public List<HeZhanSkillVo> ConvertHeZhanSkillVos(List<tg_role_war_skill> list)
        {
            list = list.Where(m => m.id != 0).ToList();
            return list.Select(EntityToVo.ToHeZhanSkillVo).ToList();
        }

        /// <summary>是否已在学习其他技能 </summary>
        public int SkillStudying(tg_role_life_skill life)
        {
            #region
            if (life.sub_archer_state == (int)SkillLearnType.STUDYING) return life.sub_archer;
            if (life.sub_artillery_state == (int)SkillLearnType.STUDYING) return life.sub_artillery;
            if (life.sub_ashigaru_state == (int)SkillLearnType.STUDYING) return life.sub_ashigaru;
            if (life.sub_build_state == (int)SkillLearnType.STUDYING) return life.sub_build;
            if (life.sub_calculate_state == (int)SkillLearnType.STUDYING) return life.sub_calculate;
            if (life.sub_craft_state == (int)SkillLearnType.STUDYING) return life.sub_craft;
            if (life.sub_eloquence_state == (int)SkillLearnType.STUDYING) return life.sub_eloquence;
            if (life.sub_equestrian_state == (int)SkillLearnType.STUDYING) return life.sub_equestrian;
            if (life.sub_etiquette_state == (int)SkillLearnType.STUDYING) return life.sub_etiquette;
            if (life.sub_martial_state == (int)SkillLearnType.STUDYING) return life.sub_martial;
            if (life.sub_medical_state == (int)SkillLearnType.STUDYING) return life.sub_medical;
            if (life.sub_mine_state == (int)SkillLearnType.STUDYING) return life.sub_mine;
            if (life.sub_ninjitsu_state == (int)SkillLearnType.STUDYING) return life.sub_ninjitsu;
            if (life.sub_reclaimed_state == (int)SkillLearnType.STUDYING) return life.sub_reclaimed;
            if (life.sub_tactical_state == (int)SkillLearnType.STUDYING) return life.sub_tactical;
            if (life.sub_tea_state == (int)SkillLearnType.STUDYING) return life.sub_tea;
            #endregion
            return 0;
        }

        #region 合战技能学习

        /// <summary>
        /// 恢复合战技能线程
        /// </summary>
        public void WarSkillRecovery()
        {
            var list = tg_role_war_skill.GetUnfinishedEntityList();
            foreach (var item in list)
            {
                WarSkillOk(item);
            }
        }

        /// <summary> 当前时间毫秒</summary>
        public Int64 CurrentTime()
        {
            // ReSharper disable once PossibleLossOfFraction
            return (DateTime.Now.Ticks - 621355968000000000) / 10000;
        }

        /// <summary>开启学习技能线程</summary>
        public void WarSkillOk(tg_role_war_skill temp)
        {
            try
            {
                var time = temp.war_skill_time - CurrentTime();
                if (time < 0) time = 0;
                var token = new CancellationTokenSource();
//# if DEBUG
//                time = 60000;
//#endif
                Object obj = new WarSkillObject { user_id = temp.user_id, rid = temp.rid, skillid = temp.war_skill_id, level = temp.war_skill_level };
                Task.Factory.StartNew(m =>
                {
                    var t = m as WarSkillObject;
                    if (t == null) return;
                    var key = t.GetKey();
                    Variable.CD.AddOrUpdate(key, false, (k, v) => false);
                    SpinWait.SpinUntil(() =>
                    {
                        if (!Variable.CD.ContainsKey(key))
                        {
                            token.Cancel();
                            return true;
                        }
                        var b = Convert.ToBoolean(Variable.CD[key]);
                        return b;
                    }, Convert.ToInt32(time));
                }, obj, token.Token)
                .ContinueWith((m, n) =>
                {
                    var lo = n as WarSkillObject;
                    if (lo == null) { token.Cancel(); return; }

                    var role = tg_role.FindByid(lo.rid);
                    if (!tg_role_war_skill.UpdateSkill(lo.rid, lo.skillid, lo.level + 1) || role == null) { token.Cancel(); return; }
                    (new RoleAttUpdate()).RoleUpdatePush(role, role.user_id, new List<string> { "hezhanSkillArrVo" });

                    var key = lo.GetKey();  //移除全局变量
                    RemoveCD(key);
                    token.Cancel();
                }, obj, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        /// <summary> 移除CD </summary>
        private void RemoveCD(string key, int count = 0)
        {
            bool r;
            if (count > 5) return;
            var b = Variable.CD.TryRemove(key, out r);
            if (!b) { RemoveCD(key, count + 1); }
        }

        public class WarSkillObject
        {
            public Int64 user_id { get; set; }

            public Int64 rid { get; set; }

            public int skillid { get; set; }

            public int level { get; set; }

            public String GetKey()
            {
                return string.Format("{0}_{1}_{2}_{3}", (int)CDType.WarSkill, user_id, rid, skillid);   //用于线程标示合战技能学习
            }
        }
        #endregion

    }
}
