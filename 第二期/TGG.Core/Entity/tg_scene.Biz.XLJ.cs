using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.Enum;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 场景部分类
    /// </summary>
    public partial class tg_scene
    {
        /// <summary>初始化场景</summary>
        /// <param name="scene_id">场景基表id</param>
        /// <param name="user_id">玩家id</param>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        public static tg_scene InitScene(Int64 scene_id, Int64 user_id, int x, int y)
        {
            try
            {
                var scene = new tg_scene
                {
                    scene_id = scene_id,
                    user_id = user_id,
                    X = x,
                    Y = y,
                    model_number=(int)ModuleNumber.SCENE,
                };
                scene.Save();
                return scene;
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return null;
            }
            
        }
    }
}
