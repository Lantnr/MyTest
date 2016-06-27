using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Enum.Type;

namespace TGG.Module.Duplicate.Service
{
    /// <summary>
    /// 爬塔小游戏公共方法类
    /// </summary>
    public partial class Common
    {
        /// <summary>忍术游戏数据组装</summary>
        public Dictionary<String, Object> NinjutsuBuildData(int result, int position, string xing)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "position", position }, { "xing", string.IsNullOrEmpty(xing) ? null : xing }, };
            return dic;
        }

        /// <summary>算术游戏数据组装</summary>
        public Dictionary<String, Object> CalculateBuildData(int result, string xing)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "xing", string.IsNullOrEmpty(xing) ? null : xing }, };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuildData(int result)
        {
            var dic = new Dictionary<string, object> { { "result", result } };
            return dic;
        }

        /// <summary>花月茶道进入</summary>
        public Dictionary<String, Object> TeaBuildData(int result, int npcTea, int userTea, List<int> photoPosition)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "npcTea", npcTea }, { "userTea", userTea }, { "photoStateList", photoPosition } };
            return dic;
        }

        /// <summary>花月茶道翻牌</summary>
        public Dictionary<String, Object> TeaFlopData(int result, int npcTea, int userTea, int loc, int uPhoto, int nLoc, int nPhoto)
        {
            var dic = new Dictionary<string, object>
            {
                { "result", result }, { "npcTea", npcTea }, { "userTea", userTea }, { "loc", loc }, { "userPhoto", uPhoto }, { "npcLoc", nLoc }, { "npcPhoto", nPhoto }
            };
            return dic;
        }

        /// <summary>更新星星数量及位置</summary>
        public string StarsUpdate(string xing, int result)
        {
            if (string.IsNullOrEmpty(xing))
            { xing = result == (int)FightResultType.WIN ? "1" : "0"; }
            else
            {
                if (result == (int)FightResultType.WIN)
                { xing = xing + "_1"; }
                else
                { xing = xing + "_0"; }
            }
            return xing;
        }

        /// <summary>
        /// 计算需要的星星数量
        /// </summary>
        /// <param name="xing">记录的星星信息</param>
        /// <param name="result">胜利或失败</param>
        /// <returns></returns>
        public int StarsCount(string xing, int result)
        {
            var number = new List<string>();
            if (string.IsNullOrEmpty(xing)) return number.Count;
            var stars = xing.Split("_").ToList();
            number.AddRange(result == (int)FightResultType.WIN ? stars.Where(item => item == "1") : stars.Where(index => index == "0"));
            return number.Count;
        }
    }
}
