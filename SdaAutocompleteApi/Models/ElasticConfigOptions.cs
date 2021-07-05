using System;

namespace SdaAutocompleteApi.Models
{
	public record ElasticConfigOptions
	{
		public const string ElasticConfig = "ElasticConfig";

		public string UserName { get; init; }

		public string Password { get; init; }

		public string Endpoint { get; init; }

		public int MaximumRetries { get; init; }

		public string PropertiesIndex { get; init; }

		public string MgmtIndex { get; init; }

		public TimeSpan RequestTimeout { get; init; }
	}
}