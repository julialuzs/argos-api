using ArgosApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArgosApi.Data
{
    /// <summary>
    /// Contexto de acesso ao banco de dados
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Projetos cadastrados
        /// </summary>
        public DbSet<Projeto> Projetos => Set<Projeto>();

        /// <summary>
        /// Usuários cadastrados
        /// </summary>
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        /// <summary>
        /// Relatórios de auditoria
        /// </summary>
        public DbSet<Relatorio> Relatorios => Set<Relatorio>();

        /// <summary>
        /// Inicializa o contexto com as opções informadas
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Aplica as configurações de entidades do assembly
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

    }
}