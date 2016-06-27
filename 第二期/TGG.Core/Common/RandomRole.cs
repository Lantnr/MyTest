using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Base;
using TGG.Core.Common.Randoms;
using TGG.Core.Global;

namespace TGG.Core.Common
{
    public class RandomRole
    {
        /// <summary> 获取随机非主角武将 </summary>
        /// <param name="list">品质概率对象集合</param>
        public BaseRoleInfo GetRoleByType(List<ObjectsDouble> list)
        {
            return GetRoleByType(1, list).First();
        }

        /// <summary> 获取随机非主角武将 </summary>
        /// <param name="count">要获取的个数</param>
        /// <param name="list">品质概率对象集合</param>
        public List<BaseRoleInfo> GetRoleByType(int count, List<ObjectsDouble> list)
        {
            var rs = new RandomSingle();
            var obj = rs.RandomFun(count, list);
            var l = obj.Select(m => Convert.ToInt32(m.Name)).ToList();
            return !l.Any() ? new List<BaseRoleInfo>() : GetRoleByType(l);
        }

        /// <summary> 根据品质类型随机获取非主角武将  </summary>
        /// <param name="type">品质类型集合</param>
        public List<BaseRoleInfo> GetRoleByType(IEnumerable<int> type)
        {
            return type.Select(GetRoleByType).ToList();
        }

        /// <summary> 根据品质类型 随机获取非主角武将 </summary>
        /// <param name="type">品质类型</param>
        public BaseRoleInfo GetRoleByType(int type)
        {
            var list = Variable.BASE_ROLE.Where(m => m.jobType == 0 && m.grade == type).ToList();
            if (!list.Any()) return null;
            var index = new Random(Guid.NewGuid().GetHashCode()).Next(0, list.Count);
            return list[index];
        }
    }
}
