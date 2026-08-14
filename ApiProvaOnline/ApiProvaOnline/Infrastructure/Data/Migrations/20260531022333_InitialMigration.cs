using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiProvaOnline.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Administrativo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataHoraRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrativo", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Professor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataHoraRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professor", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Prova",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataHoraInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataHoraFim = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataHoraRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Codigo = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdProfessor = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prova", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prova_Professor_IdProfessor",
                        column: x => x.IdProfessor,
                        principalTable: "Professor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Aluno",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataHoraRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdProva = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aluno", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aluno_Prova_IdProva",
                        column: x => x.IdProva,
                        principalTable: "Prova",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pergunta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pontuacao = table.Column<double>(type: "double", nullable: false),
                    DataHoraRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdProva = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pergunta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pergunta_Prova_IdProva",
                        column: x => x.IdProva,
                        principalTable: "Prova",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PerguntaObjetiva",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OpcaoA = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OpcaoB = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OpcaoC = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OpcaoD = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OpcaoCorreta = table.Column<string>(type: "char(1)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pontuacao = table.Column<double>(type: "double", nullable: false),
                    DataHoraRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdProva = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerguntaObjetiva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerguntaObjetiva_Prova_IdProva",
                        column: x => x.IdProva,
                        principalTable: "Prova",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AlunoResposta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdAluno = table.Column<int>(type: "int", nullable: false),
                    IdPergunta = table.Column<int>(type: "int", nullable: false),
                    Resposta = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PontuacaoFinal = table.Column<double>(type: "double", nullable: false),
                    DataHoraRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlunoResposta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlunoResposta_Aluno_IdAluno",
                        column: x => x.IdAluno,
                        principalTable: "Aluno",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlunoResposta_Pergunta_IdPergunta",
                        column: x => x.IdPergunta,
                        principalTable: "Pergunta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AlunoRespostaObjetiva",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdAluno = table.Column<int>(type: "int", nullable: false),
                    IdPerguntaObjetiva = table.Column<int>(type: "int", nullable: false),
                    Resposta = table.Column<string>(type: "char(1)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PontuacaoFinal = table.Column<double>(type: "double", nullable: false),
                    DataHoraRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlunoRespostaObjetiva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlunoRespostaObjetiva_Aluno_IdAluno",
                        column: x => x.IdAluno,
                        principalTable: "Aluno",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlunoRespostaObjetiva_PerguntaObjetiva_IdPerguntaObjetiva",
                        column: x => x.IdPerguntaObjetiva,
                        principalTable: "PerguntaObjetiva",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Aluno_IdProva",
                table: "Aluno",
                column: "IdProva");

            migrationBuilder.CreateIndex(
                name: "IX_AlunoResposta_IdAluno",
                table: "AlunoResposta",
                column: "IdAluno");

            migrationBuilder.CreateIndex(
                name: "IX_AlunoResposta_IdPergunta",
                table: "AlunoResposta",
                column: "IdPergunta");

            migrationBuilder.CreateIndex(
                name: "IX_AlunoRespostaObjetiva_IdAluno",
                table: "AlunoRespostaObjetiva",
                column: "IdAluno");

            migrationBuilder.CreateIndex(
                name: "IX_AlunoRespostaObjetiva_IdPerguntaObjetiva",
                table: "AlunoRespostaObjetiva",
                column: "IdPerguntaObjetiva");

            migrationBuilder.CreateIndex(
                name: "IX_Pergunta_IdProva",
                table: "Pergunta",
                column: "IdProva");

            migrationBuilder.CreateIndex(
                name: "IX_PerguntaObjetiva_IdProva",
                table: "PerguntaObjetiva",
                column: "IdProva");

            migrationBuilder.CreateIndex(
                name: "IX_Prova_IdProfessor",
                table: "Prova",
                column: "IdProfessor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrativo");

            migrationBuilder.DropTable(
                name: "AlunoResposta");

            migrationBuilder.DropTable(
                name: "AlunoRespostaObjetiva");

            migrationBuilder.DropTable(
                name: "Pergunta");

            migrationBuilder.DropTable(
                name: "Aluno");

            migrationBuilder.DropTable(
                name: "PerguntaObjetiva");

            migrationBuilder.DropTable(
                name: "Prova");

            migrationBuilder.DropTable(
                name: "Professor");
        }
    }
}
