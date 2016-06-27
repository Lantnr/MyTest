using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Base;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Scene;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    public partial class Common
    {
        #region 活动关闭

        /// <summary> 美浓活动结束时间到了才调用 </summary>
        public void SiegeEnd()
        {
            var eastBase = Variable.Activity.Siege.BossCondition.FirstOrDefault(m => m.player_camp == (int)CampType.East);
            var westBase = Variable.Activity.Siege.BossCondition.FirstOrDefault(m => m.player_camp == (int)CampType.West);
            if (eastBase == null || westBase == null) return;
            if (eastBase.BaseLife == westBase.BaseLife)
            { PUSH_END.GetInstance().CommandStart(SiegeResultType.DOGFALL); return; }
            if (eastBase.BaseLife > westBase.BaseLife)
                PUSH_END.GetInstance().CommandStart(SiegeResultType.EAST_WIN);
            PUSH_END.GetInstance().CommandStart(SiegeResultType.EAST_LOSE);
        }

        #endregion

        #region 获取数据

        /// <summary> 获取活动玩家数据 </summary>
        /// <param name="userid">当前玩家Id</param>
        /// <param name="camp">当前玩家阵营</param>
        /// <returns>活动玩家实体数据</returns>
        public SiegePlayer GetSiegePlayer(Int64 userid, int camp)
        {
            var playdata = Variable.Activity.Siege.PlayerData.FirstOrDefault(m => m.user_id == userid);
            if (playdata != null) return playdata;
            playdata = BuildSiegePlayer(userid, camp);
            lock (this) { Variable.Activity.Siege.PlayerData.Add(playdata); }
            return playdata;
        }

        /// <summary> 获取玩家活动场景Key </summary>
        /// <param name="userid">玩家Id</param>
        /// <returns>KEY</returns>
        public string GetKey(Int64 userid)
        {
            return (int)ModuleNumber.SIEGE + "_" + userid;
        }

        /// <summary> 修改玩家活动坐标 回到出生地 </summary>
        public view_scene_user UpdateUserScene(tg_user user, int level)
        {
            if (!Variable.Activity.ScenePlayer.ContainsKey(GetKey(user.id)))
            {
                var scence = BuildSceneUser(user, level);
                Variable.Activity.ScenePlayer.AddOrUpdate(GetKey(user.id), scence, (m, n) => n);//添加玩家活动场景数据
            }
            var scenceplay = Variable.Activity.ScenePlayer[GetKey(user.id)];
            if (scenceplay.player_camp == (int)CampType.East)
            {
                scenceplay.X = Variable.Activity.Siege.BaseData.EastBirthX;
                scenceplay.Y = Variable.Activity.Siege.BaseData.EastBirthY;
            }
            else
            {
                scenceplay.X = Variable.Activity.Siege.BaseData.WestBirthX;
                scenceplay.Y = Variable.Activity.Siege.BaseData.WestBirthY;
            }
            return scenceplay;
        }

        /// <summary> 获取基表value数据 </summary>
        /// <param name="id">基表Id</param>
        public string GetBaseData(string id)
        {
            var br = Variable.BASE_RULE.FirstOrDefault(m => m.id == id);
            return br == null ? null : br.value;
        }

        /// <summary> 读取美浓攻略基表 </summary>
        /// <param name="type">操作类型</param>
        public List<BaseSiege> GetBaseSieges(int type)
        {
            return Variable.BASE_SIEGE.Where(m => m.contentType == type).ToList();
        }

        /// <summary>获取生活技能等级</summary>
        /// <param name="type">生活技能类型</param>
        /// <param name="skill">生活技能</param>
        public int GetLifeLevel(int type, tg_role_life_skill skill)
        {
            switch (type)
            {
                case (int)LifeSkillType.ASHIGARU: return skill.sub_ashigaru_level;
                case (int)LifeSkillType.ARTILLERY: return skill.sub_artillery_level;
                case (int)LifeSkillType.ARCHER: return skill.sub_archer_level;
                case (int)LifeSkillType.BUILD: return skill.sub_build_level;
                case (int)LifeSkillType.CALCULATE: return skill.sub_calculate_level;
                case (int)LifeSkillType.CRAFT: return skill.sub_craft_level;
                case (int)LifeSkillType.ELOQUENCE: return skill.sub_eloquence_level;
                case (int)LifeSkillType.EQUESTRIAN: return skill.sub_equestrian_level;
                case (int)LifeSkillType.ETIQUETTE: return skill.sub_etiquette_level;
                case (int)LifeSkillType.MARTIAL: return skill.sub_martial_level;
                case (int)LifeSkillType.MEDICAL: return skill.sub_medical_level;
                case (int)LifeSkillType.MINE: return skill.sub_mine_level;
                case (int)LifeSkillType.NINJITSU: return skill.sub_ninjitsu_level;
                case (int)LifeSkillType.RECLAIMED: return skill.sub_reclaimed_level;
                case (int)LifeSkillType.TACTICAL: return skill.sub_tactical_level;
                case (int)LifeSkillType.TEA: return skill.sub_tea_level;
            }
            return 0;
        }

        /// <summary> 获取武将属性值 </summary>
        /// <param name="type">属性类型</param>
        /// <param name="role">武将信息</param>
        public double GetRoleRibute(int type, tg_role role)
        {
            switch (type)
            {
                case (int)RoleAttributeType.ROLE_FORCE: { return role.base_force; }
                case (int)RoleAttributeType.ROLE_GOVERN: { return role.base_govern; }
                case (int)RoleAttributeType.ROLE_CAPTAIN: { return role.base_captain; }
                case (int)RoleAttributeType.ROLE_BRAINS: { return role.base_brains; }
                case (int)RoleAttributeType.ROLE_CHARM: { return role.base_charm; }
                default: { return 0; }
            }
        }

        /// <summary> 转换 RoleAttributeType </summary>
        public RoleAttributeType ConverType(int type)
        {
            switch (type)
            {
                case (int)RoleAttributeType.ROLE_CAPTAIN: { return RoleAttributeType.ROLE_CAPTAIN; }
                case (int)RoleAttributeType.ROLE_FORCE: { return RoleAttributeType.ROLE_FORCE; }
                case (int)RoleAttributeType.ROLE_BRAINS: { return RoleAttributeType.ROLE_BRAINS; }
                case (int)RoleAttributeType.ROLE_CHARM: { return RoleAttributeType.ROLE_CHARM; }
                case (int)RoleAttributeType.ROLE_GOVERN: { return RoleAttributeType.ROLE_GOVERN; }
                default: { return RoleAttributeType.UNKNOWN; }
            }
        }

        #endregion

        #region 数据验证

        /// <summary> 获取美浓活动场景玩家（除自己外） </summary>
        /// <param name="userid">要除去的用户</param>
        public List<view_scene_user> GetOtherSceneUsers(Int64 userid)
        {
            return Variable.Activity.ScenePlayer.Values.Where(
                 m => m.model_number == (int)ModuleNumber.SIEGE && m.user_id != userid).ToList();
        }

        /// <summary> 验证玩家是否在活动中 </summary>
        /// <param name="userid">要验证的用户Id</param>
        public bool IsActivities(Int64 userid)
        {
            return Variable.Activity.ScenePlayer.ContainsKey(GetKey(userid));
        }

        /// <summary> 验证玩家坐标是否在附近 </summary>
        /// <param name="xy">坐标</param>
        /// <param name="scene">玩家坐标信息</param>
        public bool IsCoorPoint(string[] xy, view_scene_user scene)
        {
            if (scene == null) return false;
            var xmin = Convert.ToInt32(xy[0]) - 150;
            var xmax = Convert.ToInt32(xy[0]) + 150;
            var ymin = Convert.ToInt32(xy[1]) - 150;
            var ymax = Convert.ToInt32(xy[1]) + 150;
            if (!(xmin <= scene.X && scene.X <= xmax)) return false;
            if (!(ymin <= scene.Y && scene.Y <= ymax)) return false;
            return true;
        }

        #endregion

        #region 推送数据

        /// <summary> 玩家推送 </summary>
        /// <param name="session">session</param>
        /// <param name="aso">组装好的数据</param>
        /// <param name="commandnumber">指令号</param>
        /// <param name="otheruserid">要推送的玩家Id</param>
        public void PushPv(TGGSession session, ASObject aso, int commandnumber, decimal otheruserid)
        {
            var key = string.Format("{0}_{1}_{2}", (int)ModuleNumber.SIEGE, commandnumber, otheruserid);
            session.SPM.AddOrUpdate(key, aso, (m, n) => aso);
        }

        /// <summary> 玩家推送 </summary>
        /// <param name="session">session</param>
        /// <param name="aso">组装好的数据</param>
        /// <param name="commandnumber">指令号</param>
        public void PushPv(TGGSession session, ASObject aso, int commandnumber)
        {
            var key = string.Format("{0}_{1}", (int)ModuleNumber.SIEGE, commandnumber);
            session.SPM.AddOrUpdate(key, aso, (m, n) => aso);
        }

        /// <summary>推送协议</summary>
        public void TrainingSiegeEndSend(TGGSession session, ASObject data, int type)
        {
            var pv = session.InitProtocol((int)ModuleNumber.SIEGE, type, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }

        #endregion

        #region 组装数据

        /// <summary> 组装玩家活动实体 </summary>
        private SiegePlayer BuildSiegePlayer(Int64 userid, int camp)
        {
            return new SiegePlayer()
            {
                fame = 0,
                count = 0,
                //isSiege = true,
                user_id = userid,
                player_camp = camp,
                state = (int)SiegePlayerType.EXIT_DEFEND,
                time = Convert.ToDouble((DateTime.Now.Ticks - 621355968000000000) / 10000 - 5000),
                gatetime = Convert.ToDouble((DateTime.Now.Ticks - 621355968000000000) / 10000 - 10000),
            };
        }

        /// <summary> 组装玩家场景数据实体 </summary>
        public view_scene_user BuildSceneUser(tg_user user, int level)
        {
            return new view_scene_user
            {
                player_camp = user.player_camp,
                model_number = (int)ModuleNumber.SIEGE,
                player_name = user.player_name,
                player_sex = user.player_sex,
                player_vocation = user.player_vocation,
                role_level = level,
                user_id = user.id,
            };
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
            };
            return dic;
        }

        #endregion

    }
}
