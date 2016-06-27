using System.Data;
using System.IO;
using System.Threading;
using NewLife.Log;
using NewLife.Reflection;
using System;
using System.Linq;
using System.Reflection;
using NewLife.Xml;
using TGG.Core.Enum;
using TGG.Core.Global;
using TGG.Core.XML;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using TGG.Core.Enum.Type;
using TGG.Core.Entity;
using TGG.Core.Common.Randoms;
using TGG.Core.Base;
using FluorineFx;

namespace TGG.Core.Common.Util
{
    /// <summary>
    /// 公共辅助类
    /// </summary>
    public class CommonHelper
    {
        #region 公共方法

        /// <summary>字符串转换为计算公式,并得出计算结果</summary>
        /// <param name="expression">字符串计算公式</param>
        public static object EvalExpress(string expression)
        {
            expression = UConvert.ToDbc(expression);
            try
            {
#if DEBUG
                XTrace.WriteLine(expression);
#endif
                return NEvalExecute(expression);
            }
            catch
            {
                try
                {
                    return expression.Contains("Math") ? XcodeScriptExecute(expression)
                     : new DataTable().Compute(expression, "");
                }
                catch (Exception ex)
                {

                    XTrace.WriteException(ex);
                    return XcodeScriptExecute(expression);
                }
            }
        }

        public static object NEvalExecute(string expression)
        {
            var rule = expression.Replace("Math.", "");
            return new NEval().Eval(rule);
        }

        /// <summary>使用xcode脚本引擎执行</summary>
        private static object XcodeScriptExecute(string expression)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine(expression);
#endif
                expression = ReplaceExtend(expression);
#if DEBUG
                XTrace.WriteLine(expression);
#endif
                var result = ScriptEngine.Execute(expression);
                return result;
            }
            catch (Exception ex)
            {
                XTrace.WriteLine("xcode脚本引擎执行错!公式计算已经无法在进行,按0处理");
                XTrace.WriteException(ex);
                return 0;
            }
        }

        /// <summary>公式强行替换方法</summary>
        private static string ReplaceExtend(string exp)
        {
            return exp.Replace("floor", "Floor")
                .Replace("ceil", "Ceiling")
                .Replace("round", "Round")
                .Replace("abs", "Abs")
                .Replace("sqrt", "Sqrt")
                .Replace("pow", "Pow")
                ;
        }


        /// <summary>刷新时间</summary>
        /// <param name="interval">The interval.刷新时间(分钟)</param>
        /// <returns>System.Double.</returns>
        public static Int64 StopTime(Int32 interval)
        {
            Int64 timeStamp = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            timeStamp = timeStamp + interval;
            return timeStamp;
        }

        /// <summary>Ticks转换时间格式</summary>
        /// <param name="ticks">Ticks</param>
        public static DateTime TickToDateTime(Int64 ticks)
        {
            Int64 temp = ticks * 10000 + 621355968000000000;
            return new DateTime(temp);
        }

        /// <summary>反射</summary>
        public static object ReflectionMethods(string assemblystring, string classname)
        {
            var asm = Assembly.Load(assemblystring);
            var t = asm.GetTypes().FirstOrDefault(m => m.Name == classname);
            if (t == null) return new object();
            dynamic obje = Activator.CreateInstance(t);
            return obje;
        }

        #endregion

        #region 线程模块方法

        /// <summary></summary>
        public static XmlModule GetCDTSTM(string value, int count, bool isrun)
        {
#if DEBUG
            XTrace.WriteLine("{0}-{1}", value, count);
#endif
            var _module = Variable.CDTSTM.Keys.FirstOrDefault(m => m.Value == value && m.Count == count);
            bool result;
            int i;
            if (_module == null)//没有对象
            {
                _module = Variable.LMM.LastOrDefault(m => m.Value == value);
                if (_module == null) return null;
                _module.Count = 0;
                result = Variable.CDTSTM.TryAdd(_module, isrun);
                i = 0;
                while (!result)
                {
                    if (i > 10) break;
#if DEBUG
                    XTrace.WriteLine("{0} - {1} - {2} -{3}", value, count, result, i);
#endif
                    result = Variable.CDTSTM.TryAdd(_module, isrun);
                    Thread.Sleep(10);
                    i++;
                }
#if DEBUG
                XTrace.WriteLine("{0} - {1} - {2}", value, count, result);
#endif
                return _module;
            }
            //有对象
            if (isrun)
            {
                Variable.CDTSTM.TryUpdate(_module, true, false);
                return _module;
            }
            _module = _module.CloneEntity();
            _module.Count += 1;
            result = Variable.CDTSTM.TryAdd(_module, false);
            i = 0;
            while (!result)
            {
                if (i > 10) break;
#if DEBUG
                XTrace.WriteLine("{0} - {1} - {2} -{3}", value, count, result, i);
#endif
                result = Variable.CDTSTM.TryAdd(_module, isrun);
                Thread.Sleep(10);
                i++;
            }
#if DEBUG
            XTrace.WriteLine("{0} - {1} - {2}", value, count, result);
#endif
            return _module;
        }

        /// <summary>状态移除或设置false</summary>
        /// <param name="value">模块</param>
        /// <param name="isrun">是否运行</param>
        public static void GetCDTSTMRemove(string value, int count)
        {
            var flag = true;
            var _module = Variable.CDTSTM.Keys.FirstOrDefault(m => m.Value == value && m.Count == count);
            if (_module == null) return;
            Variable.CDTSTM.TryRemove(_module, out flag);
        }

        /// <summary>更新模块状态</summary>
        public static void GetCDTSTMUpdate(string value, int count)
        {
            var _module = Variable.CDTSTM.Keys.FirstOrDefault(m => m.Value == value && m.Count == count);
            if (_module == null) return;
            Variable.CDTSTM.TryUpdate(_module, false, true);
        }

        #endregion

        #region 转换方法

        /// <summary>将对象转换为字节数组</summary>
        public static byte[] OToB(object o)
        {
            var formatter = new BinaryFormatter();
            var rems = new MemoryStream();
            formatter.Serialize(rems, o);
            return rems.GetBuffer();
        }

        /// <summary>将字节数组转换为对象</summary>
        public static object BToO(byte[] b)
        {
            var formatter = new BinaryFormatter();
            var rems = new MemoryStream(b);
            return formatter.Deserialize(rems);
        }

        /// <summary>获取固定规则基表数据</summary>
        public static string BaseRule(string id)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == id);
            return rule != null ? rule.value : string.Empty;
        }

        /// <summary>Double To Int</summary>
        /// <param name="value">待扩展值</param>
        /// <param name="ratio">转换比率</param>
        public static Int32 ToInt(double value, int ratio)
        {
            return Convert.ToInt32(value * ratio);
        }

        /// <summary>Double To Int</summary>
        /// <param name="value">待扩展值</param>
        /// <param name="ratio">转换比率</param>
        public static Int32 ToInt(decimal value, int ratio)
        {
            return Convert.ToInt32(value * ratio);
        }

        /// <summary>Int To Double</summary>
        /// <param name="value">待扩展值</param>
        /// <param name="ratio">转换比率</param>
        public static Double ToDouble(int value, int ratio)
        {
            return Convert.ToDouble(value) / ratio;
        }

        #endregion

        #region 功能方法
        public static string ToNewTask(string value)
        {
            //1.把每个步骤切割开，存在arr数组中
            string newvalue = "";
            var arr = SplitTaskToList(value);
            //对每个任务步骤处理
            foreach (var item in arr)
            {
                var type = item.Split('_').ToList();
                int steptype = Convert.ToInt32(type[0]);
                if (steptype == (int)TaskStepType.NPC_FIGHT_TIMES)//有次数要求的npc
                    type[3] = "0";
                type[2] = "0";
                newvalue += string.Join("_", type);
                newvalue += "|";//任务完成数归零
            }
            newvalue = newvalue.Remove(newvalue.Length - 1);
            return newvalue;
        }

        /// <summary> 任务步骤切割 </summary>
        private static IEnumerable<string> SplitTaskToList(string data)
        {
            var mylist = new List<string>();
            if (data.Contains('|'))
            {
                var steplist = data.Split('|');
                mylist.AddRange(steplist);
            }
            else
                mylist.Add(data);
            return mylist;
        }

        /// <summary>获取生活技能基表ID</summary>
        /// <param name="type">生活技能类型</param>
        public static int EnumLifeType(LifeSkillType type)
        {
            switch (type)
            {
                case LifeSkillType.ASHIGARU: return GetBaseId((int)LifeSkillType.ASHIGARU);
                case LifeSkillType.ARTILLERY: return GetBaseId((int)LifeSkillType.ARTILLERY);
                case LifeSkillType.ARCHER: return GetBaseId((int)LifeSkillType.ARCHER);
                case LifeSkillType.BUILD: return GetBaseId((int)LifeSkillType.BUILD);
                case LifeSkillType.CALCULATE: return GetBaseId((int)LifeSkillType.CALCULATE);
                case LifeSkillType.CRAFT: return GetBaseId((int)LifeSkillType.CRAFT);
                case LifeSkillType.ELOQUENCE: return GetBaseId((int)LifeSkillType.ELOQUENCE);
                case LifeSkillType.EQUESTRIAN: return GetBaseId((int)LifeSkillType.EQUESTRIAN);
                case LifeSkillType.ETIQUETTE: return GetBaseId((int)LifeSkillType.ETIQUETTE);
                case LifeSkillType.MARTIAL: return GetBaseId((int)LifeSkillType.MARTIAL);
                case LifeSkillType.MEDICAL: return GetBaseId((int)LifeSkillType.MEDICAL);
                case LifeSkillType.MINE: return GetBaseId((int)LifeSkillType.MINE);
                case LifeSkillType.NINJITSU: return GetBaseId((int)LifeSkillType.NINJITSU);
                case LifeSkillType.RECLAIMED: return GetBaseId((int)LifeSkillType.RECLAIMED);
                case LifeSkillType.TACTICAL: return GetBaseId((int)LifeSkillType.TACTICAL);
                case LifeSkillType.TEA: return GetBaseId((int)LifeSkillType.TEA);
            }
            return 0;
        }

        /// <summary>获取生活技能基表</summary>
        /// <param name="type">生活技能类型</param>
        private static int GetBaseId(int type)
        {
            var temp = Variable.BASE_LIFESKILL.FirstOrDefault(m => m.type == type);
            return temp != null ? temp.id : 0;
        }

        #endregion

        #region 随机掉装备

        #region 洗练装备方法

        /// <summary>洗练装备方法</summary>
        /// <param name="user_id">玩家id</param>
        /// <param name="base_id">装备基表id</param>
        /// <returns>如果没有对应基表数据返回数据为null</returns>
        public static tg_bag EquipReset(Int64 user_id,Int32 base_id)
        {
            var base_equip = Variable.BASE_EQUIP.FirstOrDefault(m => m.id == base_id);
            if (base_equip == null) return null;
            //非绿品质装备
            return GradeEquip(user_id, base_equip);
        }

        #endregion

        #region 获取单个绿色装备
        // <summary>获取绿色装备</summary>
        /// <param name="user_id">玩家id</param>
        /// <param name="base_id">基表装备ID</param>
        public static tg_bag GreenEquip(Int64 user_id, Int32 base_id)
        {
            var base_equip = Variable.BASE_EQUIP.FirstOrDefault(m => m.id == base_id);
            return base_equip == null ? null : GreenEquip(user_id, base_equip);
        }
        #endregion

        #region  战斗装备掉落 Fight Equip
        /// <summary>战斗随机掉落一个装备</summary>
        /// <param name="user_id">玩家ID</param>
        /// <param name="list">装备基表id集合</param>
        public static tg_bag FightRandomEquip(Int64 user_id, List<Int32> list)
        {
            var list_equip = Variable.BASE_EQUIP.Where(m => list.Contains(m.id)).ToList();
            return FightRandomEquip(user_id, list_equip);
        }

        /// <summary>战斗随机掉落一个装备</summary>
        /// <param name="user_id">玩家ID</param>
        /// <param name="list">装备集合</param>
        private static tg_bag FightRandomEquip(Int64 user_id, List<BaseEquip> list)
        {
            //战斗胜利有一半掉落装备概率
            return RandomRateEquip(user_id, 50, list);
        }

        #endregion

        #region 传入概率随机装备
        /// <summary>根据概率随机掉落一个装备</summary>
        /// <param name="user_id">玩家ID</param>
        /// <param name="rate">成功概率</param>
        /// <param name="list">装备基表id集合</param>
        public static tg_bag RateRandomEquip(Int64 user_id, Int32 rate, List<Int32> list)
        {
            var list_equip = Variable.BASE_EQUIP.Where(m => list.Contains(m.id)).ToList();
            return RandomRateEquip(user_id, rate, list_equip);
        }

        #region RateRandomEquip 扩展
        /// <summary>根据概率随机掉落一个装备</summary>
        /// <param name="user_id">玩家ID</param>
        /// <param name="rate">成功概率</param>
        /// <param name="list">装备基表id集合</param>
        public static tg_bag RateRandomEquip(Int64 user_id, Double rate, List<Int32> list)
        {
            var list_equip = Variable.BASE_EQUIP.Where(m => list.Contains(m.id)).ToList();
            return RandomRateEquip(user_id, rate, list_equip);
        }
        #endregion

        #endregion

        #region 装备掉落私有方法

        /// <summary>根据概率随机掉落一个装备</summary>
        /// <param name="user_id">玩家ID</param>
        /// <param name="rate">成功概率</param>
        /// <param name="list">装备集合</param>
        private static tg_bag RandomRateEquip(Int64 user_id, Int32 rate, List<BaseEquip> list)
        {
            if (!list.Any()) return null;
            var rs = new RandomSingle();
            //判断是否掉装备
            var isequip = rs.IsTrue(rate);
            return isequip ? RateEquip(user_id, list) : null;
        }

        #region  RandomRateEquip 扩展
        /// <summary>根据概率随机掉落一个装备</summary>
        /// <param name="user_id">玩家ID</param>
        /// <param name="rate">成功概率</param>
        /// <param name="list">装备集合</param>
        private static tg_bag RandomRateEquip(Int64 user_id, Double rate, List<BaseEquip> list)
        {
            if (!list.Any()) return null;
            var rs = new RandomSingle();
            //判断是否掉装备
            var isequip = rs.IsTrue(rate);
            return isequip ? RateEquip(user_id, list) : null;
        }
        #endregion

        /// <summary>概率获取装备</summary>
        /// <param name="user_id">玩家ID</param>
        /// <param name="list">装备集合</param>
        private static tg_bag RateEquip(Int64 user_id, List<BaseEquip> list)
        {
            var frist = list.FirstOrDefault();
            //根据概率随机一个品质
            var _grade = RandomGrade(frist.useLevel);
            //重装备集合随机一个同品质装备
            var _l = list.Where(m => m.grade == _grade).ToList();
            if (!_l.Any()) return null;
            var base_equip = RandomEquip(_l);
            if (_grade == (int)GradeType.Green)//绿色只有一个属性
            {
                return GreenEquip(user_id, base_equip);
            }
            //非绿品质装备
            return GradeEquip(user_id, base_equip);
        }

        /// <summary>随机一个装备</summary>
        /// <param name="list">同品质装备</param>
        private static BaseEquip RandomEquip(List<BaseEquip> list)
        {
            var index = RNG.Next(list.Count - 1);
            return list[index];
        }

        /// <summary>随机一个品质</summary>
        /// <param name="level"></param>
        private static int RandomGrade(int level)
        {
            var rs = new RandomSingle();

            //读取当前等级装备概率集合
            var list = Variable.BASE_EQUIPRATE.Where(m => m.level == level)
                .OrderByDescending(m => m.rate)
                .Select(m => new Objects { Name = m.grade.ToString(), Probabilities = m.rate }).ToList();
            var result = rs.RandomFun(list);

            return Convert.ToInt32(result.Name);
        }

        /// <summary>随机属性个数</summary>
        private static int RandomAttributeNumber()
        {
            var rs = new RandomSingle();
            var list = Variable.BASE_EQUIPATTRATE.OrderByDescending(m => m.rate)
                .Select(m => new Objects { Name = m.id.ToString(), Probabilities = m.rate }).ToList();
            var result = rs.RandomFun(list);
            return Convert.ToInt32(result.Name);
        }

        /// <summary>随机属性</summary>
        /// <param name="number">属性个数</param>
        /// <param name="list">属性集合</param>
        private static List<EquipItem> RandomAttribute(Int32 number, int type, List<EquipItem> list)
        {
            var list_att = new List<EquipItem>();
            for (var i = 0; i < number; i++)
            {
                var equip = RandomSingleAttribute(type, list, list_att);
                list_att.Add(equip);
            }
            return list_att;
        }

        /// <summary>随机生成一个属性</summary>
        /// <param name="type">装备类型</param>
        /// <param name="list">属性集合</param>
        /// <param name="list_att">生成属性集合</param>
        private static EquipItem RandomSingleAttribute(int type, List<EquipItem> list, List<EquipItem> list_att)
        {
            var index = RNG.Next(list.Count - 1);
            var equip = list[index];

            if (type == (int)EquipType.WEAPON || type == (int)EquipType.MOUNTS || type == (int)EquipType.ARMOR)
            {
                //判断是否基础属性
                var l = GetCheckEquip();
                if (!l.Contains(equip.type)) return equip;
                //判断已生成属性是否已经有基础属性
                var count = list_att.Count(m => l.Contains(m.type));
                return count > 0 ? RandomSingleAttribute(type, list, list_att) : equip;
            }
            return equip;

        }

        /// <summary>需要判断的基础属性集合</summary>
        private static int[] GetCheckEquip()
        {
            return new[]
            {
                (int)RoleAttributeType.ROLE_CAPTAIN,
                (int)RoleAttributeType.ROLE_FORCE,
                (int)RoleAttributeType.ROLE_BRAINS,
                (int)RoleAttributeType.ROLE_GOVERN,
                (int)RoleAttributeType.ROLE_CHARM,
            };
        }

        /// <summary>获取品质装备</summary>
        /// <param name="user_id">玩家id</param>
        /// <param name="base_equip">基表装备实体</param>
        /// <returns></returns>
        private static tg_bag GradeEquip(Int64 user_id, BaseEquip base_equip)
        {
            //根据概率随机属性个数
            var number = RandomAttributeNumber();
            var list = GetAttribute(base_equip);
            if (list.Any())
            {
                var entity = new tg_bag();
                entity.user_id = user_id;
                entity.base_id = (int)base_equip.id;
                entity.type = (int)GoodsType.TYPE_EQUIP;
                entity.equip_type = base_equip.typeSub;
                entity.state = (int)LoadStateType.UNLOAD;
                entity.count = 1;
                //number = 3;
                var list_att = RandomAttribute(number, base_equip.typeSub, list);
                var i = 0;
                foreach (var item in list_att)
                {
                    switch (i)
                    {
                        #region 属性赋值
                        case 0:
                            {
                                entity.attribute1_type = item.type;
                                entity.attribute1_value = item.value;
                                break;
                            }
                        case 1:
                            {
                                entity.attribute2_type = item.type;
                                entity.attribute2_value = item.value;
                                break;
                            }
                        case 2:
                            {
                                entity.attribute3_type = item.type;
                                entity.attribute3_value = item.value;
                                break;
                            }
                        #endregion
                    }
                    i++;
                }
                return entity;
            }
            return null;
        }



        /// <summary>获取绿色装备</summary>
        /// <param name="user_id">玩家id</param>
        /// <param name="base_equip">基表装备实体</param>
        private static tg_bag GreenEquip(Int64 user_id, BaseEquip base_equip)
        {
            var list = GetAttribute(base_equip);
            if (list.Any())
            {
                var att = list.FirstOrDefault();
                return new tg_bag
                {
                    user_id = user_id,
                    base_id = (int)base_equip.id,
                    type = (int)GoodsType.TYPE_EQUIP,
                    equip_type = base_equip.typeSub,
                    state = (int)LoadStateType.UNLOAD,
                    count = 1,
                    attribute1_type = att.type,
                    attribute1_value = att.value,
                };
            }

            return null;
        }

        /// <summary>获取基表装备属性</summary>
        /// <param name="base_equip">基表装备实体</param>
        private static List<EquipItem> GetAttribute(BaseEquip base_equip)
        {
            var list = new List<EquipItem>();
            if (base_equip.captain > 0)
                list.Add(new EquipItem { type = (int)RoleAttributeType.ROLE_CAPTAIN, value = base_equip.captain });
            if (base_equip.force > 0)
                list.Add(new EquipItem { type = (int)RoleAttributeType.ROLE_FORCE, value = base_equip.force });
            if (base_equip.brains > 0)
                list.Add(new EquipItem { type = (int)RoleAttributeType.ROLE_BRAINS, value = base_equip.brains });
            if (base_equip.govern > 0)
                list.Add(new EquipItem { type = (int)RoleAttributeType.ROLE_GOVERN, value = base_equip.govern });
            if (base_equip.charm > 0)
                list.Add(new EquipItem { type = (int)RoleAttributeType.ROLE_CHARM, value = base_equip.charm });
            if (base_equip.attack > 0)
                list.Add(new EquipItem { type = (int)RoleAttributeType.ROLE_ATTACK, value = base_equip.attack });
            if (base_equip.defense > 0)
                list.Add(new EquipItem { type = (int)RoleAttributeType.ROLE_DEFENSE, value = base_equip.defense });
            if (base_equip.life > 0)
                list.Add(new EquipItem { type = (int)RoleAttributeType.ROLE_LIFE, value = base_equip.life });
            if (base_equip.hurtIncrease > 0)
                list.Add(new EquipItem { type = (int)RoleAttributeType.ROLE_HURTINCREASE, value = base_equip.hurtIncrease });
            if (base_equip.hurtReduce > 0)
                list.Add(new EquipItem { type = (int)RoleAttributeType.ROLE_HURTREDUCE, value = base_equip.hurtReduce });
            return list;
        }

        #endregion

        public class EquipItem
        {
            /// <summary>属性类型</summary>
            public int type { get; set; }

            /// <summary>属性值</summary>
            public double value { get; set; }
        }

        #endregion

        #region 功能开放模块

        /// <summary>功能开放模块</summary>
        /// <param name="level">等级</param>
        /// <param name="identity">武将身份</param>
        /// <returns>开发模块集合</returns>
        public static List<int> GetOpenModule(int level, int identity)
        {
            var _ide = Variable.BASE_IDENTITY.FirstOrDefault(m => m.id == identity);
            if (_ide == null) return new List<int>();
            var list = Variable.BASE_MODULEOPEN.Where(m => (m.identity <= _ide.value && m.level <= level) || m.manual == 0)
                .Select(m => m.id).ToList();
            return list;
        }

        /// <summary>获取开放功能所需等级</summary>
        /// <param name="id">开放功能id</param>
        private static int GetOpenLevel(int id)
        {
            var base_open = Variable.BASE_MODULEOPEN.FirstOrDefault(m => m.id == id);
            return base_open != null ? base_open.level : 0;
        }

        #endregion

        #region 模块验证

        /// <summary>模块验证是否开放</summary>
        /// <param name="level">玩家等级</param>
        /// <param name="openid">模块开放id</param>
        public static bool IsOpen(int level, int openid)
        {
            var open_level = GetOpenLevel(openid);
            return level >= open_level;
        }

        /// <summary>错误结果返回</summary>
        /// <param name="type">数据结构返回值</param>
        public static ASObject ErrorResult(ResultType type)
        {
            var dic = new Dictionary<string, object> { { "result", (int)type } };
            return new ASObject(dic);
        }

        /// <summary>错误结果返回</summary>
        /// <param name="type">数据结构返回值</param>
        public static ASObject ErrorResult(int type)
        {
            var dic = new Dictionary<string, object> { { "result", type } };
            return new ASObject(dic);
        }

        #endregion

        #region 配置文件启用

        /// <summary>启用GM指令</summary>
        public static bool IsGM()
        {
            // GM指令启用 0:不启用,1:启用
            var isgm = System.Configuration.ConfigurationManager.AppSettings["gm"].ToString();
            return isgm.Equals("1");
        }

        /// <summary>激活验证</summary>
        /// <param name="user_code">账号</param>
        public static bool CheckActivation(string user_code)
        {
            // 激活验证 0:不验证,1:验证
            var ischeck = System.Configuration.ConfigurationManager.AppSettings["act"].ToString();
            if (!ischeck.Equals("1")) return true;
            var _where = string.Format("[user_code]='{0}'", user_code);
            var _count = code.FindCount(_where, null, null, 0, 0);
            return _count > 0;
        }

        /// <summary>防沉迷</summary>
        public static bool CheckOpenTime()
        {
            var ischeck = System.Configuration.ConfigurationManager.AppSettings["fcm"].ToString();// 反沉迷 0:不验证,1:验证
            return ischeck.Equals("1");
        }

        #endregion

    }
}
