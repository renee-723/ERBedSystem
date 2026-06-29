using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERBedSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Patients",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "BedId",
                table: "Encounters",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Beds",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Operation = table.Column<string>(type: "TEXT", nullable: false),
                    PatientId = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "ER-A01",
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "ER-A02",
                column: "Status",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "ICU-01",
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "ICU-02",
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "PEDS-01",
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: "P101",
                column: "Age",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: "P102",
                column: "Age",
                value: 0);

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "Age", "ChiefComplaint", "Name", "Status", "TriageLevel" },
                values: new object[] { "P103", 0, "AGE", "李小奎", "Waiting", 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: "P103");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "BedId",
                table: "Encounters",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Beds",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "ER-A01",
                column: "Status",
                value: "Available");

            migrationBuilder.UpdateData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "ER-A02",
                column: "Status",
                value: "Cleaning");

            migrationBuilder.UpdateData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "ICU-01",
                column: "Status",
                value: "Available");

            migrationBuilder.UpdateData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "ICU-02",
                column: "Status",
                value: "Occupied");

            migrationBuilder.UpdateData(
                table: "Beds",
                keyColumn: "Id",
                keyValue: "PEDS-01",
                column: "Status",
                value: "Available");
        }
    }
}
