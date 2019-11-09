using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalRDemo.Srevice;

namespace SignalRDemo.Controllers
{
    [Route("/api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService = null;

        private readonly ILogger _logger = null;

        public AccountController(IAccountService accountService,ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<string>> LogInAction([FromForm]string username)
        {
            bool isOk = await _accountService.AddAccountAsync(username);
            if (isOk)
            {
                _logger.LogInformation($"登陆成功：{username}");
                return Ok();
            }
            else
            {
                _logger.LogWarning($"登陆失败：{username}");
                return NotFound();
            }
        }

        [Route("logout")]
        [HttpPost]
        public async Task<ActionResult<string>> LogOutAction([FromForm]string username)
        {
            bool isOk = await _accountService.RemoveAccountAsync(username);
            if (isOk)
            {
                _logger.LogInformation($"登出成功：{username}");
                return Ok();          
            }
            else
            {
                _logger.LogWarning($"登出失败：{username}");
                return NotFound();
            }
        }

        [Route("userlist/{page}/{count}")]
        [HttpGet]
        public async Task<IEnumerable<string>> UserList(int page,int count)
        {
            return await _accountService.AccountListAsync(page, count);
        }

        [Route("debug/usercount")]
        [HttpGet]
        public async Task<int> AccountCount()
        {
            return await _accountService.Count();
        }
    }
}