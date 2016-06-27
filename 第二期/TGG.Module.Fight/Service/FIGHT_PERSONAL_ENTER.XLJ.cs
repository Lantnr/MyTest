using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Module.Fight.Service
{
    /// <summary>
    /// 进入战斗
    /// </summary>
    public partial class FIGHT_PERSONAL_ENTER
    {
        /// <summary> 供点将用   多个Npc武将Id抽取5个 </summary>
        /// <param name="list">武将Id集合</param>
        /// <returns></returns>
        private string[] GetMatrixRid(string[] list)
        {
            int count = 5;
            int number = 4;
            var rolehome = Variable.BASE_ROLE_HOME.FirstOrDefault(m => m.id == RoleHomeId);
            if (rolehome != null)
            {
                count = rolehome.count;
                number = count - 1;
            }
            if (list.Count() <= count) return list;
            var numbers = RNG.Next(1, list.Count() - 1, number).ToList();

            ICollection<string> l = new Collection<string>();
            l.Add(list[0]);
            for (int i = 0; i < numbers.Count(); i++)
            {
                l.Add(list[numbers[i]]);
            }
            return l.ToArray();
        }

        #region 设置NPC战斗全局数据

        /// <summary>设置NPC战斗全局数据</summary>
        private bool SetNpcFightData(FightPersonal rival)
        {
            bool flag = false;
            var base_1 = rival.matrix1_rid == 0 ? null : GetNpcData(rival.matrix1_rid);
            if (base_1 != null) { list_role_hp.Add(base_1); flag = true; }
            var base_2 = rival.matrix2_rid == 0 ? null : GetNpcData(rival.matrix2_rid);
            if (base_2 != null) { list_role_hp.Add(base_2); flag = true; }
            var base_3 = rival.matrix3_rid == 0 ? null : GetNpcData(rival.matrix3_rid);
            if (base_3 != null) { list_role_hp.Add(base_3); flag = true; }
            var base_4 = rival.matrix4_rid == 0 ? null : GetNpcData(rival.matrix4_rid);
            if (base_4 != null) { list_role_hp.Add(base_4); flag = true; }
            var base_5 = rival.matrix5_rid == 0 ? null : GetNpcData(rival.matrix5_rid);
            if (base_5 != null) { list_role_hp.Add(base_5); flag = true; }
            return flag;
        }

        /// <summary> 获取NPC FightRole实体 </summary>
        /// <param name="rid">要获取的NPC 实体</param>
        /// <returns>FightRole</returns>
        private FightRole GetNpcData(Int64 rid)
        {
            var model = Variable.BASE_NPCROLE.FirstOrDefault(m => m.id == Convert.ToInt32(rid));
            if (model == null) return null;
            var role = ConvertNpcRoleFight(model);
            role.monsterType = (int)FightRivalType.MONSTER;
            return role;
        }

        #endregion


        /// <summary>设置玩家战斗全局数据</summary>
        private void SetPlayerFightData(List<Int64> ids)
        {
            var list = tg_role.GetFindAllByIds(ids);
            GetPlayerRoleSkill(list);
            list_role_hp.AddRange(ConvertRoleFightList(list));
            list_role_hp.ForEach(item => list_role.Add(item.CloneEntity()));
        }

    }
}
