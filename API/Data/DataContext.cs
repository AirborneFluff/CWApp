using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace API.Data
{
    public class DataContext : IdentityDbContext
        <AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole,
        IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Part> Parts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplySource> SupplySources { get; set; }
        public DbSet<SourcePrice> SourcePrices { get; set; }
        public DbSet<BOM> BOMs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Requisition> Requisitions { get; set; }
        public DbSet<StockLevelEntry> StockLevelEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .HasMany(au => au.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
                .HasMany(ar => ar.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            modelBuilder.Entity<Part>().HasIndex(x => x.PartCode).IsUnique();
            modelBuilder.Entity<Supplier>().HasIndex(x => x.NormalizedName).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(x => x.NormalizedName).IsUnique();

            modelBuilder.Entity<BOMEntry>()
                .HasKey(k => new { k.BOMId, k.PartId });

            modelBuilder.Entity<OutboundOrderItem>()
                .HasOne(o => o.Part)
                .WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StockLevelEntry>()
                .HasOne(s => s.User)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}