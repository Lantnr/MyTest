namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 数据返回枚举
    /// 0-9999是公共数据类型
    /// 模块自有返回值类型规则(模块号*(-10000)+N)
    /// </summary>
    public enum ResultType
    {
        #region 公共数据类型

        /// <summary>没有任何数据</summary>
        NO_DATA = 1,

        /// <summary>指令执行成功 </summary>
        SUCCESS = 0,

        /// <summary>指令执行失败 </summary>
        FAIL = -1,

        /// <summary> 数据库请求操作错误</summary>
        DATABASE_ERROR = -3,

        /// <summary> 获取前端数据错误</summary>
        FRONT_DATA_ERROR = -4,

        /// <summary>未知错误</summary>
        UNKNOW_ERROR = -200,

        /// <summary>账号未激活错误</summary>
        ACTIVATION_ERROR = -201,

        /// <summary>防沉迷时间未到</summary>
        LOGIN_OPENTIME_ERROR = -202,

        #region VIP
        /// <summary>VIP购买体力次数不足</summary>
        VIP_POWER_ERROR = -100,

        #endregion

        /// <summary>基表数据错误</summary>
        BASE_TABLE_ERROR = -1000,

        #region 玩家公共错误类型
        /// <summary>元宝不足</summary>
        BASE_PLAYER_GOLD_ERROR = -1001,

        /// <summary>金钱不足</summary>
        BASE_PLAYER_COIN_ERROR = -1002,

        /// <summary>体力不足</summary>
        BASE_PLAYER_POWER_ERROR = -1003,

        /// <summary>魂不足</summary>
        BASE_PLAYER_SPIRIT_ERROR = -1004,

        /// <summary>声望不够</summary>
        BASE_PLAYER_FAME_ERROR = -1005,

        /// <summary>未达到VIP</summary>
        BASE_PLAYER_VIP_ERROR = -1006,

        /// <summary>等级不够</summary>
        BASE_PLAYER_LEVEL_ERROR = -1007,

        /// <summary>功勋不够</summary>
        BASE_PLAYER_HONOR_ERROR = -1008,

        /// <summary>经验不够</summary>
        BASE_PLAYER_EXP_ERROR = -1009,

        /// <summary>身份不够</summary>
        BASE_PLAYER_IDENTITY_ERROR = -1010,

        /// <summary>用户不在线</summary>
        BASE_PLAYER_OFFLINE_ERROR = -1011,

        #endregion

        #region 武将公共结果类型
        /// <summary>武将等级不够</summary>
        BASE_ROLE_LEVEL_ERROR = -2001,

        /// <summary>武将体力不够</summary>
        BASE_ROLE_POWER_ERROR = -2002,

        /// <summary>武将技能等级不够</summary>
        BASE_ROLE_SKILL_LEVEL_ERROR = -2003,

        #endregion

        #region 背包模块
        /// <summary>道具不能出售</summary>
        PROP_UNBUY = -4001,

        /// <summary>道具数量不够</summary>
        PROP_LACK = -4002,

        /// <summary>道具不存在</summary>
        PROP_UNKNOW = -4003,

        /// <summary>道具不能使用</summary>
        PROP_UNUSED = -4004,

        /// <summary>背包格子不够</summary>
        PROP_BAG_LACK = -4005,

        /// <summary>背包已满</summary>
        BAG_ISFULL_ERROR = -4006,

        /// <summary> 背包格子达到上限 </summary>
        BAG_TOPLIMIT = -4007,

        /// <summary> 开启背包格子超过上限 </summary>
        BAG_EXCEED_TOPLIMIT = -4008,

        /// <summary> 该道具不能合成 </summary>
        PROP_NO_COMPOSE = -4009,

        /// <summary> 该流派技能书已开启 </summary>
        PROP_GENRE_OPENED = -4010,

        /// <summary>资源处理失败</summary>
        BAG_RESOURCES_ERROR = -4011,

        #endregion
        #endregion

        #region 用户基本模块

        /// <summary>玩家名称长度太长</summary>
        USER_NAME_LEN_MAX = -10001,

        /// <summary>玩家名称已经存在</summary>
        USER_NAME_ISEXIST = -10002,

        /// <summary>提交请求数据错误</summary>
        USER_SUBMIT_ERROR = -10003,

        /// <summary>俸禄已领取</summary>
        USER_RECEIVE = -10004,

        #endregion

        #region 任务模块

        /// <summary>当前主线任务未完成</summary>
        TASK_MAINTASK_UNFINISHED = -20001,

        /// <summary>没有新的任务了</summary>
        TASK_NONEWTASK = -20002,

        /// <summary>没有查询到该主线任务</summary>
        TASK_NO_MAINTASK = -20003,

        /// <summary>任务npc错误</summary>
        TASK_NPC_FALSE = -20004,

        /// <summary>该任务已经完成</summary>
        TASK_FINISHED = -20005,

        /// <summary>领取任务奖励错误</summary>
        REWARD_FALSE = -20006,

        /// <summary>主线任务更新失败</summary>
        TASK_UPDATE_FALSE = -20007,

        /// <summary>职业任务未完成</summary>
        TASK_VOCATION_UNFINISH = -20008,

        /// <summary> 职业任务更新失败 </summary>
        TASK_VOCATION_UPDATEWRONG = -20009,

        /// <summary> 没有该职业任务 </summary>
        TASK_VOCATION_NOTASK = -20012,

        /// <summary> 刷新失败 </summary>
        TASK_VOCATION_NOREFLASH = -20010,

        /// <summary>职业任务刷新次数不对</summary>
        TASK_VOCATION_COUNTERROR = -20011,

        /// <summary>大名没有职业任务</summary>
        TASK_VOCATION_NONEW = -20013,

        /// <summary>职业任务已经做完，不能刷新</summary>
        TASK_VOCATION_FINISHED = -20014,

        /// <summary>任务已经重置过了</summary>
        TASK_VOCATION_ISRESET = -20015,

        /// <summary>任务类型错误</summary>
        TASK_VOCATION_TYPEERROR = -20016,

        /// <summary>正在监狱中，不能进行任务</summary>
        TASK_VOCATION_PRISON = -20017,

        /// <summary>该职业任务不是护送类型</summary>
        TASK_NO_ESCORT = -20018,

        /// <summary> 该职业任务步骤为空 </summary>
        TASK_STEP_NULL = -20019,

        /// <summary> 该职业任务步骤已完成 </summary>
        TASK_STEP_OK = -20020,

        /// <summary> 任务步骤与基表不一致 </summary>
        TASK_STEP_ERROR = -20021,

        /// <summary> 任务已经开始，不能重复任务 </summary>
        TASK_START_NOW = -20022,

        /// <summary> 玩家技能等级不够 </summary>
        TASK_SKILL_LACK = -20023,

        /// <summary> 站岗玩家下班了（10.19） </summary>
        TASK_CANCLE = -20024,

        /// <summary> 任务步骤数据出错（11.19） </summary>
        TASK_STEP_WRONG = -20025,

        /// <summary> 任务已经接受了 </summary>
        TASK_RECEIVED = -20026,


        #endregion

        #region 跑商模块

        /// <summary>马车格子不足</summary>
        BUSINESS_GRID_LACK = -30001,

        /// <summary>马车格子数已达到上限</summary>
        BUSINESS_PACKET_FULL = -30002,

        /// <summary>没有马车</summary>
        BUSINESS_CAR_LACK = -30003,

        /// <summary>货物数量超过最大数量</summary>
        BUSINESS_GOODS_COUNT_FULL = -30004,

        /// <summary>没有该货物</summary>
        BUSINESS_GOODS_LACK = -30005,

        /// <summary>讲价次数不够</summary>
        BUSINESS_BARGAIN_LACK = -30006,

        /// <summary>马车不在跑商状态</summary>
        BUSINESS_CAR_LACK_RUNNING = -30007,

        /// <summary>马车跑商时间错误</summary>
        BUSINESS_TIME_UNFINISHED = -30008,

        /// <summary>该町所属商圈还未开放</summary>
        BUSINESS_TIME_OPEN_ERROR = -30009,

        /// <summary>购买次数大于讲价可购买次数</summary>
        BUSINESS_BARGAIN_MAX_ERROR = -30010,

        /// <summary>VIP开启马车错误</summary>
        BUSINESS_VIP_CAR_ERROR = -30011,

        /// <summary>货物购买量不足</summary>
        BUSINESS_GOODS_BUY_ERROR = -30012,

        /// <summary>该商圈已经开启</summary>
        BUSINESS_AREA_ISEXIST_ERROR = -30013,

        /// <summary>VIP商圈次数不足</summary>
        BUSINESS_VIP_AREA_COUNR_ERROR = -30014,

        /// <summary>VIP购买次数不足</summary>
        BUSINESS_VIP_COUNR_ERROR = -30015,

        /// <summary>町没有停靠马车</summary>
        BUSINESS_RUN_TING_ERROR = -30016,

        #endregion

        #region 场景
        /// <summary>场景错误</summary>
        SCENE_ERROR = -50001,

        /// <summary>用户场景数据不存在</summary>
        SCENE_NOFIND = -50002,

        /// <summary>基表中没有该场景id</summary>
        SCENE_BASEDATA_WRONG = -50003,

        /// <summary>该地图未开启</summary>
        SCENE_CITY_UNOPEN = -50004,

        /// <summary>切换的场景与老场景相同</summary>
        SCENE_ENTER_SAME = -50005,
        #endregion

        #region 装备模块
        /// <summary>该装备不能出售</summary>
        EQUIP_NOSELL = -60001,

        /// <summary>没有该装备</summary>
        EQUIP_UNKNOW = -60002,

        /// <summary>魂没解锁</summary>
        EQUIP_SPIRIT_UNLOCK = -60003,

        /// <summary>魂大于当前等级升阶魂</summary>
        EQUIP_SPIRIT_ENOUGH = -60004,

        /// <summary>此装备不能丢弃</summary>
        EQUIP_NO_DISCARD = -60005,

        /// <summary>背包格子数不够</summary>
        EQUIP_BAG_FULL = -60006,

        /// <summary>装备品质未达到要求</summary>
        EQUIP_GRADE_ERROR = -60007,

        /// <summary>该装备洗炼次数已满</summary>
        EQUIP_BAPTIZE_MAX = -60008,

        /// <summary> 装备类型错误 </summary>
        EQUIP_TYPE_ERROR = -60009,

        #endregion

        #region 武将模块

        /// <summary>剩余点数不足</summary>
        ROLE_GROWADDCOUNT_LACK = -70001,

        /// <summary>装备已被穿戴</summary>
        ROLE_EQUIP_LOAD = -70002,

        /// <summary>装备已卸载</summary>
        ROLE_EQUIP_UNLOAD = -70003,

        /// <summary>抽取过武将</summary>
        ROLE_LOCK = -70004,

        /// <summary>技能还在学习中</summary>
        ROLE_SKILL_LEARNING = -70005,

        /// <summary>武将不存在</summary>
        ROLE_NOT_EXIST = -70006,

        /// <summary>该家臣存在</summary>
        ROLE_EXIST = -70007,

        /// <summary>已达到武将最大数</summary>
        ROLE_NUMBER_ENOUGH = -70008,

        /// <summary>流派信息错误</summary>
        ROLE_GENRE_ERROR = -70009,

        /// <summary>已选择流派</summary>
        ROLE_GENRE_SELECTOK = -70010,

        /// <summary>无法配置合战技能</summary>
        ROLE_SELECT_SKILL_ERROR = -70011,

        /// <summary>属性加成点超过上限</summary>
        ROLE_ADDATT_OVERRUN = -70012,

        /// <summary>未完成指定任务</summary>
        ROLE_TASK_UNFINISH = -70013,

        /// <summary> 购买体力后超过体力上限 </summary>
        ROLE_POWER_OVER = -70014,

        /// <summary>被动技能不能配置</summary>
        ROLE_SKILL_CONFIGURATION_ERROR = -70015,

        /// <summary>无该雇佣武将基表数据</summary>
        ROLE_HIRE_ERROR = -70016,

        /// <summary>玩家身份错误</summary>
        ROLE_PLAY_IDENTITY_ERROR = -70017,

        /// <summary>雇佣的武将身份不对应</summary>
        ROLE_HIRE_IDENTITY_ERROR = -70018,

        /// <summary> 已雇佣武将 </summary>
        ROLE_HIRE_SELECTOK = -70019,

        /// <summary> 没有可招募武将 </summary>
        ROLE_NO_RECRUIT = -70020,

        /// <summary>不能查看自己信息</summary>
        ROLE_QUERY_MYERROR = -70021,

        /// <summary>当前玩家没有武将信息</summary>
        ROLE_QUERY_NOINFO = -70022,

        /// <summary>不存在该玩家信息</summary>
        ROLE_QUERY_NOPLAYER = -70023,

        ///<summary>查询武将视图信息失败</summary>
        ROLE_QUERY_VIEWERROR = -70024,

        ///<summary>酒馆招募武将位置错误</summary>
        ROLE_RECRUIT_POSTITION_ERROR = -70025,

        ///<summary>酒馆招募武将位置已经存在武将</summary>
        ROLE_RECRUIT_ISEXIST_ERROR = -70026,

        #endregion

        #region 副本模块
        /// <summary>闯关失败</summary>
        CHALLENGE_FAIL = -90001,

        /// <summary>翻将次数用完</summary>
        CHALLENGENUM_LACK = -90002,

        /// <summary>该层不能翻将</summary>
        MONSTER_NO_REFRESH = -90003,

        /// <summary>关卡暂未开启</summary>
        TOWER_NO_OPEN = -90004,

        /// <summary>没达到守护者关卡</summary>
        TOWER_WATCHMEN_NO_OPEN = -90005,

        /// <summary>不能挑战守护者</summary>
        TOWER_WATCHMEN_FIGHT_NORIGHT = -90006,

        /// <summary>怪物已刷新完</summary>
        TOWER_NPC_ENOUGH = -90007,

        /// <summary>该层不能挑战怪物</summary>
        TOWER_NPC_NORIGHT = -90008,

        /// <summary>挑战守护者不能刷新怪物</summary>
        TOWER_WATCHEMEN_NOREFRESH = -90009,

        /// <summary>血量为0，无法挑战</summary>
        TOWER_BOOLD_UNENOUGH = -90010,

        /// <summary>卡牌已翻过</summary>
        TOWER_CARD_FLOPED = -90011,

        /// <summary>关卡信息错误</summary>
        TOWER_SITE_ERROR = -90012,

        /// <summary>不能挑战自己</summary>
        TOWER_NOFIGHT_MYSELF = -90013,
        #endregion

        #region 邮件模块
        /// <summary>没有附件</summary>
        MESSAGE_NO_ATTACHMENT_ERROR = -110001,

        /// <summary>提交数据错误</summary>
        MESSAGE_SUBMIT_DATA_ERROR = -110002,

        #endregion

        #region 聊天模块

        /// <summary>没有该玩家名称</summary>
        CHAT_NO_PLAYER_NAME = -120001,

        /// <summary>没有加入家族</summary>
        CHAT_NO_FAMILY = -120002,

        /// <summary>该玩家不在线</summary>
        CHAT_NO_ONLINE = -120003,

        /// <summary>不能对自己发信息</summary>
        CHAT_NO_MY = -120004,

        /// <summary>该玩家在黑名单</summary>
        CHAT_BLACK = -120005,

        /// <summary>在对方黑名单</summary>
        CHAT_RIVAL_BLACK = -120006,

        /// <summary>聊天冷却时间未到</summary>
        CHAT_TIME_ERROR = -120007,

        /// <summary> 玩家不存在 </summary>
        CHAT_NO_EXIST = -120008,
        #endregion

        #region 战斗模块

        /// <summary>主角不能删除</summary>
        FIGHT_LEAD_NO_DELETE = -150001,

        /// <summary>印达到最大等级</summary>
        FIGHT_YIN_LVMAX = -150002,

        /// <summary> 战斗类型错误 </summary>
        FIGHT_TYPE_ERROR = -150003,

        /// <summary> 战斗出错错误 </summary>
        FIGHT_ERROR = -150004,

        /// <summary> 拉取对手阵形错误 </summary>
        FIGHT_RIVAL_PERSONAL_ERROR = -150005,

        /// <summary> 对手武将ID获取错误 </summary>
        FIGHT_RIVAL_ID_ERROR = -150006,

        /// <summary> NPC基表错误 </summary>
        FIGHT_NPC_BASE_ERROR = -150007,

        /// <summary> 无该武将信息 </summary>
        FIGHT_NO_ROLE = -150008,

        /// <summary> 该玩家战斗中 </summary>
        FIGHT_FIGHT_IN = -150009,

        /// <summary> 0级印不能配置 </summary>
        FIGHT_YIN_NO_LEVEL = -150010,


        #endregion

        #region 技能模块
        /// <summary>未学习前置技能</summary>
        SKILL_CONDITION_LACK = -160001,

        /// <summary>前置技能等级不足</summary>
        SKILL_CONDITION_LEVEL_LACK = -160002,

        /// <summary>技能等级达到上限</summary>
        SKILL_LEVEL_FALL = -160003,

        /// <summary>技能已学习</summary>
        SKILL_SKILL_REPEAT = -160005,

        /// <summary>已有在学习或升级的技能</summary>
        SKILL_GENRE_HAS_LEARN = -160006,

        /// <summary>技能信息不存在</summary>
        SKILL_GET_ERROR = -160007,

        /// <summary>技能已经学习或升级成功</summary>
        SKILL_LEARN_UP_ERROR = -160008,

        /// <summary>技能未开启</summary>
        SKILL_LEARN_OPEN_ERROR = -160009,
        #endregion

        #region 修行模块
        /// <summary>武将不在修行状态</summary>
        TRAINROLE_LACK_TRAINING = -170001,

        /// <summary>武将正在修行状态</summary>
        TRAINROLE_TRAINING = -170002,

        /// <summary>修行时间完结</summary>
        TRAINROLE_TIME_FINISHED = -170003,

        /// <summary>已达到最大解锁栏</summary>
        TRAINROLE_BAR_ENOUGH = -170004,

        /// <summary>武将政务不够</summary>
        TRAINROLE_GOVERN_LACK = -170005,

        /// <summary>没有该武将</summary>
        TRAINROLE_ROLE_LACK = -170006,

        /// <summary>主将不能进行传承</summary>
        TRAINROLE_MAINROLE = -170007,

        /// <summary>刷新次数已满，无法刷新</summary>
        TRAIN_HOME_REFRESH_FULL = -170009,

        /// <summary>正在监狱中，不可偷窃</summary>
        TRAIN_HOME_IN_PRISON = -170010,

        /// <summary>武将不能挑战</summary>
        TRAIN_HOME_NO_CHALLENGE = -170011,

        /// <summary>武将不能喝茶</summary>
        TRAIN_HOME_NO_TEA = -170012,

        /// <summary>未学习偷天术不能偷窃</summary>
        TRAIN_HOME_TOUTIAN_UNLEARN = -170014,

        /// <summary>茶道等级不够</summary>
        TRAIN_HOME_TEA_LEVEL_LACK = -170015,

        /// <summary>武将修炼属性值达到上限</summary>
        TRAIN_ROLE_ATT_ENOUGH = -170016,

        /// <summary>武将已挑战，不能偷窃</summary>
        TRAIN_ROLE_NO_STEAL = -170017,

        /// <summary>该城未开放武将宅</summary>
        TRAIN_ROLE_NO_OPEN = -170018,

        /// <summary>武将身上剩余魂数不够玩家喝茶</summary>
        TRAIN_HOME_SPIRIT_LACK = -170019,

        /// <summary>NPC 武将已挑战</summary>
        TRAIN_HOME_FIGHT_YES = -170020,

        /// <summary>NPC 武将已被偷窃</summary>
        TRAIN_HOME_STEAL_YES = -170021,

        /// <summary>NPC 武将信息不存在</summary>
        TRAIN_HOME_GET_ERROR = -170022,

        /// <summary>连续修行最大次数</summary>
        TRAIN_COUNT_ENOUGH = -170023,

        /// <summary>挑战次数已用完</summary>
        TRAIN_HOME_FIGHT_LACK = -170024,

        /// <summary>可购买次数已用完</summary>
        TRAIN_HOME_BUY_LACK = -170025,

        /// <summary>选择的修炼程度小于玩家可修炼的程度</summary>
        TRAINROLE_DEGREE_WRONG = -170026,

        /// <summary>挑战次数未用完</summary>
        TRAIN_HOME_FIGHT_ERROR = -170027,
        #endregion

        #region  家族模块

        /// <summary>玩家等级小于27级</summary>
        FAMILY_USERLEVEL_LACK = -180001,

        /// <summary>家族名字为空</summary>
        FAMILY_NAME_NULL = -180002,

        /// <summary>存在该家族名字</summary>
        FAMILY_NAME_EXIST = -180003,

        /// <summary>该家族人数已满</summary>
        FAMILY_MEMBER_ENOUGH = -180004,

        /// <summary>没有该操作权利</summary>
        FAMILY_DROIT_LACK = -180005,

        /// <summary>该玩家已加入其他家族</summary>
        FAMILY_MEMBER_EXIST = -180006,

        /// <summary>已达到捐献资源最大值</summary>
        FAMILY_RESOURCE_ENOUGH = -180007,

        /// <summary>玩家不存在</summary>
        FAMILY_PLAYER_NONENTITY = -180008,

        /// <summary>不能邀请自己 </summary>
        FAMILY_NOINVITE_OWN = -180009,

        /// <summary>家族不存在</summary>
        FAMILY_NOEXIST = -180010,

        /// <summary>族长职位不可点 </summary>
        FAMILY_CHIEF_NOPOST = -180011,

        /// <summary>长老人数已足够 </summary>
        FAMILY_ELDER_ENOUGH = -180012,

        /// <summary>家族没有该成员 </summary>
        FAMILY_MEMBER_NOEXIST = -180013,

        /// <summary>俸禄已领取 </summary>
        FAMILY_SALARY_RECEIVE = -180014,

        /// <summary>名字字数不足 </summary>
        FAMILY_NAME_WORDS_LACK = -180015,

        /// <summary>名字字数已够</summary>
        FAMILY_NAME_WORDS_ENOUGH = -180016,

        /// <summary>公告字数已够</summary>
        FAMILY_NOTICE_WORDS_ENOUGH = -180017,

        /// <summary>被邀请人拒绝加入家族</summary>
        FAMILY_REFUSE = -180018,

        /// <summary>名望值已达到最大</summary>
        FAMILY_RENOWN_ENOUGH = -180019,

        #endregion

        #region 好友模块

        /// <summary>查无此人，请重新输入</summary>
        FRIEND_NO_DATA_ERROR = -190001,

        /// <summary>超过最大人数</summary>
        FRIEND_MAX_ERROR = -190002,

        /// <summary>不能添加自己</summary>
        FRIEND_ON_ONESELF = -190003,

        /// <summary>该好友已在好友列表</summary>
        FRIEND_EXIST = -190004,

        /// <summary>该玩家已在黑名单列表</summary>
        FRIEND_BLACKLIST_EXIST = -190005,

        #endregion

        #region 评定系统
        /// <summary> 未找到该武将 </summary>
        APPRAISE_ROLE_LACK = -210001,

        /// <summary> 该武将正在做家臣任务 </summary>
        APPRAISE_ROLE_TASKING = -210002,

        /// <summary> 没有该任务 </summary>
        APPRAISE_TASK_LACK = -210003,

        /// <summary> 该任务已经在做 </summary>
        APPRAISE_TASK_STATEWRONG = -210004,

        /// <summary>刷新错误 </summary>
        APPRAISE_REFLASH_WRONG = -210005,

        /// <summary>家臣身份和主角一样，不能评定 </summary>
        APPRAISE_IDENTIFY_FULL = -210006,

        /// <summary>没有未接受的任务，不能刷新（10-6）</summary>
        APPRAISE_NOTASKS = -210007,
        #endregion

        #region 竞技场模块

        /// <summary>冷却时间错误</summary>
        ARENA_TIME_ERROR = -230001,

        /// <summary>挑战次数不足</summary>
        ARENA_DEKARON_COUNT_LOCK = -230002,

        /// <summary> 没有挑战次数可购买 </summary>
        ARENA_BUY_COUNT_LOCK = -230003,

        #endregion

        #region 称号模块
        /// <summary>称号格子已满</summary>
        TITLE_PACKET_FULL = -240001,

        /// <summary>称号未装备</summary>
        TITLE_UN_LOAD = -240002,

        /// <summary>称号开格已满</summary>
        TITLE_COUNT_FULL = -240003,

        /// <summary>称号未达成</summary>
        TITLE_NO_REACHED = -240004,

        /// <summary>称号已穿戴</summary>
        TITLE_HAS_LOAD = -240005,
        #endregion

        #region 监狱系统
        /// <summary> 不在监狱中</summary>
        PRISON_OUT_ERROR = -250001,

        /// <summary> 留言为空</summary>
        PRISON_MESSAGE_EMPTY = -250002,

        /// <summary> 固定规则表表出错</summary>
        PRISON_BASERULE_ERROR = -250003,

        /// <summary> 今日留言次数已满</summary>
        PRISON_MESSAGE_FULL = -250004,

        /// <summary> 留言版选择的页码不对</summary>
        PRISON_PAGE_ERROR = -250005,

        /// <summary> 服刑时间已到</summary>
        PRISON_TIMEOUT = -250006,

        /// <summary> 坐标错误</summary>
        PRISON_POINT_ERROR = -250007,

        #endregion

        #region 美浓攻略

        /// <summary> 云梯达到最大值</summary>
        SIEGE_MAX_VALUES = -260001,

        /// <summary> 不是己方传送点</summary>
        SIEGE_NO_OWN_PORT = -260002,

        /// <summary> 位置错误</summary>
        SIEGE_POSITION_ERROR = -260003,

        /// <summary> 云梯数量不足</summary>
        SIEGE_YUNTI_ERROR = -260004,

        /// <summary> 没有该玩家数据</summary>
        SIEGE_NO_PLAYER_DATA = -260005,

        /// <summary> BOSS数据错误</summary>
        SIEGE_BOSS_ERROR = -260006,

        /// <summary> 活动未开启</summary>
        SIEGE_NO_OPEN = -260007,

        /// <summary>玩家身份没达到活动要求</summary>
        SIEGE_IDENTIFY_ERROR = -260008,

        /// <summary>冷却时间错误</summary>
        SIEGE_TIME_ERROR = -260009,

        #endregion

        #region 一夜墨俣

        /// <summary>没有找到该玩家活动数据</summary>
        BUILDING_NOT_IN = -270001,

        /// <summary>基表数据错误</summary>
        BUILDING_BASE_ERROR = -270002,

        /// <summary>木材不够</summary>
        BUILDING_WOOD_LACK = -270003,

        /// <summary> 前端所传坐标错误</summary>
        BUILDING_POINT_ERROR = -270004,

        /// <summary> 活动场景错误</summary>
        BUILDING_SCENE_ERROR = -270005,

        /// <summary> BOSS还未打死</summary>
        BUILDING_BOSS_LIVE = -270006,

        /// <summary> 收集木头达到上限</summary>
        BUILDING_WOOD_FULL = -270007,

        /// <summary> 收集火把达到上限</summary>
        BUILDING_TORCH_FULL = -270008,

        /// <summary> 收集建材到达上限</summary>
        BUILDING_BUILD_FULL = -270009,

        /// <summary>玩家坐标不在范围内</summary>
        BUILDING_POINT_OUT = -270010,

        /// <summary>倒计时时间未到</summary>
        BUILDING_TIME_OUT = -270011,

        /// <summary>活动时间未到</summary>
        BUILDING_TIME_ERROR = -270012,

        /// <summary>玩家身份没达到活动要求</summary>
        BUILDING_IDENTIFY_ERROR = -270013,

        /// <summary>今日活动已经结束</summary>
        BUILDING_OVER = -270014,
        #endregion

        #region 工作系统

        /// <summary>任务冷却时间未到</summary>
        WORK_TIME_WRONG = -280001,

        /// <summary>工作已经领取</summary>
        WORK_IS_STARTED = -280002,

        #endregion

        #region 大名系统
        /// <summary>引导任务信息不存在</summary>
        DAMING_GET_NULL = -290001,

        /// <summary>未达到领取标准</summary>
        DAMING_UN_FINISH = -290002,

        /// <summary>奖励已经领取</summary>
        DAMING_IS_REWARD = -290003,

        /// <summary>大名令任务信息错误</summary>
        DAMING_INFO_ERROR = -290004,
        #endregion

        #region 游艺园系统
        /// <summary>游艺园信息不存在</summary>
        GAME_DATA_UNEXIT = -300001,

        /// <summary>剩余闯关次数不足</summary>
        GAME_COUNT_FAIL = -300002,

        /// <summary>游戏数据错误</summary>
        GAME_DATA_ERROR = -300003,

        /// <summary>未达到完成度</summary>
        GAME_REWARD_UNFINISH = -300004,

        /// <summary>未达到领取条件</summary>
        GAME_REWARD_ERROE = -300005,

        /// <summary>奖励已领取</summary>
        GAME_REWARD_RECEIVED = -300006,

        #endregion
    }
}
