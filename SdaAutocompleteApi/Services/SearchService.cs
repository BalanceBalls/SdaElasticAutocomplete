using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SdaAutocompleteApi.Models;
using SdaAutocompleteApi.Models.ViewModels;
using SdaAutocompleteApi.Repositories;

namespace SdaAutocompleteApi.Services
{
	public class SearchService : ISearchService
	{
		private readonly ISearchRepository _searchRepository;

		private readonly ElasticConfigOptions _elasticConfig;

		public SearchService(ISearchRepository searchRepository, IOptions<ElasticConfigOptions> elasticConfig)
		{
			_searchRepository = searchRepository;
			_elasticConfig = elasticConfig?.Value ?? throw new ArgumentNullException(nameof(elasticConfig));
		}

		public async Task<IEnumerable<AutocompleteSuggestion>> SearchForTermAsync(string term, string? market, int limit = 25)
		{
			const double minIndexBoost = 0.7;
			const double maxIndexBoost = 1.5;

			var searchConfig = new SearchConfig
			{
				PropertiesIndexName = _elasticConfig.PropertiesIndex,
				PropertiesIndexBoost = market is not null ? maxIndexBoost : minIndexBoost,
				MgmtIndexName = _elasticConfig.MgmtIndex,
				// if market is not defined - assume that term is related to mgmt
				MgmtIndexBoost = market is null ? maxIndexBoost : minIndexBoost
			};

			return await _searchRepository.SearchForTermAsync(term, market, searchConfig, limit);
		}
	}
}