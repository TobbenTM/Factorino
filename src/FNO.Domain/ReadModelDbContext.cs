using System;
using System.Linq;
using FNO.Domain.Models;
using FNO.Domain.Models.Market;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FNO.Domain
{
    public class ReadModelDbContext : DbContext
    {
        // Factories
        public DbSet<Factory> Factories { get; set; }
        // FactoryLocations
        public DbSet<FactoryLocation> FactoryLocations { get; set; }
        // Warehouses
        // public DbSet<Warehouse> Warehouses { get; set; }
        // WarehouseInventory
        public DbSet<WarehouseInventory> WarehouseInventories { get; set; }
        // MarketOrders
        public DbSet<MarketOrder> Orders { get; set; }
        // Shipments
        public DbSet<Shipment> Shipments { get; set; }
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

            // modelBuilder.Entity<Warehouse>()
            //     .HasData(new Warehouse
            //     {
            //         OwnerId = systemId,
            //         WarehouseId = systemId,
            //     });

            modelBuilder.Entity<Player>()
                .HasData(new Player
                {
                    Name = "<system>",
                    SteamId = "<system>",
                    PlayerId = systemId,
                    Credits = -1,
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
                    LocationId = Guid.Parse("00000000-0000-10CA-7104-000000000002"),
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
                    OwnerId = systemId,
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

            // FactorioLocationResource is the many-to-many from location to entities
            modelBuilder.Entity<FactoryLocationResource>()
                .HasKey(x => new { x.EntityId, x.LocationId });

            modelBuilder.Entity<FactoryLocationResource>()
                .HasOne(r => r.Location)
                .WithMany(l => l.Resources);

            // Don't really want a navigation property on the FactorioEntity
            modelBuilder.Entity<FactoryLocationResource>()
                .HasOne(r => r.Entity)
                .WithMany();

            var locationData = Seed.FactoryLocations.Data();

            modelBuilder.Entity<FactoryLocation>()
                .HasData(locationData.Select(d => d.location).ToArray());

            var locationResources = locationData.SelectMany(d => d.resources.Select(r =>
            {
                var intermediate = r;
                intermediate.LocationId = d.location.LocationId;
                return intermediate;
            })).ToArray();

            modelBuilder.Entity<FactoryLocationResource>()
                .HasData(locationResources);

            modelBuilder.Entity<Player>()
                .HasIndex(p => p.SteamId)
                .IsUnique();

            modelBuilder.Entity<ConsumerState>()
                .HasKey(c => new { c.GroupId, c.Topic, c.Partition });
        }
    }
}
