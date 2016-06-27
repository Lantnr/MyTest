using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.AMF;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo;
using TGG.Core.Vo.Equip;
using TGG.Core.Vo.Fusion;
using TGG.Core.Vo.Prop;

namespace TGG.Share
{
    /// <summary>
    /// 公共基类
    /// </summary>
    public class CommonBase
    {
        #region 资源入包

        /// <summary>构建背包数据</summary>
        /// <param name="list">资源项集合</param>
        /// <param name="userid">玩家ID</param>
        public List<tg_bag> BuildBags(List<ResourcesItem> list, Int64 userid)
        {
            var bags = new List<tg_bag>();
            var equips = list.Where(m => m.type == (int)GoodsType.TYPE_EQUIP);
            foreach (var item in equips)
            {
                for (var i = 0; i < item.count; i++)
                {
                    item.userid = userid;
                    bags.Add(BuildBag(item));
                }
            }
            var others = list.Where(m => m.type != (int)GoodsType.TYPE_EQUIP);
            foreach (var bag in others.Select(BuildBag).Where(bag => bag != null))
            {
                bag.user_id = userid;
                bags.Add(bag);
            }
            return bags;
        }

        /// <summary>构建背包数据</summary>
        /// <param name="model">资源项</param>
        public tg_bag BuildBag(ResourcesItem model)
        {
            switch (model.type)
            {
                case (int)GoodsType.TYPE_EQUIP: { return (new Bag()).BuildBagByEquip(model); }
                case (int)GoodsType.TYPE_PROP: { return (new Bag()).BuildBag(model.id, model.userid, model.count); }
                case (int)GoodsType.TYPE_FUSION: { return (new Bag()).BuildFusion(model.id, model.userid, model.count); }
                default: { return null; }
            }
        }

        /// <summary>组装奖励VO</summary>
        public List<RewardVo> GetReward(tg_user model, IEnumerable<int> list)
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
                    case (int)GoodsType.TYPE_MERIT:
                        {
                            reward = new RewardVo { goodsType = type, value = model.merit };
                            break;
                        }
                    case (int)GoodsType.TYPE_DONATE:
                        {
                            reward = new RewardVo { goodsType = type, value = model.donate };
                            break;
                        }
                    #endregion
                }
                rv.Add(reward);
            }

            return rv;
        }

        /// <summary>入包资源</summary>
        /// <param name="list">资源集合</param>
        public IEnumerable<ResourcesItem> ResourcesInBag(IEnumerable<ResourcesItem> list)
        {
            return list.Where(m =>
                            m.type == (int)GoodsType.TYPE_EQUIP ||
                            m.type == (int)GoodsType.TYPE_PROP ||
                            m.type == (int)GoodsType.TYPE_FUSION
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
                            m.type == (int)GoodsType.TYPE_POWER ||
                            m.type == (int)GoodsType.TYPE_MERIT ||
                            m.type == (int)GoodsType.TYPE_DONATE
                            );
        }

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


        /*
        *  奖励类型：
        *  1:货币 2:内币3:金币4:礼券5:经验6:道具7:装备8:功勋9:声望10:魂11:体力
        *  |为多个奖励分隔，#为不同职业分隔，
        *  职业顺序与游戏中相同资源格式：奖励类型_数量
        *  物品格式：奖励类型_id_是否绑定_数量 
        *  装备固定属性值:奖励类型_id_是否绑定_数量_属性项(属性类型-属性值(多个属性用,分割)例:1-100,2-70)
        */
        /// <summary>资源字符转换资源对象</summary>
        /// <param name="res">资源字符串</param>
        public ResourcesItem SplitResources(string res)
        {
            var res_item = new ResourcesItem { isfixed = false };
            var data = res.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            switch (data.Length)
            {
                #region 类型_数量
                case 2:
                    {
                        res_item.type = Convert.ToInt32(data[0]);
                        res_item.count = Convert.ToInt32(data[1]);
                        break;
                    }
                #endregion

                #region 类型_id_数量
                case 3:
                    {
                        res_item.type = Convert.ToInt32(data[0]);
                        res_item.id = Convert.ToInt32(data[1]);
                        res_item.count = Convert.ToInt32(data[2]);
                        break;
                    }
                #endregion

                #region 类型_id_是否绑定_数量
                case 4:
                    {
                        res_item.type = Convert.ToInt32(data[0]);
                        res_item.id = Convert.ToInt32(data[1]);
                        res_item.bind = Convert.ToInt32(data[2]);
                        res_item.count = Convert.ToInt32(data[3]);
                        break;
                    }
                #endregion

                #region 类型_id_是否绑定_数量_属性项(属性类型-属性值(多个属性用,分割)例:1-100,2-70)
                case 5:
                    {
                        res_item.type = Convert.ToInt32(data[0]);
                        res_item.id = Convert.ToInt32(data[1]);
                        res_item.bind = Convert.ToInt32(data[2]);
                        res_item.count = Convert.ToInt32(data[3]);
                        res_item.isfixed = true;
                        var items = data[4].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var m in items.Select(item => item.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)).Where(m => m.Length == 2))
                        {
                            res_item.items.Add(new FixedItem { type = Convert.ToInt32(m[0]), value = Convert.ToDouble(m[1]) });
                        }
                        break;
                    }
                #endregion
            }
            return res_item;
        }

        /// <summary>奖励Vo转邮件附件字符串</summary>
        /// <param name="list">奖励VO</param>
        public String ToMessageAttachment(List<RewardVo> list)
        {

            var equips = list.Where(m => m.goodsType == (int)GoodsType.TYPE_EQUIP).ToList();
            var props = list.Where(m => m.goodsType == (int)GoodsType.TYPE_FUSION || m.goodsType == (int)GoodsType.TYPE_PROP).ToList();
            var c = new List<Int32> { (int)GoodsType.TYPE_EQUIP, (int)GoodsType.TYPE_FUSION, (int)GoodsType.TYPE_PROP };
            var others = list.Where(m => !c.Contains(m.goodsType)).ToList();

            var sb = new StringBuilder();

            if (others.Any())
            {
                var str_o = OthersAttachment(others);
                sb.Append(str_o);
                sb.Append("|");
            }
            if (props.Any())
            {
                var str_pf = PropAttachment(props);
                sb.Append(str_pf);
                sb.Append("|");
            }
            if (equips.Any())
            {
                var str_e = PropAttachment(equips);
                sb.Append(str_e);
                sb.Append("|");
            }
            return sb.ToString().TrimEnd("|");
        }

        /// <summary>道具附件字符串</summary>
        private String OthersAttachment(IEnumerable<RewardVo> list)
        {
            var attachment = list.Aggregate("", (current, item) => current + (item.goodsType + "_" + item.value + "|"));
            return attachment.TrimEnd("|");
        }

        /// <summary>装备附件字符串</summary>
        private String EquipAttachment(IEnumerable<RewardVo> equips)
        {
            var sb = new StringBuilder();
            foreach (var vo in equips)
            {
                var bag = AMFConvert.AsObjectToVo<EquipVo>(vo);
                var fi = AttributeToString(bag);
                //类型_id_是否绑定_数量_属性项(属性类型-属性值(多个属性用,分割)例:1-100,2-70)
                var str = String.Format("{0}_{1}_{2}_{3}_{4}", vo.goodsType, bag.baseId, bag.bind, 1, fi);
                sb.Append(str);
                sb.Append("|");
            }
            return sb.ToString().TrimEnd("|");
        }

        private String AttributeToString(EquipVo vo)
        {
            var sb = String.Empty;
            if (vo.att1 != 0)
            {
                sb += string.Format("{0}-{1},", vo.att1, vo.value1);
            }
            else if (vo.att2 != 0)
            {
                sb += string.Format("{0}-{1},", vo.att2, vo.value2);
            }
            else if (vo.att3 != 0)
            {
                sb += string.Format("{0}-{1},", vo.att3, vo.value3);
            }
            return sb.TrimEnd(",");
        }

        /// <summary>道具附件字符串</summary>
        private String PropAttachment(List<RewardVo> list)
        {
            var sb = new StringBuilder();
            var props = list.Where(m => m.goodsType == (int)GoodsType.TYPE_PROP);
            foreach (var vo in props)
            {
                var bag = AMFConvert.AsObjectToVo<PropVo>(vo);
                //类型_id_是否绑定_数量
                var str = String.Format("{0}_{1}_{2}_{3}", vo.goodsType, bag.baseId, bag.bind, bag.count);
                sb.Append(str);
                sb.Append("|");
            }
            var fusions = list.Where(m => m.goodsType == (int)GoodsType.TYPE_FUSION);
            foreach (var vo in fusions)
            {
                var bag = AMFConvert.AsObjectToVo<FusionPropVo>(vo);
                //类型_id_数量
                var str = String.Format("{0}_{1}_{2}", vo.goodsType, bag.baseId, bag.count);
                sb.Append(str);
                sb.Append("|");
            }

            return sb.ToString().TrimEnd("|");
        }

        #endregion
    }
}
