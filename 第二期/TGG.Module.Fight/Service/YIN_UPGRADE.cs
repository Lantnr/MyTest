using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Fight.Service
{
    /// <summary>
    /// 印升级
    /// </summary>
    public class YIN_UPGRADE
    {
        public static YIN_UPGRADE ObjInstance;

        /// <summary>
        /// YIN_UPGRADE单体模式
        /// </summary>
        /// <returns></returns>
        public static YIN_UPGRADE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new YIN_UPGRADE());
        }

        /// <summary> 印升级 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var id = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);     //id:[double] 印id主键
            var user = session.Player.User.CloneEntity();
            var yin = tg_fight_yin.FindByid(id);

            if (yin.yin_level >= 10) return new ASObject(BuildData((int)ResultType.FIGHT_YIN_LVMAX, yin)); //印等级验证    
            yin.yin_level = yin.yin_level + 1;
            var base_yin = Variable.BASE_YIN.FirstOrDefault(m => m.id == yin.yin_id);
            if (base_yin == null) return new ASObject(BuildData((int)ResultType.BASE_TABLE_ERROR, null));//印基表数据验证
            var base_effect = Variable.BASE_YINEFFECT.FirstOrDefault(m => m.yinId == base_yin.id && m.level == yin.yin_level);//读取印效果表
            if (base_effect == null) return new ASObject(BuildData((int)ResultType.BASE_TABLE_ERROR, null));

            var count = base_effect.spiritFormula;
            var spirit = user.spirit;
            user.spirit = user.spirit - count;
            if (user.spirit < 0) return new ASObject(BuildData((int)ResultType.BASE_PLAYER_SPIRIT_ERROR, yin)); //玩家魂数验证

            yin.Update();
            user.Update();
            session.Player.User = user;

            //记录消耗魂日志 lzh
            var logdata = string.Format("{0}_{1}_{2}_{3}", "SpiritUse", spirit, count, user.spirit);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.FIGHT, (int)FightCommand.YIN_UPGRADE, logdata);

            Common.GetInstance().RewardsInfoToUser(session, user, (int)GoodsType.TYPE_SPIRIT); //向用户推送更新
            return new ASObject(BuildData((int)ResultType.SUCCESS, yin));
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result, tg_fight_yin yin)
        {
            var dic = new Dictionary<string, object>
            { 
            { "result", result },
            { "yin",yin==null?null: EntityToVo.ToFightYinVo(yin) } ,
            };
            return dic;
        }
    }
}
