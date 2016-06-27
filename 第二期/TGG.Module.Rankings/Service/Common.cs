namespace TGG.Module.Rankings.Service
{
    public partial class Common
    {
        private static Common ObjInstance;

        /// <summary>
        /// Common 单体模式
        /// </summary>
        /// <returns></returns>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }
    }
}
