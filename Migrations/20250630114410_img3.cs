using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaHub.Migrations
{
    /// <inheritdoc />
    public partial class img3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieImage_Movies_MovieId",
                table: "MovieImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieImage",
                table: "MovieImage");

            migrationBuilder.RenameTable(
                name: "MovieImage",
                newName: "MaovieImages");

            migrationBuilder.RenameIndex(
                name: "IX_MovieImage_MovieId",
                table: "MaovieImages",
                newName: "IX_MaovieImages_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaovieImages",
                table: "MaovieImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaovieImages_Movies_MovieId",
                table: "MaovieImages",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaovieImages_Movies_MovieId",
                table: "MaovieImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaovieImages",
                table: "MaovieImages");

            migrationBuilder.RenameTable(
                name: "MaovieImages",
                newName: "MovieImage");

            migrationBuilder.RenameIndex(
                name: "IX_MaovieImages_MovieId",
                table: "MovieImage",
                newName: "IX_MovieImage_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieImage",
                table: "MovieImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieImage_Movies_MovieId",
                table: "MovieImage",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
