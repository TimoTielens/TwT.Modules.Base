using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using TwT.Modules.Base.Processes;

namespace TwT.Modules.Base
{
	/// <summary>
	/// Class that can be used to 'flash' and boot our own logic
	/// </summary>
	public abstract class Flasher
	{
		/// <summary>
		/// Process that can be used to configure MassTransit
		/// </summary>
		private MassTransitProcess? _massTransit;

		/// <summary>
		/// Builder for web applications and services
		/// </summary>
		public WebApplicationBuilder ApplicationBuilder { get; private set; }

		/// <summary>
		/// Web application used to configure the HTTP pipelines and routes
		/// </summary>
		public WebApplication? Application { get; private set; }

		/// <summary>
		/// Holds all the possible configuration options from different sources
		/// </summary>
		public IConfiguration Configuration => ApplicationBuilder.Configuration;

		/// <summary>
		/// Class that can be used to 'flash' and boot our own logic
		/// </summary>
		/// <param name="arguments">Arguments that where provided during boot</param>
		protected Flasher(string[] arguments)
		{
			ApplicationBuilder = WebApplication.CreateBuilder(arguments);
			FlashBoot();
		}

		/// <summary>
		/// Hooks onto the <see cref="IApplicationBuilder"/> and extends it
		/// </summary>
		public void BootUp()
		{
			Application = ApplicationBuilder.Build();
		}

		/// <summary>
		/// Hooks onto the DependencyInjection (services) and injects the needed processes
		/// </summary>
		private void FlashBoot()
		{
			_massTransit = new MassTransitProcess(Configuration);
			_massTransit.FlashMassTransit(ApplicationBuilder.Services, FlashMassTransitConsumers);
			FlashServices(ApplicationBuilder.Services, Configuration);
		}

		/// <summary>
		/// Gets called so that custom services can be flashed as well
		/// </summary>
		protected abstract void FlashServices(IServiceCollection services, IConfiguration configuration);

		/// <summary>
		/// Gets called so that custom MassTransit consumer can be flashed as well
		/// </summary>
		/// <param name="busFactoryConfigurator">Bus configurator that can be sued to add the consumers to</param>
		protected abstract void FlashMassTransitConsumers(IBusRegistrationConfigurator busFactoryConfigurator);
	}
}