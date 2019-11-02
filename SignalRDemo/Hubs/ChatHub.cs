using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SignalRDemo.Srevice;

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
            string receiverConnectId = await _accountService.GetConnectIdAsync(username);
            string senderUserName = await _accountService.GetUserNameByConnectId(Context.ConnectionId);
            await Clients.Client(receiverConnectId).SendAsync("ReceiveMessage",senderUserName,message);
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
