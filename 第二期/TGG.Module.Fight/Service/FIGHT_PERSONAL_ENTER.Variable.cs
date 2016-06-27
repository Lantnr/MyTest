using System.Collections.Generic;
using TGG.Core.Entity;
using TGG.Core.Vo.Fight;
using TGG.Core.Vo.Role;

namespace TGG.Module.Fight.Service
{
    /// <summary>
    /// 战斗部分类 - 变量类
    /// </summary>
    public partial class FIGHT_PERSONAL_ENTER
    {
        /// <summary> 攻击方出手位置 </summary>
        public int attack_number = 0;

        /// <summary> 防守方出手位置 </summary>
        public int defense_number = 0;

        /// <summary> 当前回合数 </summary>
        public int Round = 0;

        /// <summary> 当前是否攻击方发起攻击 </summary>
        public bool IsAttack = true;

        /// <summary>战斗数据</summary>
        public FightVo vo = new FightVo();

        /// <summary>双方阵中武将信息集合 血量随着变化</summary>
        public List<FightRole> list_role = new List<FightRole>();

        /// <summary>双方阵中武将信息集合 血量固定 </summary>
        public List<FightRole> list_role_hp = new List<FightRole>();

        /// <summary>攻击方阵中武将信息</summary>
        public List<FightRole> list_attack_role = new List<FightRole>();

        /// <summary>防守方阵中武将信息</summary>
        public List<FightRole> list_defense_role = new List<FightRole>();

        /// <summary>攻击方的阵形</summary>
        public FightPersonal attack_matrix = new FightPersonal();

        /// <summary>防守方的阵形</summary>
        public FightPersonal defense_matrix = new FightPersonal();

        /// <summary> 所有武将身上Buff </summary>
        public List<FightRoleBuff> list_buff = new List<FightRoleBuff>();

        /// <summary> 双方阵中武将技能集合 </summary>
        public List<SkillVo> list_skill = new List<SkillVo>();

        /// <summary>单个出招过程</summary>
        public MovesVo move = new MovesVo();

        /// <summary>出招过程</summary>
        public List<MovesVo> list_move = new List<MovesVo>();

        /// <summary> 武将所在的回合  key:武将id  value:回合数</summary>
        public Dictionary<decimal, int> dic_round = new Dictionary<decimal, int>();

        /// <summary> 玩家当前印数  key:用户id  value:印数数</summary>
        public Dictionary<decimal, int> dic_yincount = new Dictionary<decimal, int>();

        /// <summary> 玩家职业系数  key:用户id  value:职业系数</summary>
        public Dictionary<decimal, double> dic_vocation = new Dictionary<decimal, double>();


        /// <summary>谁胜谁负 0:先手胜 1:后手胜</summary>
        public int WIN = 0;

        /// <summary>对手战斗类型 </summary>
        public int RivalFightType;

        /// <summary>对手昵称 </summary>
        public string RivalName = "";

        /// <summary> 武将宅时使用 </summary>
        public int RoleHomeId;
    }
}
