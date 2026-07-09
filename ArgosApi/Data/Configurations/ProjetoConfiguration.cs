using ArgosApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArgosApi.Data.Configurations
{
    public class ProjetoConfiguration : IEntityTypeConfiguration<Projeto>
    {
        public void Configure(EntityTypeBuilder<Projeto> builder)
        {
            builder.ToTable("Projeto");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .HasMaxLength(150)
                .IsRequired();

            builder
                .HasMany(p => p.Relatorios)
                .WithOne(r => r.Projeto)
                .HasForeignKey(r => r.ProjetoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(p => p.Usuarios)
                .WithMany(u => u.Projetos)
                .UsingEntity(j => j.ToTable("ProjetoUsuario"));
        }
    }
}