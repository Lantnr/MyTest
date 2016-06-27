using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;
using TGG.Share;
using TGG.Share.Event;

namespace TGG
{
    public partial class FrmTools : Form
    {
        public FrmTools()
        {
            InitializeComponent();
        }

        private void btn_ToTime_Click(object sender, EventArgs e)
        {
            ToTime();
        }

        private void ToTime()
        {
            try
            {
                var w = this.txt_write.Text;
                var temp = Convert.ToInt64(w);
                var dt = new DateTime(temp);
                this.txt_result.Text = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_60_Click(object sender, EventArgs e)
        {
            try
            {
                var w = this.txt_write.Text;
                var temp = Convert.ToInt64(w);
                temp = temp * 10000 + 621355968000000000;
                var dt = new DateTime(temp);
                this.txt_60.Text = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Test_Task_Factory();
            //test2();
            //
            //SetTest();
            //GetTest();


            //TestBTask();
            //TestRaise();

            Test_Task_1();

        }

        private void TestRaise()
        {

            GetRaiseCoin(4, 5);
        }

        public string GetRaiseCoin(int voc, int ide)
        {
            //类型_资金总数量_当前资金数量

            //获取当前身份筹措金钱区间
            var r = Variable.BASE_IDENTITY.FirstOrDefault(m => m.vocation == voc && m.value == ide);
            if (r == null || string.IsNullOrEmpty(r.raiseCoin)) return string.Empty;

            var temp = r.raiseCoin;

            var s = temp.Split('-').Select(m => Convert.ToInt32(m)).ToList();
            var min = s[0];
            var max = s[1];
            //随机生成筹措资金
            var num = RNG.Next(min, max) * 1000;

            var step_data = string.Format("{0}_{1}_{2}", (int)TaskStepType.RAISE_COIN, num, 0);

            return step_data;
        }

        private void TestBTask()
        {
            var list = new List<tg_user_area>
            {
                new tg_user_area() {id = 1, area_id = 1, user_id = 13,},
                new tg_user_area() {id = 2, area_id = 2, user_id = 13,},
            };
            GetBusinessStepTask(list);
        }

        public string GetBusinessStepTask(List<tg_user_area> areas)
        {
            //根据商圈获取一个任务目的地町
            var ba = areas.Select(m => m.area_id);

            var tings = Variable.BASE_TING.Where(m => ba.Contains(m.areaId)).ToList();

            //随机一个自己商圈的町
            var index = RNG.Next(tings.Count());
            var ting = tings[index];

            var list_all = new List<int>();

            foreach (var item in tings)
            {
                var l = SplitGoods(item.goods).Where(m => !list_all.Contains(m)).ToList();
                list_all.AddRange(l);
            }

            var target = SplitGoods(ting.goods);

            var list_random = list_all.Except(target).ToList();

            //随机一个自己商圈的非目的地町的货物
            index = RNG.Next(list_random.Count());
            var goods = list_random[index];

            //随机货物数量
            var r_n = Variable.BASE_RULE.FirstOrDefault(m => m.id == "2001");
            if (r_n == null) return null;
            var t = r_n.value.Split('-').Select(m => Convert.ToInt32(m)).ToList();
            var min = t[0];
            var max = t[1];
            var num = RNG.Next(min, max);

            //任务类型_货物出售城市_货物编号_完成数量_当前数量
            var step_data = string.Format("{0}_{1}_{2}_{3}_{4}", (int)TaskStepType.BUILD, ting.id, goods, num, 0);

            return step_data;
        }

        private List<int> SplitGoods(string str)
        {
            return str.Split(',').Select(m => Convert.ToInt32(m)).ToList();
        }


        #region 测试

        private void GetTest()
        {
            var entity = tg_arena_reports.FindByid(53);
            var fight = TGG.Core.Common.Util.CommonHelper.BToO(entity.history);
            var fightvo = fight as FightVo;
            //Expressions.GetPropertyName<tg_fight_history>(p => p.history);//获取实体属性名称

            //var t = BToO(entity.history);
            //var b = t as Player;
            //var a = b.User;

        }

        private void SetTest()
        {
            //var model = new Player
            //{
            //    User = new tg_user() { player_name = Guid.NewGuid().ToString() },
            //    Role = new RoleItem()
            //    {
            //        Kind = new tg_role() { role_id = 1, power = 100 }
            //    },
            //    Bag = new BagItem(),
            //    Order = new BusinessOrder(),
            //    Scene = new view_scene_user(),
            //};
            //var entity = new tg_fight_history { user_id = 100001, time = DateTime.Now, history = OToB(model) };
            //entity.Save();
        }

        /// <summary>将对象转换为字节数组</summary>

        public byte[] OToB(object o)
        {
            var formatter = new BinaryFormatter();
            var rems = new MemoryStream();
            formatter.Serialize(rems, o);
            return rems.GetBuffer();
        }

        /// <summary>将字节数组转换为对象</summary>

        public object BToO(byte[] b)
        {
            var formatter = new BinaryFormatter();
            var rems = new MemoryStream(b);
            return formatter.Deserialize(rems);
        }


        #endregion

        private void test2()
        {
            var entity = new tg_role();
            var role = tg_role.FindByid(1);
            entity = role;
            entity.power = 95;
            //entity.Update();
            tg_role.GetRoleUpdate(entity);
            DisplayGlobal.log.Write("执行结束...");
        }

        private bool IsBreak = false;

        private void Test_Task_Factory()
        {
            IsBreak = false;
            var token = new CancellationTokenSource();
            int time = 10000;
            Object obj = "test";
            Task.Factory.StartNew(m =>
            {
                DisplayGlobal.log.Write("运行中...");
                SpinWait.SpinUntil(() =>
                {
                    return IsBreak;
                }, Convert.ToInt32(m));
            }, time, token.Token)
                .ContinueWith((m, n) =>
                {
                    var t = n.ToString();
                    DisplayGlobal.log.Write("运行结束...");
                    DisplayGlobal.log.Write(t);
                    token.Cancel();
                }, obj, token.Token);
        }

        private void btn_task_Click(object sender, EventArgs e)
        {
            // Random rd = new Random();
            //for (int i = 0; i < 500; i++)
            //{
            //    var taskid = rd.Next(3020116, 3020198);
            //    var steptype = 0;
            //    Thread thread = new Thread(WorkTasksInit);
            //    if (taskid == 3020135)
            //        steptype = 403;
            //    if (taskid == 3020136)
            //        steptype = 404;
            //    thread.Start(new tg_task() { task_id = taskid, user_id = 1,task_step_type = steptype});
            //}
            //for (int i = 0; i < 100; i++)
            //{

            //    Thread thread = new Thread(GetWorkInfo);
            //    thread.Start(rd.Next(1, 100));

            //}

            //for (int i = 0; i < 100; i++)
            //{
            //    Thread thread = new Thread(GetWorkUpdate);
            //    thread.Start(rd.Next(1, 100));

            //}


            //for (int i = 0; i < 100; i++)
            //{
            //    Thread thread = new Thread(GetWorkDelete);
            //    thread.Start(rd.Next(1, 100));

            //}
            //var count = Variable.WorkInfo.GroupBy(x => x).Where(group => group.Count() > 1).ToList();
            //var a = count.Count;
            //foreach (var taskInfo in Variable.WorkInfo)
            //{
            //    XTrace.WriteLine("id:{0},GuardCamp:{1}", taskInfo.userid, taskInfo.GuardCamp);
            //}
            var list = new List<string> { "power", "rolepower" };
            var list1 = new List<RoleAttUpdate.RoleName>
            {
                RoleAttUpdate.RoleName.power,
                RoleAttUpdate.RoleName.rolePower
            };
            var role = tg_role.GetEntityById(1);

            dynamic obje = CommonHelper.ReflectionMethods("TGG.Share", "RoleAttUpdate");
            obje.BuildUpdateRoleData(role, list);
            dynamic obje1 = CommonHelper.ReflectionMethods("TGG.Share", "RoleAttUpdate");
            obje1.BuildUpdateRoleData(role, list1);
        }

        #region 测试方法

        /// <summary>
        /// 获取玩家工作信息，如果没有该玩家数据，默认插入一条数据
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public void GetWorkInfo(object a)
        {
            var userid = Convert.ToInt64(a);
            var info = Variable.WorkInfo.FirstOrDefault(q => q.userid == userid);
            if (info != null) return;
            var newone = new Variable.UserTaskInfo(userid);
            Variable.WorkInfo.Add(newone);

        }

        public void GetWorkUpdate(object a)
        {
            var userid = Convert.ToInt64(a);
            var info = Variable.WorkInfo.FirstOrDefault(q => q.userid == userid);
            if (info != null) info.GuardCamp = 1;
        }

        public void GetWorkDelete(object a)
        {
            lock (Variable.WorkInfo)
            {
                var userid = Convert.ToInt64(a);
                var info = Variable.WorkInfo.FirstOrDefault(q => q.userid == userid);
                if (info != null) Variable.WorkInfo.Remove(info);
            }

        }

        class TestTask
        {
            public Int64 user_id { get; set; }
            public Int32 identify { get; set; }
        }

        /// <summary>
        /// 初始新身份得到的工作任务
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="identify">身份值</param>
        /// <returns></returns>
        private static void SpecialTasksInit(object a)
        {
            var b = a as TestTask;
            var userid = b.user_id;
            var identify = b.identify;
            var entitytasks = new List<tg_task>();

            var baseidentify = Variable.BASE_IDENTITY.FirstOrDefault(q => q.id == b.identify);
            if (baseidentify == null) return;
            var mytasks = Variable.BASE_TASKVOCATION.Where(q => q.type == 2 && q.identifyValue <= baseidentify.value).ToList();
            if (!mytasks.Any()) return;
            var taskextend = Variable.WorkInfo.FirstOrDefault(q => q.userid == b.user_id);
            if (taskextend == null)
            {
                Variable.WorkInfo.Add(new Variable.UserTaskInfo() { userid = b.user_id });
            }

            foreach (var item in mytasks)
            {
                var step = item.stepInit;
                var steptype = item.stepType;
                var taskid = item.id;
                if (item.stepType == (int)TaskStepType.FIGHTING_CONTINUOUS || item.stepType == (int)TaskStepType.SEARCH_GOODS
                    || item.stepType == (int)TaskStepType.ESCORT || item.stepType == (int)TaskStepType.RUMOR
                    || item.stepType == (int)TaskStepType.FIRE || item.stepType == (int)TaskStepType.BREAK
                     || item.stepType == (int)TaskStepType.SEll_WINE || item.stepType == (int)TaskStepType.ASSASSINATION
                     || item.stepType == (int)TaskStepType.GUARD || item.stepType == (int)TaskStepType.ARREST_RUMOR
                     || item.stepType == (int)TaskStepType.ARREST_FIRE || item.stepType == (int)TaskStepType.ARREST_BREAK
                     || item.stepType == (int)TaskStepType.ARREST_SEll_WINE || item.stepType == (int)TaskStepType.STAND_GUARD)
                {
                    var steptypelist = entitytasks.Select(q => q.task_step_type).ToList();
                    var basetask = RandomTask(steptypelist, mytasks, steptype);
                    if (basetask == null) continue;
                    step = basetask.stepInit;
                    steptype = basetask.stepType;
                    taskid = basetask.id;
                    var newone = new TGTask().BuildSpecialTask(step, steptype, taskid, userid, identify,
                        (int)TaskType.WORK_TASK);
                    entitytasks.Add(newone);
#if DEBUG
                    XTrace.WriteLine("id {0},step:{1}", newone.task_id, newone.task_step_data);
#endif
                    continue;
                }
                var newone1 = new TGTask().BuildSpecialTask(step, steptype, taskid, userid, identify,
                    (int)TaskType.WORK_TASK);
#if DEBUG
                XTrace.WriteLine("id {0},step:{1}", newone1.task_id, newone1.task_step_data);
#endif
                entitytasks.Add(newone1);


            }
        }

        /// <summary>
        ///基表 随机一个生成同类型任务（玩家已有类型则跳过）
        /// </summary>
        /// <param name="entitytasks">已经拥有的任务</param>
        /// <param name="basetasks">基表筛选出来的任务数据</param>
        /// <param name="steptype">任务步骤类型</param>
        public static BaseTaskVocation RandomTask(IEnumerable<int> entitytasks, IEnumerable<BaseTaskVocation> basetasks, int steptype)
        {
            if (entitytasks.Any(q => q == steptype)) return null;//已经有该类型任务
            var s_task = basetasks.Where(q => q.stepType == steptype).ToList();
            if (!s_task.Any()) return null;
            var indextask = RNG.Next(0, s_task.Count - 1);
            return s_task[indextask];

        }

        /// <summary>
        /// 初始化同类型工作任务
        /// </summary>
        /// <param name="task"></param>
        public static void WorkTasksInit(object b)
        {
            var task = b as tg_task;
            var basetask = Variable.BASE_TASKVOCATION.FirstOrDefault(q => q.id == task.task_id);
            if (basetask == null) return;
            var task_id = basetask.id;
            var step = basetask.stepInit;
            var step_type = basetask.stepType;
            var taskextend = Variable.WorkInfo.FirstOrDefault(q => q.userid == task.user_id);
            if (taskextend == null)
            {
                Variable.WorkInfo.Add(new Variable.UserTaskInfo() { userid = task.user_id });
            }
            if (basetask.stepType == (int)TaskStepType.FIGHTING_CONTINUOUS ||
                basetask.stepType == (int)TaskStepType.SEARCH_GOODS
                || basetask.stepType == (int)TaskStepType.ESCORT || basetask.stepType == (int)TaskStepType.RUMOR
                || basetask.stepType == (int)TaskStepType.FIRE || basetask.stepType == (int)TaskStepType.BREAK
                || basetask.stepType == (int)TaskStepType.SEll_WINE ||
                basetask.stepType == (int)TaskStepType.ASSASSINATION
                || basetask.stepType == (int)TaskStepType.GUARD || basetask.stepType == (int)TaskStepType.ARREST_RUMOR
                || basetask.stepType == (int)TaskStepType.ARREST_FIRE ||
                basetask.stepType == (int)TaskStepType.ARREST_BREAK
                || basetask.stepType == (int)TaskStepType.ARREST_SEll_WINE ||
                basetask.stepType == (int)TaskStepType.STAND_GUARD)
            {
                var basetasks = Variable.BASE_TASKVOCATION.Where(q => q.stepType == basetask.stepType).ToList();

                var newtask = RandomTask(new List<int> { }, basetasks, basetask.stepType);
                if (newtask == null) return;
                task_id = newtask.id;
                step = newtask.stepInit;
                step_type = newtask.stepType;
            }
            task = new TGTask().BuildSpecialTask(step, step_type, task_id, task.user_id, task.task_base_identify, (int)TaskType.WORK_TASK);
#if DEBUG
            XTrace.WriteLine("id {0},step:{1}", task.task_id, task.task_step_data);
#endif
            var a = true;
            Variable.CD.TryRemove(string.Format("{0}_{1}_{2}", (int)CDType.WorkTask, task.user_id, task.id), out a);
            return;
        }
        #endregion

        private void btn_update_Click(object sender, EventArgs e)
        {
            IsBreak = true;
        }


        private void Test_Task_1()
        {
            DisplayGlobal.log.Write("线程开始...");

            var token = new CancellationTokenSource();
            var task = Task.Factory.StartNew(m =>
            {
                DisplayGlobal.log.Write("运行中...");
                SpinWait.SpinUntil(() => false, Convert.ToInt32(m));
            }, 10000, token.Token);

            DisplayGlobal.log.Write("线程等待...");

            task.Wait();

            DisplayGlobal.log.Write("线程结束...");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var yyyy = Convert.ToInt32(txt_time_yyyy.Text);
                var MM = Convert.ToInt32(txt_time_MM.Text);
                var dd = Convert.ToInt32(txt_time_dd.Text);
                var HH = Convert.ToInt32(txt_time_HH.Text);
                var mm = Convert.ToInt32(txt_time_hhmm.Text);
                var ss = Convert.ToInt32(txt_time_ss.Text);

                var date = new DateTime(yyyy, MM, dd, HH, mm, ss);
                this.txt_Ticks.Text = date.Ticks.ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void FrmTools_Load(object sender, EventArgs e)
        {

        }

        private void btn_Math_Click(object sender, EventArgs e)
        {
            //Regex re = new Regex(@"(?<!Math)\.[A-Z]+[\w]*");
            //string input = "Math.floor(donate/100)*5";
            //Match match = re.Match(input);
            //string pages = match.Groups[1].Value;


        }

    }
}
