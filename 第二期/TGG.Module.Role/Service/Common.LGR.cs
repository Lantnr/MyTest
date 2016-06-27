using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.SocketServer;
using XCode;

namespace TGG.Module.Role.Service
{
    public partial class Common
    {
        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result, Int64 time, List<tg_role_recruit> list)
        {
            var dic = new Dictionary<string, object> 
            { 
                { "result", result },
                { "time", time },
                { "role",list==null ? null:ConvertRecruitVo(list) },               
            };
            return dic;
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", result },
            };
            return dic;
        }

        /// <summary>向用户推送更新</summary>
        public void RewardsToUser(TGGSession session, tg_user user, int type)
        {
            session.Player.User = user;
            (new Share.User()).REWARDS_API(type, session.Player.User);
        }

        #region 插入武将招募信息

        #endregion

        #region 数据转换
        /// <summary>tg_role_recruit 转换 RecruitVo  </summary>
        private List<RecruitVo> ConvertRecruitVo(IEnumerable<tg_role_recruit> list)
        {
            return list.Select(EntityToVo.ToRecruitVo).ToList();
        }

        #endregion

    }
}
