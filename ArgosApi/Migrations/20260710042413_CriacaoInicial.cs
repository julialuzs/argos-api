using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ArgosApi.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "projeto",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projeto", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "relatorio",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "text", nullable: false),
                    projeto_id = table.Column<long>(type: "bigint", nullable: false),
                    data_hora_execucao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pontuacao = table.Column<int>(type: "integer", nullable: false),
                    json = table.Column<string>(type: "jsonb", nullable: false),
                    tradutor_libras_identificado = table.Column<bool>(type: "boolean", nullable: false),
                    quantidade_erros = table.Column<int>(type: "integer", nullable: false),
                    quantidade_avisos = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_relatorio", x => x.id);
                    table.ForeignKey(
                        name: "fk_relatorio_projeto_projeto_id",
                        column: x => x.projeto_id,
                        principalTable: "projeto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projeto_usuario",
                columns: table => new
                {
                    projetos_id = table.Column<long>(type: "bigint", nullable: false),
                    usuarios_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projeto_usuario", x => new { x.projetos_id, x.usuarios_id });
                    table.ForeignKey(
                        name: "fk_projeto_usuario_projetos_projetos_id",
                        column: x => x.projetos_id,
                        principalTable: "projeto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_projeto_usuario_usuarios_usuarios_id",
                        column: x => x.usuarios_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_projeto_usuario_usuarios_id",
                table: "projeto_usuario",
                column: "usuarios_id");

            migrationBuilder.CreateIndex(
                name: "ix_relatorio_projeto_id",
                table: "relatorio",
                column: "projeto_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "projeto_usuario");

            migrationBuilder.DropTable(
                name: "relatorio");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "projeto");
        }
    }
}
