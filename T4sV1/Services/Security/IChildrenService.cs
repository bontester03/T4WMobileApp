using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4sV1.Model.Children;

namespace T4sV1.Services.Security
{
    public interface IChildrenService
    {
        Task<IReadOnlyList<ChildDto>> ListAsync(bool includeDeleted = false, CancellationToken ct = default);
        Task<ChildDto?> GetAsync(int id, CancellationToken ct = default);
        Task<ChildDto> CreateAsync(CreateChildRequest dto, CancellationToken ct = default);
        Task UpdateAsync(int id, UpdateChildRequest dto, CancellationToken ct = default);
        Task UpdateAvatarAsync(int id, string avatarUrl, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<string>> GetAvatarUrlsAsync(CancellationToken ct = default);
    }
}
