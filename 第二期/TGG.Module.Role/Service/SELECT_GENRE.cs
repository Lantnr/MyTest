using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Role.Service
{
    /// <summary>
    /// 武将流派忍者众选择
    /// </summary>
    public class SELECT_GENRE
    {
        private static SELECT_GENRE _objInstance;

        /// <summary>SELECT_GENRE单体模式</summary>
        public static SELECT_GENRE GetInstance()
        {
            return _objInstance ?? (_objInstance = new SELECT_GENRE());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "SELECT_GENRE", "武将流派忍者众选择");
#endif
                var rid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value.ToString());            //武将主键rid
                var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value.ToString());     //流派或忍者众

                if (!CheckData(type)) return Result((int)ResultType.ROLE_GENRE_ERROR);  //验证流派id

                var role = tg_role.GetEntityById(rid);
                if (role == null) return Result((int)ResultType.DATABASE_ERROR);

                return RoleSelectGenre(session, role, type);    //家臣选择流派忍者众信息
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>选择流派忍者众信息</summary>
        private ASObject RoleSelectGenre(TGGSession session, tg_role role, int type)
        {
            if (type >= (int)RoleGenreType.SCHOOL_2 && type <= (int)RoleGenreType.SCHOOL_12)
            {
                if (role.role_genre != 0) return Result((int)ResultType.ROLE_GENRE_SELECTOK);  //验证是否已经选择流派信息
                role.role_genre = type;
            }
            else if (type >= (int)RoleGenreType.NINJA_13 && type <= (int)RoleGenreType.NINJA_21)
            {
                if (role.role_ninja != 0) return Result((int)ResultType.ROLE_GENRE_SELECTOK);
                role.role_ninja = type;
            }
            //更新家臣信息
            if (!tg_role.UpdateByRole(role)) return Result((int)ResultType.DATABASE_ERROR);

            var rmain = session.Player.Role.Kind;
            if (role.id == rmain.id)       //主角则更新session
                session.Player.Role.Kind = role;

            var rolevo = (new Share.Role()).BuildRole(role.id);
            return new ASObject(Common.GetInstance().RoleLoadData((int)ResultType.SUCCESS, rolevo));
        }

        /// <summary>验证前端数据</summary>
        private bool CheckData(int type)
        {
            return type >= (int)RoleGenreType.SCHOOL_2 && type <= (int)RoleGenreType.NINJA_21;
        }

        /// <summary>组装数据</summary>
        private ASObject Result(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }
    }
}
