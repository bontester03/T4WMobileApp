using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization; // ✅ Add this
using T4sV1.Model.Profile;

namespace T4sV1.Services.Http;

public sealed class ProfileService : IProfileService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions _json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public ProfileService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("authed");
    }

    public async Task<ProfileResponseDto?> GetProfileAsync(int? activeChildId = null, CancellationToken ct = default)
    {
        try
        {
            // ✅ Build URL with activeChildId query parameter
            var url = "api/Profile";
            if (activeChildId.HasValue)
            {
                url += $"?activeChildId={activeChildId.Value}";
            }

            System.Diagnostics.Debug.WriteLine($"📊 Fetching profile for child ID: {activeChildId?.ToString() ?? "default"}");

            var response = await _http.GetAsync(url, ct);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(ct);
                System.Diagnostics.Debug.WriteLine($"❌ Profile API error: {response.StatusCode} - {errorBody}");
                return null;
            }

            var responseBody = await response.Content.ReadAsStringAsync(ct);
            System.Diagnostics.Debug.WriteLine($"📄 Profile Response: {responseBody}");

            var profile = JsonSerializer.Deserialize<ProfileResponseDto>(responseBody, _json);

            if (profile?.Child != null)
            {
                System.Diagnostics.Debug.WriteLine($"✅ Profile loaded: {profile.Child.ChildName}, ID: {profile.Child.Id}, Gender: {profile.Child.Gender}, Age: {profile.Child.Age}");
            }

            return profile;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"🚨 Profile service error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"🚨 Stack trace: {ex.StackTrace}");
            return null;
        }
    }

    public async Task<bool> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken ct = default)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== ProfileService.UpdateProfileAsync ===");
            System.Diagnostics.Debug.WriteLine($"📤 Request details:");
            System.Diagnostics.Debug.WriteLine($"   - ChildId: {request.ChildId}");  // ✅ ADD THIS
            System.Diagnostics.Debug.WriteLine($"   - ChildName: {request.Child?.ChildName}");
            System.Diagnostics.Debug.WriteLine($"   - Gender: {request.Child?.Gender}");

            // Serialize to see the actual JSON being sent
            var json = JsonSerializer.Serialize(request, _json);
            System.Diagnostics.Debug.WriteLine($"📦 JSON Body:");
            System.Diagnostics.Debug.WriteLine(json);

            var response = await _http.PutAsJsonAsync("api/Profile", request, _json, ct);

            System.Diagnostics.Debug.WriteLine($"📥 Response: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(ct);
                System.Diagnostics.Debug.WriteLine($"❌ Update profile error: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"   Error body: {errorBody}");
                return false;
            }

            System.Diagnostics.Debug.WriteLine("✅ Profile updated successfully");
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"🚨 Update profile error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"   StackTrace: {ex.StackTrace}");
            return false;
        }
    }

    public async Task<bool> UpdateAvatarAsync(UpdateAvatarDto request, CancellationToken ct = default)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine($"🖼️ Updating avatar to: {request.AvatarUrl}");

            var response = await _http.PutAsJsonAsync("api/Profile/avatar", request, _json, ct);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(ct);
                System.Diagnostics.Debug.WriteLine($"❌ Update avatar error: {response.StatusCode} - {errorBody}");
                return false;
            }

            System.Diagnostics.Debug.WriteLine("✅ Avatar updated successfully");
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"🚨 Update avatar error: {ex.Message}");
            return false;
        }
    }
}