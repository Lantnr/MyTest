using System.Diagnostics.Eventing.Reader;
using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Chat.Service
{
    /// <summary>
    /// 获取物品指令
    /// </summary>
    public class CHATS_EXTEND
    {
        private static CHATS_EXTEND ObjInstance;

        /// <summary>CHATS_EXTEND单体模式</summary>
        public static CHATS_EXTEND GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new CHATS_EXTEND());
        }

        /// <summary>推送协议</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
                var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value);
                var id = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "id").Value);
                //var type = (int)GoodsType.TYPE_Power;
                //var id = 7010012;
                var count = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "count").Value);
                return count < 0 ? BuildData((int)ResultType.FAIL) : HandleRequest(type, id, count, session);
            }
            catch { return BuildData((int)ResultType.FAIL); }

        }

        private ASObject HandleRequest(int type, int id, int count, TGGSession session)
        {
#if DEBUG
            XTrace.WriteLine("type:{0}  id:{1} count:{2}", type, id, count);
#endif
            if (!CommonHelper.IsGM()) return BuildData((int)ResultType.SUCCESS);

            switch (type)
            {
                case (int)GoodsType.TYPE_GOLD: return UpdateUserSingle(session, type, count, "gold");
                case (int)GoodsType.TYPE_COIN: return UpdateUserSingle(session, type, count, "coin");
                case (int)GoodsType.TYPE_SPIRIT: return UpdateUserSingle(session, type, count, "spirit");
                case (int)GoodsType.TYPE_FAME: return UpdateUserSingle(session, type, count, "fame");

                case (int)GoodsType.TYPE_PROP: return UpdateProp(id, count, session);
                case (int)GoodsType.TYPE_EQUIP: return UpdateEquip(id, count, session);
                case (int)GoodsType.TYPE_HONOR: return UpdateRoleSingle(session, id, count, "honor");
                case (int)GoodsType.TYPE_POWER: return UpdateRoleSingle(session, id, count, "power");
                case (int)GoodsType.TYPE_EXP: return UpdateRoleSingleExp(session, id, count);
                case (int)GoodsType.TYPE_LEVEL: return GetRoleSingle(session, id);
                //case (int)GoodsType.TYPE_RMB: return GetEmail(session, count);
                case (int)GoodsType.TYPE_RMB: return GetVip(session, count);

            }
            return BuildData((int)ResultType.SUCCESS);
        }

        private ASObject GetVip(TGGSession session, int level)
        {
            var base_vip = Variable.BASE_VIP.FirstOrDefault(m => m.level == level);
            if (base_vip == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            var player = session.Player.CloneEntity();

            player.Vip.vip_gold = base_vip.gold;
            player.Vip.power = base_vip.power;
            player.Vip.bargain = base_vip.bargain;
            player.Vip.buy = base_vip.buy;
            player.Vip.arena_buy = 0;
            player.Vip.arena_cd = 0;
            player.Vip.train_home = base_vip.trainHome;
            player.Vip.vip_level = base_vip.level;
            player.Vip.fight = base_vip.fight;
            player.Vip.car = base_vip.car;

            player.Vip.Save();

            var gold = tg_user.IsGoldMax(player.User.gold, base_vip.gold);
            player.User.gold = gold;
            player.User.Save();
            session.Player = player;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, player.User);
            //UserUpdate(session);
            dynamic obje_user = CommonHelper.ReflectionMethods("TGG.Module.User", "USER_VIP_PUSH");
            obje_user.CommandStart(session.Player.User.id);

            return BuildData((int)ResultType.SUCCESS);

        }

        #region

        private void UserUpdate(TGGSession session)
        {
            var data = new ASObject(new Dictionary<string, object>
            {
                {"result",  (int)ResultType.SUCCESS},
                {"userInfoVo", AMFConvert.ToASObject(EntityToVo.ToUserInfoVo(session.Player))}
            });

            Push(session, data, (int)ModuleNumber.USER, (int)UserCommand.USER_LOGIN);
        }

        public void Push(TGGSession session, ASObject data, int mn, int cn)
        {
            var pv = new ProtocolVo
            {
                serialNumber = 1,
                verificationCode = 1,
                moduleNumber = mn,//(int)ModuleNumber.USER,
                commandNumber = cn,
                sendTime = 1000,
                serverTime = (DateTime.Now.Ticks - 621355968000000000) / 10000,
                status = (int)ResponseType.TYPE_SUCCESS,
                data = data,
            };
            session.SendData(pv);
        }
        #endregion

        private ASObject GetEmail(TGGSession session, int isatt)
        {
            var entity = new tg_messages
            {
                receive_id = session.Player.User.id,
                send_id = 0,
                type = 1,
                title = "测试邮件",
                isattachment = isatt,
                attachment = "6_4010005_0_99|7_6010001_0_1",
                contents = "测试邮件内容",
                create_time = (DateTime.Now.Ticks - 621355968000000000) / 10000,

            };
            entity.Save();

            (new Share.Message()).UnMessage(session.Player.User.id);
            return BuildData((int)ResultType.SUCCESS);
        }

        private ASObject GetRoleSingle(TGGSession session, int rid)
        {
            if (rid == 0) rid = session.Player.Role.Kind.role_id;
            var _role = tg_role.Find(new String[] { tg_role._.role_id, tg_role._.user_id }, new Object[] { rid, session.Player.User.id });
            if (_role != null) return BuildData((int)ResultType.FAIL);
            var base_role = Variable.BASE_ROLE.FirstOrDefault(m => m.id == rid);
            if (base_role == null) return BuildData((int)ResultType.ROLE_NOT_EXIST);
            var identify = Variable.BASE_IDENTITY.FirstOrDefault(m => m.vocation == (int)VocationType.Roles && m.value == 1);

            var entity = new RoleItem();

            var role = new tg_role
            {
                user_id = session.Player.User.id,
                role_id = base_role.id,
                role_level = 1,
                role_state = (int)RoleStateType.IDLE,
                base_captain = base_role.captain,
                base_force = base_role.force,
                base_brains = base_role.brains,
                base_charm = base_role.charm,
                base_govern = base_role.govern,
                power = base_role.power,
                att_life = base_role.life,
                role_identity = identify == null ? 0 : identify.id,
            };
            role.Save();
            entity.Kind = role;
            dynamic obje_user = CommonHelper.ReflectionMethods("TGG.Module.User", "Common");
            var life_skill = obje_user.InitLifeSkill(entity.Kind);
            entity.LifeSkill = life_skill;
            new Upgrade().UserLvUpdate(session.Player.User.id, 0, role);

            return BuildData((int)ResultType.SUCCESS);
        }

        private ASObject UpdateRoleSingleExp(TGGSession session, int rid, int count)
        {
            if (rid == 0) rid = session.Player.Role.Kind.role_id;
            var role = tg_role.Find(new String[] { tg_role._.role_id, tg_role._.user_id }, new Object[] { rid, session.Player.User.id });
            if (role == null) return BuildData((int)ResultType.NO_DATA);
            new Upgrade().UserLvUpdate(session.Player.User.id, count, role);
            return BuildData((int)ResultType.SUCCESS);
        }

        private ASObject UpdateRoleSingle(TGGSession session, int rid, int count, string obj)
        {
            if (rid == 0) rid = session.Player.Role.Kind.role_id;
            var role = tg_role.Find(new String[] { tg_role._.role_id, tg_role._.user_id }, new Object[] { rid, session.Player.User.id });
            if (role == null) return BuildData((int)ResultType.NO_DATA);
            var listname = new List<string>();
            switch (obj)
            {
                case "power": role.power = CheckPower(rid, session.Player.Role.Kind.role_id, role.power, count);
                    listname.Add("rolepower");
                    listname.Add("power"); break;
                //case "experience": role.role_exp += count; break;
                case "honor":
                    {
                        listname.Add("honor");
                        var vocation = 0;
                        if (session.Player.Role.Kind.id == role.id) vocation = session.Player.User.player_vocation;
                        else
                            vocation = (int)VocationType.Roles;
                        new Upgrade().UserIdentifyUpdate(session.Player.User.id, count, role, vocation);
                        return null;
                    }
                //case "identity": role.role_identity += count; break;
            }
            if (rid == session.Player.Role.Kind.role_id)   //主角才更新session
            {
                session.Player.Role.Kind = role;
            }
            role.Save();
            (new Share.RoleAttUpdate()).RoleUpdatePush(role, session.Player.User.id, listname);
            return BuildData((int)ResultType.SUCCESS);
        }

        /// <summary>验证体力上限</summary>
        private int CheckPower(int roleid, int mroleid, int power, int count)
        {
            if (count <= 0) count = 0;
            var basepower = power + count;
            if (roleid == mroleid)
            {
                var mrule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "1009"); //主角体力最大值
                if (mrule == null) return power;
                if (basepower >= Convert.ToInt32(mrule.value))
                    basepower = Convert.ToInt32(mrule.value);
            }
            else
            {
                var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7011");  //武将体力最大值
                if (rule == null) return power;
                if (basepower >= Convert.ToInt32(rule.value))
                    basepower = Convert.ToInt32(rule.value);
            }
            return basepower;
        }

        private ASObject UpdateUserSingle(TGGSession session, int type, int count, string obj)
        {
            var user = session.Player.User;
            switch (obj)
            {
                case "gold": user.gold =tg_user.IsGoldMax(user.gold ,count); break;
                case "coin": user.coin = tg_user.IsCoinMax(user.coin, count); break;
                case "fame": user.fame = tg_user.IsFameMax(user.fame, count); break;
                case "spirit": user.spirit = tg_user.IsSpiritMax(user.spirit, count); break;
            }
            user.Save();
            session.Player.User = user;
            (new Share.User()).REWARDS_API(type, session.Player.User);
            return BuildData((int)ResultType.SUCCESS);
        }

        private ASObject UpdateEquip(int id, int count, TGGSession session)
        {
            if (session.Player.Bag.BagIsFull) return BuildData((int)ResultType.BAG_ISFULL_ERROR);
            var base_equip = Variable.BASE_EQUIP.FirstOrDefault(m => m.id == (decimal)id);
            if (base_equip == null) return BuildData((int)ResultType.FRONT_DATA_ERROR);
            dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Equip", "Common");
            //dynamic ob = CommonHelper.ReflectionMethods("TGG.Module.Props", "Common");
            count = session.Player.Bag.Surplus > count ? count : session.Player.Bag.Surplus;
            for (var i = 0; i < count; i++)
            {
                tg_bag equip = obje.GetEquip(base_equip);
                equip.user_id = session.Player.User.id;
                (new Bag()).BuildReward(equip.user_id, new List<tg_bag>() { equip }); //入包整理并推送
#if DEBUG
                XTrace.WriteLine("{0} 数量:{1}", base_equip.id, i);
#endif
            }
            return BuildData((int)ResultType.SUCCESS);
        }

        private ASObject UpdateProp(int id, int count, TGGSession session)
        {
            if (session.Player.Bag.BagIsFull) return BuildData((int)ResultType.BAG_ISFULL_ERROR);
            var base_prop = Variable.BASE_PROP.FirstOrDefault(m => m.id == id);
            if (base_prop == null) return BuildData((int)ResultType.FRONT_DATA_ERROR);
            dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Props", "Common");
            while (count > 0)
            {
                var temp = 0;
                if (count > 99)
                {
                    temp = 99;
                    count = count - 99;
                }
                else
                {
                    temp = count;
                    count = 0;
                }
                var entity = new tg_bag
                {
                    base_id = base_prop.id,
                    count = temp,
                    type = (int)GoodsType.TYPE_PROP,
                    bind = base_prop.bind,
                    user_id = session.Player.User.id,
                };
                (new Bag()).BuildReward(entity.user_id, new List<tg_bag>() { entity }); //入包整理并推送
#if DEBUG
                XTrace.WriteLine("{0} 数量:{1}", base_prop.id, temp);
#endif

            }
            return BuildData((int)ResultType.SUCCESS);
        }

        private ASObject BuildData(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }
    }
}
