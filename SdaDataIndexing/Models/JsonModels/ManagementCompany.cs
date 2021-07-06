using Newtonsoft.Json;

namespace SDAElasticAutoComplete.Models.JsonModels
{
	public record ManagementCompany
	{
		[JsonProperty("mgmtID")]
		public int? MgmtId { get; init; }

		[JsonProperty("name")]
		public string Name { get; init; }

		[JsonProperty("market")]
		public string Market { get; init; }

		[JsonProperty("state")]
		public string State { get; init; }
	}

	public record RootManagementCompany
	{
		[JsonRequired]
		[JsonProperty("mgmt")]
		public ManagementCompany ManagementCompany { get; init; }
	}
}