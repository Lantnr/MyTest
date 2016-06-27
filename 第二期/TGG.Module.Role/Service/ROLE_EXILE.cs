using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Entity;

namespace TGG.Module.Role.Service
{
    /// <summary>
    /// 武将放逐
    /// </summary>
    public class ROLE_EXILE
    {
        private static ROLE_EXILE _objInstance;

        /// <summary>ROLE_EXILE单体模式</summary>
        public static ROLE_EXILE GetInstance()
        {
            return _objInstance ?? (_objInstance = new ROLE_EXILE());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "ROLE_EXILE", "武将放逐");
#endif
                var id = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "roleId").Value.ToString());

                if (session.Player.Role.Kind.id == id) return Result((int)ResultType.FRONT_DATA_ERROR);    //验证主角武将不能放逐
                var role = tg_role.GetEntityById(id);
                if (role == null) return Result((int)ResultType.DATABASE_ERROR);

                if (!RoleEquipUpdate(role)) return Result((int)ResultType.DATABASE_ERROR);    //验证武将身上装备信息
                if (!DeleteTrain(id)) return Result((int)ResultType.DATABASE_ERROR);         //验证武将修行信息
                CheckRoleAndUpdate(role);     //处理称号 评定 布阵信息

                if (role.Delete() <= 0) return Result((int)ResultType.DATABASE_ERROR); 
                RecordRole(role.user_id, role.role_id);   //记录删除武将

                return Result((int)ResultType.SUCCESS);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>删除武将身上装备信息</summary>
        private bool RoleEquipUpdate(tg_role role)
        {
            var ids = new List<Int64>();
            if (role.equip_weapon != 0) ids.Add(role.equip_weapon);
            if (role.equip_armor != 0) ids.Add(role.equip_armor);
            if (role.equip_mounts != 0) ids.Add(role.equip_mounts);
            if (role.equip_tea != 0) ids.Add(role.equip_tea);
            if (role.equip_book != 0) ids.Add(role.equip_book);
            if (role.equip_barbarian != 0) ids.Add(role.equip_barbarian);
            if (role.equip_craft != 0) ids.Add(role.equip_craft);
            if (role.equip_gem != 0) ids.Add(role.equip_gem);

            if (!ids.Any()) return true;
            if (!RecordEquips(role.user_id, ids)) return false;     //删除日志记录
            return tg_bag.DeleteEquips(ids);
        }

        /// <summary>验证武将关联信息并处理</summary>
        private void CheckRoleAndUpdate(tg_role role)
        {
            (new Share.Role()).RoleExile(role);    //验证处理称号信息
            (new Share.TGTask()).TaskDelete(role.id, role.user_id);   //处理评定任务
            (new Share.Role()).DeleteMatrixRole(role);   //处理阵中武将信息
        }

        /// <summary>验证处理武将修行信息</summary>
        private bool DeleteTrain(Int64 rid)
        {
            var train = tg_train_role.GetEntityByRid(rid);         //查询修行信息
            if (train == null) return true;
            return train.Delete() > 0;
        }

        /// <summary>记录删除武将信息</summary>
        private void RecordRole(Int64 userid, int roleid)
        {
            var rdata = string.Format("{0}_{1}", "RoleDelete", roleid);     //记录放逐删除武将基表id
            (new Share.Log()).WriteLog(userid, (int)LogType.Delete, (int)ModuleNumber.ROLE, (int)RoleCommand.ROLE_EXILE, rdata);
        }

        /// <summary> 放逐武将记录武将装备</summary>
        private bool RecordEquips(Int64 userid, List<long> ids)
        {
            try
            {
                var equips = tg_bag.GetEquipsByIds(ids);
                if (!equips.Any()) return false;

                foreach (var item in equips)
                {
                    var logdata = string.Format("{0}_{1}", "EquipDelete", item.base_id);     //记录放逐删除装备基表ids
                    (new Share.Log()).WriteLog(userid, (int)LogType.Delete, (int)ModuleNumber.ROLE, (int)RoleCommand.ROLE_EXILE, logdata);

                    log.BagInsertLog(item, (int)ModuleNumber.ROLE, (int)RoleCommand.ROLE_EXILE, 0);   //记录日志
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>组装信息</summary>
        private ASObject Result(int result)
        {
            return new ASObject(Common.GetInstance().RoleLoadData(result, null));
        }
    }
}
