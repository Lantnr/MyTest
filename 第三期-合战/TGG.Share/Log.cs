using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Share
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class Log
    {
        /// <summary>写入日志</summary>
        /// <param name="user_id">用户id</param>
        /// <param name="type">操作类型</param>
        /// <param name="mn">模块号</param>
        /// <param name="cn">指令号</param>
        /// <param name="mnname">模块名称</param>
        /// <param name="cnname">指令名称</param>
        /// <param name="name">资源名称</param>
        /// <param name="resourcetype">资源类型</param>
        /// <param name="count">操作数量</param>
        /// <param name="surplus">剩余数量</param>
        /// <param name="data"></param>
        public void WriteLog(Int64 user_id, int type, int mn, int cn, string mnname, string cnname, string name, int resourcetype, Int64 count, Int64 surplus, string data)
        {
            var log = new tg_log_operate
            {
                user_id = user_id,
                type = type,
                module_number = mn,
                module_name = mnname,
                command_name = cnname,
                command_number = cn,
                resource_name = name,
                resource_type = resourcetype,
                count = count,
                surplus = surplus,
                time = DateTime.Now,
                data = data,
            };
            tg_log_operate.Insert(log);
        }

        public void WriteLog(Int64 user_id, int type, int mn, int cn, string cnname, DataObject dataobject)
        {
            var log = new tg_log_operate
            {
                user_id = user_id,
                type = type,
                module_number = mn,
                module_name = GetModuleName(mn),
                command_name = cnname,
                command_number = cn,
                resource_name =  GetString(dataobject),
                resource_type = dataobject.type,
                count = dataobject.cost,
                surplus = dataobject.newres,
                time = DateTime.Now,
                data = GetData(dataobject),
            };
            tg_log_operate.Insert(log);
        }

        /// <summary>启服/关服日志</summary>
        public void WriteServerLog(int type = 0)
        {
            var run = new tg_log_run
            {
                time = DateTime.Now,
                type = type,
            };
            tg_log_run.Insert(run);
        }

        /// <summary>背包日志</summary>
        public void BagLog(tg_bag prop, LogType type, int mn, int cn, string mnname, string cnname, int getcoin, int count)
        {
            try
            {
                log.BagInsertLog(prop, mn, cn, getcoin); //记录日志
                var data = string.Format("{0}_{1}", "Prop", prop.id);//日志记录
                string s = GetString(prop);
                WriteLog(prop.user_id, (int)type, mn, cn, mnname, cnname, s, (int)GoodsType.TYPE_PROP, count, prop.count - count, data);
            }
            catch(Exception ex) { XTrace.WriteException(ex);}
        }

        /// <summary>背包日志</summary>
        public void BagLog(tg_bag prop, LogType type, int mn, int cn, string cnname, int count, int getcoin = 0)
        {
            try
            {
                var temp_prop = prop.CloneEntity();

                log.BagInsertLog(temp_prop, mn, cn, getcoin); //记录日志
                string s = GetString(temp_prop);
                var obj = new DataObject(temp_prop.type, temp_prop.count, temp_prop.count - count, count, s);
                WriteLog(temp_prop.user_id, (int)type, mn, cn, cnname, obj);
            }
            catch (Exception ex) { XTrace.WriteException(ex); }
        }


        /// <summary>获取道具名</summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public string GetString(tg_bag prop)
        {
            string s = "道具";
            #region
            switch (prop.type)
            {
                case (int)GoodsType.TYPE_PROP:
                    {
                        var bp = Variable.BASE_PROP.FirstOrDefault(m => m.id == prop.base_id);
                        if (bp != null)
                            s = bp.name;
                    }
                    break;
                case (int)GoodsType.TYPE_FUSION:
                    {
                        var bp1 = Variable.BASE_FUSION.FirstOrDefault(m => m.id == prop.base_id);
                        if (bp1 != null)
                            s = bp1.name;
                    }
                    break;
                case (int)GoodsType.TYPE_EQUIP:
                    {
                        var bp2 = Variable.BASE_EQUIP.FirstOrDefault(m => m.id == prop.base_id);
                        if (bp2 != null)
                            s = bp2.name;
                    }
                    break;
            }
            #endregion
            return s;
        }

        /// <summary>获取道具名</summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public string GetString(ResourcesItem res)
        {
            string s = "道具";
            #region
            switch (res.type)
            {
                case (int)GoodsType.TYPE_PROP:
                    {
                        var bp = Variable.BASE_PROP.FirstOrDefault(m => m.id == res.id);
                        if (bp != null)
                            s = bp.name;
                    }
                    break;
                case (int)GoodsType.TYPE_FUSION:
                    {
                        var bp1 = Variable.BASE_FUSION.FirstOrDefault(m => m.id == res.id);
                        if (bp1 != null)
                            s = bp1.name;
                    }
                    break;
                case (int)GoodsType.TYPE_EQUIP:
                    {
                        var bp2 = Variable.BASE_EQUIP.FirstOrDefault(m => m.id == res.id);
                        if (bp2 != null)
                            s = bp2.name;
                    }
                    break;
            }
            #endregion
            return s;
        }

        #region 组装资源数据


        /// <summary>获取资源名字</summary>
        private string GetString(DataObject data)
        {
            string s = "";
            if (data.type != (int)GoodsType.TYPE_PROP && data.type != (int)GoodsType.TYPE_FUSION && data.type != (int)GoodsType.TYPE_EQUIP)
            {
                s = GetResourceName(data.type);
            }
            else
                s = data.name;
            return s;
        }

        /// <summary>获取资源名</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetResourceName(int type)
        {
            switch (type)
            {
                case (int)GoodsType.TYPE_COIN: return "金钱";
                case (int)GoodsType.TYPE_RMB: return "内币";
                case (int)GoodsType.TYPE_GOLD: return "元宝";
                case (int)GoodsType.TYPE_COUPON: return "礼券";
                case (int)GoodsType.TYPE_EXP: return "经验";
                case (int)GoodsType.TYPE_HONOR: return "功勋";
                case (int)GoodsType.TYPE_FAME: return "声望";
                case (int)GoodsType.TYPE_SPIRIT: return "魂";
                case (int)GoodsType.TYPE_POWER: return "体力";
                case (int)GoodsType.TYPE_MERIT: return "战功值";
                case (int)GoodsType.TYPE_DONATE: return "贡献度";
            }
            return "资源";
        }


        /// <summary>获取模块名</summary>
        /// <param name="mn"></param>
        /// <returns></returns>
        private string GetModuleName(int mn)
        {
            switch (mn)
            {
                #region 模块名
                case (int)ModuleNumber.USER: return "用户";
                case (int)ModuleNumber.TASK: return "任务";
                case (int)ModuleNumber.BUSINESS: return "跑商";
                case (int)ModuleNumber.BAG: return "背包";
                case (int)ModuleNumber.SCENE: return "场景";
                case (int)ModuleNumber.EQUIP: return "装备";
                case (int)ModuleNumber.ROLE: return "武将";
                case (int)ModuleNumber.DUPLICATE: return "副本-一骑当千";
                case (int)ModuleNumber.RANKINGS: return "排名";
                case (int)ModuleNumber.MESSAGES: return "邮件";
                case (int)ModuleNumber.CHAT: return "聊天";
                case (int)ModuleNumber.FIGHT: return "战斗";
                case (int)ModuleNumber.SKILL: return "技能";
                case (int)ModuleNumber.ROLETRAIN: return "武将修行";
                case (int)ModuleNumber.FAMILY: return "家族";
                case (int)ModuleNumber.FRIEND: return "好友";
                case (int)ModuleNumber.NOTICE: return "公告";
                case (int)ModuleNumber.APPRAISE: return "家臣评定";
                case (int)ModuleNumber.SINGLEFIGHT: return "一将讨";
                case (int)ModuleNumber.ARENA: return "竞技场";
                case (int)ModuleNumber.TITLE: return "称号";
                case (int)ModuleNumber.PRISON: return "监狱";
                case (int)ModuleNumber.SIEGE: return "美浓攻略";
                case (int)ModuleNumber.BUILDING: return "一夜墨俣";
                case (int)ModuleNumber.WORK: return "工作";
                case (int)ModuleNumber.GUIDE: return "大名令";
                case (int)ModuleNumber.GAMES: return "小游戏";
                case (int)ModuleNumber.FUSION: return "熔炼";
                case (int)ModuleNumber.WAR: return "合战";
                case (int)ModuleNumber.ACTIVITY: return "开服活动";
                #endregion
            }
            return "系统模块";
        }


        /// <summary>组装data</summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetData(DataObject data)
        {
            switch (data.type)
            {
                #region data字符串
                case (int)GoodsType.TYPE_COIN: return GetString("Coin", data);
                case (int)GoodsType.TYPE_RMB: return GetString("Rmb", data);
                case (int)GoodsType.TYPE_GOLD: return GetString("Gold", data); ;
                case (int)GoodsType.TYPE_COUPON: return GetString("Coupon", data); ;
                case (int)GoodsType.TYPE_EXP: return GetString("Exp", data); ;
                case (int)GoodsType.TYPE_PROP: return GetString("Prop", data); ;
                case (int)GoodsType.TYPE_EQUIP: return GetString("Equip", data); ;
                case (int)GoodsType.TYPE_HONOR: return GetString("Honor", data); ;
                case (int)GoodsType.TYPE_FAME: return GetString("Fame", data); ;
                case (int)GoodsType.TYPE_SPIRIT: return GetString("Spirit", data); ;
                case (int)GoodsType.TYPE_POWER: return GetString("Power", data); ;
                case (int)GoodsType.TYPE_FUSION: return GetString("Fusion", data); ;
                case (int)GoodsType.TYPE_MERIT: return GetString("Merit", data); ;
                case (int)GoodsType.TYPE_DONATE: return GetString("Donate", data);
                #endregion
            }
            return "资源";
        }

        private string GetString(string value, DataObject data)
        {
            return string.Format("{0}_{1}_{2}_{3}", value, data.oldres, data.cost, data.newres);
        }

        public class DataObject
        {
            public DataObject(int type, int oldres, int newres, int cost, string name)
            {
                this.type = type;
                this.oldres = oldres;
                this.cost = cost;
                this.newres = newres;
                this.name = name;
            }

            /// <summary>资源类型</summary>
            public int type { get; set; }

            /// <summary>资源名称</summary>
            public string name { get; set; }

            /// <summary>旧资源</summary>
            public int oldres { get; set; }

            /// <summary>花费数量</summary>
            public int cost { get; set; }

            /// <summary>新资源</summary>
            public int newres { get; set; }
        }

        #endregion

    }


}
