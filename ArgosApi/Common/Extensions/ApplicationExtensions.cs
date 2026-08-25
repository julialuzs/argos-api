using ArgosApi.Features.Authentication;
using ArgosApi.Features.Projetos;
using ArgosApi.Features.Relatorios;
using ArgosApi.Features.Relatorios.Auditoria;
using ArgosApi.Features.Usuarios;
using ArgosApi.Infrastructure.Authentication;

namespace ArgosApi.Common.Extensions
{
    /// <summary>
    /// Extensões de registro dos serviços da aplicação
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Registra os serviços de domínio e infraestrutura da aplicação
        /// </summary>
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<ArgosAvaliadorOptions>(
                configuration.GetSection(ArgosAvaliadorOptions.SectionName));
            services.AddSingleton<AuditoriaQueue>();
            services.AddSingleton<AuditoriaExecutor>();
            services.AddHostedService<AuditoriaWorker>();

            services.AddScoped<ProjetosService>();
            services.AddScoped<RelatoriosService>();
            //services.AddScoped<DashboardService>();
            services.AddScoped<UsuariosService>();
            services.AddScoped<AuthService>();
            services.AddScoped<JwtService>();
            services.AddScoped<CurrentUser>();

            return services;
        }
    }
}