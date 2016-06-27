using System;
using System.Collections.Generic;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Vo.Role;
using TGG.Core.Global;

namespace TGG.Module.Skill.Service
{
    /// <summary>
    /// 战斗技能推送
    /// </summary>
    public class SKILL_FIGHT_PUSH
    {
        private static SKILL_FIGHT_PUSH _objInstance;

        /// <summary>SKILL_FIGHT_PUSH单体模式</summary>
        public static SKILL_FIGHT_PUSH GetInstance()
        {
            return _objInstance ?? (_objInstance = new SKILL_FIGHT_PUSH());
        }

        public void CommandStart(Int64 userid, RoleInfoVo rolevo)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "SKILL_FIGHT_PUSH", "战斗技能升级推送");
#endif
                var s = Variable.OnlinePlayer.ContainsKey(userid);
                if (!s) return;
                var session = Variable.OnlinePlayer[userid] as TGGSession;
                if (session == null) return;

                var dic = new Dictionary<string, object> { { "role", rolevo } };
                var aso = new ASObject(dic);
                var pv = session.InitProtocol((int)ModuleNumber.SKILL, (int)SkillCommand.SKILL_FIGHT_PUSH, (int)ResponseType.TYPE_SUCCESS, aso);
                session.SendData(pv);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }
    }
}
