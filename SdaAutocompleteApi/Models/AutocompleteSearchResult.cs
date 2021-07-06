using Nest;

namespace SdaAutocompleteApi.Models
{
	public record AutocompleteSearchResult
	{
		[PropertyName("name")]
		[Text]
		public string Name { get; init; }

		[PropertyName("formerName")]
		[Text]
		public string FormerName { get; init; }

		[PropertyName("market")]
		[Keyword]
		public string Market { get; init; }

		[PropertyName("state")]
		[Text]
		public string State { get; init; }

		[PropertyName("streetAddress")]
		[Text]
		public string StreetAddress { get; init; }

		[PropertyName("city")]
		[Text]
		public string City { get; init; }
	}
}