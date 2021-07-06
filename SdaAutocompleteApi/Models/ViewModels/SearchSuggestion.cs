namespace SdaAutocompleteApi.Models.ViewModels
{
	public record SearchSuggestion
	{
		public string Name { get; init; }

		public string FormerName { get; init; }

		public string Market { get; init; }

		public string City { get; init; }
	}
}