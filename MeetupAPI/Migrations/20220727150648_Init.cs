using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetupAPI.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeetupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Schedule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orgonizer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Speeker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Description", "Location", "MeetupName", "Orgonizer", "Schedule", "Speeker", "Theme", "Time" },
                values: new object[] { 1, "some description \n in few lines", "England, London", "Inovations of 2017 y.", "admin", "[\"fees\",\"greetings\",\"microsoft stand\",\"google stand\",\"amazon stand\",\"the ending\"]", "Boris Yakovlev", "Science", new DateTime(2017, 12, 12, 12, 12, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Description", "Location", "MeetupName", "Orgonizer", "Schedule", "Speeker", "Theme", "Time" },
                values: new object[] { 2, "Polina Sozontzova`s pary for her 18th birthday", "pros. Mira, 24", "Birthday party", "admin", "[]", "Polina Sozontzova", "Entertainment", new DateTime(2020, 7, 24, 18, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Description", "Location", "MeetupName", "Orgonizer", "Schedule", "Speeker", "Theme", "Time" },
                values: new object[] { 3, "The Carnival of Brazil is an annual Brazilian festival held the Friday afternoon before Ash Wednesday at noon", "Brazil", "Brazil Carnival", "admin", "[\"Imperio Serrano\",\"Grande Rio\",\"Mosidade\",\"Unidos Da Tijuca\"]", "Porto Seguro", "Entertainment", new DateTime(2020, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
