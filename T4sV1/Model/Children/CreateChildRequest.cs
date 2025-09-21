using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4sV1.Model.Enums;

namespace T4sV1.Model.Children
{
    public sealed class CreateChildRequest
    {
        public string ChildName { get; set; } = "";
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        public EngagementStatus? EngagementStatus { get; set; }
    }
}
