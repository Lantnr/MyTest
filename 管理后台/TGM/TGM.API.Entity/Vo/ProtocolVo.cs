using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;

namespace TGM.API.Entity.Vo
{
    /// <summary>
    /// ProtocolVO Socket通讯协议对象，需要和服务端进行类映射
    /// </summary>
    [Serializable]
    public class ProtocolVo : BaseVo
    {
        /// <summary>业务序列号</summary>
        public int serialNumber { get; set; }

        /// <summary>业务验证码(客户端生成，服务端验证)</summary>
        public int verificationCode { get; set; }

        /// <summary>模块号 </summary>
        public int moduleNumber { get; set; }

        /// <summary>命令号</summary>
        public int commandNumber { get; set; }

        /// <summary>服务端时间(服务端返回数据时更新客户端时间)(ms) </summary>
        public double serverTime { get; set; }

        /// <summary>发送指令的时间戳(用于指令超时计算)(ms) </summary>
        public double sendTime { get; set; }

        /// <summary>服务端处理结果状态 </summary>
        public int status { get; set; }

        /// <summary>业务数据(服务端为Map对象) </summary>
        public ASObject data { get; set; }

    }
}
