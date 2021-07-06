using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nest;
using SdaAutocompleteApi.Exceptions;
using SdaAutocompleteApi.Models;
using SdaCommon.Models;

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

		public async Task<IReadOnlyCollection<AutocompleteSearchResult>> SearchForTermAsync(string term, string? market, int limit = 25)
		{
			const double minIndexBoost = 1.2;
			const double maxIndexBoost = 1.5;

			// if market is not defined - assume that term is related to mgmt
			var mgmtIndexBoost = market is null ? maxIndexBoost : minIndexBoost;
			var propertiesIndexName = _elasticConfig.PropertiesIndex;
			var propertiesIndexBoost = market is not null ? maxIndexBoost : minIndexBoost;
			var mgmtIndexName = _elasticConfig.MgmtIndex;

			//var marketTerm = market ?? "";
			var results = await _elasticClient.SearchAsync<AutocompleteSearchResult>(x => x
				.Query(query => query
					.Bool(boolQuery => boolQuery
						.Must(must => must
							.MultiMatch(multiMatch => multiMatch 
								.Fields(fields => fields
									// boost defined on how likely the term represents a field
									.Field(field => field.Name, boost: 1.5)
									.Field(field => field.StreetAddress, boost: 1.1)
									.Field(field => field.FormerName, boost: 0.9)
									.Field(field => field.City, 0.6)
									.Field(field => field.State, 0.2)
								)
								.Fuzziness(Fuzziness.Auto)
								.Query(term)
							)
						)
						.Filter(filter => filter
							.Term(marketTerm => marketTerm
								.Field(field => field.Market)
								.Value(market)
								.Verbatim(false)
							)
						)
					)
				)
				.Index(new [] { propertiesIndexName, mgmtIndexName })
				.IndicesBoost(indexBoost => indexBoost
					.Add(propertiesIndexName, propertiesIndexBoost)
					.Add(mgmtIndexName, mgmtIndexBoost)
				)
				.Size(limit)
			);

			if (results.IsValid)
				return results.Documents;

			throw new SearchFailedException(results.DebugInformation);
		}
	}
}