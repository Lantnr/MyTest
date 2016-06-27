using FluorineFx;
using System.Collections.Generic;
using System.Linq;
using FluorineFx.Messaging.Rtmp.SO;
using TGG.Core.AMF;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using System;
using NewLife.Log;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Share;
using TGG.SocketServer;
using System.Text;
using XCode;
using XCode.DataAccessLayer;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;

namespace TGG.Module.Messages.Service
{
    /// <summary>
    /// 提取附件
    /// Author:arlen xiao
    /// </summary>
    public class MESSAGE_ATTACHMENT
    {
        private static MESSAGE_ATTACHMENT ObjInstance;

        /// <summary>MESSAGE_ATTACHMENT单体模式</summary>
        public static MESSAGE_ATTACHMENT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new MESSAGE_ATTACHMENT());
        }

        /// <summary>提取附件</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "MESSAGE_ATTACHMENT", "提取附件");
#endif
#if DEBUG
            XTrace.WriteLine("{0} {1} 剩余格子 {2}", "背包状态", session.Player.Bag.BagIsFull.ToString(), session.Player.Bag.Surplus);
#endif
            var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value);
            if (type == (int)MessageSelectType.ALL) return GetAllAttachment(session);
            var id = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value);
            return GetSingleAttachment(session, id);
        }

        /// <summary>提取单个附件</summary>
        private ASObject GetSingleAttachment(TGGSession session, Int64 id)
        {
#if DEBUG
            XTrace.WriteLine("{0} {1}", "提取单个附件", id);
#endif
            var _msg = view_messages.GetEntityById(id);
            return _msg == null || _msg.isattachment == (int)MessageIsAnnexType.UN_ANNEX ?
                Common.GetInstance().BuildData((int)ResultType.MESSAGE_NO_ATTACHMENT_ERROR) :
                 GetResources(session, new List<view_messages> { _msg });
        }

        /// <summary>提取所有附件</summary>
        private ASObject GetAllAttachment(TGGSession session)
        {
            var list = view_messages.GetMessagesIsAttByUserId(session.Player.User.id, (int)MessageIsAnnexType.HAVE_ANNEX)
                .OrderBy(m => m.create_time).ToList();
            if (!list.Any()) CommonHelper.ErrorResult(ResultType.MESSAGE_NO_ATTACHMENT_ERROR);
#if DEBUG
            XTrace.WriteLine("{0} 大小 {1}", "提取所有附件", list.Count);
#endif
            return GetResources(session, list);
        }

        #region LGR 2014.11.04

        private ASObject GetResources(TGGSession session, List<view_messages> list)
        {
            var voc = session.Player.User.player_vocation;
            var number = session.Player.Bag.Surplus;
            var userid = session.Player.User.id;
            var res = BuildResourcesItems(voc, list);
            if (!res.Any()) return CommonHelper.ErrorResult(ResultType.MESSAGE_NO_ATTACHMENT_ERROR);
            var bags = Common.GetInstance().ResourcesInBag(res).ToList();    //需要入包的资源
            if (number < bags.Count()) return CommonHelper.ErrorResult(ResultType.PROP_BAG_LACK);
            if (!UpdateState(list)) return CommonHelper.ErrorResult(ResultType.DATABASE_ERROR);
            var vr = Common.GetInstance().ResourcesNotInBag(res).ToList();   //不需要入包的资源（例:元宝、经验、金钱等）
            var rv = vr.Any() ? SetNotInBagResources(session, vr) : new List<RewardVo>();
            if (bags.Any())
            {
                var l = BuildBags(bags, userid);
                (new Bag()).BuildReward(userid, l, rv);
            }
            else (new User()).REWARDS_API(userid, rv);   //推送物品更变
            return Common.GetInstance().BuildData((int)ResultType.SUCCESS, list);
        }

        private bool UpdateState(List<view_messages> list)
        {
            foreach (var item in list)
            {
                var temp = string.Format("{0}_{1}", item.title, item.attachment + "  附件提取");
                (new Share.Log()).WriteLog(item.receive_id, (int)LogType.Update, (int)ModuleNumber.MESSAGES, (int)MessageCommand.MESSAGE_ATTACHMENT, temp);
                item.attachment = "";
                item.isread = (int)MessageIsReadType.HAVE_READ;
                item.isattachment = (int)MessageIsAnnexType.UN_ANNEX;
            }
            return tg_messages.GetMessagesUpdateList(list);
        }

        private List<tg_bag> BuildBags(IEnumerable<ResourcesItem> list, Int64 userid)
        {
            var bags = new List<tg_bag>();
            foreach (var bag in list.Select(BuildBag).Where(bag => bag != null))
            {
                bag.user_id = userid;
                bags.Add(bag);
            }
            return bags;
        }

        private tg_bag BuildBag(ResourcesItem model)
        {
            switch (model.type)
            {
                case (int)GoodsType.TYPE_EQUIP: { return (new Bag()).BuildBagByEquip(model.id); }
                case (int)GoodsType.TYPE_PROP: { return (new Bag()).BuildBag(model.id, model.count); }
                default: { return null; }
            }
        }

        private List<RewardVo> SetNotInBagResources(TGGSession session, IEnumerable<ResourcesItem> list)
        {

            var goodstype = new List<int>();
            var player = session.Player;
            string temp = "";
            foreach (var item in list)
            {
                switch (item.type)
                {
                    case (int)GoodsType.TYPE_COIN:
                        {
                            var m = (player.User.coin).ToString();
                            player.User.coin = tg_user.IsCoinMax(player.User.coin, item.count);

                            temp += string.Format("{0}_{1}_{2}_{3}", "获取铜币:", "原:" + m, "获:" + item.count, "现:" + player.User.coin + "|");

                            if (!goodstype.Contains((int)GoodsType.TYPE_COIN))
                                goodstype.Add((int)GoodsType.TYPE_COIN); break;
                        }
                    case (int)GoodsType.TYPE_GOLD:
                        {
                            var m = (player.User.gold).ToString();
                            player.User.gold += item.count;

                            temp += string.Format("{0}_{1}_{2}_{3}", "获取元宝:", "原:" + m, "获:" + item.count, "现:" + player.User.gold + "|");

                            if (!goodstype.Contains((int)GoodsType.TYPE_GOLD))
                                goodstype.Add((int)GoodsType.TYPE_GOLD); break;
                        }
                    case (int)GoodsType.TYPE_SPIRIT:
                        {
                            var m = (player.User.spirit).ToString();
                            player.User.spirit = tg_user.IsSpiritMax(player.User.spirit, item.count);

                            temp += string.Format("{0}_{1}_{2}_{3}", "获取魂:", "原:" + m, "获:" + item.count, "现:" + player.User.spirit + "|");

                            if (!goodstype.Contains((int)GoodsType.TYPE_SPIRIT))
                                goodstype.Add((int)GoodsType.TYPE_SPIRIT); break;
                        }
                    case (int)GoodsType.TYPE_FAME:
                        {
                            var m = (player.User.fame).ToString();
                            player.User.fame = tg_user.IsFameMax(player.User.fame, item.count);

                            temp += string.Format("{0}_{1}_{2}_{3}", "获取声望:", "原:" + m, "获:" + item.count, "现:" + player.User.fame + "|");

                            if (!goodstype.Contains((int)GoodsType.TYPE_FAME))
                                goodstype.Add((int)GoodsType.TYPE_FAME); break;
                        }
                }
            }
            (new Share.Log()).WriteLog(player.User.id, (int)LogType.Get, (int)ModuleNumber.MESSAGES, (int)MessageCommand.MESSAGE_ATTACHMENT, temp);
            player.User.Save();
            session.Player = player;
            return GetReward(player.User, goodstype);
        }

        private List<RewardVo> GetReward(tg_user model, IEnumerable<int> list)
        {
            var rv = new List<RewardVo>();
            foreach (var type in list)
            {
                var reward = new RewardVo();
                switch (type)
                {
                    #region

                    case (int)GoodsType.TYPE_COIN:
                        {
                            reward = new RewardVo { goodsType = type, value = model.coin };
                            break;
                        }
                    case (int)GoodsType.TYPE_COUPON:
                        {
                            reward = new RewardVo { goodsType = type, value = model.coupon };
                            break;
                        }
                    case (int)GoodsType.TYPE_GOLD:
                        {
                            reward = new RewardVo { goodsType = type, value = model.gold };
                            break;
                        }
                    case (int)GoodsType.TYPE_RMB:
                        {
                            reward = new RewardVo { goodsType = type, value = model.rmb, };
                            break;
                        }
                    case (int)GoodsType.TYPE_SPIRIT:
                        {
                            reward = new RewardVo { goodsType = type, value = model.spirit };
                            break;
                        }
                    case (int)GoodsType.TYPE_FAME:
                        {
                            reward = new RewardVo { goodsType = type, value = model.fame };
                            break;
                        }
                    #endregion
                }
                rv.Add(reward);
            }

            return rv;
        }

        /// <summary> 获取准备得到资源 </summary>
        /// <param name="voc">玩家当前职业</param>
        /// <param name="list">要获取的邮件</param>
        private List<ResourcesItem> BuildResourcesItems(int voc, IEnumerable<view_messages> list)
        {
            var rs = new List<ResourcesItem>();
            foreach (var item in list.Where(item => item.attachment != ""))
            {
                rs.AddRange(Common.GetInstance().PretreatmentResources(voc, item.attachment));
            }
            return rs;
        }

        #endregion

    }
}
