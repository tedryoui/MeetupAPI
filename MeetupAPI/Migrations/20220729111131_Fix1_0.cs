using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetupAPI.Migrations
{
    public partial class Fix1_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Orgonizers_OrgonizerId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Orgonizers");

            migrationBuilder.DropIndex(
                name: "IX_Events_OrgonizerId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "OrgonizerId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "Speeker",
                table: "Events",
                newName: "SpeekerEmail");

            migrationBuilder.AddColumn<string>(
                name: "OrgonizerEmail",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrgonizerEmail",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "SpeekerEmail",
                table: "Events",
                newName: "Speeker");

            migrationBuilder.AddColumn<int>(
                name: "OrgonizerId",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Orgonizers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orgonizers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrgonizerId",
                table: "Events",
                column: "OrgonizerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Orgonizers_OrgonizerId",
                table: "Events",
                column: "OrgonizerId",
                principalTable: "Orgonizers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
