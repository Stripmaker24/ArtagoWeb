using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static partial class DependencyInjection
    {
        private const string connectionStringKey = "sqlConnectionString";
        public static IServiceCollection addData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(
                    options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, configuration.GetConnectionString(connectionStringKey), builder =>
                    {
                        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    })
                );
            return services;
        }
    }
}