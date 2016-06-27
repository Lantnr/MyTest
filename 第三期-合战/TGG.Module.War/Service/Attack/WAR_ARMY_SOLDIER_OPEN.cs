using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Module.War.Service
{
    /// <summary>
    /// 解锁武将兵种
    /// 开发者：李德雁
    /// </summary>
    public class WAR_ARMY_SOLDIER_OPEN : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~WAR_ARMY_SOLDIER_OPEN()
        {
            Dispose();
        }

        #endregion

        //private static WAR_ARMY_SOLDIER_OPEN _objInstance;

        ///// <summary>WAR_ARMY_SOLDIER_OPEN单体模式</summary>
        //public static WAR_ARMY_SOLDIER_OPEN GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_ARMY_SOLDIER_OPEN());
        //}

        /// <summary> 解锁武将兵种 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            //获取前端数据
            var tuple = GetClientData(data);
            var roleid = tuple.Item2;
            var role = tg_war_role.FindByid(roleid);
            if (role.type == (int)WarRoleType.NPC) return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);  //备大将不能解锁兵种

            if (!tuple.Item1 || role == null) return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            var baseid = tuple.Item3;
            //验证资源
            var user = session.Player.User.CloneEntity();
            var result = CheckResourse(baseid, user);
            if (result != (int)ResultType.SUCCESS) return CommonHelper.ErrorResult(result);
            session.Player.User = user;

            //插入数据
            tg_war_army_type.Insert(new tg_war_army_type() { rid = roleid, base_id = baseid });
            var frontids = tg_war_army_type.GetListByRid(role.id).Select(q => q.base_id).ToList();
            new Share.War().SendWarRole(role, frontids, "ArmyTypes");
            return BuildData(role);
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="role">合战武将实体</param>
        /// <returns></returns>
        private ASObject BuildData(tg_war_role role)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", ResultType.SUCCESS},
                {"role", EntityToVo.ToWarRoleInfoVo(role)}
            };
            return new ASObject(dic);

        }

        /// <summary>
        /// 获取前端数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>1.数据是否正确 2.武将id 3:兵种表基表id</returns>
        private Tuple<bool, Int64, Int32> GetClientData(ASObject data)
        {
            if (!data.ContainsKey("roleId") || !data.ContainsKey("soldierId"))
                return Tuple.Create(false, 0L, 0);
            var _roleid = data.FirstOrDefault(q => q.Key == "roleId").Value.ToString();
            var _baseid = data.FirstOrDefault(q => q.Key == "soldierId").Value.ToString();
            Int64 roleid; Int32 baseid;
            Int64.TryParse(_roleid, out roleid);
            Int32.TryParse(_baseid, out baseid);

            var soldier = tg_war_army_type.GetEntityByRidAndBaseId(roleid, baseid);

            if (soldier != null)
                return Tuple.Create(false, roleid, 0);
            return Tuple.Create(true, roleid, baseid);
        }


        /// <summary>
        /// 验证资源
        /// </summary>
        /// <param name="baseid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private int CheckResourse(Int32 baseid, tg_user user)
        {
            var baseinfo = Variable.BASE_WAR_ARMY_SOLDIER.FirstOrDefault(q => q.id == baseid);
            if (baseinfo == null) return (int)ResultType.BASE_TABLE_ERROR;
            var coststring = baseinfo.lockcost;
            var split = coststring.Split("|").ToList();
            for (int i = 0; i < split.Count; i++)
            {
                var splitstring = split[i].Split("_").ToList();
                if (splitstring.Count != 2) return (int)ResultType.BASE_TABLE_ERROR;
                var type = splitstring[0];
                var costvalue = Convert.ToInt32(splitstring[1]);
                switch (type)
                {
                    case "1":
                        {
                            var merit = user.merit;
                            if (user.merit < costvalue) return (int)ResultType.WAR_MERIT_ERROR;
                            user.merit -= costvalue;

                            //日志
                            var logdata = string.Format("{0}_{1}_{2}_{3}", "Merit", merit, costvalue, user.merit);
                            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.WAR, (int)WarCommand.WAR_ARMY_SOLDIER_OPEN, "合战", "兵种解锁", "战功值", (int)GoodsType.TYPE_MERIT, costvalue, user.merit, logdata);
                        }
                        break;
                    case "2":
                        {
                            var coin = user.coin;
                            if (user.coin < costvalue) return (int)ResultType.BASE_PLAYER_COIN_ERROR;
                            user.coin -= costvalue;

                            //日志
                            var logdata = string.Format("{0}_{1}_{2}_{3}", "Coin", coin, costvalue, user.coin);
                            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.WAR, (int)WarCommand.WAR_ARMY_SOLDIER_OPEN, "合战", "兵种解锁", "金钱", (int)GoodsType.TYPE_COIN, costvalue, user.coin, logdata);
                        }
                        break;
                }
            }
            user.Update();
            var rewardlist = new List<RewardVo>
            {
                new RewardVo() {  goodsType = (int) GoodsType.TYPE_COIN, value = user.coin },
                new RewardVo() { goodsType = (int) GoodsType.TYPE_MERIT, value = user.merit }
            };
            (new Share.User()).REWARDS_API(user.id, rewardlist);
            return (int)ResultType.SUCCESS;
        }
    }
}
