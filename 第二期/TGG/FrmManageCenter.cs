using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NewLife.Log;
using NewLife.Reflection;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Global;
using TGG.Core.XML;
using TGG.Share.Event;
using XCode.DataAccessLayer;
using Task = System.Threading.Tasks.Task;
using tg_user_login_log = TGG.Core.Entity.tg_user_login_log;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq.Expressions;

namespace TGG
{
    public partial class FrmManageCenter : Form
    {

        /// <summary>当前路径 </summary>
        string PATH = Application.StartupPath;

        public FrmManageCenter()
        {
            InitializeComponent();
            InitStateBar(); //初始化状态栏信息
        }

        private void FrmManageCenter_Load(object sender, EventArgs e)
        {
            DisplayGlobal.log.LogEvent += log_LogEvent;
            InitData();//初始化数据
            InitModule();//初始化模块
            DBPreheat();//数据库预热 

        }

        void log_LogEvent(object sender, LogEventArgs e)
        {
            rtb_Loginfo.BeginInvoke((MethodInvoker)(() =>
            {
                rtb_Loginfo.AppendText(string.Format("[{0}]: {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), e.Message));
                rtb_Loginfo.ScrollToCaret();
            }));
        }

        #region Init 初始化

        /// <summary>初始化状态栏信息</summary>
        private void InitStateBar()
        {
            this.tss_Msg.Text = string.Format(" {0}", Application.StartupPath);
            this.tssl_Version.Text = string.Format("V:{0}", Application.ProductVersion);
            this.tssl_UpdateTime.Text = string.Format(" T:{0}", System.IO.File.GetLastWriteTime(Application.ExecutablePath));
            this.tsb_Stop.Enabled = false;
            this.btn_LoadNext.Enabled = false;
            this.btn_UnloadNext.Enabled = false;
            this.chk_SelectAll.Enabled = false;
        }

        /// <summary>初始化数据</summary>
        private void InitData()
        {
            DisplayGlobal.log.Write("初始化数据");
            Variable.LMM = Util.ReadModule();
            Variable.LMMQ = Util.ReadModuleQueue();
            Util.ReadBaseEntity();//加载基础实体
            LoadTime = Variable.LMM.Count();
        }

        /// <summary>数据库预热</summary>
        private void DBPreheat()
        {
            XTrace.WriteLine("数据库预热");
            DisplayGlobal.log.Write("数据库预热");
            tg_user_login_log.GetEntityByUserId(0);
        }

        /// <summary>初始化模块</summary>
        private void InitModule()
        {
            DisplayGlobal.log.Write("初始化模块");
            dgv_tgg.Rows.Clear();
            foreach (var item in Variable.LMM)
            {
                #region 数据绑定
                var dgvr = new DataGridViewRow();

                var chk = new DataGridViewCheckBoxCell();
                chk.Value = false;
                dgvr.Cells.Add(chk);

                var cname = new DataGridViewTextBoxCell();
                cname.Value = item.Name;
                dgvr.Cells.Add(cname);
                var cvalue = new DataGridViewTextBoxCell();
                cvalue.Value = item.Value;
                dgvr.Cells.Add(cvalue);
                var ctype = new DataGridViewTextBoxCell();
                ctype.Value = item.type;
                dgvr.Cells.Add(ctype);
                var corder = new DataGridViewTextBoxCell();
                corder.Value = item.Count;
                dgvr.Cells.Add(corder);
                var cisload = new DataGridViewTextBoxCell();
                cisload.Value = "";
                dgvr.Cells.Add(cisload);
                dgv_tgg.Rows.Add(dgvr);
                #endregion
            }
        }

        #endregion

        #region UI界面 事件 方法

        /// <summary>全选</summary>
        private void chk_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in this.dgv_tgg.Rows)
            {
                if (dr.Cells["Type"].Value.ToString() == "0") continue;
                //if (dr.Cells["chk"].Value.ToString() != chk_SelectAll.Checked.ToString())
                {
                    dr.Cells["chk"].Value = chk_SelectAll.Checked.ToString();
                }
            }
        }

        /// <summary>单元格单击事件</summary>
        private void dgv_tgg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;
            if (e.ColumnIndex != 0) return;
            if (dgv_tgg.Rows[e.RowIndex].Cells[3].Value.ToString() == "0") return;
            var v = dgv_tgg.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (v == null || v.ToString().ToLower() == "false")
            {
                dgv_tgg.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
            }
            else
            {
                dgv_tgg.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
            }
        }


        /// <summary>启动</summary>
        private void tsb_Start_Click(object sender, EventArgs e)
        {
            Count = 0;
            timer_Load.Enabled = true;
            tsb_Initial.Enabled = false;
            DisplayGlobal.log.Write("启动中....");
            ModuleStart();//模块启动
        }

        /// <summary>停止</summary>
        private void tsb_Stop_Click(object sender, EventArgs e)
        {
            DisplayGlobal.log.Write("停止中....");
            ModuleStop();//模块停止
            RemoveAllModule();
            this.tsb_Initial.Enabled = true;
        }

        /// <summary>再次加载一次</summary>
        private void btn_LoadNext_Click(object sender, EventArgs e)
        {
            Count = 0;
            timer_Load.Enabled = true;
            LoadNextModule();
        }

        /// <summary>卸载</summary>
        private void btn_UnloadNext_Click(object sender, EventArgs e)
        {
            UnLoadNextModule();
        }

        #endregion

        #region 事件方法


        /// <summary>模块启动</summary>
        private void ModuleStart()
        {
            SocketModuleStart();//Socket启动
            FixedModuleStart();//固定模块启动
            ExtendModuleStart();//扩展模块启动
        }

        /// <summary>模块停止</summary>
        private void ModuleStop()
        {
            ExtendModuleStop();//扩展模块停止
            FixedModuleStop();//固定模块停止
            SocketmoduleStop();//Socket停止
        }


        #region Socket

        CancellationTokenSource token_socket = new CancellationTokenSource();

        /// <summary>Socket启动</summary>
        private void SocketModuleStart()
        {
            DisplayGlobal.log.Write("Socket启动中....");
            new Task(() =>
            {
                dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Socket", "Handle");
                //CommonHelper.ModuleTaskState("TGG.Module.Socket", 0, true, true);
                CommonHelper.GetCDTSTM("TGG.Module.Socket", 0, true);
                UpdateGridView("TGG.Module.Socket", false);
                obje.Start();
            }, token_socket.Token).Start();
        }

        /// <summary>Socket停止</summary>
        private void SocketmoduleStop()
        {
            DisplayGlobal.log.Write("Socket停止中....");
            Task.Factory.StartNew(() =>
            {
                dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Socket", "Handle");
                obje.Stop();
                CommonHelper.GetCDTSTMRemove("TGG.Module.Socket", 0);
                UpdateGridView("TGG.Module.Socket", true);
            }, token_socket.Token);
            //new Task().Start();
        }

        #endregion

        private static readonly object syncRoot = new object();

        /// <summary>固定模块启动</summary>
        private void FixedModuleStart()
        {
            DisplayGlobal.log.Write("固定模块启动中....");
            var fm = Variable.LMM.Where(m => m.type == 0).ToList();

            for (var i = 0; i < fm.Count(); i++)
            {
                var token_fixed = new CancellationTokenSource();
                if (fm[i].Value == "TGG.Module.Socket") continue;
                //var _module = CommonHelper.ModuleTaskState(fm[i].Value, fm[i].Count, false, true);
                var _module = CommonHelper.GetCDTSTM(fm[i].Value, fm[i].Count, false);
                UpdateGridView(_module.Value, false);
                Task.Factory.StartNew(m =>
                {
                    var _m = m as XmlModule;
                    dynamic obje = CommonHelper.ReflectionMethods(_m.Value, "Handle");
                    obje.Start(_m.Count);
                }, _module, token_fixed.Token);
                //new Task().Start();
            }
        }

        /// <summary>固定模块停止</summary>
        private void FixedModuleStop()
        {
            DisplayGlobal.log.Write("固定模块停止中....");
            var fm = Variable.LMM.Where(m => m.type == 0).ToList();

            for (var i = 0; i < fm.Count(); i++)
            {
                var token_fixed = new CancellationTokenSource();
                if (fm[i].Value == "TGG.Module.Socket") continue;
                //var _module = Variable.TGGModuleTaskState.Keys.Where(m => m.Value == fm[i].Value).OrderBy(m => m.Count).LastOrDefault();
                var _module = Variable.CDTSTM.Keys.Where(m => m.Value == fm[i].Value).OrderBy(m => m.Count).LastOrDefault();
                if (_module == null) return;
                Task.Factory.StartNew(m =>
                {
                    var _m = m as XmlModule;
                    dynamic obje = CommonHelper.ReflectionMethods(_m.Value, "Handle");
                    obje.Stop(_m.Count);
                    UpdateGridView(_m.Value, true);
                }, _module, token_fixed.Token);
                //new Task().Start();
            }
        }

        /// <summary>扩展模块启动</summary>
        private void ExtendModuleStart()
        {
            DisplayGlobal.log.Write("扩展模块启动....");
            var fm = Variable.LMM.Where(m => m.type == 1).ToList();

            for (var i = 0; i < fm.Count(); i++)
            {
                var token_extend = new CancellationTokenSource();
                //var _module = CommonHelper.ModuleTaskState(fm[i].Value, fm[i].Count, false, true);
                var _module = CommonHelper.GetCDTSTM(fm[i].Value, fm[i].Count, false);
                UpdateGridView(_module.Value, false);
                Task.Factory.StartNew(m =>
                {
                    var _m = m as XmlModule;
                    dynamic obje = CommonHelper.ReflectionMethods(_m.Value, "Handle");
                    obje.Start(_m.Count);
                }, _module, token_extend.Token);
                //new Task().Start();
            }
        }

        /// <summary>扩展模块停止</summary>
        private void ExtendModuleStop()
        {
            var fm = Variable.LMM.Where(m => m.type == 1).ToList();
            for (var i = 0; i < fm.Count(); i++)
            {
                var token_extend = new CancellationTokenSource();
                //var _module =Variable.TGGModuleTaskState.Keys.Where(m => m.Value == fm[i].Value).OrderBy(m => m.Count).LastOrDefault();
                var _module = Variable.CDTSTM.Keys.Where(m => m.Value == fm[i].Value).OrderBy(m => m.Count).LastOrDefault();
                if (_module == null) return;
                Task.Factory.StartNew(m =>
                {
                    var _m = m as XmlModule;
                    dynamic obje = CommonHelper.ReflectionMethods(_m.Value, "Handle");
                    obje.Stop(_m.Count);
                    UpdateGridView(_m.Value, true);
                }, _module, token_extend.Token);
                //new Task().Start();

            }
        }

        #endregion

        #region

        /// <summary>GridView更新</summary>
        private void UpdateGridView(string moduledll, bool unload)
        {
            lock (syncRoot)
            {
                var count = Variable.CDTSTM.Keys.Count(m => m.Value == moduledll);
                foreach (DataGridViewRow dr in this.dgv_tgg.Rows)
                {
                    if (dr.Cells["ModuleDLL"].Value.ToString() != moduledll) continue;
                    if (unload) count = (count--) > 0 ? count : 0;
                    dr.Cells["ModuleCount"].Value = count.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>加载扩展模块</summary>
        private void LoadNextModule()
        {
            foreach (DataGridViewRow dr in this.dgv_tgg.Rows)
            {
                if (dr.Cells["chk"].Value.ToString().ToLower() != "true") continue;
                var module = dr.Cells["ModuleDLL"].Value.ToString();
                var item = Variable.CDTSTM.Keys.LastOrDefault(m => m.Value == module) ??
                           Variable.LMM.FirstOrDefault(m => m.Value == module);
                DisplayGlobal.log.Write("扩展模块启动....");
                var token_extend = new CancellationTokenSource();
                var _module = CommonHelper.GetCDTSTM(item.Value, item.Count, false);
                UpdateGridView(_module.Value, false);
                Task.Factory.StartNew(() =>
                {
                    dynamic obje = CommonHelper.ReflectionMethods(_module.Value, "Handle");
                    obje.Start(_module.Count);
                }, token_extend.Token);
            }
        }

        /// <summary>卸载扩展模块</summary>
        private void UnLoadNextModule()
        {
            this.btn_LoadNext.Enabled = false;
            this.tsb_Stop.Enabled = false;
            this.btn_UnloadNext.Enabled = false;
            foreach (DataGridViewRow dr in this.dgv_tgg.Rows)
            {
                lock (syncRoot)
                {
                    if (dr.Cells["chk"].Value.ToString().ToLower() != "true") continue;
                    var module = dr.Cells["ModuleDLL"].Value.ToString();
                    var item = Variable.CDTSTM.Keys.LastOrDefault(m => m.Value == module);
                    var _count = Variable.CDTSTM.Keys.Count(m => m.Value == module);
                    if (_count <= 0) continue;
                    if (item == null) continue;
                    DisplayGlobal.log.Write("扩展模块卸载....");
                    var _module =
                        Variable.CDTSTM.Keys.Where(m => m.Value == item.Value)
                            .OrderBy(m => m.Count)
                            .LastOrDefault();
                    if (_module == null) continue;
                    dynamic obje = CommonHelper.ReflectionMethods(_module.Value, "Handle");
                    obje.Stop(_module.Count);
                    UpdateGridView(_module.Value, true);
                }
            }
            Count = 0;
            timer_Unload.Enabled = true;
        }

        private void RemoveAllModule()
        {
            Count = 0;
            timer_remove.Enabled = true;
        }


        private int Count { get; set; }

        private void timer_remove_Tick(object sender, EventArgs e)
        {
            tsb_Stop.Enabled = false;
            if (Count >= LoadTime)
            {
                Variable.CDTSTM.Clear();
                foreach (DataGridViewRow dr in this.dgv_tgg.Rows)
                {
                    dr.Cells["ModuleCount"].Value = 0;
                }
                Count = 0;
                timer_remove.Enabled = false;
                tsb_Start.Enabled = true;
                tsb_Stop.Enabled = false;
            }
            else Count++;
        }

        private void timer_Unload_Tick(object sender, EventArgs e)
        {
            if (Count >= LoadTime)
            {
                var flag = true;
                Variable.CDTSTM.Where(m => m.Value == false).ToList()
                    .ForEach(m => Variable.CDTSTM.TryRemove(m.Key, out flag));
                timer_Unload.Enabled = false;
                this.btn_LoadNext.Enabled = true;
                this.tsb_Stop.Enabled = true;
                this.btn_UnloadNext.Enabled = true;
                this.tsb_Initial.Enabled = true;
            }
            else Count++;
        }

        private int LoadTime = 10;

        private void timer_Load_Tick(object sender, EventArgs e)
        {
            if (Count >= LoadTime)
            {
                timer_Load.Enabled = false;
                this.btn_LoadNext.Enabled = true;
                this.tsb_Stop.Enabled = true;
                this.btn_UnloadNext.Enabled = true;
                this.chk_SelectAll.Enabled = true;
                ThreadRecovery();
            }
            else
            {
                this.btn_LoadNext.Enabled = false;
                this.tsb_Stop.Enabled = false;
                this.btn_UnloadNext.Enabled = false;
                this.tsb_Start.Enabled = false;
                this.chk_SelectAll.Enabled = false;
                Count++;
            }
        }

        #region 线程恢复
        private void ThreadRecovery()
        {
            var token = new CancellationTokenSource();
            Task.Factory.StartNew(m =>
            {
                CarRecovery();
                RaiseTaskRecovery();
                LifeSkillRecovery();
                FightSkillRecovery();
                RoleTrainRecovery();
                LifeSkillStateChange();
                token.Cancel();
            }, 0, token.Token);

        }

        /// <summary>跑商马车重新加入线程</summary>
        private void CarRecovery()
        {
            try
            {
                dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Business", "Common");
                obje.BusinessCarRecovery();
            }
            catch { }

        }

        /// <summary>武将修行重新加入线程</summary>
        private void RaiseTaskRecovery()
        {
            try
            {
                dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Task", "Common");
                obje.RaiseTaskRecovery();
            }
            catch { }
        }

        /// <summary>生活技能学习升级重新加入线程</summary>
        private void LifeSkillRecovery()
        {
            try
            {
                dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Skill", "Common");
                obje.RoleLifeSkillRecovery();
            }
            catch { }
        }


        /// <summary>生活技能学习升级预处理</summary>
        private void LifeSkillStateChange()
        {
            try
            {
                dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Skill", "Common");
                obje.SkillChange();
            }
            catch { }
        }

        /// <summary>武将修行重新加入线程</summary>
        private void RoleTrainRecovery()
        {
            try
            {
                dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.RoleTrain", "Common");
                obje.RoleTrainRecovery();
            }
            catch { }
        }


        /// <summary>战斗技能学习升级重新加入线程</summary>
        private void FightSkillRecovery()
        {
            try
            {
                dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Skill", "Common");
                obje.FightSkillRecovery();
            }
            catch { }
        }



        #endregion

        private void FrmManageCenter_FormClosed(object sender, FormClosedEventArgs e)
        {
            dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Socket", "Handle");
            obje.Stop();
        }




        #endregion


        #region 测试
        /// <summary>初始化</summary>
        private void tsb_Initial_Click(object sender, EventArgs e)
        {
            //InitDBView();
        }

        /// <summary>初始化数据库视图</summary>
        private void InitDBView()
        {
            try
            {
                var connName = "TG";
                var commStr = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
                DAL.AddConnStr(connName, commStr, null, "MSSQL");
                var db = DAL.Create(connName);
                db.Session.Execute(TGG.Resources.Resource.tgg_view);
                DisplayGlobal.log.Write("初始化数据库视图成功");
            }
            catch (Exception)
            {
                DisplayGlobal.log.Write("初始化数据库视图失败");
            }
        }

        private int N = 0;
        private void tsb_Reset_Click(object sender, EventArgs e)
        {
            // testRandomSingle();
            //testRandomSingleIsTrue();

            //for (int i = 0; i < 3; i++)
            //{
            //testTask(N);

            //Thread.Sleep(100);
            //testTask(N);
            //}

            //testSpinWait(N);
            //TestRNG();

            for (int i = 0; i < 50; i++)
            {
                TestRandomEquip();
                N++;
            }

            //TestOpenMoudel();
            //Stopwatch sp = Stopwatch.StartNew();
            //LogWrite("开始任务 ");
            //var t = TestR(0, 100000,20);
            //LogWrite("任务完成 总共耗时: " + sp.ElapsedMilliseconds + " 毫秒");
            //foreach (var item in t)
            //{
            //    LogWrite(item.ToString());
            //}
        }

        void TestOpenMoudel()
        {
            //CommonHelper.GetOpenModule(10, 1001);
        }

        private void TestRandomEquip()
        {
            var temp = CommonHelper.FightRandomEquip(1, new List<int>
                {
                    6010013,6010014,6010015,6010016

                });

            DisplayGlobal.log.Write("测试数据: " + (temp != null ? temp.ToJson() : "没有数据"));
        }

        private void TestRNG()
        {
            for (var i = 0; i < 50; i++)
            {
                var temp = RNG.Next(10);
                DisplayGlobal.log.Write("随机数: " + temp);
            }
        }

        private void testSpinWait(int num)
        {
            Stopwatch sp = Stopwatch.StartNew();
            DisplayGlobal.log.Write("开始任务 " + num);
            SpinWait.SpinUntil(() => false, 2000);
            sp.Stop();
            DisplayGlobal.log.Write("任务完成 " + num + " 总共耗时: " + sp.ElapsedMilliseconds + " 毫秒");
        }

        private void testTask(int num)
        {
            Stopwatch sp = Stopwatch.StartNew();
            DisplayGlobal.log.Write("开始任务 " + num);
            var token_test = new CancellationTokenSource();
            var obj = 2000;
            var t = Task.Factory.StartNew(m =>
              {
                  var temp = (int)m;
                  SpinWait.SpinUntil(() => false, temp);
              }, obj, token_test.Token).ContinueWith((m, n) =>
            {
                var s = n as Stopwatch;
                DisplayGlobal.log.Write("任务中.... ");
                s.Stop();
                DisplayGlobal.log.Write("任务完成 " + num + " 总共耗时: " + s.ElapsedMilliseconds + " 毫秒");
            }, sp, token_test.Token);



        }

        private void testRandomSingleIsTrue()
        {
            Stopwatch sw = Stopwatch.StartNew();
            var test = new RandomSingle();


            for (int i = 1; i < 100; i++)
            {
                var temp = test.IsTrue(5.1);
                DisplayGlobal.log.Write(string.Format("概率结果:{0} ", temp.ToString()));
            }
            sw.Stop();
            DisplayGlobal.log.Write(string.Format("总耗时:{0} 毫秒", sw.ElapsedMilliseconds));

        }


        private void testRandomSingle()
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    var temp = RNG.NextDouble(15,50.00,i).ToString();
            //    LogWrite(temp); 
            //}
            Stopwatch sw = Stopwatch.StartNew();

            var test = new RandomSingle();
            test.Max = 1000;

            int a = 0, b = 0, c = 0, d = 0, f = 0;



            for (int i = 1; i < 100; i++)
            {

                var t = test.RandomFun(10, new[] { 1, 10, 49, 15, 25 });
                foreach (var item in t)
                {
                    switch (item)
                    {
                        case 1: a++; break;
                        case 10: b++; break;
                        case 49: c++; break;
                        case 15: d++; break;
                        case 25: f++; break;
                    }
                }
                //    LogWrite(string.Format("概率:{0} 结果:{1} {2}:{3}:{4}:{5}:{6}", "5, 10, 45, 15, 25", t.ToString(), a, b, c, d, f));



                DisplayGlobal.log.Write(string.Format("概率:{0} 结果:{1} {2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}", "1,10, 49, 15, 25",
                    t.Count.ToString(), t[0], t[1], t[2], t[3], t[4]
                     , t[5], t[6], t[7], t[8], t[9]
                    )
                    );


            }
            sw.Stop();
            DisplayGlobal.log.Write(string.Format("总耗时:{0} 毫秒", sw.ElapsedMilliseconds));
            DisplayGlobal.log.Write(string.Format("统计:{0}:{1}:{2}:{3}:{4}", a, b, c, d, f));
        }

        /// <summary>
        /// 生成一个非重复的随机序列
        /// </summary>
        /// <param name="low">序列最小值</param>
        /// <param name="high">序列最大值</param>
        /// <returns>序列</returns>
        private int[] TestR(int low, int high)
        {
            int x = 0, tmp = 0;
            if (low > high)
            {
                tmp = low;
                low = high;
                high = tmp;
            }
            int[] array = new int[high - low + 1];
            for (int i = low; i <= high; i++)
            {
                array[i - low] = i;
            }
            for (int i = array.Length - 1; i > 0; i--)
            {
                x = RNG.Next(0, i);
                tmp = array[i];
                array[i] = array[x];
                array[x] = tmp;

            }
            return array;
        }

        private int[] TestR(int low, int high, int count)
        {
            int x = 0, tmp = 0;
            if (low > high)
            {
                tmp = low;
                low = high;
                high = tmp;
            }
            int[] array = new int[high - low + 1];
            for (int i = low; i <= high; i++)
            {
                array[i - low] = i;
            }
            var k = count < (high - low) ? count : array.Length - 1;
            for (int i = k; i > 0; i--)
            {
                x = RNG.Next(0, i);
                tmp = array[i];
                array[i] = array[x];
                array[x] = tmp;
            }
            return array;
        }

        #endregion

        private void FrmManageCenter_KeyDown(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode.ToString();
            switch (key)
            {
                case "F1":
                    {
                        var frm = new FrmTools();
                        frm.ShowDialog();
                        break;
                    }
                case "F2":
                    {
                        var frm = new FrmDBTools();
                        frm.ShowDialog();
                        break;
                    }
                case "F10":
                    {
                        var frm = new FrmActivation();
                        frm.ShowDialog();
                        break;
                    }
            }

        }


    }
}
