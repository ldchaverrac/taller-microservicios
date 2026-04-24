using Microsoft.EntityFrameworkCore;
using OrdersService.Models;

namespace OrdersService.Persistence;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options) {
  public DbSet<Order> Orders { get; set; }
  public DbSet<OrderItem> OrderItems { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<Order>(entity => {
      entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
      entity.HasMany(e => e.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<OrderItem>(entity => { entity.HasIndex(e => e.OrderId); });
  }
}