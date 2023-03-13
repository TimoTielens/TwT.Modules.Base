namespace TwT.Modules.Base.Configuration
{
	/// <summary>
	/// Interface that needs to be used for all configurations
	/// </summary>
	public interface ITwTConfiguration
	{
		/// <summary>
		/// Verifies if the configuration is valid
		/// </summary>
		/// <param name="errors">Validations errors if any</param>
		/// <returns>True when there are no validation errors</returns>
		bool IsValid(out IEnumerable<string> errors);

		/// <summary>
		/// Verifies if the configuration is valid
		/// </summary>
		/// <returns>True when there are no validation errors</returns>
		bool IsValid();
	}
}