using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Information("Starting web application");
                WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

                // Configure logging
                builder.AddDefaultLogging();

                // Add API explorer
                builder.Services.AddEndpointsApiExplorer();

                // Add health checks
                builder.AddBasicHealthChecks();

                // Add Swagger
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ambev.DeveloperEvaluation API", Version = "v1" });

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });
                });

                // Add JWT authentication
                builder.Services.AddJwtAuthentication(builder.Configuration);

                // Configure authorization policies based on existing UserRoles
                builder.Services.AddAuthorization(options =>
                {
                    // Policy for admin only
                    options.AddPolicy("RequireAdminRole", policy =>
                        policy.RequireRole(UserRole.Admin.ToString()));

                    // Policy for managers and admins
                    options.AddPolicy("RequireManagerRole", policy =>
                        policy.RequireRole(UserRole.Manager.ToString(), UserRole.Admin.ToString()));

                    // Policy for authenticated customers and above
                    options.AddPolicy("RequireCustomerRole", policy =>
                        policy.RequireRole(UserRole.Customer.ToString(), UserRole.Manager.ToString(), UserRole.Admin.ToString()));
                });

                builder.RegisterDependencies();

                // Configure AutoMapper
                builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

                // Configure MediatR
                builder.Services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssemblies(
                        typeof(ApplicationLayer).Assembly,
                        typeof(Program).Assembly
                    );
                });

                // Add validation behavior
                builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

                // Build the application
                var app = builder.Build();

                // Configure middlewares
                app.UseMiddleware<ValidationExceptionMiddleware>();

                // Configure Swagger in development
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                // Configure other middlewares
                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseBasicHealthChecks();

                // Map controllers
                app.MapControllers();

                // Run the application
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}