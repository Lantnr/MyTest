using FluorineFx;
using System;
using System.Collections.Generic;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Vo.Friend;
using TGG.Core.Vo.Messages;
using TGG.Core.Entity;
using TGG.Core.Common.Util;

namespace TGG.Module.Messages.Service
{
    /// <summary>
    /// 部分公共方法
    /// Author:arlen xiao
    /// </summary>
    public partial class Common
    {
        private static Common ObjInstance;

        /// <summary>Common 单体模式</summary>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }

        #region 组装数据

        /// <summary>组装数据</summary>
        public ASObject BuildData(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }

        /// <summary>组装数据</summary>
        public ASObject BuildData(int result, List<view_messages> list)
        {
            var dic = new Dictionary<string, object>();
            var data = list != null ? ConvertListAsObject(list) : null;
            dic.Add("result", result);
            dic.Add("data", data);
            return new ASObject(dic);
        }

        #endregion

        #region 类型转换

        /// <summary>List集合转换ASObject</summary>
        public List<ASObject> ConvertListAsObject(List<view_messages> list)
        {
#if DEBUG
            XTrace.WriteLine("集合大小 {0}", list.Count);
#endif
            var list_aso = new List<ASObject>();
            foreach (var item in list)
            {
                list_aso.Add(AMFConvert.ToASObject(EntityToVo.ToMessagesVo(item)));
            }
            return list_aso;
        }

        #endregion
    }
}
