using System.Collections.Generic;
using System.Threading.Tasks;
using SdaAutocompleteApi.Models;

namespace SdaAutocompleteApi.Repositories
{
	public interface ISearchRepository
	{
		Task<IReadOnlyCollection<AutocompleteSearchResult>> SearchForTermAsync(string term, string? market, int limit = 25);
	}
}