using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for SaleItem entity
/// </summary>
public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    /// <summary>
    /// Configures the SaleItem entity mapping
    /// </summary>
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(i => i.ProductId)
            .IsRequired();

        builder.Property(i => i.ProductName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Quantity)
            .IsRequired();

        builder.Property(i => i.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.Discount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.IsCancelled)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(i => i.ProductId).HasField("_productId");
        builder.Property(i => i.ProductName).HasField("_productName");
        builder.Property(i => i.Quantity).HasField("_quantity");
        builder.Property(i => i.UnitPrice).HasField("_unitPrice");
        builder.Property(i => i.Discount).HasField("_discount");
        builder.Property(i => i.IsCancelled).HasField("_isCancelled");

        builder.Ignore(i => i.Total);

        builder.Property<Guid>("SaleId");
    }
}
