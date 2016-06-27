using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XCode;

namespace TGM.API.Entity
{
    public partial class tgm_goods_code
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
                if (_goods_type != null || type <= 0 || Dirtys.ContainsKey("GoodsType")) return _goods_type;
                _goods_type = tgm_goods_type.GetTypeByTypeId(type);
                Dirtys["GoodsType"] = true;
                return _goods_type;
            }
            set { _goods_type = value; }
        }

        #endregion

        /// <summary>查询服务器元宝信息记录</summary>
        /// <param name="pid">平台sid</param>   
        /// <param name="order">生成序号</param> 
        /// <param name="type">卡牌类型</param>     
        /// <param name="index">分页索引值</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数量</param>
        /// <returns></returns>
        public static EntityList<tgm_goods_code> GetPageEntity(Int32 pid, String order, Int32 type, Int32 index, Int32 size, out Int32 count)
        {
            string where;
            if (string.IsNullOrEmpty(order))
            {
                @where = type == 0 ?
                    string.Format("pid={0}", pid) :
                    string.Format("pid={0} and type={1}", pid, type);
            }
            else
            {
                @where = type == 0 ?
                    string.Format("pid={0} and kind='{1}'", pid, order) :
                    string.Format("pid={0} and kind='{1}' and type={2}", pid, order, type);
            }
            count = FindCount(where, null, null, 0, 0);
            return FindAll(where, null, "*", index * size, size);
        }

        /// <summary>批量插入福利卡激活码信息</summary>
        public static bool InsertCodes(IEnumerable<tgm_goods_code> codes)
        {
            var list = new EntityList<tgm_goods_code>();
            list.AddRange(codes);
            return list.Insert() > 0;
        }

        /// <summary>根据平台id 序号查询激活码是否存在</summary>
        public static List<tgm_goods_code> GetCodesByPidKind(Int32 pid, String order, Int32 type)
        {
            var where = string.Format("pid={0} and kind={1} and type={2}", pid, order, type);
            return FindAll(where, null, null, 0, 0);
        }

        /// <summary>根据平台id查询激活码信息</summary>
        public static List<tgm_goods_code> GetCodesByPid(Int32 pid)
        {
            return FindAll(new String[] { _.pid }, new Object[] { pid });
        }

        /// <summary>根据平台id，福利卡类型查询激活码信息</summary>
        public static List<tgm_goods_code> GetCodesByPidType(Int32 pid, Int32 type)
        {
            return FindAll(new String[] { _.pid, _.type }, new Object[] { pid, type });
        }

        /// <summary>根据平台id 序号查询激活码是否存在</summary>
        public static List<tgm_goods_code> GetExcel(Int32 pid, Int32 type, String order)
        {

            var exp = new WhereExpression();
            exp &= _.pid == pid;
            if (type > 0)
                exp &= _.type == type;
            if (String.IsNullOrEmpty(order))
                exp &= _.kind == order;

            return FindAll(exp, null, null, 0, 10000);
        }

    }
}
