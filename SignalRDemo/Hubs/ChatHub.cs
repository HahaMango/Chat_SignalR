﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SignalRDemo.Srevice;
using System.Collections.Generic;

namespace SignalRDemo.Hubs
{
    /// <summary>
    /// 聊天Hub
    /// </summary>
    public class ChatHub : Hub
    {
        private readonly ILogger _logger = null;

        private readonly IAccountService _accountService = null;

        public ChatHub(ILogger<ChatHub> logger,IAccountService accountService)
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
        public async Task SendMessage(string username,string message)
        {       
            string senderUserName = await _accountService.GetUserNameByConnectId(Context.ConnectionId);
            if (username != "ALL")
            {
                string receiverConnectId = await _accountService.GetConnectIdAsync(username);
                await Clients.Client(receiverConnectId).SendAsync("ReceiveMessage", senderUserName, message,false);
            }else
            {
                IReadOnlyList<string> sendIds = new List<string>()
                {
                    Context.ConnectionId
                };
                await Clients.AllExcept(sendIds).SendAsync("ReceiveMessage", senderUserName, message,true);
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
            if(username == null || connectId == null)
            {
                _logger.LogInformation("关联失败，参数为空");
            }

            if (await _accountService.AssociatedWithConnectIdAsync(username, connectId))
            {
                _logger.LogInformation(username + "：关联成功");
            }
            else
            {
                _logger.LogInformation(username + "：关联失败");
            }
        }
    }
}
