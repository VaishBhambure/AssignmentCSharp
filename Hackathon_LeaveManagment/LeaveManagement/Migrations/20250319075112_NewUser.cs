using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagement.Migrations
{
    public partial class NewUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "Password");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAEAACcQAAAAEJut2qDKrFmxYJ+a1/3pBV85PdI0cuWPz2BxaG6MdqemIx7E1yyEO0j8QeKvPey8Iw==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAECGAeE/sSYGlfkfreBy5oz3GwhUSmCaKIMcB6rdQhxajzMz0J6WA3b3ruRfYyJ75Pw==");
        }
    }
}
