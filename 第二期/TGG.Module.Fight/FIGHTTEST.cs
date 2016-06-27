using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;
using TGG.SocketServer;

namespace TGG.Module.Fight
{
    public class FIGHTTEST
    {
        private static FIGHTTEST _objInstance;

        /// <summary>FIGHTTEST单体模式</summary>
        public static FIGHTTEST GetInstance()
        {
            return _objInstance ?? (_objInstance = new FIGHTTEST());
        }

        /// <summary> 战斗方法 </summary>
        /// <param name="session"></param>
        /// <param name="hp">要修改己方的血量</param>
        /// <param name="rivalhp">要修改敌方的血量</param>
        /// <returns></returns>
        public ResultType FightMethod(TGGSession session, Int64 hp = 0, Int64 rivalhp = 0)
        {
            RivalFightType = (int)FightType.NPC_MONSTER;
            if (session.Fight.Personal.id == 0) session.Fight.Personal =
 tg_fight_personal.PersonalInsert(session.Player.User.id, session.Player.Role.Kind.id); //验证己方阵形 没有就插入阵
            var result = ReadyData(session.Fight.Personal, session.Fight.Rival, session.Player.User.player_vocation,
                 session.Player.Role.Kind.id, hp, rivalhp);
            if (result != ResultType.SUCCESS) return result;
            FightStart(session.Player.User.id);
            return ResultType.SUCCESS;
        }

        /// <summary> 准备数据 </summary>
        /// <param name="rivalid">对手用户Id</param>
        /// <param name="personal">当前玩家阵</param>
        /// <param name="vocation">当前玩家职业</param>
        /// <param name="roleid">当前玩家主角武将Id</param>
        /// <param name="hp">当前玩家主角武将要修改的血量</param>
        /// <param name="rivalhp">对手要修改的血量</param>
        /// <returns></returns>
        public ResultType ReadyData(tg_fight_personal personal, Int64 rivalid, int vocation, Int64 roleid, Int64 hp, Int64 rivalhp)
        {
            Init();
            var rivalmatrix = GetRivalMatrix(rivalid);
            if (rivalmatrix == null) return ResultType.FIGHT_RIVAL_PERSONAL_ERROR;
            var matrix = ConvertPlayMatrix(UpdatePersonal(personal, roleid));
            VocationAdd(personal.user_id, rivalid, vocation);
            matrix = GetHireRole(matrix);      //对matrix 进行雇佣武将操作
            UpdateHp(matrix, rivalmatrix, roleid, hp, rivalhp);
            FirstAfter(matrix, rivalmatrix);
            InitYinCount(matrix.user_id, rivalmatrix.user_id);
            return ResultType.SUCCESS;
        }

        public void FightStart(Int64 userid)
        {

            var vo = new FightVo { attackId = FirstUserId, wuJiangName = RivalName, yinA = FirstYinVo, yinB = AfterYinVo };
        start: //战斗过程组装
            if (!BuildMove(vo, true))
                goto end;
            if (BuildMove(vo, false))
                goto start;
        end:
            vo.isWin = true;
            var session = Variable.OnlinePlayer[userid];
            SendProtocol(session, new ASObject(BuildData(Convert.ToInt32(ResultType.SUCCESS), vo)));
        }

        /// <summary> 推送协议 </summary>
        private void SendProtocol(TGGSession session, ASObject aso)
        {
            var pv = session.InitProtocol((int)ModuleNumber.FIGHT, (int)FightCommand.FIGHT_PERSONAL_ENTER, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result, FightVo model)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", result },
            { "fight", model },
            };
            return dic;
        }

        private bool BuildMove(FightVo model, bool flag)
        {
            IsFirst = flag;
            var moves = new List<MovesVo>();
            var role = GetShotRole();
            if (role == null) { model.isWin = IsFirst; return false; }
            if (!AddRoleRound(role.id)) return true;
            XTrace.WriteLine(string.Format("{0} {1}", "出手id:", role.id));
            AddYinCount(role.user_id);

            if (!RoleShot(role.id, ref moves)) return false;
            model.moves.Add(moves);
            InitPosition();
            return true;
        }

        /// <summary> 武将出手 </summary>
        private bool RoleShot(Int64 roleid, ref List<MovesVo> moves)
        {
            if (RoleStateExistByType(roleid, (int)FightingSkillType.DIZZINESS)) return true;
            if (RoleStateExistByType(roleid, (int)FightingSkillType.SEAL))
                goto attack;

        attack:
            var move = Attack(roleid, false, true);
            if (move == null) return false;
            moves.Add(move);
            return true;

        }

        #region 攻击

        /// <summary> 攻击 </summary>
        /// <param name="roleid">攻击的武将Id</param>
        /// <param name="flag">是否全体攻击</param>
        /// <param name="sign">是否增加气力</param>
        private MovesVo Attack(Int64 roleid, bool flag, bool sign)
        {
            if (!AllRoles.ContainsKey(roleid)) return null;
            var roles = new List<Role>();
            var role = AllRoles[roleid];
            if (sign) role.angerCount += 1;
            if (flag)
                roles = GetRoles(!IsFirst, false);
            else
            {
                var r = GetRole();
                if (r == null) return null;
                roles.Add(r);
            }
            Attack(role, roles, flag);
            var move = BuildMovesVo(role.id, roles.Select(m => Convert.ToDouble(m.id)).ToList());
            RemoveRoleState();
            return move;
        }

        private void RemoveRoleState()
        {
            foreach (var item in AllRoles)
            {
                item.Value.buffVos2.RemoveAll(m => m.round == 0);
                item.Value.damage = 0;
            }
        }

        /// <summary> 组装出招Vo </summary>
        /// <param name="roleid"></param>
        /// <param name="hitids"></param>
        private MovesVo BuildMovesVo(Int64 roleid, List<double> hitids)
        {
            var move = new MovesVo
            {
                attackId = roleid,
                hitIds = hitids,
                rolesA = ConvertRoleFightVo(GetRoles(true, true)),
                rolesB = ConvertRoleFightVo(GetRoles(false, true)),
                times = Round
            };
            if (!FirstRoles.Any())
                move.yinA = 0;
            else
            {
                var dic = FirstRoles.First();
                move.yinA = AllRoles.ContainsKey(dic.Value) ? YinCount[AllRoles[dic.Value].user_id] : 0;
            }
            if (!AfterRoles.Any())
                move.yinB = 0;
            else
            {
                var dic = AfterRoles.First();
                move.yinB = AllRoles.ContainsKey(dic.Value) ? YinCount[AllRoles[dic.Value].user_id] : 0;
            }
            return move;
        }

        private void Attack(Role role, IEnumerable<Role> roles, bool flag)
        {
            foreach (var item in roles)
            {
                if (IsTrue(role.IgnoreDuck))
                    Attack(role, item, flag);
                else
                {
                    if (IsTrue(item.dodgeProbability))
                        Dodge(item);
                    else
                        Attack(role, item, flag);
                }
            }
        }

        private void Dodge(Role role)
        {
            var critbuff = new Buff() { type = (int)FightingSkillType.DODGE, value = 0 };
            role.buffVos2.Add(critbuff);
        }

        private void Attack(Role role, Role drole, bool flag)
        {
            Int64 damage;
            if (IsTrue(role.critProbability)) //是否暴击
            {
                damage = flag ? Convert.ToInt64(DamageCount(role, drole, true) * 0.3) : DamageCount(role, drole, true);
                var critbuff = new Buff() { type = (int)FightingSkillType.CRIT, value = -damage };
                drole.buffVos2.Add(critbuff);
            }
            else
            {
                damage = flag ? Convert.ToInt64(DamageCount(role, drole, true) * 0.3) : DamageCount(role, drole, false);
                drole.damage = damage;
            }
            drole.hp -= damage;
            //if (drole.hp > 0) return;
            //if (FirstRoles.ContainsValue(drole.id))
            //{
            //    var dic = FirstRoles.FirstOrDefault(m => m.Value == drole.id);
            //    FirstRoles.Remove(dic.Key);
            //}
            //if (AfterRoles.ContainsValue(drole.id))
            //{
            //    var dic = AfterRoles.FirstOrDefault(m => m.Value == drole.id);
            //    AfterRoles.Remove(dic.Key);
            //}
        }

        /// <summary>伤害计算</summary>
        /// <param name="attackrole">攻击武将</param>
        /// <param name="defenserole">防守武将</param>
        /// <param name="flag">是否暴击</param>
        public Int64 DamageCount(Role attackrole, Role defenserole, bool flag)
        {

            //A属性 = A基础属性+A装备属性+A生活技能属性+A称号属性
            //A总攻击 = A武力*1.5+A装备攻击力+A效果之增加攻击力-A效果之减少攻击力
            //B总防御 = B装备防御力+B效果之增加防御力-B效果之减少防御力
            //A会心几率 = A魅力/25 *0.01 +A效果之会心几率
            //A会心效果 = 150% + （A政务/15）*0.01 +A效果之会心效果
            //B闪避几率 = （B智谋/28）*0.01 +B效果之会心效果
            //A伤害加成 = A效果之增加伤害+A装备增加伤害
            //B减伤 = B装备减伤 + B效果之减伤
            //B生命 = B基础生命+B装备生命+B技能生命

            //攻击伤害 = （A总攻击-B总防御）*（1-B减伤）*（100% + A会心成功值*(A会心效果-100%）)*（100% + A伤害加成）

            var A0 = attackrole.attack;              //A总攻击
            var A1 = defenserole.defense;            //B总防御
            var A3 = flag ? 1.5 + (attackrole.critAddition / 100) : 1.0;  //A会心效果
            var A5 = A0 - A1;                        //A总攻击-B总防御
            var A6 = A5 > 0 ? A5 : 1;                //A总攻击-B总防御
            var A7 = 1 + (attackrole.hurtIncrease / 100);
            var A10 = (A6) * ((100 - defenserole.hurtReduce) / 100) * A3 * A7;

            double number = 1;
            var n = Round - 21;
            if (n >= 0)
            {
                var b = n / 10;
                if (b == 0)
                    number = number + 1;
                if (b > 0)
                    number = number + (1 + n * 0.5);
            }
            var count = A10 * number;
            if (Vocations.ContainsKey(attackrole.user_id)) count = count * Vocations[attackrole.user_id];
            return count < 0 ? 1 : Convert.ToInt64(count);
        }

        /// <summary> 几率是否成功 </summary>
        /// <param name="number">几率</param>
        public bool IsTrue(double number)
        {
            return (number > 0) && new RandomSingle().IsTrue(number);
        }

        /// <summary> 获取某一方全体武将 </summary>
        /// <param name="flag">要获取的方位 </param>
        /// <param name="sign">空缺位置是否为null</param>
        private List<Role> GetRoles(bool flag, bool sign)
        {
            var roles = new List<Role>();
            var rs = flag ? FirstRoles : AfterRoles;
            if (!sign)
                roles.AddRange(from item in rs.Values where AllRoles.ContainsKey(item) select AllRoles[item]);
            else
            {
                for (int i = 1; i <= 5; i++)
                {
                    if (rs.ContainsKey(i))
                    {
                        var roleid = rs[i];
                        if (AllRoles.ContainsKey(roleid))
                        {
                            roles.Add(AllRoles[roleid]);
                        }
                    }
                    else
                        roles.Add(null);
                }
            }
            return roles;
        }

        #endregion

        #region 验证武将状态

        /// <summary> 验证武将状态是否存在 </summary>
        /// <param name="roleid">要验证的武将</param>
        /// <param name="type">要验证的状态</param>
        private bool RoleStateExistByType(Int64 roleid, int type)
        {
            return GetRoleState(roleid).Count(m => m.type == type) > 0;
        }

        /// <summary> 获取武将所有状态 </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        private IEnumerable<Buff> GetRoleState(Int64 roleid)
        {
            return RoleState.ContainsKey(roleid) ? RoleState[roleid] : new List<Buff>();
        }

        #endregion

        /// <summary> 增加武将所在回合数 </summary>
        private bool AddRoleRound(Int64 roleid)
        {
            if (!RoleRound.ContainsKey(roleid)) InitRound(roleid);
            else
            {
                var value = RoleRound[roleid] + 1;
                if (value <= Round) RoleRound[roleid] = value;
                else
                {
                    if (!IsFirst) return false;
                    if (RoleRound.ContainsValue(Round - 1)) return false;
                    RoleRound[roleid] = value;
                    Round = value;
                }
            }
            return true;
        }

        /// <summary> 增加印计数 </summary>
        private void AddYinCount(Int64 userid)
        {
            if (YinCount.ContainsKey(userid))
                YinCount[userid] += 1;
        }

        /// <summary> 获取当前出手武将 </summary>
        private Role GetShotRole()
        {
            var dic = IsFirst ? FirstRoles : AfterRoles;
            var number = IsFirst ? FirstNumber : AfterNumber;
            for (int i = 1; i <= 5; i++)
            {
                if (!dic.ContainsKey(number))
                    number = number < 5 ? number + 1 : 1;
                else
                {
                    var r = AllRoles[dic[number]];
                    if (r.hp <= 0)
                        number = number < 5 ? number + 1 : 1;
                    else
                    {
                        if (IsFirst) FirstNumber = number;
                        else AfterNumber = number;
                        return r;
                    }
                }
            }
            return null;
        }

        /// <summary> 获取单体被攻击武将 </summary>
        private Role GetRole()
        {
            var dic = IsFirst ? AfterRoles : FirstRoles;
            if (!dic.Any()) return null;
            for (int i = 1; i <= 5; i++)
            {
                if (!dic.ContainsKey(i)) continue;
                var r = AllRoles[dic[i]];
                if (r.hp <= 0) continue;
                return r;
            }
            return null;
            //return IsFirst ? AfterRoles.Where(item => AllRoles.ContainsKey(item.Value))
            //    .Select(item => AllRoles[item.Value]).FirstOrDefault()
            //    : FirstRoles.Where(item => AllRoles.ContainsKey(item.Value))
            //    .Select(item => AllRoles[item.Value]).FirstOrDefault();
        }

        /// <summary> 获取出手武将Id </summary>
        private Int64 GetShotRoleId()
        {
            var dic = IsFirst ? FirstRoles : AfterRoles;
            var number = IsFirst ? FirstNumber : AfterNumber;
            for (int i = 0; i < 5; i++)
            {
                if (!dic.ContainsKey(number))
                    number = number < 5 ? number + 1 : 1;
                else
                    return dic[number];
            }
            return 0;
        }

        #region 获取雇佣武将

        /// <summary> 获取雇佣武将 </summary>
        private PlayMatrix GetHireRole(PlayMatrix model)
        {
            bool flag = false;
            if (model.r1 == null) { model.r1 = GetHireRole(model.user_id); flag = true; }
            if (model.r2 == null && !flag) { model.r2 = GetHireRole(model.user_id); flag = true; }
            if (model.r3 == null && !flag) { model.r3 = GetHireRole(model.user_id); flag = true; }
            if (model.r4 == null && !flag) { model.r4 = GetHireRole(model.user_id); flag = true; }
            if (model.r5 == null && !flag) model.r5 = GetHireRole(model.user_id);
            return model;
        }

        /// <summary> 获取雇佣武将 </summary>
        private Role GetHireRole(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return null;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return null;
            var hireid = session.Player.UserExtend.hire_id;
            if (hireid == 0) return null;
            var hire = Variable.BASE_NPCHIRE.FirstOrDefault(m => m.id == hireid);
            if (hire == null) return null;
            var npc = Variable.BASE_NPCROLE.FirstOrDefault(m => m.id == hire.npcRoleId);
            if (npc == null) return null;
            var role = ConvertRole(npc, userid);
            role.user_id = userid;
            role.monsterType = (int)FightRivalType.MONSTER;
            return role;
        }

        #endregion

        /// <summary> 获取总攻击力给全局AllRoles赋值 </summary>
        private double GetAattackAndGiveAllRoles(PlayMatrix model)
        {
            double attack = 0;
            var dic = new Dictionary<Int64, Role>();
            if (model.r1 != null)
            {
                dic.Add(model.r1.id, model.r1);
                attack += model.r1.attack <= 0 ? 0 : model.r1.attack;
            }
            if (model.r2 != null)
            {
                dic.Add(model.r2.id, model.r2);
                attack += model.r2.attack <= 0 ? 0 : model.r2.attack;
            }
            if (model.r3 != null)
            {
                dic.Add(model.r3.id, model.r3);
                attack += model.r3.attack <= 0 ? 0 : model.r3.attack;
            }
            if (model.r4 != null)
            {
                dic.Add(model.r4.id, model.r4);
                attack += model.r4.attack <= 0 ? 0 : model.r4.attack;
            }
            if (model.r5 != null)
            {
                dic.Add(model.r5.id, model.r5);
                attack += model.r5.attack <= 0 ? 0 : model.r5.attack;
            }

            foreach (var item in dic)
            {
                AllRoles.Add(item.Key, item.Value);
                AllRolesHp.Add(item.Key, item.Value);
            }

            return attack;
        }

        /// <summary> 判定先后手 </summary>
        /// <param name="oneself">己方</param>
        /// <param name="rival">敌方</param>
        private void FirstAfter(PlayMatrix oneself, PlayMatrix rival)
        {
            switch (RivalFightType)
            {
                case (int)FightType.SIEGE:
                case (int)FightType.BUILDING:
                    {
                        GiveFirstAfter(oneself, true);  //活动始终玩家先手
                        GiveFirstAfter(rival, false);
                        FirstUserId = oneself.user_id;
                        break;
                    }
                default:
                    {
                        var attack = GetAattackAndGiveAllRoles(oneself);
                        var rivalAttack = GetAattackAndGiveAllRoles(rival);
                        if (attack >= rivalAttack)
                        {
                            GiveFirstAfter(oneself, true); //攻击值验证  判定先后手
                            GiveFirstAfter(rival, false);
                            FirstUserId = oneself.user_id;
                        }
                        else
                        {
                            GiveFirstAfter(oneself, false); //攻击值验证  判定先后手
                            GiveFirstAfter(rival, true);
                            FirstUserId = rival.user_id;
                        }
                        break;
                    }
            }
            FirstYinVo = oneself.yin == null ? null : EntityToVo.ToFightYinVo(oneself.yin);
            AfterYinVo = rival.yin == null ? null : EntityToVo.ToFightYinVo(rival.yin);
        }

        #region 修改血量

        /// <summary> 修改血量 </summary>
        /// <param name="oneself"></param>
        /// <param name="rival"></param>
        /// <param name="roleid"></param>
        /// <param name="hp"></param>
        /// <param name="rovalhp"></param>
        private void UpdateHp(PlayMatrix oneself, PlayMatrix rival, Int64 roleid, Int64 hp, Int64 rovalhp)
        {
            switch (RivalFightType)
            {
                case (int)FightType.SIEGE:
                case (int)FightType.BUILDING: { if (rival.r1 != null) rival.r1.hp = rovalhp; return; }
                case (int)FightType.DUPLICATE_SHARP: { UpdateLeadRoleHp(oneself, roleid, hp); break; }
                default: { return; }
            }

        }

        /// <summary> 修改主角血量 </summary>
        /// <param name="model"></param>
        /// <param name="roleid"></param>
        /// <param name="hp"></param>
        private void UpdateLeadRoleHp(PlayMatrix model, Int64 roleid, Int64 hp)
        {
            if (model.r1 != null) if (model.r1.id == roleid) { model.r1.hp = hp; return; }
            if (model.r2 != null) if (model.r2.id == roleid) { model.r2.hp = hp; return; }
            if (model.r3 != null) if (model.r3.id == roleid) { model.r3.hp = hp; return; }
            if (model.r4 != null) if (model.r4.id == roleid) { model.r4.hp = hp; return; }
            if (model.r5 != null) if (model.r5.id == roleid) model.r5.hp = hp;
        }

        #endregion

        /// <summary> 给全局前后手赋值 </summary>
        /// <param name="model"></param>
        /// <param name="flag">是否先手</param>
        private void GiveFirstAfter(PlayMatrix model, bool flag)
        {
            if (model.r1 != null) if (flag) FirstRoles.Add(1, model.r1.id); else AfterRoles.Add(1, model.r1.id);
            if (model.r2 != null) if (flag) FirstRoles.Add(2, model.r2.id); else AfterRoles.Add(2, model.r2.id);
            if (model.r3 != null) if (flag) FirstRoles.Add(3, model.r3.id); else AfterRoles.Add(3, model.r3.id);
            if (model.r4 != null) if (flag) FirstRoles.Add(4, model.r4.id); else AfterRoles.Add(4, model.r4.id);
            if (model.r5 != null) if (flag) FirstRoles.Add(5, model.r5.id); else AfterRoles.Add(5, model.r5.id);
        }

        /// <summary> 验证是否修改为只有主角武将阵形 </summary>
        /// <param name="oneself">当前玩家tg_fight_personal</param>
        /// <param name="roleid">主角武将Id</param>
        private tg_fight_personal UpdatePersonal(tg_fight_personal oneself, Int64 roleid)
        {
            switch (RivalFightType)
            {
                case (int)FightType.SINGLE_FIGHT:
                    {
                        oneself.matrix1_rid = roleid;
                        oneself.matrix2_rid = 0;
                        oneself.matrix3_rid = 0;
                        oneself.matrix4_rid = 0;
                        oneself.matrix5_rid = 0;
                        return oneself;
                    }
            }
            return oneself;
        }

        #region 职业系数加成

        /// <summary> 职业系数 </summary>
        /// <param name="userid"></param>
        /// <param name="rivalid"></param>
        /// <param name="vocation"></param>
        private void VocationAdd(Int64 userid, Int64 rivalid, int vocation)
        {
            var baseVocation = Variable.BASE_VOCATION.FirstOrDefault(m => m.vocation == vocation);
            if (baseVocation == null) return;
            Vocations.Add(userid, baseVocation.fight);
            RivalVocationAdd(rivalid);
        }

        /// <summary> 对手职业系数 </summary>
        private void RivalVocationAdd(Int64 rivalid)
        {
            int v;
            switch (RivalFightType)
            {
                case (int)FightType.BOTH_SIDES:
                case (int)FightType.ONE_SIDE:
                    {
                        if (Variable.OnlinePlayer.ContainsKey(rivalid))
                        {
                            var rivalsession = Variable.OnlinePlayer[rivalid] as TGGSession;
                            if (rivalsession == null) return;
                            v = rivalsession.Player.User.player_vocation;
                            RivalName = rivalsession.Player.User.player_name;
                        }
                        else
                        {
                            var user = tg_user.FindByid(rivalid);
                            if (user == null) return;
                            v = user.player_vocation;
                            RivalName = user.player_name;
                        }
                        break;
                    }
                default: { return; }
            }
            var bv = Variable.BASE_VOCATION.FirstOrDefault(m => m.vocation == v);
            if (bv == null) return;
            Vocations.Add(rivalid, bv.fight);
        }

        #endregion

        #region 初始化数据

        /// <summary>初始化数据</summary>
        private void Init()
        {
            Round = 1;
            RivalName = "";
            FirstNumber = 1;
            AfterNumber = 1;
            IsFirst = true;
            AllRoles = new Dictionary<long, Role>();
            AllRolesHp = new Dictionary<long, Role>();
            FirstRoles = new Dictionary<int, long>();
            AfterRoles = new Dictionary<int, long>();
            Vocations = new Dictionary<decimal, double>();
            RoleState = new Dictionary<long, List<Buff>>();
            RoleRound = new Dictionary<long, int>();
            YinCount = new Dictionary<long, int>();
        }

        /// <summary> 初始化回合数 </summary>
        private void InitRound(Int64 roleid)
        {
            RoleRound.Add(roleid, 1);
            foreach (var item in AllRoles.Keys.Where(item => item != roleid))
            {
                RoleRound.Add(item, 0);
            }
        }

        /// <summary> 初始化印计数 </summary>
        private void InitYinCount(Int64 userid, Int64 rivalid)
        {
            YinCount = new Dictionary<long, int> { { userid, 0 }, { rivalid, 0 } };
        }

        /// <summary> 初始化位置 </summary>
        private void InitPosition()
        {
            var dic = IsFirst ? FirstRoles : AfterRoles;
            var d = new Dictionary<int, Int64>();
            foreach (var item in dic)
            {
                var key = item.Key - 1;
                d.Add(key <= 0 ? 5 : key, dic[item.Key]);
            }
            if (IsFirst)
            {
                FirstRoles = d;
                // FirstNumber = FirstNumber < 5 ? FirstNumber + 1 : 1;
            }
            else
            {
                AfterRoles = d;
                //AfterNumber = AfterNumber < 5 ? AfterNumber + 1 : 1;
            }

            foreach (var item in d)
            {
                XTrace.WriteLine(string.Format("{0} {1}", "位置:" + item.Key, "武将Id:" + item.Value));
            }
        }

        #endregion

        #region 获取数据方法

        /// <summary>获取对手战斗阵信息</summary>
        private PlayMatrix GetRivalMatrix(Int64 id)
        {
            dynamic npc;
            switch (RivalFightType)
            {
                case (int)FightType.SIEGE:
                case (int)FightType.BUILDING:
                case (int)FightType.NPC_FIGHT_ARMY: { npc = Variable.BASE_NPCARMY.FirstOrDefault(m => m.id == (int)id); break; }
                case (int)FightType.SINGLE_FIGHT: { npc = Variable.BASE_NPCSINGLE.FirstOrDefault(m => m.id == (int)id); break; }
                case (int)FightType.NPC_MONSTER: { npc = Variable.BASE_NPCMONSTER.FirstOrDefault(m => m.id == (int)id); break; }
                case (int)FightType.DUPLICATE_SHARP: { npc = Variable.BASE_TOWERENEMY.FirstOrDefault(m => m.id == (int)id); break; }

                case (int)FightType.BOTH_SIDES:
                case (int)FightType.ONE_SIDE: { var p = tg_fight_personal.GetFindByUserId(id); return p == null ? null : ConvertPlayMatrix(p); }
                default: { return null; }
            }
            if (npc == null) return null;
            var model = GetRivalMatrix(npc.matrix);
            model.yin = BuildYin(npc.yinEffectId);
            return model;
        }

        /// <summary>构建NPC个人战数据</summary>
        private PlayMatrix GetRivalMatrix(String matrix)
        {
            var model = new PlayMatrix();
            var matrix_rid = matrix.Split(',');
            switch (RivalFightType)
            {
                case (int)FightType.NPC_MONSTER: { matrix_rid = GetMatrixRids(matrix_rid); break; }
            }

            for (var i = 0; i < matrix_rid.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        model.r1 = ConvertRole(Variable.BASE_NPCROLE.FirstOrDefault(m => m.id == Convert.ToInt32(matrix_rid[0])), -1); break;
                    case 1:
                        model.r2 = ConvertRole(Variable.BASE_NPCROLE.FirstOrDefault(m => m.id == Convert.ToInt32(matrix_rid[1])), -2); break;
                    case 2:
                        model.r3 = ConvertRole(Variable.BASE_NPCROLE.FirstOrDefault(m => m.id == Convert.ToInt32(matrix_rid[2])), -3); break;
                    case 3:
                        model.r4 = ConvertRole(Variable.BASE_NPCROLE.FirstOrDefault(m => m.id == Convert.ToInt32(matrix_rid[3])), -4); break;
                    case 4:
                        model.r5 = ConvertRole(Variable.BASE_NPCROLE.FirstOrDefault(m => m.id == Convert.ToInt32(matrix_rid[4])), -5); break;
                }
            }
            return model;
        }

        /// <summary> 供点将用   多个Npc武将Id抽取5个 </summary>
        /// <param name="list">武将Id集合</param>
        /// <returns></returns>
        private string[] GetMatrixRids(string[] list)
        {
            int count = 5;
            int number = 4;
            var rolehome = Variable.BASE_ROLE_HOME.FirstOrDefault(m => m.id == RoleHomeId);
            if (rolehome != null)
            {
                count = rolehome.count;
                number = count - 1;
            }
            if (list.Count() <= count) return list;
            var numbers = RNG.Next(1, list.Count() - 1, number).ToList();

            ICollection<string> l = new Collection<string>();
            l.Add(list[0]);
            for (int i = 0; i < numbers.Count(); i++)
            {
                l.Add(list[numbers[i]]);
            }
            return l.ToArray();
        }

        #endregion

        #region 实体转换

        /// <summary> tg_fight_personal TO PlayMatrix </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private PlayMatrix ConvertPlayMatrix(tg_fight_personal model)
        {
            var ids = new List<Int64> { model.matrix1_rid, model.matrix2_rid, model.matrix3_rid, model.matrix4_rid, model.matrix5_rid, };
            var list = tg_role.GetFindAllByIds(ids);
            return new PlayMatrix
            {
                r1 = ConvertRole(list.FirstOrDefault(m => m.id == model.matrix1_rid)),
                r2 = ConvertRole(list.FirstOrDefault(m => m.id == model.matrix2_rid)),
                r3 = ConvertRole(list.FirstOrDefault(m => m.id == model.matrix3_rid)),
                r4 = ConvertRole(list.FirstOrDefault(m => m.id == model.matrix4_rid)),
                r5 = ConvertRole(list.FirstOrDefault(m => m.id == model.matrix5_rid)),
                yin = model.yid == 0 ? null : tg_fight_yin.FindByid(model.yid),
                user_id = model.user_id,
            };
        }

        /// <summary> BaseNpcRole TO Role </summary>
        /// <param name="model"></param>
        /// <param name="id">指定武将的主Id</param>
        private Role ConvertRole(BaseNpcRole model, Int64 id)
        {
            if (model == null) return null;
            return new Role
            {
                id = id,
                damage = 0,
                lv = model.lv,
                IgnoreDuck = 0,
                angerCount = 0,
                hp = model.life,
                baseId = model.id,
                attack = model.attack,
                defense = model.defense,
                buffVos = new List<Buff>(),
                buffVos2 = new List<Buff>(),
                hurtReduce = model.hurtReduce,
                foreverBuffVos = new List<Buff>(),
                hurtIncrease = model.hurtIncrease,
                critAddition = model.critAddition,
                critProbability = model.critProbability,
                monsterType = (int)FightRivalType.MONSTER,
                dodgeProbability = model.dodgeProbability,
                mystery = BuildSkill(model.pmystery, id),
                cheatCode = BuildSkill(model.pcheatCode, id),
                mystery_probability = model.mysteryProbability,
                user_id = 0,
            };
        }

        /// <summary> tg_role TO Role </summary>
        private Role ConvertRole(tg_role model)
        {
            if (model == null) return null;
            return new Role
            {
                damage = 0,
                id = model.id,
                angerCount = 0,
                hp = model.att_life,
                lv = model.role_level,
                baseId = model.role_id,
                user_id = model.user_id,
                buffVos = new List<Buff>(),
                buffVos2 = new List<Buff>(),
                hurtReduce = model.att_sub_hurtReduce,
                monsterType = (int)FightRivalType.ROLE,
                critAddition = tg_role.GetTotalCritAddition(model),
                hurtIncrease = model.att_sub_hurtIncrease,
                attack = tg_role.GetTotalAttack(model),
                defense = Convert.ToInt32(model.att_defense),
                critProbability = tg_role.GetTotalCritProbability(model),
                dodgeProbability = tg_role.GetTotalDodgeProbability(model),
                mystery = BuildSkill(tg_role_fight_skill.FindByid(model.art_mystery)),
                mystery_probability = tg_role.GetTotalMysteryProbability(model),
                cheatCode = BuildSkill(tg_role_fight_skill.FindByid(model.art_cheat_code)),
            };
        }

        private List<RoleFightVo> ConvertRoleFightVo(IEnumerable<Role> list)
        {
            return list.Select(ConvertRoleFightVo).ToList();
        }

        private RoleFightVo ConvertRoleFightVo(Role model)
        {
            if (model == null) return null;
            return new RoleFightVo()
            {
                id = model.id,
                baseId = model.baseId,
                monsterType = model.monsterType,
                damage = model.damage,
                hp = model.hp,
                attack = Convert.ToInt32(model.attack),
                defense = Convert.ToInt32(model.defense),
                hurtIncrease = model.hurtIncrease,
                hurtReduce = model.hurtReduce,
                critProbability = model.critProbability,
                critAddition = model.critAddition,
                dodgeProbability = model.dodgeProbability,
                angerCount = model.angerCount,
                buffVos = ConvertBuffVo(model.buffVos),
                buffVos2 = ConvertBuffVo(model.buffVos2),
                mystery = model.mystery != null ? Convert.ToInt32(model.mystery.baseid) : 0,
                cheatCode = model.cheatCode != null ? Convert.ToInt32(model.cheatCode.baseid) : 0,

            };
        }

        private List<BuffVo> ConvertBuffVo(IEnumerable<Buff> list)
        {
            return list.Select(ConvertBuffVo).ToList();
        }

        private static BuffVo ConvertBuffVo(Buff model)
        {
            return new BuffVo()
            {
                type = model.type,
                buffValue = model.value,
            };
        }

        #endregion

        #region 组装数据

        /// <summary>根据印效果id组装Yin </summary>
        /// <param name="effectId">效果表id</param>
        private tg_fight_yin BuildYin(int effectId)
        {
            var baseyinEffect = Variable.BASE_YINEFFECT.FirstOrDefault(m => m.id == effectId);
            return baseyinEffect == null ? null : new tg_fight_yin() { yin_id = baseyinEffect.yinId, yin_level = baseyinEffect.level };
        }

        /// <summary> 组装技能 </summary>
        /// <param name="eid">技能效果id</param>
        /// <param name="id">武将主id</param>
        private Skill BuildSkill(int eid, Int64 id)
        {
            if (id == 0) return null;
            var skillEffect = Variable.BASE_FIGHTSKILLEFFECT.FirstOrDefault(m => m.id == eid);
            return skillEffect == null ? null : new Skill { id = id, baseid = skillEffect.skillid, level = skillEffect.level };
        }

        /// <summary> 组装技能</summary>
        private Skill BuildSkill(tg_role_fight_skill skill)
        {
            return skill == null ? null : new Skill { id = skill.id, baseid = skill.skill_id, level = skill.skill_level };
        }

        #endregion

        #region 战斗全局变量

        /// <summary> 先手方用户id </summary>
        public Int64 FirstUserId;

        /// <summary> 先手方出手位置 </summary>
        public int FirstNumber;

        /// <summary> 后手方出手位置 </summary>
        public int AfterNumber;

        /// <summary> 当前是否攻击方发起攻击 </summary>
        public bool IsFirst = true;

        /// <summary>对手战斗类型 </summary>
        public int RivalFightType;

        /// <summary>对手昵称 </summary>
        public string RivalName = "";

        /// <summary> 先手方印</summary>
        public YinVo FirstYinVo = new YinVo();

        /// <summary> 后手方印</summary>
        public YinVo AfterYinVo = new YinVo();

        /// <summary>双方阵中武将信息集合 血量随着变化</summary>
        public Dictionary<Int64, Role> AllRoles = new Dictionary<Int64, Role>();

        /// <summary>双方阵中武将信息集合 血量固定 </summary>
        public Dictionary<Int64, Role> AllRolesHp = new Dictionary<Int64, Role>();

        /// <summary>先手方阵中武将信息 key:位置</summary>
        public Dictionary<int, Int64> FirstRoles = new Dictionary<int, Int64>();

        /// <summary>后手方阵中武将信息 key:位置</summary>
        public Dictionary<int, Int64> AfterRoles = new Dictionary<int, Int64>();

        /// <summary> 玩家当前印计数 </summary>
        public Dictionary<Int64, int> YinCount = new Dictionary<Int64, int>();

        /// <summary> 武将所有在回合 </summary>
        public Dictionary<Int64, int> RoleRound = new Dictionary<Int64, int>();

        /// <summary> 玩家职业系数  key:用户id  value:职业系数</summary>
        public Dictionary<decimal, double> Vocations = new Dictionary<decimal, double>();

        /// <summary> 武将Buff状态 </summary>
        public Dictionary<Int64, List<Buff>> RoleState = new Dictionary<Int64, List<Buff>>();

        /// <summary> 全局当前回合数 </summary>
        public int Round = 0;

        public int RoleHomeId = 0;
        #endregion
    }


    #region 战斗相关实体

    public class PlayMatrix
    {
        public Role r1 { get; set; }

        public Role r2 { get; set; }

        public Role r3 { get; set; }

        public Role r4 { get; set; }

        public Role r5 { get; set; }

        public tg_fight_yin yin { get; set; }

        /// <summary>玩家编号</summary>
        public Int64 user_id { get; set; }
    }

    public class Role
    {
        /// <summary>武将主键</summary>
        public Int64 id { get; set; }

        /// <summary>基础 id </summary>
        public int baseId { get; set; }

        /// <summary> 怪物类型  0人物  1怪物 </summary>
        public int monsterType { get; set; }

        /// <summary> 奥义</summary>
        public Skill mystery { get; set; }

        /// <summary>秘技</summary>
        public Skill cheatCode { get; set; }

        /// <summary>伤害 </summary>
        public Int64 damage { get; set; }

        /// <summary> 生命 </summary>
        public Int64 hp { get; set; }

        /// <summary>攻击</summary>
        public Double attack { get; set; }

        /// <summary>防御 </summary>
        public Double defense { get; set; }

        /// <summary> 增伤 </summary>
        public Double hurtIncrease { get; set; }

        /// <summary>减伤</summary>
        public Double hurtReduce { get; set; }

        /// <summary> 会心几率 </summary>
        public Double critProbability { get; set; }

        /// <summary> 会心加成  </summary>
        public Double critAddition { get; set; }

        /// <summary> 闪避几率 </summary>
        public Double dodgeProbability { get; set; }

        /// <summary>奥义触发几率</summary>
        public Double mystery_probability { get; set; }

        /// <summary> 无视闪避几率 </summary>
        public Double IgnoreDuck { get; set; }

        /// <summary> 气力值 </summary>
        public int angerCount { get; set; }

        /// <summary>持续技能Buff [BuffVo]</summary>
        public List<Buff> buffVos { get; set; }

        /// <summary>新增技能Buff [BuffVo]</summary>
        public List<Buff> buffVos2 { get; set; }

        /// <summary>永久Buff BuffVo</summary>
        public List<Buff> foreverBuffVos { get; set; }

        /// <summary> 等级</summary>
        public int lv { get; set; }

        /// <summary> 用户id</summary>
        public Int64 user_id { get; set; }
    }

    public class Skill
    {
        public Int64 id { get; set; }

        public int baseid { get; set; }

        public int level { get; set; }
    }

    public class Buff
    {
        public int type { get; set; }

        public int round { get; set; }

        public Int64 value { get; set; }
    }

    #endregion
}
