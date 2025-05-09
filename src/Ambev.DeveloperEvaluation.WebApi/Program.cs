using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
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
                builder.Services.AddSwaggerGen();

                // Add JWT authentication
                builder.Services.AddJwtAuthentication(builder.Configuration);

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