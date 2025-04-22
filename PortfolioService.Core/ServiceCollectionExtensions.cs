using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioService.Api.Options.CurrencyLayerApi;
using StockService;

namespace PortfolioService.Core
{
    /// <summary>
    /// Provides extension methods for registering core services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        private const string CURRENCY_LAYER_API_SECTION_NAME = "CurrencyLayerApi";

        /// <summary>
        /// Registers core services.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection RegisterCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging();

            services.Configure<CurrencyLayerApiOptions>(configuration.GetSection(CURRENCY_LAYER_API_SECTION_NAME));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

            services.AddSingleton<IStockService, StockService.StockService>();

            return services;
        }
    }
}
