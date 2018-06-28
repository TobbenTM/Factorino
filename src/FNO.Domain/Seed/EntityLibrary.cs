using FNO.Domain.Models;
using System.Collections.Generic;

namespace FNO.Domain.Seed
{
    internal static class EntityLibrary
    {
        internal static FactorioEntity[] Data()
        {
            var result = new List<FactorioEntity>();

            result.AddRange(Fluids());
            result.AddRange(Equipment());
            result.AddRange(MiningTools());
            result.AddRange(Items());
            result.AddRange(Modules());
            result.AddRange(Ammo());
            result.AddRange(Capsules());
            result.AddRange(Guns());
            result.AddRange(Armor());
            result.AddRange(Turrets());

            return result.ToArray();
        }

        private static FactorioEntity[] Fluids()
        {
            return new[]
            {
                new FactorioEntity {
                    Type = "fluid",
                    Name = "crude-oil",
                    Icon = "graphics/icons/fluid/crude-oil.png",
                    Fluid = true,
                },
                new FactorioEntity {
                    Type = "fluid",
                    Name = "heavy-oil",
                    Icon = "graphics/icons/fluid/heavy-oil.png",
                    Fluid = true,
                },
                new FactorioEntity {
                    Type = "fluid",
                    Name = "light-oil",
                    Icon = "graphics/icons/fluid/light-oil.png",
                    Fluid = true,
                },
                new FactorioEntity {
                    Type = "fluid",
                    Name = "petroleum-gas",
                    Icon = "graphics/icons/fluid/petroleum-gas.png",
                    Fluid = true,
                },
                new FactorioEntity {
                    Type = "fluid",
                    Name = "lubricant",
                    Icon = "graphics/icons/fluid/lubricant.png",
                    Fluid = true,
                },
                new FactorioEntity {
                    Type = "fluid",
                    Name = "sulfuric-acid",
                    Icon = "graphics/icons/fluid/sulfuric-acid.png",
                    Fluid = true,
                },
                new FactorioEntity {
                    Type = "fluid",
                    Name = "water",
                    Icon = "graphics/icons/fluid/water.png",
                    Fluid = true,
                },
                new FactorioEntity {
                    Type = "fluid",
                    Name = "steam",
                    Icon = "graphics/icons/fluid/steam.png",
                    Fluid = true,
                },
            };
        }

        private static FactorioEntity[] Equipment()
        {
            return new[]
            {
                new FactorioEntity {
                    Type = "item",
                    Name = "solar-panel-equipment",
                    Icon = "graphics/icons/solar-panel-equipment.png",
                    Subgroup = "equipment",
                    StackSize = 20
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "fusion-reactor-equipment",
                    Icon = "graphics/icons/fusion-reactor-equipment.png",
                    Subgroup = "equipment",
                    StackSize = 20
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "energy-shield-equipment",
                    Icon = "graphics/icons/energy-shield-equipment.png",
                    Subgroup = "equipment",
                    StackSize = 50,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "energy-shield-mk2-equipment",
                    Icon = "graphics/icons/energy-shield-mk2-equipment.png",
                    Subgroup = "equipment",
                    StackSize = 50,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "battery-equipment",
                    Icon = "graphics/icons/battery-equipment.png",
                    Subgroup = "equipment",
                    StackSize = 50,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "battery-mk2-equipment",
                    Icon = "graphics/icons/battery-mk2-equipment.png",
                    Subgroup = "equipment",
                    StackSize = 50,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "personal-laser-defense-equipment",
                    Icon = "graphics/icons/personal-laser-defense-equipment.png",
                    Subgroup = "equipment",
                    StackSize = 20
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "discharge-defense-equipment",
                    Icon = "graphics/icons/discharge-defense-equipment.png",
                    Subgroup = "equipment",
                    StackSize = 20
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "exoskeleton-equipment",
                    Icon = "graphics/icons/exoskeleton-equipment.png",
                    Subgroup = "equipment",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "personal-roboport-equipment",
                    Icon = "graphics/icons/personal-roboport-equipment.png",
                    Subgroup = "equipment",
                    StackSize = 5
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "personal-roboport-mk2-equipment",
                    Icon = "graphics/icons/personal-roboport-mk2-equipment.png",
                    Subgroup = "equipment",
                    StackSize = 5
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "night-vision-equipment",
                    Icon = "graphics/icons/night-vision-equipment.png",
                    Subgroup = "equipment",
                    StackSize = 20
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "belt-immunity-equipment",
                    Icon = "graphics/icons/belt-immunity-equipment.png",
                    Subgroup = "equipment",
                    StackSize = 1
                },
            };
        }

        private static FactorioEntity[] MiningTools()
        {
            return new[]
            {
                new FactorioEntity {
                    Type = "mining-tool",
                    Name = "iron-axe",
                    Icon = "graphics/icons/iron-axe.png",
                    Subgroup = "tool",
                    StackSize = 20
                },
                new FactorioEntity {
                    Type = "mining-tool",
                    Name = "steel-axe",
                    Icon = "graphics/icons/steel-axe.png",
                    Subgroup = "tool",
                    StackSize = 20
                },
            };
        }

        private static FactorioEntity[] Items()
        {
            return new[]
            {
                new FactorioEntity {
                    Type = "item",
                    Name = "stone-brick",
                    Icon = "graphics/icons/stone-brick.png",
                    Subgroup = "terrain",
                    StackSize = 100,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "raw-wood",
                    Icon = "graphics/icons/raw-wood.png",
                    Subgroup = "raw-resource",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "coal",
                    Icon = "graphics/icons/coal.png",
                    Subgroup = "raw-resource",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "stone",
                    Icon = "graphics/icons/stone.png",
                    Subgroup = "raw-resource",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "iron-ore",
                    Icon = "graphics/icons/iron-ore.png",
                    Subgroup = "raw-resource",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "copper-ore",
                    Icon = "graphics/icons/copper-ore.png",
                    Subgroup = "raw-resource",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "wood",
                    Icon = "graphics/icons/wood.png",
                    Subgroup = "raw-material",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "iron-plate",
                    Icon = "graphics/icons/iron-plate.png",
                    Subgroup = "raw-material",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "copper-plate",
                    Icon = "graphics/icons/copper-plate.png",
                    Subgroup = "raw-material",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "copper-cable",
                    Icon = "graphics/icons/copper-cable.png",
                    Subgroup = "intermediate-product",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "iron-stick",
                    Icon = "graphics/icons/iron-stick.png",
                    Subgroup = "intermediate-product",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "iron-gear-wheel",
                    Icon = "graphics/icons/iron-gear-wheel.png",
                    Subgroup = "intermediate-product",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "electronic-circuit",
                    Icon = "graphics/icons/electronic-circuit.png",
                    Subgroup = "intermediate-product",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "wooden-chest",
                    Icon = "graphics/icons/wooden-chest.png",
                    Subgroup = "storage",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "stone-furnace",
                    Icon = "graphics/icons/stone-furnace.png",
                    Subgroup = "smelting-machine",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "burner-mining-drill",
                    Icon = "graphics/icons/burner-mining-drill.png",
                    Subgroup = "extraction-machine",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "electric-mining-drill",
                    Icon = "graphics/icons/electric-mining-drill.png",
                    Subgroup = "extraction-machine",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "transport-belt",
                    Icon = "graphics/icons/transport-belt.png",
                    Subgroup = "belt",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "burner-inserter",
                    Icon = "graphics/icons/burner-inserter.png",
                    Subgroup = "inserter",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "inserter",
                    Icon = "graphics/icons/inserter.png",
                    Subgroup = "inserter",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "offshore-pump",
                    Icon = "graphics/icons/offshore-pump.png",
                    Subgroup = "extraction-machine",
                    StackSize = 20
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "pipe",
                    Icon = "graphics/icons/pipe.png",
                    Subgroup = "energy-pipe-distribution",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "boiler",
                    Icon = "graphics/icons/boiler.png",
                    Subgroup = "energy",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "steam-engine",
                    Icon = "graphics/icons/steam-engine.png",
                    Subgroup = "energy",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "small-electric-pole",
                    Icon = "graphics/icons/small-electric-pole.png",
                    Subgroup = "energy-pipe-distribution",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "radar",
                    Icon = "graphics/icons/radar.png",
                    Subgroup = "defensive-structure",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "computer",
                    Icon = "graphics/icons/computer.png",
                    Subgroup = "defensive-structure",
                    StackSize = 1
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "small-plane",
                    Icon = "graphics/icons/small-plane.png",
                    Subgroup = "transport",
                    StackSize = 1
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "small-lamp",
                    Icon = "graphics/icons/small-lamp.png",
                    Subgroup = "circuit-network",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "pipe-to-ground",
                    Icon = "graphics/icons/pipe-to-ground.png",
                    Subgroup = "energy-pipe-distribution",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "assembling-machine-1",
                    Icon = "graphics/icons/assembling-machine-1.png",
                    Subgroup = "production-machine",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "red-wire",
                    Icon = "graphics/icons/red-wire.png",
                    Subgroup = "circuit-network",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "green-wire",
                    Icon = "graphics/icons/green-wire.png",
                    Subgroup = "circuit-network",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "capsule",
                    Name = "raw-fish",
                    Icon = "graphics/icons/fish.png",
                    Subgroup = "raw-resource",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "repair-tool",
                    Name = "repair-pack",
                    Icon = "graphics/icons/repair-pack.png",
                    Subgroup = "tool",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "stone-wall",
                    Icon = "graphics/icons/stone-wall.png",
                    Subgroup = "defensive-structure",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "iron-chest",
                    Icon = "graphics/icons/iron-chest.png",
                    Subgroup = "storage",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "steel-chest",
                    Icon = "graphics/icons/steel-chest.png",
                    Subgroup = "storage",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "fast-transport-belt",
                    Icon = "graphics/icons/fast-transport-belt.png",
                    Subgroup = "belt",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "express-transport-belt",
                    Icon = "graphics/icons/express-transport-belt.png",
                    Subgroup = "belt",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "long-handed-inserter",
                    Icon = "graphics/icons/long-handed-inserter.png",
                    Subgroup = "inserter",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "fast-inserter",
                    Icon = "graphics/icons/fast-inserter.png",
                    Subgroup = "inserter",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "filter-inserter",
                    Icon = "graphics/icons/filter-inserter.png",
                    Subgroup = "inserter",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "stack-inserter",
                    Icon = "graphics/icons/stack-inserter.png",
                    Subgroup = "inserter",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "stack-filter-inserter",
                    Icon = "graphics/icons/stack-filter-inserter.png",
                    Subgroup = "inserter",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "assembling-machine-2",
                    Icon = "graphics/icons/assembling-machine-2.png",
                    Subgroup = "production-machine",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "assembling-machine-3",
                    Icon = "graphics/icons/assembling-machine-3.png",
                    Subgroup = "production-machine",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "solar-panel",
                    Icon = "graphics/icons/solar-panel.png",
                    Subgroup = "energy",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item-with-entity-data",
                    Name = "locomotive",
                    Icon = "graphics/icons/diesel-locomotive.png",
                    Subgroup = "transport",
                    StackSize = 5
                },
                new FactorioEntity {
                    Type = "item-with-entity-data",
                    Name = "cargo-wagon",
                    Icon = "graphics/icons/cargo-wagon.png",
                    Subgroup = "transport",
                    StackSize = 5
                },
                new FactorioEntity {
                    Type = "item-with-entity-data",
                    Name = "fluid-wagon",
                    Icon = "graphics/icons/fluid-wagon.png",
                    Subgroup = "transport",
                    StackSize = 5
                },
                new FactorioEntity {
                    Type = "item-with-entity-data",
                    Name = "artillery-wagon",
                    Icon = "graphics/icons/artillery-wagon.png",
                    Subgroup = "transport",
                    StackSize = 5
                },
                new FactorioEntity {
                    Type = "rail-planner",
                    Name = "rail",
                    Icon = "graphics/icons/rail.png",
                    Subgroup = "transport",
                    StackSize = 100,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "player-port",
                    Icon = "graphics/icons/player-port.png",
                    Subgroup = "defensive-structure",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "gate",
                    Icon = "graphics/icons/gate.png",
                    Subgroup = "defensive-structure",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item-with-entity-data",
                    Name = "car",
                    Icon = "graphics/icons/car.png",
                    Subgroup = "transport",
                    StackSize = 1
                },
                new FactorioEntity {
                    Type = "item-with-entity-data",
                    Name = "tank",
                    Icon = "graphics/icons/tank.png",
                    Subgroup = "transport",
                    StackSize = 1
                },
                new FactorioEntity {
                    Type = "tool",
                    Name = "science-pack-1",
                    Icon = "graphics/icons/science-pack-1.png",
                    Subgroup = "science-pack",
                    StackSize = 200,
                },
                new FactorioEntity {
                    Type = "tool",
                    Name = "science-pack-2",
                    Icon = "graphics/icons/science-pack-2.png",
                    Subgroup = "science-pack",
                    StackSize = 200,
                },
                new FactorioEntity {
                    Type = "tool",
                    Name = "science-pack-3",
                    Icon = "graphics/icons/science-pack-3.png",
                    Subgroup = "science-pack",
                    StackSize = 200,
                },
                new FactorioEntity {
                    Type = "tool",
                    Name = "military-science-pack",
                    Icon = "graphics/icons/military-science-pack.png",
                    Subgroup = "science-pack",
                    StackSize = 200,
                },
                new FactorioEntity {
                    Type = "tool",
                    Name = "production-science-pack",
                    Icon = "graphics/icons/production-science-pack.png",
                    Subgroup = "science-pack",
                    StackSize = 200,
                },
                new FactorioEntity {
                    Type = "tool",
                    Name = "high-tech-science-pack",
                    Icon = "graphics/icons/high-tech-science-pack.png",
                    Subgroup = "science-pack",
                    StackSize = 200,
                },
                new FactorioEntity {
                    Type = "tool",
                    Name = "space-science-pack",
                    Icon = "graphics/icons/space-science-pack.png",
                    Subgroup = "science-pack",
                    StackSize = 2000,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "lab",
                    Icon = "graphics/icons/lab.png",
                    Subgroup = "production-machine",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "train-stop",
                    Icon = "graphics/icons/train-stop.png",
                    Subgroup = "transport",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "rail-signal",
                    Icon = "graphics/icons/rail-signal.png",
                    Subgroup = "transport",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "rail-chain-signal",
                    Icon = "graphics/icons/rail-chain-signal.png",
                    Subgroup = "transport",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "steel-plate",
                    Icon = "graphics/icons/steel-plate.png",
                    Subgroup = "raw-material",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "underground-belt",
                    Icon = "graphics/icons/underground-belt.png",
                    Subgroup = "belt",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "fast-underground-belt",
                    Icon = "graphics/icons/fast-underground-belt.png",
                    Subgroup = "belt",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "express-underground-belt",
                    Icon = "graphics/icons/express-underground-belt.png",
                    Subgroup = "belt",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "splitter",
                    Icon = "graphics/icons/splitter.png",
                    Subgroup = "belt",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "fast-splitter",
                    Icon = "graphics/icons/fast-splitter.png",
                    Subgroup = "belt",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "express-splitter",
                    Icon = "graphics/icons/express-splitter.png",
                    Subgroup = "belt",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "loader",
                    Icon = "graphics/icons/loader.png",
                    Subgroup = "belt",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "fast-loader",
                    Icon = "graphics/icons/fast-loader.png",
                    Subgroup = "belt",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "express-loader",
                    Icon = "graphics/icons/express-loader.png",
                    Subgroup = "belt",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "advanced-circuit",
                    Icon = "graphics/icons/advanced-circuit.png",
                    Subgroup = "intermediate-product",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "processing-unit",
                    Icon = "graphics/icons/processing-unit.png",
                    Subgroup = "intermediate-product",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "logistic-robot",
                    Icon = "graphics/icons/logistic-robot.png",
                    Subgroup = "logistic-network",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "construction-robot",
                    Icon = "graphics/icons/construction-robot.png",
                    Subgroup = "logistic-network",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "logistic-chest-passive-provider",
                    Icon = "graphics/icons/logistic-chest-passive-provider.png",
                    Subgroup = "logistic-network",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "logistic-chest-active-provider",
                    Icon = "graphics/icons/logistic-chest-active-provider.png",
                    Subgroup = "logistic-network",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "logistic-chest-storage",
                    Icon = "graphics/icons/logistic-chest-storage.png",
                    Subgroup = "logistic-network",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "logistic-chest-buffer",
                    Icon = "graphics/icons/logistic-chest-buffer.png",
                    Subgroup = "logistic-network",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "logistic-chest-requester",
                    Icon = "graphics/icons/logistic-chest-requester.png",
                    Subgroup = "logistic-network",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "rocket-silo",
                    Icon = "graphics/icons/rocket-silo.png",
                    Subgroup = "defensive-structure",
                    StackSize = 1
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "roboport",
                    Icon = "graphics/icons/roboport.png",
                    Subgroup = "logistic-network",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "coin",
                    Icon = "graphics/icons/coin.png",
                    Subgroup = "science-pack",
                    StackSize = 100000
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "big-electric-pole",
                    Icon = "graphics/icons/big-electric-pole.png",
                    Subgroup = "energy-pipe-distribution",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "medium-electric-pole",
                    Icon = "graphics/icons/medium-electric-pole.png",
                    Subgroup = "energy-pipe-distribution",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "substation",
                    Icon = "graphics/icons/substation.png",
                    Subgroup = "energy-pipe-distribution",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "accumulator",
                    Icon = "graphics/icons/accumulator.png",
                    Subgroup = "energy",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "steel-furnace",
                    Icon = "graphics/icons/steel-furnace.png",
                    Subgroup = "smelting-machine",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "electric-furnace",
                    Icon = "graphics/icons/electric-furnace.png",
                    Subgroup = "smelting-machine",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "beacon",
                    Icon = "graphics/icons/beacon.png",
                    Subgroup = "module",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "storage-tank",
                    Icon = "graphics/icons/storage-tank.png",
                    Subgroup = "storage",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "pump",
                    Icon = "graphics/icons/pump.png",
                    Subgroup = "energy-pipe-distribution",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "pumpjack",
                    Icon = "graphics/icons/pumpjack.png",
                    Subgroup = "extraction-machine",
                    StackSize = 20
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "oil-refinery",
                    Icon = "graphics/icons/oil-refinery.png",
                    Subgroup = "production-machine",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "chemical-plant",
                    Icon = "graphics/icons/chemical-plant.png",
                    Subgroup = "production-machine",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "sulfur",
                    Icon = "graphics/icons/sulfur.png",
                    Subgroup = "raw-material",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "empty-barrel",
                    Icon = "graphics/icons/fluid/barreling/empty-barrel.png",
                    Subgroup = "intermediate-product",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "solid-fuel",
                    Icon = "graphics/icons/solid-fuel.png",
                    Subgroup = "raw-material",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "plastic-bar",
                    Icon = "graphics/icons/plastic-bar.png",
                    Subgroup = "raw-material",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "engine-unit",
                    Icon = "graphics/icons/engine-unit.png",
                    Subgroup = "intermediate-product",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "electric-engine-unit",
                    Icon = "graphics/icons/electric-engine-unit.png",
                    Subgroup = "intermediate-product",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "explosives",
                    Icon = "graphics/icons/explosives.png",
                    Subgroup = "raw-material",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "battery",
                    Icon = "graphics/icons/battery.png",
                    Subgroup = "raw-material",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "flying-robot-frame",
                    Icon = "graphics/icons/flying-robot-frame.png",
                    Subgroup = "intermediate-product",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "arithmetic-combinator",
                    Icon = "graphics/icons/arithmetic-combinator.png",
                    Subgroup = "circuit-network",
                    StackSize= 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "decider-combinator",
                    Icon = "graphics/icons/decider-combinator.png",
                    Subgroup = "circuit-network",
                    StackSize= 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "constant-combinator",
                    Icon = "graphics/icons/constant-combinator.png",
                    Subgroup = "circuit-network",
                    StackSize= 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "power-switch",
                    Icon = "graphics/icons/power-switch.png",
                    Subgroup = "circuit-network",
                    StackSize= 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "programmable-speaker",
                    Icon = "graphics/icons/programmable-speaker.png",
                    Subgroup = "circuit-network",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "low-density-structure",
                    Icon = "graphics/icons/rocket-structure.png",
                    Subgroup = "intermediate-product",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "rocket-fuel",
                    Icon = "graphics/icons/rocket-fuel.png",
                    Subgroup = "intermediate-product",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "nuclear-fuel",
                    Icon = "graphics/icons/nuclear-fuel.png",
                    Subgroup = "intermediate-product",
                    StackSize = 1
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "rocket-control-unit",
                    Icon = "graphics/icons/rocket-control-unit.png",
                    Subgroup = "intermediate-product",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "rocket-part",
                    Icon = "graphics/icons/rocket-part.png",
                    Subgroup = "intermediate-product",
                    StackSize = 5
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "satellite",
                    Icon = "graphics/icons/satellite.png",
                    Subgroup = "intermediate-product",
                    StackSize = 1,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "concrete",
                    Icon = "graphics/icons/concrete.png",
                    Subgroup = "terrain",
                    StackSize = 100,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "refined-concrete",
                    Icon = "graphics/icons/refined-concrete.png",
                    Subgroup = "terrain",
                    StackSize = 100,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "hazard-concrete",
                    Icon = "graphics/icons/hazard-concrete.png",
                    Subgroup = "terrain",
                    StackSize = 100,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "refined-hazard-concrete",
                    Icon = "graphics/icons/refined-hazard-concrete.png",
                    Subgroup = "terrain",
                    StackSize = 100,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "landfill",
                    Icon = "graphics/icons/landfill.png",
                    Subgroup = "terrain",
                    StackSize = 100,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "electric-energy-interface",
                    Icon = "graphics/icons/accumulator.png",
                    Subgroup = "energy",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "uranium-ore",
                    Icon = "graphics/icons/uranium-ore.png",
                    Subgroup = "raw-resource",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "nuclear-reactor",
                    Icon = "graphics/icons/nuclear-reactor.png",
                    Subgroup = "energy",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "uranium-235",
                    Icon = "graphics/icons/uranium-235.png",
                    Subgroup = "intermediate-product",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "uranium-238",
                    Icon = "graphics/icons/uranium-238.png",
                    Subgroup = "intermediate-product",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "centrifuge",
                    Icon = "graphics/icons/centrifuge.png",
                    Subgroup = "production-machine",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "uranium-fuel-cell",
                    Icon = "graphics/icons/uranium-fuel-cell.png",
                    Subgroup = "intermediate-product",
                    StackSize = 50,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "used-up-uranium-fuel-cell",
                    Icon = "graphics/icons/used-up-uranium-fuel-cell.png",
                    Subgroup = "intermediate-product",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "heat-exchanger",
                    Icon = "graphics/icons/heat-boiler.png",
                    Subgroup = "energy",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "steam-turbine",
                    Icon = "graphics/icons/steam-turbine.png",
                    Subgroup = "energy",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "heat-pipe",
                    Icon = "graphics/icons/heat-pipe.png",
                    Subgroup = "energy",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "simple-entity-with-force",
                    Icon = "graphics/icons/steel-chest.png",
                    Subgroup = "other",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "simple-entity-with-owner",
                    Icon = "graphics/icons/wooden-chest.png",
                    Subgroup = "other",
                    StackSize = 50
                },
                new FactorioEntity {
                    Type = "item-with-tags",
                    Name = "item-with-tags",
                    Icon = "graphics/icons/wooden-chest.png",
                    Subgroup = "other",
                    StackSize = 1
                },
                new FactorioEntity {
                    Type = "item-with-label",
                    Name = "item-with-label",
                    Icon = "graphics/icons/wooden-chest.png",
                    Subgroup = "other",
                    StackSize = 1
                },
                new FactorioEntity {
                    Type = "item-with-inventory",
                    Name = "item-with-inventory",
                    Icon = "graphics/icons/wooden-chest.png",
                    Subgroup = "other",
                    StackSize = 1,
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "infinity-chest",
                    Icon = "graphics/icons/infinity-chest.png",
                    Subgroup = "other",
                    StackSize = 10,
                },
            };
        }

        private static FactorioEntity[] Modules()
        {
            return new[]
            {
                new FactorioEntity {
                    Type = "module",
                    Name = "speed-module",
                    Icon = "graphics/icons/speed-module.png",
                    Subgroup = "module",
                    StackSize = 50,
                },
                new FactorioEntity {
                    Type = "module",
                    Name = "speed-module-2",
                    Icon = "graphics/icons/speed-module-2.png",
                    Subgroup = "module",
                    StackSize = 50,
                },
                new FactorioEntity {
                    Type = "module",
                    Name = "speed-module-3",
                    Icon = "graphics/icons/speed-module-3.png",
                    Subgroup = "module",
                    StackSize = 50,
                },
                new FactorioEntity {
                    Type = "module",
                    Name = "effectivity-module",
                    Icon = "graphics/icons/effectivity-module.png",
                    Subgroup = "module",
                    StackSize = 50,
                },
                new FactorioEntity {
                    Type = "module",
                    Name = "effectivity-module-2",
                    Icon = "graphics/icons/effectivity-module-2.png",
                    Subgroup = "module",
                    StackSize = 50,
                },
                new FactorioEntity {
                    Type = "module",
                    Name = "effectivity-module-3",
                    Icon = "graphics/icons/effectivity-module-3.png",
                    Subgroup = "module",
                    StackSize = 50,
                },
                new FactorioEntity {
                    Type = "module",
                    Name = "productivity-module",
                    Icon = "graphics/icons/productivity-module.png",
                    Subgroup = "module",
                    StackSize = 50,
                },
                new FactorioEntity {
                    Type = "module",
                    Name = "productivity-module-2",
                    Icon = "graphics/icons/productivity-module-2.png",
                    Subgroup = "module",
                    StackSize = 50,
                },
                new FactorioEntity {
                    Type = "module",
                    Name = "productivity-module-3",
                    Icon = "graphics/icons/productivity-module-3.png",
                    Subgroup = "module",
                    StackSize = 50,
                },
            };
        }

        private static FactorioEntity[] Ammo()
        {
            return new[]
            {
                new FactorioEntity {
                    Type = "ammo",
                    Name = "firearm-magazine",
                    Icon = "graphics/icons/firearm-magazine.png",
                    Subgroup = "ammo",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "ammo",
                    Name = "piercing-rounds-magazine",
                    Icon = "graphics/icons/piercing-rounds-magazine.png",
                    Subgroup = "ammo",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "ammo",
                    Name = "uranium-rounds-magazine",
                    Icon = "graphics/icons/uranium-rounds-magazine.png",
                    Subgroup = "ammo",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "ammo",
                    Name = "flamethrower-ammo",
                    Icon = "graphics/icons/flamethrower-ammo.png",
                    Subgroup = "ammo",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "ammo",
                    Name = "rocket",
                    Icon = "graphics/icons/rocket.png",
                    Subgroup = "ammo",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "ammo",
                    Name = "explosive-rocket",
                    Icon = "graphics/icons/explosive-rocket.png",
                    Subgroup = "ammo",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "ammo",
                    Name = "atomic-bomb",
                    Icon = "graphics/icons/atomic-bomb.png",
                    Subgroup = "ammo",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "ammo",
                    Name = "shotgun-shell",
                    Icon = "graphics/icons/shotgun-shell.png",
                    Subgroup = "ammo",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "ammo",
                    Name = "piercing-shotgun-shell",
                    Icon = "graphics/icons/piercing-shotgun-shell.png",
                    Subgroup = "ammo",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "ammo",
                    Name = "railgun-dart",
                    Icon = "graphics/icons/railgun-ammo.png",
                    Subgroup = "ammo",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "ammo",
                    Name = "cannon-shell",
                    Icon = "graphics/icons/cannon-shell.png",
                    Subgroup = "ammo",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "ammo",
                    Name = "explosive-cannon-shell",
                    Icon = "graphics/icons/explosive-cannon-shell.png",
                    Subgroup = "ammo",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "ammo",
                    Name = "uranium-cannon-shell",
                    Icon = "graphics/icons/uranium-cannon-shell.png",
                    Subgroup = "ammo",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "ammo",
                    Name = "explosive-uranium-cannon-shell",
                    Icon = "graphics/icons/explosive-uranium-cannon-shell.png",
                    Subgroup = "ammo",
                    StackSize = 200
                },
                new FactorioEntity {
                    Type = "ammo",
                    Name = "artillery-shell",
                    Icon = "graphics/icons/artillery-shell.png",
                    Subgroup = "ammo",
                    StackSize = 1
                },
            };
        }

        private static FactorioEntity[] Capsules()
        {
            return new[]
            {
                new FactorioEntity {
                    Type = "capsule",
                    Name = "grenade",
                    Icon = "graphics/icons/grenade.png",
                    Subgroup = "capsule",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "capsule",
                    Name = "cluster-grenade",
                    Icon = "graphics/icons/cluster-grenade.png",
                    Subgroup = "capsule",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "capsule",
                    Name = "poison-capsule",
                    Icon = "graphics/icons/poison-capsule.png",
                    Subgroup = "capsule",
                    StackSize = 100
                  },
                  new FactorioEntity {
                    Type = "capsule",
                    Name = "slowdown-capsule",
                    Icon = "graphics/icons/slowdown-capsule.png",
                    Subgroup = "capsule",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "capsule",
                    Name = "defender-capsule",
                    Icon = "graphics/icons/defender.png",
                    Subgroup = "capsule",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "capsule",
                    Name = "distractor-capsule",
                    Icon = "graphics/icons/distractor.png",
                    Subgroup = "capsule",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "capsule",
                    Name = "destroyer-capsule",
                    Icon = "graphics/icons/destroyer.png",
                    Subgroup = "capsule",
                    StackSize = 100
                },
                new FactorioEntity {
                    Type = "capsule",
                    Name = "discharge-defense-remote",
                    Icon = "graphics/equipment/discharge-defense-equipment-ability.png",
                    Subgroup = "capsule",
                    StackSize = 1
                },
                new FactorioEntity {
                    Type = "capsule",
                    Name = "cliff-explosives",
                    Icon = "graphics/icons/cliff-explosives.png",
                    Subgroup = "terrain",
                    StackSize = 20
                },
                new FactorioEntity {
                    Type = "capsule",
                    Name = "artillery-targeting-remote",
                    Icon = "graphics/icons/artillery-targeting-remote.png",
                    Subgroup = "capsule",
                    StackSize = 1
                },
            };
        }

        private static FactorioEntity[] Guns()
        {
            return new[]
            {
                new FactorioEntity {
                    Type = "gun",
                    Name = "pistol",
                    Icon = "graphics/icons/pistol.png",
                    Subgroup = "gun",
                    StackSize = 5
                },
                new FactorioEntity {
                    Type = "gun",
                    Name = "submachine-gun",
                    Icon = "graphics/icons/submachine-gun.png",
                    Subgroup = "gun",
                    StackSize = 5
                },
                new FactorioEntity {
                    Type = "gun",
                    Name = "flamethrower",
                    Icon = "graphics/icons/flamethrower.png",
                    Subgroup = "gun",
                    StackSize = 5
                },
                new FactorioEntity {
                    Type = "gun",
                    Name = "vehicle-machine-gun",
                    Icon = "graphics/icons/submachine-gun.png",
                    Subgroup = "gun",
                    StackSize = 1
                },
                new FactorioEntity {
                    Type = "gun",
                    Name = "tank-machine-gun",
                    Icon = "graphics/icons/submachine-gun.png",
                    Subgroup = "gun",
                    StackSize = 1
                },
                new FactorioEntity {
                    Type = "gun",
                    Name = "tank-flamethrower",
                    Icon = "graphics/icons/flamethrower.png",
                    Subgroup = "gun",
                    StackSize = 1
                },
                new FactorioEntity {
                    Type = "item",
                    Name = "land-mine",
                    Icon = "graphics/icons/land-mine.png",
                    Subgroup = "gun",
                    StackSize = 100,
                },
                new FactorioEntity {
                    Type = "gun",
                    Name = "rocket-launcher",
                    Icon = "graphics/icons/rocket-launcher.png",
                    Subgroup = "gun",
                    StackSize = 5
                },
                new FactorioEntity {
                    Type = "gun",
                    Name = "shotgun",
                    Icon = "graphics/icons/shotgun.png",
                    Subgroup = "gun",
                    StackSize = 5
                },
                new FactorioEntity {
                    Type = "gun",
                    Name = "combat-shotgun",
                    Icon = "graphics/icons/combat-shotgun.png",
                    Subgroup = "gun",
                    StackSize = 5
                },
                new FactorioEntity {
                    Type = "gun",
                    Name = "railgun",
                    Icon = "graphics/icons/railgun.png",
                    Subgroup = "gun",
                    StackSize = 5
                },
                new FactorioEntity {
                    Type = "gun",
                    Name = "tank-cannon",
                    Icon = "graphics/icons/tank-cannon.png",
                    Subgroup = "gun",
                },
                new FactorioEntity {
                    Type = "gun",
                    Name = "artillery-wagon-cannon",
                    Icon = "graphics/icons/tank-cannon.png",
                    Subgroup = "gun",
                },
            };
        }

        private static FactorioEntity[] Armor()
        {
            return new[]
            {
                new FactorioEntity {
                    Type = "armor",
                    Name = "light-armor",
                    Icon = "graphics/icons/light-armor.png",
                    Subgroup = "armor",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "armor",
                    Name = "heavy-armor",
                    Icon = "graphics/icons/heavy-armor.png",
                    Subgroup = "armor",
                    StackSize = 10
                },
                new FactorioEntity {
                    Type = "armor",
                    Name = "modular-armor",
                    Icon = "graphics/icons/modular-armor.png",
                    Subgroup = "armor",
                    StackSize = 1,
                },
                new FactorioEntity {
                    Type = "armor",
                    Name = "power-armor",
                    Icon = "graphics/icons/power-armor.png",
                    Subgroup = "armor",
                    StackSize = 1,
                },
                new FactorioEntity {
                    Type = "armor",
                    Name = "power-armor-mk2",
                    Icon = "graphics/icons/power-armor-mk2.png",
                    Subgroup = "armor",
                    StackSize = 1,
                },
            };
        }

        private static FactorioEntity[] Turrets()
        {
            return new[]
            {
                new FactorioEntity
                {
                    Type = "item",
                    Name = "gun-turret",
                    Icon = "graphics/icons/gun-turret.png",
                    Subgroup = "defensive-structure",
                    StackSize = 50,
                },
                new FactorioEntity
                {
                    Type = "item",
                    Name = "laser-turret",
                    Icon = "graphics/icons/laser-turret.png",
                    Subgroup = "defensive-structure",
                    StackSize = 50,
                },
                new FactorioEntity
                {
                    Type = "item",
                    Name = "flamethrower-turret",
                    Icon = "graphics/icons/flamethrower-turret.png",
                    Subgroup = "defensive-structure",
                    StackSize = 50,
                },
                new FactorioEntity
                {
                    Type = "item",
                    Name = "artillery-turret",
                    Icon = "graphics/icons/artillery-turret.png",
                    Subgroup = "defensive-structure",
                    StackSize = 10,
                },
            };
        }
    }
}
