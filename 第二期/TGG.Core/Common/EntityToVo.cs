using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Appraise;
using TGG.Core.Vo.Business;
using TGG.Core.Vo.Duplicate;
using TGG.Core.Vo.Equip;
using TGG.Core.Vo.Family;
using TGG.Core.Vo.Fight;
using TGG.Core.Vo.Friend;
using TGG.Core.Vo.Games;
using TGG.Core.Vo.Guide;
using TGG.Core.Vo.Messages;
using TGG.Core.Vo.Prop;
using TGG.Core.Vo.Rankings;
using TGG.Core.Vo.Role;
using TGG.Core.Vo.RoleTrain;
using TGG.Core.Vo.Scene;
using TGG.Core.Vo.Skill;
using TGG.Core.Vo.Task;
using TGG.Core.Vo.User;

namespace TGG.Core.Common
{
    /// <summary>
    /// 实体转换Vo公共方法
    /// </summary>
    public class EntityToVo
    {
        /// <summary>玩家信息转换成前端需要的UserInfoVo数据</summary>
        /// <param name="player">玩家信息实体</param>
        /// <returns>前端UserInfoVo</returns>
        public static UserInfoVo ToUserInfoVo(Player player)
        {
            var areas = player.BusinessArea.Select(m => m.area_id).ToList();
            var model = new UserInfoVo
            {
                id = player.User.id,
                playerName = player.User.player_name,
                sex = player.User.player_sex,
                vocation = player.User.player_vocation,
                area = player.User.player_influence,
                camp = player.User.player_camp,
                positionId = player.User.player_position,
                spirit = player.User.spirit,
                fame = player.User.fame,
                growAddCount = player.Role.Kind.att_points,
                gold = player.User.gold,
                coin = player.User.coin,
                rmb = player.User.rmb,
                coupon = player.User.coupon,
                familyId = (int)player.Family.fid,
                rewardState = player.UserExtend.salary_state,
                moduleIds = player.moduleIds,
                vipVo = new VipVo
                {
                    level = player.Vip.vip_level,
                    costGold = player.Vip.vip_gold,
                },
                areas = areas,
                onlineTime = Convert.ToInt32(player.onlinetime),
                fcm = player.UserExtend.fcm,
                //dml = 0,
                dml = player.UserExtend.dml,
            };
            return model;
        }
        /// <summary>好友实体格式化前端数据FriendVo</summary>
        /// <param name="view">好友视图实体</param>
        /// <returns>前端FriendVo</returns>
        public static FriendVo ToFriendVo(view_user_role_friend view)
        {
            return new FriendVo
            {
                id = view.id,
                friendid = view.friend_id,
                level = view.role_level,
                sex = view.player_sex,
                vocation = view.player_vocation,
                isonline = view.isonline,
                identityid = view.role_identity,
                groupState = view.friend_state,
                friendname = view.player_name,

            };
        }

        /// <summary>邮件实体转换成前端需要的MessagesVo</summary>
        /// <param name="m">邮件实体</param>
        /// <returns>前端MessagesVo</returns>
        public static MessagesVo ToMessagesVo(view_messages m)
        {
            var time = CommonHelper.TickToDateTime(m.create_time).ToString("yyyy-MM-dd HH:mm:ss");
            return new MessagesVo()
            {
                id = m.id,
                receive_id = m.receive_id,
                send_id = m.send_id,
                send_playname = m.player_name ?? "系统邮件",
                type = m.type,
                title = m.title,
                contents = m.contents,
                isread = m.isread,
                isattachment = m.isattachment,
                attachment = m.attachment,
                create_time = time,
            };
        }

        /// <summary>tg_goods_item 转换 BusinessGoodsVo</summary>
        /// <param name="goods">tg_goods_item</param>
        /// <returns>前端BusinessGoodsVo</returns>
        public static BusinessGoodsVo ToBusinessGoodsVo(tg_goods_item goods)
        {
            var bg = Variable.GOODS.FirstOrDefault(m => m.goods_id == goods.goods_id && m.ting_id == goods.ting_id);
            if (bg == null) return new BusinessGoodsVo();
            return new BusinessGoodsVo
            {
                id = goods.id,
                baseId = goods.goods_id,
                count = goods.number,
                priceBuy = bg.goods_buy_price,
                priceSell = bg.goods_sell_price,
            };
        }

        /// <summary>tg_goods_business 转换 BusinessGoodsVo</summary>
        /// <param name="goods">tg_goods_business</param>
        /// <returns>前端BusinessGoodsVo</returns>
        public static BusinessGoodsVo ToBusinessGoodsVo(tg_goods_business goods)
        {
            return new BusinessGoodsVo
            {
                id = goods.id,
                baseId = (int)goods.goods_id,
                count = goods.goods_number,
                priceBuy = goods.price,
                priceSell = 0,
            };
        }

        /// <summary>tg_car and List[BusinessGoodsVo] 组装 BusinessCarVo</summary>
        /// <param name="car">tg_car实体</param>
        /// <param name="list">List[BusinessGoodsVo] 实体</param>
        /// <returns>前端BusinessCarVo</returns>
        public static BusinessCarVo ToBusinessCarVo(tg_car car, List<BusinessGoodsVo> list)
        {
            return new BusinessCarVo
            {
                id = car.id,
                baseId = car.car_id,
                destinationId = car.stop_ting_id,
                generalId = car.rid,
                state = car.state,
                stopId = car.start_ting_id,
                time = car.time,
                volume = car.packet,
                goods = list.Any() ? list : null,
            };
        }

        /// <summary> tg_fight_yin 组装 YinVo </summary>
        /// <param name="model">印实体</param>
        /// <returns>前端YinVo</returns>
        public static YinVo ToFightYinVo(tg_fight_yin model)
        {
            return new YinVo
            {
                id = Convert.ToDouble(model.id),
                baseid = model.yin_id,
                level = model.yin_level,
                state = model.state,
            };
        }

        /// <summary> tg_fight_personal and tg_fight_yin 组装 YinVo </summary>
        /// <param name="model">个人战阵实体</param>
        /// <param name="yin">印实体</param>
        /// <returns>前端MatrixVo</returns>
        public static MatrixVo ToFightMatrixVo(tg_fight_personal model, tg_fight_yin yin)
        {
            return new MatrixVo()
            {
                id = Convert.ToDouble(model.id),
                yinVo = yin == null ? null : ToFightYinVo(yin),
                matrix1_rid = model.matrix1_rid,
                matrix2_rid = model.matrix2_rid,
                matrix3_rid = model.matrix3_rid,
                matrix4_rid = model.matrix4_rid,
                matrix5_rid = model.matrix5_rid,
            };
        }

        /// <summary> view_scene_user  组装 ScenePlayerVo </summary>
        /// <param name="model">场景视图实体</param>
        /// <summary> 前端ScenePlayerVo </summary>
        public static ScenePlayerVo ToScenePlayerVo(view_scene_user model)
        {
            return new ScenePlayerVo
            {
                camp = model.player_camp,
                id = model.user_id,
                level = model.role_level,
                name = model.player_name,
                sex = model.player_sex,
                vocation = model.player_vocation,
                x = model.X,
                y = model.Y,
                identityId = model.role_identity,
            };
        }

        /// <summary> tg_bag  组装 EquipVo </summary>
        /// <param name="equip">装备实体</param>
        /// <returns> 前端EquipVo </returns>
        public static EquipVo ToEquipVo(tg_bag equip)
        {
            return new EquipVo()
            {
                id = equip.id,
                baseId = equip.base_id,
                bind = equip.bind,
                state = equip.state,
                att1 = equip.attribute1_type,
                value1 = equip.attribute1_value + equip.attribute1_value_spirit,
                att2 = equip.attribute2_type,
                value2 = equip.attribute2_value + equip.attribute2_value_spirit,
                att3 = equip.attribute3_type,
                value3 = equip.attribute3_value + equip.attribute3_value_spirit,
                lv1 = equip.attribute1_spirit_level,
                spirit1 = equip.attribute1_spirit_value,
                lv2 = equip.attribute2_spirit_level,
                spirit2 = equip.attribute2_spirit_value,
                lv3 = equip.attribute3_spirit_level,
                spirit3 = equip.attribute3_spirit_value,
                isLock1 = equip.attribute1_spirit_lock,
                isLock2 = equip.attribute2_spirit_lock,
                isLock3 = equip.attribute3_spirit_lock,
                sopCount = 5 - equip.baptize_count
            };
        }

        /// <summary> tg_train_role  组装 TrainVo </summary>
        /// <param name="trainrole">武将修炼实体</param>
        /// <returns> 前端TrainVo </returns>
        public static TrainVo ToTrainVo(tg_train_role trainrole)
        {
            if (trainrole == null)
            {
                trainrole = new tg_train_role();
            }
            return new TrainVo()
            {
                id = trainrole.id,
                state = trainrole.state,
                type = trainrole.attribute,
                lv = trainrole.type,
                time = trainrole.time,
            };
        }


        /// <summary> 组装 LifeSkillVo </summary>
        /// <param name="id">编号</param>
        /// <param name="rid">武将主键id</param>
        /// <param name="baseId">技能基表id</param>
        /// <param name="level">技能等级</param>
        /// <param name="costTimer">修炼时间</param>
        /// <param name="progress">技能熟练度</param>
        /// <param name="state">修炼状态</param>
        /// <returns> 前端LifeSkillVo </returns>
        public static LifeSkillVo ToLifeSkillVo(decimal id, decimal rid, decimal baseId, int level, decimal costTimer, int progress, int state)
        {
            return new LifeSkillVo()
            {
                id = (double)id,
                role_id = (int)rid,
                baseId = (double)baseId,
                level = level,
                costTimer = (double)costTimer,
                progress = progress,
                state = state,
            };
        }

        /// <summary>组装FamilyVo</summary>
        /// <param name="model">家族实体</param>
        /// <param name="ranking">排名</param>
        /// <param name="list_member_vo">家族成员实体集合</param>
        /// <param name="userextend">用户拓展实体</param>
        /// <returns>前端FamilyVo</returns>
        public static FamilyVo ToFamilyVo(tg_family model, int ranking, List<FamilyMemberVo> list_member_vo, tg_user_extend userextend)
        {
            return new FamilyVo()
            {
                id = model.id,
                name = model.family_name,
                level = model.family_level,
                rankings = ranking,
                number = model.number,
                resource = model.resource,
                renown = model.renown,
                notice = model.notice,
                clanbadge = model.clanbadge,
                familyMemberArrVo = list_member_vo,
                daySalary = userextend.daySalary,
                dayDonate = userextend.donate,
            };
        }

        /// <summary>组装FamilyListVo</summary>
        /// <param name="model">家族实体</param>
        /// <param name="state">申请家族状态</param>
        /// <param name="chiefname">族长名字</param>
        /// <returns>前端FamilyListVo</returns>
        public static FamilyListVo ToFamilyListVo(tg_family model, int state, string chiefname)
        {
            return new FamilyListVo()
            {
                id = model.id,
                clanbadge = model.clanbadge,
                familyName = model.family_name,
                familyLevel = model.family_level,
                chairmanName = chiefname,
                memberValue = model.number,
                state = state,
            };
        }

        /// <summary> 组装FamilyMemberVo</summary>
        /// <param name="state">在线状态</param>
        /// <param name="model">家族成员视图实体</param>
        /// <param name="debarktime">成员登陆时间</param>
        /// <returns>前端FamilyMemberVo</returns>
        public static FamilyMemberVo ToFamilyMemberVo(int state, view_user_role_family_member model, decimal debarktime)
        {
            List<BaseFamilyPost> list_basepost = Variable.BASE_FAMILYPOST;
            if (!list_basepost.Any()) return new FamilyMemberVo();
            var base_post = list_basepost.FirstOrDefault(m => m.post == model.degree);
            if (base_post == null) return new FamilyMemberVo();

            return new FamilyMemberVo()
            {
                id = model.id,
                userid = model.userid,
                memberName = model.player_name,
                level = model.role_level,
                degree = base_post.id,
                devote = model.devote,
                debarkTime = (double)debarktime,
                state = state,
            };
        }

        /// <summary>组装FamilyLogVo </summary>
        /// <param name="model">家族日志实体</param>
        /// <param name="name">成员名字</param>
        /// <returns>前端FamilyLogVo</returns>
        public static FamilyLogVo ToFamilyLogVo(tg_family_log model, string name)
        {
            return new FamilyLogVo()
            {
                id = model.id,
                userid = (int)model.userid,
                userName = name,
                logBaseId = model.baseid,
                logType = model.type,
                time = model.time,
                familyid = (int)model.fid,
            };
        }

        /// <summary>view_user_role_family_apply 组装 FamilyApplyVo</summary>
        /// <param name="model">家族申请视图实体</param>
        /// <returns>前端FamilyApplyVo</returns>
        public static FamilyApplyVo ToFamilyApplyVo(view_user_role_family_apply model)
        {
            return new FamilyApplyVo()
            {
                id = model.id,
                userid = model.userid,
                userName = model.player_name,
                level = model.role_level,
                time = model.time,
            };
        }

        /// <summary> tg_family and user 组装InviteReceiveVo</summary>
        /// <param name="model">家族实体</param>
        /// <param name="user">用户实体</param>
        /// <returns>前端InviteReceiveVo</returns>
        public static InviteReceiveVo ToInviteReceiveVo(tg_family model, tg_user user)
        {
            return new InviteReceiveVo()
            {
                id = model.id,
                familyId = (int)model.id,
                userName = user.player_name,
                familyName = model.family_name,
                userId = Convert.ToInt32(user.id),
            };
        }

        /// <summary>组装TowerPassVo </summary>
        /// <param name="model">爬塔实体</param>
        /// <param name="enemytype">挑战类型</param>
        /// <param name="enemyid">挑战npcid</param>
        /// <param name="challengenum">挑战次数</param>
        /// <param name="npclist">挑战怪物集合</param>
        /// <returns>前端TowerPassVo</returns>
        public static TowerPassVo ToTowerPassVo(tg_duplicate_checkpoint model, int enemytype, int enemyid, int challengenum, List<int> npclist)
        {
            var towerpass = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == model.site);
            return new TowerPassVo()
            {
                id = model == null ? 1 : model.id,
                baseid = towerpass == null ? 1 : towerpass.id,
                enemyType = enemytype,
                enemyId = enemyid,
                challengeNum = challengenum,
                refreshEnemyId = npclist,
                towerHost = model.tower_site,
            };
        }

        /// <summary>RoleFight实体转换RoleFightVo </summary>
        /// <param name="model">战斗武将实体</param>
        /// <returns>前端RoleFightVo</returns>
        public static RoleFightVo ToFightRoleFightVo(FightRole model)
        {
            return new RoleFightVo
            {
                id = model.id,
                baseId = model.baseId,
                monsterType = model.monsterType,
                mystery = model.mystery != null ? Convert.ToInt32(model.mystery.baseId) : 0,
                cheatCode = model.cheatCode != null ? Convert.ToInt32(model.cheatCode.baseId) : 0,
                damage = model.damage,
                hp = model.hp,
                initHp = model.initHp,
                attack = Convert.ToInt32(model.attack),
                defense = Convert.ToInt32(model.defense),
                hurtIncrease = model.hurtIncrease,
                hurtReduce = model.hurtReduce,
                critProbability = model.critProbability,
                critAddition = model.critAddition,
                dodgeProbability = model.dodgeProbability,
                angerCount = model.angerCount,
                buffVos = model.buffVos,
                buffVos2 = model.buffVos2,
            };
        }

        /// <summary> tg_role_fight_skill 实体转换 SkillVo </summary>
        /// <param name="model">个人战技能实体</param>
        /// <returns>前端SkillVo</returns>
        public static SkillVo ToFightSkillVo(tg_role_fight_skill model)
        {
            return new SkillVo
            {
                id = Convert.ToDouble(model.id),
                baseId = model.skill_id,
                level = model.skill_level,
            };
        }

        /// <summary>
        /// tg_bag实体装换为PropVo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static PropVo ToPropVo(tg_bag model)
        {
            return new PropVo()
            {
                id = model.id,
                baseId = model.base_id,
                bind = model.bind,
                count = model.count,
            };
        }

        /// <summary>tg_role 实体转换 RoleInfoVo</summary>
        /// <param name="model">武将实体</param>
        /// <param name="equips">武将装备集合</param>
        /// <param name="fightskillvos">战斗技能Vo集合</param>
        /// <param name="lifetskillvos">生活技能Vo集合</param>
        /// <param name="genres">可学流派集合</param>
        /// <param name="trainvo">武将修行Vo</param>
        /// <param name="titles">武将称号集合</param>
        /// <returns>前端RoleInfoVo</returns>
        public static RoleInfoVo ToRoleVo(tg_role model, List<double> equips, List<FightSkillVo> fightskillvos, List<LifeSkillVo> lifetskillvos, List<int> genres, TrainVo trainvo, List<double> titles)
        {
            return new RoleInfoVo()
            {
                id = model.id,
                baseId = model.role_id,
                state = model.role_state,
                level = model.role_level,
                experience = model.role_exp,
                power = tg_role.GetTotalPower(model),
                rolePower = model.power,
                identityId = model.role_identity,
                captain = Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_CAPTAIN, model), 2),     //单项属性点总值
                force = Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, model), 2),
                brains = Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_BRAINS, model), 2),
                charm = Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, model), 2),
                govern = Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, model), 2),
                captainBase = Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_CAPTAIN, model), 2),    //固定基础属性点
                forceBase = Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_FORCE, model), 2),
                brainsBase = Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_BRAINS, model), 2),
                charmBase = Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_CHARM, model), 2),
                governBase = Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_GOVERN, model), 2),
                life = model.att_life,
                attack = Math.Round(tg_role.GetTotalAttack(model), 2),
                defense = Math.Round(model.att_defense, 2),
                hurtIncrease = Math.Round(model.att_sub_hurtIncrease, 2),
                hurtReduce = Math.Round(model.att_sub_hurtReduce, 2),
                mysteryId = (int)model.art_mystery,
                cheatCodeId = (int)model.art_cheat_code,
                genre = model.role_genre,
                ninja = model.role_ninja,
                honor = model.role_honor,
                critAddition = Math.Round(tg_role.GetTotalCritAddition(model), 2),
                critProbability = Math.Round(tg_role.GetTotalCritProbability(model), 2),
                dodgeProbability = Math.Round(tg_role.GetTotalDodgeProbability(model), 2),
                mysteryProbability = Math.Round(tg_role.GetTotalMysteryProbability(model), 2),
                equipArray = equips,                 //武将装备集合
                fightSkillArrVo = fightskillvos,   //战斗技能Vo集合
                lifeSkillArrVo = lifetskillvos,       //生活技能Vo集合
                genreTypeArr = genres,            //可学流派，忍者众技能
                trainVo = trainvo,                     //武将修行Vo
                roleTitleIdList = titles,                //武将称号集合
            };
        }

        /// <summary> tg_role_fight_skill 实体转换 FightSkillVo </summary>
        /// <param name="model">个人战技能实体</param>
        /// <returns>前端FightSkillVo</returns>
        public static FightSkillVo ToRoleFightSkillVo(tg_role_fight_skill model)
        {
            return new FightSkillVo()
            {
                id = model.id,
                baseId = model.skill_id,
                level = model.skill_level,
                costTimer = model.skill_time,
                genre = model.skill_genre,
                state = model.skill_state,
            };
        }


        /// <summary>
        /// 装换成主线任务vo
        /// </summary>
        /// <param name="task">tg_task实体</param>
        public static TaskVo ToTaskVo(tg_task task)
        {
            return new TaskVo()
            {
                id = Convert.ToDouble(task.id),
                state = task.task_state,
                stepData = task.task_step_data,
                baseId = task.task_id,
            };
        }

        /// <summary>
        /// 转换成职业任务vo
        /// </summary>
        /// <param name="task">tg_task实体</param>
        public static VocationTaskVo ToVocationTaskVo(tg_task task)
        {
            return new VocationTaskVo()
            {
                id = task.id,
                state = task.task_state,
                stepData = task.task_step_data,
                baseId = task.task_id,
                endTime = task.task_endtime,
            };
        }

        public static RoleTaskVo ToRoleVo(tg_task task)
        {
            return new RoleTaskVo()
            {
                id = task.id,
                roleId = task.rid,
                time = task.task_endtime,
                baseId = task.task_id,
                beginTime = task.task_starttime,
                state = task.task_state,
            };
        }

        /// <summary> tg_train_home 实体转换 NpcMonsterVo </summary>
        /// <param name="model">武将宅怪物Vo</param>
        /// <returns>前端NpcMonsterVo</returns>
        public static NpcMonsterVo ToNpcMonsterVo(tg_train_home model)
        {
            return new NpcMonsterVo()
            {
                id = model.id,
                baseId = model.npc_id,
                state = model.npc_state,
                spirit = model.npc_spirit,
                isSteal = model.is_steal,
            };
        }

        /// <summary> tg_role_title 实体转换 TitleVo </summary>
        /// <param name="model">武将称号Vo</param>
        /// <returns>前端TitleVo</returns>
        public static TitleVo ToTitleVo(tg_role_title model)
        {
            return new TitleVo()
            {
                id = model.id,
                baseId = model.title_id,
                state = model.title_state,
                load_state = model.title_load_state,
                packetCount = model.title_count,
                packet1_role = model.packet_role1,
                packet2_role = model.packet_role2,
                packet3_role = model.packet_role3,
            };
        }

        /// <summary> 组装HomeHireVo </summary>
        public static HomeHireVo ToRoleHomeHireVo(tg_user_extend extend)
        {
            var model = new HomeHireVo
            {
                id = extend.id,
                baseId = extend.hire_id,
                time = extend.hire_time,
                state = extend.hire_id == 0 ? 0 : 1,
            };
            return model;
        }

        /// <summary> view_ranking_honor/view_ranking_coin 实体转换 UserRankingVo </summary>
        /// <param name="model">功名榜视图/富豪榜视图</param>
        /// <returns>前端UserRankingVo</returns>
        public static UserRankingVo ToUserRankingVo(view_ranking_honor model, int oneself)
        {
            return new UserRankingVo()
            {
                id = model.id,
                userid = model.id,
                oneself = oneself,
                ranking = Convert.ToInt32(model.ranks),
                name = model.player_name,
                level = model.role_level,
                camp = model.player_camp,
                forces = model.player_influence,
                Identity = model.role_identity,
            };
        }

        /// <summary> view_ranking_coin 实体转换 UserRankingVo </summary>
        /// <param name="model">富豪榜视图</param>
        /// <returns>前端UserRankingVo</returns>
        public static UserRankingVo ToUserRankingVo(view_ranking_coin model, int oneself)
        {
            return new UserRankingVo()
            {
                id = model.id,
                userid = model.id,
                oneself = oneself,
                ranking = Convert.ToInt32(model.ranks),
                name = model.player_name,
                level = model.role_level,
                camp = model.player_camp,
                forces = model.player_influence,
                Identity = model.role_identity,
            };
        }

        /// <summary> view_ranking_game 实体转换 UserRankingVo </summary>
        /// <param name="model">闯关榜视图</param>
        /// <param name="oneself">是否自己</param>
        /// <returns>前端UserRankingVo</returns>
        public static UserRankingVo ToUserRankingVo(view_ranking_game model, int oneself)
        {
            return new UserRankingVo()
            {
                id = model.id,
                userid = model.id,
                oneself = oneself,
                ranking = Convert.ToInt32(model.ranks),
                name = model.player_name,
                level = model.role_level,
                camp = model.player_camp,
                forces = model.player_influence,
                Identity = model.role_identity,
                pass = model.week_max_pass
            };
        }

        /// <summary>tg_role_recruit 转换 RecruitVo  </summary>
        public static RecruitVo ToRecruitVo(tg_role_recruit model)
        {
            return new RecruitVo
            {
                id = model.id,
                baseId = model.role_id,
                position = model.position,
            };
        }

        /// <summary>tg_daming_log 转换 DaMingLingVo</summary>
        public static DaMingLingVo ToDaMingLingVo(tg_daming_log model)
        {
            return new DaMingLingVo()
            {
                id = model.id,
                baseid = model.base_id,
                state = model.is_reward,
                degree = model.user_finish,
            };
        }

        /// <summary>转换 YouYiyuanVo </summary>
        /// <param name="type">游戏类型</param>
        /// <param name="num">剩余次数</param>
        /// <param name="pass">最高关数</param>
        /// <returns></returns>
        public static YouYiyuanVo ToYouYiyuanVo(int type, int num, int pass)
        {
            return new YouYiyuanVo
            {
                type = type,
                num = num,
                passMax = pass,
            };
        }

        /// <summary>tg_user_vip 转换 VipVo </summary>
        /// <param name="model">tg_user_vip实体</param>
        /// <returns></returns>
        public static VipVo ToVipVo(tg_user_vip model)
        {
            return new VipVo
            {
                level = model.vip_level,
                costGold = model.vip_gold,
            };
        }
    }
}
