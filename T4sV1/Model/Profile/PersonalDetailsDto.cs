using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Model.Profile
{
    public sealed class PersonalDetailsDto
    {
        public string School { get; set; } = "";
        public string Class { get; set; } = "";
        public string ParentGuardianName { get; set; } = "";
        public string RelationshipToChild { get; set; } = "";
        public string TeleNumber { get; set; } = "";
        public string Email { get; set; } = "";
        public string Postcode { get; set; } = "";
    }
}
