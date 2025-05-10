using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for Branch entity
/// </summary>
public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    /// <summary>
    /// Configures the Branch entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.State)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(b => b.ZipCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(b => b.Phone)
            .HasMaxLength(20);

        builder.Property(b => b.Email)
            .HasMaxLength(100);

        builder.Property(b => b.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(b => b.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");
    }
}
