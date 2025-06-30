using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaHub.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieGalleryRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieId1",
                table: "MaovieImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaovieImages_MovieId1",
                table: "MaovieImages",
                column: "MovieId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MaovieImages_Movies_MovieId1",
                table: "MaovieImages",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaovieImages_Movies_MovieId1",
                table: "MaovieImages");

            migrationBuilder.DropIndex(
                name: "IX_MaovieImages_MovieId1",
                table: "MaovieImages");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MaovieImages");
        }
    }
}
