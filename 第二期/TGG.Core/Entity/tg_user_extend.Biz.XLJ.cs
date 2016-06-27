using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 玩家扩展类
    /// </summary>
    public partial class tg_user_extend
    {
        /// <summary>根据用户Id获取实体</summary>
        public static tg_user_extend GetEntityByUserId(Int64 user_id = 0)
        {
            var entity = Find(_.user_id, user_id);
            return entity;
        }

        /// <summary>更新全局玩家扩展数据</summary>
        public static int GetEntityUpdate(tg_user_extend model)
        {
            var exp = new ConcatExpression();
            exp &= _.daySalary == model.daySalary;
            exp &= _.donate == model.donate;
            exp &= _.shot_count == model.shot_count;
            exp &= _.task_role_refresh == model.task_role_refresh;
            exp &= _.task_vocation_refresh == model.task_vocation_refresh;
            exp &= _.bargain_count == model.bargain_count;
            exp &= _.npc_refresh_count == model.npc_refresh_count;
            exp &= _.challenge_count == model.challenge_count;
            exp &= _.power_buy_count == model.power_buy_count;
            exp &= _.salary_state == model.salary_state;
            exp &= _.task_vocation_isgo == model.task_vocation_isgo;
            exp &= _.eloquence_count == model.eloquence_count;
            exp &= _.tea_count == model.tea_count;
            exp &= _.calculate_count == model.calculate_count;
            exp &= _.ninjutsu_count == model.ninjutsu_count;
            exp &= _.ball_count == model.ball_count;
            exp &= _.game_finish_count == model.game_finish_count;
            exp &= _.game_receive == model.game_receive;
            exp &= _.refresh_count == model.refresh_count;
            exp &= _.steal_fail_count == model.steal_fail_count;
            exp &= _.fight_count == model.fight_count;
            exp &= _.fight_buy == model.fight_buy;
            return Update(exp, null);
        }
        /// <summary>酒馆时间更新</summary>
        public static int UpdateRecruit(Int64 id, Int64 time)
        {
            var _set = string.Format("recruit_time={0}", time);
            var _where = string.Format("id={0}", id);

            return Update(_set, _where);
        }
    }
}
