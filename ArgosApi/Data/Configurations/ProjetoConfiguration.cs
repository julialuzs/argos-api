using ArgosApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArgosApi.Data.Configurations
{
    /// <summary>
    /// Configuração da entidade Projeto para o EF
    /// </summary>
    public class ProjetoConfiguration : IEntityTypeConfiguration<Projeto>
    {
        public void Configure(EntityTypeBuilder<Projeto> builder)
        {
            builder.ToTable("projeto");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Descricao)
                .HasMaxLength(250);

            builder
                .HasMany(p => p.Relatorios)
                .WithOne(r => r.Projeto)
                .HasForeignKey(r => r.ProjetoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(p => p.Usuarios)
                .WithMany(u => u.Projetos)
                .UsingEntity(j => j.ToTable("projeto_usuario"));
        }
    }
}