using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetupAPI.Migrations
{
    public partial class Fix1_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpeekerEmail",
                table: "Events",
                newName: "SpeekerName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpeekerName",
                table: "Events",
                newName: "SpeekerEmail");
        }
    }
}
