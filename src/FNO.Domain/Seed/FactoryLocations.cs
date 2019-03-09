using FNO.Domain.Models;
using System;
using System.Linq;

namespace FNO.Domain.Seed
{
    internal static class FactoryLocations
    {
        internal static (FactoryLocation location, FactoryLocationResource[] resources)[] Data()
        {
            return new (FactoryLocation, FactoryLocationResource[])[]
            {
                (new FactoryLocation
                {
                    LocationId = Guid.Parse("00000000-0000-10CA-7104-000000000001"),
                    Name = "Northern Plains",
                    Description = "Sheep, ice and oil. Welcome to the northern plains.",
                    Latitude = 69.012539d,
                    Longitude = 23.040888d,
                    Seed = "nordics_1",
                },
                new[]
                {
                    "coal",
                    "stone",
                    "iron-ore",
                    "copper-ore",
                    "water",
                    "crude-oil",
                }.Select(r => new FactoryLocationResource { EntityId = r }).ToArray()),
                (new FactoryLocation
                {
                    LocationId = Guid.Parse("00000000-0000-10CA-7104-000000000002"),
                    Name = "Central Oasis",
                    Description = "The navel of the world, this is where the big boys meet up.",
                    Latitude = 49.523714d,
                    Longitude = 0.168514d,
                    Seed = "central_1",
                },
                new[]
                {
                    "coal",
                    "water",
                    "uranium-ore",
                }.Select(r => new FactoryLocationResource { EntityId = r }).ToArray()),
                (new FactoryLocation
                {
                    LocationId = Guid.Parse("00000000-0000-10CA-7104-000000000003"),
                    Name = "Western Sahara",
                    Description = "Rich in metals and sand, and little else.",
                    Latitude = 22.768025d,
                    Longitude = -12.608501d,
                    Seed = "sahara_1",
                },
                new[]
                {
                    "coal",
                    "iron-ore",
                    "copper-ore",
                    "crude-oil",
                }.Select(r => new FactoryLocationResource { EntityId = r }).ToArray()),
            };
        }
    }
}
