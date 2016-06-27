using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    public class Record
    {
        #region 属性
        /// <summary></summary>
        public Int64 id { get; set; }

        /// <summary></summary>
        public Int32 pid { get; set; }

        /// <summary>游戏服务器编号</summary>
        public Int32 sid { get; set; }

        /// <summary></summary>
        public String server_name { get; set; }

        /// <summary>在线人数</summary>
        public Int32 online { get; set; }

        /// <summary>离线人数</summary>
        public Int32 offline { get; set; }

        /// <summary>今日注册人数</summary>
        public Int32 register { get; set; }

        /// <summary>注册总人数</summary>
        public Int32 register_total { get; set; }

        /// <summary>今日最高在线人数</summary>
        public Int32 taday_online { get; set; }

        /// <summary>历史最高在线人数</summary>
        public Int32 history_online { get; set; }

        /// <summary>今日登陆人数</summary>
        public Int32 taday_login { get; set; }

        /// <summary>今日充值人数</summary>
        public Int32 pay_number { get; set; }

        /// <summary>今日充值次数</summary>
        public Int32 pay_count { get; set; }

        /// <summary>今日充值</summary>
        public Int32 pay_taday { get; set; }

        /// <summary>总充值</summary>
        public Int32 pay_total { get; set; }

        /// <summary>月充值</summary>
        public Int32 pay_month { get; set; }

        /// <summary>创建时间</summary>
        public String createtime { get; set; }

        /// <summary>开服天数</summary>
        public Int64 total_days { get; set; }

        /// <summary>APRU值 当日总收入/付费人数</summary>
        public Double apru { get; set; }

        /// <summary>今日元宝消耗</summary>
        public Int64 taday_cost { get; set; }

        /// <summary>消费充值比率</summary>
        public Double cost_rate { get; set; }

        #endregion
    }
}
