using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using NewLife.Reflection;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Building.Service
{
    /// <summary>
    /// 进入活动
    /// 开发者：李德雁
    /// </summary>
    public class ENTER
    {
        private static ENTER _objInstance;

        /// <summary>ENTER 单体模式</summary>
        public static ENTER GetInstance()
        {
            return _objInstance ?? (_objInstance = new ENTER());
        }


        /// <summary> 一夜墨俣--进入活动</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var userid = session.Player.User.id;
            var camp = session.Player.User.player_camp;
            var scene = session.Player.Scene;
            if (!CheckTime()) //活动时间验证
                return BuildError((int)ResultType.BUILDING_TIME_ERROR);
            if ( Variable.Activity.BuildActivity.isover) BuildError((int)ResultType.BUILDING_OVER);
            var ac = AddUserGoods(userid, camp);

            CheckScene(userid, scene, camp); //场景推送
            var otherplayers = Core.Common.Scene.ActivityOtherPlayers(userid, (int)ModuleNumber.BUILDING);
            var listscene = otherplayers.Select(item => Variable.Activity.ScenePlayer[item]).ToList();

            return BuildData((int)ResultType.SUCCESS, listscene, ac);
        }

        /// <summary> 场景处理</summary>
        private void CheckScene(Int64 userid, view_scene_user scene, int camp)
        {
            if (scene == null) return;
            scene.model_number = (int)ModuleNumber.BUILDING;
            var oldkey = string.Format("{0}_{1}", (int)ModuleNumber.BUILDING, scene.user_id);
            if (Variable.SCENCE.ContainsKey(oldkey))
                Variable.SCENCE[oldkey].model_number = (int)ModuleNumber.BUILDING;
            Common.GetInstance().PushMyLeaveScene(userid, scene); //活动外场景推送离开
            var activityscene = scene.CloneEntity();
            activityscene.model_number = (int)ModuleNumber.BUILDING;
            Common.GetInstance().GetActivityPoint(activityscene, camp); //初始坐标
            var key = string.Format("{0}_{1}", (int)ModuleNumber.BUILDING, userid);
            Variable.Activity.ScenePlayer.AddOrUpdate(key, activityscene, (m, n) => n); //加入到内存中
            Common.GetInstance().PushEnterActivity(userid, activityscene); //向活动内玩家推送进入
        }

        /// <summary> 组装返回数据 </summary>
        private ASObject BuildData(int result, IEnumerable<view_scene_user> otherplayers, BuildActivity.UserGoods goods)
        {
            var dic1 = new Dictionary<string, object>()
            {
                {"cityHp", Variable.Activity.BuildActivity.EastCityBlood},
                {"npcHp", Variable.Activity.BuildActivity.EastBoosBlood},
                //{"cityHp", Variable.Activity.BuildActivity.EastCityBlood}, //测试数据
                //{"npcHp",0},//测试数据
            };
            var dic2 = new Dictionary<string, object>()
            {
                {"cityHp", Variable.Activity.BuildActivity.WestCityBlood},
                {"npcHp", Variable.Activity.BuildActivity.WestBoosBlood},
               // {"npcHp",0}, //测试数据
            };
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"playerList", otherplayers.Any() ? Common.GetInstance().ConvertListASObject(otherplayers) : null},
                {"dataA", new ASObject(dic1)},
                {"dataB", new ASObject(dic2)},
                {"wood", goods.wood},
                {"torch", goods.torch},
                {"baseBuild", goods.basebuild},
                {"fame", goods.fame},
            };
            return new ASObject(dic);

        }

        /// <summary> 组装错误数据</summary>
        private ASObject BuildError(int result)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"playerList", null},
                {"dataA",null},
                {"dataB", null},
                {"wood", null},
                {"torch",null},
                {"baseBuild", null},
                {"fame", null},
            };
            return new ASObject(dic);
        }

        /// <summary>/验证玩家进入的时间</summary>
        private Boolean CheckTime()
        {
            var baseinfo = Variable.BASE_RULE.FirstOrDefault(q => q.id == "27007");
            if (baseinfo == null) return false;
            var starttime = DateTime.Parse(DateTime.Now.ToString("yyyy MM dd") + " " + baseinfo.value);
            var endtime = starttime.AddMinutes(Convert.ToDouble(Variable.Activity.BuildActivity.PlayTime));
            //验证活动开启时间
            if (DateTime.Now < starttime) return false;
            return DateTime.Now <= endtime;
        }

        /// <summary>
        /// 将用户数据添加到全局
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="camp"></param>
        /// <returns></returns>
        private BuildActivity.UserGoods AddUserGoods(Int64 userid, Int32 camp)
        {
            if (Variable.Activity.BuildActivity.userGoods.ContainsKey(userid))
                return Variable.Activity.BuildActivity.userGoods[userid];
            var ac = new BuildActivity.UserGoods
              {
                  user_id = userid,
                  camp = camp
              };
            Variable.Activity.BuildActivity.userGoods.TryAdd(userid, ac);
            return ac;
        }

        ///// <summary> 玩家身份验证 </summary>
        //private Boolean CheckIdentify(TGGSession session)
        //{
        //    //验证玩家身份
        //    var useridentify = session.Player.Role.Kind.role_identity;
        //    var needidentify = Variable.BASE_IDENTITY.Where(q => q.vocation == session.Player.User.player_vocation).Skip(2).FirstOrDefault();
        //    if (needidentify == null) return false;
        //    if (useridentify < needidentify.id) return false;
        //    return true;
        //}
    }
}
