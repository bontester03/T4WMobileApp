using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using T4sV1.Model;

namespace T4sV1.Services
{
    public class DashboardService
    {
        private readonly HttpClient _httpClient;

        public DashboardService()
        {
            _httpClient = new HttpClient
            {
               //BaseAddress = new Uri("http://10.0.2.2:5206/") // For Android emulator
               
             BaseAddress = new Uri(" https://time4wellbeing.azurewebsites.net/") // For Android emulator
            };
        }

        public async Task<DashboardDto?> GetDashboardOverviewAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ApiDashboard/overview/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<DashboardDto>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Dashboard fetch error: {ex.Message}");
            }
            return null;
        }
    }

}
