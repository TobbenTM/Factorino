using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FNO.Domain
{
    public class ReadModelDbContext : DbContext
    {
        public DbSet<Deed> Deeds { get; set; }
        
        public DbSet<Factory> Factories { get; set; }
        
        public DbSet<WarehouseInventory> WarehouseInventories { get; set; }
        
        public DbSet<MarketOrder> Orders { get; set; }
        
        public DbSet<Shipment> Shipments { get; set; }
        
        public DbSet<Player> Players { get; set; }
        
        public DbSet<Corporation> Corporations { get; set; }
        
        public DbSet<CorporationInvitation> CorporationInvitations { get; set; }
        
        public DbSet<FactorioEntity> EntityLibrary { get; set; }
        
        public DbSet<FactorioTechnology> TechnologyLibrary { get; set; }

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

            modelBuilder.Entity<Player>()
                .HasIndex(p => p.SteamId)
                .IsUnique();

            modelBuilder.Entity<ConsumerState>()
                .HasKey(c => new { c.GroupId, c.Topic, c.Partition });
        }
    }
}
