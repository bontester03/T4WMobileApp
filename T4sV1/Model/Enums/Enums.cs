using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace T4sV1.Model.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))] // ✅ Add this
    public enum EngagementStatus
    {
        Engaged = 0,
        Withdrawn = 1,
        Ineligible = 2,
        Uncontactable = 3
    }

    [JsonConverter(typeof(JsonStringEnumConverter))] // ✅ Add this
    public enum Gender
    {
        Male,
        Female,
        Other
    }
}