using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.War;

namespace TGG.Module.War.Service
{
    /// <summary>
    /// 部分公共方法
    /// </summary>
    public partial class Common
    {

        public static Common ObjInstance;

        /// <summary>Common 单体模式</summary>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }

        public List<ASObject> ConvertListASObject(List<view_user_area_set> list)
        {
            #region 非LINQ实现
            //var listaso1 = new List<ASObject>();
            //var ids = list.Select(q => q.id).Distinct().ToList();
            //var listvo = new List<LandInfoVo>();
            //var location = 1;
            //foreach (var id in ids)
            //{
            //    var toaso = C(list.Where(m => m.id == id).ToList());
            //    listvo.Add(new LandInfoVo()
            //    {
            //        id = id,
            //        landId = location,
            //        list = toaso
            //    });
            //    location++;
            //}
            //listaso = listvo.Select(AMFConvert.ToASObject).ToList();
            #endregion

            var listvo1 = list.GroupBy(q => q.id).Select(
                  q => new LandInfoVo()
                  {
                      id = q.Key,
                      landId = list.Where(m => m.id == q.Key).FirstOrDefault().location,
                      list = ConverAreaSet(list.Where(m => m.id == q.Key && m.base_id != 0 && m.type != (int)AreaType.陷阱).ToList()),
                      trap = ConverAreaSet(list.Where(m => m.id == q.Key && m.base_id != 0 && m.type == (int)AreaType.陷阱).ToList()),
                  }).ToList();
            return listvo1.Select(AMFConvert.ToASObject).ToList();
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<ASObject> ConverAreaSet(List<view_user_area_set> list)
        {
            return !list.Any() ? null : list.Select(set => AMFConvert.ToASObject(EntityToVo.ToAreaSetVo(set))).ToList();
        }
    }
}
