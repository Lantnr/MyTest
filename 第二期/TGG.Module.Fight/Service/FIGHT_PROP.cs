using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.Equip;
using TGG.SocketServer;

namespace TGG.Module.Fight.Service
{
    public class FIGHT_PROP
    {
        private static FIGHT_PROP _objInstance;
        /// <summary>FIGHT_PROP单体模式</summary>
        public static FIGHT_PROP GetInstance()
        {
            return _objInstance ?? (_objInstance = new FIGHT_PROP());
        }

        /// <summary> 检测有没有战斗物品</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!Variable.TempProp.ContainsKey(session.Player.User.id))
                return BuildData((int)ResultType.SUCCESS, null, 0);
            var props = Variable.TempProp[session.Player.User.id];
            return BuildData((int)ResultType.SUCCESS, BuildReward(props), 1);
        }

        /// <summary>组装数据 </summary>
        private ASObject BuildData(int result, List<RewardVo> reward, int type)
        {
            var dic = new Dictionary<string, object>()
           {
               {"result", result},
               {"type",type},
               {"reward", reward}
           };
            return new ASObject(dic);
        }

        /// <summary>
        /// 组装奖励数据
        /// </summary>
        private List<RewardVo> BuildReward(List<tg_bag> props)
        {
            var reward = new List<RewardVo>();
            var listequip = props.FindAll(q => q.type == (int)GoodsType.TYPE_EQUIP).ToList();
            var listprops = props.FindAll(q => q.type == (int)GoodsType.TYPE_PROP).ToList();
            if (listequip.Any())
            {
                var listaso = listequip.Select(item => AMFConvert.ToASObject(EntityToVo.ToEquipVo(item))).ToList();
                reward.Add(new RewardVo
            {
                goodsType = (int)GoodsType.TYPE_EQUIP,
                increases = listaso,
            });
            }
            if (!listprops.Any()) return reward;
            var listaso1 = listprops.Select(item => AMFConvert.ToASObject(EntityToVo.ToPropVo(item))).ToList();
            reward.Add(new RewardVo
            {
                goodsType = (int)GoodsType.TYPE_PROP,
                increases = listaso1,
            });

            return reward;






        }
    }
}
