using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using FluorineFx.Messaging.Rtmp.SO;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Module.Task.Service
{
    /// <summary>
    /// 职业任务购买刷新次数
    /// </summary>
    public class TASK_VOCATION_BUY
    {
        private static TASK_VOCATION_BUY _objInstance;

        public static TASK_VOCATION_BUY GetInstance()
        {
            return _objInstance ?? (_objInstance = new TASK_VOCATION_BUY());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_GOLD, session, data);
        }

        public ASObject CommandStart(int goodstype, TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}职业任务购买--{1}", session.Player.User.player_name, "TASK_VOCATION_BUY");
#endif
            const int result = (int)ResultType.SUCCESS;

            var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value);
            if (type != 1 && type != 2) return Common.getInstance().BuildVoctionData((int)ResultType.FRONT_DATA_ERROR, null, 0);
            var userid = session.Player.User.id;
            var spe = tg_task.GetSpecialVocTask(userid);
            var voc = session.Player.User.player_vocation;
            var _identify = session.Player.Role.Kind.role_identity;
            var user = session.Player.User.CloneEntity();
            var userextend = session.Player.UserExtend.CloneEntity();
            var identify = GetReflashIdentify(_identify, voc, type, userextend.task_vocation_isgo);
            if (type == 2)
            {
                if (userextend.task_vocation_isgo != 1)
                    return Common.getInstance().BuildVoctionData((int)ResultType.TASK_VOCATION_ISRESET, null, 0);
            }
            if (type == 1)
            {
                if (userextend.task_vocation_refresh < 0 || userextend.task_vocation_refresh > 1)
                    return Common.getInstance().BuildVoctionData((int)ResultType.TASK_VOCATION_COUNTERROR, null, 0);
            }
            if (!CheckIdentify(voc, identify))
                return Common.getInstance().BuildVoctionData((int)ResultType.TASK_VOCATION_NONEW, null, 0); ;//大名没有职业任务
            var newtasks = Common.getInstance().GetNewVocationTasks(identify, userid, voc);
            if (!newtasks.Any()) return Common.getInstance().BuildVoctionData((int)ResultType.TASK_VOCATION_NOREFLASH, null, 0);

            if (type == 1 && !CheckMoney(user, userextend.task_vocation_refresh, session))//金钱验证
            {
                return Common.getInstance().BuildVoctionData(userextend.task_vocation_refresh == 0 ? (int)ResultType.BASE_PLAYER_COIN_ERROR : (int)ResultType.BASE_PLAYER_GOLD_ERROR, null, 0);
            }
            tg_task.GetVocationTaskDel(userid, 0);
            tg_task.GetListInsert(newtasks);
            if (spe != null) newtasks.AddRange(spe);
            GetReflashUpdate(type, userextend, session);

            var count = (2 - userextend.task_vocation_refresh);
            return Common.getInstance().BuildVoctionData(result, newtasks, count);
        }



        /// <summary> 金钱消耗验证 </summary>
        private bool CheckMoney(tg_user user, int count, TGGSession session)
        {
            var identify = session.Player.Role.Kind.role_identity;
            var baserule = Variable.BASE_IDENTITY.FirstOrDefault(q => q.id == identify);
            if (baserule == null) return false;
            if (count == 0) //第一次刷新
            {
                var costcoin = baserule.coin;
                if (user.coin < baserule.coin) return false;
                user.coin -= costcoin;
                user.Update();
                session.Player.User.coin = user.coin;
                (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, session.Player.User);
                return true;
            }
            if (count != 1) return false;
            var costgold = baserule.gold;
            if (user.gold < costgold) return false;
            user.gold -= costgold;
            user.Update();
            session.Player.User.gold = user.gold;
            log.GoldInsertLog(costgold, user.id, (int)ModuleNumber.TASK, (int)TaskCommand.TASK_VOCATION_REFRESH);//金币消费记录
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);
            return true;
        }

        /// <summary>
        /// 验证玩家的身份
        /// </summary>
        private bool CheckIdentify(int vacation, int identify)
        {
            var baseTaskVocation = Variable.BASE_TASKVOCATION.LastOrDefault(q => q.vocation == vacation);
            return baseTaskVocation == null || identify != baseTaskVocation.id;
        }

        /// <summary>
        /// 获取玩家刷新时的身份
        /// </summary>
        /// <param name="voc"></param>
        /// <param name="type">刷新类型</param>
        /// <param name="isold">是否是老的身份</param>
        /// <param name="identify"></param>
        /// <returns></returns>
        private Int32 GetReflashIdentify(int identify, int voc, int type, int isold)
        {
            if (type != 1 || isold != 1) return identify;
            var firstidentify = Variable.BASE_IDENTITY.FirstOrDefault(q => q.vocation == voc);
            if (firstidentify != null && identify == firstidentify.id)
                return identify;
            return identify - 1;
        }

        /// <summary>
        /// 用户刷新次数更新
        /// </summary>
        /// <param name="type">刷新类型</param>
        /// <param name="userextend">用户扩展信息</param>
        /// <param name="session"></param>
        private void GetReflashUpdate(int type, tg_user_extend userextend, TGGSession session)
        {
            switch (type)
            {
                case 1:
                    userextend.task_vocation_refresh++;
                    break;
                case 2:
                    userextend.task_vocation_refresh = 0;
                    userextend.task_vocation_isgo = 0;
                    break;
            }
            userextend.Update();//更新用户和用户扩展
            session.Player.UserExtend = userextend;
        }
    }
}
