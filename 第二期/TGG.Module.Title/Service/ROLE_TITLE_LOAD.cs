using System;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.Title.Service
{
    /// <summary>
    /// 称号装备
    /// </summary>
    public class ROLE_TITLE_LOAD
    {
        private static ROLE_TITLE_LOAD ObjInstance;

        /// <summary>ROLE_TITLE_LOAD单体模式</summary>
        public static ROLE_TITLE_LOAD GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new ROLE_TITLE_LOAD());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "ROLE_TITLE_LOAD", "称号装备");
#endif
                var tid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "titleId").Value.ToString());
                var rid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "roleId").Value.ToString());

                var title = tg_role_title.GetTitleByTid(tid);
                var role = tg_role.GetEntityById(rid);
                if (title == null || role == null) return Error((int)ResultType.DATABASE_ERROR);

                if (title.title_state == (int)TitleState.NOT_REACHED) return Error((int)ResultType.TITLE_NO_REACHED);  //验证称号达成状态
                var btitle = Variable.BASE_ROLETITLE.FirstOrDefault(m => m.id == title.title_id);
                if (btitle == null) return Error((int)ResultType.BASE_TABLE_ERROR);   //验证基表信息
                if (!CheckRepeatTitle(role, tid)) return Error((int)ResultType.TITLE_HAS_LOAD);   //验证同一称号已穿戴
                if (Common.GetInstance().Count(title) >= title.title_count) return Error((int)ResultType.TITLE_PACKET_FULL);   //验证称号格子是否已满

                //更新称号格子信息
                title = UpdateNewTitle(title, rid);
                if (Common.GetInstance().Count(title) == 1) title.title_load_state = (int)LoadStateType.LOAD;    //穿戴后称号的状态

                return CheckRole(session, role, btitle, title);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>武将信息验证</summary>
        private ASObject CheckRole(TGGSession session, tg_role role, BaseRoleTitle btitle, tg_role_title title)
        {
            var mainrole = session.Player.Role.Kind.CloneEntity();
            var otid = CheckOldTitle(role, btitle.methods, title.id);                                                 //被替换称号id
            if (otid != 0) { role = CheckRoleInfo(role, (int)RoleDatatype.ROLEDATA_LOSE, otid); }  //更新武将信息 

            if (string.IsNullOrEmpty(btitle.attAddition)) return Error((int)ResultType.BASE_TABLE_ERROR);  //验证加成信息
            Common.GetInstance().UpdateRole(role, (int)RoleDatatype.ROLEDATA_ADD, btitle.attAddition);   //加成武将属性信息

            if (!tg_role.UpdateByRole(role)) return Error((int)ResultType.DATABASE_ERROR);
            if (mainrole.id == role.id) session.Player.Role.Kind = role;               //装备武将为主角武将
            Common.GetInstance().RoleUpdatePush(mainrole.user_id, role.id);               //推送武将信息
            if (!tg_role_title.UpdateByTitle(title)) return Error((int)ResultType.DATABASE_ERROR);

            return new ASObject(Common.GetInstance().BuildLoadData((int)ResultType.SUCCESS, title));
        }

        /// <summary>验证称号是否已经穿戴</summary>
        private Boolean CheckRepeatTitle(tg_role role, Int64 tid)
        {
            if (role.title_sword != 0 && role.title_sword == tid) return false;
            if (role.title_gun != 0 && role.title_gun == tid) return false;
            if (role.title_tea != 0 && role.title_tea == tid) return false;
            return role.title_eloquence == 0 || role.title_eloquence != tid;
        }

        /// <summary>更新称号格子信息</summary>
        private tg_role_title UpdateNewTitle(tg_role_title title, Int64 rid)
        {
            if (title.packet_role1 == 0) title.packet_role1 = rid;
            else if (title.packet_role2 == 0) title.packet_role2 = rid;
            else if (title.packet_role3 == 0) title.packet_role3 = rid;
            return title;
        }

        /// <summary>验证武将是否穿戴同类型装备</summary>
        private Int64 CheckOldTitle(tg_role role, int type, Int64 ntid)
        {
            Int64 oid = 0;
            switch (type)
            {
                case (int)TitleGetType.USE_SWORD: if (role.title_sword != 0) { oid = role.title_sword; }
                    role.title_sword = ntid; break;
                case (int)TitleGetType.USE_GUN: if (role.title_gun != 0) { oid = role.title_gun; }
                    role.title_gun = ntid; break;
                case (int)TitleGetType.USE_TEA_TABLE: if (role.title_tea != 0) { oid = role.title_tea; }
                    role.title_tea = ntid; break;
                case (int)TitleGetType.BARGARN_SUCCUSS: if (role.title_eloquence != 0) { oid = role.title_eloquence; }
                    role.title_eloquence = ntid; break;
            }
            return oid;
        }

        /// <summary>更新武将信息</summary>
        private tg_role CheckRoleInfo(tg_role role, int type, Int64 oid)
        {
            var title = tg_role_title.GetTitleByTid(oid);
            if (title == null) return role;
            ClearRole(title, role.id);                     //更新被替换称号信息
            var btitle = Variable.BASE_ROLETITLE.FirstOrDefault(m => m.id == title.title_id);
            if (btitle == null) return role;
            return string.IsNullOrEmpty(btitle.attAddition) ?
                role :
                Common.GetInstance().UpdateRole(role, type, btitle.attAddition);
        }

        /// <summary>更新卸载掉武将的称号信息</summary>
        private void ClearRole(tg_role_title title, Int64 rid)
        {
            if (title.packet_role1 == rid) title.packet_role1 = 0;
            else if (title.packet_role2 == rid) title.packet_role2 = 0;
            else if (title.packet_role3 == rid) title.packet_role3 = 0;
            if (Common.GetInstance().Count(title) == 0)
                title.title_load_state = (int)LoadStateType.UNLOAD;
            title.Update();
        }

        /// <summary>组装错误信息</summary>
        private ASObject Error(int error)
        {
            return new ASObject(Common.GetInstance().BuildData(error));
        }
    }
}
