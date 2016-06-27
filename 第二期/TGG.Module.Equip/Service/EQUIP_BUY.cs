using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;
using XCode;

namespace TGG.Module.Equip.Service
{
    /// <summary>
    /// 装备购买
    /// 开发者：李德雁
    /// </summary>
    public class EQUIP_BUY
    {
        private static EQUIP_BUY ObjInstance;

        public static EQUIP_BUY GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new EQUIP_BUY());
        }
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_COIN, session, data);
        }

        private List<RewardVo> _rewardlist;

        public ASObject CommandStart(int goodstype, TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "EQUIP_BUY", "装备买");
#endif
                var baseid = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "id").Value);
                var count = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "count").Value);

                if (count <= 0 || baseid == 0) return ResultData((int)ResultType.FRONT_DATA_ERROR);//前端数据不对 
                var baseinfo = Variable.BASE_EQUIP.FirstOrDefault(q => q.id == baseid);
                if (baseinfo == null || baseinfo.grade != 1) //前端数据验证
                    return ResultData((int)ResultType.BASE_TABLE_ERROR);//该装备不能购买
                var user = session.Player.User.CloneEntity();
                var price = baseinfo.buyPrice;
                if (!CheckMoney(session, price * count)) //金钱验证
                    return ResultData((int)ResultType.BASE_PLAYER_COIN_ERROR);
                if (!CheckBox(session, count)) //格子数验证
                    return ResultData((int)ResultType.EQUIP_BAG_FULL);
                var equiplist = new List<tg_bag>();
                var newequip = new tg_bag();
                GetEntity(baseinfo, session.Player.User.id, newequip);//基本属性设置
                AddAtt(newequip, baseinfo); //根据装备类型设置属性
                for (var i = 0; i < count; i++) equiplist.Add(newequip.CloneEntity());

                CreateLog(user.id,user.coin, baseid, count, price * count);

                var list = tg_bag.GetSaveList(equiplist); //批量插入   
                SendReward(list, session);
                return ResultData((int)ResultType.SUCCESS);
            }
            catch (Exception ex)
            {
                XTrace.WriteLine("{0}:{1}", "EQUIP_BUY", "装备买错误{0}", ex.Message);
                return new ASObject();
            }
        }

        /// <summary>组装数据 </summary>
        private ASObject ResultData(int result)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
            };
            return new ASObject(dic);

        }

        /// <summary> 新装备属性赋值</summary>
        private void GetEntity(BaseEquip baseinfo, Int64 userid, tg_bag newequip)
        {
            newequip.user_id = userid;
            newequip.type = (int)GoodsType.TYPE_EQUIP;
            newequip.base_id = Convert.ToInt32(baseinfo.id);
            newequip.equip_type = baseinfo.typeSub;
            newequip.state = 0;
        }

        /// <summary> 金钱验证</summary>
        private bool CheckMoney(TGGSession session, int money)
        {
            _rewardlist = new List<RewardVo>();
            var user = session.Player.User.CloneEntity();
            if (user.coin < money) return false;
            user.coin -= money;
            if (user.Update() == 0) return false;
            session.Player.User = user;       
            _rewardlist.Add(new RewardVo()
            {
                goodsType = (int)GoodsType.TYPE_COIN,
                value = user.coin,
            });
            return true;
        }

        /// <summary>验证背包格子数 </summary>
        private bool CheckBox(TGGSession session, int count)
        {
            var leftcount = session.Player.Bag.Surplus - count;
            if (leftcount < 0)
                return false;
            session.Player.Bag.Surplus -= count;
            if (leftcount == 0)
            {
                session.Player.Bag.BagIsFull = true;
            }
            return true;
        }

        /// <summary> 根据装备类型，获取装备基本属性 </summary>
        private void AddAtt(tg_bag newequip, BaseEquip baseinfo)
        {
            switch (baseinfo.typeSub)
            {
                #region  装备类别
                case (int)EquipType.ARMOR: //铠甲
                    {
                        newequip.equip_type = (int)EquipType.ARMOR;
                        newequip.attribute1_type = (int)RoleAttributeType.ROLE_DEFENSE;
                        newequip.attribute1_value = baseinfo.defense;

                    }
                    break;
                case (int)EquipType.ARTWORK: //艺术品
                    {
                        newequip.equip_type = (int)EquipType.ARTWORK;
                        newequip.attribute1_type = (int)RoleAttributeType.ROLE_GOVERN;
                        newequip.attribute1_value = baseinfo.govern;
                    }
                    break;
                case (int)EquipType.BOOK: //书籍
                    {
                        newequip.equip_type = (int)EquipType.BOOK;
                        newequip.attribute1_type = (int)RoleAttributeType.ROLE_BRAINS;
                        newequip.attribute1_value = baseinfo.brains;
                    }
                    break;
                case (int)EquipType.JEWELRY: //珠宝
                    {
                        newequip.equip_type = (int)EquipType.JEWELRY;
                        newequip.attribute1_type = (int)RoleAttributeType.ROLE_CHARM;
                        newequip.attribute1_value = baseinfo.charm;
                    }
                    break;
                case (int)EquipType.MOUNTS: //坐骑
                    {
                        newequip.equip_type = (int)EquipType.MOUNTS;
                        newequip.attribute1_type = (int)RoleAttributeType.ROLE_LIFE;
                        newequip.attribute1_value = baseinfo.life;
                    }
                    break;
                case (int)EquipType.NANBAN: //南蛮物
                    {
                        newequip.equip_type = (int)EquipType.NANBAN;
                        newequip.attribute1_type = (int)RoleAttributeType.ROLE_FORCE;
                        newequip.attribute1_value = baseinfo.force;
                    }
                    break;
                case (int)EquipType.TEA: //茶器
                    {
                        newequip.equip_type = (int)EquipType.TEA;
                        newequip.attribute1_type = (int)RoleAttributeType.ROLE_CAPTAIN;
                        newequip.attribute1_value = baseinfo.captain;
                    }
                    break;
                case (int)EquipType.WEAPON: //武器
                    {
                        newequip.equip_type = (int)EquipType.WEAPON;
                        newequip.attribute1_type = (int)RoleAttributeType.ROLE_ATTACK;
                        newequip.attribute1_value = baseinfo.attack;
                    }
                    break;
                #endregion
            }
        }

        /// <summary> 推送装备 </summary>
        private void SendReward(List<tg_bag> equiplist, TGGSession session)
        {
            var equipsvo = Common.GetInstance().ConvertListASObject(equiplist);
            _rewardlist.Add(new RewardVo()
            {
                goodsType = (int)GoodsType.TYPE_EQUIP,
                increases = equipsvo,
            });
            (new Share.User()).REWARDS_API(session.Player.User.id, _rewardlist);

        }

        /// <summary>购买装备日志</summary>
        private void CreateLog(Int64 userid,Int64 coin, int baseid, int count, int money)
        {
            var _coin = coin;
            coin -= money;
            //日志
            var logdata = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", "Coin", "EquipBuy", baseid, _coin, money, coin);
            (new Share.Log()).WriteLog(userid, (int)LogType.Use, (int)ModuleNumber.EQUIP, (int)EquipCommand.EQUIP_BUY, logdata);

            var equips = tg_bag.GetFindBag(userid, baseid);
            var _count = equips.Count + count;
            //日志
            var _logdata = string.Format("{0}_{1}_{2}_{3}_{4}", "EquipBuy", baseid, equips.Count, count, _count);
            (new Share.Log()).WriteLog(userid, (int)LogType.Get, (int)ModuleNumber.EQUIP, (int)EquipCommand.EQUIP_BUY, _logdata);
        }
    }
}
