using System.Reflection;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ambev.DeveloperEvaluation.ORM;

/// <summary>
/// Default database context for PostgreSQL
/// </summary>
public class DefaultContext : DbContext
{
    /// <summary>
    /// Users table
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Sales table
    /// </summary>
    public DbSet<Sale> Sales { get; set; }

    /// <summary>
    /// Sale items table
    /// </summary>
    public DbSet<SaleItem> SaleItems { get; set; }

    /// <summary>
    /// Branches table
    /// </summary>
    public DbSet<Branch> Branches { get; set; }

    /// <summary>
    /// Products table
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Initializes a new instance of the database context
    /// </summary>
    /// <param name="options">Context options</param>
    public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
    {
    }

    /// <summary>
    /// Configures the entity model
    /// </summary>
    /// <param name="modelBuilder">Model builder instance</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                property.SetPropertyAccessMode(PropertyAccessMode.Field);
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}

/// <summary>
/// Design-time factory for creating DefaultContext instances
/// Used by EF Core tools like migrations
/// </summary>
public class DefaultContextFactory : IDesignTimeDbContextFactory<DefaultContext>
{
    /// <summary>
    /// Creates a new context instance for design-time operations
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>DefaultContext instance</returns>
    public DefaultContext CreateDbContext(string[] args)
    {
        // Build configuration
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<DefaultContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(
            connectionString,
            b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
        );

        return new DefaultContext(builder.Options);
    }
}