using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Guide;
using TGG.SocketServer;

namespace TGG.Share
{
    /// <summary>
    /// 扩展指令共享类
    /// </summary>
    public class ExpansionCommand : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        /// <summary>当前会话</summary>
        private TGGSession Session { get; set; }

        /// <summary>指令内容</summary>
        private String[] Content { get; set; }

        /// <summary>指令类型</summary>
        private String Type { get; set; }

        /// <summary> 检测是否GM指令 </summary>
        public bool IsGmContent(string data, TGGSession session)
        {
            Session = session;
            var cmd = data.Split(' ');

            if (cmd.Length < 2) return false;
            //判断是否是GM指令 不区分大小写GM
            var gm = cmd[0].ToUpper();
            if (gm.Equals("@GET"))
            {
                var type = Type = cmd[1].ToLower();
                Content = cmd;
                switch (type)
                {
                    case "gold": { goodstype = (int)GoodsType.TYPE_GOLD; goto case "user"; }
                    case "coin": { goodstype = (int)GoodsType.TYPE_COIN; goto case "user"; }
                    case "fame": { goodstype = (int)GoodsType.TYPE_FAME; goto case "user"; }
                    case "spirit": { goodstype = (int)GoodsType.TYPE_SPIRIT; goto case "user"; }
                    case "merit": { goodstype = (int)GoodsType.TYPE_MERIT; goto case "user"; }
                    case "don": { goodstype = (int)GoodsType.TYPE_DONATE; goto case "user"; }
                    case "vip": { goto case "user"; }
                    case "office": { goto case "user"; }
                    case "copy": { goto case "user"; }
                    case "task": { goto case "user"; }
                    case "user": { return Player_Value(); }
                    case "city": { return Player_Value(); }

                    case "equip": { goodstype = (int)GoodsType.TYPE_EQUIP; goto case "bag"; }
                    case "prop": { goodstype = (int)GoodsType.TYPE_PROP; goto case "bag"; }
                    case "fusion": { goodstype = (int)GoodsType.TYPE_FUSION; goto case "bag"; }
                    case "bag": { return Player_IdValue(); }
                    case "exp": { return Player_IdValue(); }
                    case "power": { return Player_IdValue(); }
                    case "honor": { return Player_IdValue(); }
                    case "tower": { return Player_IdValue(); }

                    case "res": { goto case "resource"; }
                    case "resource": { return Player_IdValue(); }

                    case "buf": { goto case "buffpower"; }
                    case "buffpower": { return Player_IdValue(); }

                    case "notice": { return SendNotice(Content); }

                    case "msg": { goto case "message"; }
                    case "message": { return SendMessage(Content); }

                    case "cd": { return RemoveCityTime(); }
                    case "build": { return BuildCity(); }
                    case "dml": { return FinishDml(); }

                    case "scenemax":
                        {
                            var max = Convert.ToInt32(Content[2]);
                            SceneSplit.Max = max;
                            return true;
                        }


                }
                return true;
            }
            return false;

        }

        #region GM 通用指令

        /// <summary>物品类型</summary>
        private Int32 goodstype { get; set; }
        /// <summary>格式:GM|gold|value</summary>
        private bool Player_Value()
        {
            try
            {
                var number = 0;
                if (Content.Length > 2)
                    number = Convert.ToInt32(Content[2]);
                if (number < 0) return false;
                var user = Session.Player.User.CloneEntity();
                switch (Type)
                {
                    case "gold": { Session.Player.User.gold = tg_user.IsGoldMax(user.gold, number); break; }
                    case "coin": { Session.Player.User.coin = tg_user.IsCoinMax(user.coin, number); break; }
                    case "fame": { Session.Player.User.fame = tg_user.IsFameMax(user.fame, number); break; }
                    case "spirit": { Session.Player.User.spirit = tg_user.IsSpiritMax(user.spirit, number); break; }
                    case "merit": { Session.Player.User.merit = tg_user.IsMeritMax(user.merit, number); break; }
                    case "don": { Session.Player.User.donate = GetDonate(user.donate, number, user.office); break; }
                    case "vip": { return GetVip(number); }
                    case "office": { return UpdatePlayerOffice(number); }
                    case "copy": { return GetCopyCount(number); }
                    case "city": { return UpdateCity(number); }
                    case "task": { return GetMainTask(number); }
                }
                Session.Player.User.Save();
                (new User()).REWARDS_API(goodstype, Session.Player.User);
                return true;
            }
            catch (Exception ex) { XTrace.WriteException(ex); return false; }
        }

        /// <summary>格式:GM|equip|id value</summary>
        private bool Player_IdValue()
        {
            try
            {
                if (Content.Length > 3)
                {
                    var id = Convert.ToInt32(Content[2]);
                    var value = Convert.ToInt32(Content[3]);
                    if (value < 0) return false;
                    switch (Type)
                    {
                        case "prop": { return GetProp(id, value); }
                        case "equip": { return UpdateEquip(id, value); }
                        case "fusion": { return GetFusion(id, value); }
                        case "exp": { return UpdateRoleSingleExp(id, value); }
                        case "tower": { return UpdateTowerInfo(id, value); }

                        case "res": { goto case "resource"; }
                        case "resource": { return GetCityResource(id, value); }

                        case "buf": { goto case "buffpower"; }
                        case "buffpower": { return BuffPowerSendAll(value, id); }

                        case "power": { return UpdateRoleSingle(id, value, Type); }
                        case "honor": { return UpdateRoleSingle(id, value, Type); }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex) { XTrace.WriteException(ex); return false; }
        }

        /// <summary>获取道具</summary>
        /// <param name="id">基表Id</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        private bool GetProp(int id, int count)
        {
            if (Session.Player.Bag.BagIsFull) return false;
            var base_prop = Variable.BASE_PROP.FirstOrDefault(m => m.id == id);
            if (base_prop == null) return false;
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
                    user_id = Session.Player.User.id,
                };
                (new Bag()).BuildReward(entity.user_id, new List<tg_bag>() { entity }); //入包整理并推送
#if DEBUG
                XTrace.WriteLine("{0} 数量:{1}", base_prop.id, temp);
#endif
            }
            return false;
        }

        /// <summary>获取装备信息</summary>
        /// <param name="id">装备基表Id</param>
        /// <param name="count">数量</param>
        private bool UpdateEquip(int id, int count)
        {
            var player = Session.Player;

            if (player.Bag.BagIsFull) return false;
            var bequip = Variable.BASE_EQUIP.FirstOrDefault(m => m.id == (decimal)id);
            if (bequip == null) return false;

            count = player.Bag.Surplus > count ? count : player.Bag.Surplus;
            for (var i = 0; i < count; i++)
            {
                tg_bag equip = CommonHelper.EquipReset(player.User.id, bequip.id);
                equip.user_id = player.User.id;
                (new Bag()).BuildReward(equip.user_id, new List<tg_bag>() { equip }); //入包整理并推送
#if DEBUG
                XTrace.WriteLine("{0} 数量:{1}", bequip.id, i);
#endif
            }
            return true;
        }

        /// <summary>获取熔炼道具</summary>
        /// <param name="id">基表Id</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        private bool GetFusion(int id, int count)
        {
            if (Session.Player.Bag.BagIsFull) return false;
            var bfusion = Variable.BASE_FUSION.FirstOrDefault(m => m.id == id);
            if (bfusion == null) return false;
            while (count > 0)
            {
                int temp;
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
                    base_id = bfusion.id,
                    count = temp,
                    type = (int)GoodsType.TYPE_FUSION,
                    user_id = Session.Player.User.id,
                };
                (new Bag()).BuildReward(entity.user_id, new List<tg_bag> { entity });   //入包整理并推送
#if DEBUG
                XTrace.WriteLine("{0} 数量:{1}", bfusion.id, temp);
#endif
            }
            return false;
        }

        #endregion

        #region OLD
        /// <summary> 发送邮件 </summary>
        private bool SendMessage(string[] data)
        {
            if (data.Count() < 5) return false;
            var where = data[2];
            var title = data[3];
            var contents = data[4];
            var attachment = " ";
            if (data.Count() == 5)
            {
                attachment = "";
            }
            if (data.Count() == 6)
            {
                attachment = data[5];
            }
            (new Message()).MessagesSendAll(where, title, contents, attachment);
            return true;
        }

        /// <summary> 发送公告 </summary>
        /// <param name="data">data</param>
        private bool SendNotice(string[] data)
        {
            if (data.Count() < 3) return false;
            var content = "";
            var id = Convert.ToInt32(data[2]);
            if (data.Count() == 3)
            {
                content = "";
            }
            if (data.Count() == 4)
            {
                content = data[3];
            }
            if (data.Count() == 4) content = data[3];
            new Notice().TrainingPlayer(id, content);
            return true;
        }

        /// <summary>更改官职信息</summary>
        /// <param name="office">新官职基表Id</param>
        private bool UpdatePlayerOffice(int office)
        {
            var user = Session.Player.User.CloneEntity();
            user.office = office;
            user.Save();
            Session.Player.User = user;
            return true;
        }

        /// <summary>增加城市多种资源</summary>
        /// <param name="count">值</param>
        private bool UpdateCity(int count)
        {
            var user = Session.Player.User;
            var list = Variable.WarCityAll.Values.Where(m => m.user_id == user.id);
            var maxcity = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == (int)WarCitySizeType.FIVE);
            if (maxcity == null) return false;

            foreach (var item in list)
            {
                var basecity = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == item.size);
                if (basecity == null) continue;

                item.boom += count;
                item.boom = item.boom > maxcity.boom ? maxcity.boom : item.boom;
                item.peace += count;
                item.peace = item.peace > maxcity.peace ? maxcity.peace : item.peace;
                item.strong += count;
                item.strong = item.strong > maxcity.strong ? maxcity.strong : item.strong;

                item.res_foods += count;
                item.res_foods = item.res_foods > basecity.foods ? basecity.foods : item.res_foods;
                item.res_funds += count;
                item.res_funds = item.res_funds > basecity.funds ? basecity.funds : item.res_funds;
                item.res_soldier += count;
                item.res_soldier = item.res_soldier > basecity.soldier ? basecity.soldier : item.res_soldier;

                item.res_gun = basecity.goods / 4;
                item.res_kuwu = basecity.goods / 4;
                item.res_razor = basecity.goods / 4;
                item.res_horse = basecity.goods / 4;

                item.res_morale += count;
                item.res_morale = item.res_morale > basecity.morale ? basecity.morale : item.res_morale;

                item.Update();
                (new War()).SaveWarCityAll(item);
                (new War()).SendCity(item.base_id, item.user_id);
            }
            return true;
        }

        #region 获取据点资源方法

        /// <summary>获取据点资源方法</summary>
        /// <param name="type">类型</param>
        /// <param name="count">数量</param>
        private bool GetCityResource(int type, int count)
        {
            if (!CheckType(type)) return false;   //验证类型

            var player = Session.Player;
            if (count <= 0) return false;
            var cityId = player.War.PlayerInCityId;

            var identity = player.Role.Kind.role_identity;
            var rule = Variable.BASE_IDENTITY.FirstOrDefault(m => m.id == identity);
            if (rule == null) return false;
            if (rule.value < 7) return false;

            if (cityId != 0)   //只更新当前据点资源
            {
                var city = tg_war_city.GetCityByBidUserId(player.War.PlayerInCityId, player.User.id);
                if (city == null) return false;

                city = MaxNumber(type, count, city);
                city.Update();
                ResourceData(city);       //处理当前据点资源信息
            }
            else
            {
                //更新用户所有据点资源
                var citys = tg_war_city.GetEntityByUserId(player.User.id);
                if (!citys.Any()) return false;

                foreach (var ncity in citys.Select(item => MaxNumber(type, count, item)))
                {
                    ncity.Update();
                    ResourceData(ncity);
                }
            }
            return true;
        }

        /// <summary>更新当前据点资源信息</summary>
        private tg_war_city MaxNumber(int type, int count, tg_war_city city)
        {
            var info = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == city.size);
            if (info == null) return city;
            switch (type)
            {
                case (int)WarResourseType.兵粮:
                    city.res_foods += count;
                    if (city.res_foods > info.foods) { city.res_foods = info.foods; }
                    break;
                case (int)WarResourseType.军资金:
                    city.res_funds += count;
                    if (city.res_funds > info.funds) { city.res_funds = info.funds; }
                    break;
                case (int)WarResourseType.足轻:
                    city.res_soldier += count;
                    if (city.res_soldier > info.soldier) { city.res_soldier = info.soldier; }
                    break;
            }
            city = CheckMax(type, info.goods, count, city);
            return city;
        }

        /// <summary>验证最大值</summary>     
        private tg_war_city CheckMax(int type, int total, int count, tg_war_city city)
        {
            int surplus;
            switch (type)
            {
                case (int)WarResourseType.马匹:
                    surplus = total - city.res_kuwu - city.res_razor - city.res_gun;
                    city.res_horse += count;
                    city.res_horse = city.res_horse >= surplus ? surplus : city.res_horse;
                    break;
                case (int)WarResourseType.铁炮:
                    surplus = total - city.res_kuwu - city.res_razor - city.res_horse;
                    city.res_gun += count;
                    city.res_gun = city.res_gun >= surplus ? surplus : city.res_gun;
                    break;
                case (int)WarResourseType.薙刀:
                    surplus = total - city.res_kuwu - city.res_horse - city.res_gun;
                    city.res_razor += count;
                    city.res_razor = city.res_razor >= surplus ? surplus : city.res_razor;
                    break;
                case (int)WarResourseType.苦无:
                    surplus = total - city.res_horse - city.res_razor - city.res_gun;
                    city.res_kuwu += count;
                    city.res_kuwu = city.res_kuwu >= surplus ? surplus : city.res_kuwu;
                    break;
            }
            return city;
        }

        /// <summary>验证资源类型</summary>
        private bool CheckType(int type)
        {
            return type == (int)WarResourseType.兵粮 || type == (int)WarResourseType.军资金 || type == (int)WarResourseType.苦无 ||
                type == (int)WarResourseType.薙刀 || type == (int)WarResourseType.足轻 ||
                type == (int)WarResourseType.铁炮 || type == (int)WarResourseType.马匹;
        }

        /// <summary>资源更新后处理</summary>
        private void ResourceData(tg_war_city city)
        {
            (new War()).SaveWarCityAll(city);   //更新全局据点信息
            (new War()).SendCity(city.base_id, city.user_id);   //推送据点数据更新  
        }

        #endregion

        #region 发送玩家buff体力

        /// <summary> 发送玩家Buff体力 </summary>
        /// <param name="type">要发送的目标</param>
        /// <param name="values">要加的体力值</param>
        public bool BuffPowerSendAll(int type, int values)
        {
            switch (type)
            {
                case 0: { SendOnlinePlayer(values); return true; }    //在线玩家
                case 1: { SendAllLeadRole(values); return true; }      //所有玩家
                default: { SendLeadRole(values, Convert.ToInt64(type)); return true; }  //指定玩家Id
            }
        }

        /// <summary> 更新推送在线玩家主角武将体力 </summary>
        /// <param name="values">体力值</param>
        private void SendOnlinePlayer(int values)
        {
            var list = Variable.OnlinePlayer.Keys.ToList();
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(n =>
                {
                    var userid = Convert.ToInt64(n);
                    if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
                    var session = Variable.OnlinePlayer[userid] as TGGSession;
                    if (session == null) return;
                    var role = session.Player.Role.Kind;
                    if (role == null) return;
                    var power = role.buff_power + values;
                    if (power > 50) power = 50;
                    role.buff_power = power;
                    role.Update();
                    new RoleAttUpdate().RoleUpdatePush(role, userid, new List<string>() { "power", "rolepower" });
                    var data = "系统已将你的主角Buff体力提升至" + role.buff_power + "点,请查看！";
                    (new Notice()).SendNotice(role.user_id, 0, data);
                    token.Cancel();
                }, item, token.Token);
            }
        }

        private void SendAllLeadRole(int values)
        {
            var list = tg_role.GetAllLeadRole();
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(n =>
                {
                    var role = n as tg_role;
                    if (role == null) return;
                    var power = role.buff_power + values;
                    if (power > 50) power = 50;
                    role.buff_power = power;
                    role.Update();
                    if (!Variable.OnlinePlayer.ContainsKey(role.user_id)) return;
                    new RoleAttUpdate().RoleUpdatePush(role, role.user_id, new List<string>() { "power", "rolepower" });
                    var data = "系统已将你的主角Buff体力提升至" + role.buff_power + "点,请查看！";
                    (new Notice()).SendNotice(role.user_id, 0, data);
                    token.Cancel();
                }, item, token.Token);
            }
        }

        private void SendLeadRole(int values, Int64 userid)
        {
            if (Variable.OnlinePlayer.ContainsKey(userid))
            {
                var session = Variable.OnlinePlayer[userid] as TGGSession;
                if (session == null) return;
                var role = session.Player.Role.Kind;
                if (role == null) return;
                var power = role.buff_power + values;
                if (power > 50) power = 50;
                role.buff_power = power;
                role.Update();
                new RoleAttUpdate().RoleUpdatePush(role, userid, new List<string>() { "power", "rolepower" });
                var data = "系统已将你的主角Buff体力提升至" + role.buff_power + "点,请查看！";
                (new Notice()).SendNotice(role.user_id, 0, data);
            }
            else
            {
                var role = tg_role.GetEntityByUserId(userid);
                if (role == null) return;
                var power = role.buff_power + values;
                if (power > 50) power = 50;
                role.buff_power = power;
                role.Update();
            }
        }

        #endregion

        #region 游艺园每周奖励

        /// <summary> 每周奖励 </summary>
        public void SendYouYiYuanReward()
        {
            var list = view_ranking_game.GetRewardList(10);
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                var obj = new TaskObject { Userid = item.id, Ranking = item.ranks };
                Task.Factory.StartNew(n =>
                {
                    var temp = n as TaskObject;
                    if (temp == null) return;
                    var reward = Variable.BASE_YOUYIYUANREWARD.FirstOrDefault(m => m.ranking == temp.Ranking);
                    if (reward == null) return; if (reward.reward == "") return;
                    (new Message()).BuildMessagesSend(temp.Userid, "游艺园每周奖励", "游艺园第" + temp.Ranking + "名奖励发放", reward.reward);
                    token.Cancel();

                }, obj, token.Token);
            }
        }

        private class TaskObject
        {
            public Int64 Ranking { get; set; }

            public Int64 Userid { get; set; }
        }

        #endregion

        #region 合战副本每日奖励

        /// <summary>副本排行榜每日奖励</summary>
        public void CopyReward()
        {
            var list = view_ranking_copy.GetRewardList(10);
            if (!list.Any()) return;
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                var obj = new RewardObject() { UserId = item.id, Ranks = item.ranks };
                Task.Factory.StartNew(n =>
                {
                    var temp = n as RewardObject;
                    if (temp == null) return;

                    var reward = Variable.BASE_DUPLICATE_REWARD.FirstOrDefault(m => m.ranking == item.ranks);
                    if (reward == null) return;
                    if (reward.reward == "") return;

                    (new Message()).BuildMessagesSend(temp.UserId, "合战副本每日奖励", "副本排行第" + temp.Ranks + "名奖励发放", reward.reward);
                    token.Cancel();

                }, obj, token.Token);
            }
        }

        public class RewardObject
        {
            public Int64 UserId { get; set; }

            public Int64 Ranks { get; set; }
        }

        #endregion

        #endregion

        /// <summary>获取VIP等级</summary>
        private bool GetVip(int level)
        {
            var basevip = Variable.BASE_VIP.FirstOrDefault(m => m.level == level);
            if (basevip == null) return false;

            var player = Session.Player.CloneEntity();

            player.Vip.vip_gold = basevip.gold;
            player.Vip.power = basevip.power;
            player.Vip.bargain = basevip.bargain;
            player.Vip.buy = basevip.buy;
            player.Vip.arena_buy = 0;
            player.Vip.arena_cd = 0;
            player.Vip.train_home = basevip.trainHome;
            player.Vip.vip_level = basevip.level;
            player.Vip.fight = basevip.fight;
            player.Vip.car = basevip.car;

            player.Vip.Save();

            var gold = tg_user.IsGoldMax(player.User.gold, basevip.gold);
            player.User.gold = gold;
            player.User.Save();
            Session.Player = player;
            (new User()).REWARDS_API((int)GoodsType.TYPE_GOLD, player.User);

            dynamic objeuser = CommonHelper.ReflectionMethods("TGG.Module.User", "USER_VIP_PUSH");
            objeuser.CommandStart(player.User.id);
            return true;
        }

        /// <summary>更新单个武将经验信息</summary>
        private bool UpdateRoleSingleExp(int rid, int count)
        {
            var player = Session.Player;
            if (rid == 0) rid = Session.Player.Role.Kind.role_id;
            var role = tg_role.Find(new String[] { tg_role._.role_id, tg_role._.user_id }, new Object[] { rid, player.User.id });
            if (role == null) return false;

            if (role.role_state == (int)RoleStateType.PROTAGONIST)
            {
                new Upgrade().UserLvUpdate(player.User.id, count, role,0,0);  //主角
            }
            else
            {
                new Upgrade().RoleLvUpdate(player.User.id, count, role,0,0);  //其他武将
            }
            return true;
        }

        #region 更新武将体力身份信息

        /// <summary>更新武将体力身份信息</summary>
        /// <param name="rid">武将主键id</param>
        /// <param name="count">增加值</param>
        /// <param name="obj">属性名称</param>
        /// <returns></returns>
        private bool UpdateRoleSingle(int rid, int count, string obj)
        {
            var player = Session.Player.CloneEntity();
            if (rid == 0) rid = player.Role.Kind.role_id;
            var role = tg_role.Find(new String[] { tg_role._.role_id, tg_role._.user_id }, new Object[] { rid, player.User.id });
            if (role == null) return false;
            var listname = new List<string>();
            switch (obj)
            {
                case "power": role.power = CheckPower(rid, player.Role.Kind.role_id, role.power, count);
                    listname.Add("rolepower");
                    listname.Add("power"); break;
                case "honor":
                    {
                        listname.Add("honor");
                        int vocation;
                        if (player.Role.Kind.id == role.id) vocation = player.User.player_vocation;
                        else
                            vocation = (int)VocationType.Roles;
                        new Upgrade().UserIdentifyUpdate(player.User.id, count, role, vocation,0,0);
                        return true;
                    }
            }
            if (rid == player.Role.Kind.role_id)   //主角才更新session
            {
                player.Role.Kind = role;
                Session.Player = player;
            }
            role.Save();
            (new RoleAttUpdate()).RoleUpdatePush(role, player.User.id, listname);
            return true;
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

        #endregion

        #region 更新爬塔信息

        /// <summary> 爬塔跳层</summary>
        /// <param name="site">跳转层数</param>
        /// <param name="count">剩余挑战次数</param>
        /// <returns></returns>
        private bool UpdateTowerInfo(int site, int count)
        {
            var player = Session.Player.CloneEntity();
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "9009");
            if (rule == null) return false;      //验证基表信息

            if (site < 1) site = 1;
            if (site > Convert.ToInt32(rule.value)) site = Convert.ToInt32(rule.value);  //塔层信息错误改为默认塔层

            var tower = tg_duplicate_checkpoint.GetEntityByUserId(player.User.id);
            if (tower != null)      //更新更改的爬塔信息
            {
                if (!UpdateTower(tower, site)) return false;
            }
            else
            {
                if (!InsertTower(player.User.id, site)) return false;
            }
            player.UserExtend.challenge_count = count;
            if (!tg_user_extend.GetUpdate(player.UserExtend)) return false;
            Session.Player = player;
            return true;
        }

        /// <summary>更新关卡信息</summary>
        private bool UpdateTower(tg_duplicate_checkpoint tower, int site)
        {
            tower.site = site;
            tower.ninjutsu_star = null;
            tower.calculate_star = null;
            tower.state = (int)DuplicateClearanceType.CLEARANCE_UNBEGIN;
            tower.npcids = null;
            tower.npc_tea = 0;
            tower.user_tea = 0;
            tower.dekaron = 0;
            tower.blood = 1000;
            tower.user_blood = 0;
            tower.npc_blood = 0;
            tower.select_position = null;
            tower.all_cards = null;
            return tower.Update() > 0;
        }

        /// <summary>添加爬塔信息</summary>
        private bool InsertTower(Int64 userid, int site)
        {
            var tower = new tg_duplicate_checkpoint
            {
                user_id = userid,
                blood = 1000,
                state = (int)DuplicateClearanceType.CLEARANCE_UNBEGIN,
                site = site
            };
            return tower.Insert() > 0;
        }

        #endregion

        #region 更新据点保护时间

        private bool RemoveCityTime()
        {
            var player = Session.Player;
            var cityId = player.War.PlayerInCityId;
            if (cityId != 0) //只更新当前据点
            {
                var city = tg_war_city.GetCityByBidUserId(player.War.PlayerInCityId, player.User.id);
                city.guard_time = 0;
                city.Update();
                ResourceData(city);
            }
            else
            {
                //更新用户所有据点
                var citys = tg_war_city.GetEntityByUserId(player.User.id);
                if (!citys.Any()) return false;
                foreach (var item in citys)
                {
                    var city = item;
                    city.guard_time = 0;
                    city.Update();
                    ResourceData(city);
                }
            }
            return true;
        }

        #endregion

        #region 更新贡献度
        /// <summary>更新玩家贡献度</summary>
        private int GetDonate(int don, int count, int office)
        {
            var role = Session.Player.Role.Kind;
            var identity = role.role_identity;
            var ide = Variable.BASE_IDENTITY.FirstOrDefault(m => m.id == identity);
            if (ide == null) return don;
            if (ide.value < 7) return don;

            if (count <= 0) return don;
            don += count;

            //下一官职需要贡献度
            var max = Variable.BASE_OFFICE.FirstOrDefault(m => m.id == office + 1);
            if (max == null) return don;

            var maxdon = max.contribution;
            don = don >= maxdon ? maxdon : don;

            return don;
        }
        #endregion

        #region 筑城所需资源

        /// <summary>筑城所需资源</summary>
        public bool BuildCity()
        {
            var role = Session.Player.Role.Kind;
            var user = Session.Player.User.CloneEntity();
            var identity = role.role_identity;
            var ide = Variable.BASE_IDENTITY.FirstOrDefault(m => m.id == identity);
            if (ide == null) return false;
            if (ide.value < 7) return false;

            var model = Variable.WarInUser.FirstOrDefault(m => m.Key == user.id);
            if (model.Value == 0) return false;
            var prop = Variable.BASE_PROP.FirstOrDefault(m => m.targetId == model.Value);
            if (prop == null) return false;

            var count = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32085");
            if (count == null) return false;

            GetProp(prop.id, Convert.ToInt32(count.value));   //获取筑城令

            var coin = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32084");   //筑城所需金钱
            var merit = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32099");  //筑城所需战功值
            if (coin == null || merit == null) return false;

            Session.Player.User.coin = tg_user.IsCoinMax(user.coin, Convert.ToInt64(coin.value));
            Session.Player.User.merit = tg_user.IsMeritMax(user.merit, Convert.ToInt32(merit.value));
            Session.Player.User.Save();

            var type = new List<int> { (int)GoodsType.TYPE_COIN, (int)GoodsType.TYPE_MERIT };
            (new User()).REWARDS_API(type, Session.Player.User);
            return true;
        }

        #endregion

        #region 更新合战副本挑战次数

        /// <summary>获取合战副本挑战次数</summary>
        private bool GetCopyCount(int number)
        {
            var user = Session.Player.User;
            number = number > 100 ? 100 : number;
            return tg_war_copy_count.UpdateEntity(user.id, number);
        }
        #endregion

        #region 跳过完成大名令任务

        /// <summary>跳过完成大名令任务</summary>
        private bool FinishDml()
        {
            if (!CommonHelper.IsOpen(Session.Player.Role.Kind.role_level, (int)OpenModelType.大名令)) return false;

            var maintask = Session.MainTask;
            if (maintask.task_id != 2010200) return false;

            var dml = Session.Player.DamingLog;
            if (!dml.Any()) return false;
            var bigbag = dml.LastOrDefault();
            if (bigbag != null && bigbag.is_finish == 1) return false;   //大名令已经完成指令无效

            foreach (var item in dml)
            {
                var bdml = Variable.BASE_DAMING.FirstOrDefault(m => m.id == item.base_id);
                if (bdml == null) continue;
                switch (item.base_id)
                {
                    case (int)DaMingType.猎魂:
                        item.is_finish = 0;
                        item.is_reward = 0;
                        item.user_finish = bdml.degree - 1; break;   //第一条任务
                    case (int)DaMingType.大礼包:
                        var finisn = Variable.BASE_DAMING.Count;
                        item.user_finish = finisn - 2;           //大礼包完成度
                        item.is_reward = 0;
                        item.is_finish = 0; break;
                    default:
                        item.user_finish = bdml.degree;
                        item.is_finish = 1;
                        item.is_reward = 2; break;
                }
                item.Update();

                //推送协议  大名指引任务
                var _data = new ASObject(BulidData(EntityToVo.ToDaMingLingVo(item)));
                (new DaMing()).Push(item.user_id, _data);
            }
            return true;
        }

        /// <summary> 组装数据 </summary>
        private Dictionary<String, Object> BulidData(DaMingLingVo daminvo)
        {
            return new Dictionary<string, object> { { "data", daminvo } };
        }

        #endregion

        #region 更新玩家主线任务信息

        /// <summary>更新主线任务</summary>

        private bool GetMainTask(int number)
        {
            if (number == 9998)
            {
                //更新数据库任务
                Session.MainTask.task_id = 2010017;
                Session.MainTask.task_step_data = "1_200019_1|8_15020003_1_1";
                Session.MainTask.task_state = 2;
                Session.MainTask.Update();
                new TGTask().TaskPush(Session.MainTask);
                return true;
            }
            if (number == 9999)
            {
                var count = tg_bag.Delete(string.Format("user_id={0}", Session.Player.User.id));
                Session.Player.Bag.BagIsFull = false;
                Session.Player.Bag.Surplus += count;
                return true;
            }
            var taskbase = Variable.BASE_TASKMAIN.FirstOrDefault(m => m.id == number);
            if (taskbase == null) return false;

            var role = Session.Player.Role.Kind;
            if (role.role_level < taskbase.openLevel) return false;

            var maintask = Session.MainTask;
            var newtask = (new TGTask()).BuildNewMainTask(taskbase.id, taskbase.stepCondition, maintask.id, maintask.user_id);
            if (newtask == null) return false;

            //引导任务不需要接受
            if (newtask.task_step_data.StartsWith(string.Format("{0}_", (int)TaskStepType.TYPE_BUSINESS)) ||
               newtask.task_step_data.StartsWith(string.Format("{0}_", (int)TaskStepType.STUDY_SKILL)) ||
                newtask.task_step_data.StartsWith(string.Format("{0}_", (int)TaskStepType.TRAIN)))
                newtask.task_state = (int)TaskStateType.TYPE_UNFINISHED;

            //更新数据库任务
            tg_task.GetTaskUpdate(newtask.task_state, newtask.task_step_data, newtask.task_id, newtask.id, newtask.user_id);
            Session.MainTask = newtask;

            new TGTask().TaskPush(newtask);
            return true;
        }
        #endregion
    }
}
