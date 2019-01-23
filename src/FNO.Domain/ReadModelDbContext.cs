using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

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
        public DbSet<MarketOrder> MarketOrders { get; set; }
        // MarketTransactions
        // Players
        public DbSet<Player> Players { get; set; }
        // Corporations
        public DbSet<Corporation> Corporations { get; set; }
        // CorporationResearch
        // Invitations
        public DbSet<CorporationInvitation> CorporationInvitations { get; set; }
        // EntityLibrary
        public DbSet<FactorioEntity> EntityLibrary { get; set; }
        // ReseachLibrary
        public DbSet<FactorioTechnology> TechnologyLibrary { get; set; }

        // [Meta] ConsumerState
        public DbSet<ConsumerState> ConsumerStates { get; set; }

        public ReadModelDbContext(DbContextOptions<ReadModelDbContext> options) : base(options)
        {
        }

        public static ReadModelDbContext CreateContext(IConfiguration config)
        {
            return new ReadModelDbContext(CreateOptions(config));
        }

        public static DbContextOptions<ReadModelDbContext> CreateOptions(IConfiguration config)
        {
            var builder = new DbContextOptionsBuilder<ReadModelDbContext>();
            ConfigureBuilder(builder, config);
            return builder.Options;
        }

        public static DbContextOptionsBuilder ConfigureBuilder(DbContextOptionsBuilder builder, IConfiguration config, bool disableTracking = false)
        {
            var connectionString = config.GetConnectionString("ReadModel");
            builder.UseNpgsql(connectionString);
            if (disableTracking)
            {
                builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
            return builder;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var systemId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var bankCorpId = Guid.Parse("00000000-0000-0000-0000-000000000002");

            modelBuilder.Entity<Warehouse>()
                .HasData(new Warehouse
                {
                    OwnerId = systemId,
                    WarehouseId = systemId,
                });

            modelBuilder.Entity<Player>()
                .HasData(new Player
                {
                    Name = "<system>",
                    SteamId = "<system>",
                    PlayerId = systemId,
                });

            modelBuilder.Entity<Corporation>()
                .HasData(new Corporation
                {
                    Name = "Bank of Nauvis",
                    Description = "We make living life easy!™",
                    CorporationId = bankCorpId,
                    CreatedByPlayerId = systemId,
                    Credits = -1,
                });

            //modelBuilder.Entity<Warehouse>()
            //    .HasData(new Warehouse
            //    {
            //        WarehouseId = Guid.NewGuid(),
            //        CorporationId = bankCorpId,
            //    });

            modelBuilder.Entity<Factory>()
                .HasData(new Factory
                {
                    Name = "Bank of Nauvis - HQ",
                    FactoryId = systemId,
                    OwnerId = systemId,
                });

            // Seeding the market with infinite (bad) buy orders
            var entities = Seed.EntityLibrary.Data();
            var orderIds = Enumerable.Range(0, entities.Length)
                .Select(n => Guid.Parse($"00000000-0000-0000-1111-{n.ToString().PadLeft(12, '0')}"))
                .ToArray();
            var orders = Enumerable.Range(0, entities.Length)
                .Select(i => new MarketOrder
                {
                    OrderId = orderIds[i],
                    ItemId = entities[i].Name,
                    CorporationId = bankCorpId,
                    OrderType = OrderType.Buy,
                    Price = 1,
                    Quantity = -1,
                }).ToArray();

            modelBuilder.Entity<MarketOrder>()
                .HasData(orders);

            modelBuilder.Entity<FactorioEntity>()
                .HasData(entities);

            modelBuilder.Entity<FactorioTechnology>()
                .HasData(Seed.TechnologyLibrary.Data());

            modelBuilder.Entity<Player>()
                .HasIndex(p => p.SteamId)
                .IsUnique();

            modelBuilder.Entity<ConsumerState>()
                .HasKey(c => new { c.GroupId, c.Topic, c.Partition });
        }
    }
}
