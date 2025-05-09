using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.MongoDB;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

/// <summary>
/// Initializes infrastructure services and repositories
/// </summary>
public class InfrastructureModuleInitializer : IModuleInitializer
{
    /// <summary>
    /// Initializes infrastructure dependencies
    /// </summary>
    /// <param name="builder">Web application builder</param>
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());

        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
            )
        );

        builder.Services.Configure<MongoDbSettings>(
            builder.Configuration.GetSection("MongoDbSettings"));
        builder.Services.AddSingleton<MongoDbContext>();

        RegisterRepositories(builder.Services);
    }

    private void RegisterRepositories(IServiceCollection services)
    {
        // PostgreSQL repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBranchRepository, BranchRepository>();

        // MongoDB repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();
    }
}