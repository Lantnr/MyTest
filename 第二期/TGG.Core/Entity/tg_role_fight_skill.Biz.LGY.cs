using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_role_fight_skill
    {
        /// <summary>
        /// 插入武将战斗技能数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool GetInsert(tg_role_fight_skill model)
        {
            try
            {
                model.Insert();
                return true;
            }
            catch
            { return false; }
        }
    }
}
