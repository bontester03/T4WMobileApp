using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Model
{
    public class MeasurementDto
    {
        public int Id { get; set; }

        public int Age { get; set; }

        public decimal Height { get; set; }

        public decimal Weight { get; set; }

        public DateTime DateRecorded { get; set; }

        public int UserId { get; set; }

        public int CentileScore { get; set; }

        public string HealthRange { get; set; } = string.Empty; // Optional: populate from logic or backend
    }

}
