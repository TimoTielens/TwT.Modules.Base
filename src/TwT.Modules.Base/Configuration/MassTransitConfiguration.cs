namespace TwT.Modules.Base.Configuration
{
	/// <summary>
	/// Configuration that can be used to configure MassTransit
	/// </summary>
	internal class MassTransitConfiguration : ITwTConfiguration
	{
		/// <summary>
		/// Host of RabbitMq
		/// </summary>
		public string Host { get; set; } = null!;

		/// <summary>
		/// Virtual host on RabbitMq
		/// </summary>
		/// <remarks>Default is '/'</remarks>
		public string VirtualHost { get; set; } = "/";

		/// <summary>
		/// Username that will be used to authenticate against the RabbitMq server
		/// </summary>
		public string Username { get; set; } = null!;

		/// <summary>
		/// Password that will be used to authenticate against the RabbitMq server
		/// </summary>
		public string Password { get; set; } = null!;

		/// <summary>
		/// Verifies if the configuration is valid
		/// </summary>
		/// <param name="errors">Validations errors if any</param>
		/// <returns>True when there are no validation errors</returns>
		public bool IsValid(out IEnumerable<string> errors)
		{
			var errorsList = new List<string>();

			if (string.IsNullOrWhiteSpace(Host))
				errorsList.Add($"{nameof(Host)} needs to be defined");
			if (string.IsNullOrWhiteSpace(VirtualHost))
				errorsList.Add($"{nameof(VirtualHost)} needs to be defined");
			if (string.IsNullOrWhiteSpace(Username))
				errorsList.Add($"{nameof(Username)} needs to be defined");
			if (string.IsNullOrWhiteSpace(Password))
				errorsList.Add($"{nameof(Password)} needs to be defined");

			errors = errorsList;
			return !errors.Any();
		}

		/// <summary>
		/// Verifies if the configuration is valid
		/// </summary>
		/// <returns>True when there are no validation errors</returns>
		public bool IsValid()
		{
			return IsValid(out var _);
		}
	}
}