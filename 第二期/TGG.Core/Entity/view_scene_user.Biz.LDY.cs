using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace TGG.Core.Entity
{

    public partial class view_scene_user
    {
        /// <summary>
        /// 根据用户id查询场景信息
        /// </summary>
        public static view_scene_user GetSceneByUserid(Int64 userid)
        {
            return Find(new string[] { _.user_id }, new object[] { userid });
        }
    }
}
