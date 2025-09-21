using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4sV1.Services;

public interface IActiveChildStore
{
    Task<int?> GetAsync();
    Task SetAsync(int childId);
    Task ClearAsync();
}

public sealed class ActiveChildStore : IActiveChildStore
{
    private const string Key = "active.child.id";

    public Task<int?> GetAsync()
    {
        var exists = Preferences.ContainsKey(Key);
        if (!exists) return Task.FromResult<int?>(null);
        return Task.FromResult<int?>(Preferences.Get(Key, -1) is var v && v > 0 ? v : (int?)null);
    }

    public Task SetAsync(int childId)
    {
        Preferences.Set(Key, childId);
        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        Preferences.Remove(Key);
        return Task.CompletedTask;
    }
}

