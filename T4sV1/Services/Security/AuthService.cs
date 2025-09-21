using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using T4sV1.Model.Auth;
using T4sV1.Services.Security;

namespace T4sV1.Services.Security
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(string email, string password, CancellationToken ct = default);
        Task<bool> RefreshAsync(CancellationToken ct = default);
        Task LogoutAsync(bool allDevices = false, CancellationToken ct = default);
    }

    public sealed class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ITokenStore _store;

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public AuthService(HttpClient http, ITokenStore store)
        {
            _http = http;
            _store = store;
        }

        public async Task<LoginResponseDto> LoginAsync(string email, string password, CancellationToken ct = default)
        {
            var deviceId = await _store.GetOrCreateDeviceIdAsync();
            _http.DefaultRequestHeaders.Remove("X-Device-Id");
            _http.DefaultRequestHeaders.Add("X-Device-Id", deviceId);

            var payload = new LoginRequestDto { Email = email, Password = password };
            var json = JsonSerializer.Serialize(payload, JsonOpts);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var resp = await _http.PostAsync("api/auth/login", content, ct);
            var body = await resp.Content.ReadAsStringAsync(ct);
            resp.EnsureSuccessStatusCode();

            var dto = JsonSerializer.Deserialize<LoginResponseDto>(body, JsonOpts)!;
            await _store.SetTokensAsync(dto.AccessToken, dto.RefreshToken);

            return dto;
        }

        public async Task<bool> RefreshAsync(CancellationToken ct = default)
        {
            var refresh = await _store.GetRefreshTokenAsync();
            if (string.IsNullOrWhiteSpace(refresh)) return false;

            var deviceId = await _store.GetOrCreateDeviceIdAsync();
            _http.DefaultRequestHeaders.Remove("X-Device-Id");
            _http.DefaultRequestHeaders.Add("X-Device-Id", deviceId);

            var payload = new RefreshTokenRequest { RefreshToken = refresh! };
            var json = JsonSerializer.Serialize(payload, JsonOpts);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var resp = await _http.PostAsync("api/auth/refresh-token", content, ct);
            if (!resp.IsSuccessStatusCode) return false;

            var body = await resp.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(body);
            var accessToken = doc.RootElement.GetProperty("accessToken").GetString()!;
            var newRefresh = doc.RootElement.GetProperty("refreshToken").GetString()!;
            await _store.SetTokensAsync(accessToken, newRefresh);
            return true;
        }

        public async Task LogoutAsync(bool allDevices = false, CancellationToken ct = default)
        {
            var at = await _store.GetAccessTokenAsync();
            if (string.IsNullOrWhiteSpace(at)) { await _store.ClearAsync(); return; }

            var deviceId = await _store.GetOrCreateDeviceIdAsync();
            _http.DefaultRequestHeaders.Remove("X-Device-Id");
            _http.DefaultRequestHeaders.Add("X-Device-Id", deviceId);
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", at);

            using var resp = await _http.PostAsync($"api/auth/logout?allDevices={allDevices.ToString().ToLowerInvariant()}", null, ct);
            // ignore errors for logout
            await _store.ClearAsync();
        }
    }
}
