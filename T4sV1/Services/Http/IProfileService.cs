using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using T4sV1.Model.Profile;

namespace T4sV1.Services.Http;

public interface IProfileService
{
    Task<ProfileResponseDto?> GetProfileAsync(int? activeChildId = null, CancellationToken ct = default); // ✅ Add parameter
    Task<bool> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken ct = default);
    Task<bool> UpdateAvatarAsync(UpdateAvatarDto request, CancellationToken ct = default);
}
