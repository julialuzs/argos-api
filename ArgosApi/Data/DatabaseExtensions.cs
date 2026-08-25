using Microsoft.EntityFrameworkCore;

namespace ArgosApi.Data
{
    /// <summary>
    /// Extensões de configuração do banco de dados
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Registra o contexto e a conexão com o PostgreSQL
        /// </summary>
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
