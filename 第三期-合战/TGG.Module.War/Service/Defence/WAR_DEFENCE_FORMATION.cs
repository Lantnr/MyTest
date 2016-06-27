using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Defence
{
    /// <summary>
    /// 解锁防守阵
    /// </summary>
    public class WAR_DEFENCE_FORMATION : IDisposable
    {
        //private static WAR_DEFENCE_FORMATION _objInstance;

        ///// <summary>WAR_DEFENCE_FORMATION单体模式</summary>
        //public static WAR_DEFENCE_FORMATION GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DEFENCE_FORMATION());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~WAR_DEFENCE_FORMATION()
        {
            Dispose();
        }

        #endregion
        /// <summary> 解锁防守阵</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            //获取前端数据
            var tuple = GetClientData(data, session.Player.User.id);
            var baseid = tuple.Item2;
            var baseinfo = Variable.BASE_WAR_FRONT.FirstOrDefault(q => q.id == baseid);
            if (!tuple.Item1 || baseinfo == null) return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);

            //验证资源
            var user = session.Player.User.CloneEntity();
            var result = CheckResourse(baseinfo, user);
            if (result != (int)ResultType.SUCCESS) return CommonHelper.ErrorResult(result);
            session.Player.User = user;

            //插入数据
            tg_war_formation.Insert(new tg_war_formation() { base_id = baseid, user_id = session.Player.User.id });
            var role = tg_war_role.GetWarRoleRid(session.Player.Role.Kind.id);
            var frontids = tg_war_formation.GetEntityByUserId(session.Player.User.id).Select(q => q.base_id).ToList();
            new Share.War().SendWarRole(role, frontids, "ArmyFormation");
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
        private Tuple<bool, Int32> GetClientData(ASObject data, Int64 userid)
        {
            if (!data.ContainsKey("frontId")) return Tuple.Create(false, 0);
            var _frontId = data.FirstOrDefault(q => q.Key == "frontId").Value.ToString();
            Int32 frontId;
            Int32.TryParse(_frontId, out frontId);

            var entity = tg_war_formation.GetEntityByUserIdAndBaseId(userid, frontId);

            if (entity != null)
                return Tuple.Create(false, 0);
            return Tuple.Create(true, frontId);
        }


        /// <summary>
        /// 验证资源
        /// </summary>
        /// <param name="baseinfo"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private int CheckResourse(BaseWarDefenseFront baseinfo, tg_user user)
        {
            if (baseinfo == null) return (int)ResultType.BASE_TABLE_ERROR;
            var coststring = baseinfo.money;
            var split = coststring.Split("|").ToList();
            for (int i = 0; i < split.Count; i++)
            {
                var splitstring = split[i].Split("_").ToList();
                if (splitstring.Count != 2) return (int)ResultType.BASE_TABLE_ERROR;
                var type = splitstring[0];
                var costvalue = Convert.ToInt32(splitstring[1]);
                switch (type)
                {
                    case "2":
                        {
                            var coin = user.coin;
                            if (user.coin < costvalue) return (int)ResultType.BASE_PLAYER_COIN_ERROR;
                            user.coin -= costvalue;


                            //日志
                            var logdata = string.Format("{0}_{1}_{2}_{3}", "Coin", coin, costvalue, user.coin);
                            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.WAR, (int)WarCommand.WAR_ARMY_SOLDIER_OPEN, "合战", "解锁阵", "金钱", (int)GoodsType.TYPE_COIN, costvalue, user.coin, logdata);
                        }
                        break;
                    case "1":
                        {
                            var merit = user.merit;
                            if (user.merit < costvalue) return (int)ResultType.WAR_MERIT_ERROR;
                            user.merit -= costvalue;


                            //日志
                            var logdata = string.Format("{0}_{1}_{2}_{3}", "Merit", merit, costvalue, user.merit);
                            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.WAR, (int)WarCommand.WAR_DEFENCE_FORMATION, "合战", "解锁阵", "战功值", (int)GoodsType.TYPE_MERIT, costvalue, user.merit, logdata);
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
