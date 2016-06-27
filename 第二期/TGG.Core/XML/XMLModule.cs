using System;
namespace TGG.Core.XML
{
    public class XmlModule : ICloneable, IEquatable<XmlModule>
    {
        /// <summary>模块名称</summary>
        public string Name { get; set; }

        /// <summary>模块类型 0:一次性模块,只能加载一次 1:扩展模块可多次加载</summary>
        public int type { get; set; }

        /// <summary>模块dll值</summary>
        public string Value { get; set; }

        /// <summary>模块加载顺序</summary>
        public int Order { get; set; }

        /// <summary>模块计数</summary>
        public int Count { get; set; }

        /// <summary>模块状态</summary>
        public bool State { get; set; }

        /// <summary>id</summary>
        public string id {
            get { return Name + Count; }
        }

        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        /// <summary>
        /// 浅拷贝
        /// </summary>
        public XmlModule CloneEntity()
        {
            return Clone() as XmlModule;
        }

        #endregion


        public bool Equals(XmlModule other)
        {
            if (other == null) return false;
            //var temp = string.Format("{0}{1}{2}", this.Value, this.Order, this.Count);
            //var _temp = string.Format("{0}{1}{2}", other.Value, other.Order, other.Count);
            return (this.id.Equals(other.id));
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            XmlModule objAsPart = obj as XmlModule;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
