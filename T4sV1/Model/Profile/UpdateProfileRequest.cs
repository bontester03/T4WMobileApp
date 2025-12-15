using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4sV1.Model.Children;
using T4sV1.Model.Enums;

namespace T4sV1.Model.Profile
{
    public sealed class UpdateProfileRequest
    {
        public int ChildId { get; set; }  
        public UpdateChildDto? Child { get; set; }
        public PersonalDetailsDto? ParentInfo { get; set; }
    }

    public sealed class UpdateChildDto
    {
        public string? ChildName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
