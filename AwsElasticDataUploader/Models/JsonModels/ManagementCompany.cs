using Newtonsoft.Json;

namespace SDAElasticAutoComplete.Models.JsonModels
{
	public record ManagementCompany
	{
		[JsonProperty("mgmtID")]
		public int? MgmtId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("market")]
		public string Market { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }
	}

	public record RootManagementCompany
	{
		[JsonRequired]
		[JsonProperty("mgmt")]
		public ManagementCompany ManagementCompany { get; init; }
	}
}