using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioService.Core;
using PortfolioService.Core.Services.CurrencyLayerApi;
using PortfolioService.Domain.Repositories;
using PortfolioService.Infrastructure.Data;
using PortfolioService.Infrastructure.Data.Options;
using PortfolioService.Infrastructure.Repositories;
using PortfolioService.Infrastructure.Services.CurrencyLayerApi;

namespace PortfolioService.Infrastructure
{
    /// <summary>
    /// Provides extension methods for registering infrastructure services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        private const string MONGO_DATABASE_SECTION_NAME = "MongoDb";

        /// <summary>
        /// Registers infrastructure services for the booking.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterCore(configuration);

            services.Configure<MongoDbOptions>(configuration.GetSection(MONGO_DATABASE_SECTION_NAME));
            services.AddSingleton<MongoDbContext>();

            services.AddScoped<IPortfolioRepository, PortfolioRepository>();

            services.AddHttpClient<ICurrencyLayerApiClient, CurrencyLayerApiClient>();

            return services;
        }
    }
}
