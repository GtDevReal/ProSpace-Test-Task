using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Data.Entity;

namespace ProSpaceTest.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var connString = "Host=localhost;Port=5432;Database=TestTask;Username=postgres;Password=admin";

			optionsBuilder.UseNpgsql(connString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<CustomerEntity>().ToTable("Customers");
			modelBuilder.Entity<OrderEntity>().ToTable("Orders");
			modelBuilder.Entity<OrderItemEntity>().ToTable("OrderItems");
			modelBuilder.Entity<ProductEntity>().ToTable("Products");
		}
	}
}
