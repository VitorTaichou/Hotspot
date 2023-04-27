using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotspot.Model.Migrations
{
    public partial class Serveraddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultServer",
                table: "TicketsConfiguration",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultServer",
                table: "TicketsConfiguration");
        }
    }
}
