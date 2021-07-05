using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nest;
using SdaAutocompleteApi.Exceptions;
using SdaAutocompleteApi.Models;
using SdaAutocompleteApi.Models.ViewModels;

namespace SdaAutocompleteApi.Repositories
{
	public class SearchRepository : ISearchRepository
	{
		private readonly IElasticClient _elasticClient;

		private readonly ElasticConfigOptions _elasticConfig;

		public SearchRepository(IElasticClient elasticClient, IOptions<ElasticConfigOptions> elasticConfig)
		{
			_elasticClient = elasticClient;
			_elasticConfig = elasticConfig?.Value ?? throw new ArgumentNullException(nameof(elasticConfig));
		}

		public async Task<IEnumerable<AutocompleteSuggestion>> SearchForTermAsync(string term, string? market, SearchConfig config, int limit = 25)
		{
			var results = await _elasticClient.SearchAsync<AutocompleteSuggestion>(x => x
				.Query(query => query
					.Bool(boolQuery => boolQuery
						.Must(must => must
							.MultiMatch(multiMatch => multiMatch 
								.Fields(fields => fields
									// boost defined on how likely the term represents a field
									.Field(field => field.PropertyName, boost: 1.5)
									.Field(field => field.StreetAddress, boost: 1.2)
									.Field(field => field.FormerPropertyName, boost: 0.9)
									.Field(field => field.City, 0.6)
									.Field(field => field.State, 0.2)
								)
								.Fuzziness(Fuzziness.Auto)
								.Query(term)
							), must => must
							.Match(match => match 
								.Field(field => field.Market)
								.Query(market)
							)
						)
					)
				)
				.Index(new [] { config.PropertiesIndexName, config.MgmtIndexName })
				.IndicesBoost(indexBoost => indexBoost
					.Add(config.PropertiesIndexName, config.PropertiesIndexBoost)
					.Add(config.MgmtIndexName, config.MgmtIndexBoost)
				)
				.Size(limit)
			);

			if (results.IsValid)
				return results.Documents;

			throw new SearchFailedException(results.DebugInformation);
		}
	}
}