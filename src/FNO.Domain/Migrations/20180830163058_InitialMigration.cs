using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FNO.Domain.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityLibrary",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    StackSize = table.Column<int>(nullable: false),
                    Subgroup = table.Column<string>(nullable: true),
                    Fluid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityLibrary", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "TechnologyLibrary",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Icon = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    MaxLevel = table.Column<string>(nullable: true),
                    Upgradeable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologyLibrary", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "CorporationInvitations",
                columns: table => new
                {
                    InvitationId = table.Column<Guid>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: false),
                    CorporationId = table.Column<Guid>(nullable: false),
                    Accepted = table.Column<bool>(nullable: false),
                    Completed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporationInvitations", x => x.InvitationId);
                });

            migrationBuilder.CreateTable(
                name: "Factories",
                columns: table => new
                {
                    FactoryId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: false),
                    LastSeen = table.Column<long>(nullable: false),
                    PlayersOnline = table.Column<int>(nullable: false),
                    CorporationId = table.Column<Guid>(nullable: false),
                    CurrentlyResearchingId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factories", x => x.FactoryId);
                    table.ForeignKey(
                        name: "FK_Factories_TechnologyLibrary_CurrentlyResearchingId",
                        column: x => x.CurrentlyResearchingId,
                        principalTable: "TechnologyLibrary",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SteamId = table.Column<string>(nullable: true),
                    CorporationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                });

            migrationBuilder.CreateTable(
                name: "Corporations",
                columns: table => new
                {
                    CorporationId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Credits = table.Column<int>(nullable: false),
                    CreatedByPlayerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corporations", x => x.CorporationId);
                    table.ForeignKey(
                        name: "FK_Corporations_Players_CreatedByPlayerId",
                        column: x => x.CreatedByPlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
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
                    ItemId = table.Column<string>(nullable: true),
                    WarehouseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseInventories", x => x.WarehouseInventoryId);
                    table.ForeignKey(
                        name: "FK_WarehouseInventories_EntityLibrary_ItemId",
                        column: x => x.ItemId,
                        principalTable: "EntityLibrary",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseInventories_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EntityLibrary",
                columns: new[] { "Name", "Fluid", "Icon", "StackSize", "Subgroup", "Type" },
                values: new object[,]
                {
                    { "crude-oil", true, "graphics/icons/fluid/crude-oil.png", 0, null, "fluid" },
                    { "nuclear-fuel", false, "graphics/icons/nuclear-fuel.png", 1, "intermediate-product", "item" },
                    { "rocket-control-unit", false, "graphics/icons/rocket-control-unit.png", 10, "intermediate-product", "item" },
                    { "rocket-part", false, "graphics/icons/rocket-part.png", 5, "intermediate-product", "item" },
                    { "satellite", false, "graphics/icons/satellite.png", 1, "intermediate-product", "item" },
                    { "concrete", false, "graphics/icons/concrete.png", 100, "terrain", "item" },
                    { "refined-concrete", false, "graphics/icons/refined-concrete.png", 100, "terrain", "item" },
                    { "hazard-concrete", false, "graphics/icons/hazard-concrete.png", 100, "terrain", "item" },
                    { "refined-hazard-concrete", false, "graphics/icons/refined-hazard-concrete.png", 100, "terrain", "item" },
                    { "landfill", false, "graphics/icons/landfill.png", 100, "terrain", "item" },
                    { "electric-energy-interface", false, "graphics/icons/accumulator.png", 50, "energy", "item" },
                    { "uranium-ore", false, "graphics/icons/uranium-ore.png", 50, "raw-resource", "item" },
                    { "rocket-fuel", false, "graphics/icons/rocket-fuel.png", 10, "intermediate-product", "item" },
                    { "nuclear-reactor", false, "graphics/icons/nuclear-reactor.png", 10, "energy", "item" },
                    { "uranium-238", false, "graphics/icons/uranium-238.png", 100, "intermediate-product", "item" },
                    { "centrifuge", false, "graphics/icons/centrifuge.png", 50, "production-machine", "item" },
                    { "uranium-fuel-cell", false, "graphics/icons/uranium-fuel-cell.png", 50, "intermediate-product", "item" },
                    { "used-up-uranium-fuel-cell", false, "graphics/icons/used-up-uranium-fuel-cell.png", 50, "intermediate-product", "item" },
                    { "heat-exchanger", false, "graphics/icons/heat-boiler.png", 50, "energy", "item" },
                    { "steam-turbine", false, "graphics/icons/steam-turbine.png", 10, "energy", "item" },
                    { "heat-pipe", false, "graphics/icons/heat-pipe.png", 50, "energy", "item" },
                    { "simple-entity-with-force", false, "graphics/icons/steel-chest.png", 50, "other", "item" },
                    { "simple-entity-with-owner", false, "graphics/icons/wooden-chest.png", 50, "other", "item" },
                    { "item-with-tags", false, "graphics/icons/wooden-chest.png", 1, "other", "item-with-tags" },
                    { "item-with-label", false, "graphics/icons/wooden-chest.png", 1, "other", "item-with-label" },
                    { "uranium-235", false, "graphics/icons/uranium-235.png", 100, "intermediate-product", "item" },
                    { "item-with-inventory", false, "graphics/icons/wooden-chest.png", 1, "other", "item-with-inventory" },
                    { "low-density-structure", false, "graphics/icons/rocket-structure.png", 10, "intermediate-product", "item" },
                    { "power-switch", false, "graphics/icons/power-switch.png", 50, "circuit-network", "item" },
                    { "big-electric-pole", false, "graphics/icons/big-electric-pole.png", 50, "energy-pipe-distribution", "item" },
                    { "medium-electric-pole", false, "graphics/icons/medium-electric-pole.png", 50, "energy-pipe-distribution", "item" },
                    { "substation", false, "graphics/icons/substation.png", 50, "energy-pipe-distribution", "item" },
                    { "accumulator", false, "graphics/icons/accumulator.png", 50, "energy", "item" },
                    { "steel-furnace", false, "graphics/icons/steel-furnace.png", 50, "smelting-machine", "item" },
                    { "electric-furnace", false, "graphics/icons/electric-furnace.png", 50, "smelting-machine", "item" },
                    { "beacon", false, "graphics/icons/beacon.png", 10, "module", "item" },
                    { "storage-tank", false, "graphics/icons/storage-tank.png", 50, "storage", "item" },
                    { "pump", false, "graphics/icons/pump.png", 50, "energy-pipe-distribution", "item" },
                    { "pumpjack", false, "graphics/icons/pumpjack.png", 20, "extraction-machine", "item" },
                    { "oil-refinery", false, "graphics/icons/oil-refinery.png", 10, "production-machine", "item" },
                    { "programmable-speaker", false, "graphics/icons/programmable-speaker.png", 50, "circuit-network", "item" },
                    { "chemical-plant", false, "graphics/icons/chemical-plant.png", 10, "production-machine", "item" },
                    { "empty-barrel", false, "graphics/icons/fluid/barreling/empty-barrel.png", 10, "intermediate-product", "item" },
                    { "solid-fuel", false, "graphics/icons/solid-fuel.png", 50, "raw-material", "item" },
                    { "plastic-bar", false, "graphics/icons/plastic-bar.png", 100, "raw-material", "item" },
                    { "engine-unit", false, "graphics/icons/engine-unit.png", 50, "intermediate-product", "item" },
                    { "electric-engine-unit", false, "graphics/icons/electric-engine-unit.png", 50, "intermediate-product", "item" },
                    { "explosives", false, "graphics/icons/explosives.png", 50, "raw-material", "item" },
                    { "battery", false, "graphics/icons/battery.png", 200, "raw-material", "item" },
                    { "flying-robot-frame", false, "graphics/icons/flying-robot-frame.png", 50, "intermediate-product", "item" },
                    { "arithmetic-combinator", false, "graphics/icons/arithmetic-combinator.png", 50, "circuit-network", "item" },
                    { "decider-combinator", false, "graphics/icons/decider-combinator.png", 50, "circuit-network", "item" },
                    { "constant-combinator", false, "graphics/icons/constant-combinator.png", 50, "circuit-network", "item" },
                    { "sulfur", false, "graphics/icons/sulfur.png", 50, "raw-material", "item" },
                    { "coin", false, "graphics/icons/coin.png", 100000, "science-pack", "item" },
                    { "infinity-chest", false, "graphics/icons/infinity-chest.png", 10, "other", "item" },
                    { "speed-module-2", false, "graphics/icons/speed-module-2.png", 50, "module", "module" },
                    { "destroyer-capsule", false, "graphics/icons/destroyer.png", 100, "capsule", "capsule" },
                    { "discharge-defense-remote", false, "graphics/equipment/discharge-defense-equipment-ability.png", 1, "capsule", "capsule" },
                    { "cliff-explosives", false, "graphics/icons/cliff-explosives.png", 20, "terrain", "capsule" },
                    { "artillery-targeting-remote", false, "graphics/icons/artillery-targeting-remote.png", 1, "capsule", "capsule" },
                    { "pistol", false, "graphics/icons/pistol.png", 5, "gun", "gun" },
                    { "submachine-gun", false, "graphics/icons/submachine-gun.png", 5, "gun", "gun" },
                    { "flamethrower", false, "graphics/icons/flamethrower.png", 5, "gun", "gun" },
                    { "vehicle-machine-gun", false, "graphics/icons/submachine-gun.png", 1, "gun", "gun" },
                    { "tank-machine-gun", false, "graphics/icons/submachine-gun.png", 1, "gun", "gun" },
                    { "tank-flamethrower", false, "graphics/icons/flamethrower.png", 1, "gun", "gun" },
                    { "land-mine", false, "graphics/icons/land-mine.png", 100, "gun", "item" },
                    { "distractor-capsule", false, "graphics/icons/distractor.png", 100, "capsule", "capsule" },
                    { "rocket-launcher", false, "graphics/icons/rocket-launcher.png", 5, "gun", "gun" },
                    { "combat-shotgun", false, "graphics/icons/combat-shotgun.png", 5, "gun", "gun" },
                    { "railgun", false, "graphics/icons/railgun.png", 5, "gun", "gun" },
                    { "tank-cannon", false, "graphics/icons/tank-cannon.png", 0, "gun", "gun" },
                    { "artillery-wagon-cannon", false, "graphics/icons/tank-cannon.png", 0, "gun", "gun" },
                    { "light-armor", false, "graphics/icons/light-armor.png", 10, "armor", "armor" },
                    { "heavy-armor", false, "graphics/icons/heavy-armor.png", 10, "armor", "armor" },
                    { "modular-armor", false, "graphics/icons/modular-armor.png", 1, "armor", "armor" },
                    { "power-armor", false, "graphics/icons/power-armor.png", 1, "armor", "armor" },
                    { "power-armor-mk2", false, "graphics/icons/power-armor-mk2.png", 1, "armor", "armor" },
                    { "gun-turret", false, "graphics/icons/gun-turret.png", 50, "defensive-structure", "item" },
                    { "laser-turret", false, "graphics/icons/laser-turret.png", 50, "defensive-structure", "item" },
                    { "shotgun", false, "graphics/icons/shotgun.png", 5, "gun", "gun" },
                    { "speed-module", false, "graphics/icons/speed-module.png", 50, "module", "module" },
                    { "defender-capsule", false, "graphics/icons/defender.png", 100, "capsule", "capsule" },
                    { "poison-capsule", false, "graphics/icons/poison-capsule.png", 100, "capsule", "capsule" },
                    { "speed-module-3", false, "graphics/icons/speed-module-3.png", 50, "module", "module" },
                    { "effectivity-module", false, "graphics/icons/effectivity-module.png", 50, "module", "module" },
                    { "effectivity-module-2", false, "graphics/icons/effectivity-module-2.png", 50, "module", "module" },
                    { "effectivity-module-3", false, "graphics/icons/effectivity-module-3.png", 50, "module", "module" },
                    { "productivity-module", false, "graphics/icons/productivity-module.png", 50, "module", "module" },
                    { "productivity-module-2", false, "graphics/icons/productivity-module-2.png", 50, "module", "module" },
                    { "productivity-module-3", false, "graphics/icons/productivity-module-3.png", 50, "module", "module" },
                    { "firearm-magazine", false, "graphics/icons/firearm-magazine.png", 200, "ammo", "ammo" },
                    { "piercing-rounds-magazine", false, "graphics/icons/piercing-rounds-magazine.png", 200, "ammo", "ammo" },
                    { "uranium-rounds-magazine", false, "graphics/icons/uranium-rounds-magazine.png", 200, "ammo", "ammo" },
                    { "flamethrower-ammo", false, "graphics/icons/flamethrower-ammo.png", 100, "ammo", "ammo" },
                    { "slowdown-capsule", false, "graphics/icons/slowdown-capsule.png", 100, "capsule", "capsule" },
                    { "rocket", false, "graphics/icons/rocket.png", 200, "ammo", "ammo" },
                    { "atomic-bomb", false, "graphics/icons/atomic-bomb.png", 10, "ammo", "ammo" },
                    { "shotgun-shell", false, "graphics/icons/shotgun-shell.png", 200, "ammo", "ammo" },
                    { "piercing-shotgun-shell", false, "graphics/icons/piercing-shotgun-shell.png", 200, "ammo", "ammo" },
                    { "railgun-dart", false, "graphics/icons/railgun-ammo.png", 200, "ammo", "ammo" },
                    { "cannon-shell", false, "graphics/icons/cannon-shell.png", 200, "ammo", "ammo" },
                    { "explosive-cannon-shell", false, "graphics/icons/explosive-cannon-shell.png", 200, "ammo", "ammo" },
                    { "uranium-cannon-shell", false, "graphics/icons/uranium-cannon-shell.png", 200, "ammo", "ammo" },
                    { "explosive-uranium-cannon-shell", false, "graphics/icons/explosive-uranium-cannon-shell.png", 200, "ammo", "ammo" },
                    { "artillery-shell", false, "graphics/icons/artillery-shell.png", 1, "ammo", "ammo" },
                    { "grenade", false, "graphics/icons/grenade.png", 100, "capsule", "capsule" },
                    { "cluster-grenade", false, "graphics/icons/cluster-grenade.png", 100, "capsule", "capsule" },
                    { "explosive-rocket", false, "graphics/icons/explosive-rocket.png", 200, "ammo", "ammo" },
                    { "flamethrower-turret", false, "graphics/icons/flamethrower-turret.png", 50, "defensive-structure", "item" },
                    { "roboport", false, "graphics/icons/roboport.png", 10, "logistic-network", "item" },
                    { "logistic-chest-requester", false, "graphics/icons/logistic-chest-requester.png", 50, "logistic-network", "item" },
                    { "wood", false, "graphics/icons/wood.png", 50, "raw-material", "item" },
                    { "iron-plate", false, "graphics/icons/iron-plate.png", 100, "raw-material", "item" },
                    { "copper-plate", false, "graphics/icons/copper-plate.png", 100, "raw-material", "item" },
                    { "copper-cable", false, "graphics/icons/copper-cable.png", 200, "intermediate-product", "item" },
                    { "iron-stick", false, "graphics/icons/iron-stick.png", 100, "intermediate-product", "item" },
                    { "iron-gear-wheel", false, "graphics/icons/iron-gear-wheel.png", 100, "intermediate-product", "item" },
                    { "electronic-circuit", false, "graphics/icons/electronic-circuit.png", 200, "intermediate-product", "item" },
                    { "wooden-chest", false, "graphics/icons/wooden-chest.png", 50, "storage", "item" },
                    { "stone-furnace", false, "graphics/icons/stone-furnace.png", 50, "smelting-machine", "item" },
                    { "burner-mining-drill", false, "graphics/icons/burner-mining-drill.png", 50, "extraction-machine", "item" },
                    { "electric-mining-drill", false, "graphics/icons/electric-mining-drill.png", 50, "extraction-machine", "item" },
                    { "copper-ore", false, "graphics/icons/copper-ore.png", 50, "raw-resource", "item" },
                    { "transport-belt", false, "graphics/icons/transport-belt.png", 100, "belt", "item" },
                    { "inserter", false, "graphics/icons/inserter.png", 50, "inserter", "item" },
                    { "offshore-pump", false, "graphics/icons/offshore-pump.png", 20, "extraction-machine", "item" },
                    { "pipe", false, "graphics/icons/pipe.png", 100, "energy-pipe-distribution", "item" },
                    { "boiler", false, "graphics/icons/boiler.png", 50, "energy", "item" },
                    { "steam-engine", false, "graphics/icons/steam-engine.png", 10, "energy", "item" },
                    { "small-electric-pole", false, "graphics/icons/small-electric-pole.png", 50, "energy-pipe-distribution", "item" },
                    { "radar", false, "graphics/icons/radar.png", 50, "defensive-structure", "item" },
                    { "computer", false, "graphics/icons/computer.png", 1, "defensive-structure", "item" },
                    { "small-plane", false, "graphics/icons/small-plane.png", 1, "transport", "item" },
                    { "small-lamp", false, "graphics/icons/small-lamp.png", 50, "circuit-network", "item" },
                    { "pipe-to-ground", false, "graphics/icons/pipe-to-ground.png", 50, "energy-pipe-distribution", "item" },
                    { "burner-inserter", false, "graphics/icons/burner-inserter.png", 50, "inserter", "item" },
                    { "assembling-machine-1", false, "graphics/icons/assembling-machine-1.png", 50, "production-machine", "item" },
                    { "iron-ore", false, "graphics/icons/iron-ore.png", 50, "raw-resource", "item" },
                    { "coal", false, "graphics/icons/coal.png", 50, "raw-resource", "item" },
                    { "heavy-oil", true, "graphics/icons/fluid/heavy-oil.png", 0, null, "fluid" },
                    { "light-oil", true, "graphics/icons/fluid/light-oil.png", 0, null, "fluid" },
                    { "petroleum-gas", true, "graphics/icons/fluid/petroleum-gas.png", 0, null, "fluid" },
                    { "lubricant", true, "graphics/icons/fluid/lubricant.png", 0, null, "fluid" },
                    { "sulfuric-acid", true, "graphics/icons/fluid/sulfuric-acid.png", 0, null, "fluid" },
                    { "water", true, "graphics/icons/fluid/water.png", 0, null, "fluid" },
                    { "steam", true, "graphics/icons/fluid/steam.png", 0, null, "fluid" },
                    { "solar-panel-equipment", false, "graphics/icons/solar-panel-equipment.png", 20, "equipment", "item" },
                    { "fusion-reactor-equipment", false, "graphics/icons/fusion-reactor-equipment.png", 20, "equipment", "item" },
                    { "energy-shield-equipment", false, "graphics/icons/energy-shield-equipment.png", 50, "equipment", "item" },
                    { "energy-shield-mk2-equipment", false, "graphics/icons/energy-shield-mk2-equipment.png", 50, "equipment", "item" },
                    { "stone", false, "graphics/icons/stone.png", 50, "raw-resource", "item" },
                    { "battery-equipment", false, "graphics/icons/battery-equipment.png", 50, "equipment", "item" },
                    { "personal-laser-defense-equipment", false, "graphics/icons/personal-laser-defense-equipment.png", 20, "equipment", "item" },
                    { "discharge-defense-equipment", false, "graphics/icons/discharge-defense-equipment.png", 20, "equipment", "item" },
                    { "exoskeleton-equipment", false, "graphics/icons/exoskeleton-equipment.png", 10, "equipment", "item" },
                    { "personal-roboport-equipment", false, "graphics/icons/personal-roboport-equipment.png", 5, "equipment", "item" },
                    { "personal-roboport-mk2-equipment", false, "graphics/icons/personal-roboport-mk2-equipment.png", 5, "equipment", "item" },
                    { "night-vision-equipment", false, "graphics/icons/night-vision-equipment.png", 20, "equipment", "item" },
                    { "belt-immunity-equipment", false, "graphics/icons/belt-immunity-equipment.png", 1, "equipment", "item" },
                    { "iron-axe", false, "graphics/icons/iron-axe.png", 20, "tool", "mining-tool" },
                    { "steel-axe", false, "graphics/icons/steel-axe.png", 20, "tool", "mining-tool" },
                    { "stone-brick", false, "graphics/icons/stone-brick.png", 100, "terrain", "item" },
                    { "raw-wood", false, "graphics/icons/raw-wood.png", 100, "raw-resource", "item" },
                    { "battery-mk2-equipment", false, "graphics/icons/battery-mk2-equipment.png", 50, "equipment", "item" },
                    { "rocket-silo", false, "graphics/icons/rocket-silo.png", 1, "defensive-structure", "item" },
                    { "red-wire", false, "graphics/icons/red-wire.png", 200, "circuit-network", "item" },
                    { "raw-fish", false, "graphics/icons/fish.png", 100, "raw-resource", "capsule" },
                    { "high-tech-science-pack", false, "graphics/icons/high-tech-science-pack.png", 200, "science-pack", "tool" },
                    { "space-science-pack", false, "graphics/icons/space-science-pack.png", 2000, "science-pack", "tool" },
                    { "lab", false, "graphics/icons/lab.png", 10, "production-machine", "item" },
                    { "train-stop", false, "graphics/icons/train-stop.png", 10, "transport", "item" },
                    { "rail-signal", false, "graphics/icons/rail-signal.png", 50, "transport", "item" },
                    { "rail-chain-signal", false, "graphics/icons/rail-chain-signal.png", 50, "transport", "item" },
                    { "steel-plate", false, "graphics/icons/steel-plate.png", 100, "raw-material", "item" },
                    { "underground-belt", false, "graphics/icons/underground-belt.png", 50, "belt", "item" },
                    { "fast-underground-belt", false, "graphics/icons/fast-underground-belt.png", 50, "belt", "item" },
                    { "express-underground-belt", false, "graphics/icons/express-underground-belt.png", 50, "belt", "item" },
                    { "splitter", false, "graphics/icons/splitter.png", 50, "belt", "item" },
                    { "production-science-pack", false, "graphics/icons/production-science-pack.png", 200, "science-pack", "tool" },
                    { "fast-splitter", false, "graphics/icons/fast-splitter.png", 50, "belt", "item" },
                    { "loader", false, "graphics/icons/loader.png", 50, "belt", "item" },
                    { "fast-loader", false, "graphics/icons/fast-loader.png", 50, "belt", "item" },
                    { "express-loader", false, "graphics/icons/express-loader.png", 50, "belt", "item" },
                    { "advanced-circuit", false, "graphics/icons/advanced-circuit.png", 200, "intermediate-product", "item" },
                    { "processing-unit", false, "graphics/icons/processing-unit.png", 100, "intermediate-product", "item" },
                    { "logistic-robot", false, "graphics/icons/logistic-robot.png", 50, "logistic-network", "item" },
                    { "construction-robot", false, "graphics/icons/construction-robot.png", 50, "logistic-network", "item" },
                    { "logistic-chest-passive-provider", false, "graphics/icons/logistic-chest-passive-provider.png", 50, "logistic-network", "item" },
                    { "logistic-chest-active-provider", false, "graphics/icons/logistic-chest-active-provider.png", 50, "logistic-network", "item" },
                    { "logistic-chest-storage", false, "graphics/icons/logistic-chest-storage.png", 50, "logistic-network", "item" },
                    { "logistic-chest-buffer", false, "graphics/icons/logistic-chest-buffer.png", 50, "logistic-network", "item" },
                    { "express-splitter", false, "graphics/icons/express-splitter.png", 50, "belt", "item" },
                    { "green-wire", false, "graphics/icons/green-wire.png", 200, "circuit-network", "item" },
                    { "military-science-pack", false, "graphics/icons/military-science-pack.png", 200, "science-pack", "tool" },
                    { "science-pack-2", false, "graphics/icons/science-pack-2.png", 200, "science-pack", "tool" },
                    { "repair-pack", false, "graphics/icons/repair-pack.png", 100, "tool", "repair-tool" },
                    { "stone-wall", false, "graphics/icons/stone-wall.png", 100, "defensive-structure", "item" },
                    { "iron-chest", false, "graphics/icons/iron-chest.png", 50, "storage", "item" },
                    { "steel-chest", false, "graphics/icons/steel-chest.png", 50, "storage", "item" },
                    { "fast-transport-belt", false, "graphics/icons/fast-transport-belt.png", 100, "belt", "item" },
                    { "express-transport-belt", false, "graphics/icons/express-transport-belt.png", 100, "belt", "item" },
                    { "long-handed-inserter", false, "graphics/icons/long-handed-inserter.png", 50, "inserter", "item" },
                    { "fast-inserter", false, "graphics/icons/fast-inserter.png", 50, "inserter", "item" },
                    { "filter-inserter", false, "graphics/icons/filter-inserter.png", 50, "inserter", "item" },
                    { "stack-inserter", false, "graphics/icons/stack-inserter.png", 50, "inserter", "item" },
                    { "stack-filter-inserter", false, "graphics/icons/stack-filter-inserter.png", 50, "inserter", "item" },
                    { "science-pack-3", false, "graphics/icons/science-pack-3.png", 200, "science-pack", "tool" },
                    { "assembling-machine-2", false, "graphics/icons/assembling-machine-2.png", 50, "production-machine", "item" },
                    { "solar-panel", false, "graphics/icons/solar-panel.png", 50, "energy", "item" },
                    { "locomotive", false, "graphics/icons/diesel-locomotive.png", 5, "transport", "item-with-entity-data" },
                    { "cargo-wagon", false, "graphics/icons/cargo-wagon.png", 5, "transport", "item-with-entity-data" },
                    { "fluid-wagon", false, "graphics/icons/fluid-wagon.png", 5, "transport", "item-with-entity-data" },
                    { "artillery-wagon", false, "graphics/icons/artillery-wagon.png", 5, "transport", "item-with-entity-data" },
                    { "rail", false, "graphics/icons/rail.png", 100, "transport", "rail-planner" },
                    { "player-port", false, "graphics/icons/player-port.png", 50, "defensive-structure", "item" },
                    { "gate", false, "graphics/icons/gate.png", 50, "defensive-structure", "item" },
                    { "car", false, "graphics/icons/car.png", 1, "transport", "item-with-entity-data" },
                    { "tank", false, "graphics/icons/tank.png", 1, "transport", "item-with-entity-data" },
                    { "science-pack-1", false, "graphics/icons/science-pack-1.png", 200, "science-pack", "tool" },
                    { "assembling-machine-3", false, "graphics/icons/assembling-machine-3.png", 50, "production-machine", "item" },
                    { "artillery-turret", false, "graphics/icons/artillery-turret.png", 10, "defensive-structure", "item" }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "PlayerId", "CorporationId", "Name", "SteamId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), null, "<system>", "<system>" });

            migrationBuilder.InsertData(
                table: "TechnologyLibrary",
                columns: new[] { "Name", "Icon", "Level", "MaxLevel", "Upgradeable" },
                values: new object[,]
                {
                    { "military", "graphics/technology/military.png", 0, null, false },
                    { "bullet-damage-3", "graphics/technology/bullet-damage.png", 0, null, true },
                    { "bullet-damage-4", "graphics/technology/bullet-damage.png", 0, null, true },
                    { "bullet-damage-5", "graphics/technology/bullet-damage.png", 0, null, true },
                    { "bullet-damage-6", "graphics/technology/bullet-damage.png", 0, null, true },
                    { "bullet-damage-7", "graphics/technology/bullet-damage.png", 0, "infinite", true },
                    { "bullet-speed-1", "graphics/technology/bullet-speed.png", 0, null, true },
                    { "bullet-speed-2", "graphics/technology/bullet-speed.png", 0, null, true },
                    { "bullet-speed-3", "graphics/technology/bullet-speed.png", 0, null, true },
                    { "bullet-speed-4", "graphics/technology/bullet-speed.png", 0, null, true },
                    { "bullet-speed-5", "graphics/technology/bullet-speed.png", 0, null, true },
                    { "bullet-speed-6", "graphics/technology/bullet-speed.png", 0, null, true },
                    { "fluid-handling", "graphics/technology/fluid-handling.png", 0, null, false },
                    { "bullet-damage-2", "graphics/technology/bullet-damage.png", 0, null, true },
                    { "oil-processing", "graphics/technology/oil-gathering.png", 0, null, false },
                    { "coal-liquefaction", "graphics/technology/coal-liquefaction.png", 0, null, false },
                    { "sulfur-processing", "graphics/technology/sulfur-processing.png", 0, null, false },
                    { "plastics", "graphics/technology/plastics.png", 0, null, false },
                    { "modules", "graphics/technology/module.png", 0, null, false },
                    { "speed-module", "graphics/technology/speed-module.png", 0, null, true },
                    { "speed-module-2", "graphics/technology/speed-module.png", 0, null, true },
                    { "speed-module-3", "graphics/technology/speed-module.png", 0, null, true },
                    { "productivity-module", "graphics/technology/productivity-module.png", 0, null, true },
                    { "productivity-module-2", "graphics/technology/productivity-module.png", 0, null, true },
                    { "productivity-module-3", "graphics/technology/productivity-module.png", 0, null, true },
                    { "effectivity-module", "graphics/technology/effectivity-module.png", 0, null, true },
                    { "effectivity-module-2", "graphics/technology/effectivity-module.png", 0, null, true },
                    { "advanced-oil-processing", "graphics/technology/oil-processing.png", 0, null, false },
                    { "effectivity-module-3", "graphics/technology/effectivity-module.png", 0, null, true },
                    { "bullet-damage-1", "graphics/technology/bullet-damage.png", 0, null, true },
                    { "personal-roboport-equipment", "graphics/technology/personal-roboport-equipment.png", 0, null, false },
                    { "laser-turret-speed-6", "graphics/technology/laser-turret-speed.png", 0, null, true },
                    { "laser-turret-speed-7", "graphics/technology/laser-turret-speed.png", 0, null, true },
                    { "gun-turret-damage-1", "graphics/technology/gun-turret-damage.png", 0, null, true },
                    { "gun-turret-damage-2", "graphics/technology/gun-turret-damage.png", 0, null, true },
                    { "gun-turret-damage-3", "graphics/technology/gun-turret-damage.png", 0, null, true },
                    { "gun-turret-damage-4", "graphics/technology/gun-turret-damage.png", 0, null, true },
                    { "gun-turret-damage-5", "graphics/technology/gun-turret-damage.png", 0, null, true },
                    { "gun-turret-damage-6", "graphics/technology/gun-turret-damage.png", 0, null, true },
                    { "gun-turret-damage-7", "graphics/technology/gun-turret-damage.png", 0, "infinite", true },
                    { "flamethrower-damage-1", "graphics/technology/flamethrower-turret-damage.png", 0, null, true },
                    { "flamethrower-damage-2", "graphics/technology/flamethrower-turret-damage.png", 0, null, true },
                    { "flamethrower-damage-3", "graphics/technology/flamethrower-turret-damage.png", 0, null, true },
                    { "personal-roboport-equipment-2", "graphics/technology/personal-roboport-equipment.png", 0, null, false },
                    { "flamethrower-damage-4", "graphics/technology/flamethrower-turret-damage.png", 0, null, true },
                    { "flamethrower-damage-6", "graphics/technology/flamethrower-turret-damage.png", 0, null, true },
                    { "flamethrower-damage-7", "graphics/technology/flamethrower-turret-damage.png", 0, "infinite", true },
                    { "energy-shield-equipment", "graphics/technology/energy-shield-equipment.png", 0, null, false },
                    { "night-vision-equipment", "graphics/technology/night-vision-equipment.png", 0, null, false },
                    { "energy-shield-mk2-equipment", "graphics/technology/energy-shield-mk2-equipment.png", 0, null, false },
                    { "battery-equipment", "graphics/technology/battery-equipment.png", 0, null, false },
                    { "battery-mk2-equipment", "graphics/technology/battery-mk2-equipment.png", 0, null, false },
                    { "solar-panel-equipment", "graphics/technology/solar-panel-equipment.png", 0, null, false },
                    { "personal-laser-defense-equipment", "graphics/technology/personal-laser-defense-equipment.png", 0, null, false },
                    { "discharge-defense-equipment", "graphics/technology/discharge-defense-equipment.png", 0, null, false },
                    { "fusion-reactor-equipment", "graphics/technology/fusion-reactor-equipment.png", 0, null, false },
                    { "exoskeleton-equipment", "graphics/technology/exoskeleton-equipment.png", 0, null, false },
                    { "flamethrower-damage-5", "graphics/technology/flamethrower-turret-damage.png", 0, null, true },
                    { "laser-turret-speed-5", "graphics/technology/laser-turret-speed.png", 0, null, true },
                    { "combat-robotics", "graphics/technology/combat-robotics.png", 0, null, false },
                    { "combat-robotics-3", "graphics/technology/combat-robotics.png", 0, null, false },
                    { "cannon-shell-speed-5", "graphics/technology/cannon-speed.png", 0, null, true },
                    { "artillery-shell-range-1", "graphics/technology/artillery-range.png", 0, "infinite", false },
                    { "artillery-shell-speed-1", "graphics/technology/artillery-speed.png", 0, "infinite", false },
                    { "follower-robot-count-1", "graphics/technology/follower-robots.png", 0, null, true },
                    { "follower-robot-count-2", "graphics/technology/follower-robots.png", 0, null, true },
                    { "follower-robot-count-3", "graphics/technology/follower-robots.png", 0, null, true },
                    { "follower-robot-count-4", "graphics/technology/follower-robots.png", 0, null, true },
                    { "follower-robot-count-5", "graphics/technology/follower-robots.png", 0, null, true },
                    { "follower-robot-count-6", "graphics/technology/follower-robots.png", 0, null, true },
                    { "follower-robot-count-7", "graphics/technology/follower-robots.png", 14, "infinite", true },
                    { "nuclear-power", "graphics/technology/nuclear-power.png", 0, null, false },
                    { "kovarex-enrichment-process", "graphics/technology/kovarex-enrichment-process.png", 0, null, false },
                    { "cannon-shell-speed-4", "graphics/technology/cannon-speed.png", 0, null, true },
                    { "nuclear-fuel-reprocessing", "graphics/technology/nuclear-fuel-reprocessing.png", 0, null, false },
                    { "mining-productivity-4", "graphics/technology/mining-productivity.png", 0, "7", true },
                    { "mining-productivity-8", "graphics/technology/mining-productivity.png", 0, "11", true },
                    { "mining-productivity-12", "graphics/technology/mining-productivity.png", 0, "15", true },
                    { "mining-productivity-16", "graphics/technology/mining-productivity.png", 0, "infinite", true },
                    { "artillery", "graphics/technology/artillery.png", 0, null, false },
                    { "stack-inserter", "graphics/technology/stack-inserter.png", 0, null, true },
                    { "inserter-capacity-bonus-1", "graphics/technology/inserter-capacity.png", 0, null, true },
                    { "inserter-capacity-bonus-2", "graphics/technology/inserter-capacity.png", 0, null, true },
                    { "inserter-capacity-bonus-3", "graphics/technology/inserter-capacity.png", 0, null, true },
                    { "inserter-capacity-bonus-4", "graphics/technology/inserter-capacity.png", 0, null, true },
                    { "inserter-capacity-bonus-5", "graphics/technology/inserter-capacity.png", 0, null, true },
                    { "inserter-capacity-bonus-6", "graphics/technology/inserter-capacity.png", 0, null, true },
                    { "mining-productivity-1", "graphics/technology/mining-productivity.png", 0, "3", true },
                    { "combat-robotics-2", "graphics/technology/combat-robotics.png", 0, null, false },
                    { "cannon-shell-speed-3", "graphics/technology/cannon-speed.png", 0, null, true },
                    { "cannon-shell-speed-1", "graphics/technology/cannon-speed.png", 0, null, true },
                    { "combat-robot-damage-1", "graphics/technology/combat-robot-damage.png", 0, null, true },
                    { "combat-robot-damage-2", "graphics/technology/combat-robot-damage.png", 0, null, true },
                    { "combat-robot-damage-3", "graphics/technology/combat-robot-damage.png", 0, null, true },
                    { "combat-robot-damage-4", "graphics/technology/combat-robot-damage.png", 0, null, true },
                    { "combat-robot-damage-5", "graphics/technology/combat-robot-damage.png", 0, null, true },
                    { "combat-robot-damage-6", "graphics/technology/combat-robot-damage.png", 0, "infinite", true },
                    { "rocket-damage-1", "graphics/technology/rocket-damage.png", 0, null, true },
                    { "rocket-damage-2", "graphics/technology/rocket-damage.png", 0, null, true },
                    { "rocket-damage-3", "graphics/technology/rocket-damage.png", 0, null, true },
                    { "rocket-damage-4", "graphics/technology/rocket-damage.png", 0, null, true },
                    { "rocket-damage-5", "graphics/technology/rocket-damage.png", 0, null, true },
                    { "rocket-damage-6", "graphics/technology/rocket-damage.png", 0, null, true },
                    { "cannon-shell-speed-2", "graphics/technology/cannon-speed.png", 0, null, true },
                    { "rocket-damage-7", "graphics/technology/rocket-damage.png", 0, "infinite", true },
                    { "rocket-speed-2", "graphics/technology/rocket-speed.png", 0, null, true },
                    { "rocket-speed-3", "graphics/technology/rocket-speed.png", 0, null, true },
                    { "rocket-speed-4", "graphics/technology/rocket-speed.png", 0, null, true },
                    { "rocket-speed-5", "graphics/technology/rocket-speed.png", 0, null, true },
                    { "rocket-speed-6", "graphics/technology/rocket-speed.png", 0, null, true },
                    { "rocket-speed-7", "graphics/technology/rocket-speed.png", 0, null, true },
                    { "cannon-shell-damage-1", "graphics/technology/cannon-damage.png", 0, null, true },
                    { "cannon-shell-damage-2", "graphics/technology/cannon-damage.png", 0, null, true },
                    { "cannon-shell-damage-3", "graphics/technology/cannon-damage.png", 0, null, true },
                    { "cannon-shell-damage-4", "graphics/technology/cannon-damage.png", 0, null, true },
                    { "cannon-shell-damage-5", "graphics/technology/cannon-damage.png", 0, null, true },
                    { "cannon-shell-damage-6", "graphics/technology/cannon-damage.png", 0, "infinite", true },
                    { "rocket-speed-1", "graphics/technology/rocket-speed.png", 0, null, true },
                    { "laser-turret-speed-4", "graphics/technology/laser-turret-speed.png", 0, null, true },
                    { "laser-turret-speed-3", "graphics/technology/laser-turret-speed.png", 0, null, true },
                    { "laser-turret-speed-2", "graphics/technology/laser-turret-speed.png", 0, null, true },
                    { "braking-force-2", "graphics/technology/braking-force.png", 0, null, true },
                    { "braking-force-3", "graphics/technology/braking-force.png", 0, null, true },
                    { "braking-force-4", "graphics/technology/braking-force.png", 0, null, true },
                    { "braking-force-5", "graphics/technology/braking-force.png", 0, null, true },
                    { "braking-force-6", "graphics/technology/braking-force.png", 0, null, true },
                    { "braking-force-7", "graphics/technology/braking-force.png", 0, null, true },
                    { "automobilism", "graphics/technology/automobilism.png", 0, null, false },
                    { "tanks", "graphics/technology/tanks.png", 0, null, false },
                    { "logistics-2", "graphics/technology/logistics.png", 0, null, false },
                    { "logistics-3", "graphics/technology/logistics.png", 0, null, false },
                    { "optics", "graphics/technology/optics.png", 0, null, false },
                    { "solar-energy", "graphics/technology/solar-energy.png", 0, null, false },
                    { "braking-force-1", "graphics/technology/braking-force.png", 0, null, true },
                    { "laser", "graphics/technology/laser.png", 0, null, false },
                    { "explosive-rocketry", "graphics/technology/explosive-rocketry.png", 0, null, false },
                    { "heavy-armor", "graphics/technology/armor-making.png", 0, null, false },
                    { "modular-armor", "graphics/technology/armor-making.png", 0, null, false },
                    { "power-armor", "graphics/technology/power-armor.png", 0, null, false },
                    { "power-armor-2", "graphics/technology/power-armor-mk2.png", 0, null, false },
                    { "turrets", "graphics/technology/turrets.png", 0, null, false },
                    { "laser-turrets", "graphics/technology/laser-turrets.png", 0, null, false },
                    { "stone-walls", "graphics/technology/stone-walls.png", 0, null, false },
                    { "gates", "graphics/technology/gates.png", 0, null, false },
                    { "flying", "graphics/technology/flying.png", 0, null, false },
                    { "robotics", "graphics/technology/robotics.png", 0, null, false },
                    { "rocket-silo", "graphics/technology/rocket-silo.png", 0, null, false },
                    { "rocketry", "graphics/technology/rocketry.png", 0, null, false },
                    { "research-speed-1", "graphics/technology/research-speed.png", 0, null, true },
                    { "rail-signals", "graphics/technology/rail-signals.png", 0, null, false },
                    { "fluid-wagon", "graphics/technology/fluid-wagon.png", 0, null, false },
                    { "military-2", "graphics/technology/military.png", 0, null, false },
                    { "military-3", "graphics/technology/military.png", 0, null, false },
                    { "military-4", "graphics/technology/military.png", 0, null, false },
                    { "uranium-ammo", "graphics/technology/uranium-ammo.png", 0, null, false },
                    { "atomic-bomb", "graphics/technology/atomic-bomb.png", 0, null, false },
                    { "grenade-damage-1", "graphics/technology/grenade-damage.png", 0, null, true },
                    { "inserter-capacity-bonus-7", "graphics/technology/inserter-capacity.png", 0, null, true },
                    { "grenade-damage-3", "graphics/technology/grenade-damage.png", 0, null, true },
                    { "grenade-damage-4", "graphics/technology/grenade-damage.png", 0, null, true },
                    { "grenade-damage-5", "graphics/technology/grenade-damage.png", 0, null, true },
                    { "grenade-damage-6", "graphics/technology/grenade-damage.png", 0, null, true },
                    { "grenade-damage-7", "graphics/technology/grenade-damage.png", 0, "infinite", true },
                    { "automated-rail-transportation", "graphics/technology/automated-rail-transportation.png", 0, null, false },
                    { "automation", "graphics/technology/automation.png", 0, null, false },
                    { "automation-2", "graphics/technology/automation.png", 0, null, false },
                    { "automation-3", "graphics/technology/automation.png", 0, null, false },
                    { "explosives", "graphics/technology/explosives.png", 0, null, false },
                    { "cliff-explosives", "graphics/technology/cliff-explosives.png", 0, null, false },
                    { "flammables", "graphics/technology/flammables.png", 0, null, false },
                    { "land-mine", "graphics/technology/land-mine.png", 0, null, false },
                    { "flamethrower", "graphics/technology/flamethrower.png", 0, null, false },
                    { "circuit-network", "graphics/technology/circuit-network.png", 0, null, false },
                    { "advanced-electronics", "graphics/technology/advanced-electronics.png", 0, null, false },
                    { "advanced-electronics-2", "graphics/technology/advanced-electronics-2.png", 0, null, false },
                    { "logistics", "graphics/technology/logistics.png", 0, null, false },
                    { "railway", "graphics/technology/railway.png", 0, null, false },
                    { "electronics", "graphics/technology/electronics.png", 0, null, false },
                    { "research-speed-2", "graphics/technology/research-speed.png", 0, null, true },
                    { "research-speed-3", "graphics/technology/research-speed.png", 0, null, true },
                    { "research-speed-4", "graphics/technology/research-speed.png", 0, null, true },
                    { "character-logistic-slots-6", "graphics/technology/character-logistic-slots.png", 0, null, true },
                    { "character-logistic-trash-slots-1", "graphics/technology/character-logistic-trash-slots.png", 0, null, true },
                    { "character-logistic-trash-slots-2", "graphics/technology/character-logistic-trash-slots.png", 0, null, true },
                    { "auto-character-logistic-trash-slots", "graphics/technology/character-auto-logistic-trash-slots.png", 0, null, false },
                    { "shotgun-shell-damage-1", "graphics/technology/shotgun-shell-damage.png", 0, null, true },
                    { "shotgun-shell-damage-2", "graphics/technology/shotgun-shell-damage.png", 0, null, true },
                    { "shotgun-shell-damage-3", "graphics/technology/shotgun-shell-damage.png", 0, null, true },
                    { "shotgun-shell-damage-4", "graphics/technology/shotgun-shell-damage.png", 0, null, true },
                    { "shotgun-shell-damage-5", "graphics/technology/shotgun-shell-damage.png", 0, null, true },
                    { "shotgun-shell-damage-6", "graphics/technology/shotgun-shell-damage.png", 0, null, true },
                    { "shotgun-shell-damage-7", "graphics/technology/shotgun-shell-damage.png", 0, "infinite", true },
                    { "shotgun-shell-speed-1", "graphics/technology/shotgun-shell-speed.png", 0, null, true },
                    { "character-logistic-slots-5", "graphics/technology/character-logistic-slots.png", 0, null, true },
                    { "shotgun-shell-speed-2", "graphics/technology/shotgun-shell-speed.png", 0, null, true },
                    { "shotgun-shell-speed-4", "graphics/technology/shotgun-shell-speed.png", 0, null, true },
                    { "shotgun-shell-speed-5", "graphics/technology/shotgun-shell-speed.png", 0, null, true },
                    { "shotgun-shell-speed-6", "graphics/technology/shotgun-shell-speed.png", 0, null, true },
                    { "laser-turret-damage-1", "graphics/technology/laser-turret-damage.png", 0, null, true },
                    { "laser-turret-damage-2", "graphics/technology/laser-turret-damage.png", 0, null, true },
                    { "laser-turret-damage-3", "graphics/technology/laser-turret-damage.png", 0, null, true },
                    { "laser-turret-damage-4", "graphics/technology/laser-turret-damage.png", 0, null, true },
                    { "laser-turret-damage-5", "graphics/technology/laser-turret-damage.png", 0, null, true },
                    { "laser-turret-damage-6", "graphics/technology/laser-turret-damage.png", 0, null, true },
                    { "laser-turret-damage-7", "graphics/technology/laser-turret-damage.png", 0, null, true },
                    { "laser-turret-damage-8", "graphics/technology/laser-turret-damage.png", 0, "infinite", true },
                    { "laser-turret-speed-1", "graphics/technology/laser-turret-speed.png", 0, null, true },
                    { "shotgun-shell-speed-3", "graphics/technology/shotgun-shell-speed.png", 0, null, true },
                    { "character-logistic-slots-4", "graphics/technology/character-logistic-slots.png", 0, null, true },
                    { "character-logistic-slots-3", "graphics/technology/character-logistic-slots.png", 0, null, true },
                    { "character-logistic-slots-2", "graphics/technology/character-logistic-slots.png", 0, null, true },
                    { "research-speed-5", "graphics/technology/research-speed.png", 0, null, true },
                    { "research-speed-6", "graphics/technology/research-speed.png", 0, null, true },
                    { "electric-energy-distribution-1", "graphics/technology/electric-energy-distribution.png", 0, null, false },
                    { "electric-energy-distribution-2", "graphics/technology/electric-energy-distribution.png", 0, null, false },
                    { "electric-energy-accumulators-1", "graphics/technology/electric-energy-acumulators.png", 0, null, false },
                    { "advanced-material-processing", "graphics/technology/advanced-material-processing.png", 0, null, false },
                    { "advanced-material-processing-2", "graphics/technology/advanced-material-processing.png", 0, null, false },
                    { "concrete", "graphics/technology/concrete.png", 0, null, false },
                    { "effect-transmission", "graphics/technology/effect-transmission.png", 0, null, false },
                    { "toolbelt", "graphics/technology/toolbelt.png", 0, null, false },
                    { "engine", "graphics/technology/engine.png", 0, null, false },
                    { "electric-engine", "graphics/technology/electric-engine.png", 0, null, false },
                    { "battery", "graphics/technology/battery.png", 0, null, false },
                    { "landfill", "graphics/technology/landfill.png", 0, null, false },
                    { "construction-robotics", "graphics/technology/construction-robotics.png", 0, null, false },
                    { "logistic-robotics", "graphics/technology/logistic-robotics.png", 0, null, false },
                    { "logistic-system", "graphics/technology/logistic-system.png", 0, null, false },
                    { "worker-robots-speed-1", "graphics/technology/worker-robots-speed.png", 0, null, true },
                    { "worker-robots-speed-2", "graphics/technology/worker-robots-speed.png", 0, null, true },
                    { "worker-robots-speed-3", "graphics/technology/worker-robots-speed.png", 0, null, true },
                    { "worker-robots-speed-4", "graphics/technology/worker-robots-speed.png", 0, null, true },
                    { "worker-robots-speed-5", "graphics/technology/worker-robots-speed.png", 0, null, true },
                    { "worker-robots-speed-6", "graphics/technology/worker-robots-speed.png", 0, "infinite", true },
                    { "worker-robots-storage-1", "graphics/technology/worker-robots-storage.png", 0, null, true },
                    { "worker-robots-storage-2", "graphics/technology/worker-robots-storage.png", 0, null, true },
                    { "worker-robots-storage-3", "graphics/technology/worker-robots-storage.png", 0, null, true },
                    { "character-logistic-slots-1", "graphics/technology/character-logistic-slots.png", 0, null, true },
                    { "steel-processing", "graphics/technology/steel-processing.png", 0, null, false },
                    { "grenade-damage-2", "graphics/technology/grenade-damage.png", 0, null, true }
                });

            migrationBuilder.InsertData(
                table: "Corporations",
                columns: new[] { "CorporationId", "CreatedByPlayerId", "Credits", "Description", "Name" },
                values: new object[] { new Guid("2785f191-b5b2-436e-9c6c-56ddd9c741df"), new Guid("00000000-0000-0000-0000-000000000001"), 0, null, "Test Corporation" });

            migrationBuilder.InsertData(
                table: "Factories",
                columns: new[] { "FactoryId", "CorporationId", "CurrentlyResearchingId", "LastSeen", "Name", "PlayersOnline", "Port" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("2785f191-b5b2-436e-9c6c-56ddd9c741df"), null, 0L, "Test Factory", 0, 0 });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "WarehouseId", "CorporationId" },
                values: new object[] { new Guid("c941a935-20c9-4f99-878a-b265e722f372"), new Guid("2785f191-b5b2-436e-9c6c-56ddd9c741df") });

            migrationBuilder.CreateIndex(
                name: "IX_CorporationInvitations_CorporationId",
                table: "CorporationInvitations",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporationInvitations_PlayerId",
                table: "CorporationInvitations",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Corporations_CreatedByPlayerId",
                table: "Corporations",
                column: "CreatedByPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Factories_CorporationId",
                table: "Factories",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_Factories_CurrentlyResearchingId",
                table: "Factories",
                column: "CurrentlyResearchingId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CorporationId",
                table: "Players",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_SteamId",
                table: "Players",
                column: "SteamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseInventories_ItemId",
                table: "WarehouseInventories",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseInventories_WarehouseId",
                table: "WarehouseInventories",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_CorporationId",
                table: "Warehouses",
                column: "CorporationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CorporationInvitations_Corporations_CorporationId",
                table: "CorporationInvitations",
                column: "CorporationId",
                principalTable: "Corporations",
                principalColumn: "CorporationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CorporationInvitations_Players_PlayerId",
                table: "CorporationInvitations",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Factories_Corporations_CorporationId",
                table: "Factories",
                column: "CorporationId",
                principalTable: "Corporations",
                principalColumn: "CorporationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Corporations_CorporationId",
                table: "Players",
                column: "CorporationId",
                principalTable: "Corporations",
                principalColumn: "CorporationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Corporations_CorporationId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "CorporationInvitations");

            migrationBuilder.DropTable(
                name: "Factories");

            migrationBuilder.DropTable(
                name: "WarehouseInventories");

            migrationBuilder.DropTable(
                name: "TechnologyLibrary");

            migrationBuilder.DropTable(
                name: "EntityLibrary");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Corporations");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
