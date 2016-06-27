using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGM.API.Entity
{
    public partial class tgm_gm
    {
        /// <summary> 分页获取用户账户信息 </summary>
        /// <param name="role">角色</param>
        /// <param name="pid">平台pid</param>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数</param>
        public static EntityList<tgm_gm> GetPageEntity(Int32 role, Int32 pid, Int32 index, Int32 size, out Int32 count)
        {
            var _where = role == 10000 ? "state !=0" : string.Format("[pid] ={0} and state !=0", pid);
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, " createtime desc", "*", index * size, size);
        }

        /// <summary>获取玩家GM记录</summary>
        /// <param name="pid">平台pid</param>
        /// <param name="sid">服务器sid</param>
        /// <param name="state">查询状态</param>
        /// <param name="type">查询类型</param>
        /// <param name="value">查询值</param>
        /// <param name="count">总数</param>
        public static EntityList<tgm_gm> GetPlayerEntity(Int32 pid, Int32 sid, Int32 state, Int32 type, String value, out Int32 count)
        {
            var where = type == 1 ? string.Format("[pid]={0} and sid={1} and state={2} and player_code like '%{3}%'", pid, sid, state, value)
                  : string.Format("[pid]={0} and sid={1} and state={2} and player_name like '%{3}%'", pid, sid, state, value);
            count = FindCount(where, null, null, 0, 0);
            return FindAll(where, " createtime desc", "*", 0, 0);
        }

        /// <summary>全部平台</summary>
        public static EntityList<tgm_gm> GetPlayerEntity(Int32 state, out Int32 count)
        {
            var where = string.Format("state={0}", state);
            count = FindCount(where, null, null, 0, 0);
            return FindAll(where, " createtime desc", "*", 0, 0);
        }

        /// <summary>指定平台全服</summary>
        public static EntityList<tgm_gm> GetPlayerEntity(Int32 pid, Int32 state, out Int32 count)
        {
            var where = string.Format("pid={0} and state={1}", pid, state);
            count = FindCount(where, null, null, 0, 0);
            return FindAll(where, " createtime desc", "*", 0, 0);
        }

        /// <summary>指定平台指定服务器</summary>
        public static EntityList<tgm_gm> GetPlayerEntity(Int32 pid, Int32 sid, Int32 state, out Int32 count)
        {
            var where = string.Format("pid={0} and sid={1} and state={2}", pid, sid, state);
            count = FindCount(where, null, null, 0, 0);
            return FindAll(where, " createtime desc", "*", 0, 0);
        }
    }
}
