using ArgosApi.Features.Authentication;
using ArgosApi.Features.Projetos;
using ArgosApi.Features.Relatorios;
using ArgosApi.Features.Usuarios;
using ArgosApi.Infrastructure.Authentication;

namespace ArgosApi.Common.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<ProjetosService>();
            services.AddScoped<RelatoriosService>();
            //services.AddScoped<DashboardService>();
            services.AddScoped<UsuariosService>();
            services.AddScoped<AuthService>();
            services.AddScoped<JwtService>();

            return services;
        }
    }
}