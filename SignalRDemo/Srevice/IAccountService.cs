using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRDemo.Srevice
{
    public interface IAccountService
    {
        Task<bool> AddAccountAsync(string username);
        Task<bool> RemoveAccountAsync(string username);
        Task<bool> ExistAsync(string username);
        Task<bool> AssociatedWithConnectIdAsync(string username, string connectId);
        Task<IEnumerable<string>> AccountListAsync();
        Task<IEnumerable<string>> AccountListAsync(int page, int count);
        Task<string> GetConnectIdAsync(string username);
        Task<string> GetUserNameByConnectId(string connectId);
        Task<int> Count();
    }
}
