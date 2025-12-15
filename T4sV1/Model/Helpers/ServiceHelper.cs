using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace T4sV1.Helpers
{
    public static class ServiceHelper
    {
        public static IServiceProvider Services =>
            Application.Current?.Handler?.MauiContext?.Services
            ?? throw new InvalidOperationException("Service provider not ready.");

        public static T GetService<T>() where T : notnull =>
            Services.GetRequiredService<T>();
    }
}