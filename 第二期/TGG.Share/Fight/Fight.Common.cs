using System.Diagnostics;
using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;
using TGG.Core.Vo.Role;
using TGG.SocketServer;
using System.Threading;

namespace TGG.Share.Fight
{
    public partial class Fight
    {
        #region 准备数据

        /// <summary>战斗方法</summary>
        /// <param name="session">session</param>
        /// <param name="hp"> 对手方要修改的血量 </param>
        private ResultType FightMethod(tg_fight_personal personal, Int64 roleid, int vocation, Int64 rivalid, Int64 hp)
        {
            //start
            var userid = personal.user_id;
            var result = ReadyData(personal, roleid, vocation, rivalid, hp); //准备数据
            if (result != (int)ResultType.SUCCESS) return result;            //准备数据处理结果验证
            BuildFight(userid);                                              //开始战斗  战斗过程组装
            return ResultType.SUCCESS;
        }

        /// <summary> 准备数据 </summary>
        /// <param name="session">session</param>
        /// <param name="hp">要更变的血量值 不更变填0</param>
        /// <returns></returns>
        private ResultType ReadyData(tg_fight_personal personal, Int64 roleid, int vocation, Int64 rivalid, Int64 hp = 0)
        {
            Init();
            var userid = personal.user_id;

            var rivalPersonal = GetRivalFightPersonal(rivalid);       //获取对方阵形
            if (rivalPersonal == null) return ResultType.FIGHT_RIVAL_PERSONAL_ERROR;    //获取对方阵形处理结果验证

            VocationAdd(userid, rivalid, vocation);
            var oneself = ConvertFightPersonal(personal);            //己方阵形
            oneself = IsUpdatePersonal(roleid, oneself);                        //验证是否为只允许主角上阵
            var result = GetRoleByMatrixAll(oneself, rivalPersonal);             //获取双方阵武将信息
            if (result != ResultType.SUCCESS) return result;                     //获取双方阵武将信息处理结果验证

            #region 在此处修改玩家或NPC属性值等

            IsUpdateHp(userid, roleid, hp);


            #endregion


            WhoFirst(oneself, rivalPersonal);                                     //判断先后手,先后手标记全局

            SearchCardGroup();                                                    //检索卡组  套卡

            return ResultType.SUCCESS;
        }

        /// <summary> 验证玩家是否还在战斗计算中 </summary>
        /// <param name="userid">要验证的玩家</param>
        private bool IsFight(Int64 userid)
        {
            if (Variable.PlayerFight.ContainsKey(userid)) return true;
            Variable.PlayerFight.AddOrUpdate(userid, 0, (m, n) => 0);
            return false;
        }

        /// <summary> 将玩家移除战斗 </summary>
        /// <param name="userid">要修改的玩家Id</param>
        private void RemoveFightState(Int64 userid)
        {
            int a;
            if (!Variable.PlayerFight.ContainsKey(userid)) return;
            Variable.PlayerFight.TryRemove(userid, out a);
        }

        /// <summary> 职业系数 </summary>
        /// <param name="session"></param>
        private void VocationAdd(Int64 userid, Int64 rivalid, int vocation)
        {
            var base_vocation = Variable.BASE_VOCATION.FirstOrDefault(m => m.vocation == vocation);
            if (base_vocation == null) return;
            dic_vocation.Add(userid, base_vocation.fight);
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
            if (!dic_vocation.ContainsKey(rivalid))
                dic_vocation.Add(rivalid, bv.fight);
        }

        /// <summary>组装战斗数据 </summary>
        /// <param name="userid">己方用户id</param>
        private void BuildFight(Int64 userid)
        {
            vo.attackId = Convert.ToDouble(attack_matrix.user_id);  //先手用户id
            vo.wuJiangName = RivalName;
            vo.yinA = attack_matrix.yinvo;                          //先手的印Vo
            vo.yinB = defense_matrix.yinvo;                         //后手的印Vo

#if DEBUG
            XTrace.WriteLine("{0}  {1}  ", "BuildFight", "开始战斗");
#endif

        start: //战斗过程组装
            if (!BuildMove(true))                                   //先手操作
                goto win;
            if (!BuildMove(false))                                  //后手操作
                goto win;
            goto start;
        win:
            vo.isWin = GetWinResult(userid);
            //XTrace.WriteLine("{0}  {1}  {2} ", "BuildFight", "我方战斗:", vo.isWin ? "胜利" : "失败");
#if DEBUG
            XTrace.WriteLine("{0}  {1}  {2} ", "BuildFight", "战斗结束", vo.isWin ? "胜利" : "失败");
#endif
        }

        /// <summary>组装出招数据 </summary>
        /// <param name="flag">true:先手</param>
        /// <returns>是否完全组装完成</returns>
        private bool BuildMove(bool flag)
        {
            IsAttack = flag;
            move = new MovesVo();
            list_move = new List<MovesVo>();

            var attackrole = GetShotRole();                                               //最先出手未死的战斗武将
            if (attackrole == null) { WIN = IsAttack ? 1 : 0; return false; }

            var matrix = GetMatrix(true);
            move.attackId = Convert.ToDouble(attackrole.id);                              //攻击武将Id
            if (!RoundCount(attackrole, move))                                            //出手方回合计数 +1
                return true;

#if DEBUG
            XTrace.WriteLine("{0}  {1} - {2}  {3} - {4}  ---- {5} ----", "BuildMove", "当前出手武将", attackrole.id, "当前出手用户", attackrole.user_id, "一次出手");
#endif

            AddYinCount(matrix.user_id);                                                     //出手方印数+1 
            GetYinCount(move);                                                            //双方印数
            if (!Shot(attackrole))                                                        //武将出手
                return false;
            RolePositionChange(matrix, attackrole.id);                                    //改变该武将位置
            vo.moves.Add(list_move);                                                      //出手记录
            UpdateRolePosition();

            return true;
        }

        /// <summary>出手</summary>
        private bool Shot(FightRole attackrole)
        {
#if DEBUG
            XTrace.WriteLine(string.Format("{0} {1}", "出手武将", "验证"));
#endif
            if (StateVerification(attackrole, (int)FightingSkillType.DIZZINESS)) return true; //验证武将是否眩晕
            if (StateVerification(attackrole, (int)FightingSkillType.SEAL))                   //验证武将是否封印
            {
#if DEBUG
                XTrace.WriteLine(string.Format("{0} {1}", "出手武将", "被封印"));
#endif
                goto attack;                                                     //跳过秘技验证 进行普通攻击
            }

            if (IsMystery(attackrole))                                           //是否释放奥义
                goto Yin;
            if (IsSkill(attackrole))                                             //是否释放秘技
                goto Yin;                                                        //跳过普通攻击 进行印验证
        attack:
            if (!NormalAttack(attackrole))                                       //普通攻击
                return false;
        Yin:
            IsYin();                                                             //是否释放印技能
            return true;
        }


        #region BUFF操作

        /// <summary> 移除回合过期的Buff </summary>
        /// <param name="flag">是否回合计算方法处用</param>
        private void RemoveBuff(bool flag)
        {
            var list = flag ? list_buff.Where(m => m.round <= Round).ToList()
                : list_buff.Where(m => m.round < Round).ToList();
            foreach (var item in list)
            {
                var role = list_attack_role.Where(m => m != null).FirstOrDefault(m => m.id == item.id) ??
                           list_defense_role.Where(m => m != null).FirstOrDefault(m => m.id == item.id);
                if (role == null) continue;
                if (item.state == 0)
                {
                    //移除眩晕或封印 
                    if (item.type != (int)FightingSkillType.DIZZINESS && item.type != (int)FightingSkillType.SEAL)
                    {
                        list_buff.Remove(item);
                        RemoveRoleBuffValues(role, item.type, item.values);
                        continue;
                    }
                    if (Round != 0 && !flag) continue;
                    list_buff.Remove(item);
                    var b = role.buffVos2.FirstOrDefault(m => m.type == item.type);
                    role.buffVos2.Remove(b);
                    continue;
                }
                list_buff.Remove(item);
                var buff = role.buffVos.FirstOrDefault(m => m.type == item.type);
                role.buffVos.Remove(buff);
                RemoveRoleBuffValues(role, item.type, item.values);
#if DEBUG
                XTrace.WriteLine(string.Format("{0} {1} {2}", "移除 " + role.id + " 身上BUFF ", "类型 " + item.type, "值 " + item.values));
#endif
            }
        }

        /// <summary> 移除武将身上指定类型的旧Buff 用于替换Buff时用</summary>
        /// <param name="role">要操作的武将</param>
        /// <param name="type">要移除的类型</param>
        private void RemoveRoleBuff(FightRole role, int type)
        {
            var model = list_buff.FirstOrDefault(m => m.id == role.id && m.type == type && m.state == 1);
            if (model == null) return;
            var buff = role.buffVos.FirstOrDefault(m => m.type == type);
            if (buff == null) return;
            list_buff.Remove(model);
            role.buffVos.Remove(buff);
            RemoveRoleBuffValues(role, type, model.values);
        }

        /// <summary> 移除武将身上Buff值 </summary>
        private void RemoveRoleBuffValues(FightRole role, int type, double values)
        {
            var l = new List<FightRole> { role };
            var skillEffects = new FightSkillEffects { type = type, values = values, round = -100 };//round是负数表示恢复属性影响值
            if (type != (int)FightingSkillType.DIZZINESS && type != (int)FightingSkillType.SEAL) EffectType(l, skillEffects);
        }

        /// <summary> 将新Buff改为旧Buff </summary>
        private void UpdateBuff()
        {
            var list = list_buff.Where(m => m.state == 0).ToList();
            foreach (var item in list)
            {
                var role = list_attack_role.Where(m => m != null).FirstOrDefault(m => m.id == item.id) ??
                           list_defense_role.Where(m => m != null).FirstOrDefault(m => m.id == item.id);
                if (role == null) continue;
                if (item.round == 0)
                {
                    var buff = role.buffVos2.FirstOrDefault(m => m.type == item.type);
                    role.buffVos2.Remove(buff);
                    continue;
                }
                if (item.type == (int)FightingSkillType.DIZZINESS || item.type == (int)FightingSkillType.SEAL) continue;
                var b = role.buffVos2.FirstOrDefault(m => m.type == item.type);
                role.buffVos.Add(b);
                role.buffVos2.Remove(b);
                item.state = 1;
            }
        }

        #endregion



        #region 初始化数据

        /// <summary>初始化</summary>
        private void Init()
        {
            WIN = 0;
            Round = 0;
            IsAttack = true;
            attack_number = 0;
            defense_number = 0;
            vo = new FightVo();
            move = new MovesVo();
            list_move = new List<MovesVo>();
            list_skill = new List<SkillVo>();
            list_role = new List<FightRole>();
            attack_matrix = new FightPersonal();
            list_role_hp = new List<FightRole>();
            defense_matrix = new FightPersonal();
            list_buff = new List<FightRoleBuff>();
            list_attack_role = new List<FightRole>();
            list_defense_role = new List<FightRole>();
            dic_round = new Dictionary<decimal, int>();
            dic_yincount = new Dictionary<decimal, int>();
            dic_vocation = new Dictionary<decimal, double>();
        }

        /// <summary>初始化全局印计数</summary>
        private void InitYinCount()
        {
            if (!dic_yincount.ContainsKey(attack_matrix.user_id))
                dic_yincount.Add(attack_matrix.user_id, 1);
            if (!dic_yincount.ContainsKey(defense_matrix.user_id))
                dic_yincount.Add(defense_matrix.user_id, 0);
        }

        /// <summary> 初始化回合 </summary>
        private void InitRound()
        {
            foreach (var item in list_role)
            {
                dic_round.Add(item.id, 0);
            }

            var movesvo = new MovesVo();
            BuildMovesvoRole(movesvo);
            list_move.Add(movesvo);
        }

        /// <summary> 初始武将伤害值 </summary>
        private void InitRoleDamage()
        {
            foreach (var item in list_attack_role)
            {
                if (item == null) continue;
                item.damage = 0;
            }
            foreach (var item in list_defense_role)
            {
                if (item == null) continue;
                item.damage = 0;
            }
        }

        #endregion

        #region 操作全局变量方法

        /// <summary>记录全局属性的武将</summary>
        /// <param name="matrix">阵</param>
        /// <param name="flag">true:先手   false:后手</param>
        private void GlobalRole(FightPersonal matrix, bool flag)
        {
            var list = new List<FightRole>
            {
                matrix.matrix1_rid != 0 ? list_role.FirstOrDefault(m => m.id == matrix.matrix1_rid) : null,
                matrix.matrix2_rid != 0 ? list_role.FirstOrDefault(m => m.id == matrix.matrix2_rid) : null,
                matrix.matrix3_rid != 0 ? list_role.FirstOrDefault(m => m.id == matrix.matrix3_rid) : null,
                matrix.matrix4_rid != 0 ? list_role.FirstOrDefault(m => m.id == matrix.matrix4_rid) : null,
                matrix.matrix5_rid != 0 ? list_role.FirstOrDefault(m => m.id == matrix.matrix5_rid) : null
            };
            if (flag)
                list_attack_role = list;
            else
                list_defense_role = list;
        }

        /// <summary> 武将位置变化  下次攻击武将排在前面 </summary>
        /// <param name="matrix">阵</param>
        /// <param name="roleid">当前攻击武将主键id</param>
        private void RolePositionChange(FightPersonal matrix, Int64 roleid)
        {
            if (matrix.matrix1_rid == roleid)
            {
                matrix.matrix1_rid = matrix.matrix2_rid;
                matrix.matrix2_rid = matrix.matrix3_rid;
                matrix.matrix3_rid = matrix.matrix4_rid;
                matrix.matrix4_rid = matrix.matrix5_rid;
                matrix.matrix5_rid = roleid;
                return;
            }
            if (matrix.matrix2_rid == roleid)
            {
                matrix.matrix2_rid = matrix.matrix3_rid;
                matrix.matrix3_rid = matrix.matrix4_rid;
                matrix.matrix4_rid = matrix.matrix5_rid;
                matrix.matrix5_rid = matrix.matrix1_rid;
                matrix.matrix1_rid = roleid;
                return;
            }
            if (matrix.matrix3_rid == roleid)
            {
                matrix.matrix3_rid = matrix.matrix4_rid;
                matrix.matrix4_rid = matrix.matrix5_rid;
                matrix.matrix5_rid = matrix.matrix1_rid;
                matrix.matrix1_rid = matrix.matrix2_rid;
                matrix.matrix2_rid = roleid;
                return;
            }
            if (matrix.matrix4_rid == roleid)
            {
                matrix.matrix4_rid = matrix.matrix5_rid;
                matrix.matrix5_rid = matrix.matrix1_rid;
                matrix.matrix1_rid = matrix.matrix2_rid;
                matrix.matrix2_rid = matrix.matrix3_rid;
                matrix.matrix3_rid = roleid;
                return;
            }
            if (matrix.matrix5_rid == roleid)
            {
                matrix.matrix5_rid = matrix.matrix1_rid;
                matrix.matrix1_rid = matrix.matrix2_rid;
                matrix.matrix2_rid = matrix.matrix3_rid;
                matrix.matrix3_rid = matrix.matrix4_rid;
                matrix.matrix4_rid = roleid;
                return;
            }
        }

        /// <summary> 更改当前出手方下次出手武将的位置 </summary>
        private void UpdateRolePosition()
        {
            if (IsAttack)
                if (attack_number >= 4)
                    attack_number = 0;
                else
                    attack_number = attack_number + 1;
            else
                if (defense_number >= 4)
                    defense_number = 0;
                else
                    defense_number = defense_number + 1;
        }

        /// <summary>先手判定</summary>
        /// <param name="personal">己方的阵</param>
        /// <param name="rivalPersonal">敌方的阵</param>
        private void WhoFirst(FightPersonal personal, FightPersonal rivalPersonal)
        {
            switch (RivalFightType)
            {
                case (int)FightType.SIEGE:
                case (int)FightType.BUILDING:
                    {
                        GlobalMatrix(personal, rivalPersonal);  //活动始终玩家先手
                        return;
                    }
                default:
                    {
                        var attack = list_role.Where(m => m.user_id == personal.user_id).Sum(m => m.attack);
                        var rivalAttack = list_role.Where(m => m.user_id == rivalPersonal.user_id).Sum(m => m.attack);
                        if (attack >= rivalAttack) GlobalMatrix(personal, rivalPersonal);  //攻击值验证  判定先后手
                        else GlobalMatrix(rivalPersonal, personal);
                        return;
                    }
            }
        }

        /// <summary>先后手 阵和武将 记录到全局属性中</summary>
        /// <param name="matrix">先手方阵</param>
        /// <param name="_matrix">后手方阵</param>
        private void GlobalMatrix(FightPersonal matrix, FightPersonal _matrix)
        {
            attack_matrix = matrix;
            defense_matrix = _matrix;
            GlobalRole(attack_matrix, true);
            GlobalRole(defense_matrix, false);
        }

        /// <summary> 重新更新全局武将回合</summary>
        private void UpdateRound()
        {
            var list = list_role.Where(m => m.hp > 0).ToList();
            var dic = list.Where(item => dic_round.ContainsKey(item.id))
                .ToDictionary<FightRole, decimal, int>(item => item.id, item => dic_round[item.id]);
            dic_round.Clear();
            dic_round = dic;
        }

        /// <summary>增加印计数</summary>
        private void AddYinCount(decimal userid)
        {
            if (!dic_yincount.ContainsKey(userid))//初始印数
                InitYinCount();
            else
                dic_yincount[userid] += 1;
        }

        /// <summary> 回合计数 </summary>
        private bool RoundCount(FightRole role, MovesVo movesvo)
        {
            if (!dic_round.ContainsKey(role.id)) InitRound();//InitRound方法只会在第一次出手时有效

            //UpdateBuff();//将上次出手武将新Buff改为旧Buff 预防不是当前回合出手

            var values = dic_round[role.id] + 1;
            if (values <= Round) dic_round[role.id] = values;
            else
            {
                if (!IsAttack) return false;
                UpdateRound();
                if (dic_round.ContainsValue(Round - 1)) return false;
#if DEBUG
                XTrace.WriteLine("{0}", "回合结束");
                XTrace.WriteLine(string.Format("{0}  {1}", "开始回合数", values));
#endif

                dic_round[role.id] = values;
                RemoveBuff(true);
            }
            movesvo.times = Round = values;
            return true;
        }

        #endregion

        #region 获取数据

        /// <summary>获取对手战斗阵信息</summary>
        private FightPersonal GetRivalFightPersonal(Int64 id)
        {
            dynamic npc;
            switch (RivalFightType)
            {
                case (int)FightType.SIEGE:
                case (int)FightType.BUILDING:
                case (int)FightType.CONTINUOUS:
                case (int)FightType.NPC_FIGHT_ARMY: { npc = Variable.BASE_NPCARMY.FirstOrDefault(m => m.id == (int)id); break; }
                case (int)FightType.SINGLE_FIGHT: { npc = Variable.BASE_NPCSINGLE.FirstOrDefault(m => m.id == (int)id); break; }
                case (int)FightType.NPC_MONSTER: { npc = Variable.BASE_NPCMONSTER.FirstOrDefault(m => m.id == (int)id); break; }
                case (int)FightType.DUPLICATE_SHARP: { npc = Variable.BASE_TOWERENEMY.FirstOrDefault(m => m.id == (int)id); break; }

                case (int)FightType.BOTH_SIDES:
                case (int)FightType.ONE_SIDE:
                    {
                        var p = tg_fight_personal.GetFindByUserId(id); return p == null ? null : ConvertFightPersonal(p);
                    }
                default: { return null; }
            }
            if (npc == null) return null;
            var model = BuildFightPersonal(npc.matrix);
            model.yinvo = BuildYinVo(npc.yinEffectId);
            return model;
        }

        /// <summary>构建NPC个人战数据</summary>
        private FightPersonal BuildFightPersonal(String matrix)
        {
            var model = new FightPersonal();
            var matrix_rid = matrix.Split(',');
            switch (RivalFightType)
            {
                case (int)FightType.NPC_MONSTER: { matrix_rid = GetMatrixRid(matrix_rid); break; }
            }

            for (var i = 0; i < matrix_rid.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        model.matrix1_rid = Convert.ToInt64(matrix_rid[0]); break;
                    case 1:
                        model.matrix2_rid = Convert.ToInt64(matrix_rid[1]); break;
                    case 2:
                        model.matrix3_rid = Convert.ToInt64(matrix_rid[2]); break;
                    case 3:
                        model.matrix4_rid = Convert.ToInt64(matrix_rid[3]); break;
                    case 4:
                        model.matrix5_rid = Convert.ToInt64(matrix_rid[4]); break;
                }
            }
            //NPC  user_id   暂时用-10000
            //model.user_id = -10000;
            return model;
        }

        /// <summary> 获取双方阵中所有武将 (获取的武将存储到战斗全局 list_role)</summary>
        private ResultType GetRoleByMatrixAll(FightPersonal oneself, FightPersonal rival)
        {
            var ids = GetMatrixRids(oneself);
            if (!ids.Any()) return ResultType.FIGHT_RIVAL_ID_ERROR;
            switch (RivalFightType)
            {
                case (int)FightType.NPC_MONSTER:
                case (int)FightType.NPC_FIGHT_ARMY:
                    {
                        if (!SetNpcFightData(rival)) return ResultType.FIGHT_NPC_BASE_ERROR;
                        GetHireRole(oneself);
                        SetPlayerFightData(ids);

                        return ResultType.SUCCESS;
                    }
                case (int)FightType.SIEGE:
                case (int)FightType.BUILDING:
                case (int)FightType.CONTINUOUS:
                case (int)FightType.SINGLE_FIGHT:
                case (int)FightType.DUPLICATE_SHARP:
                    {
                        if (!SetNpcFightData(rival)) return ResultType.FIGHT_NPC_BASE_ERROR;
                        SetPlayerFightData(ids);
                        return ResultType.SUCCESS;
                    }

                case (int)FightType.BOTH_SIDES:
                case (int)FightType.ONE_SIDE:
                    {
                        ids.AddRange(GetMatrixRids(rival));
                        SetPlayerFightData(ids);
                        return ResultType.SUCCESS;
                    }
                default: { return ResultType.FIGHT_TYPE_ERROR; }
            }
        }

        /// <summary> 获取雇佣武将 </summary>
        private void GetHireRole(FightPersonal personal)
        {
            bool flag = false;
            if (personal.matrix1_rid == 0) //阵中的所有武将
            {
                flag = true;
                var role = GetHireRole(personal.user_id);
                if (role != null)
                {
                    personal.matrix1_rid = role.id;
                    list_role_hp.Add(role);
                }
            }
            if (personal.matrix2_rid == 0 && !flag)
            {
                flag = true;
                var role = GetHireRole(personal.user_id);
                if (role != null)
                {
                    personal.matrix2_rid = role.id;
                    list_role_hp.Add(role);
                }
            }
            if (personal.matrix3_rid == 0 && !flag)
            {
                flag = true;
                var role = GetHireRole(personal.user_id);
                if (role != null)
                {
                    personal.matrix3_rid = role.id;
                    list_role_hp.Add(role);
                }
            }
            if (personal.matrix4_rid == 0 && !flag)
            {
                flag = true;
                var role = GetHireRole(personal.user_id);
                if (role != null)
                {
                    personal.matrix4_rid = role.id;
                    list_role_hp.Add(role);
                }
            }
            if (personal.matrix5_rid == 0 && !flag)
            {
                var role = GetHireRole(personal.user_id);
                if (role != null)
                {
                    personal.matrix5_rid = role.id;
                    list_role_hp.Add(role);
                }
            }
        }

        /// <summary> 获取雇佣武将 </summary>
        private FightRole GetHireRole(Int64 userid)
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
            var role = ConvertNpcRoleFight(npc);
            role.user_id = userid;
            role.monsterType = (int)FightRivalType.MONSTER;
            role.id = -99;
            return role;
        }

        /// <summary> 获取阵中最前面未死的武将 </summary>
        /// <param name="matrix">需要获取武将的阵</param>
        private FightRole GetFrontRole(FightPersonal matrix)
        {
            var list = attack_matrix.user_id == matrix.user_id ?
                list_attack_role.Where(m => m != null).ToList()
                : list_defense_role.Where(m => m != null).ToList();

            if (matrix.matrix1_rid != 0)
            {
                var model = list.FirstOrDefault(m => m.id == matrix.matrix1_rid && m.hp > 0);
                if (model != null)
                {
                    model.damage = 0;
                    return model;
                }
            }
            if (matrix.matrix2_rid != 0)
            {
                var model = list.FirstOrDefault(m => m.id == matrix.matrix2_rid && m.hp > 0);
                if (model != null)
                {
                    model.damage = 0;
                    return model;
                }
            }
            if (matrix.matrix3_rid != 0)
            {
                var model = list.FirstOrDefault(m => m.id == matrix.matrix3_rid && m.hp > 0);
                if (model != null)
                {
                    model.damage = 0;
                    return model;
                }
            }
            if (matrix.matrix4_rid != 0)
            {
                var model = list.FirstOrDefault(m => m.id == matrix.matrix4_rid && m.hp > 0);
                if (model != null)
                {
                    model.damage = 0;
                    return model;
                }
            }
            if (matrix.matrix5_rid != 0)
            {
                var model = list.FirstOrDefault(m => m.id == matrix.matrix5_rid && m.hp > 0);
                if (model != null)
                {
                    model.damage = 0;
                    return model;
                }
            }
            return null;
        }

        /// <summary> 获取阵中武将Id </summary>
        /// <param name="personal">要获取的阵</param>
        /// <returns>阵中武将Id集合</returns>
        private List<Int64> GetMatrixRids(FightPersonal personal)
        {
            var ids = new List<Int64>();
            if (personal.matrix1_rid != 0)                             //阵中的所有武将
                ids.Add(personal.matrix1_rid);
            if (personal.matrix2_rid != 0)
                ids.Add(personal.matrix2_rid);
            if (personal.matrix3_rid != 0)
                ids.Add(personal.matrix3_rid);
            if (personal.matrix4_rid != 0)
                ids.Add(personal.matrix4_rid);
            if (personal.matrix5_rid != 0)
                ids.Add(personal.matrix5_rid);
            return ids;
        }

        /// <summary> 获取集合武将中的技能和奥义 </summary>
        private void GetPlayerRoleSkill(List<tg_role> list)
        {
            var ids = list.Select(m => Convert.ToInt64(m.art_cheat_code)).ToList();
            ids.AddRange(list.Select(m => Convert.ToInt64(m.art_mystery)));
            ids = ids.Where(m => m != 0).ToList();
            if (ids.Any())
                list_skill.AddRange(ConvertSkillVoList(tg_role_fight_skill.GetFindAllByIds(ids))); //获取的技能存储到战斗全局 list_skill
        }

        /// <summary> 获取当前出手的武将 </summary>
        private FightRole GetShotRole()
        {
            var list = IsAttack ? list_attack_role : list_defense_role;
            for (int i = 0; i < 5; i++)
            {
                var number = IsAttack ? attack_number : defense_number;
                if (number < 5 && number >= 0)
                {
                    if (list.Count() - 1 >= number)
                    {
                        var role = list[number];
                        if (role != null && role.hp > 0)
                        {
                            role.damage = 0; return role;
                        }
                    }
                }
                UpdateRolePosition();
            }
            return null;
        }

        /// <summary> 获取阵形 true:当前攻击方,false:当前防守方</summary>
        /// <param name="flag">true:当前攻击方,false:当前防守方</param>
        private FightPersonal GetMatrix(bool flag)
        {
            if (IsAttack)
                return flag ? attack_matrix : defense_matrix;
            return flag ? defense_matrix : attack_matrix;
        }

        /// <summary> 获取对手类型  玩家或怪物 </summary>
        private int GetRivalType()
        {
            switch (RivalFightType)
            {
                case (int)FightType.BOTH_SIDES:
                case (int)FightType.ONE_SIDE: { return (int)FightRivalType.ROLE; }
                default: { return (int)FightRivalType.MONSTER; }
            }
        }


        #endregion

        #region 数据验证

        /// <summary> 验证是否修改为只有主角武将阵形 </summary>
        /// <param name="roleid">当前玩家主角武将Id</param>
        /// <param name="oneself">当前玩家 FightPersonal</param>
        private FightPersonal IsUpdatePersonal(Int64 roleid, FightPersonal oneself)
        {
            switch (RivalFightType)
            {
                case (int)FightType.CONTINUOUS:
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

        /// <summary> 验证是否修改需要改变血量的模块 </summary>
        /// <param name="session"> session </param>
        /// <param name="hp"> 血量 </param>
        private void IsUpdateHp(Int64 userid, Int64 roleid, Int64 hp)
        {
            if (hp <= 0) return;
            FightRole fightrole;   //变化血量
            FightRole _fightrole;  //固定血量
            switch (RivalFightType)
            {
                case (int)FightType.SIEGE:
                case (int)FightType.BUILDING:
                    {
                        fightrole = list_role.FirstOrDefault(m => m != null && m.user_id != userid);
                        _fightrole = list_role_hp.FirstOrDefault(m => m != null && m.user_id != userid);
                        break;
                    }
                case (int)FightType.CONTINUOUS:
                case (int)FightType.DUPLICATE_SHARP:
                    {
                        fightrole = list_role.FirstOrDefault(m => m.id == roleid);
                        _fightrole = list_role_hp.FirstOrDefault(m => m.id == roleid);
                        break;
                    }
                default: { return; }
            }
            if (fightrole == null || _fightrole == null) return;
            fightrole.hp = hp;
            _fightrole.hp = hp;
        }

        /// <summary> 判断用户是否是先手 </summary>
        /// <param name="userid">用户id</param>
        private bool IsFirst(decimal userid)
        {
            return attack_matrix.user_id == userid;
        }

        /// <summary> 是否释放奥义</summary>
        /// <param name="attackrole">武将vo</param>
        private bool IsMystery(FightRole attackrole)
        {
            if (attackrole.mystery == null) return false;
            var role = list_role_hp.FirstOrDefault(m => m.id == attackrole.id);  //初始时武将状态
            if (role == null) return false;

            double hpcount = Convert.ToDouble(attackrole.hp) / Convert.ToDouble(role.hp);
            var probability = attackrole.mystery_probability;
            if (hpcount <= 0.5) probability = probability + (60 - (hpcount * 100));
#if DEBUG
            probability = 60;
#endif
            return IsTrue(probability) && SkillParsing(attackrole.mystery, (int)SkillType.MYSTERY);
        }

        /// <summary>是否释放秘技</summary>
        /// <param name="attackrole">武将vo</param>
        private bool IsSkill(FightRole attackrole)
        {
#if DEBUG
            //attackrole.cheatCode = new SkillVo()
            //{
            //    baseId = 16152012,
            //    level = 1,
            //};
            //attackrole.cheatCode = null;
#endif
            return attackrole.cheatCode != null && SkillParsing(attackrole.cheatCode, (int)SkillType.CHEATCODE);
        }

        /// <summary> 武将状态验证 </summary>
        /// <param name="role">当前武将</param>
        /// <param name="type">状态类型</param>
        /// <returns>true:有特殊状态 否则false</returns>
        private bool StateVerification(FightRole role, int type)
        {
            if (role.buffVos2.FirstOrDefault(m => m.type == type) == null) return false;
            if (type != (int)FightingSkillType.DIZZINESS) return true;
            var movesvo = move.CloneDeepEntity();
            BuildMovesVo(movesvo, (int)SkillType.COMMON);
#if DEBUG
            XTrace.WriteLine(string.Format("{0} {1}", "出手武将", "被眩晕"));
#endif
            return true;
        }

        #endregion

        #region 调用战斗涉及的其他模块

        /// <summary> 职业任务战斗结果 </summary>
        private void TaskResult(TGGSession session, FightResultType result)
        {
            if (!session.TaskItems.Any()) return;
            foreach (var item in session.TaskItems)
            {
                switch (item.Type)
                {
                    case TaskStepType.TYPE_KILL:
                    case TaskStepType.NPC_FIGHT_TIMES: { if (item.Target == session.Fight.Rival) item.Result = result; break; };
                    default: { break; }
                }
            }
            dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Task", "TASK_PUSH");//调用任务模块
            obje.TaskUpdate(session.Player.User.id);// 职业任务每日刷新
        }

        /// <summary> 调用武器使用统计 </summary>
        private void WeaponCount(Int64 userid)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "WeaponCount", "调用武器使用统计");
            var sw = Stopwatch.StartNew();
#endif
            (new Title()).RoleFightMethods(userid);

#if DEBUG
            sw.Stop();
            XTrace.WriteLine("总运行时间：{0} 毫秒", sw.ElapsedMilliseconds.ToString());
#endif
        }

        #endregion

        #region 实体转换

        /// <summary>tg_role实体转换RoleFight</summary>
        private List<RoleFightVo> ConvertRoleFightVoList(IEnumerable<FightRole> list)
        {
            var l = new List<RoleFightVo>();
            foreach (var item in list)
            {
                if (item == null) { l.Add(null); continue; }
                var model = item.CloneDeepEntity();
                l.Add(EntityToVo.ToFightRoleFightVo(model));
            }
            return l;
        }

        /// <summary>tg_role_fight_skill 实体集合 转换 SkillVo集合</summary>
        private IEnumerable<SkillVo> ConvertSkillVoList(IEnumerable<tg_role_fight_skill> list)
        {
            return list.Select(EntityToVo.ToFightSkillVo).ToList();
        }

        /// <summary>tg_role实体转换RoleFight</summary>
        private IEnumerable<FightRole> ConvertRoleFightList(IEnumerable<tg_role> list)
        {
            return list.Select(ConvertRoleFight).ToList();
        }

        /// <summary>tg_role实体转换RoleFight</summary>
        private FightRole ConvertRoleFight(tg_role model)
        {
            return new FightRole
            {
                damage = 0,
                id = model.id,
                angerCount = 0,
                hp = model.att_life,
                initHp = model.att_life,
                lv = model.role_level,
                baseId = model.role_id,
                user_id = model.user_id,
                buffVos = new List<BuffVo>(),
                hurtReduce = model.att_sub_hurtReduce,
                monsterType = (int)FightRivalType.ROLE,
                critAddition = tg_role.GetTotalCritAddition(model),
                hurtIncrease = model.att_sub_hurtIncrease,
                attack = tg_role.GetTotalAttack(model),
                defense = Convert.ToInt32(model.att_defense),
                critProbability = tg_role.GetTotalCritProbability(model),
                dodgeProbability = tg_role.GetTotalDodgeProbability(model),
                mystery = list_skill.FirstOrDefault(m => m.id == model.art_mystery),
                mystery_probability = tg_role.GetTotalMysteryProbability(model),
                cheatCode = list_skill.FirstOrDefault(m => m.id == model.art_cheat_code),
            };
        }

        /// <summary> BaseNpcRole 转换 RoleFight </summary>
        private FightRole ConvertNpcRoleFight(BaseNpcRole model)
        {
            return new FightRole
            {
                id = model.id,
                damage = 0,
                lv = model.lv,
                angerCount = 0,
                hp = model.life,
                //user_id = 10000,
                initHp = model.life,
                baseId = model.id,
                attack = model.attack,
                defense = model.defense,
                hurtReduce = model.hurtReduce,
                hurtIncrease = model.hurtIncrease,
                critAddition = model.critAddition,
                critProbability = model.critProbability,
                dodgeProbability = model.dodgeProbability,
                mystery = BuildNpcSkillVo(model.pmystery),
                cheatCode = BuildNpcSkillVo(model.pcheatCode),
                mystery_probability = model.mysteryProbability,
                buffVos = new List<BuffVo>(),
            };
        }

        /// <summary> tg_fight_personal TO FightPersonal </summary>
        private FightPersonal ConvertFightPersonal(tg_fight_personal model)
        {
            var yin = tg_fight_yin.FindByid(model.yid);
            return new FightPersonal()
            {
                id = model.id,
                matrix1_rid = model.matrix1_rid,
                matrix2_rid = model.matrix2_rid,
                matrix3_rid = model.matrix3_rid,
                matrix4_rid = model.matrix4_rid,
                matrix5_rid = model.matrix5_rid,
                user_id = model.user_id,
                yinvo = model.yid == 0 ? null : (yin == null ? null : EntityToVo.ToFightYinVo(yin)),
            };
        }

        #endregion

        #region 组装数据

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(int result, FightVo model)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", result },
            { "fight", model },
            };
            return dic;
        }

        /// <summary>根据印效果id组装YinVo </summary>
        /// <param name="effectId">效果表id</param>
        private YinVo BuildYinVo(int effectId)
        {
            var baseyinEffect = Variable.BASE_YINEFFECT.FirstOrDefault(m => m.id == effectId);
            return baseyinEffect == null ? null : new YinVo { baseid = baseyinEffect.yinId, level = baseyinEffect.level };
        }

        /// <summary> 根据技能效果id组装NPC技能Vo </summary>
        /// <param name="id">技能效果id</param>
        private SkillVo BuildNpcSkillVo(int id)
        {
            if (id == 0) return null;
            var skillEffect = Variable.BASE_FIGHTSKILLEFFECT.FirstOrDefault(m => m.id == id);
            return skillEffect == null ? null : new SkillVo { baseId = skillEffect.skillid, level = skillEffect.level };
        }

        /// <summary> 组装movesvo </summary>
        /// <param name="movesvo">movesvo</param>
        /// <param name="type">技能类型 秘技 奥义 印</param>
        private void BuildMovesVo(MovesVo movesvo, int type)
        {
            BuildMovesvoRole(movesvo);
            BuildSkillType(movesvo, type);
            GetYinCount(movesvo);
            list_move.Add(movesvo);
            UpdateBuff(); //将上次出手武将新Buff改为旧Buff
        }

        /// <summary>组装MovesVo中的双方武将</summary>
        /// <param name="movesvo">要组装的MovesVo</param>
        private void BuildMovesvoRole(MovesVo movesvo)
        {
            movesvo.rolesA = ConvertRoleFightVoList(list_attack_role);
            movesvo.rolesB = ConvertRoleFightVoList(list_defense_role);
        }

        /// <summary> 组装触发的技能类型 如：奥义、秘技、印 </summary>
        /// <param name="movesvo">出招Vo</param>
        /// <param name="type">类型</param>
        private void BuildSkillType(MovesVo movesvo, int type)
        {
            switch (type)
            {
                case (int)SkillType.MYSTERY: { movesvo.isMystery = true; break; }
                case (int)SkillType.CHEATCODE: { movesvo.isSkill = true; break; }
                case (int)SkillType.YIN: { movesvo.isYin = true; break; }
                default: { break; }
            }
        }

        #endregion

        #endregion

        #region 组装数据

        /// <summary> 供点将用   多个Npc武将Id抽取5个 </summary>
        /// <param name="list">武将Id集合</param>
        /// <returns></returns>
        private string[] GetMatrixRid(string[] list)
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

        #region 设置NPC战斗全局数据

        /// <summary>设置NPC战斗全局数据</summary>
        private bool SetNpcFightData(FightPersonal rival)
        {
            bool flag = false;
            var base_1 = rival.matrix1_rid == 0 ? null : GetNpcData(rival.matrix1_rid);
            if (base_1 != null) { list_role_hp.Add(base_1); flag = true; }
            var base_2 = rival.matrix2_rid == 0 ? null : GetNpcData(rival.matrix2_rid);
            if (base_2 != null) { list_role_hp.Add(base_2); flag = true; }
            var base_3 = rival.matrix3_rid == 0 ? null : GetNpcData(rival.matrix3_rid);
            if (base_3 != null) { list_role_hp.Add(base_3); flag = true; }
            var base_4 = rival.matrix4_rid == 0 ? null : GetNpcData(rival.matrix4_rid);
            if (base_4 != null) { list_role_hp.Add(base_4); flag = true; }
            var base_5 = rival.matrix5_rid == 0 ? null : GetNpcData(rival.matrix5_rid);
            if (base_5 != null) { list_role_hp.Add(base_5); flag = true; }
            return flag;
        }

        /// <summary> 获取NPC FightRole实体 </summary>
        /// <param name="rid">要获取的NPC 实体</param>
        /// <returns>FightRole</returns>
        private FightRole GetNpcData(Int64 rid)
        {
            var model = Variable.BASE_NPCROLE.FirstOrDefault(m => m.id == Convert.ToInt32(rid));
            if (model == null) return null;
            var role = ConvertNpcRoleFight(model);
            role.monsterType = (int)FightRivalType.MONSTER;
            return role;
        }

        #endregion


        /// <summary>设置玩家战斗全局数据</summary>
        private void SetPlayerFightData(List<Int64> ids)
        {
            var list = tg_role.GetFindAllByIds(ids);
            GetPlayerRoleSkill(list);
            list_role_hp.AddRange(ConvertRoleFightList(list));
            list_role_hp.ForEach(item => list_role.Add(item.CloneEntity()));
        }

        #endregion


        /// <summary>得到胜负结果</summary>
        private bool GetWinResult(decimal userid)
        {
            return (WIN == 0 && vo.attackId == Convert.ToDouble(userid)) || (WIN == 1 && vo.attackId != Convert.ToDouble(userid));
        }

        #region 套卡

        /// <summary>检索卡组  套卡</summary>
        private void SearchCardGroup()
        {
            var u1 = list_attack_role.FirstOrDefault(m => m != null);
            var u2 = list_defense_role.FirstOrDefault(m => m != null);
            if (u1 == null || u2 == null) return;
            if (u1.user_id != 0)
            {
                IsAttack = true;
                CardGroup(list_attack_role.Where(m => m != null).ToList());
            }
            if (u2.user_id == 0) return;
            IsAttack = false;
            CardGroup(list_defense_role.Where(m => m != null).ToList());
        }

        /// <summary> 套卡 </summary>
        private void CardGroup(List<FightRole> rolelist)
        {
            var list = new List<string>();

            foreach (var item in Variable.BASE_ROLEGROUP)
            {
                if (item.role1 != 0)
                {
                    if (rolelist.Count(m => m.baseId == item.role1) == 0) continue;
                }
                if (item.role2 != 0)
                {
                    if (rolelist.Count(m => m.baseId == item.role2) == 0) continue;
                }
                if (item.role3 != 0)
                {
                    if (rolelist.Count(m => m.baseId == item.role3) == 0) continue;
                }
                if (item.role4 != 0)
                {
                    if (rolelist.Count(m => m.baseId == item.role4) == 0) continue;
                }
                list.Add(item.effects);
            }

            if (!list.Any()) return;
            SkillEffect(list.Count() == 1 ? list.First() : GetNewString(list[0], list[1]), (int)SkillType.CARD);
        }

        /// <summary> 将2个技能效果组合成新的技能效果  同类型取最大值那条 </summary>
        private string GetNewString(string e1, string e2)
        {
            var effects1 = GetSkill(e1.Split('|'));
            var effects2 = GetSkill(e2.Split('|'));
            var types1 = effects1.Select(m => m.type).ToList();
            var types2 = effects2.Select(m => m.type).ToList();
            var types = types1.Where(types2.Contains).ToList(); //交集
            var effects3 = new List<FightSkillEffects>();
            foreach (var item in types)
            {
                var m1 = effects1.FirstOrDefault(m => m.type == item);
                var m2 = effects2.FirstOrDefault(m => m.type == item);
                if (m1 == null || m2 == null) continue;
                effects3.Add(m1.values < m2.values ? m2 : m1);
            }
            effects3.AddRange(types.Aggregate(effects1, (current, item) => current.Where(m => m.type != item).ToList()));
            effects3.AddRange(types.Aggregate(effects2, (current, item) => current.Where(m => m.type != item).ToList()));
            var strs = effects3.Select(item => item.type + "_" + item.target + "_" + item.range + "_" + item.round + "_" + item.values + "_" + item.robabilityValues).ToList();
            return string.Join("|", strs);
        }

        private List<FightSkillEffects> GetSkill(string[] list)
        {
            var ls = new List<FightSkillEffects>();
            foreach (var item in list[0].Split('|'))
            {
                var model = item.Split('_');
                if (model.Count() < 5) continue;
                ls.Add(new FightSkillEffects
                {
                    type = Convert.ToInt32(model[0]),
                    target = Convert.ToInt32(model[1]),
                    range = Convert.ToInt32(model[2]),
                    round = Convert.ToInt32(model[3]),
                    values = Convert.ToInt32(model[4]),
                    robabilityValues = model.Count() == 5 ? 100 : Convert.ToDouble(model[5])
                });
            }
            return ls;
        }

        #endregion

        /// <summary> 战斗推送协议 </summary>
        /// <param name="userid">userid</param>
        /// <param name="model">战斗Vo</param>
        public void SendProtocol(Int64 userid, FightVo model)
        {
            var s = Variable.OnlinePlayer.ContainsKey(userid);
            if (!s) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;

            var dic = new Dictionary<string, object> { { "result", (int)ResultType.SUCCESS }, { "fight", model } };
            var aso = new ASObject(dic);
            var pv = session.InitProtocol((int)ModuleNumber.FIGHT, (int)FightCommand.FIGHT_PERSONAL_ENTER, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }
    }
}
