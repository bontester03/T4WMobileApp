using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization; // ✅ Add this
using T4sV1.Model.Dashboard;

namespace T4sV1.Services.Http
{
    public interface IDashboardService
    {
        Task<DashboardResponse?> GetDashboardAsync(
            int? activeChildId = null,
            int notificationsTake = 5,
            int activitiesTake = 5,
            CancellationToken ct = default);
    }

    public sealed class DashboardService : IDashboardService
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions _json = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // ✅ Add this
        };

        public DashboardService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("authed");
        }

        public async Task<DashboardResponse?> GetDashboardAsync(
    int? activeChildId = null,
    int notificationsTake = 5,
    int activitiesTake = 5,
    CancellationToken ct = default)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("🔵 DashboardService.GetDashboardAsync START");

                // Build query string
                var queryParams = new List<string>();
                if (activeChildId.HasValue)
                    queryParams.Add($"activeChildId={activeChildId.Value}");
                queryParams.Add($"notificationsTake={notificationsTake}");
                queryParams.Add($"activitiesTake={activitiesTake}");
                var query = string.Join("&", queryParams);
                var url = $"api/ApiDashboard?{query}";

                System.Diagnostics.Debug.WriteLine($"📊 Fetching dashboard: {url}");
                System.Diagnostics.Debug.WriteLine($"🔗 BaseAddress: {_http.BaseAddress}");
                System.Diagnostics.Debug.WriteLine("⏳ About to call GetAsync...");

                var response = await _http.GetAsync(url, ct);

                System.Diagnostics.Debug.WriteLine($"✔️ GetAsync completed with status: {response.StatusCode}");

                // Get the raw response body to debug
                var responseBody = await response.Content.ReadAsStringAsync(ct);
                System.Diagnostics.Debug.WriteLine($"📄 Raw API Response (first 500 chars): {responseBody.Substring(0, Math.Min(500, responseBody.Length))}");

                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Dashboard API error: {response.StatusCode} - {responseBody}");
                    return null;
                }

                // Try to parse JSON
                var dashboard = JsonSerializer.Deserialize<DashboardResponse>(responseBody, _json);
                System.Diagnostics.Debug.WriteLine($"✅ Dashboard loaded: {dashboard?.Children?.Count ?? 0} children");

                return dashboard;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"🚨 Dashboard service error: {ex.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"🚨 Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"🚨 StackTrace: {ex.StackTrace}");
                return null;
            }
        }
    }
}