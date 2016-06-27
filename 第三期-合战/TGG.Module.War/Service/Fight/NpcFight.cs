using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Entity;
using TGG.Core.Global;
using TGG.Core.Vo.War;

namespace TGG.Module.War.Service.Fight
{
    public partial class WarFight
    {
        public WarFight(int baseid, IEnumerable<WarRolesLinesVo> attackrolRolesVos, List<tg_war_role> attackroles, Int32 frontid, int morale, int baselandid,string username)
        {

            #region 防守武将

            var basecity = Variable.BASE_WARCITY.FirstOrDefault(q => q.id == baseid);
            Random rd = new Random();
            var zhenId = rd.Next(0, Variable.BASE_WAR_FRONT.Count);
            var baseLand = Variable.BASE_LAND_POOL.FirstOrDefault(m => m.id == baselandid);
            if (baseLand == null) return;
            DefenseFrontId = zhenId;
            Area = GetMapArea(baseLand.landConfig);
            DefenseRoles = GetNpcDefenseRoles(baseLand.ambushConfig, basecity.size, basecity.influence);

            DefenseSoldierCount = DefenseRoles.Sum(m => m.SoldierCount);
            if (!isInitSuccess) { isInitSuccess = false; return; }
            //初始武将防守范围
            DefenseRange = Common.GetInstance().GetDefenseRangeInit(DefenseRoles);

            var basesize = Variable.BASE_WARCITYSIZE.FirstOrDefault(q => q.id == basecity.size);
            if (basesize == null) return;
            GetDoorAndCityInit(basesize.blood, basesize.strong); //城门本丸初始

            #endregion

            #region 进攻数据初始

            AttackRolesInit(attackrolRolesVos, attackroles, morale,username);
            AttackFrontId = frontid;

            #endregion

            WeatherState = GetWeatherStateInit();//初始天气持续回合

            FiveState = GetFiveStateInit(); //初始五常持续回合

            AttackSort = GetRolesSort();//武将出手顺序初始
        }



    }
}
