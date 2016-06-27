using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;
using XCode;

namespace TGG.Core.Entity
{

    /// <summary>
    /// tg_user 业务逻辑类
    /// </summary>
    public partial class tg_user
    {
        /// <summary>用户名称是否存在</summary>
        public static bool GetUserCodeIsExist(string user_code)
        {
            return FindCount(new String[] { _.user_code }, new Object[] { user_code }) > 0;
        }

        /// <summary>玩家名称是否存在</summary>
        public static bool GetPlayerNameExist(string player_name)
        {
            return FindCount(new String[] { _.player_name }, new Object[] { player_name }) > 0;
        }

        /// <summary>创建玩家</summary>
        public static bool GetCreatePlayer(ref tg_user model)
        {
            try
            {
                model.Insert();
                return true;
            }
            catch { return false; }
        }

        /// <summary>初始化玩家</summary>
        /// <param name="user_code">玩家关联账号</param>
        /// <param name="role_id">武将基表id</param>
        /// <param name="sex">性别</param>
        /// <param name="player_name">玩家名称</param>
        /// <param name="birthplace">出生地</param>
        /// <param name="player_vocation">职业</param>
        /// <param name="player_influence">势力</param>
        /// <param name="player_camp">阵营</param>
        /// <param name="scene_id">场景基表id</param>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        public static tg_user InitUser(string user_code, int role_id, int sex, string player_name, int birthplace, int player_vocation, int player_influence, int player_camp, Int64 scene_id, int x, int y,int gold)
        {
            try
            {
                using (var trans = Meta.CreateTrans())
                {
                    var check = FindCount(_.user_code, user_code);
                    if (check > 0) { trans.Commit(); return null; }
                    var user = new tg_user
                    {
                        user_code = user_code,
                        role_id = role_id,
                        player_sex = sex,
                        player_name = player_name,
                        birthplace = birthplace,
                        player_vocation = player_vocation,
                        player_influence = player_influence,
                        player_camp = player_camp,
                        createtime = DateTime.Now.Ticks,
                        //内测
                        gold = gold,
                    };
                    user.Save();
                    var role = tg_role.InitRole(user.id, user.player_vocation, user.role_id);
                    var life_skill = tg_role_life_skill.InitSkill(role.id);
                    var scene = tg_scene.InitScene(scene_id, user.id, x, y);
                    var car = tg_car.InitCar(user.id);
                    var task = tg_task.InitTask(user.id, role.role_identity, user.player_vocation);
                    //if (life_skill == null || scene == null || car || task == null) return null;
                    trans.Commit();
                    return user;
                }
            }
            catch { return null; }
        }

        /// <summary>根据玩家名称集合 查询用户</summary>
        public static tg_user GetFindByName(string name)
        {
            var exp = new WhereExpression();
            exp &= _.player_name == name;
            exp |= _.user_code == name;
            return Find(exp);
        }

        /// <summary>根据玩家名称集合 查询用户</summary>
        public static tg_user GetUserByName(string playname, Int64 oneself)
        {
            var exp = new WhereExpression();
            exp &= _.player_name == playname;
            exp &= _.id != oneself;
            return Find(exp);
        }

        /// <summary>加入人少阵营势力</summary>
        public static int FindCampInfluence()
        {
            var east = FindCount(new String[] { _.player_camp }, new Object[] { (int)CampType.East });
            var west = FindCount(new String[] { _.player_camp }, new Object[] { (int)CampType.West });
            if (east < west)
            {
                var WuTian = FindCount(new String[] { _.player_influence }, new Object[] { (int)InfluenceType.WuTian });
                if (WuTian == 0) return (int)InfluenceType.WuTian;
                var Shangshan = FindCount(new String[] { _.player_influence }, new Object[] { (int)InfluenceType.Shangshan });
                if (Shangshan == 0) return (int)InfluenceType.Shangshan;
                var YiDa = FindCount(new String[] { _.player_influence }, new Object[] { (int)InfluenceType.YiDa });
                if (YiDa == 0) return (int)InfluenceType.YiDa;
                return WuTian < Shangshan
                    ? (int)InfluenceType.WuTian
                    : Shangshan < YiDa ? (int)InfluenceType.Shangshan : (int)InfluenceType.YiDa;
            }

            var ZhiTian = FindCount(new String[] { _.player_influence }, new Object[] { (int)InfluenceType.ZhiTian });
            if (ZhiTian == 0) return (int)InfluenceType.ZhiTian;
            var DaoJin = FindCount(new String[] { _.player_influence }, new Object[] { (int)InfluenceType.DaoJin });
            if (DaoJin == 0) return (int)InfluenceType.DaoJin;
            var DeChuan = FindCount(new String[] { _.player_influence }, new Object[] { (int)InfluenceType.DeChuan });
            if (DeChuan == 0) return (int)InfluenceType.DeChuan;
            return ZhiTian < DaoJin
                    ? (int)InfluenceType.ZhiTian
                    : DaoJin < DeChuan ? (int)InfluenceType.DaoJin : (int)InfluenceType.DeChuan;

        }



    }
}
