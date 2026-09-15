namespace ArgosApi.Common.Extensions
{
    /// <summary>
    /// Extensões de configuração de CORS
    /// </summary>
    public static class CorsExtensions
    {
        /// <summary>
        /// Nome da política CORS usada pela API
        /// </summary>
        public const string PolicyName = "AllowSpecificOrigin";

        /// <summary>
        /// Registra a política CORS a partir de Cors:Origins
        /// </summary>
        public static IServiceCollection AddCorsPolicy(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var origins = configuration.GetSection("Cors:Origins").Get<string[]>()
                ?? [];

            if (origins.Length == 0)
            {
                origins = ["http://localhost:4200"];
            }

            services.AddCors(options =>
            {
                options.AddPolicy(PolicyName, policy =>
                {
                    policy.WithOrigins(origins)
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
