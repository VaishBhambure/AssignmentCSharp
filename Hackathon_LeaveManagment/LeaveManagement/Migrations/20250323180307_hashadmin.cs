using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagement.Migrations
{
    public partial class hashadmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Email", "Password" },
                values: new object[] { "Vaish@gmail.com", "paitRMUIN2LDZFlKojLFOVyK6I/8nrskys5ImJZa1ro=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Password",
                value: "j4xynUKA8o7xUfpx++/aM7UIcM9IXtx1PTnfs7baCJ4=");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Email", "Password" },
                values: new object[] { "vaish@gmail.com", "AQAAAAEAACcQAAAAEJ2Z5C/6wFyb8CUOcH0TGEqVfKp0HwUb2hxoOZrRHBB9/yrfsGUrq3aL7zvaVvMoeg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Password",
                value: "AQAAAAEAACcQAAAAEAng7Q9j1+TyynKdvYtWcpMqZkAWdd8uK7qgEaa1r2esHFgCJDs+JUOfAI9dkeia8A==");
        }
    }
}
