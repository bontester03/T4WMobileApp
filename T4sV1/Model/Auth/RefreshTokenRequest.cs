using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Model.Auth
{
    public sealed class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = "";
    }
}
