using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using T4sV1.Model;

namespace T4sV1.Services
{
    public class ProfileService
    {
        private readonly HttpClient _client;

        public ProfileService()
        {

            //string baseUrl = "http://10.0.2.2:5206/"; // For Android emulator


            string baseUrl = "https://time4wellbeing.azurewebsites.net/";

            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(15)
            };
        }

        public async Task<PersonalDetailsDto?> GetPersonalDetailsAsync(int userId)
        {
            try
            {
                var response = await _client.GetAsync($"api/ApiPersonal/{userId}");
                var json = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"📄 Profile API Response: {json}");

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"❌ Profile fetch failed: {response.StatusCode}");
                    return null;
                }

                return JsonSerializer.Deserialize<PersonalDetailsDto>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"🚨 Exception in ProfileService: {ex.Message}");
                return null;
            }
        }
    }
}
