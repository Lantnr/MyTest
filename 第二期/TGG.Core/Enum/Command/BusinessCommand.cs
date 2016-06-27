using System;

namespace TGG.Core.Enum.Command
{
    /// <summary>
    /// BusinessCommon 跑商相关命令
    /// </summary>
    public enum BusinessCommand
    {
        #region 跑商指令

        /// <summary>
        /// 进入跑商系统
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// car:[List[BusinessCarVo]] 当前玩家所有马车
        /// </summary>
        BUSINESS_JOIN = 1,

        /// <summary>
        /// 市价一览
        /// 前端传递数据:
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// goods:[ASObject] 购买町数据集
        /// ASObject 购买町数据集{key = [町id],value = [BusinessGoodsVo 数组]}
        /// </summary>
        BUSINESS_PRICE_VIEW = 2,

        /// <summary>
        /// 进入车库
        /// 前端传递数据:
        /// carId:[double] 马车 vo.id 
        /// type:int 0:选择武将 1:卸下武将
        /// generalId:[double] 武将 vo.id，此字段选择武将时有效
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        BUSINESS_ROLE_SELECT = 3,

        /// <summary>
        /// 买/卖
        /// 前端传递数据:
        /// id:[int] 町基础 id
        /// type:[int] 0:购买，1:出售
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// goods:[List[GoodsVo]] 货物集合
        /// </summary>
        BUSINESS_GOODS_BUY_ENTER = 4,

        /// <summary>
        /// 价格锁定
        /// 前端传递数据:
        /// type:[int] 0:购买，1:出售
        /// carId:[double] 马车 vo.id
        /// goodsId:[double] 货物 vo.id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// goods:[BusinessGoodsVo] 货物 vo
        /// count:[int] 剩余讲价次数
        /// </summary>
        BUSINESS_PRICE_LOCK = 5,

        /// <summary>
        /// 讲价
        /// 前端传递数据:
        /// type:[int] 0:买 1:卖
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// price:[int] 讲价后价格
        /// </summary>
        BUSINESS_GOODS_PRICE = 6,

        /// <summary>
        /// 交易
        /// 前端传递数据:
        /// carId:[double] 马车主键id
        /// count:int 交易数量
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// goods:[BusinessGoodsVo] 货物 vo
        /// </summary>
        BUSINESS_GOODS_BUY = 7,

        /// <summary>
        /// 购买马车格子
        /// 前端传递数据:
        /// carId:[double] 马车主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// count:[int] 马车当前总开启格子数
        /// </summary>
        BUSINESS_PACKET_BUY = 8,

        /// <summary>
        /// 跑商加速
        /// 前端传递数据:
        /// carId:[double] 马车 vo.id 
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// carVo:BusinessCarVo 马车 vo
        /// </summary>
        BUSINESS_ACCELERATE = 9,

        /// <summary>
        /// 跑商出发
        /// 前端传递数据:
        /// carId [int] 跑商马车Id
        /// idList: [List[int]] 町基础 id 数组
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// carVo:BusinessCarVo 马车 vo
        /// </summary>
        BUSINESS_START = 10,

        /// <summary>
        /// 调查
        /// 前端传递数据:
        /// id:[List[int]] 町基础 id  集合 
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        BUSINESS_PRICE_INFO = 11,

        /// <summary>
        /// 进入町
        /// 前端传递数据:
        /// id:[int] 町基础 id
        /// carId:[double] 马车主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        BUSINESS_TING_ENTER = 12,

        /// <summary>
        /// 市价情报
        /// 前端传递数据;
        /// id:[int] 商圈 id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// id:[List[int]] 已调查町Id集合
        /// </summary>
        BUSINESS_PRICE_QUERY = 13,


        /// <summary>
        /// 跑商结束
        /// 服务器响应数据:
        ///carVo:[BusinessCarVo] 马车 vo
        /// </summary>
        BUSINESS_END = 1001,
        #endregion

        #region VIP 指令

        /// <summary>
        /// 购买马车
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// carVo:[BusinessCarVo] 马车 vo
        /// </summary>
        BUSINESS_BUY_CAR = 14,


        /// <summary>
        /// 购买的议价次数
        /// 前端数据
        /// count:[int] 购买次数
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// count:[int] 剩余讲价次数
        /// </summary>
        BUSINESS_BUY_BARGAIN = 15,

        /// <summary>
        /// 补充货物
        /// 前端提交数据
        /// id:[Double] 货物主键id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// goods:[BusinessGoodsVo] 货物 vo
        /// </summary>
        BUSINESS_GOODS_ADD = 16,

        /// <summary>
        /// 开启商圈
        /// 前端提交数据
        /// id:[int] 町基础 id
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// area:[int] 开启商圈基表编号
        /// </summary>
        BUSINESS_AREA_OPEN = 17,


        /// <summary>
        /// 免税
        /// 服务器响应数据:
        /// result:[int] 返回状态值
        /// </summary>
        BUSINESS_FREE_TAX = 18,

        #endregion



    }
}
