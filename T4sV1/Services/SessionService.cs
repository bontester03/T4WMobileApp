using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Services
{
    public class SessionService : ISessionService
    {
        public string UserId { get; set; }
        public string Email { get; set; }

        public void LoadFromPreferences()
        {
            UserId = Preferences.Get("UserId", null);
            Email = Preferences.Get("UserEmail", null);
        }

        public void SaveToPreferences()
        {
            Preferences.Set("UserId", UserId);
            Preferences.Set("UserEmail", Email);
        }

        public void Clear()
        {
            Preferences.Remove("UserId");
            Preferences.Remove("UserEmail");
            UserId = null;
            Email = null;
        }
    }

}
