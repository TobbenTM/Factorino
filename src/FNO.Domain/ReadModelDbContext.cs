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

            modelBuilder.Entity<FactorioEntity>()
                .HasData(Seed.EntityLibrary.Data());

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
