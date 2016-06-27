using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Common.Util;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Entity;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Chat.Service
{
    /// <summary>
    /// 发送
    /// </summary>
    public class CHATS
    {
        public static CHATS ObjInstance;

        /// <summary>CHATS单体模式</summary>
        public static CHATS GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new CHATS());
        }

        /// <summary>发送</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1} 聊天发送", "CHATS", session.Player.User.player_name);//type:0:全部 CHATS_ALL  1:世界 CHATS_WORLD2:家族 CHATS_FAMILY3:私聊 CHATS_ONE4:系统
#endif
                int type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value);
                string _data = data.FirstOrDefault(q => q.Key == "data").Value.ToString();
                var goods = data.FirstOrDefault(q => q.Key == "goods").Value as object[];

                var player = session.Player;
#if DEBUG
                XTrace.WriteLine("-----------------    {0}   ----------------", _data);
#endif
                //if (CommonHelper.IsGM())
                //{
                if ((new ExpansionCommand()).IsGmContent(_data)) return BuildData((int)ResultType.SUCCESS); //检索内容
                //}

                switch (type)
                {
                    case (int)ChatsType.CHATS_ONE:
                        {
                            var receive = data.FirstOrDefault(q => q.Key == "receive").Value;
                            if (receive == null) return BuildData((int)ResultType.CHAT_NO_EXIST);
                            var result = CHATS_ONE(session, type, _data, receive.ToString(), goods);
                            if (result != ResultType.SUCCESS)
                                return BuildData((int)result);
                            break;
                        }
                    case (int)ChatsType.CHATS_FAMILY:
                        {
                            if (player.Family.fid == 0) return BuildData((int)ResultType.CHAT_NO_FAMILY);
                            CHATS_PARTY(player, type, _data, goods); break;
                        }
                    case (int)ChatsType.CHATS_WORLD:
                        {
                            if (!IsCharSend(session))
                                return BuildData((int)ResultType.CHAT_TIME_ERROR);
                            CHATS_WORLD(player, type, goods, _data); break;
                        }
                    default: { return BuildData((int)ResultType.FRONT_DATA_ERROR); }
                }

                return BuildData((int)ResultType.SUCCESS);
            }
            catch (Exception ex)
            {
                XTrace.WriteLine(string.Format("{0}:{1}", "聊天发送错误", ex));
                return BuildData((int)ResultType.UNKNOW_ERROR);
            }
        }

        /// <summary>世界聊天</summary>
        private void CHATS_WORLD(Player player, int type, object[] goods, string data)
        {
            var _data = goods.Any() ? AnalyticalGoods(goods) : null;
            var list = Variable.OnlinePlayer.Select(m => m.Key).Where(m => !player.BlackList.Contains(m)).ToList(); //查询在线玩家且不是黑名单的玩家集合
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    CHATS_PUSH.GetInstance().CommandStart(Convert.ToInt64(m), type, player.User.id,
                        player.User.player_name, (int)ChatsPushType.NO_PRIVATE, data, _data); //全世界
                }, item, token.Token);
            }
        }

        /// <summary> 验证聊天时间间隔 </summary>
        private bool IsCharSend(TGGSession session)
        {
            var rt = Variable.BASE_RULE.FirstOrDefault(m => m.id == "12002");
            if (rt == null) return false;
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            var t = session.CharTime + (Convert.ToInt32(rt.value) * 1000);
            if (t > time) return false;
            session.CharTime = time;
            return true;
        }

        /// <summary>解析object[] goods</summary>
        private dynamic AnalyticalGoods(object[] goods)
        {
            if (!goods.Any()) return null;
            var aso_list = new List<ASObject>();
            var propid_list = new List<int>();
            foreach (var item in goods)
            {
                string[] b = item.ToString().Split('_');
                if (b.Length != 2) continue;
                int typeid = Convert.ToInt32(b[0]);
                int goodsid = Convert.ToInt32(b[1]);
                switch (typeid)
                {
                    case (int)GoodsType.TYPE_EQUIP:
                    case (int)GoodsType.TYPE_PROP: { propid_list.Add(goodsid); break; }
                }
            }
            if (!propid_list.Any()) return null;
            var list = tg_bag.GetFindByIds(propid_list);
            var pl = list.ToList().Where(m => m.type == (int)GoodsType.TYPE_PROP).ToList();
            var el = list.ToList().Where(m => m.type == (int)GoodsType.TYPE_EQUIP).ToList();
            dynamic obje = Core.Common.Util.CommonHelper.ReflectionMethods("TGG.Module.Props", "Common");
            if (pl.Any()) aso_list.AddRange(obje.ConvertListASObject(pl, "Props"));
            if (el.Any()) aso_list.AddRange(obje.ConvertListASObject(el, "Equip"));
            return aso_list;
        }

        /// <summary>家族聊天</summary>
        private void CHATS_PARTY(Player player, int type, string data, object[] goods)
        {
            var _data = goods.Any() ? AnalyticalGoods(goods) : null;
            var list = Variable.OnlinePlayer.Select(m => m.Value as TGGSession)
               .Where(m => m.Player.Family.fid == player.Family.fid &&
                   !player.BlackList.Contains(m.Player.User.id) &&
                   !m.Player.BlackList.Contains(player.User.id)).Select(m => m.Player.User.id).ToList();//同家族且相互间不是黑名单的用户Id

            foreach (var m in list)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(n =>
                {
                    CHATS_PUSH.GetInstance().CommandStart(Convert.ToInt64(n), type, player.User.id,
                        player.User.player_name, (int)ChatsPushType.NO_PRIVATE, data, _data);
                }, m, token.Token);
            }
        }

        /// <summary>个人聊天</summary>
        private ResultType CHATS_ONE(TGGSession session, int type, string data, string receive, object[] goods)
        {
            var user = session.Player.User;
            string t = receive.Split('_')[0];
            int n = receive.IndexOf('_') + 1;
            string value = receive.Substring(n, receive.Length - n);

            TGGSession rivalsession;
            switch (t)
            {
                case "id":
                    {
                        if (session.Player.User.id == Convert.ToInt64(value)) return ResultType.CHAT_NO_MY;
                        var userid = Convert.ToInt64(value);
                        if (!tg_user.UserIdIsContains(userid)) return ResultType.CHAT_NO_EXIST;
                        if (!Variable.OnlinePlayer.ContainsKey(userid)) return ResultType.CHAT_NO_ONLINE;
                        rivalsession = Variable.OnlinePlayer[userid] as TGGSession;
                        break;
                    }
                case "name":
                    {
                        if (session.Player.User.player_name == value) return ResultType.CHAT_NO_MY;
                        if (!tg_user.PlayNameIsContains(value)) return ResultType.CHAT_NO_EXIST;
                        rivalsession = Variable.OnlinePlayer.Select(m => m.Value as TGGSession).FirstOrDefault(m => m.Player.User.player_name == value);
                        break;
                    }
                default: { return ResultType.FRONT_DATA_ERROR; }
            }
            if (rivalsession == null) return ResultType.CHAT_NO_ONLINE;
            var uid = rivalsession.Player.User.id;  //对方用户Id
            if (session.Player.BlackList.Contains(uid)) return ResultType.CHAT_BLACK;            //验证对方是否在己方黑名单
            if (rivalsession.Player.BlackList.Contains(user.id)) return ResultType.CHAT_RIVAL_BLACK; //验证己方是否在对方黑名单

            var _data = goods.Any() ? AnalyticalGoods(goods) : null;
            CHATS_PUSH.GetInstance().CommandStart(session, type, rivalsession.Player.User.id, rivalsession.Player.User.player_name, (int)ChatsPushType.ME_SAID_PLAYER, data, _data);
            CHATS_PUSH.GetInstance().CommandStart(rivalsession, type, user.id, user.player_name, (int)ChatsPushType.PLAYERME_SAID_ME, data, _data);

            return ResultType.SUCCESS;
        }

        /// <summary>组装数据</summary>
        private ASObject BuildData(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }
    }
}
