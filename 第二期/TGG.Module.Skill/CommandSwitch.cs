﻿using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Module.Skill.Service;
using TGG.SocketServer;

namespace TGG.Module.Skill
{
    /// <summary>
    /// 指令开关类
    /// </summary>
    public class CommandSwitch
    {
        public static CommandSwitch ObjInstance;

        /// <summary>CommandSwitch单例模式</summary>
        public static CommandSwitch GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new CommandSwitch());
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int commandNumber, TGGSession session, ASObject data)
        {
            return Switch((int)ModuleNumber.SKILL, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            if (!CommonHelper.IsOpen(session.Player.Role.Kind.role_level, (int)OpenModelType.技能))
                return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_LEVEL_ERROR);
            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)SkillCommand.SKILL_FIGHT_STUDY: { aso = SKILL_FIGHT_STUDY.GetInstance().CommandStart(session, data); break; }
                case (int)SkillCommand.SKILL_LIFE_STUDY: { aso = SKILL_LIFE_STUDY.GetInstance().CommandStart(session, data); break; }
                case (int)SkillCommand.SKILL_FIGHT_UPGRADE: { aso = SKILL_FIGHT_UPGRADE.GetInstance().CommandStart(session, data); break; }
                case (int)SkillCommand.SKILL_LIFE_ACCELERATE: { aso = SKILL_LIFE_ACCELERATE.GetInstance().CommandStart(session, data); break; }
                case (int)SkillCommand.SKILL_FIGHT_ACCELERATE: { aso = SKILL_FIGHT_ACCELERATE.GetInstance().CommandStart(session, data); break; }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
