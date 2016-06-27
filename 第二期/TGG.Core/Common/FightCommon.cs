//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FluorineFx;
//using NewLife.Log;
//using TGG.Core.Enum.Type;
//using TGG.Core.Global;

//namespace TGG.Core.Common
//{
//    public class FightCommon
//    {

//        /// <summary> 获取战斗实体 </summary>
//        /// <param name="session">当前玩家session</param>
//        /// <param name="rivalid">对手id</param>
//        /// <param name="type">战斗类型</param>
//        /// <param name="hp">要调控血量 (爬塔有效活动,连续战斗任务有效)</param>
//        /// <param name="of">是否获取己方战斗Vo</param>
//        /// <param name="po">是否推送己方战斗</param>
//        /// <param name="or">是否获取对方战斗Vo</param>
//        /// <param name="pr">是否推送对方战斗</param>
//        /// <param name="rolehomeid">(武将宅类型时可用)要挑战武将宅id</param>
//        /// <returns></returns>
//        public Entity.Fight GeFight(TGGSession session, Int64 rivalid, FightType type, Int64 hp = 0, bool of = false, bool po = false, bool or = false, bool pr = false, int rolehomeid = 0)
//        {
//            var roleid = session.Player.Role.Kind.id;
//            var userid = session.Player.User.id;
//            var fight = new Core.Entity.Fight();
//            fight.Result = GetResult(session, rivalid, Convert.ToInt32(type), hp);
//            if (fight.Result != ResultType.SUCCESS) return fight;

//            fight.Iswin = vo.isWin;
//            fight.PlayHp = GetPlayHp(roleid);
//            fight.Hurt = GetBossHurt(userid);
//            fight.BoosHp = GetBossHp(userid);
//            if (of) fight.Ofight = vo;
//            if (po) SendProtocol(session, new ASObject(BuildData(Convert.ToInt32(fight.Result), vo)));
//            if (or) fight.Rfight = GetRivalFightVo();
//            if (!pr) return fight;
//            if (type != FightType.BOTH_SIDES || type != FightType.ONE_SIDE) return fight;
//            var s = Variable.OnlinePlayer[rivalid] as TGGSession;
//            SendProtocol(s, new ASObject(BuildData(Convert.ToInt32(fight.Result), GetRivalFightVo())));
//            return fight;
//        }

//        /// <summary> 得到战斗执行结果 </summary>
//        /// <param name="session">session</param>
//        /// <param name="rivalid">对手Id</param>
//        /// <param name="type"> 枚举FightType中的类型 </param>
//        /// <param name="hp">血量</param>
//        /// <returns>返回NPC执行战斗结果</returns>
//        private ResultType GetResult(TGGSession session, Int64 rivalid, int type, Int64 hp)
//        {
//            if (rivalid == 0) return ResultType.FIGHT_RIVAL_ID_ERROR;
//            var userid = session.Player.User.id;
//            if (IsFight(userid)) return ResultType.FIGHT_FIGHT_IN; //检验玩家是否在战斗计算中
//            RivalFightType = Convert.ToInt32(type);
//            session.Fight.Rival = rivalid;

//            var result = FightMethod(session, hp);  //战斗执行结果
//            if (result != ResultType.SUCCESS)
//            {
//                XTrace.WriteLine("---- 战斗发生{0}错误:{1}  己方用户Id:{2}  对手Id:{3} ----", result, (int)result, session.Player.User.id, rivalid);
//                RemoveFightState(userid);   //修改玩家战斗状态
//                return result;
//            }


//            var iswin = vo.isWin ? FightResultType.WIN : FightResultType.LOSE;
//            if (vo.isWin) WeaponCount(session);  //调用武器使用统计
//            TaskResult(session, iswin);          //检索玩家任务
//            RemoveFightState(userid);            //玩家移除战斗队列
//            return result;
//        }

//        /// <summary>战斗方法</summary>
//        /// <param name="session">session</param>
//        /// <param name="hp"> 对手方要修改的血量 </param>
//        private ResultType FightMethod(TGGSession session, Int64 hp)
//        {
//            //start
//            var userid = session.Player.User.id;
//            try
//            {
//                var result = ReadyData(session, hp);                     //准备数据
//                if (result != (int)ResultType.SUCCESS) return result;    //准备数据处理结果验证
//                BuildFight(userid);                                      //开始战斗  战斗过程组装
//            }
//            catch (Exception ex)
//            {
//                Init();
//                RemoveFightState(userid);                            //玩家移除战斗队列
//                XTrace.WriteLine(string.Format("{0} {1}", "战斗爆异常----->", ex));
//                return ResultType.FIGHT_ERROR;
//            }
//            //end
//            return ResultType.SUCCESS;
//        }

//        /// <summary> 推送协议 </summary>
//        private void SendProtocol(TGGSession session, ASObject aso)
//        {
//            var pv = session.InitProtocol((int)ModuleNumber.FIGHT, (int)FightCommand.FIGHT_PERSONAL_ENTER, (int)ResponseType.TYPE_SUCCESS, aso);
//            session.SendData(pv);
//        }
//    }
//}
