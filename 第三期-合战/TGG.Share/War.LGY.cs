using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using TGG.SocketServer;
using TGG.Core.Vo.War;

namespace TGG.Share
{
    public partial class War
    {
        /// <summary>
        /// 更新玩家金币资源
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="cost">花费金币</param>
        /// <param name="cmd">指令号</param>
        ///  <param name="flag">花费标识</param>
        public void UpdateGold(Int64 userid, int cost, int cmd, string flag = "SkyCityGold")
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;

            var user = session.Player.User.CloneEntity();
            var gold = user.gold;
            user.gold = user.gold - cost;

            var s = GetString(cmd);
            var logdata = string.Format("{0}_{1}_{2}_{3}", flag, gold, cost, user.gold);  //元宝消费日志记录
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.WAR, cmd, "合战", s, "元宝", (int)GoodsType.TYPE_GOLD, cost, user.gold, logdata);

            user.Save();
            session.Player.User = user;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);
        }

        public string GetString(int cmd)
        {
            switch (cmd)
            {
                case (int)WarCommand.WAR_MILITARY_TRAN_COMPLETE:
                    return "运输加速";
                case (int)WarCommand.WAR_MILITARY_TRAN_LOCK:
                    return "运输开锁";
                case (int)WarCommand.WAR_SKYCITY_ACCELERATE:
                    return "内政/军事加速";
                case (int)WarCommand.WAR_SKYCITY_UNLOCK:
                    return "内政/军事解锁";
                case (int)WarCommand.WAR_SKYCITY_START:
                    return "内政/军事开始";
                case (int)WarCommand.WAR_COPY_HIRE_SOLDIERS:
                    return "合战副本雇兵";
                case (int)WarCommand.WAR_COPY_MORALE:
                    return "合战副本鼓舞士气";

            }
            return "";
        }

        /// <summary> 更新玩家铜币资源 </summary>
        /// <param name="userid">用户id</param>
        /// <param name="cost">花费铜币数量</param>
        /// <param name="cmd">指令号</param>
        ///  <param name="flag">花费标识</param>
        public void UpdateCoin(Int64 userid, Int64 cost, int cmd, string flag)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;

            var user = session.Player.User.CloneEntity();
            var coin = user.coin;
            user.coin = user.coin - cost;

            var logdata = string.Format("{0}_{1}_{2}_{3}", "Coin", coin, cost, user.coin);
            (new Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.USER, (int)UserCommand.OFFICIAL_UPGRADE, "用户", flag, "金钱", (int)GoodsType.TYPE_COIN, cost, user.coin, logdata);

            user.Save();
            session.Player.User = user;
            (new User()).REWARDS_API((int)GoodsType.TYPE_COIN, session.Player.User);
        }

        ///// <summary> 更新玩家铜币资源 </summary>
        ///// <param name="userid">用户id</param>
        ///// <param name="cost">花费铜币数量</param>
        ///// <param name="cmd">指令号</param>
        ///// <param name="ids">需要推送的其他类型 GoodsType</param>
        ///// <param name="flag">花费标识</param>
        //public void UpdateCoin(Int64 userid, Int64 cost, int cmd, List<int> ids, string flag)
        //{
        //    if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
        //    var session = Variable.OnlinePlayer[userid] as TGGSession;
        //    if (session == null) return;

        //    var user = session.Player.User.CloneEntity();
        //    var coin = user.coin;
        //    user.coin = user.coin - cost;

        //    var logdata = string.Format("{0}_{1}_{2}_{3}", "Coin", coin, cost, user.coin);
        //    (new Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.USER, (int)UserCommand.OFFICIAL_UPGRADE, "用户", flag, "金钱", (int)GoodsType.TYPE_COIN, Convert.ToInt32(cost), user.coin, logdata);
        //    ids.Add((int)GoodsType.TYPE_COIN);

        //    user.Save();
        //    session.Player.User = user;
        //    (new User()).REWARDS_API(ids, session.Player.User);
        //}

        /// <summary>更新据点资源 </summary>
        /// <param name="city"></param>
        /// <param name="state">开发类型</param>
        /// <param name="resource">资源</param>
        /// <returns></returns>
        public tg_war_city GetUpdateCity(tg_war_city city, int state, int resource)
        {
            switch (state)
            {
                case (int)WarRoleStateType.ASSART: city.res_foods += resource; break;
                case (int)WarRoleStateType.BUILDING: city.boom += resource; break;
                case (int)WarRoleStateType.BUILD_ADD: city.strong += resource; break;
                case (int)WarRoleStateType.MINING: city.res_funds += resource; break;
                case (int)WarRoleStateType.PEACE: city.peace += resource; break;
                case (int)WarRoleStateType.LEVY:
                    {
                        city.res_soldier += resource;
                        var res = city.res_morale - ReduceMorale(resource);  //徵兵扣除士气
                        city.res_morale = res <= 0 ? 0 : res;
                    }
                    break;
                case (int)WarRoleStateType.TRAIN:
                    {
                        city.res_morale = city.res_morale + resource + GetCharacterMorale(city.user_id);
                        var basecity = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == city.size);
                        if (basecity == null)            //基表判断
                            return null;
                        if (city.res_morale > basecity.morale)
                            city.res_morale = basecity.morale;

                    }
                    break;
            }
            return city;
        }

        /// <summary>根据玩家势力验证是否添加势力效果值</summary>
        public int GetCharacterMorale(Int64 userId)
        {
            int morale = 0;
            var user = tg_user.GetUsersById(userId);
            if (user == null) return morale;
            if (user.player_influence != (int)InfluenceType.ZhiTian) return morale;
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32065");
            if (rule == null) return morale;
            morale = Convert.ToInt32(rule.value);
            return morale;
        }

        /// <summary>减少士气值</summary>
        /// <param name="count">徵兵数量</param>
        public int ReduceMorale(int count)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32032");
            if (rule == null) return 0;
            var temp = rule.value;
            temp = temp.Replace("n", count.ToString("0.00"));
            var value = CommonHelper.EvalExpress(temp);
            var cost = Convert.ToInt32(value);
            return cost;
        }

        /// <summary> 内政 军事完成推送资源更新</summary>
        public void ResourcePush(int baseid, tg_war_role role)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "WAR_SKYCITY_PUSH", "内政 军事完成推送");
#endif

            if (!Variable.OnlinePlayer.ContainsKey(role.user_id)) return;
            var session = Variable.OnlinePlayer[role.user_id] as TGGSession;
            if (session == null) return;

            if (session.Player.War.PlayerInCityId != role.station) return;
            SendCity(baseid, role.user_id);   //推送据点数据更新  

            if (session.Player.War.Status == 0) return;
            //推送天守阁数据更新
            var skyVo = EntityToVo.ToSkyCityVo(role);
            var data = new ASObject(new Dictionary<string, object> { { "skyCity", skyVo } });
            var pv = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.WAR_SKYCITY_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }

        /// <summary> 推送据点信息 </summary>
        /// <param name="baseid">要推送的用户的据点基表Id</param>
        /// <param name="userid">要推送的用户id</param>
        public void SendCity(int baseid, Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            var city = view_war_city.GetEntityByUserId(userid, baseid);
            if (city == null) return;
            var vo = EntityToVo.ToWarCityVo(city, (int)WarCityCampType.OWN, (int)WarCityOwnershipType.PLAYER, 0);
            var data = BulidData(new List<WarCityVo> { vo });
            var pv = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.PUSH_CITY, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }

        #region 获取武将特性内政增加的效果值
        /// <summary> 获取武将特性内政增加的效果值</summary>
        public int GetCharacterEffect(tg_war_role warrole)
        {
            int value = 0;
            if (warrole.type == (int)WarRoleType.PLAYER)
            {
                var role = tg_role.FindByid(warrole.rid);
                if (role == null)
                    return 0;

                value = CharacterValue(role.character1, warrole.state);
                value += CharacterValue(role.character2, warrole.state);
                value += CharacterValue(role.character3, warrole.state);
            }
            return value;
        }

        public int CharacterValue(int character, int type)
        {
            if (character == 0)
                return 0;
            var basehezhan = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(m => m.id == character);
            if (basehezhan == null) return 0;
            if (basehezhan.effects == null) return 0;
            return Convert.ToInt32(GetCharacterValue(type, basehezhan.effects));
        }

        /// <summary>获取武将特性内政增加的效果值</summary>
        /// <param name="type">武将合战状态</param>
        /// <param name="effect">特性效果值</param>
        /// <returns></returns>
        public double GetCharacterValue(int type, string effect)
        {
            var list = new List<double>();
            switch (type)
            {
                #region

                case (int)WarRoleStateType.ASSART:
                    {
                        list.AddRange(GetValuesa(effect, (int)WarFightEffectType.AssartIncrease));
                    }
                    break;
                case (int)WarRoleStateType.BUILD_ADD:
                    {
                        list.AddRange(GetValuesa(effect, (int)WarFightEffectType.BuildingIncrease));
                    }
                    break;
                case (int)WarRoleStateType.PEACE:
                    {
                        list.AddRange(GetValuesa(effect, (int)WarFightEffectType.PeaceIncrease));
                    }
                    break;
                case (int)WarRoleStateType.MINING:
                    {
                        list.AddRange(GetValuesa(effect, (int)WarFightEffectType.MiningIncrease));
                    }
                    break;
                case (int)WarRoleStateType.TRAIN:
                    {
                        list.AddRange(GetValuesa(effect, (int)WarFightEffectType.TrainIncrease));
                    }
                    break;

                #endregion
            }
            return list.Sum();

        }

        private IEnumerable<double> GetValuesa(string effects, int type)
        {
            var eos = SpritCharacter(effects);
            return eos.Select(m => GetValues(m, type)).ToList();
        }

        /// <summary>切割字符串</summary>
        private IEnumerable<EffectObject> SpritCharacter(string effect)
        {
            var eos = new List<EffectObject>();
            if (String.IsNullOrEmpty(effect)) return eos;
            if (effect.Contains('|'))
            {
                var es = effect.Split('|');
                foreach (var item in es)
                {
                    var eo = new EffectObject();
                    var e = item.Split('_');
                    if (e.Length == 2)
                    {
                        eo.type = Convert.ToInt32(e[0]);
                        eo.value = Convert.ToDouble(e[1]);
                        eos.Add(eo);
                    }

                }
            }
            else
            {
                var eo = new EffectObject();
                var e = effect.Split('_');
                if (e.Length == 2)
                {
                    eo.type = Convert.ToInt32(e[0]);
                    eo.value = Convert.ToDouble(e[1]);
                    eos.Add(eo);
                }
            }
            return eos;
        }

        private double GetValues(EffectObject eo, int type)
        {
            if (eo.type == type)
                return eo.value;
            return 0;
        }

        public class EffectObject
        {
            public int type { get; set; }

            public double value { get; set; }
        }

        #endregion

        #region 内政策略添加的效果值

        /// <summary>获取内政策略添加的效果值</summary>
        /// <param name="userid">用户id</param>
        /// <param name="type">内政策略类型</param>
        /// <returns></returns>
        public double GetTactics(Int64 userid, int type)
        {
            var tactics = tg_war_home_tactics.GetEntityByUserid(userid);
            if (tactics == null) return 0;
            var t = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            if (tactics.end_time <= t)
                return 0;
            var ids = tactics.effect.Split('_');

            var baseeffect = ids.Select(item => Variable.BASE_CELUE.FirstOrDefault(m => m.id == Convert.ToInt32(item))).ToList();
            if (!baseeffect.Any()) return 0;
            var es = new List<double>();
            foreach (var item in baseeffect)
            {
                es.AddRange(GetValuesa(item.effectValue2, type));
            }
            return es.Sum();
        }

        /// <summary>获取内政策略添加的效果值</summary>
        /// <param name="type">内政开发类型</param>
        /// <returns></returns>
        public int GetTacticsValue(Int64 userid, int type)
        {
            int value = 0;
            switch (type)
            {
                #region

                case (int)WarRoleStateType.ASSART:
                    {
                        value = Convert.ToInt32(GetTactics(userid, (int)WarTacticsType.ASSART));
                    }
                    break;
                case (int)WarRoleStateType.BUILD_ADD:
                    {
                        value = Convert.ToInt32(GetTactics(userid, (int)WarTacticsType.BUILD_ADD));
                    }
                    break;
                case (int)WarRoleStateType.PEACE:
                    {
                        value = Convert.ToInt32(GetTactics(userid, (int)WarTacticsType.PEACE));
                    }
                    break;
                case (int)WarRoleStateType.MINING:
                    {
                        value = Convert.ToInt32(GetTactics(userid, (int)WarTacticsType.MINING));
                    }
                    break;
                case (int)WarRoleStateType.BUILDING:
                    {
                        value = Convert.ToInt32(GetTactics(userid, (int)WarTacticsType.BUILDING));
                    }
                    break;
                case (int)WarRoleStateType.LEVY:
                    {
                        value = Convert.ToInt32(GetTactics(userid, (int)WarTacticsType.LEVY));
                    }
                    break;
                #endregion
            }
            return value;

        }

        #endregion

        #region 放火扣除内政开发军粮、军资金

        /// <summary>被放火扣减资源，每半小时计算一次</summary>
        /// <param name="role"></param>
        /// <param name="type">0：正常开发  1：加速</param>
        public void ReduceAdd(tg_war_role role, int type)
        {
            if (role.state == (int)WarRoleStateType.ASSART || role.state == (int)WarRoleStateType.MINING)
            {
                var city = (new Share.War()).GetWarCity(role.station, role.user_id);//查询玩家据点信息
                if (city == null) return;
                var now = (DateTime.Now.Ticks - 621355968000000000) / 10000;
                if (city.fire_time > now)
                {
                    var count = type == 0 ? 1 : role.count + 1;
                    var myfire = Variable.FireInfo.FirstOrDefault(m => m.userid == role.user_id && m.rid == role.rid);
                    if (myfire == null)
                        Variable.FireInfo.Add(new Variable.WarRoleFire() { userid = role.user_id, rid = role.rid, count = count });
                    else
                        myfire.count += count;
                }
            }
        }

        /// <summary>扣除资源处理</summary>
        public void ReduceRes(tg_war_role role)
        {
            if (role.state == (int)WarRoleStateType.ASSART || role.state == (int)WarRoleStateType.MINING)
            {
                var myfire = Variable.FireInfo.FirstOrDefault(m => m.userid == role.user_id && m.rid == role.rid);
                if (myfire == null) return;
                if (myfire.count == 0) return;

                var city = (new Share.War()).GetWarCity(role.station, role.user_id);//查询玩家据点信息
                if (city == null) return;
                var res = GetReduceRes(role);
                res = res * myfire.count;
                switch (role.state)
                {
                    case (int)WarRoleStateType.ASSART: city.res_foods -= res;
                        break;
                    case (int)WarRoleStateType.MINING: city.res_funds -= res; ;
                        break;
                }
                city.Save();
                Variable.FireInfo.Remove(myfire);
                SendNotice(role.user_id, 100039);
            }
        }


        /// <summary>获取减少的军粮资源</summary>
        public int GetReduceRes(tg_war_role role)
        {
            var ruleas = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32113");
            var rulemi = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32114");
            if (ruleas == null || rulemi == null) return 0;
            var res = GetTactics(role.user_id, (int)WarTacticsType.FIRE);
            switch (role.state)
            {
                case (int)WarRoleStateType.ASSART:
                    res += Convert.ToInt32(ruleas.value);
                    break;
                case (int)WarRoleStateType.MINING:
                    res += Convert.ToInt32(rulemi.value);
                    break;
            }
            return Convert.ToInt32(res);
        }

        public void SendNotice(Int64 userid, int baseid)
        {
            var user = tg_user.FindByid(userid);
            if (user == null) return;
            var chat = new Share.Chat();
            var aso = new List<ASObject> { chat.BuildData((int)ChatsASObjectType.PLAYERS, null, userid, user.player_name, null) };
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid];
            new Share.Chat().SystemChatSend(session, aso, baseid);
        }
        #endregion
    }
}
