using Newtonsoft.Json;

namespace SDAElasticAutoComplete.Models.JsonModels
{
	public record Property
	{
		[JsonProperty("propertyID")]
		public int PropertyId { get; init; }

		[JsonProperty("name")]
		public string Name { get; init; }

		[JsonProperty("formerName")]
		public string FormerName { get; init; }

		[JsonProperty("streetAddress")]
		public string StreetAddress { get; init; }

		[JsonProperty("city")]
		public string City { get; init; }

		[JsonProperty("market")]
		public string Market { get; init; }

		[JsonProperty("state")]
		public string State { get; init; }
	}

	public record RootProperty
	{
		[JsonRequired]
		[JsonProperty("property")]
		public Property Property { get; init; }
	}
}