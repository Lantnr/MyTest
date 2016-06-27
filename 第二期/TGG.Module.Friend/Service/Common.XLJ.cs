using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Friend;

namespace TGG.Module.Friend.Service
{
    /// <summary>
    /// 好友公共方法部分类
    /// Author:arlen xiao
    /// </summary>
    public partial class Common
    {
        /// <summary>组装前端数据</summary>
        public Dictionary<String, Object> BuildData(int result)
        {
            var dic = new Dictionary<string, object>
            {
                {"result",  result},
            };
            return dic;
        }

        public Dictionary<String, Object> BuildData(int result, view_user_role_friend model)
        {
            var dic = new Dictionary<string, object>
            {
                {"result",  result},
                {"friend",AMFConvert.ToASObject(EntityToVo.ToFriendVo(model))}
            };
            return dic;
        }

        /// <summary>组装前端数据</summary>
        public Dictionary<String, Object> BuildData(int result, List<view_user_role_friend> list)
        {
            var dic = new Dictionary<string, object>
            {
                {"result",  result},
                {"friend",list.Any()?ConvertListASObject(list):null}
            };
            return dic;
        }

        /// <summary>格式化集合方法</summary>
        private List<ASObject> ConvertListASObject(IEnumerable<view_user_role_friend> list)
        {
            return list.Select(item => AMFConvert.ToASObject(EntityToVo.ToFriendVo(item))).ToList();
        }

        /// <summary>检查玩家在线状态</summary>
        public List<view_user_role_friend> CheckPlayerOnline(IEnumerable<view_user_role_friend> list)
        {
            var result = new List<view_user_role_friend>();
            foreach (var item in list)
            {
                result.Add(CheckSingleOnline(item));
#if DEBUG
                XTrace.WriteLine("好友在线状态 {0} {1} {2}(0:好友 1:黑名单) {3} (0:不在线 1:在线)", item.id, item.player_name, item.friend_state, item.isonline);
#endif
            }
            return result;
        }

        /// <summary>检查单个用户在线状态</summary>
        public view_user_role_friend CheckSingleOnline(view_user_role_friend model)
        {
            if (Variable.OnlinePlayer.ContainsKey(model.friend_id))
                model.isonline = (int)PlayerIsOnlineType.ONLINE;
            else
                model.isonline = (int)PlayerIsOnlineType.UN_ONLINE;
            return model;
        }

    }
}
