namespace TGG.Module.Skill.Service
{
    /// <summary>
    /// 部分公共方法
    /// </summary>
    public partial class Common
    {
        private static Common ObjInstance;

        /// <summary>Common 单体模式</summary>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }
    }
}
