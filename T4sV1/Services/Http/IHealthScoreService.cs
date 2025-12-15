using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using T4sV1.Model.HealthScore;

namespace T4sV1.Services.Http
{
    public interface IHealthScoreService
    {
        Task<List<HealthScoreDto>> GetHealthScoresAsync(int childId, CancellationToken ct = default);
        Task<HealthScoreDto?> GetHealthScoreByIdAsync(int id, CancellationToken ct = default);
        Task<HealthScoreDto?> CreateHealthScoreAsync(HealthScoreDto healthScore, CancellationToken ct = default);
        Task<HealthScoreDto?> UpdateHealthScoreAsync(int id, HealthScoreDto healthScore, CancellationToken ct = default);
        Task<bool> DeleteHealthScoreAsync(int id, CancellationToken ct = default);
        Task<HealthScoreDto?> GetLatestHealthScoreAsync(int childId, CancellationToken ct = default);
    }
}