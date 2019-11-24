using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SignalRDemo.Srevice;
using System.Collections.Generic;
using System;

namespace SignalRDemo.Hubs
{
    /// <summary>
    /// 聊天Hub
    /// </summary>
    public class ChatHub : Hub
    {
        private readonly ILogger _logger = null;

        private readonly IAccountService _accountService = null;

        public ChatHub(ILogger<ChatHub> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string sender,string receiver, string message)
        {
            try
            {
                string senderId = null;
                string receiverId = null;
                senderId = await _accountService.GetConnectIdAsync(sender);
                receiverId = await _accountService.GetConnectIdAsync(receiver);

                if (receiver != "广播聊天室")
                {
                    await Clients.Client(receiverId).SendAsync("ReceiveMessage",sender, receiver, message);
                }
                else
                {
                    IReadOnlyList<string> sendIds = new List<string>()
                    {
                        senderId
                    };
                    await Clients.AllExcept(sendIds).SendAsync("ReceiveMessage", sender, receiver, message);
                }
            }catch(ApplicationException e)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", "SYSTEM", receiver,"对方以离线");
                _logger.LogWarning($"SignalR获取connectId异常，异常信息{e.Message}");
            }
        }

        /// <summary>
        /// 关联connectId
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task AssociatedConnectId(string username)
        {
            string connectId = Context.ConnectionId;
            if (username == null || connectId == null)
            {
                _logger.LogError("关联失败，参数为空");
            }

            if (await _accountService.AssociatedWithConnectIdAsync(username, connectId))
            {
                _logger.LogInformation(username + "：关联成功");
            }
            else
            {
                _logger.LogError(username + "：关联失败");
            }
        }
    }
}
