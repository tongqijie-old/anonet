namespace Anonet.Core
{
    enum NetworkConnectionStatus
    {
        /// <summary>
        /// 无效的状态
        /// </summary>
        None = 0,

        /// <summary>
        /// 初始状态
        /// 根据保存的Peer数据，开始发送心跳命令
        /// </summary>
        Initial = 100,

        /// <summary>
        /// 等待连接状态
        /// 无法正常与Peer通讯，开始借助服务器进行打洞建立连接
        /// </summary>
        Pending = 200,

        /// <summary>
        /// 保持连接状态
        /// 需要发送心跳命令以保持连接
        /// </summary>
        Connected = 300,

        /// <summary>
        /// 挂起连接状态
        /// 不做任何事情，过一段时间后再重试连接
        /// </summary>
        Suspend = 400,

        /// <summary>
        /// 死连接
        /// 长时间无法连接到Peer，考虑放弃这样的连接
        /// </summary>
        Dead = 500,
    }
}
