using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;
using TGG.Core.Common.Randoms;

namespace TGG.Module.Duplicate.Service.CHECKPOINT
{
    /// <summary>
    /// 花月茶道进入
    /// </summary>
    public class TOWER_CHECKPOINT_TEA_GAME_ENTER
    {
        private static TOWER_CHECKPOINT_TEA_GAME_ENTER _objInstance;

        /// <summary>TOWER_CHECKPOINT_TEA_GAME_ENTER单体模式</summary>
        public static TOWER_CHECKPOINT_TEA_GAME_ENTER GetInstance()
        {
            return _objInstance ?? (_objInstance = new TOWER_CHECKPOINT_TEA_GAME_ENTER());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "TOWER_CHECKPOINT_TEA_GAME_ENTER", "花月茶道进入");
#endif
                var tower = tg_duplicate_checkpoint.GetEntityByUserId(session.Player.User.id);
                if (tower == null) return Result((int)ResultType.DATABASE_ERROR);      //验证爬塔信息
                if (tower.blood <= 0) return Result((int)ResultType.TOWER_BOOLD_UNENOUGH);   //验证玩家临时血量
                var sgame = Variable.BASE_TOWERGAME.FirstOrDefault(m => m.pass == tower.site);
                if (sgame == null || sgame.type != (int)SamllGameType.TEA)
                    return Result((int)ResultType.TOWER_SITE_ERROR);   //验证关卡是否为茶道游戏
                if (tower.state == (int)DuplicateClearanceType.CLEARANCE_FAIL || tower.state == (int)DuplicateClearanceType.CLEARANCE_SUCCESS)
                    return Result((int)ResultType.TOWER_NO_OPEN);      //验证关卡是否开启

                if (tower.state == (int)DuplicateClearanceType.CLEARANCE_UNBEGIN)    //第一次闯关
                {
                    var photo = Variable.BASE_RULE.FirstOrDefault(m => m.id == "9005");         //图标信息
                    if (photo == null || string.IsNullOrEmpty(photo.value)) return Result((int)ResultType.BASE_TABLE_ERROR);
                    var tealevel = session.Player.Role.LifeSkill.sub_tea_level;
                    return BeginClearance(tower, photo.value, tealevel);
                }

                //中途退出后再次进入
                if (string.IsNullOrEmpty(tower.select_position) || string.IsNullOrEmpty(tower.all_cards))
                    return Result((int)ResultType.DATABASE_ERROR);        //验证茶道翻开卡牌及随机好的卡牌信息
                var position = RecordPosition(tower.select_position);      //获取上一局的牌
                return new ASObject(Common.GetInstance().TeaBuildData((int)ResultType.SUCCESS, tower.npc_tea, tower.user_tea, position));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>开始闯关</summary>
        private ASObject BeginClearance(tg_duplicate_checkpoint tower, string cards, int tlevel)
        {
            var game = Variable.BASE_TOWERGAME.FirstOrDefault(m => m.pass == tower.site);
            if (game == null) return Result((int)ResultType.BASE_TABLE_ERROR);

            tower.npc_tea = game.enemyHp;
            if (tlevel >= 2)           //判断玩家的初始星星数量
            {
                var count = tlevel / 2;
                tower.user_tea = game.myHp + count;
            }
            else
            { tower.user_tea = game.myHp; }

            tower.state = (int)DuplicateClearanceType.CLEARANCE_FIGHTING;
            var photo = InitPosition();
            tower.select_position = ConvertToString(photo);      //初始化记录翻开的图案
            var list = RandomPhoto(cards);
            tower.all_cards = ConvertToString(list);

            if (!tg_duplicate_checkpoint.UpdateSite(tower)) return Result((int)ResultType.DATABASE_ERROR);  //初始化游戏信息

            return new ASObject(Common.GetInstance().TeaBuildData((int)ResultType.SUCCESS, tower.npc_tea, tower.user_tea, photo));
        }

        /// <summary>打乱位置后的图形集合</summary>
        private IEnumerable<int> RandomPhoto(string photo)
        {
            var list = new List<int>();
            if (!photo.Contains("_")) return list;
            var n = photo.Split("_").ToList();
            if (n.Count() < 30) return list;
            list.AddRange(n.Select(item => Convert.ToInt32(item)));
            var number = RNG.Next(0, list.Count - 1, 30);
            return number.Select(item => list[item]).ToList();
        }

        /// <summary>记录上一局的翻牌情况</summary>
        private List<int> RecordPosition(string cards)
        {
            var list = new List<int>();
            if (!cards.Contains("_")) return list;
            var c = cards.Split("_").ToList();
            if (c.Count != 30) return list;
            list.AddRange(c.Select(item => Convert.ToInt32(item)));
            return list;
        }

        /// <summary>初始化图案位置</summary>
        private List<int> InitPosition()
        {
            var list = new List<int>();
            for (var i = 0; i < 30; i++)
            {
                list.Add(0);
            }
            return list;
        }

        /// <summary>将位置转换为字符串</summary>
        private string ConvertToString(IEnumerable<int> position)
        {
            var record = "";
            foreach (var item in position)
            {
                record += Convert.ToString(item) + "_";
            }
            return record;
        }

        private ASObject Result(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }
    }
}
