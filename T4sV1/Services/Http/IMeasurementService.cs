using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4sV1.Model.Measurement;

namespace T4sV1.Services.Http
{
    public interface IMeasurementService
    {
        Task<List<MeasurementDto>> GetMeasurementsAsync(int childId, CancellationToken ct = default);
        Task<List<MeasurementDto>> GetMeasurementsByDateRangeAsync(int childId, DateTime startDate, DateTime endDate, CancellationToken ct = default);
        Task<MeasurementDto?> GetMeasurementByIdAsync(int id, CancellationToken ct = default);
        Task<MeasurementDto?> CreateMeasurementAsync(MeasurementDto measurement, CancellationToken ct = default);
        Task<MeasurementDto?> UpdateMeasurementAsync(int id, MeasurementDto measurement, CancellationToken ct = default);
        Task<bool> DeleteMeasurementAsync(int id, CancellationToken ct = default);
        Task<MeasurementDto?> GetLatestMeasurementAsync(int childId, CancellationToken ct = default);
    }
}
