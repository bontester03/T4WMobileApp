using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Model.HealthScore
{
    /// <summary>
    /// Health Score DTO matching the Web API response
    /// Each individual score: 0-4 (API) → mapped to 1-5 (display)
    /// TotalScore: 5-25 (sum of all 5 scores after mapping)
    /// </summary>
    public class HealthScoreDto
    {
        public int Id { get; set; }
        public int ChildId { get; set; }
        public DateTime DateRecorded { get; set; }

        // Individual scores (0-4 from API, will be mapped to 1-5)
        public int PhysicalActivityScore { get; set; }
        public int BreakfastScore { get; set; }
        public int FruitVegScore { get; set; }
        public int SweetSnacksScore { get; set; }
        public int FattyFoodsScore { get; set; }

        // Computed scores
        public int TotalScore { get; set; }        // 5-25 (after mapping: each score 1-5)
        public decimal OverallScore10 { get; set; } // 0-10 (for alternate display)
    }

    /// <summary>
    /// Request model for creating or updating a health score
    /// </summary>
    public class UpsertHealthScoreRequest
    {
        public int? Id { get; set; } // null = create, has value = update
        public int? ChildId { get; set; } // optional; if null, API uses activeChildId
        public DateTime DateRecorded { get; set; }

        public int PhysicalActivityScore { get; set; } // 0-4
        public int BreakfastScore { get; set; } // 0-4
        public int FruitVegScore { get; set; } // 0-4
        public int SweetSnacksScore { get; set; } // 0-4
        public int FattyFoodsScore { get; set; } // 0-4

        public UpsertHealthScoreRequest()
        {
            DateRecorded = DateTime.Now;
        }
    }
}
