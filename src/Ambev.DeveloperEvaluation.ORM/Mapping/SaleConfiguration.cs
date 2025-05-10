using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for Sale entity
/// </summary>
public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    /// <summary>
    /// Configures the Sale entity mapping
    /// </summary>
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.Number)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(s => s.Date)
            .IsRequired();

        builder.Property(s => s.CustomerId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.CustomerName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.BranchId)
            .IsRequired();

        builder.Property(s => s.BranchName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired(false);

        builder
            .HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey("SaleId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
