using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Common.Randoms
{
    /// <summary>
    /// 按比例数据数加载模型
    /// 使用示例:
    /// var loadedDie = new LoadedDie(new int[] { 5, 10, 20, 15, 15, 5, 10, 5, 10, 5 }); // 列表为每个数的概率
    /// int number = loadedDie.NextValue(); //根据给定的概率从0-N返回一个数字
    /// </summary>
    public class LoadedDie
    {
        //初始化一个模型
        //probs是表示相对于每一个选择的对所有的人的相对概率的数字的阵列
        //例如,如果probs是[3,4,2]，则该机会是3/9，4/9，2/9
        //因为概率加起来是9
        public LoadedDie(int probs)
        {
            this.prob = new List<long>();
            this.alias = new List<int>();
            this.total = 0;
            this.n = probs;
            this.even = true;
        }

        Random random = new Random();
        List<long> prob;
        List<int> alias;
        long total;
        int n;
        bool even;
        public LoadedDie(IEnumerable<int> probs)
        {
            // Raise an error if nil 
            if (probs == null) throw new ArgumentNullException("probs");
            this.prob = new List<long>();
            this.alias = new List<int>();
            this.total = 0;
            this.even = false;
            var small = new List<int>();
            var large = new List<int>();
            var tmpprobs = new List<long>();
            foreach (var p in probs)
            {
                tmpprobs.Add(p);
            }
            this.n = tmpprobs.Count;
            //获取最大和最小的选择和计算总
            long mx = -1, mn = -1;
            foreach (var p in tmpprobs)
            {
                if (p < 0) throw new ArgumentException("probs contains a negative probability.");
                mx = (mx < 0 || p > mx) ? p : mx; mn = (mn < 0 || p < mn) ? p : mn;
                this.total += p;
            }
            // 如果所有的概率是相等
            if (mx == mn) { this.even = true; return; }
            // 克隆概率和概率数量扩展
            for (var i = 0; i < tmpprobs.Count; i++)
            {
                tmpprobs[i] *= this.n;
                this.alias.Add(0);
                this.prob.Add(0);
            }
            //用迈克尔VOSE下一代方法
            //选择初始生命种群
            //循环
            //评价种群中的个体适应度
            //选择产生下一个种群
            //改变该种群（杂交和突变）
            //直到停止循环的条件满足
            for (var i = 0; i < tmpprobs.Count; i++)
            {
                if (tmpprobs[i] < this.total) small.Add(i);// 小于概率总和的数
                else large.Add(i); //大于概率总和的数
            }
            //计算概率和下一代
            while (small.Count > 0 && large.Count > 0)
            {
                var l = small[small.Count - 1]; small.RemoveAt(small.Count - 1);
                var g = large[large.Count - 1]; large.RemoveAt(large.Count - 1);
                this.prob[l] = tmpprobs[l];
                this.alias[l] = g;
                var newprob = (tmpprobs[g] + tmpprobs[l]) - this.total;
                tmpprobs[g] = newprob;
                if (newprob < this.total) small.Add(g);
                else large.Add(g);
            }
            foreach (var g in large)
                this.prob[g] = this.total;
            foreach (var l in small)
                this.prob[l] = this.total;
        }

        // 返回选择的数量
        public int Count { get { return this.n; } }
        //选择一个选择随机，范围从0到这个数减去1
        public int NextValue()
        {
            var i = RNG.Next(this.n - 1);
            var j = RNG.Next((int)this.total);
            return (this.even || j < this.prob[i]) ? i : this.alias[i];
        }
    }
}
