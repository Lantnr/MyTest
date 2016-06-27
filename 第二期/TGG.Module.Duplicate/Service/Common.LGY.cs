using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.Duplicate;
using TGG.Core.Vo.Prop;
using TGG.Core.Vo.User;
using TGG.SocketServer;

namespace TGG.Module.Duplicate.Service
{
    public partial class Common
    {
        #region 数据组装

        public Dictionary<String, Object> BuilData(int result, int shot_count)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "count", shot_count } };
            return dic;
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuilData(int result, dynamic shot_count)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "count", shot_count } };
            return dic;
        }
        #endregion




        #region   最新的

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BulidData(int result, tg_duplicate_checkpoint model, int enemytype, int enemyid, int challengenum, int count, UserInfoVo user, int dekaron)
        {
            var npclist = new List<int>();
            if (model != null)
            {
                if (model.npcids != null)
                    npclist = GetNpcList(model);
            }
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"towerpass", model==null?null:EntityToVo.ToTowerPassVo(model,enemytype,enemyid,challengenum,npclist)},
                {"count", count},
                {"watchmen", user},
                {"challenge", dekaron},
            };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BulidData(int result, int count, TowerPassVo towerpassvo, UserInfoVo user, int dekaron)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"towerpass", towerpassvo},
                {"count", count},
                {"watchmen", user},
                {"challenge", dekaron},
            };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BulidData(int result, int count, TowerPassVo towerpassvo, UserInfoVo user)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"towerpass", towerpassvo},
                {"count", count},
                {"watchmen", user},
            };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BulidData(int result, tg_duplicate_checkpoint model, int enemytype, int enemyid, int challengenum)
        {
            var npclist = new List<int>();
            if (model != null)
            {
                if (model.npcids != null)
                    npclist = GetNpcList(model);
            }
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"towerpass", model==null?null:EntityToVo.ToTowerPassVo(model,enemytype,enemyid,challengenum,npclist)},
            };
            return dic;
        }


        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BulidData(int result, int userblood, int npcblood)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"userBlood",userblood},
                {"npcBlood",npcblood},
            };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BulidData(int result, int type, int userblood, int npcblood)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"type", type},
                {"userBlood",userblood},
                {"npcBlood",npcblood},
            };
            return dic;
        }

        /// <summary>
        /// 获取npc集合
        /// </summary>
        public List<int> GetNpcList(tg_duplicate_checkpoint model)
        {
            var npclist = new List<int>();
            if (model == null)
                return npclist;
            var towerpass = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == model.site);
            if (towerpass == null)
                return npclist;
            if (towerpass.enemyType == (int)DuplicateEnemyType.FIGHTING)
            {
                npclist = NpcsSplit(model);
            }
            return npclist;
        }

        /// <summary>转换成前端需要的Vo数据</summary>
        public UserInfoVo ConvertUser(tg_user user)
        {
            var family = tg_family_member.GetEntityById(user.id) ?? new tg_family_member { fid = 0 };
            var role = tg_role.GetRoreByUserid(user.id, user.role_id);
            var model = new UserInfoVo
            {
                id = user.id,
                playerName = user.player_name,
                sex = user.player_sex,
                vocation = user.player_vocation,
                area = user.player_influence,
                camp = user.player_camp,
                positionId = user.player_position,
                spirit = user.spirit,
                fame = user.fame,
                growAddCount = role.att_points,
                gold = user.gold,
                coin = user.coin,
                rmb = user.rmb,
                coupon = user.coupon,
                familyId = (int)family.id,
            };
            return model;
        }

        /// <summary>
        /// 切割武将怪物id
        /// </summary>
        /// <param name="model"></param>
        public List<int> NpcsSplit(tg_duplicate_checkpoint model)
        {
            List<int> npclist = new List<int>();
            if (model.npcids.Length > 0)
            {
                if (model.npcids.Contains("_"))
                {
                    var npcids = model.npcids.Split('_');
                    npclist.Add(Convert.ToInt32(npcids[0]));
                    npclist.Add(Convert.ToInt32(npcids[1]));
                    if (npcids.Length == 3)
                        npclist.Add(Convert.ToInt32(npcids[2]));
                }
                else
                {
                    npclist.Add(Convert.ToInt32(model.npcids));
                }
            }
            return npclist;
        }

        /// <summary>刷新怪物</summary>
        public string RefreshNpc(int site)
        {
            var npcs = Variable.BASE_TOWERENEMY.Where(m => m.pass == site).ToList();
            if (!npcs.Any()) return null;
            List<ObjectsDouble> probabilities = new List<ObjectsDouble>();
            foreach (var item in npcs)
            {
                var ob = new ObjectsDouble();
                ob.Name = item.id.ToString();
                ob.Probabilities = item.odds;
                probabilities.Add(ob);
            }
            RandomSingle rs = new RandomSingle();
            var _ob = rs.RandomFun(probabilities);
            var npcid = _ob.Name;
            return npcid;
        }

        /// <summary>刷新怪物</summary>
        public string RefreshNpc(List<BaseTowerEnemy> towerenemys)
        {
            List<ObjectsDouble> probabilities = new List<ObjectsDouble>();
            foreach (var item in towerenemys)
            {
                var ob = new ObjectsDouble();
                ob.Name = item.id.ToString();
                ob.Probabilities = item.odds;
                probabilities.Add(ob);
            }
            RandomSingle rs = new RandomSingle();
            var _ob = rs.RandomFun(probabilities);
            var npcid = _ob.Name;
            return npcid;
        }

        /// <summary>向用户推送更新</summary>
        public void RewardsToUser(tg_user user, int type)
        {
            if (!Variable.OnlinePlayer.ContainsKey(user.id)) return;
            var session = Variable.OnlinePlayer[user.id] as TGGSession;
            if (session == null) return;
            session.Player.User = user;
            (new Share.User()).REWARDS_API(type, session.Player.User);
        }

        /// <summary>获取npc</summary>
        public int GetNpcId(tg_duplicate_checkpoint checkpoint)
        {
            int npcid;
            if (checkpoint.npcids == null)
            {
                npcid = Convert.ToInt32(RefreshNpc(checkpoint.site));
                checkpoint.npcids = npcid.ToString();
                checkpoint.Update();
            }
            else
                npcid = GetNpc(checkpoint);
            return npcid;
        }

        /// <summary>获取当前关卡npcid</summary>
        public int GetNpc(tg_duplicate_checkpoint checkpoint)
        {
            if (checkpoint.npcids != null)
            {
                if (checkpoint.npcids.Contains("_"))
                {
                    var npcids = checkpoint.npcids.Split('_');
                    if (npcids.Length == 2)
                        return Convert.ToInt32(npcids[1]);
                    return Convert.ToInt32(npcids[2]);
                }
                return Convert.ToInt32(checkpoint.npcids);
            }
            return 0;
        }


        /// <summary>下一关卡信息</summary>
        public TowerPassVo TowerPassMessage(int count, ref tg_duplicate_checkpoint checkpoint)
        {
            var site = checkpoint.site;
            var towerpass = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == site);
            if (towerpass == null) return null;
            var nextpass = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.id == towerpass.nextId);
            if (nextpass == null) return null;

            var towervo = new TowerPassVo();
            if (nextpass.enemyType == (int)DuplicateEnemyType.SMALL_GAME)
            {
                checkpoint.site = nextpass.pass;
                towervo = EntityToVo.ToTowerPassVo(checkpoint, nextpass.enemyType, nextpass.enemyId, count, null);
            }
            else
            {
                var npcid = RefreshNpc(nextpass.pass);
                checkpoint.npcids = npcid;
                checkpoint.site = nextpass.pass;
                var npclist = GetNpcList(checkpoint);
                towervo = EntityToVo.ToTowerPassVo(checkpoint, nextpass.enemyType, Convert.ToInt32(npcid),
                            count, npclist);
            }
            return towervo;
        }

        /// <summary>返回最新塔主信息</summary>
        public tg_duplicate_checkpoint BecomeTower(int rolelife, tg_duplicate_checkpoint checkpoint)
        {
            checkpoint.tower_site = checkpoint.site;
            checkpoint.blood = rolelife; //血量回满
            checkpoint.dekaron = (int)DuplicateRightType.HAVERIGHT;
            return checkpoint;
        }

        /// <summary> 进行守护者奖励发放 </summary>
        public void WatchmenReward()
        {
            try
            {
                var towers = tg_duplicate_checkpoint.GetTowers();
                if (!towers.Any()) return;
                var ids = string.Join(",", towers.Select(m => m.user_id).ToArray());
                var ul = tg_user.GetUsersByIds(ids);
                var base_towerpass = Variable.BASE_TOWERPASS;
                if (!base_towerpass.Any()) return;
                foreach (var item in towers)
                {
                    var towerpass = base_towerpass.FirstOrDefault(m => m.pass == item.tower_site);
                    if (towerpass == null) continue;
                    var user = ul.FirstOrDefault(m => m.id == item.user_id);
                    user.fame = tg_user.IsFameMax(user.fame, towerpass.watchmenAward);
                    user.Update();
                    RewardsToUser(user, (int)GoodsType.TYPE_FAME);
                }
            }
            catch
            {
                XTrace.WriteLine("爬塔守护者奖励定时发放错误");
            }
        }
        /// <summary>
        ///  重置时删除爬塔数据
        /// </summary>
        public void CheckPointDelete()
        {
            tg_duplicate_checkpoint.GetUpdate();
        }

        /// <summary>清空当前关卡数据</summary>
        public void CheckpointUpdate(tg_duplicate_checkpoint checkpoint)
        {
            checkpoint.ninjutsu_star = null;
            checkpoint.calculate_star = null;
            checkpoint.npc_tea = 0;
            checkpoint.user_tea = 0;
            checkpoint.user_blood = 0;
            checkpoint.npc_blood = 0;
            checkpoint.select_position = null;
            checkpoint.all_cards = null;
            checkpoint.state = (int)DuplicateClearanceType.CLEARANCE_UNBEGIN;
            checkpoint.Update();
        }

        /// <summary>挑战守护者验证</summary>
        public bool CheckWatchmenRight(int watchstate, tg_duplicate_checkpoint checkpoint)
        {
            if (watchstate == (int)DuplicateTargetType.WATCHMEM &&
                checkpoint.state == (int)DuplicateClearanceType.CLEARANCE_SUCCESS)
            {
                return true;
            }
            return false;
        }

        /// <summary>添加声望日志</summary>
        /// <param name="userid">玩家id</param>
        /// <param name="of">增加前的声望值</param>
        /// <param name="add">增加的值</param>
        /// <param name="nf">增加后的声望值</param>
        /// <param name="command">指令号</param>
        public void LogOperate(Int64 userid, int of, int add, int nf, int command)
        {
            //日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Fame", of, add, nf);
            (new Share.Log()).WriteLog(userid, (int)LogType.Get, (int)ModuleNumber.DUPLICATE, command, logdata);
        }

        #endregion
    }
}
