using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace SignalRDemo.Srevice
{
    public class AccountService : IAccountService
    {
        private readonly IDictionary<string, string> _userMapToId = null;

        private readonly IDictionary<string, string> _idMapToUser = null;

        public AccountService()
        {
            _userMapToId = new ConcurrentDictionary<string, string>();
            _idMapToUser = new ConcurrentDictionary<string, string>();
        }

        /// <summary>
        /// 返回账户列表
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> AccountListAsync()
        {
            return await Task.Run(() => _userMapToId.Keys);
        }

        /// <summary>
        /// 分页返回账户列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> AccountListAsync(int page, int count)
        {
            return await Task.Run(() => 
            {
                var userlist = _userMapToId.Keys;
                return userlist.Skip(page * count).Take(count);
            });
        }

        /// <summary>
        /// 添加账号
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> AddAccountAsync(string username)
        {
            return await Task.Run(() => 
            {
                if (_userMapToId.ContainsKey(username))
                {
                    return false;
                }
                _userMapToId.Add(username, "");
                return true;
            });
        }

        /// <summary>
        /// 关联账号和SignalR ConnectId
        /// </summary>
        /// <param name="username"></param>
        /// <param name="connectId"></param>
        /// <returns></returns>
        public async Task<bool> AssociatedWithConnectIdAsync(string username, string connectId)
        {
            return await Task.Run(() => 
            {
                if (!_userMapToId.ContainsKey(username) || connectId == null)
                {
                    return false;
                }
                _userMapToId[username] = connectId;
                _idMapToUser[connectId] = username;
                return true;
            });
        }

        /// <summary>
        /// 检查账号是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> ExistAsync(string username)
        {
            return await Task.Run(()=> _userMapToId.ContainsKey(username));
        }

        /// <summary>
        /// 返回特定账号的ConnectId
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<string> GetConnectIdAsync(string username)
        {
            return await Task.Run(()=> 
            {
                if (!_userMapToId.ContainsKey(username))
                {
                    throw new ApplicationException();
                }
                return _userMapToId[username];
            });
        }

        public async Task<string> GetUserNameByConnectId(string connectId)
        {
            return await Task.Run(() => _idMapToUser[connectId]);
        }

        /// <summary>
        /// 删除账号
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAccountAsync(string username)
        {
            return await Task.Run(()=>
            {
                if (_userMapToId.ContainsKey(username))
                {
                    string connectid = _userMapToId[username];
                    _userMapToId.Remove(username);
                    _idMapToUser.Remove(connectid);
                    return true;
                }
                return false;
            });
        }
    }
}
