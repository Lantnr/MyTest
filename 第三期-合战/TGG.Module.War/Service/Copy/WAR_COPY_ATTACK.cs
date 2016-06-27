using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.War;
using TGG.Module.War.Service.Fight;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Copy
{
    /// <summary>
    /// 出阵
    /// </summary>
    public class WAR_COPY_ATTACK : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~WAR_COPY_ATTACK()
        {
            Dispose();
        }

        #endregion

        //private static WAR_COPY_ATTACK _objInstance;

        ///// <summary>WAR_COPY_ATTACK单体模式</summary>
        //public static WAR_COPY_ATTACK GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_COPY_ATTACK());
        //}

        /// <summary> 出阵 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var planId = session.Player.War.planId;
            if (planId == 0) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);

            var _id = data.ContainsKey("id") ? data["id"] : null;
            var _zhenId = data.ContainsKey("zhenId") ? data["zhenId"] : null;
            var _list = data.ContainsKey("list") ? data["list"] : null;
            if (_id == null || _zhenId == null || _list == null)
                return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);

            var id = Convert.ToInt32(_id);
            var zhenId = Convert.ToInt32(_zhenId);
            var objs = _list as object[];
            var list = new List<WarRolesLinesVo>();
            var userid = session.Player.User.id;
            var level = session.Player.Role.Kind.role_level;
            var username = session.Player.User.player_name;

            var baseWarCopy = Variable.BASE_WAR_COPY.FirstOrDefault(m => m.id == id);
            if (baseWarCopy == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            if (level < baseWarCopy.level) return CommonHelper.ErrorResult(ResultType.WAR_LEVEL_ERROR);

            var warroles = new List<tg_war_role>();
            var result = BuildWarRole(objs, userid, ref list, ref warroles);
            if (result != ResultType.SUCCESS) return CommonHelper.ErrorResult(result);
            if (!warroles.Any()) return CommonHelper.ErrorResult(ResultType.WAR_COPY_ROLE_ERROR);

            var count = warroles.Sum(m => m.army_soldier);
            var model = tg_war_copy.GetFindByUserId(userid);
            var temp = tg_war_copy_count.GetEntityBySceneId(userid, baseWarCopy.sceneId);
            if (model == null || temp == null) return CommonHelper.ErrorResult(ResultType.WAR_COPY_SOLDIER_NULL);
            if (count <= 0) return CommonHelper.ErrorResult(ResultType.WAR_COPY_SOLDIER_COUNT_ERROR);
            if (temp.count <= 0) return CommonHelper.ErrorResult(ResultType.WAR_COPY_COUNT_ERROR);
            if (model.forces < count) return CommonHelper.ErrorResult(ResultType.WAR_COPY_SOLDIER_COUNT_ERROR);

            var entity = new WarFight(planId, baseWarCopy, list, warroles, zhenId, model.morale, username);
            var fightvo = new FightProcess().GetFightProcess(entity); //调用合战战斗
            if (fightvo == null) return CommonHelper.ErrorResult(ResultType.WAR_FIGHT_ERROR);
            if (fightvo.Item2.result.isWin == 1)   //胜利
            {
                var rewards = GetReward(baseWarCopy.reward); //组装获取奖励
                var str = GetReward(rewards);  //获取邮件通用奖励字符串

                model.score += baseWarCopy.integral;
                model.latest_time = DateTime.Now.Ticks;
                var flag = new RandomSingle().IsTrue(baseWarCopy.dropProbability);
                if (flag) //是否获得筑城令
                {
                    var prop = GetProp(baseWarCopy, userid);
                    if (prop != null)
                    {
                        str = str + "|" + (int)GoodsType.TYPE_PROP + "_" + prop.base_id + "_" + prop.bind + "_" + prop.count;
                        rewards.Add(new RewardVo
                            {
                                goodsType = (int)GoodsType.TYPE_PROP,
                                increases = new List<ASObject> { AMFConvert.ToASObject(EntityToVo.ToPropVo(prop)) },
                            });
                    }
                }
                (new Share.Message()).BuildMessagesSend(userid, "挑战合战副本胜利奖励", "挑战合战副本胜利奖励", str);
                temp.count -= 1;
                temp.Update();
                fightvo.Item2.result.reward = rewards;
            }

            model.forces -= fightvo.Item2.result.myDieCount;
            if (model.forces < 0) model.forces = 0;

            model.Update();
            return BuildData(fightvo.Item2);
        }

        /// <summary> 获取道具 </summary>
        /// <param name="baseWarCopy">合战副本基表</param>
        /// <param name="userid">用户Id</param>
        private static tg_bag GetProp(BaseWarCopy baseWarCopy, Int64 userid)
        {
            var array = baseWarCopy.dropCount.Split('_');
            var arr = baseWarCopy.zhuChengLing.Split('|');
            if (array.Length > 2 || arr.Length == 0) return null;
            var rd = new Random();
            var number = rd.Next(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]) + 1);
            var c = rd.Next(0, arr.Length);
            var propid = Convert.ToInt32(arr[c]);
            var baseProp = Variable.BASE_PROP.FirstOrDefault(m => m.id == propid);
            if (baseProp == null) return null;
            var temp = new tg_bag
            {
                base_id = propid,
                bind = baseProp.bind,
                count = number,
                type = baseProp.type,
                user_id = userid,
            };
            return temp;
        }

        private string GetReward1(string str)
        {
            var reward = new List<string>();
            foreach (var item in str.Split('|'))
            {
                var array = item.Split('_');
                if (array.Length != 2) continue;
                var values = Convert.ToInt32(array[1]);
                switch (Convert.ToInt32(array[0]))
                {
                    case 1:
                        {
                            const int type = (int)GoodsType.TYPE_MERIT;
                            reward.Add(type + "_" + values);
                            break;
                        }//1：战功值_数量
                    case 2:
                        {
                            const int type = (int)GoodsType.TYPE_COIN;
                            reward.Add(type + "_" + values);
                            break;
                        }//2：个人战游戏币_数量
                    default: { break; }
                }
            }
            return string.Join("|", reward);
        }


        private string GetReward(List<RewardVo> list)
        {
            var reward = new List<string>();
            foreach (var item in list)
            {
                reward.Add(item.goodsType + "_" + item.value);
            }
            return string.Join("|", reward);
        }

        /// <summary> 获取奖励Vo </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private List<RewardVo> GetReward(string str)
        {
            var reward = new List<RewardVo>();
            foreach (var item in str.Split('|'))
            {
                var array = item.Split('_');
                if (array.Length != 2) continue;
                var values = Convert.ToInt32(array[1]);
                switch (Convert.ToInt32(array[0]))
                {
                    case 1:
                        {
                            const int type = (int)GoodsType.TYPE_MERIT;
                            reward.Add(new RewardVo { goodsType = type, value = values });
                            break;
                        }//1：战功值_数量
                    case 2:
                        {
                            const int type = (int)GoodsType.TYPE_COIN;
                            reward.Add(new RewardVo { goodsType = type, value = values });
                            break;
                        }//2：个人战游戏币_数量
                    default: { break; }
                }
            }
            return reward;
        }

        /// <summary> 获取奖励 </summary>
        /// <param name="user"></param>
        /// <param name="str"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private tg_user GetReward(tg_user user, string str, ref List<RewardVo> list)
        {
            var coin = 0;
            var ids = new List<int>();
            foreach (var item in str.Split('|'))
            {
                var array = item.Split('_');
                if (array.Length != 2) continue;
                var values = Convert.ToInt32(array[1]);
                switch (Convert.ToInt32(array[0]))
                {
                    case 1:
                        {
                            const int type = (int)GoodsType.TYPE_MERIT;
                            ids.Add(type);
                            user.merit += values;
                            list.Add(new RewardVo { goodsType = type, value = values });
                            break;
                        }//1：战功值_数量
                    case 2:
                        {
                            const int type = (int)GoodsType.TYPE_COIN;
                            coin += values;
                            user.coin = tg_user.IsCoinMax(user.coin, values);
                            ids.Add(type);
                            list.Add(new RewardVo { goodsType = type, value = values });
                            break;
                        }//2：个人战游戏币_数量
                    default: { break; }
                }
            }

            if (!ids.Any()) return user;
            user.Update();
            (new User()).REWARDS_API(ids, user);

            if (coin == 0) return user;
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Coin", coin, coin, user.coin);
            (new Log()).WriteLog(user.id, (int)LogType.Get, (int)ModuleNumber.WAR, (int)WarCommand.WAR_COPY_ATTACK, "用户", "", "金钱", (int)GoodsType.TYPE_COIN, coin, user.coin, logdata);
            return user;
        }

        private ASObject BuildData(WarFightVo vo)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", (int) ResultType.SUCCESS},
                {"fight", vo},
            };
            return new ASObject(dic);
        }

        /// <summary> 组装合战武将实体 </summary>
        /// <param name="list"></param>
        /// <param name="userid"></param>
        /// <param name="lsvo"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        private ResultType BuildWarRole(IEnumerable<object> list, Int64 userid, ref List<WarRolesLinesVo> lsvo, ref  List<tg_war_role> rs)
        {
            var ls = list.OfType<ASObject>();
            var ids = (from item in ls where item.ContainsKey("roleId") select Convert.ToInt64(item["roleId"])).ToList();
            if (!ids.Any()) return ResultType.FRONT_DATA_ERROR;
            var roles = tg_role.GetFindAllByIds(ids);

            foreach (var item in ls)
            {
                var _roleid = item.ContainsKey("roleId") ? item["roleId"] : null;
                var _arms = item.ContainsKey("arms") ? item["arms"] : null;
                var _armsCount = item.ContainsKey("armsCount") ? item["armsCount"] : null;
                if (_roleid == null || _arms == null || _armsCount == null) return ResultType.FRONT_DATA_ERROR;

                var roleid = Convert.ToInt32(_roleid);
                var arms = Convert.ToInt32(_arms);
                var armsCount = Convert.ToInt32(_armsCount);

                var role = roles.FirstOrDefault(m => m.id == roleid);
                if (role == null) return ResultType.ROLE_NOT_EXIST;
                var baseIdenity = Variable.BASE_IDENTITY.FirstOrDefault(m => m.id == role.role_identity);
                if (baseIdenity == null) return ResultType.BASE_TABLE_ERROR;
                if (armsCount > baseIdenity.soldier) return ResultType.WAR_COPY_ARMSCOUNT_MAX;

                var key = String.Format("{0}_{1}_{2}", userid, 2, roleid);
                var model = Variable.WarLines.ContainsKey(key) ? Variable.WarLines[key] : null;
                if (armsCount <= 0) return ResultType.WAR_COPY_SOLDIER_NULL;
                if (model == null || model.lines == null) return ResultType.WAR_COPY_ROLE_LINE_ERROR;
                lsvo.Add(model.lines);

                var temp = new tg_war_role { rid = roleid, army_id = arms, army_soldier = armsCount, user_id = userid };
                rs.Add(temp);
            }
            return ResultType.SUCCESS;
        }
    }
}
