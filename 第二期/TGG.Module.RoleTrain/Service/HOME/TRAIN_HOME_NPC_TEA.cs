using System;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Entity;
using TGG.Core.Global;

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 茶道
    /// </summary>
    public class TRAIN_HOME_NPC_TEA
    {
        public static TRAIN_HOME_NPC_TEA ObjInstance;

        /// <summary>TRAIN_HOME_NPC_TEA单体模式</summary>
        public static TRAIN_HOME_NPC_TEA GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TRAIN_HOME_NPC_TEA());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "TRAIN_HOME_NPC_TEA", "茶道");
#endif
                var npcid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value.ToString());
                var lifeskill = session.Player.Role.LifeSkill;

                var npc = tg_train_home.GetNpcById(npcid);
                if (npc == null) return Error((int)ResultType.TRAIN_HOME_GET_ERROR);
                if (!(new Share.RoleTrain()).PowerOperate(session.Player.Role.Kind)) return Error((int)ResultType.BASE_ROLE_POWER_ERROR);   //验证武将体力信息               
                if (npc.is_steal == (int)TrainHomeStealType.STEAL_YES) return Error((int)ResultType.TRAIN_HOME_STEAL_YES);

                if (npc.npc_spirit == 0) return Error((int)ResultType.TRAIN_HOME_SPIRIT_LACK);  //验证NPC 剩余魂数

                var basenpc = Variable.BASE_NPCMONSTER.FirstOrDefault(m => m.id == npc.npc_id);  //获取武将宅基表NPC信息
                if (basenpc == null) return Error((int)ResultType.BASE_TABLE_ERROR);
                if (basenpc.limit == (int)TrainHomeLimitType.JUST_CHALLENGE) return Error((int)ResultType.TRAIN_HOME_NO_TEA);  //验证武将是否受限

                if (lifeskill.sub_tea_level < basenpc.level) return Error((int)ResultType.TRAIN_HOME_TEA_LEVEL_LACK);  //验证武将茶道等级

                return AcquireSpirit(session.Player.User.id, npc, lifeskill);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>处理喝茶获得魂信息</summary>
        private ASObject AcquireSpirit(Int64 userid, tg_train_home npc, tg_role_life_skill lifeskill)
        {
            var spirit = (new Share.RoleTrain()).GetSpirit(lifeskill.sub_tea, lifeskill.sub_tea_level);   //喝茶获得的魂数
            var sp = npc.npc_spirit;
            sp = sp - spirit;
            if (sp < 0)
            {
                spirit = npc.npc_spirit;
                npc.npc_spirit = 0;
            }
            else { npc.npc_spirit = sp; }

            return !tg_train_home.UpdateNpc(npc) ? Error((int)ResultType.DATABASE_ERROR) : TeaInfo(userid, npc.npc_spirit, spirit);
        }

        /// <summary>处理喝茶信息</summary>
        private ASObject TeaInfo(Int64 userid, int nspirit, int spirit)
        {
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return Error((int)ResultType.BASE_PLAYER_OFFLINE_ERROR);

            var user = session.Player.User.CloneEntity();
            var uspirit = user.spirit;
            user.spirit = tg_user.IsSpiritMax(user.spirit, spirit);     //玩家喝茶获得的魂
            user.Update();
            session.Player.User = user;

            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_SPIRIT, user); //推送魂更新 
            (new Share.Title()).IsTitleAcquire(userid, (int)TitleGetType.USE_TEA_TABLE);//判断称号信息

            //记录获得魂日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Spirit", uspirit, spirit, user.spirit);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Get, (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_HOME_NPC_TEA, logdata);

            (new Share.DaMing()).CheckDaMing(user.id, (int)DaMingType.武将宅喝茶);
            return new ASObject(Common.GetInstance().TeaBulidData((int)ResultType.SUCCESS, spirit, nspirit));
        }

        /// <summary>错误信息</summary>
        private ASObject Error(int error)
        {
            return new ASObject(Common.GetInstance().BulidData(error));
        }
    }
}
