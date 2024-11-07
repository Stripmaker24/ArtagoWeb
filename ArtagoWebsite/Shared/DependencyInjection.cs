using Microsoft.Extensions.DependencyInjection;

namespace Shared
{
    public static partial class DependencyInjection
    {
        public static IServiceCollection addShared(this IServiceCollection services)
        {
            return services;
        }
    }
}