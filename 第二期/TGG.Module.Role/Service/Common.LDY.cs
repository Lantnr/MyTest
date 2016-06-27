using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using FluorineFx.Context;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.Share;
using TGG.SocketServer;
using Convert = FluorineFx.Util.Convert;

namespace TGG.Module.Role.Service
{
    public partial class Common
    {

        /// <summary>组装更新数据 </summary>
        public ASObject BuildUpdateRoleData(tg_role role, IEnumerable<string> name)
        {
            var dic = new Dictionary<string, object> { { "id", role.id } };
            var dic1 = new Dictionary<string, object>();
            foreach (var item in name)
            {
                var _item = item.ToLower();

                switch (_item)
                {
                    case "power":  //总体力
                        {
                            dic.Add("power", tg_role.GetTotalPower(role));
                            break;
                        }
                    case "rolepower"://基础体力
                        {
                            dic.Add("rolePower", role.power);
                            break;
                        }
                    case "experience":  //经验
                        {
                            dic.Add("experience", role.role_exp);
                            break;
                        }
                    case "identityid":  //身份
                        {
                            dic.Add("identityId", role.role_identity);
                            break;
                        }
                    case "honor":  //功勋
                        {
                            dic.Add("honor", role.role_honor);
                            break;
                        }
                    case "level":  //等级
                        {
                            dic.Add("level", role.role_level);
                            break;
                        }
                    case "genre":  //流派
                        {
                            dic.Add("genre", role.role_genre);
                            break;
                        }
                    case "ninja":  //忍者众
                        {
                            dic.Add("ninja", role.role_ninja);
                            break;
                        }
                    case "captain":   //统率
                        {
                            dic.Add("captain", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_CAPTAIN, role), 2));
                            break;
                        }
                    case "captainbase":  //基础统率
                        {
                            dic.Add("captainBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_CAPTAIN, role), 2));
                            break;
                        }
                    case "force":   //武力
                        {
                            dic.Add("force", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, role), 2));
                            break;
                        }
                    case "forcebase":  //基础武力
                        {
                            dic.Add("forceBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_FORCE, role), 2));
                            break;
                        }
                    case "brains":   //智谋
                        {
                            dic.Add("brains", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_BRAINS, role), 2));
                            break;
                        }
                    case "brainbase":  //基础智谋
                        {
                            dic.Add("brainsBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_BRAINS, role), 2));
                            break;
                        }
                    case "charm":   //魅力
                        {
                            dic.Add("charm", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, role), 2));
                            break;
                        }
                    case "charmbase":  //基础魅力
                        {
                            dic.Add("charmBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_CHARM, role), 2));
                            break;
                        }
                    case "govern":  //政务
                        {
                            dic.Add("govern", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, role), 2));
                            break;
                        }
                    case "governbase":  //基础政务
                        {
                            dic.Add("governBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_GOVERN, role), 2));
                            break;
                        }
                    case "attack":    //攻击
                        {
                            dic.Add("attack", role.att_attack);
                            break;
                        }
                    case "critprobability":   //会心几率
                        {
                            dic.Add("critProbability", Math.Round(role.att_crit_probability, 2));
                            break;
                        }
                    case "critaddition":    //会心效果
                        {
                            dic.Add("critAddition", Math.Round(role.att_crit_addition, 2));
                            break;
                        }
                    case "dodgeprobability":    //闪避几率
                        {
                            dic.Add("dodgeProbability", Math.Round(role.att_dodge_probability, 2));
                            break;
                        }
                    case "mysteryprobability":   //奥义触发秘技
                        {
                            dic.Add("mysteryProbability", Math.Round(role.att_mystery_probability, 2));
                            break;
                        }
                    case "life": //血量
                        {
                            dic.Add("life", role.att_life);
                            break;
                        }
                    default:
                        {
                            return new ASObject();
                        }
                }
            }
            dic1.Add("data", new ASObject(dic));
            return new ASObject(dic1);
        }

    

    }
}
