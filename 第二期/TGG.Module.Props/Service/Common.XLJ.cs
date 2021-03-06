﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Reflection;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo;
using TGG.Share;
using TGG.SocketServer;
using FluorineFx;
using TGG.Core.AMF;

namespace TGG.Module.Props.Service
{
    /// <summary>
    /// 公共方法部分类
    /// </summary>
    public partial class Common
    {
        /// <summary>资源预处理</summary>
        public List<ResourcesItem> PretreatmentResources(int voc, string res)
        {
            var vocation = res.Contains("#") ? res.Split('#')[voc] : res;
            var data = vocation.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            return data.Select(SplitResources).ToList();
        }

        /// <summary>多个相同资源预处理</summary>
        public IEnumerable<ResourcesItem> PretreatmentResourcesList(int voc, string res, int count)
        {
            var list = new List<ResourcesItem>();
            var data = PretreatmentResources(voc, res);
            for (var i = 0; i < count; i++)
            {
                list.AddRange(data);
            }
            return list;
        }


        /// <summary>背包格子是否足够</summary>
        /// <param name="list">资源集合</param>
        /// <param name="surplus">剩余格子数</param>
        public bool BagIsEnough(IEnumerable<ResourcesItem> list, int surplus)
        {
            var count = list.Count(m =>
                m.type == (int)GoodsType.TYPE_EQUIP ||
                m.type == (int)GoodsType.TYPE_PROP
                );
            return count == 0 || surplus > count;
        }

        /// <summary>入包资源</summary>
        /// <param name="list">资源集合</param>
        public IEnumerable<ResourcesItem> ResourcesInBag(IEnumerable<ResourcesItem> list)
        {
            return list.Where(m =>
                            m.type == (int)GoodsType.TYPE_EQUIP ||
                            m.type == (int)GoodsType.TYPE_PROP
                            );
        }

        /// <summary>不需要入包资源</summary>
        /// <param name="list">资源集合</param>
        public IEnumerable<ResourcesItem> ResourcesNotInBag(IEnumerable<ResourcesItem> list)
        {
            return list.Where(m =>
                            m.type == (int)GoodsType.TYPE_COIN ||
                            m.type == (int)GoodsType.TYPE_RMB ||
                            m.type == (int)GoodsType.TYPE_GOLD ||
                            m.type == (int)GoodsType.TYPE_COUPON ||
                            m.type == (int)GoodsType.TYPE_EXP ||
                            m.type == (int)GoodsType.TYPE_HONOR ||
                            m.type == (int)GoodsType.TYPE_FAME ||
                            m.type == (int)GoodsType.TYPE_SPIRIT ||
                            m.type == (int)GoodsType.TYPE_POWER
                            );
        }

        /// <summary>资源入包</summary>
        /// <param name="list">待入包资源</param>
        public List<tg_bag> InBag(Int64 user_id, IEnumerable<ResourcesItem> list)
        {
            dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Equip", "Common");
            var list_save = new List<tg_bag>();
            foreach (var item in list)
            {
                var entity = new tg_bag();
                if (item.type == (int)GoodsType.TYPE_EQUIP)
                {
                    entity = obje.GetEquip(item.id);
                }
                entity.base_id = item.id;
                entity.type = item.type;
                entity.bind = item.bind;
                entity.count = item.count;
                entity.user_id = user_id;
                list_save.Add(entity);
            }
            //tg_bag.GetSaveList(list_save);
            return list_save;
        }

        /// <summary>资源字符转换资源对象</summary>
        /// <param name="res">资源字符串</param>
        private ResourcesItem SplitResources(string res)
        {
            var res_item = new ResourcesItem();
            var data = res.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            switch (data.Length)
            {
                case 2: //类型_数量
                    res_item.type = Convert.ToInt32(data[0]);
                    res_item.count = Convert.ToInt32(data[1]);
                    break;
                case 4: //类型_id_是否绑定_数量
                    res_item.type = Convert.ToInt32(data[0]);
                    res_item.id = Convert.ToInt32(data[1]);
                    res_item.bind = Convert.ToInt32(data[2]);
                    res_item.count = Convert.ToInt32(data[3]);
                    break;
            }
            return res_item;
        }

        /// <summary>玩家信息更新及物品推送</summary>
        /// <param name="session">session</param>
        /// <param name="list">资源集合</param>
        public void UpdatePlayer(TGGSession session, IEnumerable<ResourcesItem> list)
        {
            var list_user = new List<int>();
            var userid = session.Player.User.id;
            var player = session.Player;
            var temp = string.Empty;
            foreach (var item in list)
            {
                switch (item.type)
                {
                    #region
                    case (int)GoodsType.TYPE_RMB: player.User.rmb += item.count; break;
                    case (int)GoodsType.TYPE_GOLD: player.User.gold += item.count; break;
                    case (int)GoodsType.TYPE_COUPON: player.User.coupon += item.count; break;
                    case (int)GoodsType.TYPE_EXP: player.Role.Kind.role_exp += item.count; break;
                    case (int)GoodsType.TYPE_POWER: { player.Role.Kind.power += item.count; temp = "power"; break; }
                    case (int)GoodsType.TYPE_COIN: player.User.coin = tg_user.IsCoinMax(player.User.coin, item.count); break;
                    case (int)GoodsType.TYPE_FAME: player.User.fame = tg_user.IsFameMax(player.User.fame, item.count); break;
                    case (int)GoodsType.TYPE_SPIRIT: player.User.spirit = tg_user.IsSpiritMax(player.User.spirit, item.count); break;
                    case (int)GoodsType.TYPE_HONOR: { player.Role.Kind.role_honor = tg_user.IsHonorMax(player.Role.Kind.role_honor, item.count); temp = "honor"; break; }
                    #endregion
                }
                session.Player = player;
                if (item.type == (int)GoodsType.TYPE_POWER || item.type == (int)GoodsType.TYPE_HONOR || item.type == (int)GoodsType.TYPE_EXP)
                {
                    if (item.type == (int)GoodsType.TYPE_EXP)
                    {
                        new Share.Upgrade().UserLvUpdate(player.User.id, item.count, player.Role.Kind);
                    }
                    else
                    {
                        var liststring = new List<string> { temp };
                        new RoleAttUpdate().RoleUpdatePush(player.Role.Kind, userid, liststring);
                    }
                }
                else { list_user.Add(item.type); }
            }
            player.User.Save();
            player.Role.Kind.Save();
            (new Share.User()).REWARDS_API(list_user, session.Player.User);
        }

        /// <summary>背包使用</summary>
        /// <param name="session">session</param>
        /// <param name="res">资源字符串</param>
        /// <param name="count">数量</param>
        /// <param name="prop">使用后的道具</param>
        /// <returns>结果类型</returns>
        public ResultType BagToUse(TGGSession session, string res, int count, tg_bag prop)
        {
            var userid = session.Player.User.id;
            var list_res = PretreatmentResourcesList(session.Player.User.player_vocation, res, count).ToList();
            if (!list_res.Any()) return ResultType.BAG_RESOURCES_ERROR;
            var b = BagIsEnough(list_res, session.Player.Bag.Surplus);
            if (!b) return ResultType.BAG_ISFULL_ERROR;
            //资源发放
            var list_notin = ResourcesNotInBag(list_res).ToList();
            if (list_notin.Any())
            {
                UpdatePlayer(session, list_notin);
            }
            var list_in = ResourcesInBag(list_res).ToList();
            var inbag = new List<tg_bag>();
            if (list_in.Any())
                inbag = InBag(userid, list_in);
            inbag.Add(prop);
            (new Bag()).BuildReward(userid, inbag);
            return ResultType.SUCCESS;
        }
    }
}
