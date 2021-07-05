using System.Collections.Generic;
using System.Threading.Tasks;
using SdaAutocompleteApi.Models.ViewModels;

namespace SdaAutocompleteApi.Services
{
	public interface ISearchService
	{
		Task<IEnumerable<AutocompleteSuggestion>> SearchForTermAsync(string term, string? market, int limit = 25);
	}
}