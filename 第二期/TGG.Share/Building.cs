using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Share
{
    /// <summary>
    /// 活动共享类
    /// </summary>
    public class Building
    {
        /// <summary>
        /// 初始活动数据
        /// </summary>
        public void Init()
        {
            #region 初始化数据
            var baserule = Variable.BASE_NPC_BUILD.FirstOrDefault(q => q.type == (int)ActivityBuildType.CITY); //城池信息
            var baserule1 = Variable.BASE_NPC_BUILD.FirstOrDefault(q => q.type == (int)ActivityBuildType.BOSS && q.camp == (int)CampType.East); //东军Boss信息
            var baserule2 = Variable.BASE_NPC_BUILD.FirstOrDefault(q => q.type == (int)ActivityBuildType.BOSS && q.camp == (int)CampType.West); //西军Boss信息
            if (baserule == null || baserule1 == null || baserule2 == null) return;

            var hp = baserule.CloneEntity();
            var bhp = hp.baseHp;
            var total_hp = hp.totalHp;
            Variable.Activity.BuildActivity.WestCityBlood = bhp; //初始城市耐久度
            Variable.Activity.BuildActivity.EastCityBlood = bhp;
            Variable.Activity.BuildActivity.CityBloodFull = total_hp; //耐久度上限

            Variable.Activity.BuildActivity.EastBossId = baserule1.armyId; //战斗部队id
            Variable.Activity.BuildActivity.WestBossId = baserule2.armyId; //战斗部队id
            var army = Variable.BASE_NPCARMY.FirstOrDefault(q => q.id == baserule1.armyId); //战斗部队基表
            if (army == null) return;
            var roleid = Convert.ToInt32(army.matrix.Split(',')[0]); //战斗武将id
            var fightrole = Variable.BASE_NPCROLE.FirstOrDefault(q => q.id == roleid);
            if (fightrole == null) return;
            var boos_life = fightrole.CloneEntity().life;
            Variable.Activity.BuildActivity.EastBoosBlood = boos_life; //boss血量
            Variable.Activity.BuildActivity.WestBoosBlood = boos_life;


            var baseinfo = GetBaseData("27001");//东军出生点
            if (baseinfo == null) return;
            if (!baseinfo.Contains(',')) return;
            var split = baseinfo.Split(',');
            Variable.Activity.BuildActivity.EastBornPointX = Convert.ToInt32(split[0]);
            Variable.Activity.BuildActivity.EastBornPointY = Convert.ToInt32(split[1]);

            var baseinfo1 = GetBaseData("27002");//西军出生点
            if (baseinfo1 == null) return;
            if (!baseinfo1.Contains(',')) return;
            var split1 = baseinfo1.Split(',');
            Variable.Activity.BuildActivity.WestBornPointX = Convert.ToInt32(split1[0]);
            Variable.Activity.BuildActivity.WestBornPointY = Convert.ToInt32(split1[1]);
            Variable.Activity.BuildActivity.WoodTime = GetBaseDataToInt("27003"); //采集木材时间
            Variable.Activity.BuildActivity.MakeBuildTime = GetBaseDataToInt("27004"); //制造建材时间
            Variable.Activity.BuildActivity.TorchTime = GetBaseDataToInt("27005"); //采集火把时间
            Variable.Activity.BuildActivity.FireTime = GetBaseDataToInt("27006"); //放火时间
            Variable.Activity.BuildActivity.MakeBuildFull = GetBaseDataToInt("27009"); //建材上限
            Variable.Activity.BuildActivity.WoodFull = GetBaseDataToInt("27010"); //木材上限
            Variable.Activity.BuildActivity.TorchFull = GetBaseDataToInt("27011"); //火把上限
            Variable.Activity.BuildActivity.FameFull = GetBaseDataToInt("27012"); //声望上限
            Variable.Activity.BuildActivity.CostMakeWood = GetBaseDataToInt("27013"); //消耗建材数
            Variable.Activity.BuildActivity.MakeAddFame = GetBaseDataToInt("27014"); //制造建材增加的声望
            Variable.Activity.BuildActivity.BuildAddFame = GetBaseDataToInt("27015"); //筑城增加的声望
            Variable.Activity.BuildActivity.ReduceBlood = GetBaseDataToInt("27016"); //放火减少的耐久度
            Variable.Activity.BuildActivity.KillAddFame = GetBaseDataToInt("27017"); //击杀boss获得声望
            Variable.Activity.BuildActivity.CostWood = GetBaseDataToInt("27018"); //合成建材消耗的木头
            Variable.Activity.BuildActivity.CostFire = GetBaseDataToInt("27019"); //放火消耗的火把
            Variable.Activity.BuildActivity.BuildTime = GetBaseDataToInt("27020"); //筑城时间
            #endregion
        }

        /// <summary>
        /// 初始活动开始时间
        /// </summary>
        public void InitBulid()
        {
            var baseinfo = Variable.BASE_RULE.FirstOrDefault(q => q.id == "27007");
            if (baseinfo == null) return;
            var baseinfo1 = Variable.BASE_RULE.FirstOrDefault(q => q.id == "27008");
            if (baseinfo1 == null) return;
            var span = Convert.ToInt32(baseinfo1.value);//活动时
            Variable.Activity.BuildActivity.StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy MM dd") + " " + baseinfo.value);//开始时间
            Variable.Activity.BuildActivity.PlayTime = span;
            Variable.Activity.BuildActivity.EndTime = Variable.Activity.BuildActivity.StartTime.AddMinutes(span);//结束时间
            if (DateTime.Now > Variable.Activity.BuildActivity.EndTime)
            {
                Variable.Activity.BuildActivity.StartTime = Variable.Activity.BuildActivity.StartTime.AddDays(1);
                Variable.Activity.BuildActivity.EndTime = Variable.Activity.BuildActivity.StartTime.AddMinutes(Convert.ToDouble(baseinfo1.value));//结束时间
            }
        }

        /// <summary>
        /// 活动结束数据初始
        /// </summary>
        public void EndBulid()
        {
            Variable.Activity.BuildActivity.EndTime = Variable.Activity.BuildActivity.StartTime.AddMinutes(Variable.Activity.BuildActivity.PlayTime);
            Init();
        }

        

        /// <summary> 基表数据返回</summary>
        private string GetBaseData(string id)
        {
            var baseinfo = Variable.BASE_RULE.FirstOrDefault(q => q.id == id);
            return baseinfo == null ? null : baseinfo.CloneEntity().value;
        }

        /// <summary>
        /// 基表数据返回Int
        /// </summary>
        private int GetBaseDataToInt(string id)
        {
            var baseinfo = Variable.BASE_RULE.FirstOrDefault(q => q.id == id);
            return baseinfo == null ? 0 : Convert.ToInt32(baseinfo.CloneEntity().value);
        }

        
    }
}
