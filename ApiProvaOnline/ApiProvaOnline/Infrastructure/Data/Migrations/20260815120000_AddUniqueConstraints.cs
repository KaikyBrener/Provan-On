using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiProvaOnline.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ---- Prova.Codigo UNIQUE ----
            migrationBuilder.CreateIndex(
                name: "IX_Prova_Codigo",
                table: "Prova",
                column: "Codigo",
                unique: true);

            // ---- AlunoResposta: impedir 2+ respostas do mesmo aluno para a mesma pergunta ----
            migrationBuilder.DropIndex(
                name: "IX_AlunoResposta_IdAluno",
                table: "AlunoResposta");

            migrationBuilder.CreateIndex(
                name: "IX_AlunoResposta_IdAluno_IdPergunta",
                table: "AlunoResposta",
                columns: new[] { "IdAluno", "IdPergunta" },
                unique: true);

            // ---- AlunoRespostaObjetiva: impedir 2+ respostas do mesmo aluno para a mesma pergunta objetiva ----
            migrationBuilder.DropIndex(
                name: "IX_AlunoRespostaObjetiva_IdAluno",
                table: "AlunoRespostaObjetiva");

            migrationBuilder.CreateIndex(
                name: "IX_AlunoRespostaObjetiva_IdAluno_IdPerguntaObjetiva",
                table: "AlunoRespostaObjetiva",
                columns: new[] { "IdAluno", "IdPerguntaObjetiva" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prova_Codigo",
                table: "Prova");

            migrationBuilder.DropIndex(
                name: "IX_AlunoResposta_IdAluno_IdPergunta",
                table: "AlunoResposta");

            migrationBuilder.CreateIndex(
                name: "IX_AlunoResposta_IdAluno",
                table: "AlunoResposta",
                column: "IdAluno");

            migrationBuilder.DropIndex(
                name: "IX_AlunoRespostaObjetiva_IdAluno_IdPerguntaObjetiva",
                table: "AlunoRespostaObjetiva");

            migrationBuilder.CreateIndex(
                name: "IX_AlunoRespostaObjetiva_IdAluno",
                table: "AlunoRespostaObjetiva",
                column: "IdAluno");
        }
    }
}
