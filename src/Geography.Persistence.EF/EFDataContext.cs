using Geography.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Geography.Entities;
using OnlineShop.Entity;

namespace Geography.Persistence.EF
{
    public class EFDataContext : DbContext
    {
        public DbSet<City> Cities { get; protected set; }
        public DbSet<Province> Provinces { get; protected set; }
        public DbSet<ProductCategory> ProductCategories { get; protected set; }
        public DbSet<Product> Products { get; protected set; }

        public EFDataContext(string connectionString)
            : this(new DbContextOptionsBuilder<EFDataContext>().UseSqlite(connectionString).Options)
        {
        }

        private EFDataContext(DbContextOptions<EFDataContext> options)
            : this((DbContextOptions)options)
        {
        }

        protected EFDataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAssemblyConfigurations(typeof(EFDataContext).Assembly);
        }

        public override ChangeTracker ChangeTracker
        {
            get
            {
                var tracker = base.ChangeTracker;
                tracker.LazyLoadingEnabled = false;
                tracker.AutoDetectChangesEnabled = true;
                tracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                return tracker;
            }
        }
    }
}
