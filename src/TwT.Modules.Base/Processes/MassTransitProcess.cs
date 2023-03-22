using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwT.Modules.Base.Configuration;

namespace TwT.Modules.Base.Processes
{
	/// <summary>
	/// Process that can be used to configure MassTransit
	/// </summary>
	internal class MassTransitProcess
	{
		/// <summary>
		/// Configuration that can be used to configure MassTransit
		/// </summary>
		public MassTransitConfiguration Configuration { get; private set; }

		/// <summary>
		/// Process that can be used to configure MassTransit
		/// </summary>
		/// <param name="configuration">Configuration that can be used to configure MassTransit</param>
		/// <exception cref="ArgumentNullException">Configuration is null</exception>
		/// <exception cref="Exception">Configuration section cannot be found 'MassTransit'</exception>
		public MassTransitProcess(IConfiguration configuration)
		{
			Configuration = configuration.GetSection("MassTransit").Get<MassTransitConfiguration>() ?? throw new ArgumentNullException(nameof(configuration));

			if (Configuration == null)
				throw new Exception("Configuration section cannot be found 'MassTransit'");
		}

		/// <summary>
		/// Adds MassTransit to Dependency Injection
		/// </summary>
		/// <param name="services">Services where MassTransit needs to be added to</param>
		/// <param name="consumerFlasher">Callback action that will be called to add consumers</param>
		/// <returns>Collection of all the services</returns>
		public IServiceCollection FlashMassTransit(IServiceCollection services, Action<IBusRegistrationConfigurator> consumerFlasher)
		{
			services.AddMassTransit(x =>
			{
				x.UsingRabbitMq((context, cfg) =>
				{
					cfg.Host(Configuration.Host, Configuration.VirtualHost, h =>
					{
						h.PublisherConfirmation = true; //task returned from Send/Publish is not completed until the message has been confirmed by the broker.
						h.Username(Configuration.Username); //username for the connection to RabbitMQ
						h.Password(Configuration.Password); //password for the connection to RabbitMQ
					});
					cfg.ConfigureEndpoints(context);
				});
				consumerFlasher(x);
			});

			return services;
		}
	}
}