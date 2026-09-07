using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArgosApi.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaIndiceUnicoGuidProjeto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE projeto
                SET guid = gen_random_uuid()
                WHERE guid = '00000000-0000-0000-0000-000000000000';
                """);

            migrationBuilder.CreateIndex(
                name: "ix_projeto_guid",
                table: "projeto",
                column: "guid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_projeto_guid",
                table: "projeto");
        }
    }
}
