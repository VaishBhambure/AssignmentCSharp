using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagement.Migrations
{
    public partial class defaultrole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEIIEebRDgbDXVWTk5kt4B19K2KSijUxgwLrgtPPLka3SDSHvN48s+AubhasAOGfEng==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEFDPSwM1wq01tyZIGKen7RPspuu5ScHjPLtnE8FHtrKfEM9aMg3kfyJ5C0dbYSvmMA==");
        }
    }
}
