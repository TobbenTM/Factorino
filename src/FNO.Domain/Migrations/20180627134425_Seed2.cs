using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FNO.Domain.Migrations
{
    public partial class Seed2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseInventories_EntityLibrary_FactorioEntityName",
                table: "WarehouseInventories");

            migrationBuilder.DropColumn(
                name: "FactorioEntityId",
                table: "WarehouseInventories");

            migrationBuilder.RenameColumn(
                name: "FactorioEntityName",
                table: "WarehouseInventories",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseInventories_FactorioEntityName",
                table: "WarehouseInventories",
                newName: "IX_WarehouseInventories_ItemId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Factories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Corporations",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Corporations",
                columns: new[] { "CorporationId", "Credits", "Name" },
                values: new object[] { new Guid("69b1ab63-a179-4387-a1a8-a52705b5e77e"), 0, "Test Corporation" });

            migrationBuilder.InsertData(
                table: "Factories",
                columns: new[] { "FactoryId", "CorporationId", "CurrentlyResearchingId", "LastSeen", "Name", "PlayersOnline", "Port" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("69b1ab63-a179-4387-a1a8-a52705b5e77e"), null, 0L, "Test Factory", 0, 0 });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "WarehouseId", "CorporationId" },
                values: new object[] { new Guid("52d967b9-4ce7-4171-8283-07831d348707"), new Guid("69b1ab63-a179-4387-a1a8-a52705b5e77e") });

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseInventories_EntityLibrary_ItemId",
                table: "WarehouseInventories",
                column: "ItemId",
                principalTable: "EntityLibrary",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseInventories_EntityLibrary_ItemId",
                table: "WarehouseInventories");

            migrationBuilder.DeleteData(
                table: "Factories",
                keyColumn: "FactoryId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "WarehouseId",
                keyValue: new Guid("52d967b9-4ce7-4171-8283-07831d348707"));

            migrationBuilder.DeleteData(
                table: "Corporations",
                keyColumn: "CorporationId",
                keyValue: new Guid("69b1ab63-a179-4387-a1a8-a52705b5e77e"));

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Corporations");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "WarehouseInventories",
                newName: "FactorioEntityName");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseInventories_ItemId",
                table: "WarehouseInventories",
                newName: "IX_WarehouseInventories_FactorioEntityName");

            migrationBuilder.AddColumn<Guid>(
                name: "FactorioEntityId",
                table: "WarehouseInventories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseInventories_EntityLibrary_FactorioEntityName",
                table: "WarehouseInventories",
                column: "FactorioEntityName",
                principalTable: "EntityLibrary",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
