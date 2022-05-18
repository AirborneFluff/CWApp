namespace API.Data
{
    public class DataContext : DbContext
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Part>().HasIndex(x => x.PartCode).IsUnique();
            modelBuilder.Entity<Supplier>().HasIndex(x => x.NormalizedName).IsUnique();

        }
    }
}