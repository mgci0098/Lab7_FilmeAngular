using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab3.Migrations
{
    public partial class CascadeDeleteCommentsForFilm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarii_Filme_FilmId",
                table: "Comentarii");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarii_Filme_FilmId",
                table: "Comentarii",
                column: "FilmId",
                principalTable: "Filme",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarii_Filme_FilmId",
                table: "Comentarii");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarii_Filme_FilmId",
                table: "Comentarii",
                column: "FilmId",
                principalTable: "Filme",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
