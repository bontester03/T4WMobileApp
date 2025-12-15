using T4sV1.Model.Children;

namespace T4sV1.Model.Dashboard
{
    public class DashboardRequest
    {
        public int? ActiveChildId { get; set; }
        public int NotificationsTake { get; set; } = 5;
        public int ActivitiesTake { get; set; } = 5;
        public string ClientVersion { get; set; } = "maui-1.0";
    }

    public class DashboardResponse
    {
        public DateTime ServerTimeUtc { get; set; }
        public ParentSummaryDto Parent { get; set; } = new();
        public int? ActiveChildId { get; set; }
        public List<ChildSummaryDto> Children { get; set; } = new();

        // Totals
        public int TotalChildren { get; set; }
        public int TotalMeasurements { get; set; }
        public int TotalHealthScores { get; set; }
        public int TotalUnreadNotifications { get; set; }
        public int TotalActiveTasks { get; set; }

        // Latest data for ActiveChild
        public MeasurementDto? LatestMeasurement { get; set; }
        public HealthScoreDto? LatestHealthScore { get; set; }

        // Upcoming
        public DateTime? NextMeasurementDueUtc { get; set; }

        // Recent lists
        public List<NotificationDto> RecentNotifications { get; set; } = new();
        public List<ActivityDto> RecentActivities { get; set; } = new();
    }

    public class ParentSummaryDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = "";
        public string? LastName { get; set; }
        public string Email { get; set; } = "";
    }

    public class ChildSummaryDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public int AgeYears { get; set; }
        public string? AvatarUrl { get; set; }

        public int TotalPoints { get; set; }
        public int Level { get; set; }
    }

    public class MeasurementDto
    {
        public int Id { get; set; }
        public DateTime RecordedOnUtc { get; set; }

        // Metric values
        public decimal WeightKg { get; set; }
        public decimal? WeightLbs { get; set; }
        public decimal HeightCm { get; set; }
        public decimal? HeightIn { get; set; }

        // Derived values
        public decimal? BMI { get; set; }
        public string? CentileBand { get; set; } // "Healthy", "Overweight"
    }

    public class HealthScoreDto
    {
        public int Id { get; set; }
        public DateTime SubmittedOnUtc { get; set; }

        // Component scores (0-10)
        public int PhysicalActivityScore { get; set; }
        public int BreakfastScore { get; set; }
        public int FruitVegScore { get; set; }
        public int SweetSnacksScore { get; set; }
        public int FattyFoodsScore { get; set; }

        // Overall score (0-10)
        public decimal TotalScore { get; set; }
    }

    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Body { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedUtc { get; set; }
    }

    public class ActivityDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = "";
        public DateTime OccurredUtc { get; set; }
        public string? Category { get; set; } // "Measurement", "HealthScore", "Task"
    }
}