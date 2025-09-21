// Services/ChildrenService.cs
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using T4sV1.Model.Children;
using T4sV1.Services.Security;

namespace T4sV1.Services;

public sealed class ChildrenService : IChildrenService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions _json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() } // 👈 accepts "Male"/"Female"/"Other" etc.
    };

    public ChildrenService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("authed"); // uses BearerAuthHandler + base address
    }

    public async Task<IReadOnlyList<ChildDto>> ListAsync(bool includeDeleted = false, CancellationToken ct = default)
    {
        var url = $"api/children?includeDeleted={includeDeleted.ToString().ToLowerInvariant()}";
        using var resp = await _http.GetAsync(url, ct);

        var body = await resp.Content.ReadAsStringAsync(ct);

        if (!resp.IsSuccessStatusCode)
            throw new HttpRequestException(
                $"GET {url} => {(int)resp.StatusCode} {resp.ReasonPhrase}. Body (first 200): {Trim(body, 200)}");

        if (LooksLikeHtml(body))
            throw new InvalidOperationException(
                $"GET {url} returned HTML (likely a redirect/unauthorized). " +
                $"Ensure JWT is sent and API isn’t redirecting to a login page.");

        return JsonSerializer.Deserialize<IReadOnlyList<ChildDto>>(body, _json) ?? Array.Empty<ChildDto>();

        static bool LooksLikeHtml(string s)
            => !string.IsNullOrEmpty(s) && s.TrimStart().StartsWith("<", StringComparison.Ordinal);

        static string Trim(string s, int n) => s.Length <= n ? s : s.Substring(0, n) + "...";
    }


    public async Task<ChildDto?> GetAsync(int id, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<ChildDto>($"api/children/{id}", _json, ct);

    public async Task<ChildDto> CreateAsync(CreateChildRequest dto, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync("api/children", dto, _json, ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<ChildDto>(_json, ct))!;
    }

    public async Task UpdateAsync(int id, UpdateChildRequest dto, CancellationToken ct = default)
    {
        var resp = await _http.PutAsJsonAsync($"api/children/{id}", dto, _json, ct);
        resp.EnsureSuccessStatusCode();
    }

    public async Task UpdateAvatarAsync(int id, string avatarUrl, CancellationToken ct = default)
    {
        var resp = await _http.PatchAsync(
            $"api/children/{id}/avatar",
            new StringContent(JsonSerializer.Serialize(new UpdateAvatarRequest { AvatarUrl = avatarUrl }, _json), Encoding.UTF8, "application/json"),
            ct);
        resp.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var resp = await _http.DeleteAsync($"api/children/{id}", ct);
        resp.EnsureSuccessStatusCode();
    }

    public async Task<IReadOnlyList<string>> GetAvatarUrlsAsync(CancellationToken ct = default)
        => await _http.GetFromJsonAsync<IReadOnlyList<string>>("api/children/avatars", _json, ct)
           ?? Array.Empty<string>();
}
