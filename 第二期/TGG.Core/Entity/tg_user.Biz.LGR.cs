﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
﻿using TGG.Core.Global;
﻿using XCode;


namespace TGG.Core.Entity
{
    public partial class tg_user
    {
        /// <summary> 进行数据更新 </summary>
        /// <param name="list"></param>
        public void Update(IEnumerable<tg_user> list)
        {
            var l = new EntityList<tg_user>();
            l.AddRange(list);
            l.Update();
        }

        /// <summary>根据自定条件获取数据</summary>
        public static List<tg_user> GetEntityList(string where)
        {
            return FindAll(where, null, null, 0, 0);
        }

        /// <summary> 验证元宝是否达到上限 并返回计算结果值</summary>
        /// <param name="coincount">当前玩家金钱</param>
        /// <param name="count">要增加的数量</param>
        public static Int32 IsGoldMax(Int32 current, Int32 rise)
        {
            var number = current + rise;
            if (number < 0) return 0;
            return number < Variable.MAX_GOLD ? number : Variable.MAX_GOLD;
        }

        /// <summary> 验证金钱是否达到上限 并返回计算结果值</summary>
        /// <param name="coincount">当前玩家金钱</param>
        /// <param name="count">要增加的数量</param>
        public static Int64 IsCoinMax(Int64 coincount, Int64 count)
        {
            var number = coincount + count;
            if (number < 0) return 0;
            return number < Variable.MAX_COIN ? number : Variable.MAX_COIN;
        }

        /// <summary> 验证声望是否达到上限 并返回计算结果值</summary>
        /// <param name="famecount">当前玩家声望</param>
        /// <param name="count">要增加的数量</param>
        public static int IsFameMax(int famecount, int count)
        {
            var number = famecount + count;
            if (number < 0) return 0;
            return number < Variable.MAX_FAME ? number : Variable.MAX_FAME;
        }

        /// <summary> 验证功勋是否达到上限 并返回计算结果值</summary>
        /// <param name="honorcount">当前玩家功勋</param>
        /// <param name="count">要增加的数量</param>
        public static int IsHonorMax(int honorcount, int count)
        {
            var number = honorcount + count;
            if (number < 0) return 0;
            return number < Variable.MAX_HONOR ? number : Variable.MAX_HONOR;
        }

        /// <summary> 验证魂数是否达到上限 并返回计算结果值</summary>
        /// <param name="spiritcount">当前玩家魂数</param>
        /// <param name="count">要增加的数量</param>
        public static int IsSpiritMax(int spiritcount, int count)
        {
            var number = spiritcount + count;
            if (number < 0) return 0;
            return number < Variable.MAX_SPIRIT ? number : Variable.MAX_SPIRIT;
        }
    }
}
