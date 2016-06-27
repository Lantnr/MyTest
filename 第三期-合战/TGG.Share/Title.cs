using System;
using System.Linq;
using TGG.Core.Base;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Share
{
    public partial class Title : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region 判断称号获取方法
        /// <summary>战斗使用刀剑  枪获得称号方法</summary>
        /// <param name="userid">用户userid</param>
        public void RoleFightMethods(Int64 userid)
        {
            var b = Variable.OnlinePlayer.ContainsKey(userid);         //获取玩家session
            if (!b) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;

            var role = session.Player.Role.Kind;
            if (role.equip_weapon == 0) return;
            var equip = tg_bag.GetEntityByEquipId(role.equip_weapon);
            if (equip == null) return;
            var bequip = Variable.BASE_EQUIP.FirstOrDefault(m => m.id == equip.base_id);
            if (bequip == null) return;
            var type = bequip.typeSmall;
            switch (type)
            {
                case 1:
                    IsTitleAcquire(session, (int)TitleGetType.USE_SWORD); break;        //剑
                case 2:
                    IsTitleAcquire(session, (int)TitleGetType.USE_GUN); break;             //枪
            }
        }

        /// <summary>判断称号是否达到--------（武将宅茶道  顿悟）</summary>
        /// <param name="userid">用户userid</param>
        /// <param name="type">获取方式</param>
        public void IsTitleAcquire(Int64 userid, int type)
        {
            var b = Variable.OnlinePlayer.ContainsKey(userid);         //获取玩家session
            if (!b) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;

            var extend = session.Player.UserExtend.CloneEntity();
            switch (type)
            {
                case (int)TitleGetType.USE_SWORD: extend.sword_win_count += 1; break;                        //剑
                case (int)TitleGetType.USE_GUN: extend.gun_win_count += 1; break;                                //枪
                case (int)TitleGetType.USE_TEA_TABLE: extend.tea_table_count += 1; break;                      //茶席
                case (int)TitleGetType.BARGARN_SUCCUSS: extend.bargain_success_count += 1; break;   //讲价成功
            }
            if (!tg_user_extend.GetUpdate(extend)) return;
            session.Player.UserExtend = extend;           //更新用户session扩展表信息
            IsGetTitle(extend, type);
        }

        /// <summary>判断称号是否达到-----（跑商讲价）</summary>
        /// <param name="extend">用户拓展信息</param>
        /// <param name="type">获取方式</param>
        public void IsTitleAcquire(tg_user_extend extend, int type)
        {
            switch (type)
            {
                case (int)TitleGetType.USE_SWORD: extend.sword_win_count += 1; break;                        //剑
                case (int)TitleGetType.USE_GUN: extend.gun_win_count += 1; break;                                //枪
                case (int)TitleGetType.USE_TEA_TABLE: extend.tea_table_count += 1; break;                      //茶席
                case (int)TitleGetType.BARGARN_SUCCUSS: extend.bargain_success_count += 1; break;   //讲价成功
            }
            if (!tg_user_extend.GetUpdate(extend)) return;
            IsGetTitle(extend, type);
        }

        /// <summary>判断称号是否达到</summary>
        /// <param name="session">用户session</param>
        /// <param name="type">获取方式</param>
        private void IsTitleAcquire(TGGSession session, int type)
        {
            var extend = session.Player.UserExtend.CloneEntity();
            switch (type)
            {
                case (int)TitleGetType.USE_SWORD: extend.sword_win_count += 1; break;                        //剑
                case (int)TitleGetType.USE_GUN: extend.gun_win_count += 1; break;                                //枪
                case (int)TitleGetType.USE_TEA_TABLE: extend.tea_table_count += 1; break;                      //茶席
                case (int)TitleGetType.BARGARN_SUCCUSS: extend.bargain_success_count += 1; break;   //讲价成功
            }
            if (!tg_user_extend.GetUpdate(extend)) return;
            session.Player.UserExtend = extend;           //更新用户session扩展表信息
            IsGetTitle(extend, type);
        }

        /// <summary>判断称号是否达成</summary>
        private void IsGetTitle(tg_user_extend extend, int type)
        {
            switch (type)
            {
                case (int)TitleGetType.USE_SWORD: JudgeCount(extend, type, extend.sword_win_count); break;
                case (int)TitleGetType.USE_GUN: JudgeCount(extend, type, extend.gun_win_count); break;
                case (int)TitleGetType.USE_TEA_TABLE: JudgeCount(extend, type, extend.tea_table_count); break;
                case (int)TitleGetType.BARGARN_SUCCUSS: JudgeCount(extend, type, extend.bargain_success_count); break;
            }
        }

        /// <summary>判断次数</summary>
        private void JudgeCount(tg_user_extend extend, int type, int count)
        {
            var list = Variable.BASE_ROLETITLE.Where(m => m.methods == type && m.count <= count).ToList();
            if (!list.Any()) return;
            foreach (var item in list)
            {
                UpdateTitle(extend.user_id, item);
            }
        }

        /// <summary>验证处理称号信息</summary>
        private void UpdateTitle(Int64 userid, BaseRoleTitle basetitle)
        {
            var title = tg_role_title.GetTitleByUseridTid(userid, basetitle.id);
            if (title == null)
            {
                CreateTitle(basetitle, userid);
            }
            else if (title.title_state == (int)TitleState.NOT_REACHED)
            {
                title.title_state = (int)TitleState.HAS_BEEN_REACHED;
                title.Update();
            }
        }

        /// <summary>创建称号信息</summary>
        private void CreateTitle(BaseRoleTitle btitle, Int64 userid)
        {
            var title = new tg_role_title()
            {
                title_id = btitle.id,
                title_state = (int)TitleState.HAS_BEEN_REACHED,
                title_load_state = (int)LoadStateType.UNLOAD,
                title_count = 1,
                packet_role1 = 0,
                packet_role2 = 0,
                packet_role3 = 0,
                user_id = userid,
            };
            tg_role_title.Insert(title);
        }
        #endregion
    }
}
