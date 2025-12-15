using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Model.Dashboard
{
    public class DashboardResponseDto
    {
        // server time
        public DateTime ServerTimeUtc { get; set; }

        // parent
        public ParentSummaryDto Parent { get; set; } = new();

        // child context
        public int? ActiveChildId { get; set; }
        public List<ChildSummaryLiteDto> Children { get; set; } = new(); // or List<ChildDto> if you prefer

        // totals
        public int TotalChildren { get; set; }
        public int TotalMeasurements { get; set; }
        public int TotalHealthScores { get; set; }
        public int TotalUnreadNotifications { get; set; }
        public int TotalActiveTasks { get; set; }

        // latest for active child
        public MeasurementDto? LatestMeasurement { get; set; }
        public HealthScoreDto? LatestHealthScore { get; set; }

        // reminders
        public DateTime? NextMeasurementDueUtc { get; set; }

        // lists
        public List<NotificationDto> RecentNotifications { get; set; } = new();
        public List<TimelineActivityDto> RecentActivities { get; set; } = new();
    }
}
