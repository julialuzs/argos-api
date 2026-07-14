using Microsoft.EntityFrameworkCore;

namespace ArgosApi.Data
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabaseConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                options.UseSnakeCaseNamingConvention();
            });

            return services;
        }
    }
}
