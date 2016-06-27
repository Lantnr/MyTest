using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.Core.Vo.Skill;
using TGG.SocketServer;

namespace TGG.Share
{
    /// <summary>
    /// 武将共享类
    /// </summary>
    public partial class Role : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>武将加载  组装武将Vo信息</summary>
        /// <param name="roleitem">武将视图信息</param>
        /// <param name="genretypearr">可学流派，非空</param>
        /// <param name="roletrain">修行武将，非空</param>
        public RoleInfoVo BuildRole(RoleItem roleitem, List<int> genretypearr, tg_train_role roletrain)
        {
            var equips = GetEquips(roleitem.Kind);  //装备  称号集合
            var titles = GetTitles(roleitem.Kind);

            var fightskillvos = ConvertFight(roleitem.FightSkill);             //战斗技能Vo集合
            var lifeskillvos = LifeSkill(roleitem);                                       //生活技能Vo集合
            var genres = genretypearr;              //可学流派

            var trainvo = EntityToVo.ToTrainVo(roletrain);
            var rolevo = EntityToVo.ToRoleVo(roleitem.Kind, equips, fightskillvos, lifeskillvos, genres, trainvo, titles);
            return rolevo;
        }

        /// <summary>
        /// 组装武将Vo信息
        /// </summary>
        /// <param name="rid">武将主键id</param>
        public RoleInfoVo BuildRole(Int64 rid)
        {
            var role = view_role.GetFindRoleById(rid);        //查询武将视图信息
            if (role == null) return null;

            var trainrole = tg_train_role.GetEntityByRid(rid);   //查询武将修行信息

            var equips = GetEquips(role.Kind);                             //整理装备集合  称号集合
            var titles = GetTitles(role.Kind);

            var fightskillvos = ConvertFight(role.FightSkill);         //战斗技能Vo  生活技能Vo集合
            var lifeskillvos = LifeSkill(role);

            //开启可学技能流派
            var genres = LearnGenreOrNinja(role.FightSkill, role.Kind.role_genre, role.Kind.role_ninja); ;

            var trainvo = EntityToVo.ToTrainVo(trainrole);
            var rolevo = EntityToVo.ToRoleVo(role.Kind, equips, fightskillvos, lifeskillvos, genres, trainvo, titles);      //返回前端武将Vo信息
            return rolevo;
        }

        /// <summary>组装装备集合</summary>
        public List<double> GetEquips(tg_role role)
        {
            var list = new List<double>();
            if (role.equip_weapon != 0) { list.Add(Convert.ToDouble(role.equip_weapon)); }//武器
            if (role.equip_armor != 0) { list.Add(Convert.ToDouble(role.equip_armor)); }//铠甲
            if (role.equip_mounts != 0) { list.Add(Convert.ToDouble(role.equip_mounts)); } //坐骑
            if (role.equip_tea != 0) { list.Add(Convert.ToDouble(role.equip_tea)); } //茶器
            if (role.equip_book != 0) { list.Add(Convert.ToDouble(role.equip_book)); }//书籍
            if (role.equip_barbarian != 0) { list.Add(Convert.ToDouble(role.equip_barbarian)); }//南蛮物
            if (role.equip_craft != 0) { list.Add(Convert.ToDouble(role.equip_craft)); } //艺术品
            if (role.equip_gem != 0) { list.Add(Convert.ToDouble(role.equip_gem)); }//珠宝
            return list.Any() ? list : null;
        }

        /// <summary> 组装称号集合</summary>
        private List<double> GetTitles(tg_role role)
        {
            var titles = new List<double>();
            if (role.title_sword != 0) titles.Add(Convert.ToDouble(role.title_sword));  //剑称号
            if (role.title_gun != 0) titles.Add(Convert.ToDouble(role.title_gun));  //枪称号
            if (role.title_tea != 0) titles.Add(Convert.ToDouble(role.title_tea));  //茶道称号
            if (role.title_eloquence != 0) titles.Add(Convert.ToDouble(role.title_eloquence));  //讲价称号
            return titles.Any() ? titles : null;
        }

        /// <summary>战斗技能Vo</summary>
        public List<FightSkillVo> ConvertFight(IEnumerable<tg_role_fight_skill> fights)
        {
            var list = new List<FightSkillVo>();
            foreach (var item in fights)
            {
                if (item.id != 0 && item.skill_level >= 0)
                {
                    list.Add(EntityToVo.ToRoleFightSkillVo(item));
                }
            }
            return list.Any() ? list : null;
        }

        #region  生活技能
        /// <summary>生活技能信息</summary>
        private List<LifeSkillVo> LifeSkill(RoleItem roleitem)
        {
            List<LifeSkillVo> listlife;
            var life = roleitem.LifeSkill;
            if (life.id != 0)
            {
                if (life.sub_calculate_level == 0 && life.sub_calculate_state != (int)SkillLearnType.STUDYING)
                    life.sub_calculate_state = (int)SkillLearnType.TOLEARN;
                if (life.sub_etiquette_level == 0 && life.sub_etiquette_state != (int)SkillLearnType.STUDYING)
                    life.sub_etiquette_state = (int)SkillLearnType.TOLEARN;
                if (life.sub_martial_level == 0 && life.sub_martial_state != (int)SkillLearnType.STUDYING)
                    life.sub_martial_state = (int)SkillLearnType.TOLEARN;
                if (life.sub_ninjitsu_level == 0 && life.sub_ninjitsu_state != (int)SkillLearnType.STUDYING)
                    life.sub_ninjitsu_state = (int)SkillLearnType.TOLEARN;
                listlife = CreateListLife(roleitem.Kind.id, life);  //组装生活技能vo集合
            }
            else
            {
                life = CreateRoleLifeSkill(roleitem.Kind.id);      //创建武将生活技能
                listlife = CreateListLife(roleitem.Kind.id, life);   //组装生活技能vo集合
            }
            return listlife;
        }

        /// <summary>组装生活技能vo集合</summary>
        public List<LifeSkillVo> CreateListLife(decimal rid, tg_role_life_skill life)
        {
            var listlife = new List<LifeSkillVo>();
            listlife.Add(EntityToVo.ToLifeSkillVo(1, rid, life.sub_tea, life.sub_tea_level, life.sub_tea_time, life.sub_tea_progress, life.sub_tea_state));//茶道     
            listlife.Add(EntityToVo.ToLifeSkillVo(2, rid, life.sub_medical, life.sub_medical_level, life.sub_medical_time, life.sub_medical_progress, life.sub_medical_state)); //医术
            listlife.Add(EntityToVo.ToLifeSkillVo(3, rid, life.sub_ninjitsu, life.sub_ninjitsu_level, life.sub_ninjitsu_time, life.sub_ninjitsu_progress, life.sub_ninjitsu_state)); //忍术
            listlife.Add(EntityToVo.ToLifeSkillVo(4, rid, life.sub_calculate, life.sub_calculate_level, life.sub_calculate_time, life.sub_calculate_progress, life.sub_calculate_state)); //算数
            listlife.Add(EntityToVo.ToLifeSkillVo(5, rid, life.sub_eloquence, life.sub_eloquence_level, life.sub_eloquence_time, life.sub_eloquence_progress, life.sub_eloquence_state));//辩才
            listlife.Add(EntityToVo.ToLifeSkillVo(6, rid, life.sub_martial, life.sub_martial_level, life.sub_martial_time, life.sub_martial_progress, life.sub_martial_state));//武艺
            listlife.Add(EntityToVo.ToLifeSkillVo(7, rid, life.sub_craft, life.sub_craft_level, life.sub_craft_time, life.sub_craft_progress, life.sub_craft_state));//艺术
            listlife.Add(EntityToVo.ToLifeSkillVo(8, rid, life.sub_etiquette, life.sub_etiquette_level, life.sub_etiquette_time, life.sub_etiquette_progress, life.sub_etiquette_state));//礼法
            listlife.Add(EntityToVo.ToLifeSkillVo(9, rid, life.sub_reclaimed, life.sub_reclaimed_level, life.sub_reclaimed_time, life.sub_reclaimed_progress, life.sub_reclaimed_state));//开垦
            listlife.Add(EntityToVo.ToLifeSkillVo(10, rid, life.sub_build, life.sub_build_level, life.sub_build_time, life.sub_build_progress, life.sub_build_state));//建筑
            listlife.Add(EntityToVo.ToLifeSkillVo(11, rid, life.sub_mine, life.sub_mine_level, life.sub_mine_time, life.sub_mine_progress, life.sub_mine_state));//矿山
            listlife.Add(EntityToVo.ToLifeSkillVo(12, rid, life.sub_tactical, life.sub_tactical_level, life.sub_tactical_time, life.sub_tactical_progress, life.sub_tactical_state));//军学
            listlife.Add(EntityToVo.ToLifeSkillVo(13, rid, life.sub_ashigaru, life.sub_ashigaru_level, life.sub_ashigaru_time, life.sub_ashigaru_progress, life.sub_ashigaru_state));//足轻
            listlife.Add(EntityToVo.ToLifeSkillVo(14, rid, life.sub_equestrian, life.sub_equestrian_level, life.sub_equestrian_time, life.sub_equestrian_progress, life.sub_equestrian_state));//马术
            listlife.Add(EntityToVo.ToLifeSkillVo(15, rid, life.sub_archer, life.sub_archer_level, life.sub_archer_time, life.sub_archer_progress, life.sub_archer_state));//弓术
            listlife.Add(EntityToVo.ToLifeSkillVo(16, rid, life.sub_artillery, life.sub_artillery_level, life.sub_artillery_time, life.sub_artillery_progress, life.sub_artillery_state));//铁炮
            return listlife;
        }

        /// <summary>创建武将生活技能</summary>
        private tg_role_life_skill CreateRoleLifeSkill(Int64 rid)
        {
            return new tg_role_life_skill()
            {
                rid = rid,
                sub_tea = CommonHelper.EnumLifeType(LifeSkillType.TEA),
                sub_calculate = CommonHelper.EnumLifeType(LifeSkillType.CALCULATE),
                sub_build = CommonHelper.EnumLifeType(LifeSkillType.BUILD),
                sub_eloquence = CommonHelper.EnumLifeType(LifeSkillType.ELOQUENCE),
                sub_equestrian = CommonHelper.EnumLifeType(LifeSkillType.EQUESTRIAN),
                sub_reclaimed = CommonHelper.EnumLifeType(LifeSkillType.RECLAIMED),
                sub_ashigaru = CommonHelper.EnumLifeType(LifeSkillType.ASHIGARU),
                sub_artillery = CommonHelper.EnumLifeType(LifeSkillType.ARTILLERY),
                sub_mine = CommonHelper.EnumLifeType(LifeSkillType.MINE),
                sub_craft = CommonHelper.EnumLifeType(LifeSkillType.CRAFT),
                sub_archer = CommonHelper.EnumLifeType(LifeSkillType.ARCHER),
                sub_etiquette = CommonHelper.EnumLifeType(LifeSkillType.ETIQUETTE),
                sub_martial = CommonHelper.EnumLifeType(LifeSkillType.MARTIAL),
                sub_tactical = CommonHelper.EnumLifeType(LifeSkillType.TACTICAL),
                sub_medical = CommonHelper.EnumLifeType(LifeSkillType.MEDICAL),
                sub_ninjitsu = CommonHelper.EnumLifeType(LifeSkillType.NINJITSU),
                sub_calculate_state = (int)SkillLearnType.TOLEARN,
                sub_etiquette_state = (int)SkillLearnType.TOLEARN,
                sub_ninjitsu_state = (int)SkillLearnType.TOLEARN,
                sub_martial_state = (int)SkillLearnType.TOLEARN,
            };
        }
        #endregion

        /// <summary>组装可学流派集合</summary>
        public List<int> LearnGenreOrNinja(List<tg_role_fight_skill> listskills, int genre, int ninja)
        {
            var group = listskills.GroupBy(m => m.skill_genre).Select(m => m.Key).ToList();      //获得已学习的流派
            return LearnGenre(group, genre, ninja);
        }

        /// <summary>返回可学习流派ids集合</summary>
        private List<int> LearnGenre(List<int> lists, int genre, int ninja)
        {
            if (!lists.Contains((int)RoleGenreType.SCHOOL_UNIVERSAL)) lists.Add((int)RoleGenreType.SCHOOL_UNIVERSAL);
            if (!lists.Contains((int)RoleGenreType.NINJA_UNIVERSAL)) lists.Add((int)RoleGenreType.NINJA_UNIVERSAL);
            if (genre != 0 && !lists.Contains(genre)) lists.Add(genre);
            if (ninja != 0 && !lists.Contains(ninja)) lists.Add(ninja);
            if (lists.Contains(0)) lists.Remove(0);
            return lists;
        }

        #region 放逐武将处理称号信息

        /// <summary>武将放逐验证称号信息</summary>
        public void RoleExile(tg_role role)
        {
            var list = new List<Int64>();
            if (role.title_sword != 0) list.Add(role.title_sword);
            if (role.title_gun != 0) list.Add(role.title_gun);
            if (role.title_tea != 0) list.Add(role.title_tea);
            if (role.title_eloquence != 0) list.Add(role.title_eloquence);
            if (!list.Any()) return;
            var titles = tg_role_title.GetTitleByIds(list);
            foreach (var item in titles)
            {
                if (item == null) return;
                if (item.packet_role1 == role.id) item.packet_role1 = 0;
                else if (item.packet_role2 == role.id) item.packet_role2 = 0;
                else if (item.packet_role3 == role.id) item.packet_role3 = 0;
                if (Count(item) == 0) item.title_load_state = (int)LoadStateType.UNLOAD;
                item.Update();
            }
        }

        /// <summary>称号装备武将数量</summary>
        public int Count(tg_role_title title)
        {
            var sum = 0;
            if (title.packet_role1 != 0) sum += 1;
            if (title.packet_role2 != 0) sum += 1;
            if (title.packet_role3 != 0) sum += 1;
            return sum;
        }

        #endregion

        #region 武将放逐处理阵信息

        /// <summary> 删除阵中武将 </summary>
        /// <param name="model">要删除的武将实体</param>
        public void DeleteMatrixRole(tg_role model)
        {
            if (model.role_state != (int)RoleStateType.PERSONAL_WAR) return;

            var userid = model.user_id;
            var matrix = tg_fight_personal.GetFindByUserId(userid);
            if (matrix == null) return;
            var yin = matrix.yid == 0 ? null : tg_fight_yin.FindByid(matrix.yid);
            var m = DeletePersonalRole(matrix, model.id);
            m.Update();

            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            session.Fight.Personal = m;

            var aso = new ASObject(BuildData((int)ResultType.SUCCESS, m, yin));//matrix:[MatrixVo] 阵Vo
            var pv = session.InitProtocol((int)ModuleNumber.FIGHT, (int)FightCommand.FIGHT_PERSONAL_JOIN, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(int result, tg_fight_personal model, tg_fight_yin yin)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", result },
            { "matrix", model==null?null:EntityToVo.ToFightMatrixVo(model,yin) },
            };
            return dic;
        }

        /// <summary> 将阵中武将移出 </summary>
        /// <param name="model">要处理的阵</param>
        /// <param name="roleid">要处理的武将</param>
        /// <returns>处理后的阵</returns>
        private tg_fight_personal DeletePersonalRole(tg_fight_personal model, Int64 roleid)
        {

            if (model.matrix1_rid == roleid)
            {
                model.matrix1_rid = 0;
                return model;
            }
            if (model.matrix2_rid == roleid)
            {
                model.matrix2_rid = 0;
                return model;
            }
            if (model.matrix3_rid == roleid)
            {
                model.matrix3_rid = 0;
                return model;
            }
            if (model.matrix4_rid == roleid)
            {
                model.matrix4_rid = 0;
                return model;
            }
            if (model.matrix5_rid == roleid)
            {
                model.matrix5_rid = 0;
                return model;
            }
            return model;
        }

        #endregion

        /// <summary>
        /// 武将扣除体力并推送
        /// </summary>
        /// <param name="model">武将实体</param>
        /// <param name="power">消耗体力</param>
        /// <param name="userid">用户id</param>
        public void PowerUpdateAndSend(tg_role model, int power, Int64 userid)
        {
            if (model.buff_power > 0)
            {
                model.buff_power -= power;
                if (model.buff_power < 0) //体力buff不够，则用基础体力来扣，因为为power负数，所以用+=
                {
                    model.power += model.buff_power;
                    model.buff_power = 0;
                }
            }
            else
                model.power -= power;
            tg_role.UpdatePower(model);
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            if (model.role_state == (int)RoleStateType.PROTAGONIST) //主角
            {
                session.Player.Role.Kind.power = model.power;
                session.Player.Role.Kind.buff_power = model.buff_power;
            }
            var list = new List<string> //组装发送协议
                {
                    Expressions.GetPropertyName<RoleInfoVo>(q => q.rolePower),
                    Expressions.GetPropertyName<RoleInfoVo>(q => q.power)
                };
            new RoleAttUpdate().RoleUpdatePush(model, userid, list);
        }

        /// <summary>推送武将   加入推送模块 </summary>
        public void SendPv(TGGSession session, ASObject aso)
        {
            var key = string.Format("{0}_{1}", (int)ModuleNumber.ROLE, (int)RoleCommand.ROLE_PUSH);
            session.SPM.AddOrUpdate(key, aso, (m, n) => aso);
        }

        /// <summary>
        /// 武将体力更新
        /// </summary>
        /// <param name="model">武将实体</param>
        /// <param name="power">消耗体力值</param>
        /// <param name="userid">用户id</param> 
        public void PowerUpdate(tg_role model, int power, Int64 userid)
        {
            if (model.buff_power > 0)
            {
                model.buff_power -= power;
                if (model.buff_power < 0) //体力buff不够，则用基础体力来扣，因为为power负数，所以用+=
                {
                    model.power += model.buff_power;
                    model.buff_power = 0;
                }
            }
            else
                model.power -= power;
            tg_role.UpdatePower(model);
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            if (model.role_state == (int)RoleStateType.PROTAGONIST) //主角
            {
                session.Player.Role.Kind.power = model.power;
                session.Player.Role.Kind.buff_power = model.buff_power;
            }
        }

        #region 雇佣武将

        /// <summary> 初始化将雇佣过期的玩家 </summary>
        public void InitHire()
        {
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;

            var list = tg_user_extend.GetAllUserHire();
            foreach (var item in list)
            {
                if (item.hire_time <= (time + 4000))//+4秒预热
                    InitHireAndSendPlayer(item.user_id);
                else
                    ThreadTask(Convert.ToInt32(item.hire_time - time), item.user_id);
            }
        }

        /// <summary> 开启推送雇佣过期线程 </summary>
        public void ThreadTask(int time, Int64 userid)
        {
            try
            {
#if DEBUG
                time = 10000;
#endif
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    SpinWait.SpinUntil(() => false, Convert.ToInt32(m));
                }, time, token.Token)
                .ContinueWith((m, n) =>
                {
                    var uid = Convert.ToInt64(n);
                    InitHireAndSendPlayer(uid);
                    token.Cancel();
                }, userid, token.Token);
            }
            catch (Exception ex) { XTrace.WriteException(ex); }
        }

        /// <summary> 初始雇佣信息 </summary>
        private void InitHireAndSendPlayer(Int64 userid)
        {
            var userextend = tg_user_extend.GetByUserId(userid);
            if (userextend == null) return;
            userextend.hire_id = 0;
            userextend.hire_time = 0;
            userextend.Update();
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            session.Player.UserExtend = userextend;
            RoleHireEndSend(session);
        }

        /// <summary>推送协议</summary>
        private void RoleHireEndSend(TGGSession session)
        {
            var data = new ASObject();
            var pv = session.InitProtocol((int)ModuleNumber.ROLE, (int)RoleCommand.ROLE_HIRE_END, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }

        #endregion

        /// <summary>
        /// 根据武将ID 集合获取武将对应数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<RoleItem> GetRoleByIds(List<Int64> ids)
        {
            var roles = tg_role.GetRoleByIds(ids);
            var fights = tg_role_fight_skill.GetRoleFightSkills(ids);
            var lifes = tg_role_life_skill.GetRoleLifeSkills(ids);

            var roleitems = new List<RoleItem>();
            foreach (var id in ids)
            {
                var role = roles.FirstOrDefault(m => m.id == id);        //基础属性
                var fight = fights.Where(m => m.rid == id).ToList();    //战斗技能
                var life = lifes.FirstOrDefault(m => m.rid == id);          //生活技能

                var roletiem = new RoleItem()
                {
                    Kind = role,
                    FightSkill = fight,
                    LifeSkill = life,
                };
                roleitems.Add(roletiem);
            }
            return roleitems;
        }
    }
}
