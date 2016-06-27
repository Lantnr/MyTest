using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Props.Service
{
    /// <summary>
    /// 家臣使用
    /// </summary>
    public class PROP_ROLE_USE
    {
        private static PROP_ROLE_USE _objInstance;

        /// <summary>PROP_ROLE_USE单体模式</summary>
        public static PROP_ROLE_USE GetInstance()
        {
            return _objInstance ?? (_objInstance = new PROP_ROLE_USE());
        }

        /// <summary>家臣使用</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "PROP_ROLE_USE", "家臣使用");
#endif
                var id = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);

                var level = session.Player.Role.Kind.role_level;
                var user = session.Player.User.CloneEntity();

                var prop = tg_bag.GetEntityById(id);
                if (prop == null) return ErrorResult((int)ResultType.DATABASE_ERROR);

                var baseProp = Variable.BASE_PROP.FirstOrDefault(m => m.id == prop.base_id);
                if (baseProp == null) return ErrorResult((int)ResultType.BASE_TABLE_ERROR);
                if (baseProp.useMode != 1) return ErrorResult((int)ResultType.PROP_UNUSED);                     //道具不可使用    
                if (level < baseProp.useLevel) return ErrorResult((int)ResultType.BASE_PLAYER_LEVEL_ERROR);     //验证使用等级
                if (user.fame < baseProp.fame) return ErrorResult((int)ResultType.BASE_TABLE_ERROR);            //验证玩家声望  

                var baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7005");     //武将数量上限
                if (baserule == null) return ErrorResult((int)ResultType.BASE_TABLE_ERROR);
                var roles = tg_role.GetFindAllByUserId(user.id);                           //该玩家所有武将信息
                if (roles.Count >= Convert.ToInt32(baserule.value)) return ErrorResult((int)ResultType.ROLE_NUMBER_ENOUGH);//验证是否达到上限
                if (roles.Count(m => m.role_id == baseProp.roleId) > 0) return ErrorResult((int)ResultType.ROLE_EXIST);

                prop.count = prop.count - 1;
                if (prop.count < 0) return ErrorResult((int)ResultType.PROP_LACK);     //验证道具数量

                var baseRoleinfo = Variable.BASE_ROLE.FirstOrDefault(m => m.id == baseProp.roleId);
                if (baseRoleinfo == null) return ErrorResult((int)ResultType.BASE_TABLE_ERROR);

                var baseIdentity = Variable.BASE_IDENTITY.First(m => m.vocation == (int)VocationType.Roles);
                if (baseIdentity == null) return ErrorResult((int)ResultType.BASE_TABLE_ERROR);
                var roleitem = CreateRoleItem(baseRoleinfo, user, baseIdentity);       //创建武将
                if (!tg_role.GetRoleInsert(roleitem.Kind)) return ErrorResult((int)ResultType.DATABASE_ERROR);

                var temp = string.Format("{0}_{1}", "使用道具ID:" + prop.id, "获取家臣:" + roleitem.Kind.id);//日志记录
                (new Log()).WriteLog(prop.user_id, (int)LogType.Use, (int)ModuleNumber.BAG, (int)PropCommand.PROP_ROLE_USE, temp);

                user.fame = user.fame - baseProp.fame;
                user.Update();
                session.Player.User = user;
                var rewards = new List<RewardVo> { new RewardVo { goodsType = (int)GoodsType.TYPE_FAME, value = user.fame } };
                (new Bag()).BuildReward(user.id, new List<tg_bag> { prop }, rewards);
                return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS, roleitem.Kind.id));
            }
            catch (Exception ex)
            {
                XTrace.WriteLine(ex.Message);
                return ErrorResult((int)ResultType.FAIL);
            }
        }

        /// <summary> 获取物品更变Vo 并修改数据库 </summary>
        /// <returns></returns>
        private List<RewardVo> GetRewardVos(TGGSession session, tg_bag prop)
        {
            if (prop.count == 0)
            {
                prop.Delete();
                session.Player.Bag.Surplus += 1;
                session.Player.Bag.BagIsFull = false;
                return new List<RewardVo> { new RewardVo { goodsType = (int)GoodsType.TYPE_PROP, deleteArray = new List<double> { prop.id } } };

            }
            prop.Save();
            return new List<RewardVo>
            {
                new RewardVo
                {
                    goodsType = (int) GoodsType.TYPE_PROP,
                    decreases = (new Bag()).ConvertListASObject(new List<tg_bag> {prop}, "Props")
                }
            };
        }

        private ASObject ErrorResult(int error)
        {
            return new ASObject(Common.GetInstance().BuilData(error, 0));
        }
        /// <summary>创建武将</summary>
        private RoleItem CreateRoleItem(BaseRoleInfo baserole, tg_user user, BaseIdentity baseIdentity)
        {
            var role = new tg_role
            {
                role_id = baserole.id,
                role_identity = baseIdentity.id,
                base_captain = baserole.captain,
                base_force = baserole.force,
                base_brains = baserole.brains,
                base_charm = baserole.charm,
                base_govern = baserole.govern,
                power = baserole.power,
                att_life = baserole.life,
                user_id = user.id,
                role_state = (int)RoleStateType.IDLE,
                role_level = 1,
            };

            return new RoleItem()
            {
                Kind = role,
                LifeSkill = new tg_role_life_skill(),
                FightSkill = new List<tg_role_fight_skill>(),
            };
        }
    }
}
