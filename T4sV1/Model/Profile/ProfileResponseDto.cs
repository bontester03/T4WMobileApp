using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4sV1.Model.Children;

namespace T4sV1.Model.Profile
{
    public sealed class ProfileResponseDto
    {
        public ChildDto? Child { get; set; }
        public PersonalDetailsDto? ParentInfo { get; set; }
    }
}
