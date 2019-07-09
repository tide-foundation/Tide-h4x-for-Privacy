using Microsoft.EntityFrameworkCore.Migrations;

namespace Raziel.Logger.Migrations
{
    public partial class addedreferrer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Referrer",
                table: "Logs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Referrer",
                table: "Logs");
        }
    }
}
