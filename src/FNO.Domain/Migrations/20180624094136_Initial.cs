using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FNO.Domain.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Corporations",
                columns: table => new
                {
                    CorporationId = table.Column<Guid>(nullable: false),
                    Credits = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corporations", x => x.CorporationId);
                });

            migrationBuilder.CreateTable(
                name: "EntityLibrary",
                columns: table => new
                {
                    FactorioEntityId = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    StackSize = table.Column<int>(nullable: false),
                    Fluid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityLibrary", x => x.FactorioEntityId);
                });

            migrationBuilder.CreateTable(
                name: "Factories",
                columns: table => new
                {
                    FactoryId = table.Column<Guid>(nullable: false),
                    Port = table.Column<int>(nullable: false),
                    LastSeen = table.Column<long>(nullable: false),
                    CorporationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factories", x => x.FactoryId);
                    table.ForeignKey(
                        name: "FK_Factories_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(nullable: false),
                    CorporationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Players_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    WarehouseId = table.Column<Guid>(nullable: false),
                    CorporationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.WarehouseId);
                    table.ForeignKey(
                        name: "FK_Warehouses_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseInventories",
                columns: table => new
                {
                    WarehouseInventoryId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    WarehouseId = table.Column<Guid>(nullable: false),
                    FactorioEntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseInventories", x => x.WarehouseInventoryId);
                    table.ForeignKey(
                        name: "FK_WarehouseInventories_EntityLibrary_FactorioEntityId",
                        column: x => x.FactorioEntityId,
                        principalTable: "EntityLibrary",
                        principalColumn: "FactorioEntityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseInventories_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Factories_CorporationId",
                table: "Factories",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CorporationId",
                table: "Players",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseInventories_FactorioEntityId",
                table: "WarehouseInventories",
                column: "FactorioEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseInventories_WarehouseId",
                table: "WarehouseInventories",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_CorporationId",
                table: "Warehouses",
                column: "CorporationId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Factories");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "WarehouseInventories");

            migrationBuilder.DropTable(
                name: "EntityLibrary");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Corporations");
        }
    }
}
