using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace FNO.Domain
{
    public class ReadModelDbContext : DbContext
    {
        // Factories
        public DbSet<Factory> Factories { get; set; }
        // FactoryTrainstops
        // Warehouses
        public DbSet<Warehouse> Warehouses { get; set; }
        // WarehouseRules
        // WarehouseInventory
        public DbSet<WarehouseInventory> WarehouseInventories { get; set; }
        // WarehouseTransactions
        // MarketOrders
        // MarketTransactions
        // Players
        public DbSet<Player> Players { get; set; }
        // Corporations
        public DbSet<Corporation> Corporations { get; set; }
        // CorporationResearch
        // EntityLibrary
        public DbSet<FactorioEntity> EntityLibrary { get; set; }
        // ReseachLibrary
        public DbSet<FactorioTechnology> TechnologyLibrary { get; set; }

        public ReadModelDbContext(DbContextOptions<ReadModelDbContext> options) : base(options)
        {
        }

        public static ReadModelDbContext CreateContext(IConfiguration config)
        {
            var builder = new DbContextOptionsBuilder<ReadModelDbContext>();
            var connectionString = config.GetConnectionString("ReadModel");
            builder.UseNpgsql(connectionString);
            return new ReadModelDbContext(builder.Options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var testCorpId = Guid.NewGuid();
            var testFactoryId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            modelBuilder.Entity<Corporation>()
                .HasData(new Corporation
                {
                    Name = "Test Corporation",
                    CorporationId = testCorpId,
                });

            modelBuilder.Entity<Warehouse>()
                .HasData(new Warehouse
                {
                    WarehouseId = Guid.NewGuid(),
                    CorporationId = testCorpId,
                });

            modelBuilder.Entity<Factory>()
                .HasData(new Factory
                {
                    Name = "Test Factory",
                    FactoryId = testFactoryId,
                    CorporationId = testCorpId,
                });

            modelBuilder.Entity<FactorioEntity>()
                .HasData(Seed.EntityLibrary.Data());

            modelBuilder.Entity<FactorioTechnology>()
                .HasData(Seed.TechnologyLibrary.Data());
        }
    }
}
