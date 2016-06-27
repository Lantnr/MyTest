using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Duplicate;
using TGG.Core.Vo.User;
using TGG.SocketServer;

namespace TGG.Module.Duplicate.Service.CHECKPOINT
{
    /// <summary>
    /// 点击下一关
    /// </summary>
    public class TOWER_CHECKPOINT_NEXT_PASS
    {
        private static TOWER_CHECKPOINT_NEXT_PASS ObjInstance;

        /// <summary>TOWER_CHECKPOINT_NEXT_PASS单体模式</summary>
        public static TOWER_CHECKPOINT_NEXT_PASS GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TOWER_CHECKPOINT_NEXT_PASS());
        }

        /// <summary>点击下一关</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "TOWER_CHECKPOINT_NEXT_PASS", "点击下一关");
#endif
            var checkpoint = tg_duplicate_checkpoint.GetEntityByUserId(session.Player.User.id);
            var towervo = new TowerPassVo();  
            if(checkpoint==null)
                return new ASObject(Common.GetInstance().BulidData((int)ResultType.DATABASE_ERROR, 0, null, null));
            if(checkpoint.state==(int)DuplicateClearanceType.CLEARANCE_FAIL||checkpoint.blood<=0)
                return new ASObject(Common.GetInstance().BulidData((int)ResultType.CHALLENGE_FAIL, 0, null, null));

            var towerpass = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == checkpoint.site);                    
            if (towerpass == null)
                return new ASObject(Common.GetInstance().BulidData((int)ResultType.BASE_TABLE_ERROR, 0, null, null));

            if (towerpass.watchmen == (int)DuplicateTargetType.WATCHMEM &&
                checkpoint.state == (int)DuplicateClearanceType.CLEARANCE_SUCCESS)
            {
                towervo = TowerPassMessage(session.Player.UserExtend.npc_refresh_count, ref checkpoint);
                checkpoint.dekaron = (int) DuplicateRightType.HAVERIGHT;
               Common.GetInstance(). CheckpointUpdate(checkpoint);
            }
            else
            {
                return new ASObject(Common.GetInstance().BulidData((int)ResultType.CHALLENGE_FAIL, 0, null, null)); 
            }
            return new ASObject(Common.GetInstance().BulidData((int)ResultType.SUCCESS, 
                session.Player.UserExtend.challenge_count, towervo, null));
        }

        /// <summary>下一关卡信息</summary>
        private TowerPassVo TowerPassMessage(int count, ref tg_duplicate_checkpoint checkpoint)//, ref UserInfoVo uservo)
        {
            return Common.GetInstance().TowerPassMessage(count, ref checkpoint);//, ref uservo);
        }
    }
}
