using FluorineFx;
using NewLife.Log;
using System.Linq;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.Consume;
using TGG.SocketServer;

namespace TGG.Module.User.Service
{
    /// <summary>
    /// 消费类
    /// </summary>
    public class CONSUME
    {
        private static CONSUME _objInstance;

        /// <summary>CONSUME单例模式</summary>
        public static CONSUME GetInstance()
        {
            return _objInstance ?? (_objInstance = new CONSUME());
        }

        /// <summary>消费</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "CONSUME", "消费");
#endif
            #region 参数说明
            //goodsType:int GoodsType 物品类型
            //module:int ModuleNumber 模块号
            //cmd:int 命令号
            //data:Object 业务数据
            #endregion

            var module = int.Parse(data.FirstOrDefault(q => q.Key == "module").Value.ToString());
            var cmd = int.Parse(data.FirstOrDefault(q => q.Key == "cmd").Value.ToString());

            var bll_data = new ASObject();
            switch (module)
            {
                #region 模块
                case (int)ModuleNumber.BUSINESS:
                    {
                        bll_data = CS_BUSINESS(cmd, session, data);
                        break;
                    }
                case (int)ModuleNumber.BAG:
                    {
                        bll_data = CS_Prop(cmd, session, data);
                        break;
                    }
                case (int)ModuleNumber.ROLE:
                    {
                        bll_data = CS_ROLE(cmd, session, data);
                        break;
                    }
                case (int)ModuleNumber.EQUIP:
                    {
                        bll_data = CS_EQUIP(cmd, session, data);
                        break;
                    }
                case (int)ModuleNumber.TASK:
                    {
                        bll_data = CS_TASK(cmd, session, data);
                        break;
                    }
                case (int)ModuleNumber.ROLETRAIN:
                    {
                        bll_data = CS_ROLETRAIN(cmd, session, data);
                        break;
                    }
                case (int)ModuleNumber.FAMILY:
                    {
                        bll_data = CS_FAMILY(cmd, session, data);
                        break;
                    }
                case (int)ModuleNumber.ARENA:
                    {
                        bll_data = CS_ARENA(cmd, session, data);
                        break;
                    }
                case (int)ModuleNumber.APPRAISE:
                    {
                        bll_data = CS_APPRAISE(cmd, session, data);
                        break;
                    }
                case (int)ModuleNumber.PRISON:
                    {
                        bll_data = CS_PRISON(cmd, session, data);
                        break;
                    }
                #endregion
            }
            return new ASObject(Common.GetInstance().BuildData(session.Player, module, cmd, bll_data));
        }

        /// <summary>跑商消耗</summary>
        private ASObject CS_BUSINESS(int cmd, TGGSession session, ASObject data)
        {
            if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.跑商))
                return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            var aso = new ASObject();
            var blldata = data.FirstOrDefault(q => q.Key == "data").Value as ASObject;
            switch (cmd)
            {
                case (int)BusinessCommand.BUSINESS_ACCELERATE:
                    {
                        aso = (new BUSINESS_ACCELERATE()).Execute(session.Player.User.id, blldata);
                        break;
                    }
                case (int)BusinessCommand.BUSINESS_PRICE_INFO:
                    {
                        aso = (new BUSINESS_PRICE_INFO()).Execute(session.Player.User.id, blldata);
                        break;
                    }
                case (int)BusinessCommand.BUSINESS_PACKET_BUY:
                    {
                        aso = (new BUSINESS_PACKET_BUY()).Execute(session.Player.User.id, blldata);
                        break;
                    }
                case (int)BusinessCommand.BUSINESS_GOODS_BUY:
                    {
                        aso = (new BUSINESS_GOODS_BUY()).Execute(session.Player.User.id, blldata);
                        break;
                    }
                case (int)BusinessCommand.BUSINESS_BUY_CAR:
                    {
                        aso = (new BUSINESS_BUY_CAR()).Execute(session.Player.User.id, blldata);
                        break;
                    }
                case (int)BusinessCommand.BUSINESS_GOODS_ADD:
                    {
                        aso = (new BUSINESS_GOODS_ADD()).Execute(session.Player.User.id, blldata);
                        break;
                    }
                case (int)BusinessCommand.BUSINESS_BUY_BARGAIN:
                    {
                        aso = (new BUSINESS_BUY_BARGAIN()).Execute(session.Player.User.id, blldata);
                        break;
                    }
                case (int)BusinessCommand.BUSINESS_AREA_OPEN:
                    {
                        aso = (new BUSINESS_AREA_OPEN()).Execute(session.Player.User.id, blldata);
                        break;
                    }
                case (int)BusinessCommand.BUSINESS_FREE_TAX:
                    {
                        aso = (new BUSINESS_FREE_TAX()).Execute(session.Player.User.id, blldata);
                        break;
                    }
            }
            return aso;
        }

        /// <summary>道具消耗</summary>
        private ASObject CS_Prop(int cmd, TGGSession session, ASObject data)
        {
            var aso = new ASObject();
            var blldata = data.FirstOrDefault(q => q.Key == "data").Value as ASObject;
            switch (cmd)
            {
                case (int)PropCommand.PROP_OPEN_GRID: { return (new PROP_OPEN_GRID()).Execute(session.Player.User.id, blldata); }
            }
            return aso;
        }

        /// <summary>武将模块</summary>
        private ASObject CS_ROLE(int cmd, TGGSession session, ASObject data)
        {
            if (cmd == (int)RoleCommand.RECRUIT_GET ||
                cmd == (int)RoleCommand.RECRUIT_REFRESH
                )
            {
                if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.酒馆))
                    return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            }
            else if (cmd == (int)RoleCommand.ROLE_HIRE)
            {
                if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.忍卫))
                    return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            }
            var aso = new ASObject();
            var blldata = data.FirstOrDefault(q => q.Key == "data").Value as ASObject;
            switch (cmd)
            {
                case (int)RoleCommand.RECRUIT_GET: { return (new RECRUIT_GET()).Execute(session.Player.User.id, blldata); }
                case (int)RoleCommand.RECRUIT_REFRESH: { return (new RECRUIT_REFRESH()).Execute(session.Player.User.id, blldata); }
                case (int)RoleCommand.POWER_BUY:
                    {
                        aso = (new POWER_BUY()).Execute(session.Player.User.id, blldata);
                        break;
                    }
                case (int)RoleCommand.ROLE_HIRE: { return (new ROLE_HIRE()).Execute(session.Player.User.id, blldata); }
                case (int)RoleCommand.ROLE_POWER_BUY:
                    {

                        aso = (new ROLE_POWER_BUY()).Execute(session.Player.User.id, blldata);
                        break;
                    }
            }
            return aso;
        }

        /// <summary>竞技场消耗</summary>
        private ASObject CS_ARENA(int cmd, TGGSession session, ASObject data)
        {
            if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.竞技场))
                return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            var aso = new ASObject();
            var blldata = data.FirstOrDefault(q => q.Key == "data").Value as ASObject;
            switch (cmd)
            {
                case (int)ArenaCommand.ARENA_DEKARON_ADD: { return (new ARENA_DEKARON_ADD()).Execute(session.Player.User.id, blldata); }
                case (int)ArenaCommand.ARENA_REMOVE_COOLING: { return (new ARENA_REMOVE_COOLING()).Execute(session.Player.User.id, blldata); }
            }
            return aso;
        }

        /// <summary>自宅系统消耗 </summary>
        private ASObject CS_APPRAISE(int cmd, TGGSession session, ASObject data)
        {
            if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.自宅))
                return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);

            var aso = new ASObject();
            var blldata = data.FirstOrDefault(q => q.Key == "data").Value as ASObject;
            switch (cmd)
            {
                case (int)AppraiseCommand.TASK_REFLASH:
                    {
                        aso = (new TASK_REFLASH()).Execute(session.Player.User.id, blldata);
                        break;
                    }
                case (int)AppraiseCommand.TASK_BUY:
                    {
                        aso = (new TASK_BUY()).Execute(session.Player.User.id, blldata);
                        break;
                    }
            }
            return aso;
        }

        /// <summary>装备模块</summary>
        private ASObject CS_EQUIP(int cmd, TGGSession session, ASObject data)
        {
            var aso = new ASObject();
            var blldata = data.FirstOrDefault(q => q.Key == "data").Value as ASObject;
            switch (cmd)
            {
                case (int)EquipCommand.EQUIP_SPIRIT_LOCK:
                    {
                        aso = (new EQUIP_SPIRIT_LOCK()).Execute(session.Player.User.id, blldata);
                        break;
                    }
                case (int)EquipCommand.EQUIP_BUY:
                    {
                        aso = (new EQUIP_BUY()).Execute(session.Player.User.id, blldata);
                        break;
                    }
            }
            return aso;
        }

        /// <summary>职业评定模块</summary>
        private ASObject CS_TASK(int cmd, TGGSession session, ASObject data)
        {
            if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.职业评定))
                return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);

            var aso = new ASObject();
            var blldata = data.FirstOrDefault(q => q.Key == "data").Value as ASObject;
            switch (cmd)
            {
                case (int)TaskCommand.TASK_VOCATION_BUY:
                    {
                        aso = (new TASK_VOCATION_BUY()).Execute(session.Player.User.id, blldata);
                        break;
                    }
                case (int)TaskCommand.TASK_VOCATION_REFRESH:
                    {
                        aso = (new TASK_VOCATION_REFRESH()).Execute(session.Player.User.id, blldata);
                        break;
                    }
            }
            return aso;
        }

        /// <summary>武将修行模块</summary>
        private ASObject CS_ROLETRAIN(int cmd, TGGSession session, ASObject data)
        {
            switch (cmd)
            {
                case (int)RoleTrainCommand.TRAIN_HOME_NPC_REFRESH:
                    if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.自宅))
                        return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
                    break;
                case (int)RoleTrainCommand.TRAIN_ROLE_START:
                case (int)RoleTrainCommand.TRAIN_ROLE_ACCELERATE:
                case (int)RoleTrainCommand.TRAIN_ROLE_LOCK:
                    if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.修行))
                        return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
                    break;
                default:
                    if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.武将宅))
                        return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
                    break;
            }
            var aso = new ASObject();
            var blldata = data.FirstOrDefault(q => q.Key == "data").Value as ASObject;
            switch (cmd)
            {
                case (int)RoleTrainCommand.TRAIN_ROLE_START:
                    {
                        aso = (new TRAIN_ROLE_START()).Execute(session.Player.User.id, blldata); break;
                    }
                case (int)RoleTrainCommand.TRAIN_ROLE_ACCELERATE:
                    {
                        aso = (new TRAIN_ROLE_ACCELERATE()).Execute(session.Player.User.id, blldata); break;
                    }
                case (int)RoleTrainCommand.TRAIN_ROLE_LOCK:
                    {
                        aso = (new TRAIN_ROLE_LOCK()).Execute(session.Player.User.id, blldata); break;
                    }
                case (int)RoleTrainCommand.TRAIN_HOME_NPC_REFRESH:
                    {
                        aso = (new TRAIN_HOME_NPC_REFRESH()).Execute(session.Player.User.id, blldata); break;
                    }
                case (int)RoleTrainCommand.TRAIN_TEA_INSIGHT:
                    {
                        aso = (new TRAIN_TEA_INSIGHT()).Execute(session.Player.User.id, blldata); break;
                    }
                case (int)RoleTrainCommand.TRAIN_HOME_FIGHT_BUY:
                    {
                        aso = (new TRAIN_HOME_FIGHT_BUY()).Execute(session.Player.User.id, blldata); break;
                    }
            }
            return aso;
        }

        /// <summary>家族模块</summary>
        private ASObject CS_FAMILY(int cmd, TGGSession session, ASObject data)
        {
            if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.家族))
                return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            var aso = new ASObject();
            var blldata = data.FirstOrDefault(q => q.Key == "data").Value as ASObject;
            switch (cmd)
            {
                case (int)FamilyCommand.FAMILY_CREATE:
                    {
                        aso = (new FAMILY_CREATE()).Execute(session.Player.User.id, blldata); break;
                    }
                case (int)FamilyCommand.FAMILY_DONATE:
                    {
                        aso = (new FAMILY_DONATE()).Execute(session.Player.User.id, blldata); break;
                    }
                case (int)FamilyCommand.FAMILY_RECEIVE:
                    {
                        aso = (new FAMILY_RECEIVE()).Execute(session.Player.User.id, blldata); break;
                    }
            }
            return aso;
        }

        /// <summary>监狱系统模块</summary>
        private ASObject CS_PRISON(int cmd, TGGSession session, ASObject data)
        {
            var aso = new ASObject();
            var blldata = data.FirstOrDefault(q => q.Key == "data").Value as ASObject;
            switch (cmd)
            {
                case (int)PrisonCommand.MESSAGE:
                    {
                        aso = (new MESSAGE()).Execute(session.Player.User.id, blldata); break;
                    }
                case (int)PrisonCommand.BREAK:
                    {
                        aso = (new BREAK()).Execute(session.Player.User.id, blldata); break;
                    }
            }
            return aso;
        }
    }
}
