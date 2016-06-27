using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 装备铸魂解锁
    /// </summary>
    public class EQUIP_SPIRIT_LOCK : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(Int64 user_id, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "EQUIP_SPIRIT_LOCK", "装备铸魂解锁");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_OFFLINE_ERROR);
            var equipid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);
            var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);
            var location = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "location").Value);
            return SpiritLockLogic(equipid, type, location, session);
        }
        /// <summary> 装备铸魂解锁</summary>
        public ASObject SpiritLockLogic(Int64 equipid, int type, int location, TGGSession session)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "EQUIP_SPIRIT_LOCK", "装备铸魂解锁");
#endif
                var user = session.Player.User.CloneEntity();
                var equipinfo = tg_bag.GetEntityById(equipid);

                if (equipinfo == null) return CommonHelper.ErrorResult((int)ResultType.EQUIP_UNKNOW);
                if (!(new Equip()).IsContainAttritute(type, equipinfo)) return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);

                var level = (new Equip()).GetEquipLevel(location, equipinfo);
                var base_equip = Variable.BASE_EQUIP.FirstOrDefault(m => m.id == equipinfo.base_id);
                if (base_equip == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);
                var base_spirit = Variable.BASE_SPIRIT.FirstOrDefault(m => m.userLv == base_equip.useLevel && m.lv == level);
                if (base_spirit == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);
                var next_spirit = Variable.BASE_SPIRIT.FirstOrDefault(m => m.id == base_spirit.updateId);
                if (next_spirit == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);

                var gold = user.gold;
                if (user.gold < next_spirit.gold) return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_GOLD_ERROR);

                equipinfo = EquipUnLock(type, equipinfo, location);
                user.gold = user.gold - next_spirit.gold;
                //日志
                var logdata = string.Format("{0}_{1}_{2}_{3}", "Gold", gold, next_spirit.gold, user.gold);
                (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.EQUIP, (int)EquipCommand.EQUIP_SPIRIT_LOCK, logdata);

                if (tg_bag.GetEquipUpdate(equipinfo) <= 0)  //更新装备
                    return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
                if (!tg_user.GetUserUpdate(user))   //更新用户
                    return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
                RewardsToUser(session, user); //推送用户消耗
                return new ASObject(BuilDataSpiritLock((int)ResultType.SUCCESS, equipinfo));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>
        /// 解锁处理
        /// </summary>
        /// <param name="type">装备属性类型</param>
        /// <param name="equipinfo">装备信息</param>
        /// <param name="user">玩家信息</param>
        /// <returns>更新的装备信息</returns>
        private tg_bag EquipUnLock(int type, tg_bag equipinfo, int location)
        {
            switch (location)
            {
                case (int)EquipPositionType.ATT1_LOCATION:
                    if (type == equipinfo.attribute1_type)
                    {
                        equipinfo.attribute1_spirit_lock = (int)SpiritLockType.UNLOCK;//解锁
                    }
                    break;
                case (int)EquipPositionType.ATT2_LOCATION:
                    if (type == equipinfo.attribute2_type)
                    {
                        equipinfo.attribute2_spirit_lock = (int)SpiritLockType.UNLOCK;
                    }
                    break;
                case (int)EquipPositionType.ATT3_LOCATION:
                    if (type == equipinfo.attribute3_type)
                    {
                        equipinfo.attribute3_spirit_lock = (int)SpiritLockType.UNLOCK;
                    }
                    break;
            }
            return equipinfo;
        }


        /// <summary> 向用户推送更新 </summary>
        private void RewardsToUser(TGGSession session, tg_user user)
        {
            session.Player.User = user;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);
        }

        private ASObject Error(int error)
        {
            return new ASObject(BuilDataSpiritLock(error, null));
        }

        /// <summary>数据组装 </summary>
        public Dictionary<String, Object> BuilDataSpiritLock(int result, dynamic equip)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"equip", equip != null ? EntityToVo.ToEquipVo(equip) : null}
            };
            return dic;
        }
    }
}
