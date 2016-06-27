using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 开始修行
    /// </summary>
    public class TRAIN_ROLE_START : IConsume
    {
        public ASObject Execute(Int64 userid, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "TRAIN_ROLE_START", "开始修行");
#endif
                var session = Variable.OnlinePlayer[userid] as TGGSession;
                if (session == null) return CommonHelper.ErrorResult((int)ResultType.FAIL);
                var rid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);
                var attribute = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "attribute").Value);
                var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);
                var count = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "count").Value);
                if (attribute == 0 || type == 0)
                    return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);

                var user = session.Player.User.CloneEntity();
                var train_role = tg_train_role.GetEntityByRid(rid);
                var role = tg_role.GetRoleById(rid);
                if (train_role == null || role == null)
                    return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
                if (train_role.state == (int)RoleTrainStatusType.TRAINING)
                    return CommonHelper.ErrorResult((int)ResultType.TRAINROLE_TRAINING);
                var _role = role.CloneEntity();

                var baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17016");
                var base_train = Variable.BASE_TRAIN.FirstOrDefault(m => m.id == type);
                if (base_train == null || baserule == null)
                    return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);
                var result = Check(user, role, attribute, base_train, count, Convert.ToInt32(baserule.value));
                if (result != 0) return CommonHelper.ErrorResult(result);

                role = tg_role.GetRoleById(rid);
                train_role = CreateTrainRole(train_role, base_train.time * count, attribute, type, count);
                train_role.Save();
                (new Share.RoleTrain()).RoleTrainThreading(session.Player.User.id, role.id, count * base_train.time * 60 * 60 * 1000); //推送武将修行完信息

                (new Share.TGTask()).MainTaskUpdate(TaskStepType.TRAIN, userid, 0, 0); //任务验证
                (new Share.Skill()).PowerLog(_role, (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_ROLE_START);
                (new Share.DaMing()).CheckDaMing(role.user_id, (int)DaMingType.修行);    //检测大名修行完成度
                return new ASObject(BuilData((int)ResultType.SUCCESS, (new Share.Role()).BuildRole(rid)));

            }
            catch (Exception ex)
            {
                XTrace.WriteLine("{0}:{1}", "TRAIN_ROLE_START", "开始修行错误{0}", ex.Message);
                return new ASObject();
            }
        }

        private int Check(tg_user user, tg_role role, int attribute, BaseTrain base_train, int count, int limit)
        {
            var value = base_train.count * count;
            if (count > limit)
                return (int)ResultType.DATABASE_ERROR;
            var att = (new RoleTrain()).GetCanAtt(user, role, attribute, base_train, count);
            //属性值检查            
            if (att <= 0 || att < value)
                return (int)ResultType.TRAIN_ROLE_ATT_ENOUGH;
            var att2 = CheckCanTrain(role, base_train, attribute, count);
            if (att2 <= 0 || att2 < value)
                return (int)ResultType.TRAIN_ROLE_ATT_ENOUGH;

            var govern = tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, role);
            if (govern < base_train.govern)
                return (int)ResultType.TRAINROLE_GOVERN_LACK;

            if (base_train.nextId > 0)
            {
                var nexttrain = Variable.BASE_TRAIN.FirstOrDefault(m => m.id == base_train.nextId);
                if (nexttrain == null)
                    return (int)ResultType.BASE_TABLE_ERROR;
                if (att >= nexttrain.govern)
                    return (int)ResultType.TRAINROLE_DEGREE_WRONG;
            }

            if (!PowerOperate(role, count)) //体力判断
                return (int)ResultType.BASE_ROLE_POWER_ERROR;
            if (!CheckGold(count, user)) //元宝判断
                return (int)ResultType.BASE_PLAYER_GOLD_ERROR;
            return 0;

            //if (att <= 0 || !CheckCanTrain(role, base_train, attribute,count))
            //    return (int)ResultType.TRAIN_ROLE_ATT_ENOUGH;
        }

        private bool CheckGovern(int nextid, double att)
        {
            var nexttrain = Variable.BASE_TRAIN.FirstOrDefault(m => m.id == nextid);
            if (nexttrain == null)
                return false;
            if (att > nexttrain.govern)
                return false;
            return true;
        }

        /// <summary>体力操作</summary>
        private bool PowerOperate(tg_role role, int count)
        {
            var power = RuleConvert.GetCostPower();
            power = power * count;
            var totalpower = tg_role.GetTotalPower(role);
            if (totalpower < power) return false;
            new Share.Role().PowerUpdate(role, power, role.user_id);
            return true;
        }

        /// <summary>武将属性总值判断</summary>
        private double CheckCanTrain(tg_role role, BaseTrain basetrain, int attribute, int count)
        {
            count = basetrain.count * count;
            var att = tg_role.GetSingleCanTrain((RoleAttributeType)attribute, count, role);
            return att;
            //return att > 0;
        }

        private tg_train_role CreateTrainRole(tg_train_role train_role, int time, int attribute, int type, int count)
        {
            //# if DEBUG
            //            base_train.time = 1;
            //#endif
            train_role.state = (int)RoleTrainStatusType.TRAINING;
            train_role.time = time * 60 * 60 * 1000 + CurrentTime();
            train_role.attribute = attribute;
            train_role.type = type;
            train_role.count = count;
            return train_role;
        }

        /// <summary> 当前时间毫秒</summary>
        public Int64 CurrentTime()
        {
            // ReSharper disable once PossibleLossOfFraction
            return (DateTime.Now.Ticks - 621355968000000000) / 10000;
        }

        /// <summary>验证政务值是否足够</summary>
        private bool GovernProcess(tg_role role, BaseTrain base_train)
        {
            var govern = tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, role);
            return govern >= base_train.govern;
        }

        /// <summary>验证消耗元宝</summary>
        /// <param name="count">修炼次数</param>
        private bool CheckGold(int count, tg_user user)
        {
            if (!Variable.OnlinePlayer.ContainsKey(user.id)) return false;
            var session = Variable.OnlinePlayer[user.id] as TGGSession;
            if (session == null) return false;

            var gold = user.gold;
            var cost = GetCost(count);
            if (user.gold < cost) return false;
            user.gold -= cost;
            user.Save();
            session.Player.User = user;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);

            //日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Gold", gold, cost, user.gold);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_ROLE_START, logdata);
            return true;
        }

        /// <summary>获取消耗的元宝</summary>
        private int GetCost(int count)
        {
            var baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17015");
            if (baserule == null) return 0;
            var temp = baserule.value;
            temp = temp.Replace("count", count.ToString("0.00"));
            var express = CommonHelper.EvalExpress(temp);
            var cost = Convert.ToInt32(express);
            return cost;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilData(int result, RoleInfoVo rolevo)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"role", rolevo}
            };
            return dic;
        }
    }
}
