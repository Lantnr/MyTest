using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using FluorineFx.Messaging.Rtmp.SO;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Props.Service
{
    /// <summary>
    /// 流派技能书使用
    /// </summary>
    public class PROP_GENRE_USE
    {
        private static PROP_GENRE_USE _objInstance;
        // private int result;

        /// <summary>PROP_GENRE_USE 单体模式</summary>
        public static PROP_GENRE_USE GetInstance()
        {
            return _objInstance ?? (_objInstance = new PROP_GENRE_USE());
        }
        /// <summary>流派技能书使用</summary>

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "PROP_GENRE_USE", "流派技能书使用");
#endif
            var rid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "rid").Value);
            var id = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);
            var userid = session.Player.User.id;
            var role = tg_role.GetEntityById(rid);
            var prop = tg_bag.GetEntityById(id);
            if (role == null || prop == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.FRONT_DATA_ERROR));
            if (prop.count < 1) return new ASObject(Common.GetInstance().BuildData((int)ResultType.PROP_LACK));//数量的判断

            var baseProp = Common.GetInstance().GetBaseProp(prop.base_id); //道具基表数据  
            var result = IsBaseData(baseProp, role.role_level);
            if (result != ResultType.SUCCESS) return new ASObject(Common.GetInstance().BuildData((int)result));

            var fskills = tg_role_fight_skill.GetRoleSkillByRid(rid);       //已学战斗技能信息
            var genres = (new Share.Role()).LearnGenreOrNinja(fskills, role.role_genre, role.role_ninja);
            if (genres.Contains(baseProp.genre)) return new ASObject(Common.GetInstance().BuildData((int)ResultType.PROP_GENRE_OPENED));

            var model = GetFightSkillFirst(baseProp.genre);
            if (model == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.BASE_TABLE_ERROR));
            var skill = BuildSkill(role.id, baseProp.genre, model);//组装流派战斗技能

            prop.count--;                    //扣除道具
            skill.Insert();                  //插入数据库
            genres.Add(baseProp.genre); //增加流派

            (new Bag()).BuildReward(userid, new List<tg_bag>() { prop });
            return new ASObject(BuildData((int)ResultType.SUCCESS, role.id, genres));
        }

        /// <summary> 获取流派战斗技能基表第一条数据 </summary>
        /// <param name="genre">流派</param>
        private BaseFightSkill GetFightSkillFirst(int genre)
        {
            var list = Variable.BASE_FIGHTSKILL.Where(m => m.genre == genre && m.type != (int)RolePersonalSkillType.MYSTERY).ToList();
            return !list.Any() ? null : list.First();
        }

        /// <summary> 验证道具基表 </summary>
        /// <param name="baseprop">基表数据</param>
        /// <param name="level">武将等级</param>
        private ResultType IsBaseData(BaseProp baseprop, int level)
        {
            if (baseprop == null) return ResultType.DATABASE_ERROR;
            if (baseprop.useMode != 1) return ResultType.PROP_UNUSED;
            return baseprop.useLevel > level ? ResultType.BASE_PLAYER_LEVEL_ERROR : ResultType.SUCCESS;
        }

        /// <summary> 组装数据 </summary>
        /// <param name="result">处理结果</param>
        /// <param name="id">武将Id</param>
        /// <param name="list">流派技能集合</param>
        private Dictionary<string, object> BuildData(int result, Int64 id, List<int> list)
        {
            var dic = new Dictionary<string, object> { { "id", id }, { "genreTypeArr", list } };
            return new Dictionary<string, object> { { "result", result }, { "data", new ASObject(dic) } };
        }

        /// <summary> 组装流派技能 </summary>
        /// <param name="rid">武将ID</param>
        /// <param name="model">战斗技能基表</param>
        /// <param name="genre">流派</param>
        private tg_role_fight_skill BuildSkill(Int64 rid, int genre, BaseFightSkill model)
        {
            return new tg_role_fight_skill()
            {
                rid = rid,
                skill_id = model.id,
                skill_type = model.type,
                skill_genre = genre,
                type_sub = model.typeSub,
                skill_level = -1,
                skill_time = 0,
            };
        }
    }
}
