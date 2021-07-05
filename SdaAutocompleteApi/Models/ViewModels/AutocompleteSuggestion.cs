using Nest;

namespace SdaAutocompleteApi.Models.ViewModels
{
	public record AutocompleteSuggestion
	{
		[PropertyName("name")]
		[Text]
		public string PropertyName { get; set; }

		[PropertyName("formerName")]
		[Text]
		public string FormerPropertyName { get; set; }

		[PropertyName("market")]
		[Text]
		public string Market { get; set; }

		[PropertyName("state")]
		[Text]
		public string State { get; set; }

		[PropertyName("streetAddress")]
		[Text]
		public string StreetAddress { get; set; }

		[PropertyName("city")]
		[Text]
		public string City { get; set; }
	}
}