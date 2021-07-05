namespace SdaAutocompleteApi.Models
{
	public record SearchConfig
	{
		public string PropertiesIndexName { get; init; }

		public string MgmtIndexName { get; init; }

		public double MgmtIndexBoost { get; init; }

		public double PropertiesIndexBoost { get; init; }
	}
}