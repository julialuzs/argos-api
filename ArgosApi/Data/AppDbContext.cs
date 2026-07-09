using ArgosApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArgosApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Projeto> Projetos => Set<Projeto>();

        public DbSet<Usuario> Usuarios => Set<Usuario>();

        public DbSet<Relatorio> Relatorios => Set<Relatorio>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

    }
}