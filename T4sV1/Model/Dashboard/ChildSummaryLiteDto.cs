using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Model.Dashboard
{
    public class ChildSummaryLiteDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public int AgeYears { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
