using ArgosApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArgosApi.Data.Configurations
{
    public class RelatorioConfiguration : IEntityTypeConfiguration<Relatorio>
    {
        public void Configure(EntityTypeBuilder<Relatorio> builder)
        {
            builder.ToTable("Relatorio");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Json)
                .HasColumnType("nvarchar(max)")
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