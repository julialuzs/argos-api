using ArgosApi.Features.Projetos;
using ArgosApi.Features.Relatorios;
using ArgosApi.Features.Usuarios;

namespace ArgosApi.Common.Extensions
{
    /// <summary>
    /// Collection de services
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<ProjetosService>();
            services.AddScoped<RelatoriosService>();
            //services.AddScoped<DashboardService>();
            services.AddScoped<UsuariosService>();

            return services;
        }
    }
}