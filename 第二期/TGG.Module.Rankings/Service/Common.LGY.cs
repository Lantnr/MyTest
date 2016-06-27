using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Rankings;

namespace TGG.Module.Rankings.Service
{
    public partial class Common
    {
        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuildData(int result, List<UserRankingVo> listvo)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result",result},
                {"ranks",listvo},
            };
            return dic;
        }

        /// <summary> List[view_ranking_honor]/  List[view_ranking_coin]TO  List[UserRankingVo] </summary>
        public List<UserRankingVo> ConverListHonorVo(dynamic listrank, Int64 userid)
        {
            var list = new List<UserRankingVo>();
            foreach (var item in listrank)
            {
                list.Add(item.id == userid
                    ? EntityToVo.ToUserRankingVo(item, (int)RankingUserType.MYSELF)
                    : EntityToVo.ToUserRankingVo(item, (int)RankingUserType.OTHER));
            }
            return list;
        }


    }
}
