using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers
{
    /// <summary>
    /// Initializes WebAPI-specific dependencies
    /// </summary>
    public class WebApiModuleInitializer : IModuleInitializer
    {
        /// <summary>
        /// Initializes WebAPI dependencies
        /// </summary>
        /// <param name="builder">Web application builder</param>
        public void Initialize(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services.AddHealthChecks();
        }
    }
}