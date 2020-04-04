using System.IO;
using FNO.Domain.Seed;
using Xunit;

namespace FNO.Domain.Tests.Seed
{
    public class SeedGeneratorTests
    {
        [Fact]
        public void GetAllEntities_WithExpectedFileContent_LoadsFluids()
        {
            // Arrange
            var fileContent = @"Script @__DataRawSerpent__/data-final-fixes.lua:1: {
  fluid = {
    [""crude-oil""] = {
      base_color = {
        b = 0,
        g = 0,
        r = 0
      },
      default_temperature = 25,
      flow_color = {
        b = 0.5,
        g = 0.5,
        r = 0.5
      },
      heat_capacity = ""0.1KJ"",
      icon = ""__base__/graphics/icons/fluid/crude-oil.png"",
      icon_mipmaps = 4,
      icon_size = 64,
      max_temperature = 100,
      name = ""crude-oil"",
      order = ""a[fluid]-b[crude-oil]"",
      type = ""fluid""
    }
  }
}";
            var filePath = Path.GetTempFileName();
            File.WriteAllText(filePath, fileContent);

            // Act
            var generator = new SeedGenerator(filePath);
            var fluids = generator.GetAllEntities();

            // Assert
            Assert.Collection(fluids, entity =>
            {
                Assert.True(entity.Fluid);
                Assert.Equal("fluid", entity.Type);
                Assert.Equal("crude-oil", entity.Name);
                Assert.Equal("graphics/icons/fluid/crude-oil.png", entity.Icon);
            });
        }

        [Fact]
        public void GetAllEntities_WithExpectedFileContent_LoadsItems()
        {
            // Arrange
            var fileContent = @"Script @__DataRawSerpent__/data-final-fixes.lua:1: {
  item = {
    accumulator = {
      icon = ""__base__/graphics/icons/accumulator.png"",
      icon_mipmaps = 4,
      icon_size = 64,
      name = ""accumulator"",
      order = ""e[accumulator]-a[accumulator]"",
      place_result = ""accumulator"",
      stack_size = 50,
      subgroup = ""energy"",
      type = ""item""
    }
  }
}";
            var filePath = Path.GetTempFileName();
            File.WriteAllText(filePath, fileContent);

            // Act
            var generator = new SeedGenerator(filePath);
            var fluids = generator.GetAllEntities();

            // Assert
            Assert.Collection(fluids, entity =>
            {
                Assert.False(entity.Fluid);
                Assert.Equal("item", entity.Type);
                Assert.Equal("accumulator", entity.Name);
                Assert.Equal("graphics/icons/accumulator.png", entity.Icon);
            });
        }
    }
}
