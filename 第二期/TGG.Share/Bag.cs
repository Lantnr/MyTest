using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Share
{
    /// <summary>
    /// 背包共享类
    /// </summary>
    public class Bag
    {
        /// <summary> 物品整理，物品入包，并推送物品更变接口 </summary>
        /// <param name="session">玩家session</param>
        /// <param name="list">更变或保存的物品集合</param>
        public void BuildReward(Int64 user_id, List<tg_bag> list)
        {
            BuildReward(user_id, list, new List<RewardVo>());
        }

        /// <summary> 物品整理，物品入包，并推送物品更变接口 </summary>
        /// <param name="session">玩家session</param>
        /// <param name="list">更变或保存的物品集合</param>
        /// <param name="reward">附带需要推送的奖励Vo集合</param>
        public void BuildReward(Int64 user_id, List<tg_bag> list, List<RewardVo> reward)
        {
            var insertlist = new List<tg_bag>();
            var updatelist = new List<tg_bag>();
            var deletelist = new List<tg_bag>();
            BagMerge(list, ref insertlist, ref updatelist, ref deletelist);
            TrainingProps(user_id, insertlist, updatelist, deletelist, reward);
        }

        /// <summary>集合转换ASObject集合</summary>
        public List<ASObject> ConvertListASObject(dynamic list, string classname)
        {
            var list_aso = new List<ASObject>();
            foreach (var item in list)
            {
                dynamic model;
                switch (classname)
                {
                    case "Props": model = EntityToVo.ToPropVo(item); break;
                    case "Equip": model = EntityToVo.ToEquipVo(item); break;
                    default: model = null; break;
                }
                list_aso.Add(AMFConvert.ToASObject(model));
            }
            return list_aso;
        }

        /// <summary> 组装道具 </summary>
        /// <param name="id">道具基表Id</param>
        /// <param name="count">道具数量</param>
        /// <returns></returns>
        public tg_bag BuildBag(int id, int count = 1)
        {
            var bprop = Variable.BASE_PROP.FirstOrDefault(m => m.id == id);
            if (bprop == null) return null;
            return new tg_bag
            {
                base_id = bprop.id,
                bind = bprop.bind,
                count = count,
                type = bprop.type,
            };
        }

        #region 组装装备类型物品

        /// <summary> 组装装备类型物品 </summary>
        /// <param name="equip">要组装物品的装备基表Id</param>
        public tg_bag BuildBagByEquip(Int64 equip)
        {
            var baseEquip = Variable.BASE_EQUIP.FirstOrDefault(m => m.id == equip);
            return baseEquip == null ? null : BuildEquipAtt(baseEquip);
        }


        /// <summary>组装获得装备属性</summary>
        public tg_bag BuildEquipAtt(BaseEquip equip)
        {
            var newequip = new tg_bag();
            #region
            if (equip.captain != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_CAPTAIN, equip.captain);
            }
            if (equip.force != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_FORCE, equip.force);
            }
            if (equip.brains != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_BRAINS, equip.brains);
            }
            if (equip.govern != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_GOVERN, equip.govern);
            }
            if (equip.charm != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_CHARM, equip.charm);
            }
            if (equip.attack != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_ATTACK, equip.attack);
            }
            if (equip.hurtIncrease != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_HURTINCREASE, Convert.ToDouble(equip.hurtIncrease));
            }
            if (equip.defense != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_DEFENSE, equip.defense);
            }
            if (equip.hurtReduce != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_HURTREDUCE, Convert.ToDouble(equip.hurtReduce));
            }
            if (equip.life != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_LIFE, equip.life);
            }
            #endregion
            newequip.base_id = equip.id;
            newequip.type = (int)GoodsType.TYPE_EQUIP;
            newequip.equip_type = equip.typeSub;
            newequip.state = 0;
            newequip.count = 1;
            return newequip;
        }

        /// <summary>装备的基础属性</summary>
        /// <param name="equip">装备tg_bag</param>
        /// <param name="type">属性类型</param>
        /// <param name="value">加成属性值</param>
        /// <returns>装备tg_bag</returns>
        public tg_bag EquipAtt(tg_bag equip, int type, double value)
        {
            if (equip.attribute1_type == 0)
            {
                equip.attribute1_type = type;
                equip.attribute1_value = value;
            }
            else if (equip.attribute2_type == 0)
            {
                equip.attribute2_type = type;
                equip.attribute2_value = value;
            }
            else if (equip.attribute3_type == 0)
            {
                equip.attribute3_type = type;
                equip.attribute3_value = value;
            }
            return equip;
        }

        #endregion

        /// <summary>背包整理</summary>
        /// <param name="props">要修改，或要入包的道具</param>
        /// <param name="insertlist">整理后，插入数据库后的道具</param>
        /// <param name="updatelist">整理后，修改数据库后的道具</param>
        /// <param name="deletelist">整理后，删除数据库后的道具</param>
        private void BagMerge(List<tg_bag> props, ref List<tg_bag> insertlist, ref List<tg_bag> updatelist, ref List<tg_bag> deletelist)
        {
#if DEBUG
            XTrace.WriteLine("{0} 集合大小 {1}", "背包整理", props.Count);
#endif
            deletelist.AddRange(props.Where(m => m.count == 0).ToList());
            var proplist = props.Where(m => m.count > 0).ToList();

            var equiplist = proplist.Where(m => m.type == (int)GoodsType.TYPE_EQUIP).ToList(); //装备
            if (equiplist.Any()) insertlist.AddRange(equiplist);

            var list = proplist.Where(m => m.type != (int)GoodsType.TYPE_EQUIP).ToList();       //道具
            if (list.Any())
            {
                var ids = list.GroupBy(m => m.base_id).Select(m => m.Key);//查询所有BaseId
                var no_ids = list.Where(m => m.id > 0).Select(m => m.id); //查询所有有Id的道具Id
                var bag = tg_bag.GetFindBagNotFull(list.First().user_id, (int)GoodsType.TYPE_EQUIP, ids, no_ids);//查询排除传进来有Id的数据库道具
                list.AddRange(bag);
#if DEBUG
                XTrace.WriteLine("{0} 集合大小 {1}", "待整理道具", bag.Count);
#endif
                var _list = list.OrderByDescending(m => m.id).GroupBy(m => m.base_id);
                foreach (var item in _list)
                {
                    SingleMerge(item, ref insertlist, ref updatelist, ref deletelist);
                }
            }

            if (insertlist.Any()) insertlist.ForEach(item => item.Save());
            if (updatelist.Any()) updatelist.ForEach(item => tg_bag.UpdateCount(item));
            if (deletelist.Any()) tg_bag.GetDeleteIds(deletelist.Select(m => m.id).ToList());
#if DEBUG
            XTrace.WriteLine("{0} 添加 {1}  更新 {2}  删除 {3}", "整理道具结果", insertlist.Count, updatelist.Count, deletelist.Count);
#endif
        }

        /// <summary>单次整理</summary>
        /// <param name="props">要修改，或要入包的道具</param>
        /// <param name="insertlist">整理后，插入数据库后的道具</param>
        /// <param name="updatelist">整理后，修改数据库后的道具</param>
        /// <param name="deletelist">整理后，删除数据库后的道具</param>
        private void SingleMerge(IEnumerable<tg_bag> props, ref List<tg_bag> insertlist, ref List<tg_bag> updatelist, ref List<tg_bag> deletelist)
        {
#if DEBUG
            XTrace.WriteLine("整理前集合大小 insert:{0} update:{1} detele:{2}", "", insertlist.Count, updatelist.Count, deletelist.Count);
#endif
            tg_bag prop = null;
            foreach (var p in props)
            {
                if (prop == null) { prop = p.CloneEntity(); continue; }
                var _p = p.CloneEntity();
                var total = prop.count + _p.count;
                if (total <= 99)
                {
                    prop.count = total;
                    if (_p.id != 0) deletelist.Add(_p);
                    if (total == 99)
                    {
                        if (prop.id == 0) insertlist.Add(prop);
                        else updatelist.Add(prop);
                        _p = null;
                    }
                    else _p = prop;
                }
                else
                {
                    prop.count = 99;
                    _p.count = total - 99;
                    if (prop.id == 0) insertlist.Add(prop);
                    else updatelist.Add(prop);
                }
                prop = _p;
            }
            if (prop == null) return;
            if (prop.id == 0) insertlist.Add(prop);
            else updatelist.Add(prop);
#if DEBUG
            XTrace.WriteLine("整理后集合大小 insert:{0} update:{1} detele:{2}", "", insertlist.Count, updatelist.Count, deletelist.Count);
#endif
        }

        /// <summary> 
        /// 推送前端物品
        /// 对已做数据库操作的物品进行物品更变推送 
        /// </summary>
        /// <param name="session">玩家session</param>
        /// <param name="insertlist">以对数据库进行插入操作的物品</param>
        /// <param name="updatelist">以对数据库进行修改操作的物品</param>
        /// <param name="deletelist">以对数据库进行删除操作的物品</param>
        /// <param name="reward">其他数值更变的奖励Vo</param>
        private void TrainingProps(Int64 user_id, List<tg_bag> insertlist, List<tg_bag> updatelist, List<tg_bag> deletelist, List<RewardVo> reward)
        {
            var rewards = new List<RewardVo>();
            if (insertlist.Any())
            {
                var list = BuildRewardVo(insertlist, "insert");  //组装 增加物品 奖励Vo
                if (list.Any())
                {
                    rewards.AddRange(list);
                    GetBagSurplus(user_id, insertlist.Count(), false);
                }
            }
            if (updatelist.Any())
            {
                var list = BuildRewardVo(updatelist, "update");  //组装 修改物品 奖励Vo
                if (list.Any()) rewards.AddRange(list);
            }
            if (deletelist.Any())
            {
                var list = BuildRewardVo(deletelist, "delete");  //组装 删除物品 奖励Vo
                if (list.Any()) rewards.AddRange(list);
                GetBagSurplus(user_id, deletelist.Count(), true);
            }
            if (reward.Any()) rewards.AddRange(reward);          //涉及到其他数值更变的奖励Vo
            if (rewards.Any())
                (new Share.User()).REWARDS_API(user_id, rewards);   //推送物品更变
        }

        /// <summary> 将物品集合组装成物品更变接口需要的奖励Vo </summary>
        /// <param name="list">需要通知前端更变的物品集合</param>
        /// <param name="name">操作名  类型有:update、insert、delete</param>
        /// <returns>组装好的奖励Vo集合</returns>
        private List<RewardVo> BuildRewardVo(List<tg_bag> list, string name)
        {
            name = name.ToLower();
            var rewards = new List<RewardVo>();
            var equiplist = list.Where(m => m.type == (int)GoodsType.TYPE_EQUIP).ToList();
            var proplist = list.Where(m => m.type != (int)GoodsType.TYPE_EQUIP).ToList();
            switch (name)
            {
                case "update":
                    {
                        rewards.Add(new RewardVo { goodsType = (int)GoodsType.TYPE_PROP, decreases = ConvertListASObject(list, "Props") });
                        break;
                    }
                case "delete":
                    {
                        var equipids = equiplist.Where(m => m.id != 0).Select(m => Convert.ToDouble(m.id)).ToList();
                        var propids = proplist.Where(m => m.id != 0).Select(m => Convert.ToDouble(m.id)).ToList();
                        if (equipids.Any()) //装备类型
                            rewards.Add(new RewardVo { goodsType = (int)GoodsType.TYPE_EQUIP, deleteArray = equipids });
                        if (propids.Any())  //道具类型
                            rewards.Add(new RewardVo { goodsType = (int)GoodsType.TYPE_PROP, deleteArray = propids });
                        break;
                    }
                case "insert":
                    {
                        if (equiplist.Any()) //装备类型
                            rewards.Add(new RewardVo { goodsType = (int)GoodsType.TYPE_EQUIP, increases = ConvertListASObject(equiplist, "Equip") });
                        if (proplist.Any())  //道具类型
                            rewards.Add(new RewardVo { goodsType = (int)GoodsType.TYPE_PROP, increases = ConvertListASObject(proplist, "Props") });
                        break;
                    }
                default: { return null; }
            }
            return rewards;
        }

        /// <summary> 操作背包格子数量 </summary>
        /// <param name="session">玩家session</param>
        /// <param name="count">要加减的数</param>
        /// <param name="flag">true:增加剩余格子 flag:减少格子数</param>
        private void GetBagSurplus(Int64 user_id, int count, bool flag)
        {
            var b = Variable.OnlinePlayer.ContainsKey(user_id);
            if (!b) return;
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return;

            if (flag)
                session.Player.Bag.Surplus += count;
            else
                session.Player.Bag.Surplus -= count;
            session.Player.Bag.BagIsFull = session.Player.Bag.Surplus <= 0;
        }


    }
}
