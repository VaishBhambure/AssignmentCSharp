﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtExhibition.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Artists_ArtistID",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ArtistID",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Artists_ArtistID",
                table: "AspNetUsers",
                column: "ArtistID",
                principalTable: "Artists",
                principalColumn: "ArtistID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Artists_ArtistID",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ArtistID",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Artists_ArtistID",
                table: "AspNetUsers",
                column: "ArtistID",
                principalTable: "Artists",
                principalColumn: "ArtistID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
