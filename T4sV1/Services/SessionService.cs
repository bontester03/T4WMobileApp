using Microsoft.Maui.Storage;

namespace T4sV1.Services
{
    public class SessionService : ISessionService
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public int? ActiveChildId { get; set; }

        public void LoadFromPreferences()
        {
            UserId = Preferences.ContainsKey("UserId")
                ? Preferences.Get("UserId", string.Empty)
                : null;

            if (string.IsNullOrWhiteSpace(UserId))
                UserId = null;

            Email = Preferences.ContainsKey("UserEmail")
                ? Preferences.Get("UserEmail", string.Empty)
                : null;

            if (string.IsNullOrWhiteSpace(Email))
                Email = null;

            ActiveChildId = Preferences.ContainsKey("ActiveChildId")
                ? Preferences.Get("ActiveChildId", 0)
                : 0;

            if (ActiveChildId <= 0)
                ActiveChildId = null;
        }

        public void SaveToPreferences()
        {
            if (!string.IsNullOrWhiteSpace(UserId))
                Preferences.Set("UserId", UserId);
            else
                Preferences.Remove("UserId");

            if (!string.IsNullOrWhiteSpace(Email))
                Preferences.Set("UserEmail", Email);
            else
                Preferences.Remove("UserEmail");

            if (ActiveChildId.HasValue && ActiveChildId.Value > 0)
                Preferences.Set("ActiveChildId", ActiveChildId.Value);
            else
                Preferences.Remove("ActiveChildId");
        }

        public void Clear()
        {
            Preferences.Remove("UserId");
            Preferences.Remove("UserEmail");
            Preferences.Remove("ActiveChildId");

            UserId = null;
            Email = null;
            ActiveChildId = null;
        }
    }
}
