using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_scene
    {
        ///// <summary>
        ///// 根据用户id查询场景信息
        ///// </summary>
        //public static void GetSceneUpdate(view_scene_user viewdata)
        //{
        //    var entity = FindByid(viewdata.id);
        //    if (entity != null)
        //    {
        //        entity.X = viewdata.X;
        //        entity.Y = viewdata.Y;
        //        entity.scene_id = viewdata.scene_id;
        //    }
        //    Update(entity);
        //}

        /// <summary>
        /// 更新场景的数据
        /// </summary>
        public static int GetSceneUpdate(view_scene_user viewscene)
        {
            return Update(string.Format("x={0},y={1},scene_id={2},model_number ={3}", viewscene.X, viewscene.Y, viewscene.scene_id, viewscene.model_number),
                 string.Format(" id ={0}", viewscene.id));
        }
    }
}
