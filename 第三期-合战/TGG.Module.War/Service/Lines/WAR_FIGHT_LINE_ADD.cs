using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Global;
using TGG.Core.Vo.War;
using TGG.SocketServer;

namespace TGG.Module.War.Service
{
    /// <summary>
    /// 合战武将战斗线路添加
    /// </summary>
    public class WAR_FIGHT_LINE_ADD : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_FIGHT_LINE_ADD()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_FIGHT_LINE_ADD _objInstance;

        ///// <summary>WAR_FIGHT_LINE_ADD单体模式</summary>
        //public static WAR_FIGHT_LINE_ADD GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_FIGHT_LINE_ADD());
        //}

        /// <summary> 合战武将战斗线路添加 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("roles") || !data.ContainsKey("line") || !data.ContainsKey("type")) return null;
            var roleid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "roles").Value.ToString());
            var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value.ToString());
            if (!CheckRole(roleid)) return null;
            var line = data.FirstOrDefault(q => q.Key == "line").Value.ToString();
            if (!CheckLine(line, session.Player.User.id, type, roleid)) return null;
            return CommonHelper.SuccessResult();
        }

        /// <summary>
        /// 验证是否有武将
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        private bool CheckRole(Int64 roleid)
        {
            var role = tg_role.GetEntityById(roleid);
            if (role == null) return false;
            return true;
        }

        /// <summary>
        /// 验证线路
        /// </summary>
        /// <param name="line"></param>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        private bool CheckLine(string line, Int64 userid, int type, Int64 rid)
        {
            var oneroleline = new WarRolesLinesVo()
            {
                rid = rid,
                lines = new List<PointVo>(),
            };

            if (!line.Contains("|") || !line.Contains("_")) return false;

            var stringsplit = line.Split(new[] { '|', '_' });

            //if (stringsplit.Count() % 2 != 0) return false;
            for (int index = 0; index < stringsplit.Length; index++)
            {
                if (index % 2 != 0) continue;
                if (index > stringsplit.Count() - 2) continue;
                oneroleline.lines.Add(new PointVo()
                 {
                     x = Convert.ToInt32(stringsplit[index]),
                     y = Convert.ToInt32(stringsplit[index + 1]),
                 });
            }

            Variable.AddLine(userid, type, oneroleline);

            return true;

        }
    }
}
