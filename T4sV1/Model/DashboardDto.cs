using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Model
{
   public  class DashboardDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<ChildDto> Children { get; set; }
        public List<MeasurementDto> Measurements { get; set; }
        public List<HealthScoreDto> HealthScores { get; set; }
        public List<NotificationDto> Notifications { get; set; }
        public List<TimelineActivityDto> Activities { get; set; }
    }
}
