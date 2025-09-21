using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Services.Security
{
    public interface ITokenStore
    {
        Task<string?> GetAccessTokenAsync();
        Task<string?> GetRefreshTokenAsync();
        Task SetTokensAsync(string accessToken, string refreshToken);
        Task ClearAsync();
        Task<string> GetOrCreateDeviceIdAsync();
    }
}
