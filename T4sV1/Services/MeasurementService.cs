using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using T4sV1.Model.Measurement;

namespace T4sV1.Services.Http
{
    public class MeasurementService : IMeasurementService
    {
        private readonly HttpClient _httpClient;

        public MeasurementService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ✅ Fetch all measurements for a child
        public async Task<List<MeasurementDto>> GetMeasurementsAsync(int childId, CancellationToken ct = default)
        {
            try
            {
                // Matches: ApiMeasurementsController → GET api/ApiMeasurements?activeChildId={childId}
                var response = await _httpClient.GetAsync($"api/apimeasurements?activeChildId={childId}", ct);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<MeasurementDto>>(cancellationToken: ct)
                       ?? new List<MeasurementDto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching measurements: {ex.Message}");
            }
        }

        // ✅ Fetch by date range
        public async Task<List<MeasurementDto>> GetMeasurementsByDateRangeAsync(
            int childId, DateTime startDate, DateTime endDate, CancellationToken ct = default)
        {
            try
            {
                // Matches: GET api/ApiMeasurements/range?activeChildId={childId}&start={start}&end={end}
                string start = startDate.ToString("yyyy-MM-dd");
                string end = endDate.ToString("yyyy-MM-dd");

                var response = await _httpClient.GetAsync(
                    $"api/apimeasurements/range?activeChildId={childId}&start={start}&end={end}", ct);

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<MeasurementDto>>(cancellationToken: ct)
                       ?? new List<MeasurementDto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching measurements by date range: {ex.Message}");
            }
        }

        // ✅ Fetch by ID
        public async Task<MeasurementDto?> GetMeasurementByIdAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/apimeasurements/{id}", ct);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<MeasurementDto>(cancellationToken: ct);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching measurement: {ex.Message}");
            }
        }

        // ✅ Create new measurement
        public async Task<MeasurementDto?> CreateMeasurementAsync(MeasurementDto measurement, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/apimeasurements", measurement, ct);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<MeasurementDto>(cancellationToken: ct);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating measurement: {ex.Message}");
            }
        }

        // ✅ Update existing measurement
        public async Task<MeasurementDto?> UpdateMeasurementAsync(int id, MeasurementDto measurement, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/apimeasurements/{id}", measurement, ct);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<MeasurementDto>(cancellationToken: ct);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating measurement: {ex.Message}");
            }
        }

        // ✅ Delete measurement
        public async Task<bool> DeleteMeasurementAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/apimeasurements/{id}", ct);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting measurement: {ex.Message}");
            }
        }

        // ✅ Get latest measurement for a child
        public async Task<MeasurementDto?> GetLatestMeasurementAsync(int childId, CancellationToken ct = default)
        {
            try
            {
                // Matches: GET api/ApiMeasurements/latest?activeChildId={childId}
                var response = await _httpClient.GetAsync($"api/apimeasurements/latest?activeChildId={childId}", ct);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<MeasurementDto>(cancellationToken: ct);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching latest measurement: {ex.Message}");
            }
        }
    }
}
