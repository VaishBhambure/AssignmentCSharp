using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtExhibition.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removefavartid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteArtWorks",
                table: "FavoriteArtWorks");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "FavoriteArtWorks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteArtWorks",
                table: "FavoriteArtWorks",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteArtWorks",
                table: "FavoriteArtWorks");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "FavoriteArtWorks",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteArtWorks",
                table: "FavoriteArtWorks",
                column: "ID");
        }
    }
}
