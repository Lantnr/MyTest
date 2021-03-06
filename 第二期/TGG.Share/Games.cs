﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Share
{
    public class Games : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>更新游戏使用次数</summary>
        /// <param name="type">游戏类型</param>
        /// <param name="ex">拓展表实体</param>
        private void GameCountUpdate(int type, tg_user_extend ex)
        {
            switch (type)
            {
                case (int)GameEnterType.辩驳游戏: ex.eloquence_count++; break;
                case (int)GameEnterType.老虎机: ex.calculate_count++; break;
                case (int)GameEnterType.花月茶道: ex.tea_count++; break;
                case (int)GameEnterType.猜宝游戏: ex.ninjutsu_count++; break;
                case (int)GameEnterType.猎魂: ex.ball_count++; break;
            }
            ex.Save();
        }

        /// <summary>更新每日完成次数</summary>
        /// <param name="ex">拓展表实体</param>
        private void CheckDay(tg_user_extend ex)
        {
            var finish = Variable.BASE_RULE.FirstOrDefault(m => m.id == "30004");
            if (finish == null) return;
            var total = Convert.ToInt32(finish.value);

            if (ex.game_finish_count >= total || ex.game_receive != (int)GameRewardType.TYPE_UNREWARD) return;
            ex.game_finish_count++;

            if (ex.game_finish_count < total)
            {
                ex.Update();
                return;
            }
            ex.game_receive = (int)GameRewardType.TYPE_CANREWARD;
            ex.Update();
        }
    }
}
