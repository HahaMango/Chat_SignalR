using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SignalRDemo.Srevice;
using System.Collections.Generic;
using System;
using SignalRDemo.Model;

namespace SignalRDemo.Hubs
{
    /// <summary>
    /// 聊天Hub
    /// </summary>
    public class ChatHub : Hub<IFlashChatClient>
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
        public async Task SendMessage(ChatRecord chatRecord)
        {
            try
            {
                string sender = chatRecord.Sender;
                string receiver = chatRecord.Receiver;
                chatRecord.IsSend = false;

                if (receiver != "广播聊天室")
                {
                    string receiverId = await _accountService.GetConnectIdAsync(receiver);
                    await Clients.Client(receiverId).ReceiveMessage(chatRecord);
                }
                else
                {
                    await Clients.Others.ReceiveMessage(chatRecord);
                }
            }catch(ApplicationException e)
            {
                await Clients.Client(Context.ConnectionId).ReceiveMessage(new ChatRecord
                {
                    Sender = "SYSTEM",
                    Receiver = chatRecord.Receiver,
                    Message = "对方已离线",
                    Date = chatRecord.Date
                });
                _logger.LogWarning($"SignalR获取connectId异常，异常信息{e.Message}");
            }
        }

        /// <summary>
        /// 关联connectId
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task Handshake(string username)
        {
            string connectId = Context.ConnectionId;
            if (username == null || connectId == null)
            {
                _logger.LogError("关联失败，参数为空");
            }

            if (await _accountService.AssociatedWithConnectIdAsync(username, connectId))
            {
                await Clients.Caller.Handshake(1);
                _logger.LogInformation(username + "：关联成功");
            }
            else
            {
                await Clients.Caller.Handshake(-1);
                _logger.LogError(username + "：关联失败");
            }
        }
    }
}
