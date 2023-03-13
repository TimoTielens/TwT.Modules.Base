using MassTransit;
using Microsoft.Extensions.Logging;

namespace TwT.Modules.Base.MassTransit.Abstracts
{
	/// <summary>
	/// Base that can be used for consumers
	/// </summary>
	/// <typeparam name="TMessage"></typeparam>
	public abstract class MassTransitReceiverBase<TMessage> : IConsumer<TMessage> where TMessage : class
	{
		/// <summary>
		/// Logger used for logging information about the process
		/// </summary>
		protected readonly ILogger Logger;

		/// <summary>
		/// Base that can be used to receive messages from the MassTransit broker
		/// </summary>
		/// <param name="logger">Logger that can be used for logging information about the process</param>
		/// <exception cref="NullReferenceException">Logger is null</exception>
		protected MassTransitReceiverBase(ILogger logger)
		{
			Logger = logger ?? throw new NullReferenceException(nameof(logger));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public abstract Task Consume(ConsumeContext<TMessage> context);
	}
}