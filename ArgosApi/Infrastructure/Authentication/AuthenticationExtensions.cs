using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ArgosApi.Infrastructure.Authentication
{
    /// <summary>
    /// Extensões de registro da autenticação JWT
    /// </summary>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Configura autenticação e validação de tokens JWT
        /// </summary>
        public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));

            services.AddScoped<JwtService>();

            var jwtOptions = configuration
                .GetSection(JwtOptions.SectionName)
                .Get<JwtOptions>()
                ?? throw new InvalidOperationException("Seção Jwt não configurada.");

            if (string.IsNullOrWhiteSpace(jwtOptions.Key) || jwtOptions.Key.Length < 32)
            {
                throw new InvalidOperationException(
                    "Jwt:Key não configurada ou muito curta. Defina a variável de ambiente Jwt__Key com pelo menos 32 caracteres.");
            }

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.Key))
                    };
                });

            return services;
        }

    }
}
