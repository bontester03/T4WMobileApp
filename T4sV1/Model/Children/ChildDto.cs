using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4sV1.Model.Enums;


namespace T4sV1.Model.Children
{
    public sealed class ChildDto
    {
        public int Id { get; set; }
        public Guid ChildGuid { get; set; }
        public string ChildName { get; set; } = "";
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int TotalPoints { get; set; }
        public int Level { get; set; }
        public string? AvatarUrl { get; set; }
        public EngagementStatus EngagementStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Age { get; set; }
    }
}
