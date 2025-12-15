using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Model.Measurement
{
    /// <summary>
    /// Measurement DTO matching the Web API response
    /// </summary>
    public class MeasurementDto
    {
        public int Id { get; set; }
        public int ChildId { get; set; }
        public DateTime DateRecorded { get; set; }

        public decimal Height { get; set; } // cm
        public decimal Weight { get; set; } // kg

        // Computed fields
        public decimal? BMI { get; set; }
        public string? HealthRange { get; set; } // centile band / health range
    }

    /// <summary>
    /// Request model for creating or updating a measurement
    /// </summary>
    public class UpsertMeasurementRequest
    {
        public int? Id { get; set; } // null = create, has value = update
        public int? ChildId { get; set; } // optional; if null, API uses activeChildId
        public DateTime DateRecorded { get; set; }

        public decimal Height { get; set; } // cm
        public decimal Weight { get; set; } // kg

        public UpsertMeasurementRequest()
        {
            DateRecorded = DateTime.Now;
        }
    }
}
