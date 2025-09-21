using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace T4sV1.Model.Auth
{
    public sealed class LoginResponseDto
    {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";

        [JsonPropertyName("roles")]
        public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = "";

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = "";
    }
}
