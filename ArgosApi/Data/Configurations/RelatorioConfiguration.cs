using ArgosApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArgosApi.Data.Configurations
{
    /// <summary>
    /// Configuração da entidade Relatorio para o EF
    /// </summary>
    public class RelatorioConfiguration : IEntityTypeConfiguration<Relatorio>
    {
        public void Configure(EntityTypeBuilder<Relatorio> builder)
        {
            builder.ToTable("relatorio");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Json)
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(r => r.Pontuacao)
                .IsRequired();

            builder.Property(r => r.DataHoraExecucao)
                .IsRequired();

            builder.Property(r => r.TradutorLibrasIdentificado)
                .IsRequired();

            builder.Property(r => r.QuantidadeErros)
                .IsRequired();

            builder.Property(r => r.QuantidadeAvisos)
                .IsRequired();

        }
    }
}