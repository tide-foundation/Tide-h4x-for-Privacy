using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Raziel.Logger.Migrations
{
    public partial class addedTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Logs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Logs");
        }
    }
}
