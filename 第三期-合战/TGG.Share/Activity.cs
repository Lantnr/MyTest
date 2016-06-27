using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.Activity;
using TGG.SocketServer;

namespace TGG.Share
{
    /// <summary>
    /// 活动共享类
    /// </summary>
    public class Activity
    {
        #region 美浓活动

        #region 初始化数据

        /// <summary> 读取固定规则表作为初始数据 </summary>
        public void InitSiegeBase()
        {
            if (!GetBirth()) return;

            var mlt = GetBaseData("26003");
            var gt = GetBaseData("26004");
            var pt = GetBaseData("26008");
            var wf = GetBaseData("26007");
            var gl = GetBaseData("26009");
            var bt = GetBaseData("26010");
            var at = GetBaseData("26011");
            if (mlt == null || gt == null || pt == null || wf == null || gl == null || bt == null || at == null) return;

            Variable.Activity.Siege.BaseData.MakeLadderTime = Convert.ToInt16(mlt) * 1000;
            Variable.Activity.Siege.BaseData.GateTime = Convert.ToInt16(gt) * 1000;
            Variable.Activity.Siege.BaseData.WinFame = Convert.ToInt32(wf);
            Variable.Activity.Siege.BaseData.ProtectTime = Convert.ToInt16(pt) * 1000;
            Variable.Activity.Siege.BaseData.GateLadder = Convert.ToInt32(gl);
            Variable.Activity.Siege.BaseData.BossLadder = Convert.ToInt32(bt);
            Variable.Activity.Siege.BaseData.AShotTime = Convert.ToInt32(Convert.ToDouble(at) * 1000);
            GetBossLife();
            Variable.Activity.Siege.IsOpen = true;
            new Share.Activity().PushActivity(1, 14);
        }

        /// <summary> 获取东西方出生点 </summary>
        /// <returns>获取结果</returns>
        private bool GetBirth()
        {
            var east = GetBaseData("26001");
            var xy = east.Split(',');
            if (xy.Length != 2) return false;
            Variable.Activity.Siege.BaseData.EastBirthX = Convert.ToInt16(xy[0]);
            Variable.Activity.Siege.BaseData.EastBirthY = Convert.ToInt16(xy[1]);

            var west = GetBaseData("26002");
            xy = west.Split(',');
            if (xy.Length != 2) return false;
            Variable.Activity.Siege.BaseData.WestBirthX = Convert.ToInt16(xy[0]);
            Variable.Activity.Siege.BaseData.WestBirthY = Convert.ToInt16(xy[1]);

            return true;
        }

        /// <summary> 获取Boss血量 </summary>
        private void GetBossLife()
        {
            Variable.Activity.Siege.BossCondition.Add(BuildCampCondition((int)CampType.East));
            Variable.Activity.Siege.BossCondition.Add(BuildCampCondition((int)CampType.West));
        }

        /// <summary> 组装全局Boss血量 </summary>
        /// <param name="camp">阵营</param>
        private CampCondition BuildCampCondition(int camp)
        {
            var model = new CampCondition
            {
                player_camp = camp,
                //BaseLife = 10,
                BaseLife = GetBaseNpcSiegeLife((int)SiegeNpcType.BASE, camp),
                //BossLife = 100000,
                BossLife = GetBaseBossLife((int)SiegeNpcType.BOSS, camp),
                // GateLife = 1,
                GateLife = GetBaseNpcSiegeLife((int)SiegeNpcType.GATE, camp),
            };
            return model;
        }

        /// <summary> 获取大将的血量 </summary>
        /// <returns>血量</returns>
        private int GetBaseBossLife(int type, int camp)
        {
            var model = Variable.BASE_NPCSIEGE.FirstOrDefault(m => m.type == type && m.camp == camp);
            if (model == null) return 0;
            var budui = Variable.BASE_NPCARMY.FirstOrDefault(m => m.id == model.armyId);
            if (budui == null) return 0;
            var npcid = budui.matrix.Split(',');
            if (npcid.Length < 1) return 0;
            var npc = Variable.BASE_NPCROLE.FirstOrDefault(m => m.id == Convert.ToInt32(npcid[0]));
            return npc == null ? 0 : npc.life;
        }

        /// <summary> 获取基表Boss总血量 </summary>
        /// <param name="type">Boss类型</param>
        /// <param name="camp">阵营</param>
        private int GetBaseNpcSiegeLife(int type, int camp)
        {
            var model = Variable.BASE_NPCSIEGE.FirstOrDefault(m => m.type == type && m.camp == camp);
            return model == null ? 0 : model.totalHp;
        }

        /// <summary> 获取基表value数据 </summary>
        /// <param name="id">基表Id</param>
        public string GetBaseData(string id)
        {
            var br = Variable.BASE_RULE.FirstOrDefault(m => m.id == id);
            return br == null ? null : br.value;
        }

        #endregion

        #endregion

        #region 活动推送
        /// <summary>
        /// 推送活动开始
        /// </summary>
        /// <param name="state">0：结束 1：开始</param>
        /// <param name="id">功能开放表id</param>
        public void PushActivity(Int32 state, Int32 id)
        {
            foreach (var item in Variable.OnlinePlayer.Keys)
            {
                var user_id = item;
                var myap = new ActivityPush()
                {
                    userid = user_id,
                    id = id,
                    state = state
                };
                var token = new CancellationTokenSource();

                Task.Factory.StartNew(m =>
                {
                    var ap = m as ActivityPush;
                    if (ap == null) return;
                    var aso = BuildData(ap.state, ap.id);

                    SendPv(ap.userid, aso, (int)UserCommand.ACTIVITY_PUSH, (int)ModuleNumber.USER);
                    token.Cancel();
                }, myap, token.Token);

            }

        }

        /// <summary>
        /// 推送个人活动开始
        /// </summary>
        /// <param name="state">0：结束 1：开始</param>
        /// <param name="id">功能开放表id</param>
        public void PushActivityOne(Int32 state, Int32 id, Int64 userid)
        {
            var aso = BuildData(state, id);
            SendPv(userid, aso, (int)UserCommand.ACTIVITY_PUSH, (int)ModuleNumber.USER);
        }

        class ActivityPush
        {
            /// <summary>用户id </summary>
            public Int64 userid { get; set; }

            /// <summary>活动状态0：开始 1：结束 </summary>
            public Int32 state { get; set; }

            /// <summary>功能开放表id </summary>
            public Int32 id { get; set; }
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="state"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private ASObject BuildData(Int32 state, Int32 id)
        {
            var dic = new Dictionary<string, object>()
            {
               {"activityOpenVo",new ActivityOpenVo()
               {
                   state = state,
                   openId =id
               }}
            };
            return new ASObject(dic);
        }
        #endregion

        #region 推送协议
        /// <summary> 向其他人推送协议 </summary>
        //public void SendPv(Int64 userid, ASObject aso, int commandnumber, Int64 otheruserid, int modulenumber)
        //{
        //    if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
        //    var key = string.Format("{0}_{1}_{2}", modulenumber, commandnumber, otheruserid);
        //    var session = Variable.OnlinePlayer[userid] as TGGSession;
        //    if (session == null) return;
        //    session.SPM.AddOrUpdate(key, aso, (m, n) => aso);
        //}

        /// <summary>
        /// 向增加推送协议
        /// </summary>
        public void SendPv(Int64 other_user_id, ASObject aso, int commandnumber, int modulenumber)
        {
            if (!Variable.OnlinePlayer.ContainsKey(other_user_id)) return;
            var session = Variable.OnlinePlayer[other_user_id] as TGGSession;
            if (session == null) return;
            Send(session, aso, commandnumber, modulenumber);
        }
        public void Send(TGGSession session, ASObject data, int commandNumber, int modulenumber)
        {
            var pv = new ProtocolVo
            {
                serialNumber = 1,
                verificationCode = 1,
                moduleNumber = modulenumber,
                commandNumber = commandNumber,
                sendTime = 1000,
                serverTime = (DateTime.Now.Ticks - 621355968000000000) / 10000,
                status = (int)ResponseType.TYPE_SUCCESS,
                data = data,
            };
            session.SendData(pv);
        }
        #endregion
    }
}
