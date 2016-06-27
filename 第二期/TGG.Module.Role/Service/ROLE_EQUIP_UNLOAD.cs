using System.Collections.Generic;
using FluorineFx;
using System;
using System.Linq;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Role.Service
{
    /// <summary>
    /// 武将装备卸载
    /// </summary>
    public class ROLE_EQUIP_UNLOAD
    {
        private static ROLE_EQUIP_UNLOAD _objInstance;

        /// <summary>ROLE_EQUIP_UNLOAD单体模式</summary>
        public static ROLE_EQUIP_UNLOAD GetInstance()
        {
            return _objInstance ?? (_objInstance = new ROLE_EQUIP_UNLOAD());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "ROLE_EQUIP_UNLOAD", "武将装备卸载");
#endif
                var roleid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "roleId").Value.ToString());
                var equipid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "equipId").Value.ToString());
                var player = session.Player.CloneEntity();

                if (player.Bag.BagIsFull || player.Bag.Surplus <= 0) return Error((int)ResultType.EQUIP_BAG_FULL);  //验证背包是否已满
                var equip = tg_bag.GetEntityByEquipId(equipid);
                var role = tg_role.GetEntityById(roleid);
                if (equip == null || role == null) return Error((int)ResultType.DATABASE_ERROR); //验证装备武将信息

                //验证装备是否处于卸载状态
                if (equip.state == (int)LoadStateType.UNLOAD) return Error((int)ResultType.ROLE_EQUIP_UNLOAD);

                RoleUpdate(role, equip.equip_type);
                Common.GetInstance().RoleInfoCheck(role, equip, (int)RoleDatatype.ROLEDATA_LOSE); //更新武将属性信息
                equip.state = (int)LoadStateType.UNLOAD;

                //验证武将  装备更新数据库信息
                if (!tg_role.UpdateByRole(role) || tg_bag.GetEquipUpdate(equip) <= 0) return Error((int)ResultType.DATABASE_ERROR);

                player.Bag.Surplus -= 1;
                if (player.Bag.Surplus == 0) player.Bag.BagIsFull = true;

                //更新主角武将信息
                if (player.Role.Kind.id == role.id) session.Player.Role.Kind = role;

                var logdata = string.Format("{0}_{1}_{2}", "EquipUnLoad", roleid, equipid);     //记录武将卸载装备信息
                (new Share.Log()).WriteLog(role.user_id, (int)LogType.Use, (int)ModuleNumber.ROLE, (int)RoleCommand.ROLE_EQUIP_UNLOAD, logdata);

                session.Player = player;
                var rolevo = (new Share.Role()).BuildRole(role.id);
                return new ASObject(Common.GetInstance().RoleLoadData((int)ResultType.SUCCESS, rolevo));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>更新武将装备信息</summary>
        /// <param name="role">tg_role</param>
        /// <param name="etype">etype装备类型</param>
        private void RoleUpdate(tg_role role, int etype)
        {
            switch (etype)
            {
                case (int)EquipType.WEAPON: role.equip_weapon = 0; break;
                case (int)EquipType.ARMOR: role.equip_armor = 0; break;
                case (int)EquipType.MOUNTS: role.equip_mounts = 0; break;
                case (int)EquipType.TEA: role.equip_tea = 0; break;
                case (int)EquipType.BOOK: role.equip_book = 0; break;
                case (int)EquipType.NANBAN: role.equip_barbarian = 0; break;
                case (int)EquipType.ARTWORK: role.equip_craft = 0; break;
                case (int)EquipType.JEWELRY: role.equip_gem = 0; break;
            }
        }

        /// <summary>返回错误信息提示</summary>
        private ASObject Error(int error)
        {
            return new ASObject(new Dictionary<string, object> { { "result", error } });
        }
    }
}
