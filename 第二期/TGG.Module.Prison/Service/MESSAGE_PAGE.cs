using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Prison;
using TGG.SocketServer;

namespace TGG.Module.Prison.Service
{
    /// <summary>
    /// 获取留言
    /// </summary>
    public class MESSAGE_PAGE
    {
        private static MESSAGE_PAGE _objInstance;

        /// <summary>MESSAGE_PAGE 单体模式</summary>
        public static MESSAGE_PAGE GetInstance()
        {
            return _objInstance ?? (_objInstance = new MESSAGE_PAGE());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_GOLD, session, data);
        }

        public ASObject CommandStart(int goodstype, TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}获取留言--{1}", session.Player.User.player_name, "MESSAGE_PAGE");
#endif
            var page = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "page").Value);//从0开始
            if (page < 0 || page > 9)
                return BuildData((int)ResultType.PRISON_PAGE_ERROR, null);
            var msg = tg_prison_messages.GetUserMessages(page, 100);

            return BuildData((int)ResultType.SUCCESS, msg);
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        private ASObject BuildData(int result, List<tg_prison_messages> messages)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"list", messages==null?null:ConvertListASObject(messages)}
            };
            return new ASObject(dic);
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        public List<ASObject> ConvertListASObject(IEnumerable<tg_prison_messages> list)
        {
            var list_aso = new List<ASObject>();
            foreach (var item in list)
            {
                var model = ConvertPrisonMessageVo(item);
                list_aso.Add(Core.AMF.AMFConvert.ToASObject(model));
            }
            return list_aso;
        }

        private PrisonMessageVo ConvertPrisonMessageVo(tg_prison_messages message)
        {
            return new PrisonMessageVo()
            {
                name = message.play_name,
                content = message.message,
                date = new DateTime(message.writetime).ToString("yyyy-MM-dd HH:mm:ss"), 
            };
        }
    }
}
