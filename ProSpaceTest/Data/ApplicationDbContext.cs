using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Data.Entity;

namespace ProSpaceTest.Data
{
	public class ApplicationDbContext : IdentityDbContext<UsersEntity>
    {
		public DbSet<ProductEntity> Products { get; set; }
		public DbSet<OrderEntity> Orders { get; set; }
		public DbSet<OrderItemEntity> OrderItems { get; set; }
		public DbSet<CustomerEntity> Customers { get; set; }
		public DbSet<UsersEntity> AspNetUsers {  get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<UsersEntity>(entity =>
			{
				entity.HasOne(u => u.Customer)
					.WithMany(c => c.Users)
					.HasForeignKey(u => u.CustomerId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<CustomerEntity>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Name).IsRequired();
				entity.Property(e => e.Code).IsRequired();
			});

			modelBuilder.Entity<OrderEntity>(entity =>
			{
				entity.HasKey(o => o.Id);
				entity.Property(o => o.OrderDate).IsRequired();
				entity.Property(o => o.OrderNumber).ValueGeneratedOnAdd();

				entity.HasOne(o => o.Customer)
					.WithMany(c => c.Orders)
					.HasForeignKey(o => o.CustomerId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<OrderItemEntity>(entity =>
			{
				entity.HasKey(oi => oi.Id);
				entity.Property(oi => oi.ItemId).IsRequired();
				entity.Property(oi => oi.ItemsCount).IsRequired();
				entity.Property(oi => oi.ItemPrice).IsRequired();

				entity.HasOne(oi => oi.Order)
					.WithMany(o => o.Items)
					.HasForeignKey(oi => oi.OrderId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<ProductEntity>(entity =>
			{
				entity.HasKey(p => p.Id);

				entity.Property(p => p.Code).IsRequired();
				entity.Property(p => p.Category).HasMaxLength(30);
			});
		}
	}
}
