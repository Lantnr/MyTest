using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.War;
using TGG.Module.War.Service.Fight;
using TGG.Share.Event;

namespace TGG.Module.War.Service
{
    public partial class Common
    {
        #region 公共方法

        public List<ASObject> ConverDefenseAreas(Int64 planid)
        {
            var area = tg_war_plan_area.GetEntityByPlanId(planid);
            return !area.Any() ? null : area.Select(set => AMFConvert.ToASObject(EntityToVo.ToAreaSetVo(set))).ToList();
        }

        /// <summary>
        /// 获取进攻武将的攻击范围
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public List<Point> GetAttackRangeInit(int baseid, int x, int y)
        {
            var list = new List<Point>();

            var basesoldier = Variable.BASE_WAR_ARMY_SOLDIER.FirstOrDefault(q => q.id == baseid);
            if (basesoldier == null) return new List<Point>();
            list.AddRange(GetOneRoleRange(basesoldier.range, x, y));

            return list;
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="baseid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Resourse> GetResourseString(Int32 baseid, int type)
        {
            var listentity = new List<Resourse>();
            var baseinfo = Variable.BASE_WAR_ARMY_SOLDIER.FirstOrDefault(q => q.id == baseid);
            if (baseinfo == null) return null;
            var re_string = baseinfo.cost;
            listentity.AddRange(Resourse.GetList(re_string, type));

            return listentity;
        }

        /// <summary>合战资源类 </summary>
        public class Resourse
        {
            /// <summary> 合战资源类型</summary>
            public Int32 type { get; set; }

            /// <summary> 合战资源值</summary>
            public Int32 value { get; set; }

            /// <summary> 增加或者减少 1：增加 2：减少</summary>
            public Int32 addOrReduce { get; set; }

            public static List<Resourse> GetList(string resourseString, int addOrReduce)
            {
                var list = new List<Resourse>();

                var re_info = resourseString.Split("|");
                foreach (var s in re_info)
                {
                    var sp = s.Split("_");
                    if (sp.Count() != 2) continue;
                    list.Add(new Resourse()
                    {
                        type = Convert.ToInt32(sp[0]),
                        value = Convert.ToInt32(sp[1])
                    });
                }
                return list;
            }

        }

        /// <summary>
        /// 获取单个进攻武将攻击范围
        /// </summary>
        /// <param name="baserange"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public List<Point> GetOneRoleRange(string baserange, int x, int y)
        {
            //将每种形状转换成战争地图信息
            var sharplist = new Common().GetSharp(baserange, x, y);

            return (from sharp in sharplist
                    select GetFightNewPoint(sharp)
                        into newpoint
                        where newpoint != null
                        select new Point()
                        {
                            X = newpoint.X,
                            Y = newpoint.Y,
                        }).ToList();
        }

        /// <summary>
        ///初始防守武将攻击范围
        /// </summary>
        /// <param name="roles"> </param>
        /// <returns></returns>
        public List<DefenseRange> GetDefenseRangeInit(List<DefenseRoles> roles)
        {
            var list = new List<DefenseRange>();

            foreach (var role in roles)
            {
                if (role.SoldierCount <= 0) continue;
                var basesoldier = Variable.BASE_WAR_ARMY_SOLDIER.FirstOrDefault(q => q.id == role.SoldierId);
                if (basesoldier == null)
                {
                    return null;
                }
                list.AddRange(GetOneRoleRange(basesoldier.range, role.X, role.Y, role.RoleId, role.type));
            }
            return list;
        }

        /// <summary>
        /// 将每一个防守武将攻击位置添加到战争地图
        /// </summary>
        /// <param name="baserange">攻击范围基表数据</param>
        /// <param name="x">武将在战争地图的x坐标</param>
        /// <param name="y">武将在战争地图的y坐标</param>
        /// <param name="rid">武将主键id</param>
        /// <param name="type">武将类型</param>
        /// <returns></returns>
        public List<DefenseRange> GetOneRoleRange(string baserange, int x, int y, Int64 rid, int type)
        {
            //将每种形状转换成战争地图信息
            var sharplist = new Common().GetSharp(baserange, x, y);

            return (from sharp in sharplist
                    select GetFightNewPoint(sharp)
                        into newpoint
                        where newpoint != null
                        select new DefenseRange()
                        {
                            RoleId = rid,
                            X = newpoint.X,
                            Y = newpoint.Y,
                            type = type,
                        }).ToList();
        }

        /// <summary>
        /// 获取新的坐标信息
        /// </summary>
        /// <param name="sharp">缩略图形状实体</param>
        /// <returns></returns>
        public static Point GetFightNewPoint(AreaSharp sharp)
        {
            #region 获取新的坐标信息

            var point = new Point();
            switch (sharp.index)
            {
                case 0: { point.X = sharp.X - 2; point.Y = sharp.Y - 2; } break;
                case 1: { point.X = sharp.X - 1; point.Y = sharp.Y - 2; } break;
                case 2: { point.X = sharp.X - 0; point.Y = sharp.Y - 2; } break;
                case 3: { point.X = sharp.X + 1; point.Y = sharp.Y - 2; } break;
                case 4: { point.X = sharp.X + 2; point.Y = sharp.Y - 2; } break;

                case 5: { point.X = sharp.X - 2; point.Y = sharp.Y - 1; } break;
                case 6: { point.X = sharp.X - 1; point.Y = sharp.Y - 1; } break;
                case 7: { point.X = sharp.X - 0; point.Y = sharp.Y - 1; } break;
                case 8: { point.X = sharp.X + 1; point.Y = sharp.Y - 1; } break;
                case 9: { point.X = sharp.X + 2; point.Y = sharp.Y - 1; } break;

                case 10: { point.X = sharp.X - 2; point.Y = sharp.Y; } break;
                case 11: { point.X = sharp.X - 1; point.Y = sharp.Y; } break;
                case 12: { point.X = sharp.X; point.Y = sharp.Y; } break;
                case 13: { point.X = sharp.X + 1; point.Y = sharp.Y; } break;
                case 14: { point.X = sharp.X + 2; point.Y = sharp.Y; } break;

                case 15: { point.X = sharp.X - 2; point.Y = sharp.Y + 1; } break;
                case 16: { point.X = sharp.X - 1; point.Y = sharp.Y + 1; } break;
                case 17: { point.X = sharp.X - 0; point.Y = sharp.Y + 1; } break;
                case 18: { point.X = sharp.X + 1; point.Y = sharp.Y + 1; } break;
                case 19: { point.X = sharp.X + 2; point.Y = sharp.Y + 1; } break;

                case 20: { point.X = sharp.X - 2; point.Y = sharp.Y + 2; } break;
                case 21: { point.X = sharp.X - 1; point.Y = sharp.Y + 2; } break;
                case 22: { point.X = sharp.X - 0; point.Y = sharp.Y + 2; } break;
                case 23: { point.X = sharp.X + 1; point.Y = sharp.Y + 2; } break;
                case 24: { point.X = sharp.X + 2; point.Y = sharp.Y + 2; } break;
            }

            #endregion

            if (point.X < 0 || point.X > 15 || point.Y < 0 || point.Y > 8) return null;
            return point;
        }

        /// <summary>
        /// 根据用户地形设定获取玩家战争地图中的地形设定
        /// </summary>
        /// <param name="listset">地形设定集合</param>
        /// <returns></returns>
        public List<MapArea> GetMapArea(List<tg_war_area_set> listset)
        {
            var list = new List<MapArea>();
            foreach (var item in listset)
            {
                if (item.base_point_x <= 0 || item.base_point_y <= 0) continue;
                //查询地形基表
                var baseinfo = Variable.BASE_WAR_AREA.FirstOrDefault(q => q.id == item.base_id);
                if (baseinfo == null) continue;
                item.type = baseinfo.type;
                //将每种地形转换成战争地图信息
                var sharplist = GetSharp(baseinfo.sharp, item.base_point_x, item.base_point_y);
                list.AddRange(GetOneSharpToMap(sharplist, item.base_id, baseinfo.type, baseinfo.sunEffect, baseinfo.rainEffect));
            }
            return list;
        }

        /// <summary>
        /// 根据用户防守方案地形设定生成战争地图地形设定
        /// </summary>
        /// <param name="listset"></param>
        /// <returns></returns>
        public List<MapArea> GetMapArea(List<tg_war_plan_area> listset)
        {
            var list = new List<MapArea>();
            foreach (var item in listset)
            {
                if (item.base_point_x <= 0 || item.base_point_y <= 0) continue;
                //查询地形基表
                var baseinfo = Variable.BASE_WAR_AREA.FirstOrDefault(q => q.id == item.base_id);
                if (baseinfo == null) continue;

                //将每种地形转换成战争地图信息
                var sharplist = GetSharp(baseinfo.sharp, item.base_point_x, item.base_point_y);
                list.AddRange(GetOneSharpToMap(sharplist, item.base_id, baseinfo.type, baseinfo.sunEffect, baseinfo.rainEffect));
            }
            return list;
        }

        /// <summary>
        /// 将基表地形装换成AreaSharp实体集合
        /// </summary>
        /// <param name="basesharp">基表数据 例如1_0_0_0_0_0_0_0_0</param>
        /// <param name="x">在战争地图上的x坐标</param>
        /// <param name="y">在战争地图上的y坐标</param>
        public List<AreaSharp> GetSharp(string basesharp, int x, int y)
        {
            var list = new List<AreaSharp>();
            if (x == 0 || x == 1 || x == 2) basesharp = "0_0_0_0_0_0_1_1_1_0_0_1_1_1_0_0_1_1_1_0_0_0_0_0_0";
            var splitlist = basesharp.Split("_").ToList();

            for (var i = 0; i < splitlist.Count; i++)
            {
                if (splitlist[i] != "1") continue;
                list.Add(new AreaSharp()
                {
                    index = i,
                    X = x,
                    Y = y,
                });
            }
            return list;
        }


        /// <summary>
        /// 将每一种形状装换成战争地图信息 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="baseid"></param>
        /// <param name="type">地形类型</param>
        /// <param name="suneffect">晴天地形效果</param>
        /// <param name="raineffect">雨天地形效果</param>
        /// <returns></returns>
        public List<MapArea> GetOneSharpToMap(List<AreaSharp> list, Int32 baseid, int type, string suneffect, string raineffect)
        {
            return (from sharp in list
                    select new MapArea()
                    {
                        base_id = baseid,
                        type = type,
                        X = GetNewPoint(sharp).Item1,
                        Y = GetNewPoint(sharp).Item2,
                        position = sharp.index + 1,
                        suneffect = suneffect,
                        raineffect = raineffect,
                    }).ToList();
        }


        /// <summary>
        /// 获取新的坐标信息
        /// </summary>
        /// <param name="sharp"></param>
        /// <returns></returns>
        private Tuple<int, int> GetNewPoint(AreaSharp sharp)
        {
            #region 获取新的坐标信息
            switch (sharp.index)
            {
                case 1: { return Tuple.Create(sharp.X + 1, sharp.Y); }
                case 2: { return Tuple.Create(sharp.X + 2, sharp.Y); }
                case 3: { return Tuple.Create(sharp.X, sharp.Y + 1); }
                case 4: { return Tuple.Create(sharp.X + 1, sharp.Y + 1); }
                case 5: { return Tuple.Create(sharp.X + 2, sharp.Y + 1); }
                case 6: { return Tuple.Create(sharp.X, sharp.Y + 2); }
                case 7: { return Tuple.Create(sharp.X + 1, sharp.Y + 2); }
                case 8: { return Tuple.Create(sharp.X + 2, sharp.Y + 2); }
            }
            #endregion
            return Tuple.Create(sharp.X, sharp.Y);
        }


        #endregion

        /// <summary>
        /// 战争地图地形类
        /// </summary>
        public class MapArea : Point
        {
            /// <summary> 地形基表id </summary>
            public int base_id { get; set; }

            /// <summary> 地形类型 </summary>
            public int type { get; set; }

            /// <summary> 九宫格的位置 1开始</summary>
            public int position { get; set; }

            /// <summary> 晴天效果 </summary>
            public string suneffect { get; set; }

            /// <summary> 雨天效果 </summary>
            public string raineffect { get; set; }

            /// <summary> 晴天效果作用兵种 </summary>
            public string sunSoldiers { get; set; }

            /// <summary> 雨天效果作用兵种 </summary>
            public string rainSoldiers { get; set; }
        }

        /// <summary> 地形类 </summary>
        public class AreaSharp : Point
        {
            /// <summary>九宫格的索引 </summary>
            public int index { get; set; }
        }

        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }



        /// <summary>
        /// 公式计算
        /// </summary>
        /// <param name="id"></param>
        /// <param name="basedata"></param>
        /// <returns></returns>
        public int GetRule(string id, double basedata)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == id);
            if (rule == null) return 0;
            var str = "";
            switch (id)
            {
                case "32056": { str = rule.value.Replace("n", Convert.ToString(basedata)); break; }
                case "32091": { str = rule.value.Replace("captain", Convert.ToString(basedata)); break; }

                case "32087": { str = rule.value.Replace("attack", Convert.ToString(basedata)); break; } //合战装备攻击力
                case "32095": { str = rule.value.Replace("hurt", Convert.ToString(basedata)); break; }
                case "32090": { str = rule.value.Replace("defense", Convert.ToString(basedata)); break; } //合战装备防御力
                case "32092": { str = rule.value.Replace("brain", Convert.ToString(basedata)); break; } //合战减伤

            }
            var express = CommonHelper.EvalExpress(str);
            return Convert.ToInt32(express);

        }
        public double GetRule(string id, double basedata, double basedata1)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == id);
            if (rule == null) return 0;
            var str = "";
            switch (id)
            {
                case "32094": //总命中
                    {
                        str = rule.value.Replace("a", Convert.ToString(basedata));
                        str = str.Replace("b", Convert.ToString(basedata1)); break;
                    }
                case "32088":
                    {
                        str = rule.value.Replace("force", Convert.ToString(basedata));
                        str = str.Replace("captain", Convert.ToString(basedata));
                        return (int)Math.Ceiling(Convert.ToDouble(CommonHelper.EvalExpress(str)));
                    }

            }
            var express = CommonHelper.EvalExpress(str);
            return Convert.ToDouble(express);

        }

        public double GetRule(string id, List<double> data)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == id);
            if (rule == null) return 0;
            var str = "";
            switch (id)
            {
                case "32093": //伤害
                    {
                        str = rule.value.Replace("b", Convert.ToString(data[0]));
                        str = str.Replace("c", Convert.ToString(data[1]));
                        str = str.Replace("d", Convert.ToString(data[2]));
                        str = str.Replace("e", Convert.ToString(data[3]));
                        str = str.Replace("f", Convert.ToString(data[4]));
                        str = str.Replace("g", Convert.ToString(data[5]));
                        str = str.Replace("h", Convert.ToString(data[6]));
                        str = str.Replace("j", Convert.ToString(data[8]));
                        str = str.Replace("i", Convert.ToString(data[7]));
                        break;
                    }
            }
            var express = CommonHelper.EvalExpress(str);
            return Convert.ToDouble(express);

        }

        /// <summary>
        /// 公式计算
        /// </summary>
        /// <param name="id"></param>
        /// <param name="basedata"></param>
        /// <returns></returns>
        public int GetRule(string id, double basedata, double basedata1, double basedata2)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == id);
            if (rule == null) return 0;
            var str = "";
            switch (id)
            {
                case "32089":
                    {
                        str = rule.value.Replace("attack", Convert.ToString(basedata));
                        str = str.Replace("energy", Convert.ToString(basedata1));
                        str = str.Replace("count", Convert.ToString(basedata2));
                        break;
                    }

            }
            var express = CommonHelper.EvalExpress(str);
            return Convert.ToInt32(express);

        }

        /// <summary>
        /// 公式计算
        /// </summary>
        /// <param name="id"></param>
        /// <param name="basedata"></param>
        /// <returns></returns>
        public int GetRule(string id)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == id);
            if (rule == null) return 0;

            return Convert.ToInt32(rule.value);

        }

        /// <summary>
        /// 验证进攻路线的坐标
        /// </summary>
        /// <param name="rolelines">前端线路vo集合</param>
        /// <param name="planid">防守方案id</param>
        /// <returns></returns>
        public bool CheckPoint(List<WarRolesLinesVo> rolelines, Int64 planid)
        {
            var m_areas = tg_war_plan_area.GetListByPlanAndAreaType(planid, (int)AreaType.山脉);
            //  var allpoint = rolelines.SelectMany(q => q.lines).ToList();    //效率有待验证
            // if (allpoint.lines.Any(q => q.x > 17 || q.x < 0 || q.y > 8 || q.y < 0)) return false;
            var roles = rolelines.Select(q => q.rid).Distinct().ToList();
            foreach (var roleline in roles)
            {
                Int64 roleline1 = roleline;
                var lines = rolelines.FirstOrDefault(q => q.rid == roleline1).lines;
                for (int i = 0; i < lines.Count; i++)
                {
                    //验证山脉
                    if (lines.Any(q => q.x > 17 || q.x < 0 || q.y > 8 || q.y < 0 || m_areas.Any(m => m.base_point_x == q.x &&
                        m.base_point_y == q.y))) return false;
                    //验证首坐标
                    if (i == 0) { if (lines[i].x != 16 && lines[i].x != 17) return false; continue; }

                    //验证尾坐标
                    if (i == lines.Count - 1)
                    {
                        if (lines[i].x != 0 || lines[i].y != 4) return false;
                        continue;
                    }

                    //验证坐标正确性  1.x相等，abs(y)==1符合  2.y相等，前一个x-后一个x==1
                    if ((lines[i].x == lines[i - 1].x && Math.Abs(lines[i].y - lines[i - 1].y) == 1)
                        || (lines[i].y == lines[i - 1].y && lines[i - 1].x - lines[i].x == 1))

                        continue;

                    return false;
                }
            }
            return true;
        }


        #region Arlen 2015-02-12 城门攻陷后攻击方攻击范围

        /// <summary>城门攻陷后攻击范围方法</summary>
        public List<Point> Sacked(int x, int y)
        {
            //将每种形状转换成战争地图信息
            var sharplist = new Common().GetSharp(x, y);
            return (from sharp in sharplist
                    select GetFightNewPoint(sharp)
                        into newpoint
                        where newpoint != null
                        select new Point()
                        {
                            X = newpoint.X,
                            Y = newpoint.Y,
                        }).ToList();
        }

        /// <summary>将基表地形装换成AreaSharp实体集合</summary>
        public List<AreaSharp> GetSharp(int x, int y)
        {
            var list = new List<AreaSharp>();
            var basesharp = "0_0_0_0_0_0_1_1_1_0_0_1_1_1_0_0_1_1_1_0_0_0_0_0_0";
            var splitlist = basesharp.Split("_").ToList();
            for (var i = 0; i < splitlist.Count; i++)
            {
                if (splitlist[i] != "1") continue;
                list.Add(new AreaSharp()
                {
                    index = i,
                    X = x,
                    Y = y,
                });
            }
            return list;
        }

        #endregion

    }
}
