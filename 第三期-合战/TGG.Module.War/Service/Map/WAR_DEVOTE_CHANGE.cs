using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Module.War.Service.SkyCity;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary>
    /// 贡献度兑现
    /// </summary>
    public class WAR_DEVOTE_CHANGE : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_DEVOTE_CHANGE()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_DEVOTE_CHANGE _objInstance;

        ///// <summary> WAR_DEVOTE_CHANGE单体模式 </summary>
        //public static WAR_DEVOTE_CHANGE GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DEVOTE_CHANGE());
        //}

        /// <summary> 贡献度兑现</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "WAR_DEVOTE_CHANGE", "贡献度兑现");
#endif
            if (!data.ContainsKey("devote"))
                return null;
            var devote1 =Convert.ToString(data.FirstOrDefault(m => m.Key == "devote").Value);
            if (devote1 == "" || !Regex.IsMatch(devote1, @"^\d+$"))
                return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            var devote = Convert.ToInt32(devote1);
            if (devote <= 0) return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            var user=session.Player.User.CloneEntity();
            var userextend = session.Player.UserExtend.CloneEntity();
            var rule1 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32063");
            if (rule1 == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            if (session.Player.UserExtend.devote_limit == Convert.ToInt32(rule1.value))
                return CommonHelper.ErrorResult(ResultType.WAR_DEVOTE_ENOUGH);
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32062");
            if (rule == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);    
            var merit = GetMerit(rule, devote);
            if (user.merit >= merit)
            {
                var oldmerit = user.merit;
                var olddonate = user.donate;
                var m = user.merit - merit;
                user.merit = m;
                var d=userextend.devote_limit + devote;
                if (d > Convert.ToInt32(rule1.value))
                    return CommonHelper.ErrorResult(ResultType.WAR_DEVOTE_ENOUGH);
                userextend.devote_limit += devote;
                user.donate += devote;
                user.Save();
                userextend.Save();
                session.Player.User = user;
                session.Player.UserExtend = userextend;
                var list = new List<int> { (int)GoodsType.TYPE_DONATE, (int)GoodsType.TYPE_MERIT };
                (new Share.User()).REWARDS_API(list, user);


                //日志
                var logdata1 = string.Format("{0}_{1}_{2}_{3}", "Merit", oldmerit, merit, user.merit);
                (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.WAR, (int)WarCommand.WAR_DEVOTE_CHANGE, "合战", "贡献度兑换", "战功值", (int)GoodsType.TYPE_MERIT, merit, user.merit, logdata1);
                //日志
                var logdata2 = string.Format("{0}_{1}_{2}_{3}", "Donate", olddonate, devote, user.coin);
                (new Share.Log()).WriteLog(user.id, (int)LogType.Get, (int)ModuleNumber.WAR, (int)WarCommand.WAR_DEVOTE_CHANGE, "合战", "贡献度兑换", "贡献度", (int)GoodsType.TYPE_DONATE, devote, user.donate, logdata2);
            }
            else
            {
                return CommonHelper.ErrorResult(ResultType.WAR_MERIT_ERROR);
            }
            var s = Convert.ToInt32(rule1.value)-userextend.devote_limit;
            return new ASObject(Common.GetInstance().BuilDevoteData((int)ResultType.SUCCESS, s));
        }


        /// <summary>转换成贡献度所需要的战功值</summary>
        public int GetMerit(BaseRule rule, int devote)
        {       
            var temp = rule.value;
            temp = temp.Replace("devote", devote.ToString("0.00"));
            var express = CommonHelper.EvalExpress(temp);
            var cost = Convert.ToInt32(express);
            return cost;
        }
    }
}
