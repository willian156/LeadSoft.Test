using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeadSoft.Test.Migrations
{
    /// <inheritdoc />
    public partial class FK_anime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Animes_UserId",
                table: "Animes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_Users_UserId",
                table: "Animes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_Users_UserId",
                table: "Animes");

            migrationBuilder.DropIndex(
                name: "IX_Animes_UserId",
                table: "Animes");
        }
    }
}
