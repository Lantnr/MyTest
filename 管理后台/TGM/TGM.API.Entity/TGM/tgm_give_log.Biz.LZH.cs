using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TGM.API.Entity
{
    public partial class tgm_give_log
    {
        #region 扩展属性
        /// <summary>福利卡类型</summary>
        [NonSerialized]
        private tgm_goods_type _goods_type;
        /// <summary>福利卡类型</summary>
        [XmlIgnore]
        public tgm_goods_type GoodsType
        {
            get
            {
                if (_goods_type != null || give_type <= 0 || Dirtys.ContainsKey("GoodsType")) return _goods_type;
                _goods_type = tgm_goods_type.GetTypeByTypeId(give_type);
                Dirtys["GoodsType"] = true;
                return _goods_type;
            }
            set { _goods_type = value; }
        }

        #endregion

        /// <summary>根据服务器sid 序号查询该批次激活码是否已发放</summary>
        public static tgm_give_log GetCodesBySidKind(Int32 sid, String order, Int32 type)
        {
            var where = string.Format("sid={0} and kind={1} and give_type={2}", sid, order, type);
            return Find(where);
        }

        /// <summary>根据服务器pid查询平台发放记录</summary>
        public static List<tgm_give_log> GetCodeLogsByPid(Int32 pid)
        {
            return FindAll(new String[] { _.pid }, new Object[] { pid });
        }

        /// <summary>根据服务器sid type查询已发放激活码批次</summary>
        public static List<tgm_give_log> GetCodesBySidType(Int32 sid, Int32 type)
        {
            return FindAll(new String[] { _.sid, _.give_type }, new Object[] { sid, type });
        }
    }
}
