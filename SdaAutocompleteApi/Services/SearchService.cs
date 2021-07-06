using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SdaAutocompleteApi.Models.ViewModels;
using SdaAutocompleteApi.Repositories;

namespace SdaAutocompleteApi.Services
{
	public class SearchService : ISearchService
	{
		private readonly ISearchRepository _searchRepository;

		public SearchService(ISearchRepository searchRepository)
		{
			_searchRepository = searchRepository;
		}

		public async Task<List<SearchSuggestion>> SearchForTermAsync(string term, string? market, int limit = 25)
		{
			var suggestions = await _searchRepository.SearchForTermAsync(term, market, limit);

			var result = suggestions.Select(x => new SearchSuggestion
			{
				Name = $"{(x.StreetAddress is null ? "Management: " : string.Empty)}{x.Name}",
				FormerName = x.FormerName,
				Market = x.Market,
				City = x.City
			}).ToList();

			return result;
		}
	}
}