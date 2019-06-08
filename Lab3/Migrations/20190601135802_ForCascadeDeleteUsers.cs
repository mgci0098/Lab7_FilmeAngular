using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab3.Migrations
{
    public partial class ForCascadeDeleteUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Filme_OwnerId",
                table: "Filme");
   
            migrationBuilder.DropForeignKey(
                name: "FK_Filme_Users_OwnerId",
                table: "Filme");

            migrationBuilder.CreateIndex(
                name: "IX_Filme_OwnerId",
                table: "Filme",
                column: "OwnerId");


            migrationBuilder.AddForeignKey(
                name: "FK_Filme_Users_OwnerId",
                table: "Filme",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Filme_OwnerId",
                table: "Filme");

            migrationBuilder.DropForeignKey(
                name: "FK_Filme_Users_OwnerId",
                table: "Filme");

            migrationBuilder.CreateIndex(
                name: "IX_Filme_OwnerId",
                table: "Filme",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Filme_Users_OwnerId",
                table: "Filme",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
