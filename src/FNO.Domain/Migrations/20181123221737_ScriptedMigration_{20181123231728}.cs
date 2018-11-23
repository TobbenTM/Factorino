using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FNO.Domain.Migrations
{
    public partial class ScriptedMigration_20181123231728 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "WarehouseId",
                keyValue: new Guid("949bca62-34c6-4d1b-8f37-03bfd140ebfb"));

            migrationBuilder.DeleteData(
                table: "Corporations",
                keyColumn: "CorporationId",
                keyValue: new Guid("965da2ac-e660-426a-a847-38a5b9785721"));

            migrationBuilder.CreateTable(
                name: "ConsumerStates",
                columns: table => new
                {
                    GroupId = table.Column<string>(nullable: false),
                    Topic = table.Column<string>(nullable: false),
                    Partition = table.Column<int>(nullable: false),
                    Offset = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumerStates", x => new { x.GroupId, x.Topic, x.Partition });
                });

            migrationBuilder.InsertData(
                table: "Corporations",
                columns: new[] { "CorporationId", "CreatedByPlayerId", "Credits", "Description", "Name" },
                values: new object[] { new Guid("ae609789-3f01-4dd6-a118-e8935a9072a1"), new Guid("00000000-0000-0000-0000-000000000001"), 0, "Please Ignore", "TEST Corporation" });

            migrationBuilder.UpdateData(
                table: "Factories",
                keyColumn: "FactoryId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "CorporationId",
                value: new Guid("ae609789-3f01-4dd6-a118-e8935a9072a1"));

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "WarehouseId", "CorporationId" },
                values: new object[] { new Guid("fa6c45cd-4179-4e89-8696-b3b66264814d"), new Guid("ae609789-3f01-4dd6-a118-e8935a9072a1") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumerStates");

            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "WarehouseId",
                keyValue: new Guid("fa6c45cd-4179-4e89-8696-b3b66264814d"));

            migrationBuilder.DeleteData(
                table: "Corporations",
                keyColumn: "CorporationId",
                keyValue: new Guid("ae609789-3f01-4dd6-a118-e8935a9072a1"));

            migrationBuilder.InsertData(
                table: "Corporations",
                columns: new[] { "CorporationId", "CreatedByPlayerId", "Credits", "Description", "Name" },
                values: new object[] { new Guid("965da2ac-e660-426a-a847-38a5b9785721"), new Guid("00000000-0000-0000-0000-000000000001"), 0, "Please Ignore", "TEST Corporation" });

            migrationBuilder.UpdateData(
                table: "Factories",
                keyColumn: "FactoryId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "CorporationId",
                value: new Guid("965da2ac-e660-426a-a847-38a5b9785721"));

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "WarehouseId", "CorporationId" },
                values: new object[] { new Guid("949bca62-34c6-4d1b-8f37-03bfd140ebfb"), new Guid("965da2ac-e660-426a-a847-38a5b9785721") });
        }
    }
}
