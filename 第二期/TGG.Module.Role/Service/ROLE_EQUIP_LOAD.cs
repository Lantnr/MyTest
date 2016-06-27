using System.Diagnostics;
using System.Linq;
using FluorineFx;
using System;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Vo.Equip;
using TGG.Core.Global;

namespace TGG.Module.Role.Service
{
    /// <summary>
    /// 武将装备穿戴
    /// </summary>
    public class ROLE_EQUIP_LOAD
    {
        private static ROLE_EQUIP_LOAD ObjInstance;

        /// <summary>ROLE_EQUIP_LOAD单体模式</summary>
        public static ROLE_EQUIP_LOAD GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new ROLE_EQUIP_LOAD());
        }

        private EquipVo _equipvo;

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "ROLE_EQUIP_LOAD", "武将装备穿戴");
#endif
                var roleid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "roleId").Value.ToString());          //主将主键id
                var equipid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "equipId").Value.ToString());     //装备主键id
                _equipvo = null;

                var role = tg_role.GetEntityById(roleid);
                var equip = tg_bag.GetEntityByEquipId(equipid);
                if (equip == null || role == null) return Result((int)ResultType.DATABASE_ERROR);  //验证装备武将信息
                if (equip.state == (int)LoadStateType.LOAD) return Result((int)ResultType.ROLE_EQUIP_LOAD);   //验证装备是否已经穿戴

                //验证武将等级
                if (!CheckRoleLevel(equip.base_id, role.role_level)) return Result((int)ResultType.BASE_ROLE_LEVEL_ERROR);

                RoleEquipCheck(role, equip);     //根据装备类型处理
                if (session.Player.Role.Kind.id == role.id) session.Player.Role.Kind = role;      //更新session武将信息

                var rolevo = (new Share.Role()).BuildRole(role.id);
                return new ASObject(Common.GetInstance().RoleLoadEquipData((int)ResultType.SUCCESS, rolevo, _equipvo));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>根据装备类型判断</summary>
        private void RoleEquipCheck(tg_role role, tg_bag equip)
        {
            switch (equip.equip_type)
            {
                case (int)EquipType.WEAPON: EquipUpdate(role, equip, role.equip_weapon); break;//武器  
                case (int)EquipType.ARMOR: EquipUpdate(role, equip, role.equip_armor); break; //铠甲
                case (int)EquipType.MOUNTS: EquipUpdate(role, equip, role.equip_mounts); break;//坐骑
                case (int)EquipType.TEA: EquipUpdate(role, equip, role.equip_tea); break; //茶器
                case (int)EquipType.BOOK: EquipUpdate(role, equip, role.equip_book); break;//书籍
                case (int)EquipType.NANBAN: EquipUpdate(role, equip, role.equip_barbarian); break;//南蛮物
                case (int)EquipType.ARTWORK: EquipUpdate(role, equip, role.equip_craft); break; //艺术品
                case (int)EquipType.JEWELRY: EquipUpdate(role, equip, role.equip_gem); break;//珠宝
            }
        }

        /// <summary>更新装备信息</summary>
        private void EquipUpdate(tg_role role, tg_bag nequip, Int64 id)
        {
            var s = Variable.OnlinePlayer.ContainsKey(role.user_id);
            if (!s) return;
            var session = Variable.OnlinePlayer[role.user_id] as TGGSession;
            if (session == null) return;

            if (id == 0)   //未穿戴同类型装备
            {
                var player = session.Player.CloneEntity();
                player.Bag.Surplus += 1;                            //剩余格子数+1
                if (player.Bag.BagIsFull) player.Bag.BagIsFull = false;
                session.Player = player;
            }
            else
            {
                var equip = tg_bag.GetEntityByEquipId(id);
                if (equip == null) return;
                equip.state = (int)LoadStateType.UNLOAD;
                equip.Update();
                Common.GetInstance().RoleInfoCheck(role, equip, (int)RoleDatatype.ROLEDATA_LOSE); //武将属性削减 
                _equipvo = EntityToVo.ToEquipVo(equip);
            }

            var logdata = string.Format("{0}_{1}_{2}_{3}", "EquipLoad", role.id, id, nequip.id);     //记录武将穿戴装备信息
            (new Share.Log()).WriteLog(role.user_id, (int)LogType.Use, (int)ModuleNumber.ROLE, (int)RoleCommand.ROLE_EQUIP_LOAD, logdata);

            nequip.state = (int)LoadStateType.LOAD;   //新装备
            nequip.Update();
            Common.GetInstance().RoleInfoCheck(role, nequip, (int)RoleDatatype.ROLEDATA_ADD);  //武将属性加成
            RoleUpdate(role, nequip);
        }

        /// <summary>更新武将信息</summary>
        private void RoleUpdate(tg_role role, tg_bag nequip)
        {
            switch (nequip.equip_type)
            {
                case (int)EquipType.WEAPON: role.equip_weapon = nequip.id; break;
                case (int)EquipType.ARMOR: role.equip_armor = nequip.id; break;
                case (int)EquipType.MOUNTS: role.equip_mounts = nequip.id; break;
                case (int)EquipType.TEA: role.equip_tea = nequip.id; break;
                case (int)EquipType.BOOK: role.equip_book = nequip.id; break;
                case (int)EquipType.NANBAN: role.equip_barbarian = nequip.id; break;
                case (int)EquipType.ARTWORK: role.equip_craft = nequip.id; break;
                case (int)EquipType.JEWELRY: role.equip_gem = nequip.id; break;
            }
            role.Update();
        }

        /// <summary>验证武将等级</summary>
        private bool CheckRoleLevel(int id, int level)
        {
            var bequip = Variable.BASE_EQUIP.FirstOrDefault(m => m.id == id);
            if (bequip == null) return false;
            return bequip.useLevel <= level;
        }

        private ASObject Result(int result)
        {
            return new ASObject(Common.GetInstance().RoleLoadData(result, null));
        }
    }
}
