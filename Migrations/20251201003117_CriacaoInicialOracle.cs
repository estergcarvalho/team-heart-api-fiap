using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamHeartFiap.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicialOracle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CANDIDATOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR2(450)", nullable: true),
                    DataCandidatura = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Demografico = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PrioridadeDiversidade = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CANDIDATOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "METRICAS_DIVERSIDADE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Categoria = table.Column<string>(type: "NVARCHAR2(450)", nullable: true),
                    Contagem = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_METRICAS_DIVERSIDADE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TREINAMENTOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Titulo = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Obrigatorio = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TREINAMENTOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CONCLUSAO_TREINAMENTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CandidatoId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TreinamentoId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONCLUSAO_TREINAMENTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CONCLUSAO_TREINAMENTO_CANDIDATOS_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "CANDIDATOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CONCLUSAO_TREINAMENTO_TREINAMENTOS_TreinamentoId",
                        column: x => x.TreinamentoId,
                        principalTable: "TREINAMENTOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CANDIDATOS_Email",
                table: "CANDIDATOS",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_CONCLUSAO_TREINAMENTO_CandidatoId_TreinamentoId",
                table: "CONCLUSAO_TREINAMENTO",
                columns: new[] { "CandidatoId", "TreinamentoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CONCLUSAO_TREINAMENTO_TreinamentoId",
                table: "CONCLUSAO_TREINAMENTO",
                column: "TreinamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_METRICAS_DIVERSIDADE_Categoria",
                table: "METRICAS_DIVERSIDADE",
                column: "Categoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CONCLUSAO_TREINAMENTO");

            migrationBuilder.DropTable(
                name: "METRICAS_DIVERSIDADE");

            migrationBuilder.DropTable(
                name: "CANDIDATOS");

            migrationBuilder.DropTable(
                name: "TREINAMENTOS");
        }
    }
}
