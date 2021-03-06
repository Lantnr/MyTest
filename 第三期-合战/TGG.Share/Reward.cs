﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx.Messaging.Rtmp.SO;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Share
{
    public class Reward : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary> 切割奖励字符串并组装数据任务奖励 </summary>
        public List<RewardVo> GetReward(string reward, Int64 userid, int modulenumber, int command, ref List<tg_bag> proplist)
        {
            try
            {
                if (!Variable.OnlinePlayer.ContainsKey(userid)) return null;
                var session = Variable.OnlinePlayer[userid] as TGGSession;
                if (session == null) return null;
                var voc = session.Player.User.player_vocation;
                //获取奖励实体
                var rewardlist = new RewardEntity().Get(reward, voc);

                var props_count = rewardlist.Where(q => q.base_id != 0).Sum(q => q.value); //验证格子数
                if (session.Player.Bag.Surplus < props_count) return null;
                var returnreward = new List<RewardVo>();
                var user = session.Player.User.CloneEntity();
                var role = session.Player.Role.Kind.CloneEntity();
                var otherroles = tg_role.GetWarRoles(user.id);
                otherroles.Add(role);

                var tuple = new Share.Upgrade().GetName(modulenumber, command);
                foreach (var item in rewardlist)
                {
                    var type = item.goods_type;
                    var count = item.value;
                    var baseid = Convert.ToInt32(item.base_id);
                    switch (type) //奖励类型
                    {
                        #region 货币

                        case (int)GoodsType.TYPE_COIN:
                            {
                                var coin = user.coin;
                                user.coin = tg_user.IsCoinMax(user.coin, count);
                                if (user.Update() == 0) return null;
                                session.Player.User = user;
                                (new Share.Log()).WriteLog(userid, (int)LogType.Get, modulenumber, command, tuple.Item1, tuple.Item2, "金钱", (int)GoodsType.TYPE_COIN, count, user.coin, "Coin_" + coin + "_" + count + "_" + user.coin);
                                count = user.coin;
                                returnreward.Add(new RewardVo
                                {
                                    goodsType = (int)GoodsType.TYPE_COIN,
                                    value = count
                                });
                                break;
                            }

                        #endregion

                        #region 内币

                        case (int)GoodsType.TYPE_RMB:
                            {
                                user.rmb += (int)count;
                                if (user.Update() == 0) return null;
                                ;
                                session.Player.User = user;
                                (new Share.Log()).WriteLog(userid, (int)LogType.Get, modulenumber, command, tuple.Item1, tuple.Item2, "内币", (int)GoodsType.TYPE_RMB, Convert.ToInt32(count), user.rmb, "Rmb");
                                count = user.rmb;
                                returnreward.Add(new RewardVo()
                                {
                                    goodsType = (int)GoodsType.TYPE_RMB,
                                    value = Convert.ToDouble(count)
                                });
                                break;
                            }

                        #endregion

                        #region 金币

                        case (int)GoodsType.TYPE_GOLD:
                            {
                                var gold = user.gold;
                                user.gold += (int)count;
                                if (user.Update() == 0) return null;
                                session.Player.User = user;
                                (new Share.Log()).WriteLog(userid, (int)LogType.Get, modulenumber, command, tuple.Item1, tuple.Item2, "元宝", (int)GoodsType.TYPE_GOLD, Convert.ToInt32(count), user.gold, "Gold_" + gold + "_" + count + "_" + user.gold);
                                count = user.gold;
                                returnreward.Add(new RewardVo()
                                {
                                    goodsType = (int)GoodsType.TYPE_GOLD,
                                    value = count
                                });
                                break;
                            }

                        #endregion

                        #region 礼券

                        case (int)GoodsType.TYPE_COUPON:
                            {
                                user.coupon += (int)count;
                                if (user.Update() == 0) return null;
                                session.Player.User = user;
                                count = user.coupon;
                                returnreward.Add(new RewardVo()
                                {
                                    goodsType = (int)GoodsType.TYPE_COUPON,
                                    value = count
                                });
                                break;
                            }

                        #endregion

                        #region 经验

                        case (int)GoodsType.TYPE_EXP:
                            {
                                var upgrade = new Upgrade();
                                foreach (var otherrole in otherroles)
                                {
                                    if (otherrole.role_state == (int)RoleStateType.PROTAGONIST)
                                    {
                                        upgrade.UserLvUpdate(user.id, (int)count, otherrole, modulenumber, command); //用户是否升级
                                        continue;
                                    }
                                    var b = upgrade.RoleLvUpdate(session.Player.User.id, (int)count, otherrole, modulenumber, command);
                                    if (!b)
                                    {
                                        return null;
                                    }
                                }
                                returnreward.Add(new RewardVo()
                                {
                                    goodsType = (int)GoodsType.TYPE_EXP,
                                    value = count
                                });
                                upgrade.Dispose();
                                break;
                            }

                        #endregion

                        #region 道具

                        case (int)GoodsType.TYPE_PROP:
                            {
                                var baseinfo = Variable.BASE_PROP.FirstOrDefault(q => q.id == baseid);
                                if (baseinfo == null) continue;
                                if (baseinfo.typeSub == 1)
                                {
                                    (new Role()).InsertCollects(user.id, baseinfo); //插入家臣卡收集信息
                                }
                                var prop = BaseToEntity(baseinfo, user.id, (int)count);
                                proplist.Add(prop);

                                //日志
                                var ps = tg_bag.GetFindBag(userid, baseid);
                                var c = ps.Count + count;
                                (new Share.Log()).WriteLog(userid, (int)LogType.Get, modulenumber, command, tuple.Item1, tuple.Item2, baseinfo.name, (int)GoodsType.TYPE_PROP, Convert.ToInt32(count), c, "Prop_" + ps.Count + "_" + count + "_" + c);
                                break;
                            }

                        #endregion

                        #region 装备

                        case (int)GoodsType.TYPE_EQUIP:
                            {
                                for (var i = 0; i < count; i++)
                                {
                                    var equip = CommonHelper.GreenEquip(user.id, baseid);
                                    proplist.Add(equip);
                                }
                                //日志
                                var baseinfo = Variable.BASE_EQUIP.FirstOrDefault(q => q.id == baseid);
                                if (baseinfo == null) continue;
                                var ps = tg_bag.GetFindBag(userid, baseid);
                                var c = ps.Count + count;
                                (new Share.Log()).WriteLog(userid, (int)LogType.Get, modulenumber, command, tuple.Item1, tuple.Item2, baseinfo.name, (int)GoodsType.TYPE_EQUIP, Convert.ToInt32(count), c, "Equip_" + ps.Count + "_" + count + "_" + c);
                                break;
                            }

                        #endregion

                        #region 功勋

                        case (int)GoodsType.TYPE_HONOR:
                            {
                                new Upgrade().UserIdentifyUpdate(user.id, (int)count, role, user.player_vocation, modulenumber, command);
                                //用户是否身份提升
                                returnreward.Add(new RewardVo()
                                {
                                    goodsType = (int)GoodsType.TYPE_HONOR,
                                    value = count
                                });
                            }
                            break;

                        #endregion

                        #region 声望

                        case (int)GoodsType.TYPE_FAME:
                            {
                                var oldfame = user.fame;
                                user.fame = tg_user.IsFameMax(user.fame, (int)count);
                                if (user.Update() == 0) return null;
                                session.Player.User = user;
                                (new Share.Log()).WriteLog(userid, (int)LogType.Get, modulenumber, command, tuple.Item1, tuple.Item2, "声望", (int)GoodsType.TYPE_FAME, Convert.ToInt32(count), user.fame, "Fame_" + oldfame + "_" + count + "_" + user.fame);
                                count = user.fame;
                                returnreward.Add(new RewardVo
                                {
                                    goodsType = (int)GoodsType.TYPE_FAME,
                                    value = count
                                });
                                break;
                            }

                        #endregion

                        #region 魂

                        case (int)GoodsType.TYPE_SPIRIT:
                            {
                                var oldspirit = user.spirit;
                                user.spirit = tg_user.IsSpiritMax(user.spirit, (int)count);
                                if (user.Update() == 0) return null;
                                session.Player.User = user;
                                (new Share.Log()).WriteLog(userid, (int)LogType.Get, modulenumber, command, tuple.Item1, tuple.Item2, "魂", (int)GoodsType.TYPE_SPIRIT, Convert.ToInt32(count), user.spirit, "Spirit_" + oldspirit + "_" + count + "_" + user.spirit);
                                count = user.spirit;
                                returnreward.Add(new RewardVo
                                {
                                    goodsType = (int)GoodsType.TYPE_SPIRIT,
                                    value = count
                                });
                                break;
                            }

                        #endregion

                    }
                }
                return returnreward;
            }
            catch (Exception e)
            {
                XTrace.WriteLine("主线任务领取奖励出错");
                return null;
            }

        }

        /// <summary>
        /// 基表数据转换为实体
        /// </summary>
        private tg_bag BaseToEntity(BaseProp baseinfo, Int64 userid, int count)
        {
            return new tg_bag()
            {
                base_id = baseinfo.id,
                type = baseinfo.type,
                bind = baseinfo.bind,
                user_id = userid,
                count = count,
            };
        }

        /// <summary>领取任务奖励 ：如果有奖励道具，则判断背包格子数是否足够入包，不够则不入包</summary>
        public bool GetReward(String rewarddata, Int64 userid, int modulenumber, int command)
        {
            var proplist = new List<tg_bag>();
            var reward = new TGG.Share.Reward();
            var vo = reward.GetReward(rewarddata, userid, modulenumber, command, ref proplist);//
            if (vo == null) return false;
            if (proplist.Any()) //奖励道具处理
                (new Bag()).BuildReward(userid, proplist); //入包整理并推送
            (new Share.User()).REWARDS_API(userid, vo);
            reward.Dispose();
            return true;
        }

        /// <summary> 领取任务奖励 (玩家不在线) </summary>
        public void GetRewardNotOnline(string reward, long userid, int modulenumber, int command)
        {
            var user = tg_user.FindByid(userid).CloneEntity();
            var otherroles = tg_role.GetWarRoles(user.id);
            var myrole = tg_role.GetRoreByUserid(userid);
            otherroles.Add(myrole);
            if (reward == "") return;
            if (reward.Contains('#'))
                reward = reward.Split('#')[user.player_vocation];
            var rewardlist = reward.Split('|');

            var tuple = new Share.Upgrade().GetName(modulenumber, command);
            foreach (var item in rewardlist)
            {
                var typelist = item.Split('_');
                var type = Convert.ToInt32(typelist[0]);
                switch (type)
                {
                    #region 货币
                    case (int)GoodsType.TYPE_COIN:
                        {
                            var coin = user.coin;
                            var count = Convert.ToInt64(typelist[1]);
                            user.coin = tg_user.IsCoinMax(user.coin, count);
                            if (user.Update() == 0) return;
                            (new Share.Log()).WriteLog(userid, (int)LogType.Get, modulenumber, command, tuple.Item1, tuple.Item2, "金钱", (int)GoodsType.TYPE_COIN, count, user.coin, "Coin_" + coin + "_" + count + "_" + user.coin);
                            break;
                        }
                    #endregion

                    #region 内币
                    case (int)GoodsType.TYPE_RMB:
                        {
                            var count = Convert.ToInt32(typelist[1]);
                            user.rmb += count;
                            if (user.Update() == 0) return;
                            break;
                        }
                    #endregion

                    #region 金币
                    case (int)GoodsType.TYPE_GOLD:
                        {
                            var gold = user.gold;
                            var count = Convert.ToInt32(typelist[1]);
                            user.gold = tg_user.IsGoldMax(user.gold, count);
                            if (user.Update() == 0) return;
                            (new Share.Log()).WriteLog(userid, (int)LogType.Get, modulenumber, command, tuple.Item1, tuple.Item2, "元宝", (int)GoodsType.TYPE_GOLD, Convert.ToInt32(count), user.gold, "Gold_" + gold + "_" + count + "_" + user.gold);
                            break;
                        }
                    #endregion

                    #region 礼券
                    case (int)GoodsType.TYPE_COUPON:
                        {
                            var count = Convert.ToInt32(typelist[1]);
                            user.coupon += count;
                            if (user.Update() == 0) return;
                            break;
                        }
                    #endregion

                    #region 功勋
                    case (int)GoodsType.TYPE_HONOR:
                        {
                            var count = Convert.ToInt32(typelist[1]);
                            new Upgrade().UserIdentifyUpdate(userid, count, myrole, user.player_vocation, modulenumber, command);  //用户身份是否提升
                        }
                        break;
                    #endregion

                    #region 声望
                    case (int)GoodsType.TYPE_FAME:
                        {
                            var fame = user.fame;
                            var count = Convert.ToInt32(typelist[1]);
                            user.fame = tg_user.IsFameMax(user.fame, count);
                            if (user.Update() == 0) return;
                            (new Share.Log()).WriteLog(userid, (int)LogType.Get, modulenumber, command, tuple.Item1, tuple.Item2, "声望", (int)GoodsType.TYPE_FAME, Convert.ToInt32(count), user.fame, "Fame_" + fame + "_" + count + "_" + user.fame);
                            break;
                        }
                    #endregion
                }
            }
        }
        private sealed class RewardEntity
        {
            public Int32 goods_type { get; set; }

            public Int64 value { get; set; }

            public Int32 base_id { get; set; }

            public List<RewardEntity> Get(string rewardstring, int voc)
            {
                var list = new List<RewardEntity>();
                if (rewardstring == "") return null;
                if (rewardstring.Contains('#'))
                    rewardstring = rewardstring.Split('#')[voc];
                var rewardlist = rewardstring.Split('|');
                foreach (var item in rewardlist)
                {
                    var entity = new RewardEntity();
                    var typelist = item.Split('_');
                    var type = Convert.ToInt32(typelist[0]);
                    entity.goods_type = type;
                    if (typelist.Count() == 2)
                    {
                        entity.value = Convert.ToInt32(typelist[1]);
                        list.Add(entity);
                        continue;
                    }
                    entity.base_id = Convert.ToInt32(typelist[1]);
                    entity.value = Convert.ToInt32(typelist[2]);
                    list.Add(entity);
                }
                return list;
            }
        }
    }
}
