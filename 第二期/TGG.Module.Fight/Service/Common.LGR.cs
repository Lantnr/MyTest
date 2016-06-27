using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Vo.Fight;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Fight.Service
{
    /// <summary>
    /// 部分公共方法
    /// </summary>
    public partial class Common
    {

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result, tg_fight_personal model, tg_fight_yin yin)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", result },
            { "matrix", model==null?null:EntityToVo.ToFightMatrixVo(model,yin) },
            };
            return dic;
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result}
            };
            return dic;
        }

        /// <summary>向用户推送更新</summary>
        public void RewardsInfoToUser(TGGSession session, tg_user user, int type)
        {
            session.Player.User = user;
            (new Share.User()).REWARDS_API(type, session.Player.User);
        }

        /// <summary>武将属性推送更新</summary>
        public void RoleInfoToRole(Int64 userid, tg_role role, string name)
        {
            var list = new List<string> { name };
            new RoleAttUpdate().RoleUpdatePush(role, userid, list);
        }

        /// <summary> 将武将放置对应的位置 </summary>
        /// <param name="matrix">阵</param>
        /// <param name="location">要放置的位置</param>
        /// <param name="roleid">要放置的武将id</param>
        public void PositionUpdate(ref tg_fight_personal matrix, int location, Int64 roleid)
        {
            switch (location)
            {
                case 1:
                    {
                        matrix.matrix1_rid = roleid;
                        break;
                    }
                case 2:
                    {
                        matrix.matrix2_rid = roleid;
                        break;
                    }
                case 3:
                    {
                        matrix.matrix3_rid = roleid;
                        break;
                    }
                case 4:
                    {
                        matrix.matrix4_rid = roleid;
                        break;
                    }
                case 5:
                    {
                        matrix.matrix5_rid = roleid;
                        break;
                    }
                default: { break; }
            }
        }

        /// <summary> 转换前端需要的List[YinVo] </summary>
        public List<YinVo> ConvertListYinVo(List<tg_fight_yin> list)
        {
            return list.Select(EntityToVo.ToFightYinVo).ToList();
        }
    }
}
