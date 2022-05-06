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



/*
            modelBuilder.Entity<BOMEntry>()
                .HasForeignKey(e => e.BOMId)
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<BOMEntry>()
                .HasForeignKey(e => e.PartId)
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<BOM>()
                .HasForeignKey(b => b.ProductId)
                .WillCascadeOnDelete(false);

                /*

            modelBuilder.Entity<SourcePrice>()
                .HasForeignKey(p => p.SupplySourceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SupplySource>()
                .HasForeignKey(s => s.PartId)
                .WillCascadeOnDelete(false)
                .HasForeignKey(s => s.SupplierId)
                .WillCascadeOnDelete(false);

                */
            }
    }
}