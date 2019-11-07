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
            _userMapToId.Add("a", null);
            _userMapToId.Add("b", null);
            _userMapToId.Add("c", null);
            _userMapToId.Add("d", null);
            _userMapToId.Add("e", null);
            //_userMapToId.Add("f", null);
            //_userMapToId.Add("g", null);
            //_userMapToId.Add("h", null);
            //_userMapToId.Add("i", null);
            //_userMapToId.Add("j", null);
            //_userMapToId.Add("k", null);
            //_userMapToId.Add("l", null);
            //_userMapToId.Add("m", null);
            //_userMapToId.Add("n", null);
            //_userMapToId.Add("o", null);
            //_userMapToId.Add("p", null);
            //_userMapToId.Add("q", null);
            //_userMapToId.Add("r", null);
            //_userMapToId.Add("s", null);
            //_userMapToId.Add("t", null);
            //_userMapToId.Add("x", null);
            //_userMapToId.Add("z", null);
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
                return userlist.Skip(page).Take(count);
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
                if (username == null || _userMapToId.ContainsKey(username))
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
                if (username !=null && _userMapToId.ContainsKey(username))
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
