﻿using System;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.Title.Service
{
    /// <summary>
    /// 称号卸载
    /// </summary>
    public class ROLE_TITLE_UNLOAD
    {
        private static ROLE_TITLE_UNLOAD ObjInstance;

        /// <summary>ROLE_TITLE_UNLOAD单体模式</summary>
        public static ROLE_TITLE_UNLOAD GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new ROLE_TITLE_UNLOAD());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "ROLE_TITLE_UNLOAD", "称号卸载");
#endif
                var rid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "roleId").Value.ToString());
                var tid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "titleId").Value.ToString());

                var role = tg_role.GetEntityById(rid);
                var title = tg_role_title.GetTitleByTid(tid);
                if (role == null || title == null) return Error((int)ResultType.DATABASE_ERROR);  //验证称号信息
                if (!CheckRole(role, tid) || !CheckTitle(title, rid)) return Error((int)ResultType.FRONT_DATA_ERROR); //验证武将 称号信息
                if (title.title_load_state == (int)LoadStateType.UNLOAD) return Error((int)ResultType.TITLE_UN_LOAD);//验证称号是否已经卸载

                title = DeleteRole(title, rid);
                return !tg_role_title.UpdateByTitle(title) ? Error((int)ResultType.DATABASE_ERROR) : UpdateRole(session, role, title.title_id, title);   //更新称号信息
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>处理武将信息</summary>
        private ASObject UpdateRole(TGGSession session, tg_role role, int basetid, tg_role_title title)
        {
            var mainrole = session.Player.Role.Kind.CloneEntity();
            var btitle = Variable.BASE_ROLETITLE.FirstOrDefault(m => m.id == basetid);
            if (btitle == null) return Error((int)ResultType.BASE_TABLE_ERROR);   //验证称号基表信息
            if (string.IsNullOrEmpty(btitle.attAddition)) return Error((int)ResultType.BASE_TABLE_ERROR);     //验证属性加成信息

            role = RoleUpdateTitle(role, title.id);            //更新武将身上称号信息
            role = Common.GetInstance().UpdateRole(role, (int)RoleDatatype.ROLEDATA_LOSE, btitle.attAddition);
            if (!tg_role.UpdateByRole(role)) return Error((int)ResultType.DATABASE_ERROR);

            if (mainrole.id == role.id) session.Player.Role.Kind = role;
            Common.GetInstance().RoleUpdatePush(mainrole.user_id, role.id);            //推送武将属性更新

            return new ASObject(Common.GetInstance().BuildLoadData((int)ResultType.SUCCESS, title));
        }

        /// <summary>更新卸载掉武将的称号信息</summary>
        private tg_role_title DeleteRole(tg_role_title title, Int64 rid)
        {
            if (title.packet_role1 == rid) title.packet_role1 = 0;
            else if (title.packet_role2 == rid) title.packet_role2 = 0;
            else if (title.packet_role3 == rid) title.packet_role3 = 0;
            if (Common.GetInstance().Count(title) == 0)
                title.title_load_state = (int)LoadStateType.UNLOAD;
            return title;
        }

        /// <summary>验证称号是否装备在武将身上</summary>
        private Boolean CheckTitle(tg_role_title title, Int64 rid)
        {
            if (title.packet_role1 == rid) return true;
            if (title.packet_role2 == rid) return true;
            return title.packet_role3 == rid;
        }

        /// <summary>验证武将身上是否有对应称号</summary>
        private Boolean CheckRole(tg_role role, Int64 tid)
        {
            if (role.title_sword == tid) return true;
            if (role.title_gun == tid) return true;
            if (role.title_tea == tid) return true;
            return role.title_eloquence == tid;
        }

        /// <summary>更新武将称号信息</summary>
        private tg_role RoleUpdateTitle(tg_role role, Int64 tid)
        {
            if (role.title_sword == tid) role.title_sword = 0;
            else if (role.title_gun == tid) role.title_gun = 0;
            else if (role.title_tea == tid) role.title_tea = 0;
            else if (role.title_eloquence == tid) role.title_eloquence = 0;
            return role;
        }

        /// <summary>组装错误信息</summary>
        private ASObject Error(int error)
        {
            return new ASObject(Common.GetInstance().BuildData(error));
        }
    }
}
