using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductAPI.Domain;

namespace ProductAPI.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entity)
    {
        entity.HasKey(p => p.Id);

        entity.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        entity.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        entity.Property(p => p.Stock)
            .IsRequired();

        entity.Property(p => p.IsAvailable)
            .HasDefaultValue(true)
            .IsRequired();

        entity.HasMany(p => p.OrderItems)
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}