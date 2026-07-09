using ArgosApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArgosApi.Data.Configurations
{
	public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
	{
		public void Configure(EntityTypeBuilder<Usuario> builder)
		{
			builder.ToTable("Usuario");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Nome)
				.HasMaxLength(150)
				.IsRequired();

		}
	}
}