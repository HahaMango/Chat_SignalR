using SignalRDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo.Hubs
{
    public interface IFlashChatClient
    {
        /// <summary>
        /// 客户端接收信息
        /// </summary>
        /// <param name="chatRecord"></param>
        /// <returns></returns>
        Task ReceiveMessage(ChatRecord chatRecord);

        /// <summary>
        /// 登陆数
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task LoginCount(int count);

        /// <summary>
        /// 通信握手
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task Handshake(int code);
    }
}
