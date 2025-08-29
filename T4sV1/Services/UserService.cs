using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using T4sV1.Model; // Adjust namespace as needed

namespace T4sV1.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService()
        {
            _httpClient = new HttpClient();
        }

        private string GetBaseUrl()
        {
#if DEBUG
#if ANDROID
        return "http://10.0.2.2:5206";
#elif IOS
            return "http://localhost:5206";
#else
        return "http://localhost:5206";
#endif
#else
    // Use live hosted API in Release mode
    return "https://time4wellbeing.azurewebsites.net";
#endif
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
            string url = $"{GetBaseUrl()}/api/ApiAuth/login";
            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginResponseDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
