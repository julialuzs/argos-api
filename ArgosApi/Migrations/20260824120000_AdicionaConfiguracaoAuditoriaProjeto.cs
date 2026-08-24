using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArgosApi.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaConfiguracaoAuditoriaProjeto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "url_base",
                table: "projeto",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string[]>(
                name: "rotas",
                table: "projeto",
                type: "text[]",
                nullable: false,
                defaultValue: new[] { "/" });

            migrationBuilder.AddColumn<bool>(
                name: "incluir_w3c",
                table: "projeto",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "status_execucao",
                table: "projeto",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "mensagem_erro_execucao",
                table: "projeto",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "url_base",
                table: "projeto");

            migrationBuilder.DropColumn(
                name: "rotas",
                table: "projeto");

            migrationBuilder.DropColumn(
                name: "incluir_w3c",
                table: "projeto");

            migrationBuilder.DropColumn(
                name: "status_execucao",
                table: "projeto");

            migrationBuilder.DropColumn(
                name: "mensagem_erro_execucao",
                table: "projeto");
        }
    }
}
