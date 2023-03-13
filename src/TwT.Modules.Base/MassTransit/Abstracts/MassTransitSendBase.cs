using MassTransit;
using Microsoft.Extensions.Logging;

namespace TwT.Modules.Base.MassTransit.Abstracts
{
	/// <summary>
	/// Base that can be used to send messages to the MassTransit broker
	/// </summary>
	public abstract class MassTransitSendBase
	{
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IClientFactory _clientFactory;

		/// <summary>
		/// Logger used for logging information about the process
		/// </summary>
		protected readonly ILogger Logger;

		/// <summary>
		/// Base that can be used to send messages to the MassTransit broker
		/// </summary>
		/// <param name="publishEndpoint">Publish endpoint to which the message is sent. For example, an exchange on RabbitMQ and a topic on Azure Service bus.</param>
		/// <param name="busControl">Bus system that can be used to create a client factory which uses the default bus endpoint for any response messages</param>
		/// <param name="logger">Logger that can be used for logging information about the process</param>
		/// <exception cref="NullReferenceException">PublishEndpoint is null</exception>
		/// <exception cref="NullReferenceException">BusControl is null</exception>
		/// <exception cref="NullReferenceException">Logger is null</exception>
		protected MassTransitSendBase(IPublishEndpoint publishEndpoint, IBusControl busControl, ILogger logger)
		{
			_publishEndpoint = publishEndpoint ?? throw new NullReferenceException(nameof(publishEndpoint));

			if (busControl == null)
				throw new NullReferenceException(nameof(busControl));
			_clientFactory = busControl.CreateClientFactory();

			Logger = logger ?? throw new NullReferenceException(nameof(logger));
		}

		/// <summary>
		/// Publishes a message to all subscribed consumers for the message type.
		/// </summary>
		/// <typeparam name="TMessage">Type of the message</typeparam>
		/// <param name="message">Message that needs to be published</param>
		/// <param name="cancellationToken">Token that can be used to cancel the publish process</param>
		/// <returns>True when the message could be published</returns>
		/// <remarks>Swallows the exception!</remarks>
		protected async Task<bool> Publish<TMessage>(TMessage message, CancellationToken cancellationToken) where TMessage : class
		{
			Logger.LogDebug("Publishing message. Message: '{@message}'", message);
			try
			{
				await _publishEndpoint.Publish(message, cancellationToken);

				return true;
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, "Could not publish message. Message: '{@message}'", message);
				return false;
			}
		}

		/// <summary>
		/// Send a messages and retrieves the response for it
		/// </summary>
		/// <typeparam name="TRequestMessage">Type of the request message</typeparam>
		/// <typeparam name="TResponseMessage">Type of the response message</typeparam>
		/// <param name="message">Message that needs to be send</param>
		/// <param name="uri">Uri where the message needs to be send to</param>
		/// <param name="cancellationToken">Token that an be used to cancel the publish process</param>
		/// <returns>Tuple that defines if the action was a success and the response message</returns>
		protected async Task<(bool IsSuccess, TResponseMessage? Result)> Request<TRequestMessage, TResponseMessage>(TRequestMessage message, Uri uri, CancellationToken cancellationToken)
						where TRequestMessage : class
						where TResponseMessage : class
		{
			Logger.LogDebug("Send a request (and receiving a response message) to '{uri}'. Request message: '{@message}'. Type of response message '{responseType}'", uri.AbsoluteUri, message, typeof(TResponseMessage));

			try
			{
				return (true, (await _clientFactory.CreateRequest(uri, message, cancellationToken).GetResponse<TResponseMessage>()).Message);
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, "Could not send a request (and receiving a response message) to '{uri}'. Request message: '{@message}'. Type of response message '{responseType}'", uri.AbsoluteUri, message, typeof(TResponseMessage));
				return (false, null);
			}
		}
	}
}