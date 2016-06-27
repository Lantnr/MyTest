using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;
using TGG.SocketServer;

namespace TGG.Share.Fight
{
    public partial class Fight : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private delegate Core.Entity.Fight AddHandler(Int64 userid, Int64 rivalid, FightType type, Int64 hp = 0, bool of = false, bool po = false, bool or = false, bool pr = false, int rolehomeid = 0);

        public Core.Entity.Fight GeFight(Int64 userid, Int64 rivalid, FightType type, Int64 hp = 0, bool of = false, bool po = false, bool or = false, bool pr = false, int rolehomeid = 0)
        {
            AddHandler handler = new AddHandler(GeFight1);
            IAsyncResult result = handler.BeginInvoke(userid, rivalid, type, hp, of, po, or, pr, rolehomeid, null, null);
            AddHandler handler1 = (AddHandler)((AsyncResult)result).AsyncDelegate;
            return handler1.EndInvoke(result);
        }

        /// <summary> 获取战斗实体 </summary>
        /// <param name="userid">当前玩家id</param>
        /// <param name="rivalid">对手id</param>
        /// <param name="type">战斗类型</param>
        /// <param name="hp">要调控血量 (爬塔、活动、连续战斗调用)</param>
        /// <param name="of">是否获取己方战斗Vo</param>
        /// <param name="po">是否推送己方战斗</param>
        /// <param name="or">是否获取对方战斗Vo</param>
        /// <param name="pr">是否推送对方战斗</param>
        /// <param name="rolehomeid">(武将宅类型时可用)要挑战武将宅id</param>
        /// <returns></returns>
        private Core.Entity.Fight GeFight1(Int64 userid, Int64 rivalid, FightType type, Int64 hp = 0, bool of = false, bool po = false, bool or = false, bool pr = false, int rolehomeid = 0)
        {
            var fight = new Core.Entity.Fight();
            if (!Variable.OnlinePlayer.ContainsKey(userid)) { fight.Result = ResultType.BASE_PLAYER_OFFLINE_ERROR; return fight; }
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            var roleid = session.Player.Role.Kind.id;
            RoleHomeId = rolehomeid;

            fight.Result = GetResult(session, rivalid, Convert.ToInt32(type), hp);
            if (fight.Result != ResultType.SUCCESS) return fight;

            fight.PlayHp = GetPlayHp(roleid);
            fight.Hurt = GetBossHurt(userid);
            fight.BoosHp = GetBossHp(userid);
            fight.ShotCount = vo.moves.Sum(m => m.Count());
            UpdateWin(fight.PlayHp, roleid);
            fight.Iswin = vo.isWin;

            if (of) fight.Ofight = vo;
            if (po) SendProtocol(session, new ASObject(BuildData(Convert.ToInt32(fight.Result), vo)));
            if (or)
            {
                fight.Rfight = GetRivalFightVo();
                fight.Rfight.wuJiangName = session.Player.User.player_name;
            }
            if (!pr) return fight;
            if (type != FightType.BOTH_SIDES || type != FightType.ONE_SIDE) return fight;
            var s = Variable.OnlinePlayer[rivalid] as TGGSession;
            SendProtocol(s, new ASObject(BuildData(Convert.ToInt32(fight.Result), GetRivalFightVo())));
            return fight;
        }

        /// <summary>
        /// 爬塔用 人跟怪物打 主角死了必须输
        /// </summary>
        /// <param name="playhp">主角血量</param>
        private void UpdateWin(Int64 playhp, Int64 roleid)
        {
            switch (RivalFightType)
            {
                case (int)FightType.DUPLICATE_SHARP:
                    {
                        if (playhp <= 0)
                        {
                            bool flag = false;
                            vo.isWin = false;
                            var list = new List<List<MovesVo>>();
                            foreach (var l in vo.moves)
                            {
                                if (flag) continue;
                                var model = new List<MovesVo>();
                                foreach (var item in l)
                                {
                                    var role = item.rolesA.FirstOrDefault(m => m != null && m.id == roleid);
                                    if (role == null) { role = item.rolesB.FirstOrDefault(m => m != null && m.id == roleid); if (role == null)return; }
                                    if (flag) continue;
                                    if (role.hp <= 0) flag = true;
                                    model.Add(item);
                                }
                                list.Add(model);
                            }
                            vo.moves = list;
                        }
                        break;
                    }
            }
        }

        /// <summary> 得到战斗执行结果 </summary>
        /// <param name="session">session</param>
        /// <param name="rivalid">对手Id</param>
        /// <param name="type"> 枚举FightType中的类型 </param>
        /// <param name="hp">血量</param>
        /// <returns>返回NPC执行战斗结果</returns>
        private ResultType GetResult(TGGSession session, Int64 rivalid, int type, Int64 hp)
        {
            if (rivalid == 0) return ResultType.FIGHT_RIVAL_ID_ERROR;
            var userid = session.Player.User.id;
            if (IsFight(userid)) return ResultType.FIGHT_FIGHT_IN; //检验玩家是否在战斗计算中

            var roleid = session.Player.Role.Kind.id;
            if (session.Fight.Personal.id == 0) session.Fight.Personal =
   tg_fight_personal.PersonalInsert(userid, roleid); //验证己方阵形 没有就插入阵
            var personal = session.Fight.Personal;
            var vocation = session.Player.User.player_vocation;
            RivalFightType = type;

            try
            {
                var result = FightMethod(personal, roleid, vocation, rivalid, hp);  //战斗执行结果
                if (result != ResultType.SUCCESS)
                    XTrace.WriteLine("---- 战斗发生{0}错误  己方用户Id:{1}  对手Id:{2} ----", (int)result, userid, rivalid);
                else
                    if (vo.isWin) WeaponCount(userid);                  //调用武器使用统计
                RemoveFightState(userid);                               //玩家移除战斗队列
                return result;
            }
            catch (Exception ex)
            {
                Init();
                RemoveFightState(userid);                   //玩家移除战斗队列
                return ResultType.FIGHT_ERROR;
            }
        }

        #region 私有方法

        /// <summary> 推送协议 </summary>
        private void SendProtocol(TGGSession session, ASObject aso)
        {
            var pv = session.InitProtocol((int)ModuleNumber.FIGHT, (int)FightCommand.FIGHT_PERSONAL_ENTER, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary> 获取对方战斗Vo </summary>
        private FightVo GetRivalFightVo()
        {
            var fightvo = vo.CloneEntity();
            fightvo.isWin = !fightvo.isWin;
            return fightvo;
        }

        /// <summary> 获取当前玩家剩余血量 </summary>
        private Int64 GetPlayHp(Int64 roleid)
        {
            var fightrole = list_role.FirstOrDefault(m => m.id == roleid);
            if (fightrole == null) return 0;
            return fightrole.hp;
        }

        /// <summary> 获取对Boss的伤害值 </summary>
        private Int64 GetBossHurt(Int64 userid)
        {
            var fightrole = list_role.FirstOrDefault(m => m != null && m.user_id != userid);
            var _fightrole = list_role_hp.FirstOrDefault(m => m != null && m.user_id != userid);
            if (fightrole == null || _fightrole == null) return 0;

            if (fightrole.hp <= 0) return _fightrole.hp;
            return _fightrole.hp - fightrole.hp;
        }

        /// <summary> 获取Boss剩余血量 </summary>
        private Int64 GetBossHp(Int64 userid)
        {
            var fightrole = list_role.FirstOrDefault(m => m != null && m.user_id != userid);
            return fightrole == null ? 0 : fightrole.hp;
        }

        #endregion
    }
}
