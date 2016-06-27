9月
----------------------------------------------
		/// <summary>该黑名单已在列表</summary>
		FRIEND_BLACKLIST_EXIST = -190005,

        /// <summary>无该雇佣武将基表数据</summary>
        ROLE_HIRE_ERROR = -70016,

        /// <summary>玩家身份错误</summary>
        ROLE_PLAY_IDENTITY_ERROR = -70017,

        /// <summary>雇佣的武将身份不对应</summary>
        ROLE_HIRE_IDENTITY_ERROR=-70018,

        /// <summary> 已雇佣武将 </summary>
        ROLE_HIRE_SELECTOK=-70019,

		  /// <summary> 没有可招募武将 </summary>
        ROLE_NO_RECRUIT=-70020,

		/// <summary>冷却时间错误</summary>
        SIEGE_TIME_ERROR=-260009,

10月
----------------------------------------------
		/// <summary>不能查看自己信息</summary>
        ROLE_QUERY_MYERROR = -70021,

        /// <summary>当前玩家没有武将信息</summary>
        ROLE_QUERY_NOINFO = -70022,

		/// <summary>不存在该玩家信息</summary>
        ROLE_QUERY_NOPLAYER = -70023,

		/// <summary>账号未激活错误</summary>
        ACTIVATION_ERROR = -201,

	    /// <summary>查询武将视图信息失败</summary>
        ROLE_QUERY_VIEWERROR = -70024,

		/// <summary>该町所属商圈还未开放</summary>
        BUSINESS_TIME_OPEN_ERROR = -30009,

		/// <summary>任务类型错误</summary>
        TASK_VOCATION_TYPEERROR = -20016,

        /// <summary>正在监狱中，不能进行任务</summary>
        TASK_VOCATION_PRISON = -20017,

		/// <summary>购买次数大于讲价可购买次数</summary>
        BUSINESS_BARGAIN_MAX_ERROR = -30010,

		/// <summary>VIP开启马车错误</summary>
        BUSINESS_VIP_CAR_ERROR = -30011,

		 /// <summary> 没有挑战次数可购买 </summary>
        ARENA_BUY_COUNT_LOCK = -230003,

		 /// <summary>该职业任务不是护送类型</summary>
        TASK_NO_ESCORT = -20018,

        /// <summary> 该职业任务步骤为空 </summary>
        TASK_STEP_NULL = -20019,

        /// <summary> 该职业任务步骤已完成 </summary>
        TASK_STEP_OK = -20020,

        /// <summary> 任务步骤与基表不一致 </summary>
        TASK_STEP_ERROR = -20021,

		/// <summary>货物购买量不足</summary>
        BUSINESS_GOODS_BUY_ERROR = -30012,

	    /// <summary> 任务已经开始，不能重复任务 </summary>
        TASK_START_NOW = -20022,

		/// <summary>该商圈已经开启</summary>
        BUSINESS_AREA_ISEXIST_ERROR = -30013,

		/// <summary>VIP商圈次数不足</summary>
        BUSINESS_VIP_AREA_COUNR_ERROR = -30014,

		/// <summary>玩家身份没达到活动要求</summary>
        SIEGE_IDENTIFY_ERROR = -260008,

		/// <summary>VIP购买次数不足</summary>
        BUSINESS_VIP_COUNR_ERROR = -30015,

		/// <summary>技能信息不存在</summary>
        SKILL_GET_ERROR = -160007,

        /// <summary>技能已经学习或升级成功</summary>
        SKILL_LEARN_UP_ERROR = -160008,

	    /// <summary>武将身上剩余魂数不够玩家喝茶</summary>
        TRAIN_HOME_SPIRIT_LACK = -170019,

		/// <summary>町没有停靠马车</summary>
        BUSINESS_RUN_TING_ERROR = -30016,

		   /// <summary>任务冷却时间未到</summary>
        WORK_TIME_WRONG = -280001,

        /// <summary>工作已经领取</summary>
        WORK_IS_STARTED = -280002,

		   /// <summary> 玩家技能等级不够，不能领取任务（10.8） </summary>
        TASK_SKILL_LACK = -20023,

        /// <summary> 站岗玩家下班了（10.19） </summary>
        TASK_CANCLE = -20024,

		/// <summary>技能未开启</summary>
        SKILL_LEARN_OPEN_ERROR = -160009,

		/// <summary>酒馆招募武将位置错误</summary>
        ROLE_RECRUIT_POSTITION_ERROR = -70025,

		///<summary>酒馆招募武将位置已经存在武将</summary>
        ROLE_RECRUIT_ISEXIST_ERROR = -70026,

  11月
  ----------------------------------------------
		/// <summary>引导任务信息不存在</summary>
        DAMING_GET_NULL = -290001,

        /// <summary>未达到领取标准</summary>
        DAMING_UN_FINISH = -290002,

        /// <summary>奖励已经领取</summary>
        DAMING_IS_REWARD = -290003,

        /// <summary>大名令任务信息错误</summary>
        DAMING_INFO_ERROR = -290004,

		/// <summary>今日活动已经结束</summary>
        BUILDING_OVER = -270014,

		/// <summary>武将技能等级不够</summary>
        BASE_ROLE_SKILL_LEVEL_ERROR = -2003,

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

		/// <summary>装备品质未达到要求</summary>
        EQUIP_GRADE_ERROR = -60007,

		/// <summary>该装备洗炼次数已满</summary>
        EQUIP_BAPTIZE_MAX = -60008,

		/// <summary>NPC 武将已挑战</summary>
        TRAIN_HOME_FIGHT_YES = -170020,

        /// <summary>NPC 武将已被偷窃</summary>
        TRAIN_HOME_STEAL_YES = -170021,

        /// <summary>NPC 武将信息不存在</summary>
        TRAIN_HOME_GET_ERROR = -170022,

		 /// <summary>连续修行最大次数</summary>
        TRAIN_COUNT_ENOUGH = -170023,

		/// <summary> 任务步骤数据出错 </summary>
        TASK_STEP_WRONG = -20025,

		/// <summary> 任务已经接受了 </summary>
        TASK_RECEIVED = -20026,

		/// <summary>玩家等级小于27级</summary>
        FAMILY_USERLEVEL_LACK = -180001,

		/// <summary>选择的修炼程度小于玩家可修炼的程度</summary>
        TRAINROLE_DEGREE_WRONG = -170026,

		/// <summary>武将政务不够</summary>
        TRAINROLE_GOVERN_LACK = -170005,

		/// <summary>挑战次数已用完</summary>
        TRAIN_HOME_FIGHT_LACK = -170024,

        /// <summary>可购买次数已用完</summary>
        TRAIN_HOME_BUY_LACK = -170025,

	    /// <summary>挑战次数未用完</summary>
        TRAIN_HOME_FIGHT_ERROR = -170027,
