using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NewLife.Threading;
using XCode;
using NewLife.Log;
using TGG.Core.Enum.Type;
using TGG.Core.Common.Util;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 生活技能类
    /// </summary>
    public partial class tg_role_life_skill
    {
        /// <summary>获取武将生活技能</summary>
        public static tg_role_life_skill GetFindByRid(Int64 rid)
        {
            var exp = new WhereExpression();
            if (rid > 0) exp &= _.rid == rid;
            return Find(exp);
        }

        /// <summary>删除指定武将生活技能</summary>
        public static bool GetDelete(Int64 rid)
        {
            var exp = new WhereExpression();
            exp &= _.rid == rid;
            return Delete(exp) > 0;
        }

        /// <summary>初始化生活技能</summary>
        /// <param name="rid">武将表id</param>
        public static tg_role_life_skill InitSkill(Int64 rid)
        {
            try
            {
                if (rid <= 0) return null;
                var skill = new tg_role_life_skill
                {
                    rid = rid,
                    sub_archer = CommonHelper.EnumLifeType(LifeSkillType.ARCHER),
                    sub_artillery = CommonHelper.EnumLifeType(LifeSkillType.ARTILLERY),
                    sub_ashigaru = CommonHelper.EnumLifeType(LifeSkillType.ASHIGARU),
                    sub_build = CommonHelper.EnumLifeType(LifeSkillType.BUILD),
                    sub_calculate = CommonHelper.EnumLifeType(LifeSkillType.CALCULATE),
                    sub_craft = CommonHelper.EnumLifeType(LifeSkillType.CRAFT),
                    sub_eloquence = CommonHelper.EnumLifeType(LifeSkillType.ELOQUENCE),
                    sub_equestrian = CommonHelper.EnumLifeType(LifeSkillType.EQUESTRIAN),
                    sub_etiquette = CommonHelper.EnumLifeType(LifeSkillType.ETIQUETTE),
                    sub_martial = CommonHelper.EnumLifeType(LifeSkillType.MARTIAL),
                    sub_medical = CommonHelper.EnumLifeType(LifeSkillType.MEDICAL),
                    sub_mine = CommonHelper.EnumLifeType(LifeSkillType.MINE),
                    sub_ninjitsu = CommonHelper.EnumLifeType(LifeSkillType.NINJITSU),
                    sub_reclaimed = CommonHelper.EnumLifeType(LifeSkillType.RECLAIMED),
                    sub_tactical = CommonHelper.EnumLifeType(LifeSkillType.TACTICAL),
                    sub_tea = CommonHelper.EnumLifeType(LifeSkillType.TEA)
                };
                skill.Save();
                return skill;
            }
            catch (Exception ex)
            {

                XTrace.WriteException(ex);
                return null;
            }
        }


    }
}
