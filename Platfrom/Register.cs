using Microsoft.Extensions.DependencyInjection;
using Platform.LogHelper;
using Platform.Utility;
using System.Reflection.Emit;

namespace Platform
{
    public static class Register
    {
        public static void RegisterPlatform(this IServiceCollection service)
        {
            service.AddSingleton(typeof(LogService<>));
        }
    }
}

