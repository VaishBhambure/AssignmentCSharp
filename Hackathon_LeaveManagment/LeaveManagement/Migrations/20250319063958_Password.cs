using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagement.Migrations
{
    public partial class Password : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAECGAeE/sSYGlfkfreBy5oz3GwhUSmCaKIMcB6rdQhxajzMz0J6WA3b3ruRfYyJ75Pw==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEIIEebRDgbDXVWTk5kt4B19K2KSijUxgwLrgtPPLka3SDSHvN48s+AubhasAOGfEng==");
        }
    }
}
