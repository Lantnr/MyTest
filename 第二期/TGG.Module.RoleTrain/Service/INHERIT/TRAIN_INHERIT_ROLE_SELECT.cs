using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 武将传承选择后数值变化
    /// 开发者：李德雁
    /// </summary>
    public class TRAIN_INHERIT_ROLE_SELECT
    {
        public static TRAIN_INHERIT_ROLE_SELECT objInstance = null;

        private double SumHonor = 0d;
        public static TRAIN_INHERIT_ROLE_SELECT getInstance()
        {
            return objInstance ?? (objInstance = new TRAIN_INHERIT_ROLE_SELECT());
        }
        private TRAIN_INHERIT_ROLE_SELECT()
        {
            var firstidentify = Variable.BASE_IDENTITY.FirstOrDefault(q => q.vocation == (int)VocationType.Roles);
            if (firstidentify != null)
                _firstidentify = firstidentify.id;
        }

        private readonly int _firstidentify;
        private tg_role _fatherrole;
        private tg_role _sonrole;
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "TRAIN_INHERIT_ROLE_SELECT", "武将传承选择后数值变化\r\n");
#endif
            const int result = (int)ResultType.SUCCESS;
            var leftid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "left").Value);
            var rightid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "right").Value);
            var mainroleid = session.Player.Role.Kind.id;
            if (mainroleid == leftid || mainroleid == rightid) return new ASObject(BuildData(null, (int)ResultType.TRAINROLE_MAINROLE));
            _fatherrole = tg_role.FindByid(leftid);
            _sonrole = tg_role.FindByid(rightid);
            if (_fatherrole == null || _sonrole == null) return new ASObject(BuildData(null, (int)ResultType.TRAINROLE_ROLE_LACK));
            var adddata = GetAddData();
            return new ASObject(BuildData(adddata, result));
        }

        /// <summary> 获取传承增加值</summary>
        private List<double> GetAddData()
        {
            var data = new List<double> { GetNewLevel(), GetNewIdentify(), SumHonor };
            return data;
        }

        /// <summary> 获取新的等级 </summary>
        private double GetNewLevel()
        {
            if (_fatherrole.role_level <= _sonrole.role_level) return _sonrole.role_level;
            if (_fatherrole.role_level <= 1) return _sonrole.role_level;
            var sumexp = Variable.BASE_ROLELVUPDATE.Take(_fatherrole.role_level - 1).Sum(q => q.exp);
            sumexp += _fatherrole.role_exp;
            var newexp = Math.Floor(Convert.ToDouble(sumexp / 2));
            var newlevel = 1;
            foreach (var item in Variable.BASE_ROLELVUPDATE)
            {
                if (newexp - item.exp >= 0)
                {
                    if (item != Variable.BASE_ROLELVUPDATE.LastOrDefault()) //满级不再升级
                    {
                        newlevel++;
                    }
                    newexp -= item.exp;
                }
                else
                {
                    break;
                }
            }
            if (newlevel < _sonrole.role_level) return _sonrole.role_level;
            _sonrole.role_level = newlevel;
            _sonrole.role_exp = Convert.ToInt32(newexp);
            return newlevel > _sonrole.role_level ? newlevel : _sonrole.role_level;
        }

        /// <summary> 获取新的身份 </summary>
        private double GetNewIdentify()
        {
            if (_fatherrole.role_identity <= _sonrole.role_identity) return _sonrole.role_identity;
            if (_fatherrole.role_identity == _firstidentify) return _sonrole.role_identity;
            var baseinfo = Variable.BASE_IDENTITY.Where(q => q.vocation == (int)VocationType.Roles).ToList();
            var sumhonor = baseinfo.Where(q => q.id >= _firstidentify && q.id < _fatherrole.role_identity).Sum(q => q.honor); //升到该身份总功勋值
            sumhonor += _fatherrole.role_honor;
            var newhonor = Math.Floor(Convert.ToDouble(sumhonor / 2));
            SumHonor = newhonor;
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
                else
                    break;
            }
            if (newidentify < _sonrole.role_identity) return _sonrole.role_identity;
            _sonrole.role_identity = newidentify;              //获取新的身份值
            _sonrole.role_honor = Convert.ToInt32(newhonor);
            return newidentify > _sonrole.role_identity ? newidentify : _sonrole.role_identity;
        }

        /// <summary>组装数据 </summary>
        private Dictionary<string, object> BuildData(List<double> data, int result)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"data", data}
            };
            return dic;
        }
    }
}
