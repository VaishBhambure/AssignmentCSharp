using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtExhibition.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addrows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "AspNetUsers",
                newName: "BirthDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "AspNetUsers",
                newName: "DateOfBirth");
        }
    }
}
