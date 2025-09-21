using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Services
{
    public interface ISessionService
    {
        string? UserId { get; set; }
        string? Email { get; set; }

        // NEW
        int? ActiveChildId { get; set; }

        void SaveToPreferences();
        void LoadFromPreferences();

        void Clear();
    }

}
