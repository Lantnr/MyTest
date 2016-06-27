using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Global;
using TGG.Core.Vo.Activity;
using TGG.SocketServer;

namespace TGG.Module.User.Service
{
    /// <summary>
    /// 活动开启
    /// </summary>
    public class ACTIVITY_OPEN : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~ACTIVITY_OPEN()
        {
            Dispose();
        }

        #endregion
        //private static ACTIVITY_OPEN ObjInstance;
        ///// <summary>ACTIVITY_OPEN单例模式</summary>
        //public static ACTIVITY_OPEN GetInstance()
        //{
        //    return ObjInstance ?? (ObjInstance = new ACTIVITY_OPEN());
        //}


        /// <summary>活动开启</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            //筑城活动是否开启
            var buildingisopen = Variable.Activity.BuildActivity.isover ? 0 : 1;
            var siegeisopen = Variable.Activity.Siege.IsOpen ? 1 : 0;
            var list = tg_activity.FindAll();

            var listvo = new List<ActivityOpenVo>();

            #region MyRegion

            if (buildingisopen != 0)
            {
                listvo.Add(new ActivityOpenVo
                {
                    openId = 15,
                    state = buildingisopen
                });
            }
            if (siegeisopen != 0)
            {
                listvo.Add(new ActivityOpenVo
                {
                    openId = 14,
                    state = siegeisopen
                });
            };

            #endregion
            #region 开服活动
            foreach (var item in list)
            {
                if (item.baseid == 33)
                {
                    if (item.isfinish == 0)
                    {
                        listvo.Add(new ActivityOpenVo
                        {
                            openId = 33,
                            state = 1
                        });
                    }
                }
                if (item.baseid == 34)
                {
                    if (session.Player.UserExtend.level_packet_state == 0)
                    {
                        listvo.Add(new ActivityOpenVo
                        {
                            openId = 34,
                            state = 1
                        });
                    }
                }
                if (item.baseid == 35)
                {
                    if (item.isfinish == 0)
                    {
                        listvo.Add(new ActivityOpenVo
                        {
                            openId = 35,
                            state = 1
                        });
                    }
                }
            }
            #endregion


            if (session.Player.UserExtend.game_update == 1)
            {
                listvo.Add(new ActivityOpenVo
                {
                    openId = 40,
                    state = 1
                });
            }
            var list_aso = listvo.Select(AMFConvert.ToASObject).ToList();
            return BuildData(list_aso, session.Player.Vip.shake_count);
        }

        /// <summary>
        /// 组装返回数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private ASObject BuildData(List<ASObject> list, int count)
        {
            var dic = new Dictionary<string, object>()
            {
                {"listVo", list},
                  {"count", count},
            };
            return new ASObject(dic);

        }
    }
}
