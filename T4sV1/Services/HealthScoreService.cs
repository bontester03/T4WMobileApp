using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using T4sV1.Model.HealthScore;

namespace T4sV1.Services.Http
{
    public class HealthScoreService : IHealthScoreService
    {
        private readonly HttpClient _httpClient;

        public HealthScoreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<HealthScoreDto>> GetHealthScoresAsync(int childId, CancellationToken ct = default)
        {
            try
            {
                // ✅ Match your backend route: api/ApiHealthScores?activeChildId={id}
                var response = await _httpClient.GetAsync($"api/apihealthscores?activeChildId={childId}", ct);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<HealthScoreDto>>(cancellationToken: ct)
                       ?? new List<HealthScoreDto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching health scores: {ex.Message}");
            }
        }

        public async Task<HealthScoreDto?> GetHealthScoreByIdAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/apihealthscores/{id}", ct);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<HealthScoreDto>(cancellationToken: ct);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching health score: {ex.Message}");
            }
        }

        public async Task<HealthScoreDto?> CreateHealthScoreAsync(HealthScoreDto healthScore, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/apihealthscores", healthScore, ct);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<HealthScoreDto>(cancellationToken: ct);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating health score: {ex.Message}");
            }
        }

        public async Task<HealthScoreDto?> UpdateHealthScoreAsync(int id, HealthScoreDto healthScore, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/apihealthscores/{id}", healthScore, ct);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<HealthScoreDto>(cancellationToken: ct);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating health score: {ex.Message}");
            }
        }

        public async Task<bool> DeleteHealthScoreAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/apihealthscores/{id}", ct);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting health score: {ex.Message}");
            }
        }

        public async Task<HealthScoreDto?> GetLatestHealthScoreAsync(int childId, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/apihealthscores/latest?activeChildId={childId}", ct);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<HealthScoreDto>(cancellationToken: ct);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching latest health score: {ex.Message}");
            }
        }
    }
}
