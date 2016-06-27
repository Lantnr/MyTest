using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using SuperSocket.Common;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.ActivityOpen;
using TGG.SocketServer;

namespace TGG.Share
{
    public class ActivityOpenService
    {
        /// <summary>创建活动实体</summary>
        /// <param name="userid">用户id</param>
        /// <param name="isreward">是否领取</param>
        /// <param name="type">活动类型</param>
        /// <param name="typesub">子类型</param>
        /// <param name="value">活动内容基表id</param>
        /// <returns></returns>
        public tg_activity_content CreatModel(Int64 userid, int isreward, int type, int typesub, int value, int baseid)
        {
            return new tg_activity_content()
            {
                userid = userid,
                is_reward = isreward,
                type = type,
                typesub = typesub,
                value = value,
                baseid = baseid
            };
        }


        #region 精彩活动
        #region  家族
        public void FamilyCountAdd(Int64 userid, int count)
        {
            if (!CheckTime()) return;
            var base1 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33031");
            if (base1 == null) return;
            if (count == Convert.ToInt32(base1.value))
            {
                var a = tg_activity_content.GetEntityByType(userid, (int)ActivityType.GAME, (int)ActivitySubType.开疆拓土,
                    Convert.ToInt32(base1.value));
                if (a.Count == 0)
                {
                    var model = CreatModel(userid, (int)DaMingRewardType.TYPE_CANREWARD, (int)ActivityType.GAME, (int)ActivitySubType.开疆拓土,
                        Convert.ToInt32(base1.value), Convert.ToInt32(base1.id));
                    model.Save();
                }
            }
        }

        public void FamilyLevelAdd(Int64 userid, int level)
        {
            if (!CheckTime()) return;
            var base1 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33034");
            if (base1 == null) return;
            if (level == Convert.ToInt32(base1.value))
            {
                var a = tg_activity_content.GetEntityByType(userid, (int)ActivityType.GAME, (int)ActivitySubType.开疆拓土,
                    Convert.ToInt32(base1.value));
                if (a.Count == 0)
                {
                    var model = CreatModel(userid, (int)DaMingRewardType.TYPE_CANREWARD, (int)ActivityType.GAME, (int)ActivitySubType.开疆拓土,
                        Convert.ToInt32(base1.value), Convert.ToInt32(base1.id));
                    model.Save();
                }
            }
        }


        #endregion

        #region  等级

        public void ActivityLevelAdd(Int64 userid, int level)
        {
            if (!CheckTime()) return;
            var list = new List<int>() { 32, 35, 38 };
            foreach (var item in list)
            {
                var baserule = new BaseRule();
                switch (item)
                {
                    case 32:
                        baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33041");
                        break;
                    case 35:
                        baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33042");
                        break;
                    case 38:
                        baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33043");
                        break;
                }
                if (baserule == null) return;
                GetLevelIntert(baserule, item, level, userid);
            }
        }

        /// <summary>特殊处理，比如使用GM指令跳过时用的</summary>
        public void GetLevelIntert(BaseRule rule, int level, int newlevel, Int64 userid)
        {
            var a = tg_activity_content.GetEntityByType(userid, (int)ActivityType.GAME, (int)ActivitySubType.奋勇争先,
           level);
            if (a.Count == 0)
            {
                if (level <= newlevel)
                {
                    var reward = rule.value;
                    (new Message()).BuildMessagesSend(userid, "奋勇争先奖励", "恭喜获得【奋勇争先】达到" + level + "级活动奖励", reward);
                    var model = CreatModel(userid, (int)DaMingRewardType.TYPE_REWARDED, (int)ActivityType.GAME, (int)ActivitySubType.奋勇争先,
                level, 0);
                    model.Save();
                }
            }
        }
        #endregion

        #region  身份

        public void ActivityIdentityAdd(Int64 userid, int baseid)
        {
            if (!CheckTime()) return;
            var baseiden = Variable.BASE_IDENTITY.FirstOrDefault(m => m.id == baseid);
            if (baseiden == null) return;
            var list = new List<int>() { 2, 3, 4 };
            foreach (var item in list)
            {
                var baserule = new BaseRule();
                switch (item)
                {
                    case 2:
                        baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33038");
                        break;
                    case 3:
                        baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33039");
                        break;
                    case 4:
                        baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33040");
                        break;
                }
                if (baserule == null) return;
                GetIdentityIntert(baserule, item, Convert.ToInt32(baseiden.value), userid);
            }
        }

        /// <summary>特殊处理，比如使用GM指令跳过时用的</summary>
        public void GetIdentityIntert(BaseRule rule, int iden, int newiden, Int64 userid)
        {
            var a = tg_activity_content.GetEntityByType(userid, (int)ActivityType.GAME, (int)ActivitySubType.建功立业,
                  iden);
            if (a.Count == 0)
            {
                if (iden <= newiden)
                {
                    var reward = rule.value;
                    (new Message()).BuildMessagesSend(userid, "建功立业奖励", "恭喜获得【建功立业】达到第" + iden + "阶身份的活动奖励", reward);
                    var model = CreatModel(userid, (int)DaMingRewardType.TYPE_REWARDED, (int)ActivityType.GAME, (int)ActivitySubType.建功立业,
                      iden, 0);
                    model.Save();
                }
            }
        }

        #endregion


        #endregion

        #region 荣誉

        #region  等级

        public void LevelAdd(Int64 userid, int level)
        {
            if (!CheckIsFinish()) return;
            var levels = new List<int>() { 35, 45, 55 };
            foreach (var item in levels)
            {
                if (level >= item)
                {
                    var a = tg_activity_content.GetEntityByType((int)ActivityType.HONOR,
                        (int)ActivityHonorType.SPECIAL_LEVEL,
                        item);
                    if (a.Count == 0)
                    {
                        var model = CreatModel(userid, (int)DaMingRewardType.TYPE_REWARDED, (int)ActivityType.HONOR,
                            (int)ActivityHonorType.SPECIAL_LEVEL,
                            item, 0);
                        model.Save();
                        var baserule = new BaseRule();
                        switch (item)
                        {
                            case 35:
                                baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33007");
                                break;
                            case 45:
                                baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33008");
                                break;
                            case 55:
                                baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33009");
                                break;
                        }
                        if (baserule == null) return;
                        (new Message()).BuildMessagesSend(userid, "荣誉榜奖励", "恭喜获得首个达到" + item + "级活动奖励", baserule.value);
                        SendLevelNoyice(item, userid);
                        ActivityHonorPush();
                    }
                }
            }
        }
        #endregion

        #region  身份

        public void IdentityAdd(Int64 userid, int baseid)
        {
            if (!CheckIsFinish()) return;
            var baseiden = Variable.BASE_IDENTITY.FirstOrDefault(m => m.id == baseid);
            if (baseiden == null) return;
            var idens = new List<int>() { 2, 3, 4, 5, 6, 7 };
            foreach (var item in idens)
            {
                if (Convert.ToInt32(baseiden.value) >= item)
                {
                    var a = tg_activity_content.GetEntityByType((int)ActivityType.HONOR,
                        (int)ActivityHonorType.SPECIAL_IDENTITY,
                       item);
                    if (a.Count == 0)
                    {
                        var model = CreatModel(userid, (int)DaMingRewardType.TYPE_REWARDED, (int)ActivityType.HONOR,
                            (int)ActivityHonorType.SPECIAL_IDENTITY,
                            item, 0);
                        model.Save();
                        var baserule = new BaseRule();
                        switch (item)
                        {
                            case 2:
                                baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33010");
                                break;
                            case 3:
                                baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33011");
                                break;
                            case 4:
                                baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33012");
                                break;
                            case 5:
                                baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33013");
                                break;
                            case 6:
                                baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33014");
                                break;
                            case 7:
                                baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33015");
                                break;
                        }
                        if (baserule == null) return;
                        var reward = baserule.value;
                        if (item == 7)
                            (new Message()).BuildMessagesSend(userid, "荣誉榜奖励", "恭喜获得首个达到大名身份的活动奖励", reward);
                        else
                            (new Message()).BuildMessagesSend(userid, "荣誉榜奖励", "恭喜获得首个达到第" + item + "阶身份的活动奖励",
                                reward);
                        SendIdentityNoyice(item, userid);
                        ActivityHonorPush();
                    }
                }
            }
        }

        #endregion

        #region  生活技能

        public void LifeAdd(tg_role_life_skill life, Int64 userid)
        {
            if (!CheckIsFinish()) return;
            int levellimt = 10;
            var count = GetLifeCount(life, levellimt);
            if (count == 0) return;
            var list = new List<int>() { 1, 3, 5, 10, 16 };
            if (list.Contains(count))
            {
                var a = tg_activity_content.GetEntityByType((int)ActivityType.HONOR, (int)ActivityHonorType.SPECIAL_LIFE, count);
                if (a.Count == 0)
                {
                    var model = CreatModel(userid, (int)DaMingRewardType.TYPE_REWARDED, (int)ActivityType.HONOR, (int)ActivityHonorType.SPECIAL_LIFE,
                     count, 0);
                    model.Save();
                    var baserule = new BaseRule();
                    switch (count)
                    {
                        case 1:
                            baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33016");
                            break;
                        case 3:
                            baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33017");
                            break;
                        case 5:
                            baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33018");
                            break;
                        case 10:
                            baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33019");
                            break;
                        case 16:
                            baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33020");
                            break;
                    }
                    if (baserule == null) return;
                    var reward = baserule.value;
                    if (count == 16)
                        (new Message()).BuildMessagesSend(userid, "荣誉榜奖励", " 恭喜获得首个学习完所有生活技能的活动奖励", reward);
                    else
                    {
                        (new Message()).BuildMessagesSend(userid, "荣誉榜奖励", "恭喜获得" + count + "个任意生活技能达到10级的活动奖励", reward);
                    }
                    SendSkillNoyice(count, userid);
                    ActivityHonorPush();
                }
            }
        }

        public int GetLifeCount(tg_role_life_skill life, int baselevel)
        {
            int i = 0;
            if (life.sub_archer_level == baselevel)
                i++;
            if (life.sub_artillery_level == baselevel)
                i++;
            if (life.sub_ashigaru_level == baselevel)
                i++;
            if (life.sub_build_level == baselevel)
                i++;
            if (life.sub_calculate_level == baselevel)
                i++;
            if (life.sub_craft_level == baselevel)
                i++;
            if (life.sub_eloquence_level == baselevel)
                i++;
            if (life.sub_equestrian_level == baselevel)
                i++;
            if (life.sub_etiquette_level == baselevel)
                i++;
            if (life.sub_martial_level == baselevel)
                i++;
            if (life.sub_medical_level == baselevel)
                i++;
            if (life.sub_mine_level == baselevel)
                i++;
            if (life.sub_ninjitsu_level == baselevel)
                i++;
            if (life.sub_reclaimed_level == baselevel)
                i++;
            if (life.sub_tactical_level == baselevel)
                i++;
            if (life.sub_tea_level == baselevel)
                i++;
            return i;
        }

        #endregion

        #region 战斗技能

        public void FightAdd(List<tg_role_fight_skill> fightlist, Int64 userid, int skillid)
        {
            if (!CheckIsFinish()) return;
            var fs = fightlist.FirstOrDefault(m => m.skill_id == skillid);
            if (fs == null) return;
            GetNinjiaList(fightlist, userid, fs.skill_genre);
            ActivityHonorPush();
        }

        /// <summary>获取忍者众技能集合</summary>
        public void GetNinjiaList(List<tg_role_fight_skill> fightlist, Int64 userid, int type)
        {
            switch (type)
            {
                #region 忍者众
                case (int)RoleGenreType.NINJA_13:
                case (int)RoleGenreType.NINJA_14:
                case (int)RoleGenreType.NINJA_15:
                case (int)RoleGenreType.NINJA_16:
                case (int)RoleGenreType.NINJA_17:
                case (int)RoleGenreType.NINJA_18:
                case (int)RoleGenreType.NINJA_19:
                case (int)RoleGenreType.NINJA_20:
                case (int)RoleGenreType.NINJA_21:
                    FightNinjiaAdd(fightlist, userid, type);
                    break;
                #endregion
                #region 流派
                case (int)RoleGenreType.SCHOOL_2:
                case (int)RoleGenreType.SCHOOL_3:
                case (int)RoleGenreType.SCHOOL_4:
                case (int)RoleGenreType.SCHOOL_5:
                case (int)RoleGenreType.SCHOOL_6:
                case (int)RoleGenreType.SCHOOL_7:
                case (int)RoleGenreType.SCHOOL_8:
                case (int)RoleGenreType.SCHOOL_9:
                case (int)RoleGenreType.SCHOOL_10:
                case (int)RoleGenreType.SCHOOL_11:
                case (int)RoleGenreType.SCHOOL_12:
                    FightGenreAdd(fightlist, userid, type);
                    break;
                #endregion
            }
        }

        /// <summary>流派技能统计</summary>
        /// <param name="fightlist"></param>
        /// <param name="userid"></param>
        public void FightGenreAdd(List<tg_role_fight_skill> fightlist, Int64 userid, int type)
        {
            var genreskill = fightlist.Where(m => m.skill_genre == type).ToList();
            if (genreskill.Count == 7)
            {
                var a1 = tg_activity_content.GetEntityByType((int)ActivityType.HONOR, (int)ActivityHonorType.SPECIAL_FIGHT, 7);
                if (a1.Count == 0)
                {
                    if (CheckSkillLevel(genreskill))
                    {
                        Insert(userid, 7);
                        var base1 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33021");
                        if (base1 == null) return;
                        var reward = base1.value;
                        (new Message()).BuildMessagesSend(userid, "荣誉榜奖励", " 恭喜获得首个学习完任意一个流派所有技能的活动奖励", reward);
                        SendFightNoyice(genreskill.Count, userid);
                    }
                }
            }
        }

        /// <summary>忍者众技能统计</summary>
        /// <param name="fightlist"></param>
        /// <param name="userid"></param>
        public void FightNinjiaAdd(List<tg_role_fight_skill> fightlist, Int64 userid, int type)
        {
            var ninjiaskill = fightlist.Where(m => m.skill_genre == type).ToList();
            if (ninjiaskill.Count == 5)
            {
                var a2 = tg_activity_content.GetEntityByType((int)ActivityType.HONOR, (int)ActivityHonorType.SPECIAL_FIGHT, 5);
                if (a2.Count == 0)
                {
                    if (CheckSkillLevel(ninjiaskill))
                    {
                        Insert(userid, 5);
                        var base2 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33022");
                        if (base2 == null) return;
                        var reward = base2.value;
                        (new Message()).BuildMessagesSend(userid, "荣誉榜奖励", " 恭喜获得首个学习完任意一个忍者众所有技能的活动奖励", reward);
                        SendFightNoyice(ninjiaskill.Count, userid);
                    }
                }
            }
        }

        /// <summary>检测技能等级是否达到最大值</summary>
        public bool CheckSkillLevel(List<tg_role_fight_skill> list)
        {
            foreach (var item in list)
            {
                var b = Variable.BASE_FIGHTSKILL.FirstOrDefault(m => m.id == item.skill_id);
                if (b == null) return false;
                if (item.skill_level != b.levelLimit) return false;
            }
            return true;
        }
        public void Insert(Int64 userid, int count)
        {
            var model = CreatModel(userid, (int)DaMingRewardType.TYPE_REWARDED, (int)ActivityType.HONOR, (int)ActivityHonorType.SPECIAL_FIGHT,
           count, 0);
            model.Save();
        }

        #endregion

        #endregion

        #region  等级大礼包

        public void LevelPacketAdd(Int64 userid, int level)
        {
            var al = tg_activity_content.GetEntityByType((int)ActivityType.PACKAGE, userid);
            if (al.Count == 4)
                return;
            var list = new List<int>() { 30, 40, 50, 60 };

            foreach (var item in list)
            {
                if (level >= item)
                {
                    var a = tg_activity_content.GetEntityPacket((int)ActivityType.PACKAGE, userid, item);
                    if (a.Count == 0)
                    {
                        var baseid = Give(item);
                        if (baseid != 0)
                        {
                            var model = CreatModel(userid, (int)DaMingRewardType.TYPE_CANREWARD,
                                (int)ActivityType.PACKAGE, 0, item, baseid);
                            model.Save();
                            ActivityRecivePush(userid);
                        }
                    }
                }
            }
        }
        public int Give(int baselevel)
        {
            if (baselevel == 30)
                return 33027;
            if (baselevel == 40)
                return 33028;
            if (baselevel == 50)
                return 33029;
            if (baselevel == 60)
                return 33030;
            return 0;
        }


        /// <summary>等级大礼包</summary>
        public List<LevelObject> CreatePacketLevel()
        {
            var levels = new List<LevelObject>
            {
                new LevelObject(1, 30),
                new LevelObject(2, 40),
                new LevelObject(3, 50),
                new LevelObject(4, 60)
            };
            return levels;
        }
        public class LevelObject
        {
            public LevelObject(int index, int level)
            {
                this.index = index;
                this.level = level;
            }

            public int index { get; set; }

            public int level { get; set; }
        }

        #endregion

        #region 充值

        public void PayAdd(Int64 userid, int gold)
        {
            if (!CheckTime())
                return;
            var basepay = GetBase(gold);
            if (basepay != null)
            {
                var user = tg_user.FindByid(userid);
                if (user == null) return;
                var _gold = user.gold;
                //更新User
                var ismax = tg_user.IsGoldMax(user.gold, basepay.give);
                user.gold = ismax;
                user.Save();
                if (Variable.OnlinePlayer.ContainsKey(userid))
                {
                    var session = Variable.OnlinePlayer[userid] as TGGSession;
                    if (session == null) return;
                    session.Player.User = user;
                    new Share.User().REWARDS_API((int)GoodsType.TYPE_GOLD, user);
                    //日志
                    var logdata = string.Format("{0}_{1}_{2}_{3}", "Gold", _gold, basepay.give, user.gold);
                    (new Share.Log()).WriteLog(user.id, (int)LogType.Get, (int)ModuleNumber.API, (int)ApiCommand.API_PAYMENT, "充值", "充值活动回馈", "元宝", (int)GoodsType.TYPE_GOLD, basepay.give, Convert.ToInt64(user.gold), logdata);
                }
                //var s = (int)GoodsType.TYPE_GOLD + "_" + basepay.give;
                //(new Message()).BuildMessagesSend(userid, "充值赠送元宝", " 7天开服活动内充值赠送元宝", s);
            }
        }

        public BaseActivityPay GetBase(int gold)
        {
            var list = Variable.BASE_ACTIVITYPAY;
            foreach (var item in list)
            {
                if (item.pay.Contains("-"))
                {
                    var v = item.pay.Split('-');
                    if (gold >= Convert.ToInt32(v[0]) && gold <= Convert.ToInt32(v[1]))
                        return item;
                }
                else
                {
                    if (gold >= Convert.ToInt32(item.pay))
                        return item;
                }
            }
            return null;
        }

        #endregion

        #region  初始化活动
        /// <summary>
        /// 初始化活动
        /// </summary>
        public void Init()
        {
            var ids = string.Format("33,34,35,36");
            var acs = tg_activity.GetEntityByType(ids);

            var startserver = System.Configuration.ConfigurationManager.AppSettings["startserver"]; //读取配置文件填写的开服时间
            if (startserver == "") return;
            var starttime = new DateTime();
            var isdate = DateTime.TryParse(startserver, out starttime);
            if (!isdate) return;

            if (acs.Count == 0)
            {
                var time = starttime.AddDays(7).Ticks;
                //var time = DateTime.Now.AddMinutes(5).Ticks;
                Variable.AT = time;
                var list = new List<tg_activity>() { new tg_activity() { baseid = 33, time = time }, new tg_activity() { baseid = 34, time = 0 }, new tg_activity() { baseid = 35, time = time }, new tg_activity() { baseid = 36, time = 0 } };
                tg_activity.InsertActivitys(list);
            }
            else
            {
                var a = acs.Where(m => m.baseid == 33 || m.baseid == 35).ToList();
                if (a.Any())
                {
                    var time = starttime.AddDays(7).Ticks;
                    foreach (var item in a)
                    {
                        item.time = time;
                        item.Save();
                    }
                    Variable.AT = time;
                }

            }
        }
        #endregion

        #region 推送

        /// <summary>
        /// 七天过后数据处理
        /// </summary>
        public void ActivityRewardPush()
        {
            var a = tg_activity.GetEntityByType(33);
            if (a == null) return;
            if (a.isfinish == 1 && a.time <= DateTime.Now.Ticks)
                return;
            RankInsert();
            FamilyReward();
            ActivityEndPush();
            PayEnd();
        }

        /// <summary>
        /// 活动推送
        /// </summary>
        public void ActivityEndPush()
        {
            var a = tg_activity.GetEntityByType(33);
            if (a != null)
            {
                if (a.isfinish == 0)
                {
                    new Share.Activity().PushActivity(0, 33);
                    new Share.Activity().PushActivity(0, 35);
                }
            }
        }

        /// <summary>
        /// 等级大礼包领取奖励推送
        /// </summary>
        public void ActivityRecivePush(Int64 user_id)
        {
            if (!Variable.OnlinePlayer.ContainsKey(user_id)) return;
            //向在线玩家推送数据
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return;

            var state = GetPacketStates(user_id);
            var dic = new Dictionary<string, object> { { "state", state } };
            var aso = new ASObject(dic);

            var pv = session.InitProtocol((int)ModuleNumber.ACTIVITY, (int)ActivityCommand.ACTIVITY_RECEIVE_PUSH, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);

        }

        /// <summary>获取等级大礼包的奖励状态</summary>
        public List<int> GetPacketStates(Int64 user_id)
        {
            var list = tg_activity_content.GetEntityByType((int)ActivityType.PACKAGE, user_id);

            var state = new List<int>();
            var idens = CreatePacketLevel();
            for (int i = 1; i < 5; i++)
            {
                var l = idens.FirstOrDefault(m => m.index == i);
                if (l == null) continue;
                var a = list.FirstOrDefault(m => m.value == l.level);
                state.Add(a != null ? a.is_reward : 0);
            }
            return state;
        }

        /// <summary>
        /// 荣誉数据推送
        /// </summary>
        public void ActivityHonorPush()
        {
            var users = tg_user.FindAll().ToList();
            var honors = AllHonor();
            foreach (var item in users)
            {
                var token = new CancellationTokenSource();
                var obj = new ActivityObject
                {
                    userid = item.id,
                };
                Task.Factory.StartNew(m =>
                {
                    var _obj = m as ActivityObject;
                    if (_obj == null) return;
                    ActivityHonorPush(_obj.userid, honors);
                    token.Cancel();
                }, obj, token.Token);
            }

        }

        /// <summary>
        /// 活动结束推送
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="type">活动类型</param>
        public void ActivityHonorPush(Int64 user_id, List<SuccessfulVo> list)
        {
            if (!Variable.OnlinePlayer.ContainsKey(user_id)) return;
            //向在线玩家推送数据
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return;
            var dic = new Dictionary<string, object> { { "successfulVo", list } };
            var aso = new ASObject(dic);

            var pv = session.InitProtocol((int)ModuleNumber.ACTIVITY, (int)ActivityCommand.ACTIVITY_SPECIAL_PUSH, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary>
        /// 线程对象
        /// </summary>
        class ActivityObject
        {
            public Int64 userid { get; set; }

            public int type { get; set; }
        }
        #endregion

        #region  发送公告
        /// <summary>
        ///等级发送系统公告
        /// </summary>
        /// <param name="type"></param>
        private void SendLevelNoyice(int level, Int64 userid)
        {
            var baseid = 0;
            switch (level)
            {
                case 35:
                    {
                        baseid = 100023;
                    } break;
                case 45:
                    {
                        baseid = 100024;
                    } break;
                case 55:
                    {
                        baseid = 100025;
                    }
                    break;
            }
            SendNotice(userid, baseid);
        }

        /// <summary>
        ///身份发送系统公告
        /// </summary>
        private void SendIdentityNoyice(int value, Int64 userid)
        {
            var baseid = 0;
            switch (value)
            {
                case 2:
                    {
                        baseid = 100026;
                    } break;
                case 3:
                    {
                        baseid = 100027;
                    } break;
                case 4:
                    {
                        baseid = 100028;
                    }
                    break;
                case 5:
                    {
                        baseid = 100029;
                    } break;
                case 6:
                    {
                        baseid = 100030;
                    } break;
                case 7:
                    {
                        baseid = 100031;
                    }
                    break;
            }
            SendNotice(userid, baseid);
        }

        /// <summary>
        ///生活技能发送系统公告
        /// </summary>
        private void SendSkillNoyice(int count, Int64 userid)
        {
            var baseid = 0;
            switch (count)
            {
                case 1:
                    {
                        baseid = 100032;
                    } break;
                case 3:
                    {
                        baseid = 100033;
                    } break;
                case 5:
                    {
                        baseid = 100034;
                    }
                    break;
                case 10:
                    {
                        baseid = 100035;
                    } break;
                case 16:
                    {
                        baseid = 100036;
                    }
                    break;
            }
            SendNotice(userid, baseid);
        }

        /// <summary>
        ///战斗技能发送系统公告
        /// </summary>
        private void SendFightNoyice(int count, Int64 userid)
        {
            var baseid = 0;
            switch (count)
            {
                case 7:
                    {
                        baseid = 100037;
                    }
                    break;
                case 5:
                    {
                        baseid = 100038;
                    }
                    break;
            }
            //new Share.Notice().TrainingPlayer(baseid, content);
            SendNotice(userid, baseid);
        }

        public void SendNotice(Int64 userid, int baseid)
        {
            var user = tg_user.FindByid(userid);
            if (user == null) return;
            var chat = new Share.Chat();
            List<ASObject> aso;
            aso = new List<ASObject> { chat.BuildData((int)ChatsASObjectType.PLAYERS, null, userid, user.player_name, null) };
            var list = Variable.OnlinePlayer.Keys;
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                var model = item;
                System.Threading.Tasks.Task.Factory.StartNew(m =>
                {
                    var userId = Convert.ToInt64(model);
                    if (!Variable.OnlinePlayer.ContainsKey(userId)) return;
                    var session = Variable.OnlinePlayer[userId];
                    new Share.Chat().SystemChatSend(session, aso, baseid);
                    token.Cancel();
                }, model, token.Token);
            }
        }
        #endregion

        #region  等级/身份排名奖励
        public void GiveReward(List<view_ranking_exp> explist, List<view_ranking_honor> honorlist)
        {
            for (int i = 1; i < 4; i++)
            {
                var user = explist.FirstOrDefault(m => m.ranks == i);
                if (user != null)
                {
                    var bl = CreateLevel();
                    var b = bl.FirstOrDefault(m => m.index == i);
                    if (b == null) return;
                    (new Message()).BuildMessagesSend(user.id, "勇夺桂冠奖励", " 恭喜您获得【勇夺桂冠】活动等级第" + i + "的奖励。", b.reward);
                }
            }

            for (int i = 1; i < 4; i++)
            {
                var user = honorlist.FirstOrDefault(m => m.ranks == i);
                if (user != null)
                {
                    var bl = CreateIdentity();
                    var b = bl.FirstOrDefault(m => m.index == i);
                    if (b == null) return;
                    (new Message()).BuildMessagesSend(user.id, "出人头地奖励", " 恭喜您获得【出人头地】活动身份第" + i + "的奖励。", b.reward);
                }
            }
        }

        /// <summary>等级排名奖励</summary>
        public List<RankObject> CreateLevel()
        {
            var base1 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33001");
            var base2 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33002");
            var base3 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33003");
            if (base1 == null || base2 == null || base3 == null) return new List<RankObject>();

            var levels = new List<RankObject>();
            levels.Add(new RankObject(1, base1.value));
            levels.Add(new RankObject(2, base2.value));
            levels.Add(new RankObject(3, base3.value));
            return levels;
        }

        /// <summary>身份排名奖励</summary>
        public List<RankObject> CreateIdentity()
        {
            var base1 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33004");
            var base2 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33005");
            var base3 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33006");
            if (base1 == null || base2 == null || base3 == null) return new List<RankObject>();

            var levels = new List<RankObject>();
            levels.Add(new RankObject(1, base1.value));
            levels.Add(new RankObject(2, base2.value));
            levels.Add(new RankObject(3, base3.value));
            return levels;
        }

        /// <summary>
        ///身份排名及发放奖励
        /// </summary>
        public void RankInsert()
        {
            var a = tg_activity.GetEntityByType(33);
            if (a == null) return;
            if (a.isfinish == 1) return;
            var explist = view_ranking_exp.GetEntityList(3);
            var honorlist = view_ranking_honor.GetEntityList(3);
            if (!explist.Any() || !honorlist.Any())
                return;
            GiveReward(explist, honorlist);
        }
        public class RankObject
        {
            public RankObject(int index, string reward)
            {
                this.index = index;
                this.reward = reward;
            }

            public int index { get; set; }

            public string reward { get; set; }
        }

        #endregion

        #region  家族奖励

        /// <summary>
        /// 家族奖励
        /// </summary>
        public void FamilyReward()
        {
            var a = tg_activity.GetEntityByType(33);
            if (a == null) return;
            if (a.isfinish == 1) return;
            var familylevels = tg_activity_content.GetEntityByType((int)ActivityType.GAME, (int)ActivitySubType.开疆拓土, 2);//等级
            if (familylevels.Any())
            {
                var rule1 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33035");
                var rule2 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33036");
                if (rule1 == null || rule2 == null)
                    return;
                FamilyReward(familylevels, rule1.value, rule2.value, 2);

            }
            var familycount = tg_activity_content.GetEntityByType((int)ActivityType.GAME, (int)ActivitySubType.开疆拓土, 20);//人数
            if (familycount.Any())
            {
                var rule1 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33032");
                var rule2 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33033");
                if (rule1 == null || rule2 == null)
                    return;
                FamilyReward(familycount, rule1.value, rule2.value, 1);

            }
        }

        /// <summary>
        /// 家族等级奖励
        /// </summary>
        /// <param name="list"></param>
        /// <param name="reward1">督长奖励</param>
        /// <param name="reward2">成员奖励</param>
        /// <param name="type">1：人数 2：等级</param>
        public void FamilyReward(List<tg_activity_content> list, string reward1, string reward2, int type)
        {
            var fids = new List<Int64>();
            foreach (var item in list)
            {
                var family = tg_family.GetEntityByChief(item.userid);
                if (family == null)
                    continue;
                Message(item.userid, reward1, type);
                var members = tg_family_member.GetAllById(family.id);
                var ml = members.Where(m => m.userid != item.userid).ToList();
                if (ml.Any())
                {
                    MembersPush(ml, reward2, type);
                }
                fids.Add(family.id);
            }
            FamilyProcess(fids, type);
        }

        /// <summary>
        /// 家族达到人数等级没发奖励处理
        /// </summary>
        /// <param name="fids">已奖励的家族id</param>
        /// <param name="type"></param>
        public void FamilyProcess(List<Int64> fids, int type)
        {
            BaseRule baserule;
            List<tg_family> fs;
            var familys = tg_family.FindAll().ToList();
            if (type == 1)
            {
                baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33031");
                if (baserule == null) return;
                fs = familys.Where(m => m.number >= Convert.ToInt32(baserule.value)).ToList();
            }
            else
            {
                baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33034");
                if (baserule == null) return;
                fs = familys.Where(m => m.family_level >= Convert.ToInt32(baserule.value)).ToList();
            }
            if (fs.Any())
            {
                var list = fs.Where(item => !fids.Contains(item.id)).ToList();
                if (list.Any())
                {
                    BaseRule rule1;
                    BaseRule rule2;
                    if (type == 1)
                    {
                        rule1 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33032");
                        rule2 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33033");
                    }
                    else
                    {
                        rule1 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33035");
                        rule2 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "33036");
                    }
                    if (rule1 == null || rule2 == null) return;
                    foreach (var item in list)
                    {
                        Message(item.userid, rule1.value, type);
                        var members = tg_family_member.GetAllById(item.id);
                        var ml = members.Where(m => m.userid != item.userid).ToList();
                        if (ml.Any())
                        {
                            MembersPush(ml, rule2.value, type);
                        }
                    }
                }
            }
        }

        public void Message(Int64 userid, string reward, int type)
        {
            if (type == 1)
                (new Message()).BuildMessagesSend(userid, " 开疆拓土奖励", " 恭喜您获得家族人数达到20人以上。", reward);
            else
            {
                (new Message()).BuildMessagesSend(userid, " 开疆拓土奖励", " 恭喜您获得家族等级达到2级人以上。", reward);
            }
        }

        /// <summary>
        /// 家族奖励发送
        /// </summary>
        public void MembersPush(List<tg_family_member> list, string reward, int type)
        {
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                var obj = new FamilyObject
                {
                    userid = item.userid,
                    reward = reward,
                    type = type,
                };
                Task.Factory.StartNew(m =>
                {
                    var _obj = m as FamilyObject;
                    if (_obj == null) return;
                    Message(_obj.userid, _obj.reward, _obj.type);
                    token.Cancel();
                }, obj, token.Token);
            }

        }

        /// <summary>
        /// 线程对象
        /// </summary>
        class FamilyObject
        {
            public Int64 userid { get; set; }

            public string reward { get; set; }

            public int type { get; set; }
        }

        #endregion

        #region 检测活动是否结束

        /// <summary>
        /// 检测荣誉活动是否结束
        /// </summary>
        public bool CheckIsFinish()
        {
            var a = tg_activity.GetEntityByType(36);
            if (a == null)
                return false;
            if (a.isfinish == 1)
                return false;
            return true;
        }

        /// <summary>
        /// 检测是否是7天开服活动时间
        /// </summary>
        /// <returns></returns>
        public bool CheckTime()
        {
            var a = tg_activity.GetEntityByType(33);
            if (a == null)
            {
                return false;
            }
            if (a.isfinish == 1 && a.time <= DateTime.Now.Ticks)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新充值、精彩活动信息
        /// </summary>
        public void PayEnd()
        {
            var ids = string.Format("33,35");
            var list = tg_activity.GetEntityByType(ids);
            if (list.Any())
            {
                foreach (var item in list)
                {
                    if (item.isfinish == 0)
                    {
                        item.isfinish = 1;
                        item.Save();
                    }
                }
            }
        }

        #endregion

        #region   获取荣誉所有名单


        /// <summary> 达特定等级的玩家 </summary>
        public List<SuccessfulVo> AllHonor()
        {
            var lists = tg_activity_content.GetEntityBySubType((int)ActivityType.HONOR, GetString());
            var vos = new List<SuccessfulVo>();
            if (lists.Any())
            {
                var users = tg_user.FindAll().ToList();
                vos.AddRange(GetLevel(lists, users));
                vos.AddRange(GetIdevtity(lists, users));
                vos.AddRange(GetLifeSkill(lists, users));
                vos.AddRange(GetFightSkill(lists, users));
            }
            return vos;
        }

        public string GetString()
        {
            var types = string.Format("{0},{1},{2},{3}", (int)ActivityHonorType.SPECIAL_LEVEL, (int)ActivityHonorType.SPECIAL_IDENTITY, (int)ActivityHonorType.SPECIAL_FIGHT, (int)ActivityHonorType.SPECIAL_LIFE);
            return types;
        }

        public List<SuccessfulVo> GetLevel(List<tg_activity_content> lists, List<tg_user> users)
        {
            var list = lists.Where(m => m.typesub == (int)ActivityHonorType.SPECIAL_LEVEL).ToList();
            if (!list.Any()) { return NotUsers((int)ActivityHonorType.SPECIAL_LEVEL); }
            var vos = new List<SuccessfulVo>();
            var levels = CreateBaseLevel();
            for (int i = 1; i < 4; i++)
            {
                var l = levels.FirstOrDefault(m => m.index == i);
                if (l == null) continue;
                var a = list.FirstOrDefault(m => m.value == l.level);
                if (a != null)
                {
                    var u = users.FirstOrDefault(m => m.id == a.userid);
                    if (u != null)
                        vos.Add(EntityToVo.ToSuccessVoVo(u.id, 1, i, u.player_name));
                }
                else
                {
                    vos.Add(EntityToVo.ToSuccessVoVo(0, 1, i, null));
                }
            }
            return vos;
        }

        /// <summary>
        /// 特定身份
        /// </summary>
        /// <param name="lists"></param>
        /// <returns></returns>
        public List<SuccessfulVo> GetIdevtity(List<tg_activity_content> lists, List<tg_user> users)
        {
            var list = lists.Where(m => m.typesub == (int)ActivityHonorType.SPECIAL_IDENTITY).ToList();
            if (!list.Any())
                return NotUsers((int)ActivityHonorType.SPECIAL_IDENTITY);
            var vos = new List<SuccessfulVo>();
            var idens = CreateBaseIdentity();
            for (int i = 1; i < 7; i++)
            {
                var l = idens.FirstOrDefault(m => m.index == i);
                if (l == null) continue;
                var a = list.FirstOrDefault(m => m.value == l.level);
                if (a != null)
                {
                    var u = users.FirstOrDefault(m => m.id == a.userid);
                    if (u != null)
                        vos.Add(EntityToVo.ToSuccessVoVo(u.id, 2, i, u.player_name));
                }
                else
                {
                    vos.Add(EntityToVo.ToSuccessVoVo(0, 2, i, null));
                }
            }
            return vos;
        }

        /// <summary>
        /// 特定技能
        /// </summary>
        /// <param name="lists"></param>
        /// <returns></returns>
        public List<SuccessfulVo> GetLifeSkill(List<tg_activity_content> lists, List<tg_user> users)
        {
            var list = lists.Where(m => m.typesub == (int)ActivityHonorType.SPECIAL_LIFE).ToList();
            if (!list.Any())
                return NotUsers((int)ActivityHonorType.SPECIAL_LIFE);
            var vos = new List<SuccessfulVo>();

            var idens = CreateBaseSkill();
            for (int i = 1; i < 6; i++)
            {
                var l = idens.FirstOrDefault(m => m.index == i);
                if (l == null) continue;
                var a = list.FirstOrDefault(m => m.value == l.level);
                if (a != null)
                {
                    var u = users.FirstOrDefault(m => m.id == a.userid);
                    if (u != null)
                        vos.Add(EntityToVo.ToSuccessVoVo(u.id, 3, i, u.player_name));
                }
                else
                {
                    vos.Add(EntityToVo.ToSuccessVoVo(0, 3, i, null));
                }
            }
            return vos;
        }

        /// <summary>
        /// 特定技能
        /// </summary>
        /// <param name="lists"></param>
        /// <returns></returns>
        public List<SuccessfulVo> GetFightSkill(List<tg_activity_content> lists, List<tg_user> users)
        {
            var list = lists.Where(m => m.typesub == (int)ActivityHonorType.SPECIAL_FIGHT).ToList();
            if (!list.Any())
                return NotUsers((int)ActivityHonorType.SPECIAL_FIGHT);
            var vos = new List<SuccessfulVo>();
            var idens = CreateBaseFight();
            for (int i = 6; i < 8; i++)
            {
                var l = idens.FirstOrDefault(m => m.index == i);
                if (l == null) continue;
                var a = list.FirstOrDefault(m => m.value == l.level);
                if (a != null)
                {
                    var u = users.FirstOrDefault(m => m.id == a.userid);
                    if (u != null)
                        vos.Add(EntityToVo.ToSuccessVoVo(u.id, 3, i, u.player_name));
                }
                else
                {
                    vos.Add(EntityToVo.ToSuccessVoVo(0, 3, i, null));
                }
            }
            return vos;
        }


        /// <summary>
        /// 没有玩家的数据
        /// </summary>
        public List<SuccessfulVo> NotUsers(int type)
        {
            var vos = new List<SuccessfulVo>();
            switch (type)
            {
                case (int)ActivityHonorType.SPECIAL_LEVEL:
                    {
                        var levels = CreateBaseLevel();
                        vos = levels.Select(item => EntityToVo.ToSuccessVoVo(0, 1, item.index, null)).ToList();
                        break;
                    }
                case (int)ActivityHonorType.SPECIAL_IDENTITY:
                    {
                        var idens = CreateBaseIdentity();
                        vos = idens.Select(item => EntityToVo.ToSuccessVoVo(0, 2, item.index, null)).ToList();
                        break;
                    }
                case (int)ActivityHonorType.SPECIAL_LIFE:
                    {
                        var skills = CreateBaseSkill();
                        vos = skills.Select(item => EntityToVo.ToSuccessVoVo(0, 3, item.index, null)).ToList();
                        break;
                    }
                case (int)ActivityHonorType.SPECIAL_FIGHT:
                    var fights = CreateBaseFight();
                    vos = fights.Select(item => EntityToVo.ToSuccessVoVo(0, 3, item.index, null)).ToList();
                    break;
            }
            return vos;
        }

        #region 特定条件
        /// <summary>特定等级</summary>
        public List<LevelObject> CreateBaseLevel()
        {
            var levels = new List<LevelObject> { new LevelObject(1, 35), new LevelObject(2, 45), new LevelObject(3, 55) };
            return levels;
        }

        /// <summary>特定身份</summary>
        public List<LevelObject> CreateBaseIdentity()
        {
            var levels = new List<LevelObject>
            {
                new LevelObject(1, 2),
                new LevelObject(2, 3),
                new LevelObject(3, 4),
                new LevelObject(4, 5),
                new LevelObject(5, 6),
                new LevelObject(6, 7)
            };
            return levels;
        }

        /// <summary>特定生活技能等级</summary>
        public List<LevelObject> CreateBaseSkill()
        {
            var levels = new List<LevelObject>
            {
                new LevelObject(1, 1),
                new LevelObject(2, 3),
                new LevelObject(3, 5),
                new LevelObject(4, 10),
                new LevelObject(5, 16)
            };
            return levels;
        }

        /// <summary>特定战斗技能等级</summary>
        public List<LevelObject> CreateBaseFight()
        {
            var levels = new List<LevelObject> { new LevelObject(6, 7), new LevelObject(7, 5) };
            return levels;
        }

        #endregion
        #endregion




    }
}
