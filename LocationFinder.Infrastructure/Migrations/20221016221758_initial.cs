using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocationFinder.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    CreatedUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "City", "CreatedUpdatedDate", "Email", "Latitude", "LocationName", "Longitude" },
                values: new object[] { new Guid("5d2f55c5-90a1-421c-b327-07b535831b12"), "Harelbeke", new DateTime(2022, 10, 17, 3, 47, 57, 894, DateTimeKind.Local).AddTicks(1083), "harelbeke@dundermifflin.com", 50.855365999999997, "Dunder Mifflin Harelbeke", 3.3125529999999999 });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "City", "CreatedUpdatedDate", "Email", "Latitude", "LocationName", "Longitude" },
                values: new object[] { new Guid("af4d661a-78e8-486b-8fd2-97239f161226"), "Kortrijk", new DateTime(2022, 10, 17, 3, 47, 57, 894, DateTimeKind.Local).AddTicks(1069), "kortrijk@dundermifflin.com", 50.822955999999998, "Dunder Mifflin Kortrijk", 3.2509619999999999 });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "City", "CreatedUpdatedDate", "Email", "Latitude", "LocationName", "Longitude" },
                values: new object[] { new Guid("fe32036e-498f-44a7-8c9d-5f97b25643fa"), "Ghent", new DateTime(2022, 10, 17, 3, 47, 57, 894, DateTimeKind.Local).AddTicks(1047), "ghent@dundermifflin.com", 51.043475999999998, "Dunder Mifflin Ghent", 3.7091379999999998 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
