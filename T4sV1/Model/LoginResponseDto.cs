using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace T4sV1.Model
{
    public class LoginResponseDto
    {
       
        public int ?UserId { get; set; }
        public string ?Email { get; set; }
        public bool ? IsAdmin { get; set; }
    }
}
