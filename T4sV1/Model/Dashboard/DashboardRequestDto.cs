using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Model.Dashboard
{
    public class DashboardRequestDto
    {
        public int? ActiveChildId { get; set; }
        public int NotificationsTake { get; set; } = 5;
        public int ActivitiesTake { get; set; } = 5;
        public string? ClientVersion { get; set; } = "maui-1.0";
    }
}
