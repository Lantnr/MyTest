using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.SocketServer;

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 武将传承
    /// 开发者：李德雁
    /// </summary>
    public class TRAIN_INHERIT_ROLE
    {
        public static TRAIN_INHERIT_ROLE objInstance = null;

        /// <summary> TRAIN_INHERIT_ROLE单体模式 </summary>
        public static TRAIN_INHERIT_ROLE getInstance()
        {
            return objInstance ?? (objInstance = new TRAIN_INHERIT_ROLE());
        }

        private TRAIN_INHERIT_ROLE()
        {
            var baseIdentity = Variable.BASE_IDENTITY.FirstOrDefault(q => q.vocation == (int)VocationType.Roles);
            if (baseIdentity != null)
                _firstidentify = baseIdentity.id;
        }

        private tg_role _fatherrole;
        private tg_role _sonrole;
        private readonly int _firstidentify;
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "TRAIN_INHERIT_ROLE", "武将传承\r\n");
#endif
            const int result = (int)ResultType.SUCCESS;
            var leftid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "left").Value);
            var rightid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "right").Value);
            var mainroleid = session.Player.Role.Kind.id;
            //主将不可传承和被传承
            if (mainroleid == leftid || mainroleid == rightid) return new ASObject(BuildData((int)ResultType.TRAINROLE_MAINROLE));
            _fatherrole = tg_role.FindByid(leftid);
            _sonrole = tg_role.FindByid(rightid);
            //验证前端传输的数据
            if (_fatherrole == null || _sonrole == null) return new ASObject(BuildData((int)ResultType.TRAINROLE_ROLE_LACK));

            GetSonRoleInfo();
            _sonrole.Update();
            GetFatherRoleInfo();
            _fatherrole.Update();

            return new ASObject(BuildData(result));
        }


        /// <summary>传承武将的信息</summary>
        private void GetSonRoleInfo()
        {
            var baserole = Variable.BASE_ROLE.FirstOrDefault(q => q.id == _fatherrole.role_id);
            if (baserole == null) return;
            //传承属性
            GetNewLevel();
            GetNewIdentify();
        }

        /// <summary>
        /// 被传承武将的信息
        /// </summary>
        private void GetFatherRoleInfo()
        {
            var baserole = Variable.BASE_ROLE.FirstOrDefault(q => q.id == _fatherrole.role_id);
            if (baserole == null) return;
            var frb = Variable.BASE_ROLELVUPDATE.Where(q => q.level <= _fatherrole.role_level - 1).Sum(q => q.AddBlood);//加成血量
            _fatherrole.role_level = 1;
            _fatherrole.att_life -= frb;
            if (_fatherrole.att_life < 100) _fatherrole.att_life = baserole.life;
            _fatherrole.role_exp = 0;
            if (_firstidentify != 0)
                _fatherrole.role_identity = _firstidentify;
            _fatherrole.role_honor = 0;
        }

        /// <summary> 获取新的等级 </summary>
        private void GetNewLevel()
        {
            if (_fatherrole.role_level <= _sonrole.role_level) return;
            if (_fatherrole.role_level <= 1) return;
            var sumexp = Variable.BASE_ROLELVUPDATE.Take(_fatherrole.role_level - 1).Sum(q => q.exp);

            sumexp += _fatherrole.role_exp;
            var newexp = Math.Floor(Convert.ToDouble(sumexp / 2));
            var newlevel = 1;
            foreach (var item in Variable.BASE_ROLELVUPDATE)
            {
                if (newexp - item.exp >= 0)
                {
                    if (item != Variable.BASE_ROLELVUPDATE.LastOrDefault())
                    {
                        newlevel++;
                    }
                    newexp -= item.exp;
                }
                else break;
            }
            if (newlevel <= _sonrole.role_level) return;
            var blood = Variable.BASE_ROLELVUPDATE.Where(q => q.level >= _sonrole.role_level && q.level <= newlevel - 1).Sum(q => q.AddBlood);//加成血量
            _sonrole.att_life += blood;
            //请牢记,先改血量再更新等级,不然sum的结果就一个
            _sonrole.role_level = newlevel;
            _sonrole.role_exp = Convert.ToInt32(newexp);
        }

        /// <summary> 获取新的身份 </summary>
        private void GetNewIdentify()
        {
            if (_fatherrole.role_identity <= _sonrole.role_identity) return;
            if (_fatherrole.role_identity == _firstidentify) return;
            var baseinfo = Variable.BASE_IDENTITY.Where(q => q.vocation == (int)VocationType.Roles).ToList();

            var sumhonor = baseinfo.Where(q => q.id >= _firstidentify && q.id < _fatherrole.role_identity).Sum(q => q.honor); //升到该身份总功勋值
            sumhonor = tg_user.IsHonorMax(sumhonor, _fatherrole.role_honor);
            var newhonor = Math.Floor(Convert.ToDouble(sumhonor / 2));
            var newidentify = _firstidentify;       //身份初始值
            foreach (var item in baseinfo)   //判断新功勋值能到达的身份
            {
                if (newhonor - item.honor >= 0)
                {
                    if (item != baseinfo.LastOrDefault())
                    {
                        newidentify++;
                    }
                    newhonor -= item.honor;
                }
                else break;
            }
            if (newidentify < _sonrole.role_identity) return;
            _sonrole.role_identity = newidentify;              //获取新的身份值
            _sonrole.role_honor = Convert.ToInt32(newhonor);
        }

        /// <summary>组装数据 </summary>
        private Dictionary<string, object> BuildData(int result)
        {
            //组装武将vo
            var fatherrolevo = _fatherrole != null ? (new Share.Role()).BuildRole(_fatherrole.id) : null;
            var sonrolevo = _fatherrole != null ? (new Share.Role()).BuildRole(_sonrole.id) : null;
            var dic = new Dictionary<string, object>()
            {
                {"result",result },
                {"left", fatherrolevo},
                {"right",sonrolevo}
            };
            return dic;
        }

        ///// <summary>获取装备的加成属性</summary>
        //private void GetEquip(decimal baseid, int type)
        //{
        //    if (baseid == 0) return;
        //    var equip = tg_equip.FindByid(baseid);
        //    switch (type)
        //    {
        //        case 0: //减少属性
        //            FatherRoleReduce(equip.attribute1_type, equip.attribute1_value);
        //            FatherRoleReduce(equip.attribute2_type, equip.attribute2_value);
        //            FatherRoleReduce(equip.attribute3_type, equip.attribute3_value);
        //            break;
        //        case 1: //增加属性
        //            FatherRoleAdd(equip.attribute1_type, equip.attribute1_value);
        //            FatherRoleAdd(equip.attribute2_type, equip.attribute2_value);
        //            FatherRoleAdd(equip.attribute3_type, equip.attribute3_value);
        //            break;
        //    }
        //}

        ///// <summary> 减去装备的加成属性</summary>
        //private void FatherRoleAdd(int attribute_type, int attribute_value)
        //{
        //    switch (attribute_type)
        //    {
        //        case (int)RolePropertyType.BRAINS:
        //            fatherrole.base_brains += attribute_value;
        //            break;
        //        case (int)RolePropertyType.CATTAIN:
        //            fatherrole.base_captain += attribute_value;
        //            break;
        //        case (int)RolePropertyType.CHARM:
        //            fatherrole.base_charm += attribute_value;
        //            break;
        //        case (int)RolePropertyType.FORCE:
        //            fatherrole.base_force += attribute_value;
        //            break;
        //        case (int)RolePropertyType.GOVERN:
        //            fatherrole.base_govern += attribute_value;
        //            break;
        //    }
        //}

        ///// <summary> 减去装备的加成属性</summary>
        //private void FatherRoleReduce(int attribute_type, int attribute_value)
        //{
        //    switch (attribute_type)
        //    {
        //        case (int)RolePropertyType.BRAINS:
        //            fatherrole.base_brains -= attribute_value;
        //            break;
        //        case (int)RolePropertyType.CATTAIN:
        //            fatherrole.base_captain -= attribute_value;
        //            break;
        //        case (int)RolePropertyType.CHARM:
        //            fatherrole.base_charm -= attribute_value;
        //            break;
        //        case (int)RolePropertyType.FORCE:
        //            fatherrole.base_force -= attribute_value;
        //            break;
        //        case (int)RolePropertyType.GOVERN:
        //            fatherrole.base_govern -= attribute_value;
        //            break;
        //    }
        //}

    }
}
