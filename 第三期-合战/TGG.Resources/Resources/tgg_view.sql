IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_ranking_exp]'))
DROP VIEW view_ranking_exp
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_war_partner]'))
DROP VIEW [view_war_partner]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_war_city]'))
DROP VIEW [view_war_city]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_user_role_task]'))
DROP VIEW [view_user_role_task]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_user_role_friend]'))
DROP VIEW [view_user_role_friend]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_user_role_family_member]'))
DROP VIEW [view_user_role_family_member]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_user_role_family_apply]'))
DROP VIEW [view_user_role_family_apply]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_user_area_set]'))
DROP VIEW [view_user_area_set]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_ting_not_car]'))
DROP VIEW [view_ting_not_car]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_scene_user]'))
DROP VIEW [view_scene_user]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_role]'))
DROP VIEW [view_role]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_ranking_honor]'))
DROP VIEW [view_ranking_honor]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_ranking_game]'))
DROP VIEW [view_ranking_game]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_ranking_coin]'))
DROP VIEW [view_ranking_coin]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_player]'))
DROP VIEW [view_player]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_messages]'))
DROP VIEW [view_messages]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_city_defense_plan]'))
DROP VIEW [view_city_defense_plan]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_arena_report]'))
DROP VIEW [view_arena_report]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_arena_ranking]'))
DROP VIEW [view_arena_ranking]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_war_role]'))
DROP VIEW [view_war_role]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_role_user]'))
DROP VIEW [view_role_user]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_player_detail]'))
DROP VIEW [view_player_detail]
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_ranking_copy]'))
DROP VIEW [view_ranking_copy]

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_role_user]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_role_user]
AS
SELECT   dbo.tg_role.id, dbo.tg_role.role_id, dbo.tg_role.power, dbo.tg_role.role_level, dbo.tg_role.role_state, dbo.tg_role.user_id, 
                dbo.tg_user.player_name, dbo.tg_role.role_identity, dbo.tg_role.total_honor
FROM      dbo.tg_role LEFT OUTER JOIN
                dbo.tg_user ON dbo.tg_role.user_id = dbo.tg_user.id AND dbo.tg_role.role_id = dbo.tg_user.role_id
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_war_role]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_war_role]
AS
SELECT   dbo.tg_war_role.id, dbo.tg_war_role.user_id, dbo.tg_war_role.rid, dbo.tg_war_role.type, dbo.tg_war_role.state, 
                dbo.tg_war_role.station, dbo.tg_war_role.resource, dbo.tg_war_role.army_id, dbo.tg_war_role.army_horse, 
                dbo.tg_war_role.army_gun, dbo.tg_war_role.army_soldier, dbo.tg_war_role.army_morale, dbo.tg_war_role.army_funds, 
                dbo.view_role_user.role_id, dbo.view_role_user.role_identity, dbo.view_role_user.role_state, 
                dbo.view_role_user.power, dbo.view_role_user.player_name, dbo.view_role_user.total_honor, 
                dbo.tg_war_role.state_end_time, dbo.tg_war_role.count
FROM      dbo.tg_war_role LEFT OUTER JOIN
                dbo.view_role_user ON dbo.tg_war_role.rid = dbo.view_role_user.id
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_arena_ranking]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_arena_ranking]
AS
SELECT   TOP (100) PERCENT dbo.tg_user.player_camp, dbo.tg_user.player_influence, 
                dbo.tg_user.player_vocation, dbo.tg_user.player_sex, dbo.tg_user.role_id, 
                dbo.tg_user.player_name, dbo.tg_user.fame, dbo.tg_role.power, dbo.tg_role.role_level, dbo.tg_role.role_state, 
                dbo.tg_role.role_exp, dbo.tg_role.role_honor, dbo.tg_role.role_identity, dbo.tg_arena.id, dbo.tg_arena.ranking, 
                dbo.tg_arena.totalCount, dbo.tg_arena.count, dbo.tg_arena.time, dbo.tg_arena.winCount, dbo.tg_arena.remove_cooling, 
                dbo.tg_arena.user_id, dbo.tg_arena.buy_count
FROM      dbo.tg_arena INNER JOIN
                dbo.tg_user ON dbo.tg_arena.user_id = dbo.tg_user.id INNER JOIN
                dbo.tg_role ON dbo.tg_user.id = dbo.tg_role.user_id AND dbo.tg_user.role_id = dbo.tg_role.role_id
ORDER BY dbo.tg_arena.ranking
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_arena_report]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_arena_report]
AS
SELECT   dbo.tg_arena_reports.id, dbo.tg_arena_reports.other_user_id, dbo.tg_arena_reports.type, dbo.tg_arena_reports.isWin, 
                dbo.tg_arena_reports.history, dbo.tg_arena_reports.time, dbo.tg_arena_reports.user_id, dbo.tg_user.player_name, 
                dbo.tg_user.player_sex, dbo.tg_user.player_vocation, dbo.tg_role.role_level
FROM      dbo.tg_arena_reports INNER JOIN
                dbo.tg_user ON dbo.tg_arena_reports.other_user_id = dbo.tg_user.id INNER JOIN
                dbo.tg_role ON dbo.tg_user.id = dbo.tg_role.user_id AND dbo.tg_user.role_id = dbo.tg_role.role_id
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_city_defense_plan]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_city_defense_plan]
AS
SELECT   dbo.tg_war_city_plan.user_id, dbo.tg_war_city_plan.formation, dbo.tg_war_city_defense.role_id, 
                dbo.tg_war_city_defense.point_y, dbo.tg_war_city_defense.point_x, dbo.tg_war_city_plan.is_choose, 
                dbo.tg_war_city_plan.location, dbo.tg_war_plan_area.base_id, dbo.tg_war_plan_area.base_point_x, 
                dbo.tg_war_plan_area.base_point_y, dbo.tg_war_city_defense.plan_id, dbo.tg_war_city_defense.city_id, 
                dbo.tg_war_city_defense.role_base_id, dbo.tg_war_city_defense.type, dbo.tg_war_city_plan.is_update
FROM      dbo.tg_war_city_plan INNER JOIN
                dbo.tg_war_city_defense ON dbo.tg_war_city_plan.id = dbo.tg_war_city_defense.plan_id INNER JOIN
                dbo.tg_war_plan_area ON dbo.tg_war_city_plan.id = dbo.tg_war_plan_area.plan_id
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_messages]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_messages]
AS
SELECT   dbo.tg_messages.id, dbo.tg_messages.receive_id, dbo.tg_messages.send_id, dbo.tg_messages.type, 
                dbo.tg_messages.title, dbo.tg_messages.contents, dbo.tg_messages.isread, dbo.tg_messages.isattachment, 
                dbo.tg_messages.attachment, dbo.tg_messages.create_time, dbo.tg_user.player_name
FROM      dbo.tg_messages LEFT OUTER JOIN
                dbo.tg_user ON dbo.tg_messages.send_id = dbo.tg_user.id
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_player]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_player]
AS
SELECT   dbo.tg_user.id, dbo.tg_user.createtime, dbo.tg_role.role_level, dbo.tg_role.role_state
FROM      dbo.tg_user INNER JOIN
                dbo.tg_role ON dbo.tg_user.id = dbo.tg_role.user_id
WHERE   (dbo.tg_role.role_state = 1)
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_ranking_coin]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_ranking_coin]
AS
SELECT   ROW_NUMBER() OVER (ORDER BY dbo.tg_user.coin DESC) AS ranks, dbo.tg_user.id, dbo.tg_user.player_name, 
dbo.tg_user.player_camp, dbo.tg_user.player_influence, dbo.tg_role.id AS rid, dbo.tg_role.role_level, dbo.tg_user.coin
,dbo.tg_role.role_identity
FROM      dbo.tg_user INNER JOIN
                dbo.tg_role ON dbo.tg_user.id = dbo.tg_role.user_id AND dbo.tg_user.role_id = dbo.tg_role.role_id
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_ranking_game]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_ranking_game]
AS
SELECT    ROW_NUMBER() OVER (ORDER BY dbo.tg_game.week_max_pass DESC) AS ranks,
dbo.tg_user.id, dbo.tg_user.player_name, dbo.tg_user.player_camp, dbo.tg_user.player_influence, dbo.tg_role.id AS rid, 
                dbo.tg_role.role_level, dbo.tg_role.role_identity, dbo.tg_game.tea_max_pass, dbo.tg_game.ninjutsu_max_pass, 
                dbo.tg_game.calculate_max_pass, dbo.tg_game.eloquence_max_pass, dbo.tg_game.spirit_max_pass, 
                dbo.tg_game.week_max_pass
FROM      dbo.tg_user INNER JOIN
                dbo.tg_role ON dbo.tg_user.id = dbo.tg_role.user_id AND dbo.tg_user.role_id = dbo.tg_role.role_id LEFT OUTER JOIN
                dbo.tg_game ON dbo.tg_user.id = dbo.tg_game.user_id
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_ranking_honor]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_ranking_honor]
AS
SELECT   ROW_NUMBER() OVER (ORDER BY dbo.tg_role.total_honor DESC) AS ranks, dbo.tg_user.id, dbo.tg_user.player_name, 
dbo.tg_user.role_id, dbo.tg_user.player_influence, dbo.tg_user.player_camp, dbo.tg_role.role_level, dbo.tg_role.id AS rid, 
dbo.tg_role.total_honor AS role_honor, dbo.tg_role.role_honor AS honor,dbo.tg_role.role_identity
FROM      dbo.tg_user INNER JOIN
                dbo.tg_role ON dbo.tg_user.id = dbo.tg_role.user_id AND dbo.tg_user.role_id = dbo.tg_role.role_id
WHERE   dbo.tg_role.role_state = 1
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_role]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_role]
AS
SELECT   dbo.tg_role.id, dbo.tg_role.user_id, dbo.tg_role.role_id, dbo.tg_role.power, dbo.tg_role.role_level, dbo.tg_role.role_state, 
                dbo.tg_role.role_exp,dbo.tg_role.total_exp, dbo.tg_role.role_honor, dbo.tg_role.role_identity, dbo.tg_role.base_captain, 
                dbo.tg_role.base_force, dbo.tg_role.base_brains, dbo.tg_role.base_charm, dbo.tg_role.base_govern, 
                dbo.tg_role.att_life, dbo.tg_role.att_attack, dbo.tg_role.att_defense, dbo.tg_role.att_sub_hurtIncrease, 
                dbo.tg_role.att_sub_hurtReduce, dbo.tg_role.art_mystery, dbo.tg_role.art_cheat_code, dbo.tg_role.equip_weapon, 
                dbo.tg_role.equip_barbarian, dbo.tg_role.equip_mounts, dbo.tg_role.equip_armor, dbo.tg_role.equip_gem, 
                dbo.tg_role.equip_craft, dbo.tg_role.equip_tea, dbo.tg_role.equip_book, dbo.tg_role.att_points, 
                dbo.tg_role_life_skill.id AS lid, dbo.tg_role_life_skill.sub_tea, dbo.tg_role_life_skill.sub_calculate, 
                dbo.tg_role_life_skill.sub_build, dbo.tg_role_life_skill.sub_eloquence, dbo.tg_role_life_skill.sub_equestrian, 
                dbo.tg_role_life_skill.sub_reclaimed, dbo.tg_role_life_skill.sub_ashigaru, dbo.tg_role_life_skill.sub_artillery, 
                dbo.tg_role_life_skill.sub_mine, dbo.tg_role_life_skill.sub_craft, dbo.tg_role_life_skill.sub_archer, 
                dbo.tg_role_life_skill.sub_etiquette, dbo.tg_role_life_skill.sub_martial, dbo.tg_role_life_skill.sub_tactical, 
                dbo.tg_role_life_skill.sub_medical, dbo.tg_role_life_skill.sub_ninjitsu, dbo.tg_role_life_skill.sub_tea_progress, 
                dbo.tg_role_life_skill.sub_calculate_progress, dbo.tg_role_life_skill.sub_build_progress, 
                dbo.tg_role_life_skill.sub_eloquence_progress, dbo.tg_role_life_skill.sub_equestrian_progress, 
                dbo.tg_role_life_skill.sub_reclaimed_progress, dbo.tg_role_life_skill.sub_ashigaru_progress, 
                dbo.tg_role_life_skill.sub_artillery_progress, dbo.tg_role_life_skill.sub_mine_progress, 
                dbo.tg_role_life_skill.sub_craft_progress, dbo.tg_role_life_skill.sub_archer_progress, 
                dbo.tg_role_life_skill.sub_etiquette_progress, dbo.tg_role_life_skill.sub_martial_progress, 
                dbo.tg_role_life_skill.sub_tactical_progress, dbo.tg_role_life_skill.sub_medical_progress, 
                dbo.tg_role_life_skill.sub_ninjitsu_progress, dbo.tg_role_life_skill.sub_tea_time, 
                dbo.tg_role_life_skill.sub_calculate_time, dbo.tg_role_life_skill.sub_build_time, 
                dbo.tg_role_life_skill.sub_eloquence_time, dbo.tg_role_life_skill.sub_equestrian_time, 
                dbo.tg_role_life_skill.sub_reclaimed_time, dbo.tg_role_life_skill.sub_ashigaru_time, 
                dbo.tg_role_life_skill.sub_artillery_time, dbo.tg_role_life_skill.sub_mine_time, dbo.tg_role_life_skill.sub_craft_time, 
                dbo.tg_role_life_skill.sub_archer_time, dbo.tg_role_life_skill.sub_etiquette_time, 
                dbo.tg_role_life_skill.sub_martial_time, dbo.tg_role_life_skill.sub_tactical_time, 
                dbo.tg_role_life_skill.sub_medical_time, dbo.tg_role_life_skill.sub_ninjitsu_time, dbo.tg_role_life_skill.sub_tea_level, 
                dbo.tg_role_life_skill.sub_calculate_level, dbo.tg_role_life_skill.sub_build_level, 
                dbo.tg_role_life_skill.sub_eloquence_level, dbo.tg_role_life_skill.sub_equestrian_level, 
                dbo.tg_role_life_skill.sub_reclaimed_level, dbo.tg_role_life_skill.sub_ashigaru_level, 
                dbo.tg_role_life_skill.sub_artillery_level, dbo.tg_role_life_skill.sub_mine_level, dbo.tg_role_life_skill.sub_craft_level, 
                dbo.tg_role_life_skill.sub_archer_level, dbo.tg_role_life_skill.sub_etiquette_level, 
                dbo.tg_role_life_skill.sub_martial_level, dbo.tg_role_life_skill.sub_tactical_level, 
                dbo.tg_role_life_skill.sub_medical_level, dbo.tg_role_life_skill.sub_ninjitsu_level, dbo.tg_role_life_skill.sub_tea_state, 
                dbo.tg_role_life_skill.sub_calculate_state, dbo.tg_role_life_skill.sub_build_state, 
                dbo.tg_role_life_skill.sub_eloquence_state, dbo.tg_role_life_skill.sub_equestrian_state, 
                dbo.tg_role_life_skill.sub_reclaimed_state, dbo.tg_role_life_skill.sub_ashigaru_state, 
                dbo.tg_role_life_skill.sub_artillery_state, dbo.tg_role_life_skill.sub_mine_state, dbo.tg_role_life_skill.sub_craft_state, 
                dbo.tg_role_life_skill.sub_archer_state, dbo.tg_role_life_skill.sub_etiquette_state, 
                dbo.tg_role_life_skill.sub_martial_state, dbo.tg_role_life_skill.sub_tactical_state, 
                dbo.tg_role_life_skill.sub_medical_state, dbo.tg_role_life_skill.sub_ninjitsu_state, dbo.tg_role_fight_skill.id AS fid, 
                dbo.tg_role_fight_skill.skill_id, dbo.tg_role_fight_skill.skill_type, dbo.tg_role_fight_skill.skill_level, 
                dbo.tg_role_fight_skill.skill_time, dbo.tg_role_fight_skill.skill_genre, dbo.tg_role.role_genre, 
                dbo.tg_role.att_crit_probability, dbo.tg_role.att_crit_addition, dbo.tg_role.att_dodge_probability, 
                dbo.tg_role.att_mystery_probability, dbo.tg_role_fight_skill.skill_state, dbo.tg_role.role_ninja, 
                dbo.tg_role.base_captain_life, dbo.tg_role.base_captain_train, dbo.tg_role.base_captain_level, 
                dbo.tg_role.base_captain_spirit, dbo.tg_role.base_captain_equip, dbo.tg_role.base_captain_title, 
                dbo.tg_role.base_force_life, dbo.tg_role.base_force_train, dbo.tg_role.base_force_level, dbo.tg_role.base_force_spirit, 
                dbo.tg_role.base_force_equip, dbo.tg_role.base_force_title, dbo.tg_role.base_brains_life, 
                dbo.tg_role.base_brains_train, dbo.tg_role.base_brains_level, dbo.tg_role.base_brains_spirit, 
                dbo.tg_role.base_brains_equip, dbo.tg_role.base_brains_title, dbo.tg_role.base_charm_life, 
                dbo.tg_role.base_charm_train, dbo.tg_role.base_charm_level, dbo.tg_role.base_charm_spirit, 
                dbo.tg_role.base_charm_equip, dbo.tg_role.base_charm_title, dbo.tg_role.base_govern_life, 
                dbo.tg_role.base_govern_train, dbo.tg_role.base_govern_level, dbo.tg_role.base_govern_spirit, 
                dbo.tg_role.base_govern_equip, dbo.tg_role.base_govern_title, dbo.tg_role_fight_skill.type_sub, 
                dbo.tg_role.title_sword, dbo.tg_role.title_gun, dbo.tg_role.title_tea, dbo.tg_role.title_eloquence, dbo.tg_role.buff_power, 
                dbo.tg_role.total_honor, dbo.tg_role_war_skill.id AS wid, dbo.tg_role_war_skill.war_skill_id, 
                dbo.tg_role_war_skill.war_skill_type, dbo.tg_role_war_skill.war_skill_level, dbo.tg_role_war_skill.war_skill_time, 
                dbo.tg_role_war_skill.war_skill_state, dbo.tg_role_war_skill.war_skill_wuchang, dbo.tg_role.art_ninja_mystery, 
                dbo.tg_role.art_ninja_cheat_code3, dbo.tg_role.art_ninja_cheat_code2, dbo.tg_role.art_ninja_cheat_code1, 
                dbo.tg_role.art_war_mystery, dbo.tg_role.art_war_cheat_code, dbo.tg_role.character1, dbo.tg_role.character2, 
                dbo.tg_role.character3, dbo.tg_role.war_att_attack, dbo.tg_role.war_att_defense
FROM      dbo.tg_role INNER JOIN
                dbo.tg_role_life_skill ON dbo.tg_role.id = dbo.tg_role_life_skill.rid LEFT OUTER JOIN
                dbo.tg_role_fight_skill ON dbo.tg_role.id = dbo.tg_role_fight_skill.rid LEFT OUTER JOIN
                dbo.tg_role_war_skill ON dbo.tg_role.id = dbo.tg_role_war_skill.rid
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_scene_user]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_scene_user]
AS
SELECT   dbo.tg_scene.id, dbo.tg_scene.scene_id, dbo.tg_scene.user_id, dbo.tg_scene.X, dbo.tg_scene.Y, 
                dbo.tg_user.player_name, dbo.tg_user.player_sex, dbo.tg_user.player_vocation, dbo.tg_role.role_level, 
                dbo.tg_scene.model_number, dbo.tg_user.player_camp, dbo.tg_role.role_identity
FROM      dbo.tg_scene INNER JOIN
                dbo.tg_user ON dbo.tg_scene.user_id = dbo.tg_user.id INNER JOIN
                dbo.tg_role ON dbo.tg_user.id = dbo.tg_role.user_id AND dbo.tg_user.role_id = dbo.tg_role.role_id
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_ting_not_car]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_ting_not_car]
AS
SELECT   id, user_id, ting_id, state, area_id
FROM      dbo.tg_user_ting
WHERE   (id NOT IN
                    (SELECT   tg_user_ting_1.id
                     FROM      dbo.tg_user_ting AS tg_user_ting_1 INNER JOIN
                                     dbo.tg_car ON tg_user_ting_1.user_id = dbo.tg_car.user_id AND 
                                     tg_user_ting_1.ting_id = dbo.tg_car.start_ting_id
                     WHERE   (dbo.tg_car.state = 0)
                     GROUP BY tg_user_ting_1.id))
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_user_area_set]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_user_area_set]
AS
SELECT   dbo.tg_war_area.id, dbo.tg_war_area.user_id, dbo.tg_war_area_set.base_id, dbo.tg_war_area_set.base_point_x, 
                dbo.tg_war_area_set.base_point_y, dbo.tg_war_area.location, dbo.tg_war_area_set.type
FROM      dbo.tg_war_area INNER JOIN
                dbo.tg_war_area_set ON dbo.tg_war_area.id = dbo.tg_war_area_set.area_id
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_user_role_family_apply]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_user_role_family_apply]
AS
SELECT   dbo.tg_family_apply.id, dbo.tg_family_apply.userid, dbo.tg_family_apply.state, dbo.tg_family_apply.time, 
                dbo.tg_family_apply.fid, dbo.tg_user.player_name, dbo.tg_user.role_id, dbo.tg_user.player_sex, 
                dbo.tg_user.player_vocation,  dbo.tg_user.player_influence, dbo.tg_user.player_camp, 
                dbo.tg_role.power, dbo.tg_role.role_level, dbo.tg_role.role_state, dbo.tg_role.role_identity, dbo.tg_role.role_honor
FROM      dbo.tg_family_apply INNER JOIN
                dbo.tg_user ON dbo.tg_family_apply.userid = dbo.tg_user.id INNER JOIN
                dbo.tg_role ON dbo.tg_user.id = dbo.tg_role.user_id AND dbo.tg_user.role_id = dbo.tg_role.role_id
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_user_role_family_member]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_user_role_family_member]
AS
SELECT   dbo.tg_family_member.id, dbo.tg_family_member.fid, dbo.tg_family_member.userid, dbo.tg_family_member.degree, 
                dbo.tg_family_member.devote, dbo.tg_user.player_name, dbo.tg_user.player_sex, dbo.tg_user.player_vocation, 
                 dbo.tg_user.player_influence, dbo.tg_user.player_camp, dbo.tg_user.spirit, 
                dbo.tg_user.fame, dbo.tg_role.role_level, dbo.tg_role.power, dbo.tg_role.role_state, dbo.tg_role.role_exp, 
                dbo.tg_role.role_honor, dbo.tg_role.role_identity
FROM      dbo.tg_family_member INNER JOIN
                dbo.tg_user ON dbo.tg_family_member.userid = dbo.tg_user.id INNER JOIN
                dbo.tg_role ON dbo.tg_user.id = dbo.tg_role.user_id AND dbo.tg_user.role_id = dbo.tg_role.role_id
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_user_role_friend]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [view_user_role_friend]
AS
SELECT   dbo.tg_friends.id, dbo.tg_friends.friend_id, dbo.tg_friends.user_id, dbo.tg_friends.friend_state, dbo.tg_user.id AS uid, 
                dbo.tg_user.player_name, dbo.tg_user.role_id, dbo.tg_user.player_sex, dbo.tg_user.player_vocation, 
                 dbo.tg_user.player_influence, dbo.tg_user.player_camp, dbo.tg_role.id AS rid, 
                dbo.tg_role.role_identity, dbo.tg_role.role_honor, dbo.tg_role.role_genre, dbo.tg_role.role_level, 0 AS isonline, 
                dbo.tg_role.role_exp
FROM      dbo.tg_role INNER JOIN
                dbo.tg_user ON dbo.tg_role.user_id = dbo.tg_user.id AND dbo.tg_role.role_id = dbo.tg_user.role_id INNER JOIN
                dbo.tg_friends ON dbo.tg_user.id = dbo.tg_friends.friend_id

' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_user_role_task]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_user_role_task]
AS
SELECT   dbo.tg_role.user_id, dbo.tg_user.player_vocation, dbo.tg_role.role_identity, dbo.tg_user.player_camp
FROM      dbo.tg_user INNER JOIN
                dbo.tg_role ON dbo.tg_user.id = dbo.tg_role.user_id AND dbo.tg_role.role_state = 1
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_war_city]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_war_city]
AS
SELECT   dbo.tg_user.player_name, dbo.tg_war_city.*
FROM      dbo.tg_user INNER JOIN
                dbo.tg_war_city ON dbo.tg_user.id = dbo.tg_war_city.user_id
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_war_partner]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_war_partner]
AS
SELECT   dbo.tg_war_partner.*, dbo.tg_user.player_name, dbo.tg_user.office
FROM      dbo.tg_war_partner INNER JOIN
                dbo.tg_user ON dbo.tg_war_partner.partner_id = dbo.tg_user.id
' 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_ranking_exp]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_ranking_exp]
AS
SELECT    ROW_NUMBER() OVER (ORDER BY dbo.tg_role.total_exp DESC) AS ranks,dbo.tg_user.id, dbo.tg_user.player_name, dbo.tg_user.player_camp, dbo.tg_user.player_influence, 
                dbo.tg_role.role_level, dbo.tg_user.role_id, dbo.tg_role.id AS rid, dbo.tg_role.role_exp, dbo.tg_role.role_identity, 
                dbo.tg_role.total_exp
FROM      dbo.tg_user INNER JOIN
                dbo.tg_role ON dbo.tg_user.id = dbo.tg_role.user_id AND dbo.tg_user.role_id = dbo.tg_role.role_id
WHERE   dbo.tg_role.role_state = 1
'

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_player_detail]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_player_detail]
AS
SELECT   dbo.tg_user.id, dbo.tg_user.user_code, dbo.tg_user.player_name, dbo.tg_role.role_level, dbo.tg_role.role_identity, 
                dbo.tg_user.coin, dbo.tg_user.gold, dbo.tg_user_vip.vip_level, dbo.tg_user_vip.vip_gold, 
                dbo.tg_user_login_log.login_time
FROM      dbo.tg_user INNER JOIN
                dbo.tg_user_login_log ON dbo.tg_user.id = dbo.tg_user_login_log.user_id INNER JOIN
                dbo.tg_user_vip ON dbo.tg_user.id = dbo.tg_user_vip.user_id INNER JOIN
                dbo.tg_role ON dbo.tg_user.id = dbo.tg_role.user_id AND dbo.tg_user.role_id = dbo.tg_role.role_id
'

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[view_ranking_copy]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [view_ranking_copy]
AS
SELECT   ROW_NUMBER() OVER (ORDER BY dbo.tg_war_copy.score DESC,dbo.tg_war_copy.latest_time) AS ranks, 
              dbo.tg_user.id, dbo.tg_user.player_name, dbo.tg_user.player_camp, dbo.tg_user.player_influence, dbo.tg_war_copy.score, 
              dbo.tg_war_copy.latest_time
FROM    dbo.tg_user INNER JOIN
              dbo.tg_war_copy ON dbo.tg_user.id = dbo.tg_war_copy.user_id
' 