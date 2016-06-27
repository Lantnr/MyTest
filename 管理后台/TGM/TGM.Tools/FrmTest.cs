using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TGM.API.Entity.Helper;

namespace TGM.Tools
{
    public partial class FrmTest : Form
    {
        public FrmTest()
        {
            InitializeComponent();

        }

        private void FrmTest_Load(object sender, EventArgs e)
        {
            DisplayGlobal.log.LogEvent += log_LogEvent;
        }

        private void log_LogEvent(object sender, LogEventArgs e)
        {
            rtb_msg.BeginInvoke((MethodInvoker)(() => rtb_msg.AppendText(String.Format("[{0}]: {1} \r\n", DateTime.Now, e.Message))));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var str = SerialNumber.GenerateString(this.txt_Prefix.Text);
            DisplayGlobal.log.Write(str);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var count = Convert.ToInt32(this.txt_count.Text);
            var sw = Stopwatch.StartNew();
            var list = SerialNumber.GenerateStringList(this.txt_Prefix.Text, count);
            sw.Stop();
            DisplayGlobal.log.Write(string.Format("总共耗时:{0} 毫秒", sw.ElapsedMilliseconds));
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m => DisplayGlobal.log.Write(m.ToString()), item, token.Token);

            }
            
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb_msg.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //当日

            var t0 = DateTime.Now.Date.Ticks;
            var t1 = DateTime.Now.Date.AddDays(1).Ticks;
            DisplayGlobal.log.Write(string.Format("当日0:Ticks={0}",t0));
            DisplayGlobal.log.Write(string.Format("当日24:Ticks={0}", t1));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //当月
            DateTime now = DateTime.Now;
            DateTime m0 = new DateTime(now.Year, now.Month, 1);
            DateTime m1 = m0.AddMonths(1).AddDays(-1);

            DisplayGlobal.log.Write(string.Format("当月第一天:Ticks={0}", m0.Ticks));
            DisplayGlobal.log.Write(string.Format("当月最后一天:Ticks={0}", m1.Ticks));
            DisplayGlobal.log.Write("");
        }
    }
}
