using ArgosApi.Features.Projetos;
using ArgosApi.Features.Usuarios;

namespace ArgosApi.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<ProjetosService>();
            //services.AddScoped<ReportService>();
            //services.AddScoped<DashboardService>();
            services.AddScoped<UsuariosService>();

            return services;
        }
    }
}