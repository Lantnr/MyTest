using System;
using FluorineFx;
using NewLife.Log;
using System.Diagnostics;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Module.War.Service;
using TGG.Module.War.Service.Attack;
using TGG.Module.War.Service.Copy;
using TGG.Module.War.Service.Defence;
using TGG.Module.War.Service.Fight;
using TGG.Module.War.Service.Home;
using TGG.Module.War.Service.Map;
using TGG.Module.War.Service.Military;
using TGG.Module.War.Service.SkyCity;
using TGG.SocketServer;

namespace TGG.Module.War
{
    /// <summary>
    /// 指令开关类
    /// </summary>
    public class CommandSwitch : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~CommandSwitch()
        {
            Dispose();
        }

        #endregion

        /// <summary>指令处理</summary>
        public ASObject Switch(int commandNumber, TGGSession session, ASObject data)
        {
            return Switch((int)ModuleNumber.WAR, commandNumber, session, data);
        }

        /// <summary>指令处理</summary>
        public ASObject Switch(int moduleNumber, int commandNumber, TGGSession session, ASObject data)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            var aso = new ASObject();
            //指令匹配
            switch (commandNumber)
            {
                case (int)WarCommand.WAR_MODEL_IN:
                    {
                        var war = new WAR_MODEL_IN();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_MODEL_OUT:
                    {
                        var war = new WAR_MODEL_OUT();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_CITY_BUILD:
                    {
                        var war = new WAR_CITY_BUILD();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_CITY_IN:
                    {
                        var war = new WAR_CITY_IN();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DIPLOMACY:
                    {
                        var war = new WAR_DIPLOMACY();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DIPLOMACY_IN:
                    {
                        var war = new WAR_DIPLOMACY_IN();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_OK_DIPLOMACY:
                    {
                        var war = new WAR_OK_DIPLOMACY();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_UPDATE_CITY_NAME:
                    {
                        var war = new WAR_UPDATE_CITY_NAME();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_UPDATE_CITY:
                    {
                        var war = new WAR_UPDATE_CITY();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DONATION:
                    {
                        var war = new WAR_DONATION();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DIPLOMACY_JOIN:
                    {
                        var war = new WAR_DIPLOMACY_JOIN();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_UPDATE_BASE:
                    {
                        var war = new WAR_UPDATE_BASE();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_ARSON:
                    {
                        var war = new WAR_ARSON();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DESTRUCTION:
                    {
                        var war = new WAR_DESTRUCTION();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_SELETE_TARGET:
                    {
                        var war = new WAR_SELETE_TARGET();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DETERMINE_TARGET:
                    {
                        var war = new WAR_DETERMINE_TARGET();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_STATIONED:
                    {
                        var war = new WAR_STATIONED();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_TRANSFER_TARGET:
                    {
                        var war = new WAR_TRANSFER_TARGET();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_TRANSFER_DETERMINE_TARGET:
                    {
                        var war = new WAR_TRANSFER_DETERMINE_TARGET();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_GO:
                    {
                        var war = new WAR_GO();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_RESOURCES_SUMMARY:
                    {
                        var war = new WAR_RESOURCES_SUMMARY();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_TRANSFER_FORMAL:
                    {
                        var war = new WAR_TRANSFER_FORMAL();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DEVOTE_CHANGE:
                    {
                        var war = new WAR_DEVOTE_CHANGE();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_CITY_LOOK:
                    {
                        var war = new WAR_CITY_LOOK();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_ROLE_FREE:
                    {
                        var war = new WAR_ROLE_FREE();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                #region 内政策略

                case (int)WarCommand.WAR_HOME_TACTICS_JOIN:
                    {
                        var war = new WAR_HOME_TACTICS_JOIN();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_HOME_TACTICS_EXECUTION:
                    {
                        var war = new WAR_HOME_TACTICS_EXECUTION();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }

                #endregion

                #region 战争防守设定
                case (int)WarCommand.AREA_JOIN:
                    {
                        var war = new AREA_JOIN();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.AREA_SAVE:
                    {
                        var war = new AREA_SAVE();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DEFENCE_PLAN:
                    {
                        var war = new WAR_DEFENCE_PLAN();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DEFENCE_ROLE_ADD:
                    {
                        var war = new WAR_DEFENCE_ROLE_ADD();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DEFENCE_ROLE_REMOVE:
                    {
                        var war = new WAR_DEFENCE_ROLE_REMOVE();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DEFENCE_PLAN_UPDATE:
                    {
                        var war = new WAR_DEFENCE_PLAN_UPDATE();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DEFENCE_PLAN_OPEN:
                    {
                        var war = new WAR_DEFENCE_PLAN_OPEN();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DEFENSE_PLAN_CLEAR:
                    {
                        var war = new WAR_DEFENSE_PLAN_CLEAR();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }

                case (int)WarCommand.WAR_DEFENCE_PLAN_COST:
                    {
                        var war = new WAR_DEFENCE_PLAN_COST();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_ARMY_SOLDIER_SET:
                    {
                        var war = new WAR_ARMY_SOLDIER_SET();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_ARMY_SOLDIER_OPEN:
                    {
                        var war = new WAR_ARMY_SOLDIER_OPEN();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DEFENCE_FORMATION:
                    {
                        var war = new WAR_DEFENCE_FORMATION();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_DEFENCE_PLAN_CHANGE:
                    {
                        var war = new WAR_DEFENSE_PLAN_CHANGE();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                #endregion

                #region 进攻设定
                case (int)WarCommand.WAR_FIGHT_SET:
                    {
                        var war = new WAR_FIGHT_SET();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_FIGHT:
                    {
                        var war = new WAR_FIGHT();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                #endregion
                #region 武将宅
                case (int)WarCommand.WAR_HOME_JOIN:
                    {
                        var war = new WAR_HOME_JOIN();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_HOME_VIEW:
                    {
                        var war = new WAR_HOME_VIEW();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                #endregion
                #region
                case (int)WarCommand.WAR_SKYCITY_ENTER:
                    {
                        var war = new WAR_SKYCITY_ENTER();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_SKYCITY_START:
                    {
                        var war = new WAR_SKYCITY_START();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_SKYCITY_UNLOCK:
                    {
                        var war = new WAR_SKYCITY_UNLOCK();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }

                case (int)WarCommand.WAR_SKYCITY_QUIT:
                    {
                        var war = new WAR_SKYCITY_QUIT();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_SKYCITY_SELECT:
                    {
                        var war = new WAR_SKYCITY_SELECT();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                #endregion
                #region 军事 - 军需品
                case (int)WarCommand.WAR_MILITARY_ENTER:
                    {
                        var war = new WAR_MILITARY_ENTER();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_MILITARY_GOODS_BUY:
                    {
                        var war = new WAR_MILITARY_GOODS_BUY();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_MILITARY_GOODS_REFEESH:
                    {
                        var war = new WAR_MILITARY_GOODS_REFEESH();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                #endregion
                #region 军事 - 运输
                case (int)WarCommand.WAR_MILITARY_TRAN_LOCK:
                    {
                        var war = new WAR_MILITARY_TRAN_LOCK();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_MILITARY_TRAN_ENTER:
                    {
                        var war = new WAR_MILITARY_TRAN_ENTER();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_MILITARY_TRAN_MAP:
                    {
                        var war = new WAR_MILITARY_TRAN_MAP();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_MILITARY_TRAN_TARGET:
                    {
                        var war = new WAR_MILITARY_TRAN_TARGET();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_MILITARY_TRAN_START:
                    {
                        var war = new WAR_MILITARY_TRAN_START();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_MILITARY_TRAN_COMPLETE:
                    {
                        var war = new WAR_MILITARY_TRAN_COMPLETE();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                #endregion

                #region 合战副本

                case (int)WarCommand.WAR_COPY_JOIN:
                    {
                        var war = new WAR_COPY_JOIN();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_COPY_ATTACK:
                    {
                        var war = new WAR_COPY_ATTACK();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_COPY_MORALE:
                    {
                        var war = new WAR_COPY_MORALE();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_COPY_HIRE_SOLDIERS:
                    {
                        var war = new WAR_COPY_HIRE_SOLDIERS();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }

                #endregion

                #region 线路设定
                case (int)WarCommand.WAR_FIGHT_LINE_ADD:
                    {
                        var war = new WAR_FIGHT_LINE_ADD();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                case (int)WarCommand.WAR_FIGHT_LINE_REMOVE:
                    {
                        var war = new WAR_FIGHT_LINE_REMOVE();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                #endregion

                #region 战报
                case (int)WarCommand.WAR_REPORT_JOIN:
                    {
                        var war = new WAR_REPORT_JOIN();
                        aso = war.CommandStart(session, data);
                        war.Dispose(); break;
                    }
                #endregion

                default: { aso = null; break; }
            }
#if DEBUG
            sw.Stop();
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", sw.ElapsedMilliseconds.ToString(), GetType().Namespace);
#endif
            return aso;
        }
    }
}
