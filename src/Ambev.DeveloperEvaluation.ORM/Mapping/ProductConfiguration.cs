using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for Product entity
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configures the Product entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Image)
            .HasMaxLength(500);

        builder.Property(p => p.StockQuantity)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.RatingRate)
            .HasColumnType("decimal(3,1)")
            .HasDefaultValue(0);

        builder.Property(p => p.RatingCount)
            .HasDefaultValue(0);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(p => p.Title).HasField("_title");
        builder.Property(p => p.Price).HasField("_price");
        builder.Property(p => p.Description).HasField("_description");
        builder.Property(p => p.Category).HasField("_category");
        builder.Property(p => p.Image).HasField("_image");
        builder.Property(p => p.StockQuantity).HasField("_stockQuantity");
        builder.Property(p => p.RatingRate).HasField("_ratingRate");
        builder.Property(p => p.RatingCount).HasField("_ratingCount");
        builder.Property(p => p.CreatedAt).HasField("_createdAt");
        builder.Property(p => p.UpdatedAt).HasField("_updatedAt");
    }
}
