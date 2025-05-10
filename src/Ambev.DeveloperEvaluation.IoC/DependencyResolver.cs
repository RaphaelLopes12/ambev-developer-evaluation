using Ambev.DeveloperEvaluation.IoC.ModuleInitializers;
using Microsoft.AspNetCore.Builder;

namespace Ambev.DeveloperEvaluation.IoC
{
    /// <summary>
    /// Provides dependency registration for the application
    /// </summary>
    public static class DependencyResolver
    {
        /// <summary>
        /// Registers all dependencies by initializing all modules
        /// </summary>
        /// <param name="builder">Web application builder</param>
        public static void RegisterDependencies(this WebApplicationBuilder builder)
        {
            new ApplicationModuleInitializer().Initialize(builder);
            new InfrastructureModuleInitializer().Initialize(builder);
            new WebApiModuleInitializer().Initialize(builder);
        }
    }
}