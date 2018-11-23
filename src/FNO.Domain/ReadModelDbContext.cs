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
        // Invitations
        public DbSet<CorporationInvitation> CorporationInvitations {get;set;}
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
            var testCorpId = Guid.NewGuid();

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
                    Name = "TEST Corporation",
                    Description = "Please Ignore",
                    CorporationId = testCorpId,
                    CreatedByPlayerId = systemId,
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
                    Name = "TEST Factory",
                    FactoryId = systemId,
                    CorporationId = testCorpId,
                });

            modelBuilder.Entity<FactorioEntity>()
                .HasData(Seed.EntityLibrary.Data());

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
