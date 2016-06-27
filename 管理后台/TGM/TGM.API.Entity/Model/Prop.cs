using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    public class Prop : BaseEntity
    {
        /// <summary>道具id</summary>
        public string propid { get; set; }

        /// <summary>品质</summary>
        public int grade { get; set; }
    }
}
