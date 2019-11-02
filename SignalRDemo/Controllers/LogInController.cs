using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalRDemo.Srevice;

namespace SignalRDemo.Controllers
{
    [ApiController]
    public class LogInController : ControllerBase
    {

        private readonly IAccountService _accountService = null;

        public LogInController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Route("/login")]
        [HttpPost]
        public async Task<ActionResult<string>> LogInAction(string username)
        {
            bool isOk = await _accountService.AddAccountAsync(username);
            if (isOk)
            {
                return "OK";
            }
            else
            {
                return "FAIL";
            }
        }

        [Route("/logout")]
        [HttpPost]
        public async Task<ActionResult<string>> LogOutAction(string username)
        {
            bool isOk = await _accountService.RemoveAccountAsync(username);
            if (isOk)
            {
                return "OK";
            }
            else
            {
                return "FAIL";
            }
        }

        [Route("/userlist")]
        [HttpGet]
        public async Task<IEnumerable<string>> UserList(int page,int count)
        {
            return await _accountService.AccountListAsync(page, count);
        }
    }
}