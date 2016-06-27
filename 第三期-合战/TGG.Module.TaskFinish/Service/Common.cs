namespace TGG.Module.TaskFinish.Service
{
    public partial class Common
    {
        public static Common ObjInstance;

        /// <summary>Common 单体模式</summary>
        public static Common getInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }
    }
}
