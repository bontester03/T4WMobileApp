using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace T4sV1.Services.Security
{
    public sealed class TokenStore : ITokenStore
    {
        const string AccessKey = "auth.access";
        const string RefreshKey = "auth.refresh";
        const string DeviceKey = "auth.deviceId";

        public async Task<string?> GetAccessTokenAsync() =>
            await SecureStorage.GetAsync(AccessKey);

        public async Task<string?> GetRefreshTokenAsync() =>
            await SecureStorage.GetAsync(RefreshKey);

        public async Task SetTokensAsync(string accessToken, string refreshToken)
        {
            await SecureStorage.SetAsync(AccessKey, accessToken);
            await SecureStorage.SetAsync(RefreshKey, refreshToken);
        }

        public Task ClearAsync()
        {
            SecureStorage.Default.Remove(AccessKey);
            SecureStorage.Default.Remove(RefreshKey);
            return Task.CompletedTask;
        }


        public async Task<string> GetOrCreateDeviceIdAsync()
        {
            var existing = Preferences.Get(DeviceKey, null);
            if (!string.IsNullOrWhiteSpace(existing)) return existing!;
            var id = Guid.NewGuid().ToString("N");
            Preferences.Set(DeviceKey, id);
            return id;
        }
    }
}
