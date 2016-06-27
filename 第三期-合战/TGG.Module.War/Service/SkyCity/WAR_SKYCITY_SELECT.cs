using System;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.SkyCity
{
    /// <summary>
    /// 选择武将
    /// </summary>
    public class WAR_SKYCITY_SELECT : IDisposable
    {
        //private static WAR_SKYCITY_SELECT _objInstance;

        ///// <summary> WAR_SKYCITY_SELECT单体模式 </summary>
        //public static WAR_SKYCITY_SELECT GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_SKYCITY_SELECT());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 选择武将</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "WAR_SKYCITY_SELECT", "选择武将");
#endif
            if (!data.ContainsKey("id") || !data.ContainsKey("type")) return null;   //验证前端参数信息
            var id = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);
            var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);

            if (id == 0 || type == 0) return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);
            var warrole = tg_war_role.GetEntityById(id);
            if (warrole == null) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
            if (warrole.user_id != session.Player.User.id) return null;   //验证玩家信息
            if (warrole.type == (int)WarRoleType.NPC) return null;   //验证是否为备大将
            warrole.state = type;
            return WarRoleSelect(warrole);
        }

        /// <summary>进入方法分类</summary>
        private ASObject WarRoleSelect(tg_war_role warrole)
        {
            int resource = 0;
            switch (warrole.state)   //根据天守阁任务类型进行分类处理
            {
                case (int)WarRoleStateType.ASSART:
                case (int)WarRoleStateType.BUILDING:
                case (int)WarRoleStateType.BUILD_ADD:
                case (int)WarRoleStateType.MINING:
                case (int)WarRoleStateType.PEACE:
                    resource = GetInterior(warrole);
                    break;
                case (int)WarRoleStateType.LEVY:
                    resource = GetLevyArmy(warrole);
                    break;
                case (int)WarRoleStateType.TRAIN:
                    resource = GetTrain(warrole);
                    break;
            }
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, resource));
        }

        /// <summary>内政资源</summary>
        private int GetInterior(tg_war_role warrole)
        {
            int resource;
            if (warrole.type == (int)WarRoleType.PLAYER) //家臣
            {
                var role = tg_role.GetEntityById(warrole.rid);
                if (role == null) return 0;

                var level = Common.GetInstance().GetLifeLevel(warrole.rid, warrole.state);
                var value = Common.GetInstance().GetRoleAtt(role, warrole.state);
                resource = GetResource(warrole, value, level);
                resource += new Share.War().GetCharacterEffect(warrole);
                resource += new Share.War().GetTacticsValue(warrole.user_id, warrole.state);
            }
            else //备大将
            {
                resource = Common.GetInstance().GetResource(warrole);
                resource += new Share.War().GetTacticsValue(warrole.user_id, warrole.state);
            }
            return resource;
        }

        /// <summary>计算徵兵数量</summary>
        private int GetLevyArmy(tg_war_role warrole)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32044");
            if (rule == null) return 0;
            int resource;
            if (warrole.type == 0)  //普通玩家武将
            {
                var role = tg_role.GetEntityById(warrole.rid);
                if (role == null) return 0;

                //计算武将魅力点总数
                var charm = tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, role);
                //获取生活技能等级信息
                var level = Common.GetInstance().GetLifeLevel(warrole.rid, warrole.state);
                //计算生活技能效果值
                var effect = Common.GetInstance().GetLifeValue(warrole.state, level);
                resource = Convert.ToInt32(Common.GetInstance().GetRule(rule, charm) + effect);  //普通武将徵兵数量= 魅力值+军学技能效果值
            }
            else
            {
                var baseInfo = Variable.BASE_ROLE.FirstOrDefault(m => m.id == warrole.rid);
                if (baseInfo == null) return 0;
                var pcharm = baseInfo.charm;   //固定魅力属性点
                var peffect = Common.GetInstance().GetLifeValue(warrole.state, 1);  //固定生活技能等级为1级
                resource = Convert.ToInt32(Common.GetInstance().GetRule(rule, pcharm) + peffect);   //备大将徵兵= 魅力值+军学技能效果值
            }
            resource += (new Share.War()).GetTacticsValue(warrole.user_id, (int)WarRoleStateType.LEVY);   //内政策略加成的徵兵效果值
            return resource;
        }

        /// <summary>训练士气</summary>
        private int GetTrain(tg_war_role warrole)
        {
            int resource;
            if (warrole.type == (int)WarRoleType.PLAYER) //家臣
            {
                var role = tg_role.FindByid(warrole.rid);
                if (role == null) return 0;
                var level = Common.GetInstance().GetLifeLevel(warrole.rid, warrole.state);
                var value = Common.GetInstance().GetRoleAtt(role, warrole.state);
                resource = GetResource(warrole, value, level);
                resource += new Share.War().GetCharacterEffect(warrole);
                resource += Common.GetInstance().GetCharacterMorale(warrole.user_id);
            }
            else //备大将
                resource = Common.GetInstance().GetResource(warrole); ;
            return resource;
        }

        /// <summary>获取内政开发一次的值</summary>
        private int GetResource(tg_war_role warrole, double govern, int level)
        {
            var effect = Common.GetInstance().GetLifeValue(warrole.type, level);
            var value = Common.GetInstance().GetInteriorValue(warrole.state, govern, effect);
            return value;
        }
    }
}
