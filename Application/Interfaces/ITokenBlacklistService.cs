using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITokenBlacklistService
    {
        Task AddTokenToBlacklist(string token);
        Task<bool> IsTokenBlacklisted(string token);
        Task RemoveTokenFromBlacklist(string token);
    }

    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly HashSet<string> _blacklist = new HashSet<string>();

        public Task AddTokenToBlacklist(string token)
        {
            _blacklist.Add(token);
            return Task.CompletedTask;
        }

        public Task<bool> IsTokenBlacklisted(string token)
        {
            return Task.FromResult(_blacklist.Contains(token));
        }

        public Task RemoveTokenFromBlacklist(string token)
        {
            _blacklist.Remove(token);
            return Task.CompletedTask;
        }
    }
}
