using System;
using System.Linq;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.XML;
using TGG.SocketServer;
using TGG.Core.Global;
using TGG.Core.Enum.Command;

namespace TGG.Module.War.Service.Map
{
    /// <summary>
    /// 据点更改名字
    /// </summary>
    public class WAR_UPDATE_CITY_NAME : IDisposable
    {
        //private static WAR_UPDATE_CITY_NAME _objInstance;

        ///// <summary>WAR_UPDATE_CITY_NAME单体模式</summary>
        //public static WAR_UPDATE_CITY_NAME GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_UPDATE_CITY_NAME());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 据点更改名字 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("name") || !data.ContainsKey("id")) return null;

            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);
            var name = data.FirstOrDefault(m => m.Key == "name").Value.ToString();

            name = CommonHelper.FilterCityName(name);

            if (Util.IsFilterWords(name)) return CommonHelper.ErrorResult((int)ResultType.DATA_WORDS_ERROR);

            var user = session.Player.User;
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32054");
            if (rule == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            var gold = Convert.ToInt32(rule.value);
            if (user.gold < gold) return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_GOLD_ERROR);
            //name = name + "城";
            if (IsExistByName(name))
                return CommonHelper.ErrorResult(ResultType.WAR_NAME_EXIST);
            var city = session.Player.War.City;

            if (city == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);
            if (city.base_id != id) return CommonHelper.ErrorResult(ResultType.WAR_CITY_NOEXIST);
            // var city = tg_war_city.GetEntityByBaseId(id);
            city.name = name;
            city.Update();
            Common.GetInstance().UpdateGold(user.id, gold, (int)WarCommand.WAR_UPDATE_CITY_NAME);
            var temp = view_war_city.GetEntityById(city.id);
            //var count = tg_war_role.GetRoleCountByCityId(id);
            (new Share.War()).SendCityBuild(temp);
            return Common.GetInstance().BulidData(temp, (int)WarCityCampType.OWN);
        }

        private bool IsExistByName(string name)
        {
            return Variable.WarCityAll.Values.Count(m => m.name == name) > 0;
        }
    }
}
