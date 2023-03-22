using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TwT.Modules.Base.Processes
{
	/// <summary>
	/// Process that can be used to configure ApplicationInsights
	/// </summary>
	internal class ApplicationInsightsProcess
	{
		/// <summary>
		/// True when ApplicationInsights.ConnectionString exists in the configuration
		/// </summary>
		private readonly bool _connectionStringIsPresent;

		/// <summary>
		/// Process that can be used to configure ApplicationInsights
		/// </summary>
		/// <param name="configuration">Configuration that can be used to configure ApplicationInsights</param>
		/// <exception cref="ArgumentNullException">Configuration is null</exception>
		/// <exception cref="Exception">Configuration section cannot be found 'ApplicationInsights'</exception>
		public ApplicationInsightsProcess(IConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if (configuration.GetSection("ApplicationInsights").Exists())
				_connectionStringIsPresent = true;
		}


		/// <summary>
		/// Adds ApplicationInsights to Dependency Injection
		/// </summary>
		/// <param name="services">Services where ApplicationInsights needs to be added to</param>
		/// <returns>Collection of all the services</returns>
		public IServiceCollection FlashApplicationInsights(IServiceCollection services)
		{
			if (_connectionStringIsPresent)
				services.AddApplicationInsightsTelemetry();

			return services;
		}
	}
}