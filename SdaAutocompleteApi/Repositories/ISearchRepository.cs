using System.Collections.Generic;
using System.Threading.Tasks;
using SdaAutocompleteApi.Models;
using SdaAutocompleteApi.Models.ViewModels;

namespace SdaAutocompleteApi.Repositories
{
	public interface ISearchRepository
	{
		Task<IEnumerable<AutocompleteSuggestion>> SearchForTermAsync(string term, string? market, SearchConfig config, int limit = 25);
	}
}