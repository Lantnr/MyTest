using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;

namespace TGG.Module.Messages.Service
{
    /// <summary>
    /// 公共方法
    /// Author:arlen xiao
    /// </summary>
    public partial class Common
    {
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
            return surplus > count;
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
    }
}
