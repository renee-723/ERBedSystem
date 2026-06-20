using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERBedSystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Beds",
                columns: new[] { "Id", "Description", "Status", "Zone" },
                values: new object[,]
                {
                    { "ER-A01", "留觀 A 區靠窗", "Available", "Ward" },
                    { "ER-A02", "留觀 A 區，剛出院消毒中", "Cleaning", "Ward" },
                    { "ICU-01", "靠近護理站，有生理監視器", "Available", "ICU" },
                    { "ICU-02", "葉克膜專用床", "Occupied", "ICU" },
                    { "PEDS-01", "兒科急診獨立隔離床", "Available", "Peds" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "ChiefComplaint", "Name", "Status", "TriageLevel" },
                values: new object[,]
                {
                    { "P101", "車禍右腳擦傷", "張小明", "Waiting", 3 },
                    { "P102", "疑似心肌梗塞，OHCA", "王大同", "Waiting", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "ER-A01");

            migrationBuilder.DeleteData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "ER-A02");

            migrationBuilder.DeleteData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "ICU-01");

            migrationBuilder.DeleteData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "ICU-02");

            migrationBuilder.DeleteData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "PEDS-01");

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: "P101");

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: "P102");
        }
    }
}
